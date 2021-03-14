using System;
using System.Collections;
using Peter.CSParser;

namespace Peter.JavaParser
{
    public class Parser
    {
        const int _EOF = 0;
        const int _ident = 1;
        const int _intLit = 2;
        const int _floatLit = 3;
        const int _charLit = 4;
        const int _stringLit = 5;
        const int _bool = 6;
        const int _byte = 7;
        const int _char = 8;
        const int _class = 9;
        const int _double = 10;
        const int _false = 11;
        const int _final = 12;
        const int _float = 13;
        const int _int = 14;
        const int _long = 15;
        const int _new = 16;
        const int _null = 17;
        const int _short = 18;
        const int _static = 19;
        const int _super = 20;
        const int _this = 21;
        const int _true = 22;
        const int _void = 23;
        const int _colon = 24;
        const int _comma = 25;
        const int _dec = 26;
        const int _dot = 27;
        const int _inc = 28;
        const int _lbrace = 29;
        const int _lbrack = 30;
        const int _lpar = 31;
        const int _minus = 32;
        const int _not = 33;
        const int _plus = 34;
        const int _rbrace = 35;
        const int _rbrack = 36;
        const int _rpar = 37;
        const int _tilde = 38;
        const int maxT = 101;

        const bool T = true;
        const bool x = false;
        const int minErrDist = 2;

        public Scanner scanner;
        public Errors errors;

        public Token t;    // last recognized token
        public Token la;   // lookahead token
        int errDist = minErrDist;
        private string m_Current = "";
        private JavaCodeInfo m_CodeInfo = new JavaCodeInfo();

        /// <summary>
        /// Gets the Info about the Code...
        /// </summary>
        public JavaCodeInfo CodeInfo
        {
            get { return this.m_CodeInfo; }
        }

        public class Modifier
        {
            public static int _public = 0x0001;
            public static int _private = 0x0002;
            public static int _protected = 0x0004;
            public static int _static = 0x0008;
            public static int _final = 0x0010;
            public static int _synchronized = 0x0020;
            public static int _volatile = 0x0040;
            public static int _transient = 0x0080;
            public static int _native = 0x0100;
            public static int _abstract = 0x0400;
            public static int _strictfp = 0x0800;

            /* sets of modifiers that can be attached to certain program elements    *
             * e.g., "constants" marks all modifiers that may be used with constants */
            public static int
              none = 0x0000,
              access = _public | _protected | _private,    // 0x0007
              classes = access | _abstract | _static | _final | _strictfp,    // 0x0c1f
              fields = access | _static | _final | _transient | _volatile,  // 0x00df
              methods = access | _abstract | _static | _final | _synchronized | _native | _strictfp, // 0x0d3f
              constructors = access, // 0x0007
              interfaces = access | _abstract | _static | _strictfp, // 0x0c0f
              constants = _public | _static | _final, // 0x0019
              all = 0x0dff;
        }

        public class Modifiers
        {
            private long cur = 0L;
            private Parser parser;

            public Modifiers(Parser parser)
            {
                this.parser = parser;
            }

            public void add(long m)
            {
                if ((cur & m) == 0) cur |= m;
                else parser.error("repeated modifier");
            }

            public void check(long allowed)
            {
                long wrong = cur & (allowed ^ Modifier.all);
                if (wrong != Modifier.none)
                    parser.error("modifier(s) " + toString(wrong) + "not allowed here");
                else
                    checkAccess();
            }

            private void checkAccess()
            {
                long access = cur & Modifier.access;
                if (access != Modifier.none && access != Modifier._public &&
                    access != Modifier._protected && access != Modifier._private)
                    parser.error("illegal combination of modifiers: " + toString(access));
            }

            private String toString(long m)
            {
                String s = "";
                if ((m & Modifier._public) != 0) s += "public ";
                if ((m & Modifier._private) != 0) s += "private ";
                if ((m & Modifier._protected) != 0) s += "protected ";
                if ((m & Modifier._static) != 0) s += "static ";
                if ((m & Modifier._final) != 0) s += "final ";
                if ((m & Modifier._synchronized) != 0) s += "synchronized ";
                if ((m & Modifier._volatile) != 0) s += "volatile ";
                if ((m & Modifier._transient) != 0) s += "transient ";
                if ((m & Modifier._native) != 0) s += "native ";
                if ((m & Modifier._abstract) != 0) s += "abstract ";
                if ((m & Modifier._strictfp) != 0) s += "strictfp ";
                return s;
            }
        }

        /*-------------------- expression handling ----------------------------------*/

        public class ExprKind
        {
            public static int NONE = 0;
            public static int CONDEXPR = 17;
            public static int APPLY = 25;
            public static int NEWCLASS = 26;
            public static int NEWARRAY = 27;
            public static int PARENS = 28;
            public static int ASSIGN = 29;
            public static int TYPECAST = 30;
            public static int TYPETEST = 31;
            public static int SELECT = 33;
            public static int IDENT = 34;
            public static int LITERAL = 35;
            public static int POS = 41;
            public static int NEG = 42;
            public static int NOT = 43;
            public static int COMPL = 44;
            public static int PREINC = 45;
            public static int PREDEC = 46;
            public static int POSTINC = 47;
            public static int POSTDEC = 48;
            public static int BINARY = 50;
        }

        class ExprInfo
        {
            private int kind = ExprKind.NONE;
            private Parser parser;

            public ExprInfo(Parser parser)
            {
                this.parser = parser;
            }

            public int getKind()
            {
                return kind;
            }

            public void setKind(int k)
            {
                kind = k;
            }

            public void checkExprStat()
            {
                if (kind != ExprKind.APPLY && kind != ExprKind.NEWCLASS &&
                     kind != ExprKind.ASSIGN && kind != ExprKind.PREINC &&
                     kind != ExprKind.PREDEC && kind != ExprKind.POSTINC &&
                     kind != ExprKind.POSTDEC)
                    parser.error("not a statement" + " (" + kind + ")");
            }
        }

        /*---------------------------- token sets -----------------------------------*/

        static int maxTerminals = 160;  // set size

        static BitArray newSet(int[] values)
        {
            BitArray s = new BitArray(maxTerminals);
            for (int i = 0; i < values.Length; i++) s.Set(values[i], true);
            return s;
        }

