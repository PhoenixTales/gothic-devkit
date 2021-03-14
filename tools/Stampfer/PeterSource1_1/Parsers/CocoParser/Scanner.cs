using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System.Text;

namespace Peter.CocoParser
{
    public class Token
    {
        public int kind;		// token kind
        public int pos;		  // token position in the source text (starting at 0)
        public int col;		  // token column (starting at 0)
        public int line;		// token line (starting at 1)
        public string val;	// token value
    }

    public class Buffer
    {
        static byte[] buf;
        static int bufLen;
        static int pos;
        public const int eof = '\uffff';

        public static void Fill(string fileName)
        {
            try
            {
                FileStream s = new FileStream(fileName, FileMode.Open);
                bufLen = (int)s.Length;
                buf = new byte[bufLen];
                s.Read(buf, 0, bufLen); pos = 0;
            }
            catch (IOException)
            {
                Console.WriteLine("--- Cannot open file {0}", fileName);
                System.Environment.Exit(0);
            }
        }

        public static int Read()
        {
            if (pos < bufLen)
                return buf[pos++];
            else
                return eof;
        }

        public static int Pos
        {
            get { return pos; }
            set
            {
                if (value < 0) pos = 0; else if (value >= bufLen) pos = bufLen; else pos = value;
            }
        }
    }


    public class Scanner
    {
        const char EOF = '\0';
        const char CR = '\r';
        const char LF = '\n';
        const int noSym = 39;
        static short[] start = {
	 30,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
	  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
	  0,  0, 11,  0, 10,  0,  0,  5, 21, 22,  0, 15,  0, 16, 14,  0,
	  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  0, 29, 18, 13, 19,  0,
	  0,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,
	  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1, 23,  0, 24,  0,  0,
	  0,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,
	  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1, 25, 20, 26,  0,  0,
	  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
	  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
	  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
	  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
	  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
	  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
	  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
	  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,
	  0};


        static Token t;			// current token
        static char ch;			// current input character
        static char lastCh; // last input character
        static int pos;     // column number of current character
        static int line;		// line number of current character
        static int lineStart;	    // start position of current line
        static Queue oldEols;		  // EOLs that appeared in a comment;
        static BitArray ignore;	  // set of characters to be ignored by the scanner	

        public static void Init(String fileName)
        {
            Buffer.Fill(fileName);
            pos = -1; line = 1; lineStart = 0; lastCh = '\0';
            oldEols = new Queue();
            NextCh();
            ignore = new BitArray(256);
            ignore[9] = true; ignore[10] = true; ignore[13] = true; ignore[32] = true;

        }

        private static void NextCh()
        {
            if (oldEols.Count > 0)
            {
                ch = (char)oldEols.Dequeue();
            }
            else
            {
                lastCh = ch;
                ch = (char)Buffer.Read(); pos++;
                if (ch == '\uffff') ch = EOF;
                else if (ch == CR) { line++; lineStart = pos + 1; }
                else if (ch == LF)
                {
                    if (lastCh != CR) line++;
                    lineStart = pos + 1;
                }
                else if (ch > '\u00ff')
                {
                    Console.WriteLine("-- line {0} col {1}: illegal character", line, pos - lineStart + 1);
                    Errors.count++; ch = ' ';
                }
            }
        }


