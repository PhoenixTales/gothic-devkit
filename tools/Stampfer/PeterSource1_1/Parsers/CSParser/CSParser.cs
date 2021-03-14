using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Peter.CSParser
{
    public class CSParser
    {
        public static void ParseToTree (string fileName, TreeNodeCollection nodes)
        {
            Peter.CSParser.Scanner scanner = new Peter.CSParser.Scanner(fileName);
            Peter.CSParser.Parser parser = new Peter.CSParser.Parser(scanner);
            parser.Parse();

            // Using...
            TreeNode nUsing = new TreeNode("Usings");
            foreach (TokenMatch tm in parser.CodeInfo.Usings)
            {
                TreeNode n = new TreeNode(tm.Value);
                n.Tag = tm.Position;
                nUsing.Nodes.Add(n);
            }
            if (nUsing.Nodes.Count > 0)
            {
                nodes.Add(nUsing);
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

            // Namespace...
            /*TreeNode nName = new TreeNode("namespaces");
            foreach (string s in parser.CodeInfo.NameSpaces)
            {
                TreeNode n = new TreeNode(s);
                nName.Nodes.Add(n);
            }
            if (nName.Nodes.Count > 0)
            {
                this.m_Control.AddNode(nName);
            }*/

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

            // Properties...
            TreeNode nProps = new TreeNode("Properties");
            foreach (TokenMatch tm in parser.CodeInfo.Properties)
            {
                TreeNode n = new TreeNode(tm.Value);
                n.Tag = tm.Position;
                nProps.Nodes.Add(n);
            }
            if (nProps.Nodes.Count > 0)
            {
                nodes.Add(nProps);
                nProps.Expand();
            }
        }
    }
}