        static BitArray or(BitArray s1, BitArray s2)
        {
            s1.Or(s2);
            return s1;
        }

        static int[] typeKWarr = {_byte, _short, _char, _int, _long, _float, _double, 
                          _bool};
        static int[] castFollowerArr = {_ident, _new, _super, _this, _void, _intLit,
                                _floatLit, _charLit, _stringLit, _true, _false,
                                _null, _lpar, _not, _tilde};
        static int[] prefixArr = { _inc, _dec, _not, _tilde, _plus, _minus };

        static BitArray
          typeKW = newSet(typeKWarr),
          castFollower = or(newSet(castFollowerArr), typeKW),
          prefix = newSet(prefixArr);

        /*---------------------------- auxiliary methods ----------------------------*/

        public void error(String s)
        {
            if (errDist >= minErrDist) errors.SemErr(la.line, la.col, s);
            errDist = 0;
        }

        // "(" BasicType {"[""]"} ")"
        bool isSimpleTypeCast()
        {
            // assert: la.kind == _lpar
            scanner.ResetPeek();
            Token pt1 = scanner.Peek();

            if (typeKW.Get(pt1.kind))
            {
                Token pt = scanner.Peek();
                pt = skipDims(pt);
                if (pt != null)
                {
                    return pt.kind == _rpar;
                }
            }
            return false;
        }

        // "(" Qualident {"[" "]"} ")" castFollower
        bool guessTypeCast()
        {
            // assert: la.kind == _lpar
            scanner.ResetPeek();
            Token pt = scanner.Peek();
            pt = rdQualident(pt);
            if (pt != null)
            {
                pt = skipDims(pt);
                if (pt != null)
                {
                    Token pt1 = scanner.Peek();
                    return pt.kind == _rpar && castFollower.Get(pt1.kind);
                }
            }
            return false;
        }

        // "[" "]"
        Token skipDims(Token pt)
        {
            if (pt.kind != _lbrack) return pt;
            do
            {
                pt = scanner.Peek();
                if (pt.kind != _rbrack) return null;
                pt = scanner.Peek();
            } while (pt.kind == _lbrack);
            return pt;
        }

        /* Checks whether the next sequence of tokens is a qualident *
         * and returns the qualident string                          *
         * !!! Proceeds from current peek position !!!               */
        Token rdQualident(Token pt)
        {
            String qualident = "";

            if (pt.kind == _ident)
            {
                qualident = pt.val;
                pt = scanner.Peek();
                while (pt.kind == _dot)
                {
                    pt = scanner.Peek();
                    if (pt.kind != _ident) return null;
                    qualident += "." + pt.val;
                    pt = scanner.Peek();
                }
                return pt;
            }
            else return null;
        }

        // Return the n-th token after the current lookahead token
        Token peek(int n)
        {
            scanner.ResetPeek();
            Token x = la;
            while (n > 0) { x = scanner.Peek(); n--; }
            return x;
        }

        /*-----------------------------------------------------------------*
         * Resolver routines to resolve LL(1) conflicts:                   *
         * These routines return a bool value that indicates            *
         * whether the alternative at hand shall be choosen or not.        *
         * They are used in IF ( ... ) expressions.                        *       
         *-----------------------------------------------------------------*/

        // ',' (no '}')
        bool commaAndNoRBrace()
        {
            return (la.kind == _comma && peek(1).kind != _rbrace);
        }

        // '.' ident
        bool dotAndIdent()
        {
            return la.kind == _dot && peek(1).kind == _ident;
        }

        // ident '('
        bool identAndLPar()
        {
            return la.kind == _ident && peek(1).kind == _lpar;
        }

        // ident ':'
        bool isLabel()
        {
            return la.kind == _ident && peek(1).kind == _colon;
        }

        // '[' (no ']')
        bool nonEmptyBracket()
        {
            return (la.kind == _lbrack && peek(1).kind != _rbrack);
        }

        // '['']'
        bool emptyBracket()
        {
            return (la.kind == _lbrack && peek(1).kind == _rbrack);
        }

        // final or Type ident
        bool isLocalVarDecl(bool finalIsSuccess)
        {
            Token pt = la;
            scanner.ResetPeek();

            if (la.kind == _final)
                if (finalIsSuccess) return true;
                else pt = scanner.Peek();

            // basicType | ident
            if (typeKW.Get(pt.kind))
                pt = scanner.Peek();
            else
                pt = rdQualident(pt);

            if (pt != null)
            {
                pt = skipDims(pt);
                if (pt != null)
                {
                    return pt.kind == _ident;
                }
            }
            return false;
        }

        bool isTypeCast()
        {
            if (la.kind != _lpar) return false;
            if (isSimpleTypeCast()) return true;
            return guessTypeCast();
        }

        // '.' ("super" '.' | "class" | "this") | '(' | '['']'
        bool isIdentSuffix()
        {
            if (la.kind == _dot)
            {
                scanner.ResetPeek();
                Token pt = scanner.Peek();
                if (pt.kind == _super) return scanner.Peek().kind == _dot;
                return (pt.kind == _class || pt.kind == _this);
            }
            return (la.kind == _lpar || emptyBracket());
        }

        /*-------------------------------------------------------------------------*/



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


        void CompilationUnit()
        {
            if (la.kind == 39)
            {
                Get();
                Qualident();
                Expect(40);
            }
            while (la.kind == 41)
            {
                ImportDeclaration();
            }
            while (StartOf(1))
            {
                TypeDeclaration();
            }
            if (la.kind != _EOF) error("'class' or 'interface' expected");
        }

        void Qualident()
        {
            Expect(1);
            while (la.kind == 27)
            {
                Get();
                Expect(1);
            }
        }

        void ImportDeclaration()
        {
            Expect(41);
            TokenMatch tm = new TokenMatch(la.val, la.pos);
            this.m_Current = la.val;
            Expect(1);
            QualifiedImport();
            Expect(40);
            tm.Value = this.m_Current;
            this.m_CodeInfo.Imports.Add(tm);
        }

        void TypeDeclaration()
        {
            if (StartOf(2))
            {
                ClassOrInterfaceDeclaration();
            }
            else if (la.kind == 40)
            {
                Get();
            }
            else SynErr(102);
        }