        static bool Comment0()
        {
            int level = 1, line0 = line, lineStart0 = lineStart;
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
                            if (level == 0)
                            {
                                while (line0 < line) { oldEols.Enqueue('\r'); oldEols.Enqueue('\n'); line0++; }
                                NextCh(); return true;
                            }
                            NextCh();
                        }
                    }
                    else if (ch == '/')
                    {
                        NextCh();
                        if (ch == '*')
                        {
                            level++; NextCh();
                        }
                    }
                    else if (ch == EOF) return false;
                    else NextCh();
                }
            }
            else
            {
                if (ch == CR) { line--; lineStart = lineStart0; }
                pos = pos - 2; Buffer.Pos = pos + 1; NextCh();
            }
            return false;
        }


        static void CheckLiteral()
        {
            switch (t.val)
            {
                case "COMPILER": t.kind = 6; break;
                case "PRODUCTIONS": t.kind = 7; break;
                case "END": t.kind = 10; break;
                case "CHARACTERS": t.kind = 11; break;
                case "TOKENS": t.kind = 12; break;
                case "PRAGMAS": t.kind = 13; break;
                case "COMMENTS": t.kind = 14; break;
                case "FROM": t.kind = 15; break;
                case "TO": t.kind = 16; break;
                case "NESTED": t.kind = 17; break;
                case "IGNORE": t.kind = 18; break;
                case "ANY": t.kind = 22; break;
                case "WEAK": t.kind = 26; break;
                case "SYNC": t.kind = 33; break;
                case "CONTEXT": t.kind = 34; break;
                case "using": t.kind = 37; break;
                default: break;

            }
        }

        public static Token Scan()
        {
            while (ignore[ch]) NextCh();
            if (ch == '/' && Comment0()) return Scan();
            t = new Token();
            t.pos = pos; t.col = pos - lineStart + 1; t.line = line;
            int state = start[ch];
            StringBuilder buf = new StringBuilder(16);
            buf.Append(ch); NextCh();

            switch (state)
            {
                case 0: { t.kind = noSym; goto done; } // NextCh already done
                case 1:
                    if ((ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'Z' || ch >= 'a' && ch <= 'z')) { buf.Append(ch); NextCh(); goto case 1; }
                    else { t.kind = 1; t.val = buf.ToString(); CheckLiteral(); return t; }
                case 2:
                    if ((ch >= '0' && ch <= '9')) { buf.Append(ch); NextCh(); goto case 2; }
                    else { t.kind = 2; goto done; }
                case 3:
                    { t.kind = 3; goto done; }
                case 4:
                    { t.kind = 4; goto done; }
                case 5:
                    if ((ch >= 1 && ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '&' || ch >= '(' && ch <= '[' || ch >= ']')) { buf.Append(ch); NextCh(); goto case 6; }
                    else if (ch == 92) { buf.Append(ch); NextCh(); goto case 7; }
                    else { t.kind = noSym; goto done; }
                case 6:
                    if (ch == 39) { buf.Append(ch); NextCh(); goto case 9; }
                    else { t.kind = noSym; goto done; }
                case 7:
                    if ((ch >= ' ' && ch <= '~')) { buf.Append(ch); NextCh(); goto case 8; }
                    else { t.kind = noSym; goto done; }
                case 8:
                    if ((ch >= '0' && ch <= '9' || ch >= 'a' && ch <= 'f')) { buf.Append(ch); NextCh(); goto case 8; }
                    else if (ch == 39) { buf.Append(ch); NextCh(); goto case 9; }
                    else { t.kind = noSym; goto done; }
                case 9:
                    { t.kind = 5; goto done; }
                case 10:
                    if ((ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'Z' || ch >= 'a' && ch <= 'z')) { buf.Append(ch); NextCh(); goto case 10; }
                    else { t.kind = 40; goto done; }
                case 11:
                    if ((ch >= 1 && ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '!' || ch >= '#' && ch <= '[' || ch >= ']')) { buf.Append(ch); NextCh(); goto case 11; }
                    else if ((ch == 10 || ch == 13)) { buf.Append(ch); NextCh(); goto case 4; }
                    else if (ch == '"') { buf.Append(ch); NextCh(); goto case 3; }
                    else if (ch == 92) { buf.Append(ch); NextCh(); goto case 12; }
                    else { t.kind = noSym; goto done; }
                case 12:
                    if ((ch >= ' ' && ch <= '~')) { buf.Append(ch); NextCh(); goto case 11; }
                    else { t.kind = noSym; goto done; }
                case 13:
                    { t.kind = 8; goto done; }
                case 14:
                    if (ch == '.') { buf.Append(ch); NextCh(); goto case 17; }
                    else if (ch == ')') { buf.Append(ch); NextCh(); goto case 28; }
                    else { t.kind = 9; goto done; }
                case 15:
                    { t.kind = 19; goto done; }
                case 16:
                    { t.kind = 20; goto done; }
                case 17:
                    { t.kind = 21; goto done; }
                case 18:
                    { t.kind = 23; goto done; }
                case 19:
                    { t.kind = 24; goto done; }
                case 20:
                    { t.kind = 25; goto done; }
                case 21:
                    if (ch == '.') { buf.Append(ch); NextCh(); goto case 27; }
                    else { t.kind = 27; goto done; }
                case 22:
                    { t.kind = 28; goto done; }
                case 23:
                    { t.kind = 29; goto done; }
                case 24:
                    { t.kind = 30; goto done; }
                case 25:
                    { t.kind = 31; goto done; }
                case 26:
                    { t.kind = 32; goto done; }
                case 27:
                    { t.kind = 35; goto done; }
                case 28:
                    { t.kind = 36; goto done; }
                case 29:
                    { t.kind = 38; goto done; }
                case 30: { t.kind = 0; goto done; }
            }
        done:
            t.val = buf.ToString();
            return t;
        }

    } // end Scanner


    public delegate void ErrorProc(int n, int line, int col);

    public class Errors
    {
        public static int count = 0;	// number of errors detected
        public static ErrorProc SynErr = new ErrorProc(Default);	// syntactic errors
        public static ErrorProc SemErr = new ErrorProc(Default);	// semantic errors

        public static void Exception(string s)
        {
            Console.WriteLine(s); System.Environment.Exit(0);
        }

        static void Default(int n, int line, int col)
        {
            Console.WriteLine("-- line {0} col {1}: error {2}", line, col, n);
            count++;
        }
    } // Errors

} // end namespace
