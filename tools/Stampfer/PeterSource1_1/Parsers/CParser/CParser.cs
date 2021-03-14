using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Peter.CSParser;

namespace Peter.CParser
{
    class CParser
    {
        public static void ParseToTree (string fileName, TreeNodeCollection nodes)
        {
            Peter.CParser.Scanner scanner = new Peter.CParser.Scanner(fileName);
            Peter.CParser.Parser parser = new Peter.CParser.Parser(scanner);
            parser.Parse();

            // Include...
            TreeNode nInclude = new TreeNode("Includes");
            foreach (TokenMatch tm in parser.CodeInfo.Includes)
            {
                TreeNode n = new TreeNode(tm.Value);
                n.Tag = tm.Position;
                nInclude.Nodes.Add(n);
            }
            if (nInclude.Nodes.Count > 0)
            {
                nodes.Add(nInclude);
            }

            // Defines...
            TreeNode nDefine = new TreeNode("Defines");
            foreach (TokenMatch tm in parser.CodeInfo.Defines)
            {
                TreeNode n = new TreeNode(tm.Value);
                n.Tag = tm.Position;
                nDefine.Nodes.Add(n);
            }
            if (nDefine.Nodes.Count > 0)
            {
                nodes.Add(nDefine);
            }

            // GlobalVars...
            TreeNode nGlobalVars = new TreeNode("Global Variables");
            foreach (TokenMatch tm in parser.CodeInfo.GlobalVariables)
            {
                TreeNode n = new TreeNode(tm.Value);
                n.Tag = tm.Position;
                nGlobalVars.Nodes.Add(n);
            }
            if (nGlobalVars.Nodes.Count > 0)
            {
                nodes.Add(nGlobalVars);
                nGlobalVars.Expand();
            }

            // Structs...
            TreeNode nStructs = new TreeNode("Structs");
            foreach (TokenMatch tm in parser.CodeInfo.Structs)
            {
                TreeNode n = new TreeNode(tm.Value);
                n.Tag = tm.Position;
                nStructs.Nodes.Add(n);
            }
            if (nStructs.Nodes.Count > 0)
            {
                nodes.Add(nStructs);
                nStructs.Expand();
            }

            // TypeDefs...
            TreeNode nTypeDefs = new TreeNode("TypeDefs");
            foreach (TokenMatch tm in parser.CodeInfo.TypeDefs)
            {
                TreeNode n = new TreeNode(tm.Value);
                n.Tag = tm.Position;
                nTypeDefs.Nodes.Add(n);
            }
            if (nTypeDefs.Nodes.Count > 0)
            {
                nodes.Add(nTypeDefs);
                nTypeDefs.Expand();
            }

            // Prototypes...
            TreeNode nPrototypes = new TreeNode("Prototypes");
            foreach (TokenMatch tm in parser.CodeInfo.Prototypes)
            {
                TreeNode n = new TreeNode(tm.Value);
                n.Tag = tm.Position;
                nPrototypes.Nodes.Add(n);
            }
            if (nPrototypes.Nodes.Count > 0)
            {
                nodes.Add(nPrototypes);
                nPrototypes.Expand();
            }

            // Functions...
            TreeNode nFunctions = new TreeNode("Functions");
            foreach (TokenMatch tm in parser.CodeInfo.Functions)
            {
                TreeNode n = new TreeNode(tm.Value);
                n.Tag = tm.Position;
                nFunctions.Nodes.Add(n);
            }
            if (nFunctions.Nodes.Count > 0)
            {
                nodes.Add(nFunctions);
                nFunctions.Expand();
            }
        }
    }
}