        void QualifiedImport()
        {
            Expect(27);
            this.m_Current += "." + la.val;
            if (la.kind == 1)
            {
                Get();
                if (la.kind == 27)
                {
                    QualifiedImport();
                }
            }
            else if (la.kind == 42)
            {
                Get();
            }
            else SynErr(103);
        }

        void ClassOrInterfaceDeclaration()
        {
            Modifiers m = new Modifiers(this);
            while (StartOf(3))
            {
                ClassModifier(m);
            }
            if (la.kind == 9)
            {
                ClassDeclaration(m);
            }
            else if (la.kind == 56)
            {
                InterfaceDeclaration(m);
            }
            else SynErr(104);
        }

        void ClassModifier(Modifiers m)
        {
            switch (la.kind)
            {
                case 43:
                    {
                        Get();
                        m.add(Modifier._public);
                        break;
                    }
                case 44:
                    {
                        Get();
                        m.add(Modifier._protected);
                        break;
                    }
                case 45:
                    {
                        Get();
                        m.add(Modifier._private);
                        break;
                    }
                case 46:
                    {
                        Get();
                        m.add(Modifier._abstract);
                        break;
                    }
                case 19:
                    {
                        Get();
                        m.add(Modifier._static);
                        break;
                    }
                case 12:
                    {
                        Get();
                        m.add(Modifier._final);
                        break;
                    }
                case 47:
                    {
                        Get();
                        m.add(Modifier._strictfp);
                        break;
                    }
                default: SynErr(105); break;
            }
        }

        void ClassDeclaration(Modifiers m)
        {
            m.check(Modifier.classes);
            Expect(9);
            Expect(1);
            if (la.kind == 53)
            {
                Get();
                Type();
            }
            if (la.kind == 54)
            {
                Get();
                TypeList();
            }
            ClassBody();
        }

        void InterfaceDeclaration(Modifiers m)
        {
            m.check(Modifier.interfaces);
            Expect(56);
            Expect(1);
            if (la.kind == 53)
            {
                Get();
                TypeList();
            }
            InterfaceBody();
        }

        void Modifier2(Modifiers m)
        {
            if (la.kind == 19)
            {
                Get();
                m.add(Modifier._static);
            }
            else if (StartOf(4))
            {
                Modifier1(m);
            }
            else SynErr(106);
        }

        void Modifier1(Modifiers m)
        {
            switch (la.kind)
            {
                case 43:
                    {
                        Get();
                        m.add(Modifier._public);
                        break;
                    }
                case 44:
                    {
                        Get();
                        m.add(Modifier._protected);
                        break;
                    }
                case 45:
                    {
                        Get();
                        m.add(Modifier._private);
                        break;
                    }
                case 46:
                    {
                        Get();
                        m.add(Modifier._abstract);
                        break;
                    }
                case 12:
                    {
                        Get();
                        m.add(Modifier._final);
                        break;
                    }
                case 48:
                    {
                        Get();
                        m.add(Modifier._native);
                        break;
                    }
                case 49:
                    {
                        Get();
                        m.add(Modifier._synchronized);
                        break;
                    }
                case 50:
                    {
                        Get();
                        m.add(Modifier._transient);
                        break;
                    }
                case 51:
                    {
                        Get();
                        m.add(Modifier._volatile);
                        break;
                    }
                case 47:
                    {
                        Get();
                        m.add(Modifier._strictfp);
                        break;
                    }
                default: SynErr(107); break;
            }
        }

        void Type()
        {
            if (la.kind == 1)
            {
                Qualident();
            }
            else if (StartOf(5))
            {
                BasicType();
            }
            else SynErr(108);
            BracketsOpt();
        }

        void BasicType()
        {
            switch (la.kind)
            {
                case 7:
                    {
                        Get();
                        break;
                    }
                case 18:
                    {
                        Get();
                        break;
                    }
                case 8:
                    {
                        Get();
                        break;
                    }
                case 14:
                    {
                        Get();
                        break;
                    }
                case 15:
                    {
                        Get();
                        break;
                    }
                case 13:
                    {
                        Get();
                        break;
                    }
                case 10:
                    {
                        Get();
                        break;
                    }
                case 6:
                    {
                        Get();
                        break;
                    }
                default: SynErr(109); break;
            }
        }

        void BracketsOpt()
        {
            while (la.kind == 30)
            {
                Get();
                Expect(36);
            }
        }

        void TypeList()
        {
            Type();
            while (la.kind == 25)
            {
                Get();
                Type();
            }
        }

        void FormalParameter()
        {
            if (la.kind == 12)
            {
                Get();
            }
            if (!this.m_Current.Equals("("))
            {
                this.m_Current += ", ";
            }
            this.m_Current += la.val;
            Type();
            VariableDeclaratorId();
        }

        void VariableDeclaratorId()
        {
            Expect(1);
            BracketsOpt();
        }

        void QualidentList()
        {
            Qualident();
            while (la.kind == 25)
            {
                Get();
                Qualident();
            }
        }

        void VariableDeclarator()
        {
            Expect(1);
            VariableDeclaratorRest();
        }

        void VariableDeclaratorRest()
        {
            BracketsOpt();
            if (la.kind == 52)
            {
                Get();
                VariableInitializer();
            }
        }

        void VariableInitializer()
        {
            ExprInfo dummy = new ExprInfo(this);
            if (la.kind == 29)
            {
                ArrayInitializer();
            }
            else if (StartOf(6))
            {
                Expression(dummy);
            }
            else SynErr(110);
        }

        void ArrayInitializer()
        {
            Expect(29);
            if (StartOf(7))
            {
                VariableInitializer();
                while (commaAndNoRBrace())
                {
                    Expect(25);
                    VariableInitializer();
                }
            }
            if (la.kind == 25)
            {
                Get();
            }
            Expect(35);
        }

        void Expression(ExprInfo info)
        {
            Expression1(info);
            while (StartOf(8))
            {
                ExprInfo dummy = new ExprInfo(this);
                info.setKind(ExprKind.ASSIGN);
                AssignmentOperator();
                Expression1(dummy);
            }
        }

        void ClassBody()
        {
            Expect(29);
            while (StartOf(9))
            {
                ClassBodyDeclaration();
            }
            Expect(35);
        }

