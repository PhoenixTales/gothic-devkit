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
        public static void ParseToTree(MainForm MainF, bool errormeld, string fileName, ref List<Instance> lInstances, ref List<Instance> lFuncs, ref List<Instance> lVars, ref List<Instance> lConsts)
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
                string[] err = ErrorAusgabe.Split('\n');
                ErrorAusgabe = err[0].Trim(); ;
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

                if (errormeld) MainF.MyActiveEditor.JumpToError(Convert.ToInt32(s1), Convert.ToInt32(s2));
                


            }
            else
            {
                MainF.Trace("Keine Syntaxfehler gefunden!");
            }
       

            // Using...
           // MessageBox.Show(parser.errors.count.ToString());

            //MessageBox.Show(parser.m_CodeInfo.ConstDeclarations.Count.ToString());
            foreach (TokenMatch tm in parser.m_CodeInfo.Instances)
            {
                lInstances.Add(new Instance(tm.Value, tm.Position));
            }
            foreach (TokenMatch tm in parser.m_CodeInfo.Functions)
            {
                lFuncs.Add(new Instance(tm.Value, tm.Position));
            }
            foreach (TokenMatch tm in parser.m_CodeInfo.VarDeclarations)
            {
                lVars.Add(new Instance(tm.Value, tm.Position));
            }
            foreach (TokenMatch tm in parser.m_CodeInfo.ConstDeclarations)
            {
                lConsts.Add(new Instance(tm.Value, tm.Position));
            }


           
        }
    }
}
