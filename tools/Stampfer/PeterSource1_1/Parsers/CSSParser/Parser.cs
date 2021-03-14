using System;
using System.Collections.Generic;
using System.Text;

namespace Peter.CSSParser
{
    public class Parser
    {
        const int _EOF = 0;
        const int _identifier = 1;
        const int _newline = 2;
        const int _whitespace = 3;
        const int _number = 4;
        const int _string = 5;
        const int maxT = 33;

        const bool T = true;
        const bool x = false;
        const int minErrDist = 2;

        public Scanner scanner;
        public Errors errors;

        public Token t;    // last recognized token
        public Token la;   // lookahead token
        int errDist = minErrDist;

        public CSS CSSDocument;

        /*------------------------------------------------------------------------*
         *----- SCANNER DESCRIPTION ----------------------------------------------*
         *------------------------------------------------------------------------*/


        public Parser(Scanner scanner)
        {
            this.scanner = scanner;
            errors = new Errors();
        }

        void SynErr(int n)
        {
            if (errDist >= minErrDist) errors.SynErr(la.line, la.col, n);
            errDist = 0;
        }

        public void SemErr(string msg)
        {
            if (errDist >= minErrDist) errors.SemErr(t.line, t.col, msg);
            errDist = 0;
        }

        void Get()
        {
            for (; ; )
            {
                t = la;
                la = scanner.Scan();
                if (la.kind <= maxT) { ++errDist; break; }

                la = t;
            }
        }

        void Expect(int n)
        {
            if (la.kind == n) Get(); else { SynErr(n); }
        }

        bool StartOf(int s)
        {
            return set[s, la.kind];
        }

        void ExpectWeak(int n, int follow)
        {
            if (la.kind == n) Get();
            else
            {
                SynErr(n);
                while (!StartOf(follow)) Get();
            }
        }


        bool WeakSeparator(int n, int syFol, int repFol)
        {
            int kind = la.kind;
            if (kind == n) { Get(); return true; }
            else if (StartOf(repFol)) { return false; }
            else
            {
                SynErr(n);
                while (!(set[syFol, kind] || set[repFol, kind] || set[0, kind]))
                {
                    Get();
                    kind = la.kind;
                }
                return StartOf(syFol);
            }
        }


        void CSSDoc()
        {
            CSSDocument = new CSS();
            Selector sel = null;
            Tag tag = null;

            while (la.kind == 3)
            {
                Get();
            }
            while (StartOf(1))
            {
                SubTags(out tag);
                sel = new Selector();
                CSSDocument.Selectors.Add(sel);
                if (tag != null) { sel.Tags.Add(tag); }

                while (la.kind == 3)
                {
                    Get();
                }
                while (la.kind == 6)
                {
                    Get();
                    tag = null;
                    while (la.kind == 3)
                    {
                        Get();
                    }
                    SubTags(out tag);
                    if (tag != null) { sel.Tags.Add(tag); }
                }
                Expect(7);
                while (la.kind == 1 || la.kind == 3)
                {
                    if (la.val.Equals(";")) { break; }
                    Property p = null;

                    while (la.kind == 3)
                    {
                        Get();
                    }
                    if (la.val.Equals("}")) { break; }
                    PropertyDecl(out p);
                    sel.Properties.Add(p); p = null;
                }
                while (la.kind == 3)
                {
                    Get();
                }
                Expect(8);
                while (la.kind == 3)
                {
                    Get();
                }
            }
        }

        void SubTags(out Tag tag)
        {
            tag = null;
            Tag sub = null;
            Tag tmp = null;

            SelectorName(out tag);
            tmp = tag;
            while (la.kind == 3)
            {
                Get();
                while (la.kind == 3)
                {
                    Get();
                }
                if (la.val.Equals("{")) { return; }
                SelectorName(out sub);
                tmp.SubTags.Add(sub);
                tmp = sub;

            }
            while (la.kind == 3)
            {
                Get();
            }
        }