        void ClassBodyDeclaration()
        {
            if (la.kind == 40)
            {
                Get();
            }
            else if (StartOf(10))
            {
                Modifiers m = new Modifiers(this);
                if (la.kind == 19)
                {
                    Get();
                    m.add(Modifier._static);
                }
                if (la.kind == 29)
                {
                    Block();
                }
                else if (StartOf(11))
                {
                    if (StartOf(4))
                    {
                        Modifier1(m);
                        while (StartOf(12))
                        {
                            Modifier2(m);
                        }
                    }
                    MemberDecl(m);
                }
                else SynErr(111);
            }
            else SynErr(112);
        }

        void Block()
        {
            Expect(29);
            while (StartOf(13))
            {
                BlockStatement();
            }
            Expect(35);
        }

        void MemberDecl(Modifiers m)
        {
            string dataType = la.val;
            TokenMatch tm = new TokenMatch();
            if (identAndLPar())
            {
                tm.Position = la.pos;
                tm.Value = la.val;
                this.m_Current = "(";
                Expect(1);
                ConstructorDeclaratorRest(m);
                if (!this.m_Current.Equals("()"))
                {
                    this.m_Current += ")";
                }
                tm.Value += this.m_Current;
                this.m_CodeInfo.Constructors.Add(tm);
            }
            else if (StartOf(14))
            {
                this.m_Current = "";
                tm.Position = la.pos;
                MethodOrFieldDecl(m);
                tm.Value = this.m_Current + ":" + dataType;
                this.m_CodeInfo.Fields.Add(tm);
            }
            else if (la.kind == 23)
            {
                m.check(Modifier.methods);
                Get();
                tm.Position = la.pos;
                tm.Value = la.val;
                this.m_Current = "(";
                Expect(1);
                VoidMethodDeclaratorRest();
                if (this.m_Current.Equals("()"))
                {
                    this.m_Current += ":" + dataType;
                }
                else
                {
                    this.m_Current += ")" + ":" + dataType;
                }
                tm.Value += this.m_Current;
                this.m_CodeInfo.Methods.Add(tm);
            }
            else if (la.kind == 9)
            {
                ClassDeclaration(m);
            }
            else if (la.kind == 56)
            {
                InterfaceDeclaration(m);
            }
            else SynErr(113);
        }

        void ConstructorDeclaratorRest(Modifiers m)
        {
            m.check(Modifier.constructors);
            FormalParameters();
            if (la.kind == 55)
            {
                Get();
                QualidentList();
            }
            Block();
        }

        void MethodOrFieldDecl(Modifiers m)
        {
            Type();
            string gen = "";
            if (la.kind == 92)
            {
                gen = la.val;
                Get();
                gen += la.val;
                Get();
                gen += la.val;
                Get();
            }
            this.m_Current += la.val + gen;
            Expect(1);
            MethodOrFieldRest(m);
        }

        void VoidMethodDeclaratorRest()
        {
            FormalParameters();
            if (la.kind == 55)
            {
                Get();
                QualidentList();
            }
            if (la.kind == 29)
            {
                Block();
            }
            else if (la.kind == 40)
            {
                Get();
            }
            else SynErr(114);
        }

        void MethodOrFieldRest(Modifiers m)
        {
            if (StartOf(15))
            {
                m.check(Modifier.fields);
                VariableDeclaratorsRest();
                Expect(40);
            }
            else if (la.kind == 31)
            {
                m.check(Modifier.methods);
                MethodDeclaratorRest();
            }
            else SynErr(115);
        }

        void VariableDeclaratorsRest()
        {
            VariableDeclaratorRest();
            while (la.kind == 25)
            {
                Get();
                VariableDeclarator();
            }
        }

        void MethodDeclaratorRest()
        {
            FormalParameters();
            BracketsOpt();
            if (la.kind == 55)
            {
                Get();
                QualidentList();
            }
            if (la.kind == 29)
            {
                Block();
            }
            else if (la.kind == 40)
            {
                Get();
            }
            else SynErr(116);
        }

        void FormalParameters()
        {
            Expect(31);
            if (StartOf(16))
            {
                FormalParameter();
                while (la.kind == 25)
                {
                    Get();
                    FormalParameter();
                }
            }
            Expect(37);
        }

        void InterfaceBody()
        {
            Expect(29);
            while (StartOf(17))
            {
                InterfaceBodyDeclaration();
            }
            Expect(35);
        }

        void InterfaceBodyDeclaration()
        {
            Modifiers m = new Modifiers(this);
            if (la.kind == 40)
            {
                Get();
            }
            else if (StartOf(18))
            {
                while (StartOf(12))
                {
                    Modifier2(m);
                }
                InterfaceMemberDecl(m);
            }
            else SynErr(117);
        }

        void InterfaceMemberDecl(Modifiers m)
        {
            if (StartOf(14))
            {
                InterfaceMethodOrFieldDecl(m);
            }
            else if (la.kind == 23)
            {
                m.check(Modifier.interfaces);
                Get();
                Expect(1);
                VoidInterfaceMethodDeclaratorRest();
            }
            else if (la.kind == 9)
            {
                ClassDeclaration(m);
            }
            else if (la.kind == 56)
            {
                InterfaceDeclaration(m);
            }
            else SynErr(118);
        }

        void InterfaceMethodOrFieldDecl(Modifiers m)
        {
            Type();
            Expect(1);
            InterfaceMethodOrFieldRest(m);
        }

        void VoidInterfaceMethodDeclaratorRest()
        {
            FormalParameters();
            if (la.kind == 55)
            {
                Get();
                QualidentList();
            }
            Expect(40);
        }

        void InterfaceMethodOrFieldRest(Modifiers m)
        {
            if (la.kind == 30 || la.kind == 52)
            {
                m.check(Modifier.constants);
                ConstantDeclaratorsRest();
                Expect(40);
            }
            else if (la.kind == 31)
            {
                m.check(Modifier.interfaces);
                InterfaceMethodDeclaratorRest();
            }
            else SynErr(119);
        }

        void ConstantDeclaratorsRest()
        {
            ConstantDeclaratorRest();
            while (la.kind == 25)
            {
                Get();
                ConstantDeclarator();
            }
        }

        void InterfaceMethodDeclaratorRest()
        {
            FormalParameters();
            BracketsOpt();
            if (la.kind == 55)
            {
                Get();
                QualidentList();
            }
            Expect(40);
        }

