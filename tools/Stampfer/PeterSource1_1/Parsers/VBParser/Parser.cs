using System;
using System.Collections.Generic;
using System.Text;
using Peter.CSParser;

namespace Peter.VBParser
{
    public class Parser
    {
        const int _EOF = 0;
        const int _number = 1;
        const int _quotedString = 2;
        const int _identifier = 3;
        const int maxT = 89;

        const bool T = true;
        const bool x = false;
        const int minErrDist = 2;

        public Scanner scanner;
        public Errors errors;

        public Token t;    // last recognized token
        public Token la;   // lookahead token
        int errDist = minErrDist;
        private VBCodeInfo m_CodeInfo = new VBCodeInfo();

        //public Util util = new Util();


        public VBCodeInfo CodeInfo
        {
            get { return this.m_CodeInfo; }
        }

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


        void VBNET()
        {
            string modifier = "";
            while (la.kind == 5)
            {
                OptionStmt();
            }
            while (la.kind == 4)
            {
                ImportStmt();
            }
            if (la.kind == 39)
            {
                AttributeList();
            }
            if (StartOf(1))
            {
                ModifierGroup(out modifier);
            }
            TypeDeclaration(modifier);
        }

        void OptionStmt()
        {
            Expect(5);
            if (la.kind == 6)
            {
                Get();
                if (la.kind == 7)
                {
                    Get();
                }
                else if (la.kind == 8)
                {
                    Get();
                }
                else SynErr(90);
            }
            else if (la.kind == 9)
            {
                Get();
                if (la.kind == 7)
                {
                    Get();
                }
                else if (la.kind == 8)
                {
                    Get();
                }
                else SynErr(91);
            }
            else if (la.kind == 10)
            {
                Get();
                if (la.kind == 11)
                {
                    Get();
                }
                else if (la.kind == 12)
                {
                    Get();
                }
                else SynErr(92);
            }
            else SynErr(93);
        }

        void ImportStmt()
        {
            string nmspc;
            Expect(4);
            this.m_CodeInfo.Imports.Add(new TokenMatch(la.val, la.pos));
            QualName(out nmspc);
            //util.Writeline("using " + nmspc + ";");
        }

        void AttributeList()
        {
            string attlist;
            string attspec;

            Expect(39);
            AttribSpec(out attlist);
            while (la.kind == 15)
            {
                Get();
                attlist += la.val;
                AttribSpec(out attspec);
                attlist += attspec;
            }
            Expect(40);
            //util.Writeline("[" + attlist + "]");

        }

        void ModifierGroup(out string modgroup)
        {
            string modifier;
            modgroup = "";

            Modifier(out modifier);
            modgroup += modifier;
            while (StartOf(1))
            {
                Modifier(out modifier);
                modgroup += modifier;
            }
        }

        void TypeDeclaration(string modifier)
        {
            string bases = "";
            string decl = modifier;
            string typename;

            if (la.kind == 16)
            {
                Get();
                decl += "class ";
            }
            else if (la.kind == 17)
            {
                Get();
                decl += "struct ";
            }
            else SynErr(94);
            QualName(out typename);
            decl += typename;
            if (la.kind == 13)
            {
                InheritanceDecl(out bases);
            }
            if (la.kind == 14)
            {
                ImplementsDecl(ref bases);
            }
            decl += bases;
            //util.Writeline(decl);
            //util.OpenBlock();

            while (StartOf(2))
            {
                MemberDeclaration();
            }
            Expect(18);
            if (la.kind == 16)
            {
                Get();
            }
            else if (la.kind == 19)
            {
                Get();
            }
            else SynErr(95);
            //util.CloseBlock();
        }

        void Modifier(out string modifier)
        {
            switch (la.kind)
            {
                case 46:
                    {
                        Get();
                        break;
                    }
                case 47:
                    {
                        Get();
                        break;
                    }
                case 48:
                    {
                        Get();
                        break;
                    }
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
                default: SynErr(96); break;
            }
            modifier = la.val;// util.ConvertModifier(la.val);

        }

