
using System;

namespace Peter.DParser
{


    public class Parser
    {
        const int _EOF = 0;
        const int _identifier = 1;
        const int _integervalue = 2;
        const int _floatnumber = 3;
        const int _stringvalue = 4;
        const int _int = 5;
        const int _string = 6;
        const int _float = 7;
        const int _void = 8;
        const int _var = 9;
        const int _const = 10;
        const int _instance = 11;
        const int _if = 12;
        const int _else = 13;
        const int _prototype = 14;
        const int _return = 15;
        const int _citem = 16;
        const int _cnpc = 17;
        const int _cmission = 18;
        const int _cfocus = 19;
        const int _cinfo = 20;
        const int _citemreact = 21;
        const int _cspell = 22;
        const int _func = 23;
        const int _class = 24;
        const int maxT = 67;

        const bool T = true;
        const bool x = false;
        const int minErrDist = 2;

        private string m_Current = "";
        private int m_CurrentPos = 0;
        public Scanner scanner;
        public Errors errors;
        public DCodeInfo m_CodeInfo = new DCodeInfo();

        public Token t;    // last recognized token
        public Token la;   // lookahead token
        int errDist = minErrDist;



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


        void D()
        {
            while (StartOf(1))
            {
                Block();
            }
        }

        void Block()
        {
            if (la.kind == 9 || la.kind == 10)
            {
                Declaration();
            }
            else if (la.kind == 23)
            {
                Function();
            }
            else if (la.kind == 11)
            {
                Instance();
            }
            else if (la.kind == 14)
            {
                Prototype();
            }
            else if (la.kind == 24)
            {
                Class();
            }
            else SynErr(68);
        }

        void Declaration()
        {
            if (la.kind == 10)
            {
                ConstDecl();
            }
            else if (la.kind == 9)
            {
                VarDecl();
            }
            else SynErr(69);
        }

        void Function()
        {
            Expect(23);
            if (StartOf(2))
            {
                this.m_Current = la.val.ToLower() + " ";
                type();
            }
            else if (la.kind == 8)
            {
                this.m_Current = la.val.ToLower() + " ";
                Get();
            }
            else SynErr(70);
            this.m_CurrentPos = la.pos;
            this.m_Current += la.val+" ";
            Expect(1);
            this.m_Current += la.val;
            Expect(32);
            //this.m_Current += la.val;
            if (la.kind == 9)
            {
                Parameter();
                
                while (la.kind == 27)
                {
                    Get();
                    Parameter();
                    
                }
            }
            TokenMatch tm = new TokenMatch();
            tm.Position = this.m_CurrentPos;
            //this.m_Current += la.val;
            m_Current = m_Current.Trim();
            if (!m_Current.EndsWith(")"))
                m_Current+=")";

            tm.Value = this.m_Current;
            this.m_CodeInfo.Functions.Add(tm);
            Expect(33);
            Expect(26);
            while (StartOf(3))
            {
                Body();
            }
            Expect(28);
            Expect(29);
        }

        void Instance()
        {
            Expect(11);
            TokenMatch tm = new TokenMatch();
            tm.Position = la.pos;
            this.m_Current = la.val;
            tm.Value = this.m_Current;
            this.m_CodeInfo.Instances.Add(tm);
            Expect(1);
            while (la.kind == 27)
            {
                Get();
                Expect(1);
            }
            Expect(32);
            if (la.kind == 1)
            {
                Get();
            }
            else if (StartOf(4))
            {
                classname();
            }
            else SynErr(71);
            Expect(33);
            if (la.kind == 26)
            {
                Get();
                while (StartOf(3))
                {
                    Body();
                }
                Expect(28);
            }
            Expect(29);
        }

        void Prototype()
        {
            Expect(14);
            Expect(1);
            Expect(32);
            if (la.kind == 1)
            {
                Get();
            }
            else if (StartOf(4))
            {
                classname();
            }
            else SynErr(72);
            Expect(33);
            Expect(26);
            while (StartOf(3))
            {
                Body();
            }
            Expect(28);
            Expect(29);
        }

