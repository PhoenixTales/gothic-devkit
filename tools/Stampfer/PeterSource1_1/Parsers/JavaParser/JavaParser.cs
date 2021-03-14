using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Peter.CSParser;

namespace Peter.JavaParser
{
    public class JavaParser
    {
        public static void ParseToTree (string fileName, TreeNodeCollection nodes)
        {
            Peter.JavaParser.Scanner scanner = new Peter.JavaParser.Scanner(fileName);
            Peter.JavaParser.Parser parser = new Peter.JavaParser.Parser(scanner);
            parser.Parse();

            // Import...
            TreeNode nImport = new TreeNode("Imports");
            foreach (TokenMatch tm in parser.CodeInfo.Imports)
            {
                TreeNode n = new TreeNode(tm.Value);
                n.Tag = tm.Position;
                nImport.Nodes.Add(n);
            }
            if (nImport.Nodes.Count > 0)
            {
                nodes.Add(nImport);
            }

            // Fields...
            TreeNode nField = new TreeNode("Fields");
            foreach (TokenMatch tm in parser.CodeInfo.Fields)
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

            // Constructors...
            TreeNode nConstruct = new TreeNode("Constructors");
            foreach (TokenMatch tm in parser.CodeInfo.Constructors)
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

            // Methods...
            TreeNode nMethod = new TreeNode("Methods");
            foreach (TokenMatch tm in parser.CodeInfo.Methods)
            {
                TreeNode n = new TreeNode(tm.Value);
                n.Tag = tm.Position;
                nMethod.Nodes.Add(n);
            }
            if (nMethod.Nodes.Count > 0)
            {
                nodes.Add(nMethod);
                nMethod.Expand();
            }
        }
    }
}