        void QualName(out string typename)
        {
            Expect(3);
            typename = la.val;// util.ConverteTipo(la.val);
        }

        void InheritanceDecl(out string decl)
        {
            string typename;
            Expect(13);
            QualName(out typename);
            decl = " : " + typename;
        }

        void ImplementsDecl(ref string decl)
        {
            string typename;
            Expect(14);
            QualName(out typename);
            if (decl == "")
                decl = " : " + typename;
            else
                decl += ", " + typename;

            while (la.kind == 15)
            {
                Get();
                QualName(out typename);
                decl += ", " + typename;

            }
        }

        void MemberDeclaration()
        {
            string modifier = "";

            if (la.kind == 39)
            {
                AttributeList();
            }
            if (StartOf(1))
            {
                ModifierGroup(out modifier);
            }
            switch (la.kind)
            {
                case 52:
                    {
                        InterfaceDeclaration(modifier);
                        break;
                    }
                case 16:
                case 17:
                    {
                        TypeDeclaration(modifier);
                        break;
                    }
                case 53:
                    {
                        EnumDeclaration(modifier);
                        break;
                    }
                case 60:
                    {
                        SubDeclaration(modifier);
                        break;
                    }
                case 61:
                    {
                        FunctionDeclaration(modifier);
                        break;
                    }
                case 57:
                    {
                        ConstDeclaration(modifier);
                        break;
                    }
                case 3:
                    {
                        FieldDeclaration(modifier);
                        break;
                    }
                case 54:
                    {
                        BeginRegion();
                        break;
                    }
                case 55:
                    {
                        EndRegion();
                        break;
                    }
                default: SynErr(97); break;
            }
        }

        void ArithOp(out string op)
        {
            op = "";
            switch (la.kind)
            {
                case 20:
                case 21:
                case 22:
                case 23:
                    {
                        if (la.kind == 20)
                        {
                            Get();
                        }
                        else if (la.kind == 21)
                        {
                            Get();
                        }
                        else if (la.kind == 22)
                        {
                            Get();
                        }
                        else
                        {
                            Get();
                        }
                        op = la.val;
                        break;
                    }
                case 24:
                    {
                        Get();
                        op = "/";
                        break;
                    }
                case 25:
                    {
                        Get();
                        op = "%";
                        break;
                    }
                case 26:
                    {
                        Get();
                        op = "+";
                        break;
                    }
                case 27:
                    {
                        Get();
                        op = "!";
                        break;
                    }
                case 44:
                case 45:
                    {
                        BoolOp(out op);
                        break;
                    }
                case 38:
                case 39:
                case 40:
                case 41:
                case 42:
                case 43:
                    {
                        LogicalOp(out op);
                        break;
                    }
                default: SynErr(98); break;
            }
        }

        void BoolOp(out string op)
        {
            op = "";
            if (la.kind == 44)
            {
                Get();
                op = " && ";
            }
            else if (la.kind == 45)
            {
                Get();
                op = " || ";
            }
            else SynErr(99);
        }

        void LogicalOp(out string op)
        {
            switch (la.kind)
            {
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
                default: SynErr(100); break;
            }
            op = la.val;//util.ConverteLogicalOp(la.val);
        }

        void ArithExpr(out string exp)
        {
            string op, exp2;
            ArithOp(out op);
            Expression(out exp2);
            exp = op + " " + exp2;
        }

