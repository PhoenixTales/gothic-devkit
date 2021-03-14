using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections.Specialized;

namespace Peter.VBParser
{
    public class Util
    {
        string identStr = "";

        public TextWriter output;

        StringCollection linePush = new StringCollection();

        public void Ident()
        {
            identStr += "    ";
        }

        public void Unident()
        {
            identStr = identStr.Substring(0, identStr.Length - 4);
        }

        public void OpenBlock()
        {
            Writeline("{");
            Ident();
        }

        public void CloseBlock()
        {
            Unident();
            Writeline("}" + Environment.NewLine);
        }

        public void Writeline(string line)
        {
            output.Write(identStr);
            output.WriteLine(line);
            if (linePush.Count != 0)
            {
                foreach (string s in linePush)
                {
                    output.Write(identStr);
                    output.WriteLine(s);
                }
                linePush.Clear();
            }
        }

        public void PushLine(string line)
        {
            linePush.Add(line);
        }

        public string ConvertModifier(string mod)
        {
            switch (mod)
            {
                case "Overrides":
                    return "override ";
                case "Shared":
                    return "static";
                case "Friend":
                    return "internal";
                default:
                    return mod.ToLower() + " ";
            }
        }

        public string ConverteTipo(string tipo)
        {
            switch (tipo)
            {
                case "Integer":
                    return "int";
                case "String":
                    return "string";
                case "Boolean":
                    return "bool";
                case "Long":
                    return "long";
                case "Single":
                    return "float";
                case "Double":
                    return "double";
                case "Short":
                    return "short";
                case "Byte":
                    return "byte";
                case "Char":
                    return "char";
                default:
                    return tipo;
            }
        }

        public string ConverteString(string str)
        {
            if (str.IndexOf('\\') >= 0)
                return "@" + str;
            else
                return str;
        }

        public string ConverteLogicalOp(string str)
        {
            switch (str)
            {
                case "=":
                    return " == ";
                case "<>":
                    return " != ";
                default:
                    return " " + str + " ";
            }
        }

        public string ConvertByX(string byx)
        {
            if (byx == "ByVal")
                return "";
            else
                return "ref ";
        }
    }
}