        void PropertyDecl(out Property p)
        {
            p = new Property();
            Expect(1);
            p.Attribute = t.val;
            while (la.kind == 3)
            {
                Get();
            }
            Expect(11);
            while (StartOf(2))
            {
                PropertyValue pv = null;
                while (la.kind == 3)
                {
                    Get();
                }
                PropertyValue(out pv);
                if (pv != null) { p.Values.Add(pv); }
                while (la.kind == 3)
                {
                    Get();
                }
                if (la.kind == 6)
                {
                    Get();
                }
            }
            ExpectWeak(13, 3);
        }

        void SelectorName(out Tag tag)
        {
            tag = new Tag();
            bool cls = false;
            bool id = false;
            bool pseudo = false;

            if (la.kind == 9 || la.kind == 10 || la.kind == 11)
            {
                if (la.kind == 9)
                {
                    Get();
                    cls = true;
                }
                else if (la.kind == 10)
                {
                    Get();
                    id = true;
                }
                else
                {
                    Get();
                    pseudo = true;
                }
            }
            if (la.kind == 1)
            {
                Get();
            }
            else if (la.kind == 12)
            {
                Get();
            }
            else SynErr(34);
            if (cls)
            {
                tag.Class = t.val; tag.TagType |= TagType.Classed;
            }
            else if (id)
            {
                tag.Id = t.val; tag.TagType |= TagType.IDed;
            }
            else if (pseudo)
            {
                tag.Pseudo = t.val; tag.TagType |= TagType.Pseudoed;
            }
            else
            {
                tag.Name = t.val; tag.TagType |= TagType.Named;
            }

            while (la.kind == 9 || la.kind == 10 || la.kind == 11)
            {
                cls = false; id = false; pseudo = false;
                if (la.kind == 9)
                {
                    Get();
                    cls = true;
                }
                else if (la.kind == 10)
                {
                    Get();
                    id = true;
                }
                else
                {
                    Get();
                    pseudo = true;
                }
                if (la.kind == 1)
                {
                    Get();
                }
                else if (la.kind == 12)
                {
                    Get();
                }
                else SynErr(35);
                if (cls)
                {
                    tag.Class = t.val; tag.TagType |= TagType.Classed;
                }
                else if (id)
                {
                    tag.Id = t.val; tag.TagType |= TagType.IDed;
                }
                else if (pseudo)
                {
                    tag.Pseudo = t.val; tag.TagType |= TagType.Pseudoed;
                }

            }
        }

        void PropertyValue(out PropertyValue p)
        {
            p = new PropertyValue();
            if (la.kind == 4 || la.kind == 14)
            {
                if (la.kind == 14)
                {
                    Get();
                }
                p.Value = t.val;
                Expect(4);
                if (p.Value != null) { p.Value += t.val; } else { p.Value = t.val; }
                while (la.kind == 3)
                {
                    Get();
                }
                if (StartOf(4))
                {
                    if (StartOf(5))
                    {
                        Unit();
                        p.Type = ValType.Unit;
                        p.Unit = (Unit)Enum.Parse(typeof(Unit), t.val.ToUpper());

                    }
                    else
                    {
                        Get();
                        p.Type = ValType.Percent;
                    }
                }
            }
            else if (la.kind == 16)
            {
                Get();
                p.Value = ""; p.Type = ValType.Url;
                while (StartOf(6))
                {
                    Get();
                    p.Value += t.val;
                }
                Expect(17);
            }
            else if (la.kind == 1)
            {
                Get();
                p.Value = t.val;
                p.Type = ValType.String;

            }
            else if (la.kind == 10)
            {
                Get();
                if (la.kind == 4)
                {
                    Get();
                    p.Value = t.val;
                }
                else if (la.kind == 1)
                {
                    Get();
                    p.Value = t.val;
                }
                else SynErr(36);
                while (la.kind == 1 || la.kind == 4)
                {
                    if (la.kind == 4)
                    {
                        Get();
                        p.Value += t.val;
                    }
                    else
                    {
                        Get();
                        p.Value += t.val;
                    }
                }
                p.Type = ValType.Hex;
            }
            else SynErr(37);
        }