        void Expression(out string exp)
        {
            string parms = "";
            string typename;
            string exp2 = "";
            exp = "";

            switch (la.kind)
            {
                case 32:
                    {
                        Get();
                        exp = "true";
                        break;
                    }
                case 33:
                    {
                        Get();
                        exp = "false";
                        break;
                    }
                case 34:
                    {
                        Get();
                        exp = "null";
                        break;
                    }
                case 35:
                    {
                        Get();
                        exp = "this";
                        break;
                    }
                case 28:
                    {
                        TypeOfExp(out exp);
                        break;
                    }
                case 1:
                    {
                        NumericConstant(out exp);
                        break;
                    }
                case 2:
                    {
                        StringConstant(out exp);
                        break;
                    }
                case 30:
                    {
                        ParentExp(out exp);
                        break;
                    }
                case 3:
                    {
                        QualName(out exp);
                        if (la.kind == 29 || la.kind == 30)
                        {
                            if (la.kind == 29)
                            {
                                Get();
                                Expect(34);
                                exp += " == null";
                            }
                            else
                            {
                                CallParams(out parms);
                                exp += parms;
                                if (la.kind == 36)
                                {
                                    Get();
                                    Expression(out exp2);
                                    exp += "." + exp2;
                                }
                            }
                        }
                        break;
                    }
                case 37:
                    {
                        Get();
                        QualName(out typename);
                        if (la.kind == 30)
                        {
                            CallParams(out parms);
                        }
                        exp = "new " + typename + parms;
                        break;
                    }
                default: SynErr(101); break;
            }
            if (StartOf(3))
            {
                ArithExpr(out exp2);
                exp += exp2;
            }
        }

        void TypeOfExp(out string exp)
        {
            string exp2, typespec;
            Expect(28);
            QualName(out exp2);
            Expect(29);
            QualName(out typespec);
            exp = exp2 + " is " + typespec;
        }

        void ParentExp(out string exp)
        {
            string exp2;
            Expect(30);
            Expression(out exp2);
            Expect(31);
            exp = "(" + exp2 + ")";
        }

        void NumericConstant(out string exp)
        {
            Expect(1);
            exp = la.val;
        }

        void StringConstant(out string exp)
        {
            Expect(2);
            exp = la.val;//util.ConverteString(la.val);
        }

        void CallParams(out string parms)
        {
            string expList;
            Expect(30);
            parms = la.val;
            if (StartOf(4))
            {
                ExpressionList(out expList);
                parms += expList;
            }
            Expect(31);
            parms += la.val;
        }

        void ExpressionList(out string expList)
        {
            string exp;
            Expression(out exp);
            expList = exp;
            while (la.kind == 15)
            {
                Get();
                expList += la.val + " ";
                Expression(out exp);
                expList += exp;
            }
        }

        void AttribSpec(out string attspec)
        {
            string pl;
            string typename;

            QualName(out typename);
            attspec = typename;
            if (la.kind == 30)
            {
                CallParams(out pl);
                attspec += pl;
            }
        }

        void InterfaceDeclaration(string modifier)
        {
            Expect(52);
            Expect(3);
            //util.Writeline(modifier + "interface " + la.val);
            //util.OpenBlock();

            while (la.kind == 60 || la.kind == 61)
            {
                if (la.kind == 60)
                {
                    SubHeader("");
                }
                else
                {
                    FunctionHeader("");
                }
            }
            Expect(18);
            Expect(52);
            //util.CloseBlock();
        }

        void EnumDeclaration(string modifier)
        {
            string constName;
            string constValue;

            Expect(53);
            Expect(3);
            //util.Writeline(modifier + " enum " + la.val);
            //util.OpenBlock();

            while (la.kind == 3)
            {
                Get();
                constName = la.val; constValue = null;
                if (la.kind == 38)
                {
                    Get();
                    Expression(out constValue);
                }
                //if (constValue == null)
                    //util.Writeline(constName);
                //else
                    //util.Writeline(constName + " = " + constValue);

            }
            Expect(18);
            Expect(53);
            //util.CloseBlock();
        }

        void SubDeclaration(string modifier)
        {
            SubHeader(modifier);
            //util.OpenBlock();
            if (StartOf(5))
            {
                RoutineBody();
            }
            Expect(18);
            Expect(60);
            //util.CloseBlock();
        }