        void Class()
        {
            Expect(24);
            if (la.kind == 1)
            {
                Get();
            }
            else if (StartOf(4))
            {
                classname();
            }
            else SynErr(73);
            Expect(26);
            while (StartOf(3))
            {
                Body();
            }
            Expect(28);
            Expect(29);
        }

        void ConstDecl()
        {
            Expect(10);
            this.m_Current = "@" + la.val.ToLower() + " ";
            m_CurrentPos = la.pos;
            TypeDecl();
           
            Expect(25);
            this.m_Current += (" = " + la.val);
           
            if (StartOf(5))
            {
                Expression();
                
            }
            else if (la.kind == 26)
            {
                Get();
                Expression();
                
                while (la.kind == 27)
                {
                    Get();
                    Expression();
                }                
                Expect(28);
                
                
            }
            else SynErr(74);

            
            Expect(29);
            
                this.m_Current = this.m_Current.Remove(0,1);
                TokenMatch tm = new TokenMatch();
                tm.Position = m_CurrentPos;
                //this.m_Current += la.val;

                tm.Value = this.m_Current;
                this.m_CodeInfo.ConstDeclarations.Add(tm);
            
            
            
            

        }

        void VarDecl()
        {
            Expect(9);
            this.m_Current = la.val.ToLower() + " ";
            TypeDecl();
            if (la.kind == 25)
            {
                Get();
                if (StartOf(5))
                {
                    Expression();
                }
                else if (la.kind == 26)
                {
                    Get();
                    Expression();
                    while (la.kind == 27)
                    {
                        Get();
                        Expression();
                    }
                    Expect(28);
                }
                else SynErr(75);
            }
            Expect(29);

            TokenMatch tm = new TokenMatch();
            tm.Position = la.pos;
            //this.m_Current += la.val;

            tm.Value = this.m_Current;
            this.m_CodeInfo.VarDeclarations.Add(tm);
        }

        void TypeDecl()
        {
            type();
            this.m_Current += la.val;
            Expect(1);
            while (la.kind == 27)
            {
                Get();
                Expect(1);
            }
            if (la.kind == 30)
            {
                Get();
                if (la.kind == 2)
                {
                    Get();
                }
                else if (la.kind == 1)
                {
                    Get();
                }
                else SynErr(76);
                Expect(31);
            }
        }

        void Expression()
        {
            AndExpr();
            while (la.kind == 44)
            {
                Get();
                AndExpr();
            }

        }

        void type()
        {
            switch (la.kind)
            {
                case 5:
                    {
                        Get();
                        break;
                    }
                case 6:
                    {
                        Get();
                        break;
                    }
                case 7:
                    {
                        Get();
                        break;
                    }
                case 17:
                    {
                        Get();
                        break;
                    }
                case 16:
                    {
                        Get();
                        break;
                    }
                case 18:
                    {
                        Get();
                        break;
                    }
                case 20:
                    {
                        Get();
                        break;
                    }
                case 22:
                    {
                        Get();
                        break;
                    }
                case 19:
                    {
                        Get();
                        break;
                    }
                case 21:
                    {
                        Get();
                        break;
                    }
                case 23:
                    {
                        Get();
                        break;
                    }
                default: SynErr(77); break;
            }
        }

        void classname()
        {
            switch (la.kind)
            {
                case 17:
                    {
                        Get();
                        break;
                    }
                case 16:
                    {
                        Get();
                        break;
                    }
                case 18:
                    {
                        Get();
                        break;
                    }
                case 20:
                    {
                        Get();
                        break;
                    }
                case 22:
                    {
                        Get();
                        break;
                    }
                case 19:
                    {
                        Get();
                        break;
                    }
                case 21:
                    {
                        Get();
                        break;
                    }
                default: SynErr(78); break;
            }
        }

        void Body()
        {
            if (la.kind == 9 || la.kind == 10)
            {
                Declaration();
            }
            else if (StartOf(6))
            {
                Statement();
            }
            else SynErr(79);
        }