        void ConstantDeclaratorRest()
        {
            BracketsOpt();
            Expect(52);
            VariableInitializer();
        }

        void ConstantDeclarator()
        {
            Expect(1);
            ConstantDeclaratorRest();
        }

        void Statement()
        {
            ExprInfo dummy = new ExprInfo(this);
            if (la.kind == 29)
            {
                Block();
            }
            else if (la.kind == 57)
            {
                Get();
                ParExpression();
                Statement();
                if (la.kind == 58)
                {
                    Get();
                    Statement();
                }
            }
            else if (la.kind == 59)
            {
                Get();
                Expect(31);
                if (StartOf(19))
                {
                    ForInit();
                }
                Expect(40);
                if (StartOf(6))
                {
                    Expression(dummy);
                }
                Expect(40);
                if (StartOf(6))
                {
                    ForUpdate();
                }
                Expect(37);
                Statement();
            }
            else if (la.kind == 60)
            {
                Get();
                ParExpression();
                Statement();
            }
            else if (la.kind == 61)
            {
                Get();
                Statement();
                Expect(60);
                ParExpression();
                Expect(40);
            }
            else if (la.kind == 62)
            {
                Get();
                Block();
                if (la.kind == 69)
                {
                    Catches();
                    if (la.kind == 63)
                    {
                        Get();
                        Block();
                    }
                }
                else if (la.kind == 63)
                {
                    Get();
                    Block();
                }
                else SynErr(120);
            }
            else if (la.kind == 64)
            {
                Get();
                ParExpression();
                Expect(29);
                SwitchBlockStatementGroups();
                Expect(35);
            }
            else if (la.kind == 49)
            {
                Get();
                ParExpression();
                Block();
            }
            else if (la.kind == 65)
            {
                Get();
                if (StartOf(6))
                {
                    Expression(dummy);
                }
                Expect(40);
            }
            else if (la.kind == 66)
            {
                Get();
                Expression(dummy);
                Expect(40);
            }
            else if (la.kind == 67)
            {
                Get();
                if (la.kind == 1)
                {
                    Get();
                }
                Expect(40);
            }
            else if (la.kind == 68)
            {
                Get();
                if (la.kind == 1)
                {
                    Get();
                }
                Expect(40);
            }
            else if (la.kind == 40)
            {
                Get();
            }
            else if (isLabel())
            {
                Expect(1);
                Expect(24);
                Statement();
            }
            else if (StartOf(6))
            {
                StatementExpression();
                Expect(40);
            }
            else SynErr(121);
        }

        void ParExpression()
        {
            ExprInfo dummy = new ExprInfo(this);
            Expect(31);
            Expression(dummy);
            Expect(37);
        }

        void ForInit()
        {
            if (isLocalVarDecl(true))
            {
                LocalVariableDeclaration();
            }
            else if (StartOf(6))
            {
                StatementExpression();
                MoreStatementExpressions();
            }
            else SynErr(122);
        }

        void ForUpdate()
        {
            StatementExpression();
            MoreStatementExpressions();
        }

        void Catches()
        {
            CatchClause();
            while (la.kind == 69)
            {
                CatchClause();
            }
        }

        void SwitchBlockStatementGroups()
        {
            while (la.kind == 70 || la.kind == 71)
            {
                SwitchBlockStatementGroup();
            }
        }

        void StatementExpression()
        {
            ExprInfo info = new ExprInfo(this);
            Expression(info);
            info.checkExprStat();
        }

        void BlockStatement()
        {
            if (isLocalVarDecl(false))
            {
                LocalVariableDeclaration();
                Expect(40);
            }
            else if (StartOf(2))
            {
                ClassOrInterfaceDeclaration();
            }
            else if (StartOf(20))
            {
                Statement();
            }
            else SynErr(123);
        }

        void LocalVariableDeclaration()
        {
            if (la.kind == 12)
            {
                Get();
            }
            Type();
            VariableDeclarators();
        }

        void VariableDeclarators()
        {
            VariableDeclarator();
            while (la.kind == 25)
            {
                Get();
                VariableDeclarator();
            }
        }

        void MoreStatementExpressions()
        {
            while (la.kind == 25)
            {
                Get();
                StatementExpression();
            }
        }

        void CatchClause()
        {
            Expect(69);
            Expect(31);
            FormalParameter();
            Expect(37);
            Block();
        }

        void SwitchBlockStatementGroup()
        {
            SwitchLabel();
            while (StartOf(13))
            {
                BlockStatement();
            }
        }

        void SwitchLabel()
        {
            if (la.kind == 70)
            {
                ExprInfo dummy = new ExprInfo(this);
                Get();
                Expression(dummy);
                Expect(24);
            }
            else if (la.kind == 71)
            {
                Get();
                Expect(24);
            }
            else SynErr(124);
        }

        void Expression1(ExprInfo info)
        {
            Expression2(info);
            if (la.kind == 72)
            {
                info.setKind(ExprKind.CONDEXPR);
                ConditionalExpr();
            }
        }

        void AssignmentOperator()
        {
            switch (la.kind)
            {
                case 52:
                    {
                        Get();
                        break;
                    }
                case 74:
                    {
                        Get();
                        break;
                    }
                case 75:
                    {
                        Get();
                        break;
                    }
                case 76:
                    {
                        Get();
                        break;
                    }
                case 77:
                    {
                        Get();
                        break;
                    }
                case 78:
                    {
                        Get();
                        break;
                    }
                case 79:
                    {
                        Get();
                        break;
                    }
                case 80:
                    {
                        Get();
                        break;
                    }
                case 81:
                    {
                        Get();
                        break;
                    }
                case 82:
                    {
                        Get();
                        break;
                    }
                case 83:
                    {
                        Get();
                        break;
                    }
                case 84:
                    {
                        Get();
                        break;
                    }
                default: SynErr(125); break;
            }
        }

        void Expression2(ExprInfo info)
        {
            Expression3(info);
            if (StartOf(21))
            {
                Expression2Rest(info);
            }
        }

        void ConditionalExpr()
        {
            ExprInfo dummy = new ExprInfo(this);
            Expect(72);
            Expression(dummy);
            Expect(24);
            Expression1(dummy);
        }