        void FunctionDeclaration(string modifier)
        {
            FunctionHeader(modifier);
            //util.OpenBlock();
            if (StartOf(5))
            {
                RoutineBody();
            }
            Expect(18);
            Expect(61);
            //util.CloseBlock();
        }

        void ConstDeclaration(string modifier)
        {
            string decl;
            Expect(57);
            VarDeclaration(out decl);
            //util.Writeline(modifier + "const " + decl + ";");
        }

        void FieldDeclaration(string modifier)
        {
            string decl;
            VarDeclaration(out decl);
            //util.Writeline(modifier + decl + ";");
        }

        void BeginRegion()
        {
            Expect(54);
            Expect(2);
            //util.Writeline("#region " + la.val);
        }

        void EndRegion()
        {
            Expect(55);
            Expect(56);
            //util.Writeline("#endregion");
        }

        void SubHeader(string modifier)
        {
            string decl = modifier;
            string paramList;

            Expect(60);
            decl += "void ";
            Expect(3);
            decl += la.val;
            if (la.kind == 30)
            {
                ParameterList(out paramList);
                decl += paramList;
            }
            //if (modifier == "") // In interface declaration?
                //util.Writeline(decl + ";");
            //else
                //util.Writeline(decl);

            if (la.kind == 14)
            {
                SubImplementsInterface();
            }
            if (la.kind == 59)
            {
                Handles();
            }
        }

        void FunctionHeader(string modifier)
        {
            string funcName;
            string paramList = "";
            string returnType;

            Expect(61);
            Expect(3);
            funcName = la.val;
            if (la.kind == 30)
            {
                ParameterList(out paramList);
            }
            Expect(58);
            QualName(out returnType);
            //util.Writeline(modifier + returnType + " " + funcName + paramList);

            if (la.kind == 14)
            {
                SubImplementsInterface();
            }
            if (la.kind == 59)
            {
                Handles();
            }
        }

        void VarDeclaration(out string declaration)
        {
            string typename;
            string varname;
            string exp;
            string dimensions = null;
            bool bIsArray = false;
            string parms = "";
            declaration = "";

            QualName(out varname);
            if (la.kind == 30)
            {
                Get();
                if (StartOf(4))
                {
                    ExpressionList(out dimensions);
                }
                Expect(31);
                bIsArray = true;
            }
            Expect(58);
            if (la.kind == 37)
            {
                Get();
                QualName(out typename);
                if (la.kind == 30)
                {
                    CallParams(out parms);
                }
                declaration = typename + " " + varname + " = new " + typename + parms;

            }
            else if (la.kind == 3)
            {
                QualName(out typename);
                if (!bIsArray)
                    declaration = typename + " " + varname;
                else
                {
                    declaration = typename + "[] " + varname;
                    if (dimensions != null)
                        declaration += String.Format(" = new {0}[{1}]", typename, dimensions);
                }

            }
            else SynErr(102);
            if (la.kind == 38)
            {
                Get();
                Expression(out exp);
                declaration += " = " + exp;
            }
        }

        void SubImplementsInterface()
        {
            string lixo;

            Expect(14);
            QualName(out lixo);
        }

        void Handles()
        {
            string lixo;

            Expect(59);
            QualName(out lixo);
        }

        void ParameterList(out string paramList)
        {
            string decl;
            Expect(30);
            paramList = la.val;
            if (la.kind == 3 || la.kind == 87 || la.kind == 88)
            {
                ParameterDeclaration(out decl);
                paramList += decl;
                while (la.kind == 15)
                {
                    Get();
                    paramList += la.val + " ";
                    ParameterDeclaration(out decl);
                    paramList += decl;
                }
            }
            Expect(31);
            paramList += la.val;
        }

        void RoutineBody()
        {
            RoutineStmt();
            while (StartOf(5))
            {
                RoutineStmt();
            }
        }