        void Parameter()
        {
            this.m_Current += la.val;
            Expect(9);
            this.m_Current += " "+la.val;
            type();
            this.m_Current += " "+la.val;
            Expect(1);
            this.m_Current += la.val+" ";
        }

        void Statement()
        {
            if (la.kind == 12)
            {
                IfStatement();
                while (la.kind == 12)
                {
                    IfStatement();
                }
                Expect(29);
            }
            else if (StartOf(5))
            {
                Expression();
                if (StartOf(7))
                {
                    Assign();
                    Expression();
                }
                Expect(29);
            }
            else if (la.kind == 15)
            {
                Get();
                if (StartOf(5))
                {
                    Expression();
                }
                Expect(29);
            }
            else SynErr(80);
        }

        void IfStatement()
        {
            Expect(12);
            Expression();
            Expect(26);
            while (StartOf(3))
            {
                Body();
            }
            Expect(28);
            while (la.kind == 13)
            {
                Get();
                if (la.kind == 12)
                {
                    Get();
                    Expression();
                    Expect(26);
                    while (StartOf(3))
                    {
                        Body();
                    }
                    Expect(28);
                }
                else if (la.kind == 26)
                {
                    Get();
                    while (StartOf(3))
                    {
                        Body();
                    }
                    Expect(28);
                }
                else SynErr(81);
            }
        }

        void Assign()
        {
            switch (la.kind)
            {
                case 25:
                    {
                        Get();
                        break;
                    }
                case 34:
                    {
                        Get();
                        break;
                    }
                case 35:
                    {
                        Get();
                        break;
                    }
                case 36:
                    {
                        Get();
                        break;
                    }
                case 37:
                    {
                        Get();
                        break;
                    }
                case 38:
                    {
                        Get();
                        break;
                    }
                case 39:
                    {
                        Get();
                        break;
                    }
                case 40:
                    {
                        Get();
                        break;
                    }
                case 41:
                    {
                        Get();
                        break;
                    }
                case 42:
                    {
                        Get();
                        break;
                    }
                case 43:
                    {
                        Get();
                        break;
                    }
                default: SynErr(82); break;
            }
        }

        void AndExpr()
        {
            BitOrExpr();
            while (la.kind == 45)
            {
                Get();
                BitOrExpr();
            }
        }

        void BitOrExpr()
        {
            BitXorExpr();
            while (la.kind == 46)
            {
                Get();
                BitXorExpr();
            }
        }

        void BitXorExpr()
        {
            BitAndExpr();
            while (la.kind == 47)
            {
                Get();
                BitAndExpr();
            }
        }

        void BitAndExpr()
        {
            CondExpr();
            while (la.kind == 48)
            {
                Get();
                CondExpr();
            }
        }

        void CondExpr()
        {
            ShiftExpr();
            while (StartOf(8))
            {
                switch (la.kind)
                {
                    case 49:
                        {
                            Get();
                            break;
                        }
                    case 50:
                        {
                            Get();
                            break;
                        }
                    case 51:
                        {
                            Get();
                            break;
                        }
                    case 52:
                        {
                            Get();
                            break;
                        }
                    case 53:
                        {
                            Get();
                            break;
                        }
                    case 54:
                        {
                            Get();
                            break;
                        }
                }
                ShiftExpr();
            }
        }

        void ShiftExpr()
        {
            AddExpr();
            while (la.kind == 55 || la.kind == 56)
            {
                if (la.kind == 55)
                {
                    Get();
                }
                else
                {
                    Get();
                }
                AddExpr();
            }
        }

        void AddExpr()
        {
            MulExpr();
            while (la.kind == 57 || la.kind == 58)
            {
                if (la.kind == 57)
                {
                    Get();
                }
                else
                {
                    Get();
                }
                MulExpr();
            }
        }

        void MulExpr()
        {
            CastExpr();
            while (la.kind == 59 || la.kind == 60 || la.kind == 61)
            {
                if (la.kind == 59)
                {
                    Get();
                }
                else if (la.kind == 60)
                {
                    Get();
                }
                else
                {
                    Get();
                }
                CastExpr();
            }
        }

