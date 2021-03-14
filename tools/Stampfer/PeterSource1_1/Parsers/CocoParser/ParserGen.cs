using System;
using System.IO;
using System.Collections;
using System.Text;

namespace Peter.CocoParser
{
    public class ParserGen
    {

        const int maxTerm = 3;		// sets of size < maxTerm are enumerated
        const char CR = '\r';
        const char LF = '\n';
        const char TAB = '\t';
        const int EOF = -1;

        const int tErr = 0;			// error codes
        const int altErr = 1;
        const int syncErr = 2;

        public static Position usingPos; // "using" definitions from the attributed grammar

        static int errorNr;				// highest parser error number
        static Symbol curSy;			// symbol whose production is currently generated
        static FileStream fram;		// parser frame file
        static StreamWriter gen;	// generated parser source file
        static TextWriter trace;	// trace output stream
        static StringWriter err;	// generated parser error messages
        static ArrayList symSet = new ArrayList();

        static void Indent(int n)
        {
            for (int i = 1; i <= n; i++) gen.Write('\t');
        }

        static int Alternatives(Node p)
        {
            int i = 0;
            while (p != null) { i++; p = p.down; }
            return i;
        }

        static void CopyFramePart(string stop)
        {
            char startCh = stop[0];
            int endOfStopString = stop.Length - 1;
            int ch = fram.ReadByte();
            while (ch != EOF)
                if (ch == startCh)
                {
                    int i = 0;
                    do
                    {
                        if (i == endOfStopString) return; // stop[0..i] found
                        ch = fram.ReadByte(); i++;
                    } while (ch == stop[i]);
                    // stop[0..i-1] found; continue with last read character
                    gen.Write(stop.Substring(0, i));
                }
                else
                {
                    gen.Write((char)ch); ch = fram.ReadByte();
                }
            Errors.Exception(" -- incomplete or corrupt parser frame file");
        }

        static void CopySourcePart(Position pos, int indent)
        {
            // Copy text described by pos from atg to gen
            int ch, nChars, i;
            if (pos != null)
            {
                Buffer.Pos = pos.beg; ch = Buffer.Read(); nChars = pos.len - 1;
                Indent(indent);
                while (nChars >= 0)
                {
                    while (ch == CR || ch == LF)
                    {
                        gen.WriteLine(); Indent(indent);
                        int lastCh = ch; ch = Buffer.Read(); nChars--;
                        if (ch == LF && lastCh == CR) { ch = Buffer.Read(); nChars--; }
                        for (i = 1; i <= pos.col && ch <= ' '; i++)
                        { // skip blanks at beginning of line
                            ch = Buffer.Read(); nChars--;
                        }
                        if (i <= pos.col) pos.col = i - 1; // heading TABs => not enough blanks
                        if (nChars < 0) goto done;
                    }
                    gen.Write((char)ch);
                    ch = Buffer.Read(); nChars--;
                }
            done:
                if (indent > 0) gen.WriteLine();
            }
        }

        static string EscapedName(string s)
        {
            StringBuilder buf = new StringBuilder();
            for (int i = 1; i < s.Length - 1; i++)
            {
                switch (s[i])
                {
                    case '\\': buf.Append("\\\\"); break;
                    case '\'': buf.Append("\\\'"); break;
                    case '\"': buf.Append("\\\""); break;
                    default: buf.Append(s[i]); break;
                }
            }
            return buf.ToString();
        }

        static void GenErrorMsg(int errTyp, Symbol sym)
        {
            errorNr++;
            err.Write("\t\t\tcase " + errorNr + ": s = ");
            switch (errTyp)
            {
                case tErr:
                    {
                        if (sym.name[0] == '"') err.Write('"' + EscapedName(sym.name) + " expected");
                        else err.Write('"' + sym.name + " expected"); break;
                    }
                case altErr: { err.Write("\"invalid " + sym.name); break; }
                case syncErr: { err.Write("\"this symbol not expected in " + sym.name); break; }
            }
            err.WriteLine(" ({0})\"; break;", errorNr);
        }