        void Expression3(ExprInfo info)
        {
            int pre = ExprKind.NONE;
            while (prefix.Get(la.kind) || isTypeCast())
            {
                if (StartOf(22))
                {
                    PrefixOp(info);
                    if (pre == ExprKind.NONE) pre = info.getKind();
                }
                else if (la.kind == 31)
                {
                    Get();
                    Type();
                    Expect(37);
                    info.setKind(ExprKind.TYPECAST);
                }
                else SynErr(126);
            }
            Primary(info);
            while (la.kind == 27 || la.kind == 30)
            {
                Selector(info);
            }
            while (la.kind == 26 || la.kind == 28)
            {
                PostfixOp(info);
            }
            if (pre != ExprKind.NONE) info.setKind(pre);
        }

        void Expression2Rest(ExprInfo info)
        {
            ExprInfo dummy = new ExprInfo(this);
            if (StartOf(23))
            {
                Infixop();
                Expression3(dummy);
                while (StartOf(23))
                {
                    Infixop();
                    Expression3(dummy);
                }
                info.setKind(ExprKind.BINARY);
            }
            else if (la.kind == 73)
            {
                Get();
                Type();
                info.setKind(ExprKind.TYPETEST);
            }
            else SynErr(127);
        }

        void Infixop()
        {
            switch (la.kind)
            {
                case 85:
                    {
                        Get();
                        break;
                    }
                case 86:
                    {
                        Get();
                        break;
                    }
                case 87:
                    {
                        Get();
                        break;
                    }
                case 88:
                    {
                        Get();
                        break;
                    }
                case 89:
                    {
                        Get();
                        break;
                    }
                case 90:
                    {
                        Get();
                        break;
                    }
                case 91:
                    {
                        Get();
                        break;
                    }
                case 92:
                    {
                        Get();
                        break;
                    }
                case 93:
                    {
                        Get();
                        break;
                    }
                case 94:
                    {
                        Get();
                        break;
                    }
                case 95:
                    {
                        Get();
                        break;
                    }
                case 96:
                    {
                        Get();
                        break;
                    }
                case 97:
                    {
                        Get();
                        break;
                    }
                case 98:
                    {
                        Get();
                        break;
                    }
                case 34:
                    {
                        Get();
                        break;
                    }
                case 32:
                    {
                        Get();
                        break;
                    }
                case 42:
                    {
                        Get();
                        break;
                    }
                case 99:
                    {
                        Get();
                        break;
                    }
                case 100:
                    {
                        Get();
                        break;
                    }
                default: SynErr(128); break;
            }
        }

        void PrefixOp(ExprInfo info)
        {
            switch (la.kind)
            {
                case 28:
                    {
                        Get();
                        info.setKind(ExprKind.PREINC);
                        break;
                    }
                case 26:
                    {
                        Get();
                        info.setKind(ExprKind.PREDEC);
                        break;
                    }
                case 33:
                    {
                        Get();
                        info.setKind(ExprKind.NOT);
                        break;
                    }
                case 38:
                    {
                        Get();
                        info.setKind(ExprKind.COMPL);
                        break;
                    }
                case 34:
                    {
                        Get();
                        info.setKind(ExprKind.POS);
                        break;
                    }
                case 32:
                    {
                        Get();
                        info.setKind(ExprKind.NEG);
                        break;
                    }
                default: SynErr(129); break;
            }
        }

        void Primary(ExprInfo info)
        {
            switch (la.kind)
            {
                case 31:
                    {
                        Get();
                        Expression(info);
                        Expect(37);
                        info.setKind(ExprKind.PARENS);
                        break;
                    }
                case 21:
                    {
                        Get();
                        info.setKind(ExprKind.IDENT);
                        ArgumentsOpt(info);
                        break;
                    }
                case 20:
                    {
                        Get();
                        SuperSuffix(info);
                        break;
                    }
                case 2:
                case 3:
                case 4:
                case 5:
                case 11:
                case 17:
                case 22:
                    {
                        Literal();
                        info.setKind(ExprKind.LITERAL);
                        break;
                    }
                case 16:
                    {
                        Get();
                        Creator(info);
                        break;
                    }
                case 1:
                    {
                        Get();
                        while (dotAndIdent())
                        {
                            Expect(27);
                            Expect(1);
                        }
                        info.setKind(ExprKind.IDENT);
                        if (isIdentSuffix())
                        {
                            IdentifierSuffix(info);
                        }
                        break;
                    }
                case 6:
                case 7:
                case 8:
                case 10:
                case 13:
                case 14:
                case 15:
                case 18:
                    {
                        BasicType();
                        BracketsOpt();
                        Expect(27);
                        Expect(9);
                        info.setKind(ExprKind.SELECT);
                        break;
                    }
                case 23:
                    {
                        Get();
                        Expect(27);
                        Expect(9);
                        info.setKind(ExprKind.SELECT);
                        break;
                    }
                default: SynErr(130); break;
            }
        }

        void Selector(ExprInfo info)
        {
            ExprInfo dummy = new ExprInfo(this);
            if (la.kind == 27)
            {
                Get();
                if (la.kind == 1)
                {
                    Get();
                    ArgumentsOpt(info);
                }
                else if (la.kind == 20)
                {
                    Get();
                    Arguments();
                }
                else if (la.kind == 16)
                {
                    Get();
                    InnerCreator();
                }
                else SynErr(131);
            }
            else if (la.kind == 30)
            {
                Get();
                Expression(dummy);
                Expect(36);
            }
            else SynErr(132);
        }

        void PostfixOp(ExprInfo info)
        {
            if (la.kind == 28)
            {
                Get();
                info.setKind(ExprKind.POSTINC);
            }
            else if (la.kind == 26)
            {
                Get();
                info.setKind(ExprKind.POSTDEC);
            }
            else SynErr(133);
        }

        void ArgumentsOpt(ExprInfo info)
        {
            if (la.kind == 31)
            {
                info.setKind(ExprKind.APPLY);
                Arguments();
            }
        }

        void SuperSuffix(ExprInfo info)
        {
            if (la.kind == 31)
            {
                Arguments();
                info.setKind(ExprKind.APPLY);
            }
            else if (la.kind == 27)
            {
                Get();
                Expect(1);
                info.setKind(ExprKind.IDENT);
                ArgumentsOpt(info);
            }
            else SynErr(134);
        }