        void CastExpr()
        {
            UnaryExp();
        }

        void UnaryExp()
        {
            if (StartOf(9))
            {
                PostFixExp();
            }
            else if (la.kind == 62 || la.kind == 63)
            {
                if (la.kind == 62)
                {
                    Get();
                }
                else
                {
                    Get();
                }
                UnaryExp();
            }
            else if (StartOf(10))
            {
                if (la.kind == 64)
                {
                    Get();
                }
                else if (la.kind == 65)
                {
                    Get();
                }
                else if (la.kind == 58)
                {
                    Get();
                }
                else
                {
                    Get();
                }
                CastExpr();
            }
            else SynErr(83);
        }

        void PostFixExp()
        {
            Primary();
            while (StartOf(11))
            {
                if (la.kind == 30)
                {
                    Get();
                    Expression();
                    Expect(31);
                }
                else if (la.kind == 32)
                {
                    Get();
                    if (StartOf(5))
                    {
                        ActualParameters();
                    }
                    Expect(33);
                }
                else if (la.kind == 66)
                {
                    Get();
                    Expect(1);
                }
                else if (la.kind == 62)
                {
                    Get();
                }
                else
                {
                    Get();
                }
            }
        }

        void Primary()
        {
            if (la.kind == 1)
            {
                Get();
            }
            else if (la.kind == 2)
            {
                Get();
            }
            else if (la.kind == 4)
            {
                Get();
            }
            else if (la.kind == 3)
            {
                Get();
            }
            else if (la.kind == 32)
            {
                Get();
                Expression();
                Expect(33);
            }
            else SynErr(84);
        }

        void ActualParameters()
        {
            Expression();
            while (la.kind == 27)
            {
                Get();
                Expression();
            }
        }



        public void Parse()
        {
            la = new Token();
            la.val = "";
            Get();
            D();

            Expect(0);
        }

        static readonly bool[,] set = {
		{T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x},
		{x,x,x,x, x,x,x,x, x,T,T,T, x,x,T,x, x,x,x,x, x,x,x,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x},
		{x,x,x,x, x,T,T,T, x,x,x,x, x,x,x,x, T,T,T,T, T,T,T,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x},
		{x,T,T,T, T,x,x,x, x,T,T,x, T,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,T,x, x,x,T,T, T,T,x,x, x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,T,T,T, T,T,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x},
		{x,T,T,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,T,x, x,x,T,T, T,T,x,x, x},
		{x,T,T,T, T,x,x,x, x,x,x,x, T,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,T,x, x,x,T,T, T,T,x,x, x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,x,x, x,x,T,T, T,T,T,T, T,T,T,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,T,T, T,T,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x},
		{x,T,T,T, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,T,x, x,x,x,x, T,T,x,x, x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,T, x,x,T,x, x}

	};
    } // end Parser


    public class Errors
    {
        public int count = 0;                                    // number of errors detected
        public System.IO.TextWriter errorStream = Console.Out;   // error messages go to this stream
        public string errMsgFormat = "Zeile {0}, Spalte {1}: {2}"; // 0=line, 1=column, 2=text