        static int NewCondSet(BitArray s)
        {
            for (int i = 1; i < symSet.Count; i++) // skip symSet[0] (reserved for union of SYNC sets)
                if (Sets.Equals(s, (BitArray)symSet[i])) return i;
            symSet.Add(s.Clone());
            return symSet.Count - 1;
        }

        static void GenCond(BitArray s)
        {
            int n = Sets.Elements(s);
            if (n == 0) gen.Write("false"); // should never happen
            else if (n <= maxTerm)
                foreach (Symbol sym in Symbol.terminals)
                {
                    if (s[sym.n])
                    {
                        gen.Write("t.kind == {0}", sym.n);
                        n--; if (n > 0) gen.Write(" || ");
                    }
                }
            else gen.Write("StartOf({0})", NewCondSet(s));
        }

        static void PutCaseLabels(BitArray s)
        {
            foreach (Symbol sym in Symbol.terminals)
                if (s[sym.n]) gen.Write("case {0}: ", sym.n);
        }

        static void GenCode(Node p, int indent, BitArray isChecked)
        {
            Node p2;
            BitArray s1, s2;
            while (p != null)
            {
                switch (p.typ)
                {
                    case Node.nt:
                        {
                            Indent(indent);
                            gen.Write(p.sym.name + "(");
                            CopySourcePart(p.pos, 0);
                            gen.WriteLine(");");
                            break;
                        }
                    case Node.t:
                        {
                            Indent(indent);
                            if (isChecked[p.sym.n]) gen.WriteLine("Get();");
                            else gen.WriteLine("Expect({0});", p.sym.n);
                            break;
                        }
                    case Node.wt:
                        {
                            Indent(indent);
                            s1 = Tab.Expected(p.next, curSy);
                            s1.Or(Tab.allSyncSets);
                            gen.WriteLine("ExpectWeak({0}, {1});", p.sym.n, NewCondSet(s1));
                            break;
                        }
                    case Node.any:
                        {
                            Indent(indent);
                            gen.WriteLine("Get();");
                            break;
                        }
                    case Node.eps: break; // nothing
                    case Node.sem:
                        {
                            CopySourcePart(p.pos, indent);
                            break;
                        }
                    case Node.sync:
                        {
                            Indent(indent);
                            GenErrorMsg(syncErr, curSy);
                            s1 = (BitArray)p.set.Clone();
                            gen.Write("while (!("); GenCond(s1); gen.Write(")) {");
                            gen.Write("Error({0}); Get();", errorNr); gen.WriteLine("}");
                            break;
                        }
                    case Node.alt:
                        {
                            s1 = Tab.First(p);
                            bool equal = Sets.Equals(s1, isChecked);
                            int alts = Alternatives(p);
                            if (alts > 5) { Indent(indent); gen.WriteLine("switch (t.kind) {"); }
                            p2 = p;
                            while (p2 != null)
                            {
                                s1 = Tab.Expected(p2.sub, curSy);
                                Indent(indent);
                                if (alts > 5) { PutCaseLabels(s1); gen.WriteLine("{"); }
                                else if (p2 == p) { gen.Write("if ("); GenCond(s1); gen.WriteLine(") {"); }
                                else if (p2.down == null && equal) gen.WriteLine("} else {");
                                else { gen.Write("} else if ("); GenCond(s1); gen.WriteLine(") {"); }
                                s1.Or(isChecked);
                                GenCode(p2.sub, indent + 1, s1);
                                if (alts > 5)
                                {
                                    Indent(indent); gen.WriteLine("\tbreak;");
                                    Indent(indent); gen.WriteLine("}");
                                }
                                p2 = p2.down;
                            }
                            Indent(indent);
                            if (equal)
                            {
                                gen.WriteLine("}");
                            }
                            else
                            {
                                GenErrorMsg(altErr, curSy);
                                if (alts > 5)
                                {
                                    gen.WriteLine("default: Error({0}); break;", errorNr);
                                    Indent(indent); gen.WriteLine("}");
                                }
                                else
                                {
                                    gen.Write("} "); gen.WriteLine("else { Error(" + /*{0}*/errorNr + "); } "/*, errorNr*/);
                                }
                            }
                            break;
                        }
                    case Node.iter:
                        {
                            Indent(indent);
                            p2 = p.sub;
                            gen.Write("while (");
                            if (p2.typ == Node.wt)
                            {
                                s1 = Tab.Expected(p2.next, curSy);
                                s2 = Tab.Expected(p.next, curSy);
                                gen.Write("WeakSeparator({0},{1},{2}) ", p2.sym.n, NewCondSet(s1), NewCondSet(s2));
                                s1 = new BitArray(Symbol.terminals.Count); // for inner structure
                                if (p2.up || p2.next == null) p2 = null; else p2 = p2.next;
                            }
                            else
                            {
                                s1 = Tab.First(p2); GenCond(s1);
                            }
                            gen.WriteLine(") {");
                            GenCode(p2, indent + 1, s1);
                            Indent(indent); gen.WriteLine("}");
                            break;
                        }
                    case Node.opt:
                        s1 = Tab.First(p.sub);
                        if (!Sets.Equals(isChecked, s1))
                        {
                            Indent(indent);
                            gen.Write("if ("); GenCond(s1); gen.WriteLine(") {");
                            GenCode(p.sub, indent + 1, s1);
                            Indent(indent); gen.WriteLine("}");
                        }
                        else GenCode(p.sub, indent, isChecked);
                        break;
                }
                if (p.typ != Node.eps && p.typ != Node.sem && p.typ != Node.sync)
                    isChecked.SetAll(false);// = new BitArray(Symbol.terminals.Count);
                if (p.up) break;
                p = p.next;
            }
        }