        void RoutineStmt()
        {
            switch (la.kind)
            {
                case 3:
                    {
                        AssignCall();
                        break;
                    }
                case 62:
                    {
                        SyncLockStmt();
                        break;
                    }
                case 63:
                    {
                        ThrowStmt();
                        break;
                    }
                case 64:
                    {
                        DoWhileStmt();
                        break;
                    }
                case 68:
                    {
                        ExitStmt();
                        break;
                    }
                case 66:
                    {
                        WhileStmt();
                        break;
                    }
                case 75:
                    {
                        CallStmt();
                        break;
                    }
                case 80:
                    {
                        SelectCase();
                        break;
                    }
                case 69:
                    {
                        ForStmt();
                        break;
                    }
                case 85:
                    {
                        IfStmt();
                        break;
                    }
                case 77:
                    {
                        TryCatch();
                        break;
                    }
                case 76:
                    {
                        ReturnStmt();
                        break;
                    }
                case 57:
                case 86:
                    {
                        LocalDeclaration();
                        break;
                    }
                default: SynErr(103); break;
            }
        }

        void AssignCall()
        {
            string call;
            string operation = "";
            string rvalue = null;

            LeftAssign(out call);
            if (StartOf(6))
            {
                if (StartOf(7))
                {
                    if (la.kind == 20)
                    {
                        Get();
                    }
                    else if (la.kind == 21)
                    {
                        Get();
                    }
                    else if (la.kind == 22)
                    {
                        Get();
                    }
                    else if (la.kind == 23)
                    {
                        Get();
                    }
                    else
                    {
                        Get();
                        operation = la.val;
                    }
                }
                Expect(38);
                Expression(out rvalue);
            }
            //if (rvalue != null)
                //util.Writeline(call + " " + operation + "= " + rvalue + ";");
            //else
                //util.Writeline(call + ";");

        }

        void SyncLockStmt()
        {
            string exp;
            Expect(62);
            Expression(out exp);
            //util.Writeline("lock (" + exp + ")");
            //util.OpenBlock();

            if (StartOf(5))
            {
                RoutineBody();
            }
            Expect(18);
            Expect(62);
            //util.CloseBlock();
        }

        void ThrowStmt()
        {
            string exp;
            Expect(63);
            Expression(out exp);
            //util.Writeline("throw " + exp + ";");
        }

        void DoWhileStmt()
        {
            string exp;
            Expect(64);
            //util.Writeline("do");
            //util.OpenBlock();

            RoutineBody();
            Expect(65);
            if (la.kind == 66)
            {
                Get();
                Expression(out exp);
                //util.CloseBlock();
                //util.Writeline("while (" + exp + ")");

            }
            else if (la.kind == 67)
            {
                Get();
                Expression(out exp);
                //util.CloseBlock();
                //util.Writeline("while (!(" + exp + "))");

            }
            else SynErr(104);
        }

        void ExitStmt()
        {
            Expect(68);
            if (la.kind == 69)
            {
                Get();
            }
            else if (la.kind == 66)
            {
                Get();
            }
            else if (la.kind == 64)
            {
                Get();
            }
            else SynErr(105);
            //util.Writeline("break;");
        }

        void WhileStmt()
        {
            string exp;
            Expect(66);
            Expression(out exp);
            //util.Writeline("while (" + exp + ")");
            //util.OpenBlock();

            if (StartOf(5))
            {
                RoutineBody();
            }
            Expect(18);
            Expect(66);
            //util.CloseBlock();
        }

        void CallStmt()
        {
            string funcname;
            string parms = "";

            Expect(75);
            QualName(out funcname);
            if (la.kind == 30)
            {
                CallParams(out parms);
            }
            //util.Writeline(funcname + parms + ";");
        }

        void SelectCase()
        {
            string exp;
            Expect(80);
            Expect(81);
            Expression(out exp);
            //util.Writeline("switch (" + exp + ")");
            //util.OpenBlock();

            CaseStmts();
            Expect(18);
            Expect(80);
            //util.CloseBlock();
        }

