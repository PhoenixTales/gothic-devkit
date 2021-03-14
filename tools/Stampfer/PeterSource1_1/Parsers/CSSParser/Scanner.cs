using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Peter.CSSParser
{
    public class Token
    {
        public int kind;    // token kind
        public int pos;     // token position in the source text (starting at 0)
        public int col;     // token column (starting at 1)
        public int line;    // token line (starting at 1)
        public string val;  // token value
        public Token next;  // ML 2005-03-11 Tokens are kept in linked list
    }

    //-----------------------------------------------------------------------------------
    // Buffer
    //-----------------------------------------------------------------------------------
    public class Buffer
    {
        public const int EOF = char.MaxValue + 1;
        const int MAX_BUFFER_LENGTH = 64 * 1024; // 64KB
        byte[] buf;         // input buffer
        int bufStart;       // position of first byte in buffer relative to input stream
        int bufLen;         // length of buffer
        int fileLen;        // length of input stream
        int pos;            // current position in buffer
        Stream stream;      // input stream (seekable)
        bool isUserStream;  // was the stream opened by the user?

        public Buffer(Stream s, bool isUserStream)
        {
            stream = s; this.isUserStream = isUserStream;
            fileLen = bufLen = (int)s.Length;
            if (stream.CanSeek && bufLen > MAX_BUFFER_LENGTH) bufLen = MAX_BUFFER_LENGTH;
            buf = new byte[bufLen];
            bufStart = Int32.MaxValue; // nothing in the buffer so far
            Pos = 0; // setup buffer to position 0 (start)
            if (bufLen == fileLen) Close();
        }

        protected Buffer(Buffer b)
        { // called in UTF8Buffer constructor
            buf = b.buf;
            bufStart = b.bufStart;
            bufLen = b.bufLen;
            fileLen = b.fileLen;
            pos = b.pos;
            stream = b.stream;
            b.stream = null;
            isUserStream = b.isUserStream;
        }

        ~Buffer() { Close(); }

        protected void Close()
        {
            if (!isUserStream && stream != null)
            {
                stream.Close();
                stream = null;
            }
        }

        public virtual int Read()
        {
            if (pos < bufLen)
            {
                return buf[pos++];
            }
            else if (Pos < fileLen)
            {
                Pos = Pos; // shift buffer start to Pos
                return buf[pos++];
            }
            else
            {
                return EOF;
            }
        }

        public int Peek()
        {
            int curPos = Pos;
            int ch = Read();
            Pos = curPos;
            return ch;
        }

        public string GetString(int beg, int end)
        {
            int len = end - beg;
            char[] buf = new char[len];
            int oldPos = Pos;
            Pos = beg;
            for (int i = 0; i < len; i++) buf[i] = (char)Read();
            Pos = oldPos;
            return new String(buf);
        }

        public int Pos
        {
            get { return pos + bufStart; }
            set
            {
                if (value < 0) value = 0;
                else if (value > fileLen) value = fileLen;
                if (value >= bufStart && value < bufStart + bufLen)
                { // already in buffer
                    pos = value - bufStart;
                }
                else if (stream != null)
                { // must be swapped in
                    stream.Seek(value, SeekOrigin.Begin);
                    bufLen = stream.Read(buf, 0, buf.Length);
                    bufStart = value; pos = 0;
                }
                else
                {
                    pos = fileLen - bufStart; // make Pos return fileLen
                }
            }
        }
    }

    //-----------------------------------------------------------------------------------
    // UTF8Buffer
    //-----------------------------------------------------------------------------------
    public class UTF8Buffer : Buffer
    {
        public UTF8Buffer(Buffer b) : base(b) { }

        public override int Read()
        {
            int ch;
            do
            {
                ch = base.Read();
                // until we find a uft8 start (0xxxxxxx or 11xxxxxx)
            } while ((ch >= 128) && ((ch & 0xC0) != 0xC0) && (ch != EOF));
            if (ch < 128 || ch == EOF)
            {
                // nothing to do, first 127 chars are the same in ascii and utf8
                // 0xxxxxxx or end of file character
            }
            else if ((ch & 0xF0) == 0xF0)
            {
                // 11110xxx 10xxxxxx 10xxxxxx 10xxxxxx
                int c1 = ch & 0x07; ch = base.Read();
                int c2 = ch & 0x3F; ch = base.Read();
                int c3 = ch & 0x3F; ch = base.Read();
                int c4 = ch & 0x3F;
                ch = (((((c1 << 6) | c2) << 6) | c3) << 6) | c4;
            }
            else if ((ch & 0xE0) == 0xE0)
            {
                // 1110xxxx 10xxxxxx 10xxxxxx
                int c1 = ch & 0x0F; ch = base.Read();
                int c2 = ch & 0x3F; ch = base.Read();
                int c3 = ch & 0x3F;
                ch = (((c1 << 6) | c2) << 6) | c3;
            }
            else if ((ch & 0xC0) == 0xC0)
            {
                // 110xxxxx 10xxxxxx
                int c1 = ch & 0x1F; ch = base.Read();
                int c2 = ch & 0x3F;
                ch = (c1 << 6) | c2;
            }
            return ch;
        }
    }

    //-----------------------------------------------------------------------------------
    // Scanner
    //-----------------------------------------------------------------------------------
    public class Scanner
    {
        const char EOL = '\n';
        const int eofSym = 0; /* pdt */
        const int maxT = 33;
        const int noSym = 33;
        char valCh;       // current input character (for token.val)

        public Buffer buffer; // scanner buffer

        Token t;          // current token
        int ch;           // current input character
        int pos;          // byte position of current character
        int col;          // column number of current character
        int line;         // line number of current character
        int oldEols;      // EOLs that appeared in a comment;
        Dictionary<int, int> start; // maps first token character to start state

        Token tokens;     // list of tokens already peeked (first token is a dummy)
        Token pt;         // current peek token

        char[] tval = new char[128]; // text of current token
        int tlen;         // length of current token

        public Scanner(string fileName)
        {
            try
            {
                Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                buffer = new Buffer(stream, false);
                Init();
            }
            catch (IOException)
            {
                throw new FatalError("Cannot open file " + fileName);
            }
        }

        public Scanner(Stream s)
        {
            buffer = new Buffer(s, true);
            Init();
        }

        void Init()
        {
            pos = -1; line = 1; col = 0;
            oldEols = 0;
            NextCh();
            if (ch == 0xEF)
            { // check optional byte order mark for UTF-8
                NextCh(); int ch1 = ch;
                NextCh(); int ch2 = ch;
                if (ch1 != 0xBB || ch2 != 0xBF)
                {
                    throw new FatalError(String.Format("illegal byte order mark: EF {0,2:X} {1,2:X}", ch1, ch2));
                }
                buffer = new UTF8Buffer(buffer); col = 0;
                NextCh();
            }
            start = new Dictionary<int, int>(128);
            for (int i = 95; i <= 95; ++i) start[i] = 1;
            for (int i = 97; i <= 116; ++i) start[i] = 1;
            for (int i = 118; i <= 122; ++i) start[i] = 1;
            for (int i = 10; i <= 10; ++i) start[i] = 2;
            for (int i = 13; i <= 13; ++i) start[i] = 2;
            for (int i = 9; i <= 9; ++i) start[i] = 3;
            for (int i = 11; i <= 12; ++i) start[i] = 3;
            for (int i = 32; i <= 32; ++i) start[i] = 3;
            for (int i = 48; i <= 57; ++i) start[i] = 4;
            start[46] = 21;
            start[34] = 7;
            start[44] = 10;
            start[123] = 11;
            start[125] = 12;
            start[35] = 13;
            start[58] = 14;
            start[42] = 15;
            start[59] = 16;
            start[45] = 17;
            start[37] = 18;
            start[117] = 22;
            start[41] = 20;
            start[Buffer.EOF] = -1;

            pt = tokens = new Token();  // first token is a dummy
        }

        void NextCh()
        {
            if (oldEols > 0) { ch = EOL; oldEols--; }
            else
            {
                pos = buffer.Pos;
                ch = buffer.Read(); col++;
                // replace isolated '\r' by '\n' in order to make
                // eol handling uniform across Windows, Unix and Mac
                if (ch == '\r' && buffer.Peek() != '\n') ch = EOL;
                if (ch == EOL) { line++; col = 0; }
            }
            valCh = (char)ch;
            if (ch != Buffer.EOF) ch = char.ToLower((char)ch);
        }

        void AddCh()
        {
            if (tlen >= tval.Length)
            {
                char[] newBuf = new char[2 * tval.Length];
                Array.Copy(tval, 0, newBuf, 0, tval.Length);
                tval = newBuf;
            }
            tval[tlen++] = valCh;
            NextCh();
        }



        bool Comment0()
        {
            int level = 1, pos0 = pos, line0 = line, col0 = col;
            NextCh();
            if (ch == '*')
            {
                NextCh();
                for (; ; )
                {
                    if (ch == '*')
                    {
                        NextCh();
                        if (ch == '/')
                        {
                            level--;
                            if (level == 0) { oldEols = line - line0; NextCh(); return true; }
                            NextCh();
                        }
                    }
                    else if (ch == Buffer.EOF) return false;
                    else NextCh();
                }
            }
            else
            {
                buffer.Pos = pos0; NextCh(); line = line0; col = col0;
            }
            return false;
        }


        void CheckLiteral()
        {
            switch (t.val.ToLower())
            {
                case "ex": t.kind = 18; break;
                case "em": t.kind = 19; break;
                case "px": t.kind = 20; break;
                case "cm": t.kind = 21; break;
                case "mm": t.kind = 22; break;
                case "pc": t.kind = 23; break;
                case "in": t.kind = 24; break;
                case "pt": t.kind = 25; break;
                case "deg": t.kind = 26; break;
                case "rad": t.kind = 27; break;
                case "grad": t.kind = 28; break;
                case "m": t.kind = 29; break;
                case "s": t.kind = 30; break;
                case "k": t.kind = 31; break;
                case "hz": t.kind = 32; break;
                default: break;
            }
        }

        Token NextToken()
        {
            while (ch == 10 || ch == 13) NextCh();
            if (ch == '/' && Comment0()) return NextToken();
            t = new Token();
            t.pos = pos; t.col = col; t.line = line;
            int state;
            try { state = start[ch]; }
            catch (KeyNotFoundException) { state = 0; }
            tlen = 0; AddCh();

            switch (state)
            {
                case -1: { t.kind = eofSym; break; } // NextCh already done
                case 0: { t.kind = noSym; break; }   // NextCh already done
                case 1:
                    if (ch == '-' || ch >= '0' && ch <= '9' || ch == '_' || ch >= 'a' && ch <= 'z') { AddCh(); goto case 1; } else { t.kind = 1; t.val = new String(tval, 0, tlen); CheckLiteral(); return t; }
                case 2: { t.kind = 2; break; }
                case 3: { t.kind = 3; break; }
                case 4:
                    if (ch >= '0' && ch <= '9') { AddCh(); goto case 4; } else if (ch == '.') { AddCh(); goto case 5; } else { t.kind = 4; break; }
                case 5:
                    if (ch >= '0' && ch <= '9') { AddCh(); goto case 6; } else { t.kind = noSym; break; }
                case 6:
                    if (ch >= '0' && ch <= '9') { AddCh(); goto case 6; } else { t.kind = 4; break; }
                case 7:
                    if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '!' || ch >= '#' && ch <= '[' || ch >= ']' && ch <= 65535) { AddCh(); goto case 8; } else { t.kind = noSym; break; }
                case 8:
                    if (ch == '"') { AddCh(); goto case 9; } else { t.kind = noSym; break; }
                case 9: { t.kind = 5; break; }
                case 10: { t.kind = 6; break; }
                case 11: { t.kind = 7; break; }
                case 12: { t.kind = 8; break; }
                case 13: { t.kind = 10; break; }
                case 14: { t.kind = 11; break; }
                case 15: { t.kind = 12; break; }
                case 16: { t.kind = 13; break; }
                case 17: { t.kind = 14; break; }
                case 18: { t.kind = 15; break; }
                case 19: { t.kind = 16; break; }
                case 20: { t.kind = 17; break; }
                case 21:
                    if (ch >= '0' && ch <= '9') { AddCh(); goto case 4; } else { t.kind = 9; break; }
                case 22:
                    if (ch == '-' || ch >= '0' && ch <= '9' || ch == '_' || ch >= 'a' && ch <= 'q' || ch >= 's' && ch <= 'z') { AddCh(); goto case 1; } else if (ch == 'r') { AddCh(); goto case 23; } else { t.kind = 1; t.val = new String(tval, 0, tlen); CheckLiteral(); return t; }
                case 23:
                    if (ch == '-' || ch >= '0' && ch <= '9' || ch == '_' || ch >= 'a' && ch <= 'k' || ch >= 'm' && ch <= 'z') { AddCh(); goto case 1; } else if (ch == 'l') { AddCh(); goto case 24; } else { t.kind = 1; t.val = new String(tval, 0, tlen); CheckLiteral(); return t; }
                case 24:
                    if (ch == '-' || ch >= '0' && ch <= '9' || ch == '_' || ch >= 'a' && ch <= 'z') { AddCh(); goto case 1; } else if (ch == '(') { AddCh(); goto case 19; } else { t.kind = 1; t.val = new String(tval, 0, tlen); CheckLiteral(); return t; }

            }
            t.val = new String(tval, 0, tlen);
            return t;
        }

        // get the next token (possibly a token already seen during peeking)
        public Token Scan()
        {
            if (tokens.next == null)
            {
                return NextToken();
            }
            else
            {
                pt = tokens = tokens.next;
                return tokens;
            }
        }

        // peek for the next token, ignore pragmas
        public Token Peek()
        {
            if (pt.next == null)
            {
                do
                {
                    pt = pt.next = NextToken();
                } while (pt.kind > maxT); // skip pragmas
            }
            else
            {
                do
                {
                    pt = pt.next;
                } while (pt.kind > maxT);
            }
            return pt;
        }

        // make sure that peeking starts at the current scan position
        public void ResetPeek() { pt = tokens; }

    } // end Scanner

}