        // 07_09_2002 ML 
        static void GenTokens()
        {
            string name;
            foreach (Symbol sym in Symbol.terminals)
            {
                name = sym.name;

                if (name.Equals("\"") || name.Equals("\"\"\"") || name.Equals("\\\"")) name = "DoubleQuote";
                name = name.Replace("\"", null);
                if (name.Equals(".")) name = "Dot";
                else if (name.Equals("'")) name = "SingleQuote";
                else if (name.Equals("(")) name = "BraceOpen";
                else if (name.Equals(")")) name = "BraceClose";
                else if (name.Equals("{")) name = "CurlBraceOpen";
                else if (name.Equals("}")) name = "CurlBraceClose";
                else if (name.Equals("[")) name = "SquareBraceOpen";
                else if (name.Equals("]")) name = "SquareBraceClose";
                else if (name.Equals(";")) name = "Semicolon";
                else if (name.Equals(":")) name = "Colon";
                else if (name.Equals(",")) name = "Comma";
                else if (name.Equals("*eof*")) name = "Eof";
                else if (name.Equals("->")) name = "PointerAccess";

                name = name.Replace("\\", "Backslash");
                name = name.Replace("==", "Equals");
                name = name.Replace("=", "Assign");
                name = name.Replace("!", "Not");
                name = name.Replace("<", "Smaller");
                name = name.Replace(">", "Bigger");
                name = name.Replace("~", "Tilde");
                name = name.Replace("+", "Plus");
                name = name.Replace("-", "Minus");
                name = name.Replace("&", "And");
                name = name.Replace("|", "Or");
                name = name.Replace("*", "Times");
                name = name.Replace("/", "Div");
                name = name.Replace("%", "Mod");
                name = name.Replace("^", "Pow");
                name = name.Replace("?", "QuestionMark");

                gen.WriteLine("\tpublic const int _{0}={1};", name, sym.n);
            }
        }

        static void GenCodePragmas()
        {
            gen.WriteLine("\t\t\tif (!peekMode) {");																// ML 16_10_2002
            foreach (Symbol sym in Symbol.pragmas)
            {
                gen.Write("\t\t\t\tif (t.kind == {0}) ", sym.n); gen.WriteLine("{");
                CopySourcePart(sym.semPos, 4);
                gen.WriteLine("\t\t\t\t}");
            }
            gen.WriteLine("\t\t\t}");																								// ML 16_10_2002
        }