        public void SynErr(int line, int col, int n)
        {
            string s;
            switch (n)
            {
                case 0: s = "Dateiende erwartet"; break;
                case 1: s = "Bezeichner erwartet"; break;
                case 2: s = "Integer-Wert erwartet"; break;
                case 3: s = "Float-Wert erwartet"; break;
                case 4: s = "String-Wert erwartet"; break;
                case 5: s = "int erwartet"; break;
                case 6: s = "string erwartet"; break;
                case 7: s = "float erwartet"; break;
                case 8: s = "void erwartet"; break;
                case 9: s = "var erwartet"; break;
                case 10: s = "const erwartet"; break;
                case 11: s = "instance erwartet"; break;
                case 12: s = "if erwartet"; break;
                case 13: s = "else erwartet"; break;
                case 14: s = "prototype erwartet"; break;
                case 15: s = "return erwartet"; break;
                case 16: s = "c_item erwartet"; break;
                case 17: s = "c_npc erwartet"; break;
                case 18: s = "c_mission erwartet"; break;
                case 19: s = "c_focus erwartet"; break;
                case 20: s = "c_info erwartet"; break;
                case 21: s = "c_itemreact erwartet"; break;
                case 22: s = "c_spell erwartet"; break;
                case 23: s = "func erwartet"; break;
                case 24: s = "class erwartet"; break;
                case 25: s = "\"=\" erwartet"; break;
                case 26: s = "\"{\" erwartet"; break;
                case 27: s = "\",\" erwartet"; break;
                case 28: s = "\"}\" erwartet"; break;
                case 29: s = "\";\" erwartet"; break;
                case 30: s = "\"[\" erwartet"; break;
                case 31: s = "\"]\" erwartet"; break;
                case 32: s = "\"(\" erwartet"; break;
                case 33: s = "\")\" erwartet"; break;
                case 34: s = "\"*=\" erwartet"; break;
                case 35: s = "\"/=\" erwartet"; break;
                case 36: s = "\"%=\" erwartet"; break;
                case 37: s = "\"+=\" erwartet"; break;
                case 38: s = "\"-=\" erwartet"; break;
                case 39: s = "\"&=\" erwartet"; break;
                case 40: s = "\"^=\" erwartet"; break;
                case 41: s = "\"|=\" erwartet"; break;
                case 42: s = "\"<<=\" erwartet"; break;
                case 43: s = "\">>=\" erwartet"; break;
                case 44: s = "\"||\" erwartet"; break;
                case 45: s = "\"&&\" erwartet"; break;
                case 46: s = "\"|\" erwartet"; break;
                case 47: s = "\"^\" erwartet"; break;
                case 48: s = "\"&\" erwartet"; break;
                case 49: s = "\"!=\" erwartet"; break;
                case 50: s = "\"==\" erwartet"; break;
                case 51: s = "\"<=\" erwartet"; break;
                case 52: s = "\"<\" erwartet"; break;
                case 53: s = "\">=\" erwartet"; break;
                case 54: s = "\">\" erwartet"; break;
                case 55: s = "\"<<\" erwartet"; break;
                case 56: s = "\">>\" erwartet"; break;
                case 57: s = "\"+\" erwartet"; break;
                case 58: s = "\"-\" erwartet"; break;
                case 59: s = "\"*\" erwartet"; break;
                case 60: s = "\"/\" erwartet"; break;
                case 61: s = "\"%\" erwartet"; break;
                case 62: s = "\"++\" erwartet"; break;
                case 63: s = "\"--\" erwartet"; break;
                case 64: s = "\"~\" erwartet"; break;
                case 65: s = "\"!\" erwartet"; break;
                case 66: s = "\".\" erwartet"; break;
                case 67: s = "??? erwartet"; break;
                case 68: s = "Ungültiger Block"; break;
                case 69: s = "Ungültige Deklaration"; break;
                case 70: s = "Ungültige Funktion"; break;
                case 71: s = "Ungültige Instance"; break;
                case 72: s = "Ungültiger Prototyp"; break;
                case 73: s = "Ungültige Klasse"; break;
                case 74: s = "Ungültige Konstantendeklaration"; break;
                case 75: s = "Ungültige Variablendeklaration"; break;
                case 76: s = "Ungültige Typendeklaration"; break;
                case 77: s = "Ungültiger Typ"; break;
                case 78: s = "Ungültige Klasse"; break;
                case 79: s = "Ungültiger Block"; break;
                case 80: s = "Ungültiger Ausdruck"; break;
                case 81: s = "Ungültige Abfrage"; break;
                case 82: s = "Ungültige Zuweisung"; break;
                case 83: s = "Ungültiger Operator"; break;
                case 84: s = "Ungültiger Ausdruck"; break;
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