        void ForStmt()
        {
            Expect(69);
            if (la.kind == 3)
            {
                ForVar();
            }
            else if (la.kind == 73)
            {
                ForEach();
            }
            else SynErr(106);
        }

        void IfStmt()
        {
            string exp;
            Expect(85);
            Expression(out exp);
            Expect(84);
            //util.Writeline("if (" + exp + ")");
            //util.OpenBlock();

            RoutineBody();
            if (la.kind == 82 || la.kind == 83)
            {
                ElseStmt();
            }
            Expect(18);
            Expect(85);
            //util.CloseBlock();

        }

        void TryCatch()
        {
            string varname = "Exception";
            Expect(77);
            //util.Writeline("try");
            //util.OpenBlock();

            RoutineBody();
            if (la.kind == 78)
            {
                Get();
                if (la.kind == 3)
                {
                    VarDeclaration(out varname);
                }
                //util.CloseBlock();
                //util.Writeline("catch (" + varname + ")");
                //util.OpenBlock();

                if (StartOf(5))
                {
                    RoutineBody();
                }
            }
            if (la.kind == 79)
            {
                Get();
                //util.CloseBlock();
                //util.Writeline("finally");
                //util.OpenBlock();

                RoutineBody();
            }
            Expect(18);
            Expect(77);
            //util.CloseBlock();

        }

        void ReturnStmt()
        {
            string exp;
            Expect(76);
            Expression(out exp);
            //util.Writeline("return " + exp + ";");
        }

        void LocalDeclaration()
        {
            string decl;
            //string constant = "";

            if (la.kind == 86)
            {
                Get();
            }
            else if (la.kind == 57)
            {
                Get();
                //constant = "const ";
            }
            else SynErr(107);
            VarDeclaration(out decl);
            //util.Writeline(constant + decl + ";");
            while (la.kind == 15)
            {
                Get();
                VarDeclaration(out decl);
                //util.Writeline(constant + decl + ";");
            }
        }

        void ForVar()
        {
            string loopvar;
            string expIni;
            string expEnd;
            string stepExp;
            string stepCS;

            QualName(out loopvar);
            stepCS = loopvar + "++";
            Expect(38);
            Expression(out expIni);
            Expect(70);
            Expression(out expEnd);
            if (la.kind == 71)
            {
                Get();
                Expression(out stepExp);
                stepCS = loopvar + " += " + stepExp;
            }
            //util.Writeline(string.Format("for ({0}={1}; {0} <= {2}; {3})", loopvar, expIni, expEnd, stepCS));
            //util.OpenBlock();

            RoutineBody();
            Expect(72);
            if (la.kind == 3)
            {
                Get();
            }
            //util.CloseBlock();

        }

        void ForEach()
        {
            string loopvar;
            string expIni;

            Expect(73);
            QualName(out loopvar);
            Expect(74);
            Expression(out expIni);
            //util.Writeline(string.Format("foreach ({0} in {1})", loopvar, expIni));
            //util.OpenBlock();

            RoutineBody();
            Expect(72);
            if (la.kind == 3)
            {
                Get();
            }
            //util.CloseBlock();

        }

        void CaseStmts()
        {
            CaseStmt();
            while (la.kind == 81)
            {
                CaseStmt();
            }
        }

        void CaseStmt()
        {
            string exp;
            Expect(81);
            if (la.kind == 82)
            {
                Get();
                //util.Writeline("default:");
                //util.Ident();

            }
            else if (StartOf(4))
            {
                Expression(out exp);
                //util.Writeline("case " + exp + ":");
                //util.Ident();

                while (la.kind == 15)
                {
                    Get();
                    Expression(out exp);
                    //util.Unident();
                    //util.Writeline("case " + exp + ":");
                    //util.Ident();

                }
            }
            else SynErr(108);
            RoutineBody();
            //util.Unident();

        }