        void Literal()
        {
            switch (la.kind)
            {
                case 2:
                    {
                        Get();
                        break;
                    }
                case 3:
                    {
                        Get();
                        break;
                    }
                case 4:
                    {
                        Get();
                        break;
                    }
                case 5:
                    {
                        Get();
                        break;
                    }
                case 22:
                    {
                        Get();
                        break;
                    }
                case 11:
                    {
                        Get();
                        break;
                    }
                case 17:
                    {
                        Get();
                        break;
                    }
                default: SynErr(135); break;
            }
        }

        void Creator(ExprInfo info)
        {
            if (StartOf(5))
            {
                BasicType();
                ArrayCreatorRest();
                info.setKind(ExprKind.NEWARRAY);
            }
            else if (la.kind == 1)
            {
                Qualident();
                if (la.kind == 30)
                {
                    ArrayCreatorRest();
                    info.setKind(ExprKind.NEWARRAY);
                }
                else if (la.kind == 31)
                {
                    ClassCreatorRest();
                    info.setKind(ExprKind.NEWCLASS);
                }
                else SynErr(136);
            }
            else SynErr(137);
        }

        void IdentifierSuffix(ExprInfo info)
        {
            if (la.kind == 30)
            {
                Get();
                Expect(36);
                BracketsOpt();
                Expect(27);
                Expect(9);
                info.setKind(ExprKind.SELECT);
            }
            else if (la.kind == 31)
            {
                Arguments();
                info.setKind(ExprKind.APPLY);
            }
            else if (la.kind == 27)
            {
                Get();
                if (la.kind == 9)
                {
                    Get();
                }
                else if (la.kind == 21)
                {
                    Get();
                }
                else if (la.kind == 20)
                {
                    Get();
                    Expect(27);
                    Expect(1);
                    ArgumentsOpt(info);
                }
                else SynErr(138);
            }
            else SynErr(139);
        }

        void Arguments()
        {
            ExprInfo dummy = new ExprInfo(this);
            Expect(31);
            if (StartOf(6))
            {
                Expression(dummy);
                while (la.kind == 25)
                {
                    Get();
                    Expression(dummy);
                }
            }
            Expect(37);
        }

        void ArrayCreatorRest()
        {
            ExprInfo dummy = new ExprInfo(this);
            Expect(30);
            if (la.kind == 36)
            {
                Get();
                BracketsOpt();
                ArrayInitializer();
            }
            else if (StartOf(6))
            {
                Expression(dummy);
                Expect(36);
                while (nonEmptyBracket())
                {
                    Expect(30);
                    Expression(dummy);
                    Expect(36);
                }
                while (emptyBracket())
                {
                    Expect(30);
                    Expect(36);
                }
            }
            else SynErr(140);
        }

        void ClassCreatorRest()
        {
            Arguments();
            if (la.kind == 29)
            {
                ClassBody();
            }
        }

        void InnerCreator()
        {
            Expect(1);
            ClassCreatorRest();
        }



        public void Parse()
        {
            la = new Token();
            la.val = "";
            Get();
            CompilationUnit();

            Expect(0);
        }

