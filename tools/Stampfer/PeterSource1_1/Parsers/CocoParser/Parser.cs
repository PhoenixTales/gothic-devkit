using System;
using System.Collections;
using System.Text;

namespace Peter.CocoParser
{
    public class Parser
    {
        const int maxT = 39;

        const bool T = true;
        const bool x = false;
        const int minErrDist = 2;

        static Token token;			// last recognized token
        static Token t;				// lookahead token
        static int errDist = minErrDist;

        const int id = 0;
        const int str = 1;

        static bool genScanner;

        /*-------------------------------------------------------------------------*/


        static void Error(int n)
        {
            if (errDist >= minErrDist) Errors.SynErr(n, t.line, t.col);
            errDist = 0;
        }

        public static void SemErr(int n)
        {
            if (errDist >= minErrDist) Errors.SemErr(n, token.line, token.col);
            errDist = 0;
        }

        static void Get()
        {
            for (; ; )
            {
                token = t;
                t = Scanner.Scan();
                if (t.kind <= maxT) { errDist++; return; }
                if (t.kind == 40)
                {
                    Tab.SetDDT(t.val);
                }

                t = token;
            }
        }

        static void Expect(int n)
        {
            if (t.kind == n) Get(); else Error(n);
        }

        static bool StartOf(int s)
        {
            return set[s, t.kind];
        }

        static void ExpectWeak(int n, int follow)
        {
            if (t.kind == n) Get();
            else
            {
                Error(n);
                while (!StartOf(follow)) Get();
            }
        }

        static bool WeakSeparator(int n, int syFol, int repFol)
        {
            bool[] s = new bool[maxT + 1];
            if (t.kind == n) { Get(); return true; }
            else if (StartOf(repFol)) return false;
            else
            {
                for (int i = 0; i <= maxT; i++)
                {
                    s[i] = set[syFol, i] || set[repFol, i] || set[0, i];
                }
                Error(n);
                while (!s[t.kind]) Get();
                return StartOf(syFol);
            }
        }

        static void Coco()
        {
            Symbol sym; Graph g;
            if (t.kind == 37)
            {
                UsingDecl(out ParserGen.usingPos);
            }
            Expect(6);
            int gramLine = token.line;
            genScanner = true;
            bool ok = true;
            Tab.ignored = null;
            Expect(1);
            string gramName = token.val;
            int beg = t.pos;
            while (StartOf(1))
            {
                Get();
            }
            Tab.semDeclPos = new Position(beg, t.pos - beg, 0);
            while (StartOf(2))
            {
                Declaration();
            }
            while (!(t.kind == 0 || t.kind == 7)) { Error(40); Get(); }
            Expect(7);
            if (genScanner) DFA.MakeDeterministic();
            Graph.DeleteNodes();
            while (t.kind == 1)
            {
                Get();
                sym = Symbol.Find(token.val);
                bool undef = sym == null;
                if (undef)
                {
                    sym = new Symbol(Node.nt, token.val, token.line);
                }
                else
                {
                    if (sym.typ == Node.nt)
                    {
                        if (sym.graph != null) SemErr(7);
                    }
                    else SemErr(8);
                    sym.line = token.line;
                }
                bool noAttrs = sym.attrPos == null; sym.attrPos = null;

                if (t.kind == 23)
                {
                    AttrDecl(sym);
                }
                if (!undef)
                    if (noAttrs != (sym.attrPos == null)) SemErr(5);

                if (t.kind == 35)
                {
                    SemText(out sym.semPos);
                }
                ExpectWeak(8, 3);
                Expression(out g);
                sym.graph = g.l;
                Graph.Finish(g);
                ExpectWeak(9, 4);
            }
            Expect(10);
            Expect(1);
            if (gramName != token.val) SemErr(17);
            Tab.gramSy = Symbol.Find(gramName);
            if (Tab.gramSy == null) SemErr(11);
            else
            {
                sym = Tab.gramSy;
                if (sym.attrPos != null) SemErr(12);
            }
            Tab.noSym = new Symbol(Node.t, "???", 0); // noSym gets highest number
            Tab.SetupAnys();
            Tab.RenumberPragmas();
            if (Tab.ddt[2]) Node.PrintNodes();
            if (Errors.count == 0)
            {
                Console.WriteLine("checking");
                Tab.CompSymbolSets();
                ok = ok && Tab.GrammarOk();

                if (Tab.ddt[7]) Tab.XRef();
                if (ok)
                {
                    Console.Write("parser");
                    ParserGen.WriteParser();
                    if (genScanner)
                    {
                        Console.Write(" + scanner");
                        DFA.WriteScanner();
                        if (Tab.ddt[0]) DFA.PrintStates();
                    }
                    Console.WriteLine(" generated");
                    if (Tab.ddt[8]) ParserGen.WriteStatistics();
                }
            }
            if (Tab.ddt[6]) Tab.PrintSymbolTable();
            Expect(9);
        }

