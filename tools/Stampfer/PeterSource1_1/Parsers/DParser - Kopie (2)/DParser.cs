using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace Peter.DParser
{
    public class DParser
    {
        public static void ParseToTree (MainForm MainF,string fileName, TreeNodeCollection nodes)
        {
            Peter.DParser.Scanner scanner = new Peter.DParser.Scanner(fileName);
            Peter.DParser.Parser parser = new Peter.DParser.Parser(scanner);
            TextWriter errorString = new StringWriter(); 
            parser.errors.errorStream = errorString;
            parser.Parse();
            scanner.buffer = null;
            scanner.stream.Close();
            scanner.stream.Dispose();
            string ErrorAusgabe = errorString.ToString();

            if (ErrorAusgabe.Length > 0)
            {
                if (MainF.m_ParserMessageBox)
                {
                    MessageBox.Show(ErrorAusgabe, Path.GetFileName(fileName), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                }
                
                MainF.Trace(ErrorAusgabe);
                
                Regex r1 = new Regex("Zeile ");
                Regex r2 = new Regex("Spalte ");
                Match m1 = r1.Match(ErrorAusgabe);
                Match m2 = r2.Match(ErrorAusgabe);
                string s1= "";
                string s2= "";
                for (int i=m1.Index+m1.Length;i<ErrorAusgabe.Length;i++)
                {
                    if (ErrorAusgabe[i]==',')
                    {
                        break;
                    }                    
                    s1=s1+ErrorAusgabe[i];
                }
                for (int i=m2.Index+m2.Length;i<ErrorAusgabe.Length;i++)
                {
                    if (ErrorAusgabe[i]==':')
                    {
                        break;
                    }                   
                    s2=s2+ErrorAusgabe[i];
                }

                MainF.ActiveEditor.JumpToError(Convert.ToInt32(s1), Convert.ToInt32(s2));
                


            }
            else
            {
                MainF.Trace("Keine Syntaxfehler gefunden!");
            }
       

            // Using...
           // MessageBox.Show(parser.errors.count.ToString());

            TreeNode nConstDec = new TreeNode("Konstanten-Deklarationen");
            foreach (TokenMatch tm in parser.m_CodeInfo.ConstDeclarations)
            {
                TreeNode n = new TreeNode(tm.Value);
                n.Tag = tm.Position;
                nConstDec.Nodes.Add(n);
            }
            if (nConstDec.Nodes.Count > 0)
            {
                nodes.Add(nConstDec);
            }
            TreeNode nVarDec = new TreeNode("Variablen-Deklarationen");
            foreach (TokenMatch tm in parser.m_CodeInfo.VarDeclarations)
            {
                TreeNode n = new TreeNode(tm.Value);
                n.Tag = tm.Position;
                nVarDec.Nodes.Add(n);
            }
            if (nVarDec.Nodes.Count > 0)
            {
                nodes.Add(nVarDec);
            }
            // Constructors...
            TreeNode nConstruct = new TreeNode("Funktionen");
            foreach (TokenMatch tm in parser.m_CodeInfo.Functions)
            {
                TreeNode n = new TreeNode(tm.Value);
                n.Tag = tm.Position;
                nConstruct.Nodes.Add(n);
            }
            if (nConstruct.Nodes.Count > 0)
            {
                nodes.Add(nConstruct);
                nConstruct.Expand();
            }

           
            TreeNode nField = new TreeNode("Instanzen");
            foreach (TokenMatch tm in parser.m_CodeInfo.Instances)
            {
                TreeNode n = new TreeNode(tm.Value);
                n.Tag = tm.Position;
                nField.Nodes.Add(n);
            }
            if (nField.Nodes.Count > 0)
            {
                nodes.Add(nField);
                nField.Expand();
            }

           
        }
    }
}