        static void GenProductions()
        {
            foreach (Symbol sym in Symbol.nonterminals)
            {
                curSy = sym;
                gen.Write("\tstatic void {0}(", sym.name);
                CopySourcePart(sym.attrPos, 0);
                gen.WriteLine(") {");
                CopySourcePart(sym.semPos, 2);
                GenCode(sym.graph, 2, new BitArray(Symbol.terminals.Count));
                gen.WriteLine("\t}"); gen.WriteLine();
            }
        }

        static void InitSets()
        {
            for (int i = 0; i < symSet.Count; i++)
            {
                BitArray s = (BitArray)symSet[i];
                gen.Write("\t{"); int j = 0;
                foreach (Symbol sym in Symbol.terminals)
                {
                    if (s[sym.n]) gen.Write("T,"); else gen.Write("x,");
                    j++; if (j % 4 == 0) gen.Write(" ");
                }
                if (i == symSet.Count - 1) gen.WriteLine("x}"); else gen.WriteLine("x},");
            }
        }

        public static string GetString(int beg, int end)
        {
            StringBuilder s = new StringBuilder(64);
            int oldPos = Buffer.Pos;
            Buffer.Pos = beg;
            while (beg < end) { s.Append((char)Buffer.Read()); beg++; }
            Buffer.Pos = oldPos;
            return s.ToString();
        }

        public static void WriteParser()
        {
            FileStream s;
            symSet.Add(Tab.allSyncSets);
            string dir = System.Environment.CurrentDirectory;
            string fr = dir + "\\Parser.frame";
            if (!File.Exists(fr))
            {
                string frameDir = Environment.GetEnvironmentVariable("crframes");
                if (frameDir != null) fr = frameDir.Trim() + "\\Parser.frame";
                if (!File.Exists(fr)) Errors.Exception("-- Cannot find Parser.frame");
            }
            try
            {
                fram = new FileStream(fr, FileMode.Open);
            }
            catch (IOException)
            {
                Errors.Exception("-- Cannot open Parser.frame.");
            }
            try
            {
                string fn = dir + "\\Parser.cs";
                if (File.Exists(fn)) File.Copy(fn, fn + ".old", true);
                s = new FileStream(fn, FileMode.Create);
                gen = new StreamWriter(s);
            }
            catch (IOException)
            {
                Errors.Exception("-- Cannot generate parser file");
            }
            err = new StringWriter();
            foreach (Symbol sym in Symbol.terminals) GenErrorMsg(tErr, sym);
            if (usingPos != null) CopySourcePart(usingPos, 0);
            CopyFramePart("-->namespace"); gen.Write(Tab.gramSy.name);
            CopyFramePart("-->tokens"); GenTokens(); // 07_09_2002 ML write the tokenkinds
            CopyFramePart("-->constants");
            gen.WriteLine("\tconst int maxT = {0};", Symbol.terminals.Count - 1);
            CopyFramePart("-->declarations"); CopySourcePart(Tab.semDeclPos, 0);
            CopyFramePart("-->pragmas"); GenCodePragmas();
            CopyFramePart("-->productions"); GenProductions();
            CopyFramePart("-->parseRoot"); gen.WriteLine("\t\t{0}();", Tab.gramSy.name);
            CopyFramePart("-->errors"); gen.Write(err.ToString());
            CopyFramePart("-->initialization"); InitSets();
            CopyFramePart("$$$");
            gen.Close();
        }

        public static void WriteStatistics()
        {
            trace.WriteLine();
            trace.WriteLine("{0} terminals", Symbol.terminals.Count);
            trace.WriteLine("{0} symbols", Symbol.terminals.Count + Symbol.pragmas.Count
                + Symbol.nonterminals.Count);
            trace.WriteLine("{0} nodes", Node.nodes.Count);
            trace.WriteLine("{0} sets", symSet.Count);
        }

        public static void Init(TextWriter w)
        {
            trace = w;
            errorNr = -1;
            usingPos = null;
        }

    } // end ParserGen

} // end namespace