        void ElseStmt()
        {
            string exp;
            if (la.kind == 82)
            {
                Get();
                //util.CloseBlock();
                //util.Writeline("else");
                //util.OpenBlock();

                RoutineBody();
            }
            else if (la.kind == 83)
            {
                Get();
                Expression(out exp);
                Expect(84);
                //util.CloseBlock();
                //util.Writeline("else if (" + exp + ")");
                //util.OpenBlock();

                RoutineBody();
                if (la.kind == 82 || la.kind == 83)
                {
                    ElseStmt();
                }
            }
            else SynErr(109);
        }

        void LeftAssign(out string call)
        {
            string typename, pl, call2;
            QualName(out typename);
            call = typename;
            if (la.kind == 30)
            {
                CallParams(out pl);
                call += pl;
                if (la.kind == 36)
                {
                    Get();
                    LeftAssign(out call2);
                    call += "." + call2;
                }
            }
        }

        void ParameterDeclaration(out string declaration)
        {
            string decl;
            if (la.kind == 87 || la.kind == 88)
            {
                if (la.kind == 87)
                {
                    Get();
                }
                else
                {
                    Get();
                }
            }
            declaration = la.val;//util.ConvertByX(la.val);
            VarDeclaration(out decl);
            declaration += decl;
        }



        public void Parse()
        {
            la = new Token();
            la.val = "";
            Get();
            VBNET();

            Expect(0);
        }