        static void UsingDecl(out Position pos)
        {
            Expect(37);
            int beg = token.pos;
            while (StartOf(5))
            {
                Get();
            }
            Expect(38);
            int end = token.pos;
            while (t.kind == 37)
            {
                Get();
                while (StartOf(5))
                {
                    Get();
                }
                Expect(38);
                end = token.pos;
            }
            pos = new Position(beg, end - beg + 1, 0);
        }

        static void Declaration()
        {
            Graph g1, g2; bool nested = false;
            if (t.kind == 11)
            {
                Get();
                while (t.kind == 1)
                {
                    SetDecl();
                }
            }
            else if (t.kind == 12)
            {
                Get();
                while (t.kind == 1 || t.kind == 3 || t.kind == 5)
                {
                    TokenDecl(Node.t);
                }
            }
            else if (t.kind == 13)
            {
                Get();
                while (t.kind == 1 || t.kind == 3 || t.kind == 5)
                {
                    TokenDecl(Node.pr);
                }
            }
            else if (t.kind == 14)
            {
                Get();
                Expect(15);
                TokenExpr(out g1);
                Expect(16);
                TokenExpr(out g2);
                if (t.kind == 17)
                {
                    Get();
                    nested = true;
                }
                else if (StartOf(6))
                {
                    nested = false;
                }
                else Error(41);
                new Comment(g1.l, g2.l, nested);
            }
            else if (t.kind == 18)
            {
                Get();
                Set(out Tab.ignored);
                Tab.ignored[' '] = true; /* ' ' is always ignored */
                if (Tab.ignored[0]) SemErr(9);
            }
            else Error(42);
        }

        static void AttrDecl(Symbol sym)
        {
            Expect(23);
            int beg = t.pos; int col = t.col;
            while (StartOf(7))
            {
                if (StartOf(8))
                {
                    Get();
                }
                else
                {
                    Get();
                    SemErr(18);
                }
            }
            Expect(24);
            sym.attrPos = new Position(beg, token.pos - beg, col);
        }

        static void SemText(out Position pos)
        {
            Expect(35);
            int beg = t.pos; int col = t.col;
            while (StartOf(9))
            {
                if (StartOf(10))
                {
                    Get();
                }
                else if (t.kind == 4)
                {
                    Get();
                    SemErr(18);
                }
                else
                {
                    Get();
                    SemErr(19);
                }
            }
            Expect(36);
            pos = new Position(beg, token.pos - beg, col);
        }

        static void Expression(out Graph g)
        {
            Graph g2;
            Term(out g);
            bool first = true;
            while (WeakSeparator(25, 11, 12))
            {
                Term(out g2);
                if (first) { Graph.MakeFirstAlt(g); first = false; }
                Graph.MakeAlternative(g, g2);
            }
        }

        static void SetDecl()
        {
            BitArray s;
            Expect(1);
            string name = token.val;
            CharClass c = CharClass.Find(name);
            if (c != null) SemErr(7);
            Expect(8);
            Set(out s);
            if (Sets.Elements(s) == 0) SemErr(1);
            c = new CharClass(name, s);
            Expect(9);
        }