        bool[,] set = {
		{T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,x,x,x, x,x,x,x, x,T,x,x, T,x,x,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,T, T,T,T,T, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,x,x,x, x,x,x,x, x,T,x,x, T,x,x,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, T,T,T,T, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, T,T,T,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, T,T,T,T, T,T,T,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,x,x,x, x,x,T,T, T,x,T,x, x,T,T,T, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,T,T, T,T,T,T, T,x,T,T, x,T,T,T, T,T,T,x, T,T,T,T, x,x,T,x, T,x,x,T, T,T,T,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,T,T, T,T,T,T, T,x,T,T, x,T,T,T, T,T,T,x, T,T,T,T, x,x,T,x, T,T,x,T, T,T,T,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,T, T,T,T,T, T,T,T,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,x,x, x,x,T,T, T,T,T,x, T,T,T,T, x,x,T,T, x,x,x,T, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, T,x,x,T, T,T,T,T, T,T,T,T, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,x,x, x,x,T,T, T,T,T,x, T,T,T,T, x,x,T,T, x,x,x,T, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,T, T,T,T,T, T,T,T,T, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,x,x, x,x,T,T, T,T,T,x, T,T,T,T, x,x,T,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, T,T,T,T, T,T,T,T, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, T,T,T,T, T,T,T,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, x,x,T,x, T,T,x,T, T,T,T,x, x,x,T,x, T,x,x,T, T,T,T,T, x,T,x,x, x,x,x,x, T,T,x,T, T,T,T,x, T,T,T,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,x,x, x,x,T,T, T,x,T,x, x,T,T,T, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,T,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,x,x, x,x,T,T, T,x,T,x, T,T,T,T, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,x,x, x,x,T,T, T,T,T,x, T,T,T,T, x,x,T,T, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,T, T,T,T,T, T,T,T,T, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,x,x, x,x,T,T, T,T,T,x, T,T,T,T, x,x,T,T, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, T,T,T,T, T,T,T,T, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,T,T, T,T,T,T, T,x,T,T, T,T,T,T, T,T,T,x, T,T,T,T, x,x,T,x, T,x,x,T, T,T,T,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,T,T, T,T,T,T, T,x,T,T, x,T,T,T, T,T,T,x, T,T,T,T, x,x,T,x, T,T,x,T, T,T,T,x, x,x,T,x, T,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,T,x,T, T,T,T,x, T,T,T,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,T,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,x,x, x,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, T,x,x,x, T,T,T,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,T,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,x,x}

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
                case 1: s = "ident expected"; break;
                case 2: s = "intLit expected"; break;
                case 3: s = "floatLit expected"; break;
                case 4: s = "charLit expected"; break;
                case 5: s = "stringLit expected"; break;
                case 6: s = "bool expected"; break;
                case 7: s = "byte expected"; break;
                case 8: s = "char expected"; break;
                case 9: s = "class expected"; break;
                case 10: s = "double expected"; break;
                case 11: s = "false expected"; break;
                case 12: s = "final expected"; break;
                case 13: s = "float expected"; break;
                case 14: s = "int expected"; break;
                case 15: s = "long expected"; break;
                case 16: s = "new expected"; break;
                case 17: s = "null expected"; break;
                case 18: s = "short expected"; break;
                case 19: s = "static expected"; break;
                case 20: s = "super expected"; break;
                case 21: s = "this expected"; break;
                case 22: s = "true expected"; break;
                case 23: s = "void expected"; break;
                case 24: s = "colon expected"; break;
                case 25: s = "comma expected"; break;
                case 26: s = "dec expected"; break;
                case 27: s = "dot expected"; break;
                case 28: s = "inc expected"; break;
                case 29: s = "lbrace expected"; break;
                case 30: s = "lbrack expected"; break;
                case 31: s = "lpar expected"; break;
                case 32: s = "minus expected"; break;
                case 33: s = "not expected"; break;
                case 34: s = "plus expected"; break;
                case 35: s = "rbrace expected"; break;
                case 36: s = "rbrack expected"; break;
                case 37: s = "rpar expected"; break;
                case 38: s = "tilde expected"; break;
                case 39: s = "\"package\" expected"; break;
                case 40: s = "\";\" expected"; break;
                case 41: s = "\"import\" expected"; break;
                case 42: s = "\"*\" expected"; break;
                case 43: s = "\"public\" expected"; break;
                case 44: s = "\"protected\" expected"; break;
                case 45: s = "\"private\" expected"; break;
                case 46: s = "\"abstract\" expected"; break;
                case 47: s = "\"strictfp\" expected"; break;
                case 48: s = "\"native\" expected"; break;
                case 49: s = "\"synchronized\" expected"; break;
                case 50: s = "\"transient\" expected"; break;
                case 51: s = "\"volatile\" expected"; break;
                case 52: s = "\"=\" expected"; break;
                case 53: s = "\"extends\" expected"; break;
                case 54: s = "\"implements\" expected"; break;
                case 55: s = "\"throws\" expected"; break;
                case 56: s = "\"interface\" expected"; break;
                case 57: s = "\"if\" expected"; break;
                case 58: s = "\"else\" expected"; break;
                case 59: s = "\"for\" expected"; break;
                case 60: s = "\"while\" expected"; break;
                case 61: s = "\"do\" expected"; break;
                case 62: s = "\"try\" expected"; break;
                case 63: s = "\"finally\" expected"; break;
                case 64: s = "\"switch\" expected"; break;
                case 65: s = "\"return\" expected"; break;
                case 66: s = "\"throw\" expected"; break;
                case 67: s = "\"break\" expected"; break;
                case 68: s = "\"continue\" expected"; break;
                case 69: s = "\"catch\" expected"; break;
                case 70: s = "\"case\" expected"; break;
                case 71: s = "\"default\" expected"; break;
                case 72: s = "\"?\" expected"; break;
                case 73: s = "\"instanceof\" expected"; break;
                case 74: s = "\"+=\" expected"; break;
                case 75: s = "\"-=\" expected"; break;
                case 76: s = "\"*=\" expected"; break;
                case 77: s = "\"/=\" expected"; break;
                case 78: s = "\"&=\" expected"; break;
                case 79: s = "\"|=\" expected"; break;
                case 80: s = "\"^=\" expected"; break;
                case 81: s = "\"%=\" expected"; break;
                case 82: s = "\"<<=\" expected"; break;
                case 83: s = "\">>=\" expected"; break;
                case 84: s = "\">>>=\" expected"; break;
                case 85: s = "\"||\" expected"; break;
                case 86: s = "\"&&\" expected"; break;
                case 87: s = "\"|\" expected"; break;
                case 88: s = "\"^\" expected"; break;
                case 89: s = "\"&\" expected"; break;
                case 90: s = "\"==\" expected"; break;
                case 91: s = "\"!=\" expected"; break;
                case 92: s = "\"<\" expected"; break;
                case 93: s = "\">\" expected"; break;
                case 94: s = "\"<=\" expected"; break;
                case 95: s = "\">=\" expected"; break;
                case 96: s = "\"<<\" expected"; break;
                case 97: s = "\">>\" expected"; break;
                case 98: s = "\">>>\" expected"; break;
                case 99: s = "\"/\" expected"; break;
                case 100: s = "\"%\" expected"; break;
                case 101: s = "??? expected"; break;
                case 102: s = "invalid TypeDeclaration"; break;
                case 103: s = "invalid QualifiedImport"; break;
                case 104: s = "invalid ClassOrInterfaceDeclaration"; break;
                case 105: s = "invalid ClassModifier"; break;
                case 106: s = "invalid Modifier"; break;
                case 107: s = "invalid Modifier1"; break;
                case 108: s = "invalid Type"; break;
                case 109: s = "invalid BasicType"; break;
                case 110: s = "invalid VariableInitializer"; break;
                case 111: s = "invalid ClassBodyDeclaration"; break;
                case 112: s = "invalid ClassBodyDeclaration"; break;
                case 113: s = "invalid MemberDecl"; break;
                case 114: s = "invalid VoidMethodDeclaratorRest"; break;
                case 115: s = "invalid MethodOrFieldRest"; break;
                case 116: s = "invalid MethodDeclaratorRest"; break;
                case 117: s = "invalid InterfaceBodyDeclaration"; break;
                case 118: s = "invalid InterfaceMemberDecl"; break;
                case 119: s = "invalid InterfaceMethodOrFieldRest"; break;
                case 120: s = "invalid Statement"; break;
                case 121: s = "invalid Statement"; break;
                case 122: s = "invalid ForInit"; break;
                case 123: s = "invalid BlockStatement"; break;
                case 124: s = "invalid SwitchLabel"; break;
                case 125: s = "invalid AssignmentOperator"; break;
                case 126: s = "invalid Expression3"; break;
                case 127: s = "invalid Expression2Rest"; break;
                case 128: s = "invalid Infixop"; break;
                case 129: s = "invalid PrefixOp"; break;
                case 130: s = "invalid Primary"; break;
                case 131: s = "invalid Selector"; break;
                case 132: s = "invalid Selector"; break;
                case 133: s = "invalid PostfixOp"; break;
                case 134: s = "invalid SuperSuffix"; break;
                case 135: s = "invalid Literal"; break;
                case 136: s = "invalid Creator"; break;
                case 137: s = "invalid Creator"; break;
                case 138: s = "invalid IdentifierSuffix"; break;
                case 139: s = "invalid IdentifierSuffix"; break;
                case 140: s = "invalid ArrayCreatorRest"; break;

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