        void Unit()
        {
            switch (la.kind)
            {
                case 18:
                    {
                        Get();
                        break;
                    }
                case 19:
                    {
                        Get();
                        break;
                    }
                case 20:
                    {
                        Get();
                        break;
                    }
                case 21:
                    {
                        Get();
                        break;
                    }
                case 22:
                    {
                        Get();
                        break;
                    }
                case 23:
                    {
                        Get();
                        break;
                    }
                case 24:
                    {
                        Get();
                        break;
                    }
                case 25:
                    {
                        Get();
                        break;
                    }
                case 26:
                    {
                        Get();
                        break;
                    }
                case 27:
                    {
                        Get();
                        break;
                    }
                case 28:
                    {
                        Get();
                        break;
                    }
                case 29:
                case 30:
                    {
                        if (la.kind == 29)
                        {
                            Get();
                        }
                        Expect(30);
                        break;
                    }
                case 31:
                case 32:
                    {
                        if (la.kind == 31)
                        {
                            Get();
                        }
                        Expect(32);
                        break;
                    }
                default: SynErr(38); break;
            }
        }



        public void Parse()
        {
            la = new Token();
            la.val = "";
            Get();
            CSSDoc();

            Expect(0);
        }

        bool[,] set = {
		{T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,x,x, x,x,x,x, x,T,T,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,x,T, T,x,x,x, x,x,T,x, x,x,T,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{T,T,x,T, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,x,x},
		{x,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,x,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,x}

	};
    } // end Parser


    public class Errors
    {
        public int count = 0;                                    // number of errors detected
        public System.IO.TextWriter errorStream = Console.Out;   // error messages go to this stream
        public string errMsgFormat = "-- line {0} col {1}: {2}"; // 0=line, 1=column, 2=text

        public void SynErr(int line, int col, int n)
        {
            string s;
            switch (n)
            {
                case 0: s = "EOF expected"; break;
                case 1: s = "identifier expected"; break;
                case 2: s = "newline expected"; break;
                case 3: s = "whitespace expected"; break;
                case 4: s = "number expected"; break;
                case 5: s = "string expected"; break;
                case 6: s = "\",\" expected"; break;
                case 7: s = "\"{\" expected"; break;
                case 8: s = "\"}\" expected"; break;
                case 9: s = "\".\" expected"; break;
                case 10: s = "\"#\" expected"; break;
                case 11: s = "\":\" expected"; break;
                case 12: s = "\"*\" expected"; break;
                case 13: s = "\";\" expected"; break;
                case 14: s = "\"-\" expected"; break;
                case 15: s = "\"%\" expected"; break;
                case 16: s = "\"url(\" expected"; break;
                case 17: s = "\")\" expected"; break;
                case 18: s = "\"ex\" expected"; break;
                case 19: s = "\"em\" expected"; break;
                case 20: s = "\"px\" expected"; break;
                case 21: s = "\"cm\" expected"; break;
                case 22: s = "\"mm\" expected"; break;
                case 23: s = "\"pc\" expected"; break;
                case 24: s = "\"in\" expected"; break;
                case 25: s = "\"pt\" expected"; break;
                case 26: s = "\"deg\" expected"; break;
                case 27: s = "\"rad\" expected"; break;
                case 28: s = "\"grad\" expected"; break;
                case 29: s = "\"m\" expected"; break;
                case 30: s = "\"s\" expected"; break;
                case 31: s = "\"k\" expected"; break;
                case 32: s = "\"hz\" expected"; break;
                case 33: s = "??? expected"; break;
                case 34: s = "invalid SelectorName"; break;
                case 35: s = "invalid SelectorName"; break;
                case 36: s = "invalid PropertyValue"; break;
                case 37: s = "invalid PropertyValue"; break;
                case 38: s = "invalid Unit"; break;

                default: s = "error " + n; break;
            }
            errorStream.WriteLine(errMsgFormat, line, col, s);
            count++;
        }

        public void SemErr(int line, int col, string s)
        {
            errorStream.WriteLine(errMsgFormat, line, col, s);
            count++;
        }

        public void SemErr(string s)
        {
            errorStream.WriteLine(s);
            count++;
        }

        public void Warning(int line, int col, string s)
        {
            errorStream.WriteLine(errMsgFormat, line, col, s);
        }

        public void Warning(string s)
        {
            errorStream.WriteLine(s);
        }
    } // Errors


    public class FatalError : Exception
    {
        public FatalError(string m) : base(m) { }
    }

}