        static void TokenDecl(int typ)
        {
            string name; int kind; Symbol sym; Graph g;
            Sym(out name, out kind);
            sym = Symbol.Find(name);
            if (sym != null) SemErr(7);
            else
            {
                sym = new Symbol(typ, name, token.line);
                sym.tokenKind = Symbol.classToken;
            }
            while (!(StartOf(13))) { Error(43); Get(); }
            if (t.kind == 8)
            {
                Get();
                TokenExpr(out g);
                Expect(9);
                if (kind != id) SemErr(13);
                Graph.Finish(g);
                DFA.ConvertToStates(g.l, sym);
            }
            else if (StartOf(14))
            {
                if (kind == id) genScanner = false;
                else DFA.MatchLiteral(sym);
            }
            else Error(44);
            if (t.kind == 35)
            {
                SemText(out sym.semPos);
                if (typ == Node.t) SemErr(14);
            }
        }

        static void TokenExpr(out Graph g)
        {
            Graph g2;
            TokenTerm(out g);
            bool first = true;
            while (WeakSeparator(25, 15, 16))
            {
                TokenTerm(out g2);
                if (first) { Graph.MakeFirstAlt(g); first = false; }
                Graph.MakeAlternative(g, g2);
            }
        }

        static void Set(out BitArray s)
        {
            BitArray s2;
            SimSet(out s);
            while (t.kind == 19 || t.kind == 20)
            {
                if (t.kind == 19)
                {
                    Get();
                    SimSet(out s2);
                    s.Or(s2);
                }
                else
                {
                    Get();
                    SimSet(out s2);
                    Sets.Subtract(s, s2);
                }
            }
        }

        static void SimSet(out BitArray s)
        {
            int n1, n2;
            s = new BitArray(CharClass.charSetSize);
            if (t.kind == 1)
            {
                Get();
                CharClass c = CharClass.Find(token.val);
                if (c == null) SemErr(15); else s.Or(c.set);
            }
            else if (t.kind == 3)
            {
                Get();
                string name = token.val;
                name = DFA.Unescape(name.Substring(1, name.Length - 2));
                foreach (char ch in name) s[ch] = true;
            }
            else if (t.kind == 5)
            {
                Char(out n1);
                s[n1] = true;
                if (t.kind == 21)
                {
                    Get();
                    Char(out n2);
                    for (int i = n1; i <= n2; i++) s[i] = true;
                }
            }
            else if (t.kind == 22)
            {
                Get();
                s = new BitArray(CharClass.charSetSize, true);
                s[0] = false;
            }
            else Error(45);
        }

        static void Char(out int n)
        {
            Expect(5);
            string name = token.val;
            name = DFA.Unescape(name.Substring(1, name.Length - 2));
            int max = CharClass.charSetSize;
            if (name.Length != 1 || name[0] > max - 1) SemErr(2);
            n = name[0] % max;

        }

        static void Sym(out string name, out int kind)
        {
            name = "???"; kind = id;
            if (t.kind == 1)
            {
                Get();
                kind = id; name = token.val;
            }
            else if (t.kind == 3 || t.kind == 5)
            {
                if (t.kind == 3)
                {
                    Get();
                    name = token.val;
                }
                else
                {
                    Get();
                    name = "\"" + token.val.Substring(1, token.val.Length - 2) + "\"";
                }
                kind = str;
            }
            else Error(46);
        }

        static void Term(out Graph g)
        {
            Graph g2;
            g = new Graph();
            if (StartOf(17))
            {
                Factor(out g);
                while (StartOf(17))
                {
                    Factor(out g2);
                    Graph.MakeSequence(g, g2);
                }
            }
            else if (StartOf(18))
            {
                g = new Graph(new Node(Node.eps, null, 0));
            }
            else Error(47);
        }