        bool[,] set = {
		{T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,T, T,T,T,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x, T,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,T, x,x,x,x, x,x,T,T, T,T,T,T, T,T,T,T, x,T,x,x, T,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,T,T,T, T,T,T,T, x,x,x,x, x,x,x,x, x,x,T,T, T,T,T,T, T,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,T,T,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,x,T,x, T,T,T,T, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,x,T,T, T,x,T,x, T,T,x,x, x,x,x,T, T,T,x,x, T,x,x,x, x,T,T,x, x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,T,T,T, x,x,T,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x},
		{x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,T,T,T, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x}

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
                case 1: s = "number expected"; break;
                case 2: s = "quotedString expected"; break;
                case 3: s = "identifier expected"; break;
                case 4: s = "\"Imports\" expected"; break;
                case 5: s = "\"Option\" expected"; break;
                case 6: s = "\"Explicit\" expected"; break;
                case 7: s = "\"On\" expected"; break;
                case 8: s = "\"Off\" expected"; break;
                case 9: s = "\"Strict\" expected"; break;
                case 10: s = "\"Compare\" expected"; break;
                case 11: s = "\"Text\" expected"; break;
                case 12: s = "\"Binary\" expected"; break;
                case 13: s = "\"Inherits\" expected"; break;
                case 14: s = "\"Implements\" expected"; break;
                case 15: s = "\",\" expected"; break;
                case 16: s = "\"Class\" expected"; break;
                case 17: s = "\"Structure\" expected"; break;
                case 18: s = "\"End\" expected"; break;
                case 19: s = "\"Module\" expected"; break;
                case 20: s = "\"+\" expected"; break;
                case 21: s = "\"-\" expected"; break;
                case 22: s = "\"*\" expected"; break;
                case 23: s = "\"/\" expected"; break;
                case 24: s = "\"\\\\\" expected"; break;
                case 25: s = "\"Mod\" expected"; break;
                case 26: s = "\"&\" expected"; break;
                case 27: s = "\"Not\" expected"; break;
                case 28: s = "\"TypeOf\" expected"; break;
                case 29: s = "\"Is\" expected"; break;
                case 30: s = "\"(\" expected"; break;
                case 31: s = "\")\" expected"; break;
                case 32: s = "\"True\" expected"; break;
                case 33: s = "\"False\" expected"; break;
                case 34: s = "\"Nothing\" expected"; break;
                case 35: s = "\"Me\" expected"; break;
                case 36: s = "\".\" expected"; break;
                case 37: s = "\"New\" expected"; break;
                case 38: s = "\"=\" expected"; break;
                case 39: s = "\"<\" expected"; break;
                case 40: s = "\">\" expected"; break;
                case 41: s = "\">=\" expected"; break;
                case 42: s = "\"<=\" expected"; break;
                case 43: s = "\"<>\" expected"; break;
                case 44: s = "\"And\" expected"; break;
                case 45: s = "\"Or\" expected"; break;
                case 46: s = "\"Public\" expected"; break;
                case 47: s = "\"Private\" expected"; break;
                case 48: s = "\"Protected\" expected"; break;
                case 49: s = "\"Overrides\" expected"; break;
                case 50: s = "\"Shared\" expected"; break;
                case 51: s = "\"Friend\" expected"; break;
                case 52: s = "\"Interface\" expected"; break;
                case 53: s = "\"Enum\" expected"; break;
                case 54: s = "\"#Region\" expected"; break;
                case 55: s = "\"#End\" expected"; break;
                case 56: s = "\"Region\" expected"; break;
                case 57: s = "\"Const\" expected"; break;
                case 58: s = "\"As\" expected"; break;
                case 59: s = "\"Handles\" expected"; break;
                case 60: s = "\"Sub\" expected"; break;
                case 61: s = "\"Function\" expected"; break;
                case 62: s = "\"SyncLock\" expected"; break;
                case 63: s = "\"Throw\" expected"; break;
                case 64: s = "\"Do\" expected"; break;
                case 65: s = "\"Loop\" expected"; break;
                case 66: s = "\"While\" expected"; break;
                case 67: s = "\"Until\" expected"; break;
                case 68: s = "\"Exit\" expected"; break;
                case 69: s = "\"For\" expected"; break;
                case 70: s = "\"To\" expected"; break;
                case 71: s = "\"Step\" expected"; break;
                case 72: s = "\"Next\" expected"; break;
                case 73: s = "\"Each\" expected"; break;
                case 74: s = "\"In\" expected"; break;
                case 75: s = "\"Call\" expected"; break;
                case 76: s = "\"Return\" expected"; break;
                case 77: s = "\"Try\" expected"; break;
                case 78: s = "\"Catch\" expected"; break;
                case 79: s = "\"Finally\" expected"; break;
                case 80: s = "\"Select\" expected"; break;
                case 81: s = "\"Case\" expected"; break;
                case 82: s = "\"Else\" expected"; break;
                case 83: s = "\"ElseIf\" expected"; break;
                case 84: s = "\"Then\" expected"; break;
                case 85: s = "\"If\" expected"; break;
                case 86: s = "\"Dim\" expected"; break;
                case 87: s = "\"ByVal\" expected"; break;
                case 88: s = "\"ByRef\" expected"; break;
                case 89: s = "??? expected"; break;
                case 90: s = "invalid OptionStmt"; break;
                case 91: s = "invalid OptionStmt"; break;
                case 92: s = "invalid OptionStmt"; break;
                case 93: s = "invalid OptionStmt"; break;
                case 94: s = "invalid TypeDeclaration"; break;
                case 95: s = "invalid TypeDeclaration"; break;
                case 96: s = "invalid Modifier"; break;
                case 97: s = "invalid MemberDeclaration"; break;
                case 98: s = "invalid ArithOp"; break;
                case 99: s = "invalid BoolOp"; break;
                case 100: s = "invalid LogicalOp"; break;
                case 101: s = "invalid Expression"; break;
                case 102: s = "invalid VarDeclaration"; break;
                case 103: s = "invalid RoutineStmt"; break;
                case 104: s = "invalid DoWhileStmt"; break;
                case 105: s = "invalid ExitStmt"; break;
                case 106: s = "invalid ForStmt"; break;
                case 107: s = "invalid LocalDeclaration"; break;
                case 108: s = "invalid CaseStmt"; break;
                case 109: s = "invalid ElseStmt"; break;

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