        static void Factor(out Graph g)
        {
            string name;
            int kind;
            Position pos;
            bool weak = false;
            g = null;
            switch (t.kind)
            {
                case 1:
                case 3:
                case 5:
                case 26:
                    {
                        if (t.kind == 26)
                        {
                            Get();
                            weak = true;
                        }
                        Sym(out name, out kind);
                        Symbol sym = Symbol.Find(name);
                        bool undef = sym == null;
                        if (undef)
                        {
                            if (kind == id)
                                sym = new Symbol(Node.nt, name, 0); // forward nt
                            else if (genScanner)
                            {
                                sym = new Symbol(Node.t, name, token.line);
                                DFA.MatchLiteral(sym);
                            }
                            else
                            { // undefined string in production
                                SemErr(6); sym = Tab.eofSy; // dummy
                            }
                        }
                        int typ = sym.typ;
                        if (typ != Node.t && typ != Node.nt) SemErr(4);
                        if (weak)
                            if (typ == Node.t) typ = Node.wt; else SemErr(23);
                        Node p = new Node(typ, sym, token.line);
                        g = new Graph(p);

                        if (t.kind == 23)
                        {
                            Attribs(p);
                            if (kind != id) SemErr(3);
                        }
                        if (undef)
                            sym.attrPos = p.pos; // dummy
                        else if ((p.pos == null) != (sym.attrPos == null))
                            SemErr(5);

                        break;
                    }
                case 27:
                    {
                        Get();
                        Expression(out g);
                        Expect(28);
                        break;
                    }
                case 29:
                    {
                        Get();
                        Expression(out g);
                        Expect(30);
                        Graph.MakeOption(g);
                        break;
                    }
                case 31:
                    {
                        Get();
                        Expression(out g);
                        Expect(32);
                        Graph.MakeIteration(g);
                        break;
                    }
                case 35:
                    {
                        SemText(out pos);
                        Node p = new Node(Node.sem, null, 0);
                        p.pos = pos;
                        g = new Graph(p);
                        break;
                    }
                case 22:
                    {
                        Get();
                        Node p = new Node(Node.any, null, 0); // p.set is set in Tab.SetupAnys
                        g = new Graph(p);
                        break;
                    }
                case 33:
                    {
                        Get();
                        Node p = new Node(Node.sync, null, 0);
                        g = new Graph(p);
                        break;
                    }
                default: Error(48); break;
            }
        }

        static void Attribs(Node p)
        {
            Expect(23);
            int beg = t.pos; int col = t.col;
            while (StartOf(7))
            {
                if (StartOf(8))
                {
                    Get();
                }
                else
                {
                    Get();
                    SemErr(18);
                }
            }
            Expect(24);
            p.pos = new Position(beg, token.pos - beg, col);
        }

        static void TokenTerm(out Graph g)
        {
            Graph g2;
            TokenFactor(out g);
            while (StartOf(15))
            {
                TokenFactor(out g2);
                Graph.MakeSequence(g, g2);
            }
            if (t.kind == 34)
            {
                Get();
                Expect(27);
                TokenExpr(out g2);
                Graph.SetContextTrans(g2.l); Graph.MakeSequence(g, g2);
                Expect(28);
            }
        }

        static void TokenFactor(out Graph g)
        {
            string name; int kind;
            g = new Graph();
            if (t.kind == 1 || t.kind == 3 || t.kind == 5)
            {
                Sym(out name, out kind);
                if (kind == id)
                {
                    CharClass c = CharClass.Find(name);
                    if (c == null)
                    {
                        SemErr(15);
                        c = new CharClass(name, new BitArray(CharClass.charSetSize));
                    }
                    Node p = new Node(Node.clas, null, 0); p.val = c.n;
                    g = new Graph(p);
                }
                else g = Graph.StrToGraph(name); // str

            }
            else if (t.kind == 27)
            {
                Get();
                TokenExpr(out g);
                Expect(28);
            }
            else if (t.kind == 29)
            {
                Get();
                TokenExpr(out g);
                Expect(30);
                Graph.MakeOption(g);
            }
            else if (t.kind == 31)
            {
                Get();
                TokenExpr(out g);
                Expect(32);
                Graph.MakeIteration(g);
            }
            else Error(49);
        }



        public static void Parse()
        {
            Errors.SynErr = new ErrorProc(SynErr);
            t = new Token();
            Get();
            Coco();

        }

        static void SynErr(int n, int line, int col)
        {
            Errors.count++;
            Console.Write("-- line {0} col {1}: ", line, col);
            string s;
            switch (n)
            {
                case 0: s = "EOF expected"; break;
                case 1: s = "ident expected"; break;
                case 2: s = "number expected"; break;
                case 3: s = "string expected"; break;
                case 4: s = "badString expected"; break;
                case 5: s = "char expected"; break;
                case 6: s = "COMPILER expected"; break;
                case 7: s = "PRODUCTIONS expected"; break;
                case 8: s = "= expected"; break;
                case 9: s = ". expected"; break;
                case 10: s = "END expected"; break;
                case 11: s = "CHARACTERS expected"; break;
                case 12: s = "TOKENS expected"; break;
                case 13: s = "PRAGMAS expected"; break;
                case 14: s = "COMMENTS expected"; break;
                case 15: s = "FROM expected"; break;
                case 16: s = "TO expected"; break;
                case 17: s = "NESTED expected"; break;
                case 18: s = "IGNORE expected"; break;
                case 19: s = "+ expected"; break;
                case 20: s = "- expected"; break;
                case 21: s = ".. expected"; break;
                case 22: s = "ANY expected"; break;
                case 23: s = "< expected"; break;
                case 24: s = "> expected"; break;
                case 25: s = "| expected"; break;
                case 26: s = "WEAK expected"; break;
                case 27: s = "( expected"; break;
                case 28: s = ") expected"; break;
                case 29: s = "[ expected"; break;
                case 30: s = "] expected"; break;
                case 31: s = "{ expected"; break;
                case 32: s = "} expected"; break;
                case 33: s = "SYNC expected"; break;
                case 34: s = "CONTEXT expected"; break;
                case 35: s = "(. expected"; break;
                case 36: s = ".) expected"; break;
                case 37: s = "using expected"; break;
                case 38: s = "; expected"; break;
                case 39: s = "??? expected"; break;
                case 40: s = "this symbol not expected in Coco"; break;
                case 41: s = "invalid Declaration"; break;
                case 42: s = "invalid Declaration"; break;
                case 43: s = "this symbol not expected in TokenDecl"; break;
                case 44: s = "invalid TokenDecl"; break;
                case 45: s = "invalid SimSet"; break;
                case 46: s = "invalid Sym"; break;
                case 47: s = "invalid Term"; break;
                case 48: s = "invalid Factor"; break;
                case 49: s = "invalid TokenFactor"; break;

                default: s = "error " + n; break;
            }
            Console.WriteLine(s);
        }

        static bool[,] set = {
	{T,T,x,T, x,T,x,T, T,x,x,T, T,T,T,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,x,x, x},
	{x,T,T,T, T,T,T,x, T,T,T,x, x,x,x,T, T,T,x,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, x},
	{x,x,x,x, x,x,x,x, x,x,x,T, T,T,T,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x},
	{T,T,x,T, x,T,x,T, T,T,x,T, T,T,T,x, x,x,T,x, x,x,T,x, x,T,T,T, x,T,x,T, x,T,x,T, x,x,x,x, x},
	{T,T,x,T, x,T,x,T, T,x,T,T, T,T,T,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,x,x, x},
	{x,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,x,T, x},
	{x,x,x,x, x,x,x,T, x,x,x,T, T,T,T,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x},
	{x,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, x,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, x},
	{x,T,T,T, x,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, x,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, x},
	{x,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, x,T,T,T, x},
	{x,T,T,T, x,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,x, x,T,T,T, x},
	{x,T,x,T, x,T,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,T,T,T, T,T,T,T, T,T,x,T, x,x,x,x, x},
	{x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,T,x, T,x,x,x, x,x,x,x, x},
	{T,T,x,T, x,T,x,T, T,x,x,T, T,T,T,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,x,x, x},
	{x,T,x,T, x,T,x,T, x,x,x,T, T,T,T,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,x,x, x},
	{x,T,x,T, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,T,x,T, x,x,x,x, x,x,x,x, x},
	{x,x,x,x, x,x,x,T, x,T,x,T, T,T,T,x, T,T,T,x, x,x,x,x, x,x,x,x, T,x,T,x, T,x,x,x, x,x,x,x, x},
	{x,T,x,T, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,T,T, x,T,x,T, x,T,x,T, x,x,x,x, x},
	{x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, T,x,T,x, T,x,x,x, x,x,x,x, x}

	};
    } // end Parser

} // end namespace
