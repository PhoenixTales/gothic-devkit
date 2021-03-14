using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Peter.CSSParser
{
    class CSSParser
    {

        public static void ParseToTree (string fileName, TreeNodeCollection nodes)
        {
            FillCSSTree(ParseFile(fileName), nodes);
        }

        private static CSS ParseFile(string file)
        {
            StringBuilder sb = new StringBuilder();
            TextWriter errorCatch = new StringWriter(sb);
            Scanner scanner = new Scanner(file);
            Parser parser = new Parser(scanner);
            parser.errors.errorStream = errorCatch;
            parser.Parse();
            return parser.CSSDocument;
        }

        private static void FillCSSTree (CSS css, TreeNodeCollection nodes)
        {
            foreach (Selector sel in css.Selectors)
            {
                TreeNode s = new TreeNode();
                s.ImageIndex = 0;
                bool first = true;
                foreach (Tag t in sel.Tags)
                {
                    if (first) { first = false; } else { s.Text += ", "; }
                    s.Text += t.ToString();
                }
                s.Tag = sel;
                nodes.Add(s);
                foreach (Tag tag in sel.Tags)
                {
                    TreeNode t = AddChild(tag);
                    s.Nodes.Add(t);
                }
                foreach (Property prp in sel.Properties)
                {
                    TreeNode p = new TreeNode(prp.Attribute, 2, 2);
                    p.Tag = prp;
                    s.Nodes.Add(p);

                    foreach (PropertyValue pv in prp.Values)
                    {
                        TreeNode v = new TreeNode(pv.ToString(), 3, 3);
                        if (pv.IsColor)
                        {
                            v.ForeColor = pv.ToColor();
                        }
                        v.Tag = pv;
                        p.Nodes.Add(v);
                    }
                }
            }
        }
        private static TreeNode AddChild (Tag tag)
        {
            TreeNode t = new TreeNode(tag.ToShortString(), 1, 1);
            t.Tag = tag;
            foreach (Tag sub in tag.SubTags)
            {
                t.Nodes.Add(AddChild(sub));
            }
            return t;
        }

        public List<Token> GetTokens(string file)
        {
            StringBuilder sb = new StringBuilder();
            TextWriter errorCatch = new StringWriter(sb);
            Scanner scanner = new Scanner(file);

            List<Token> ts = new List<Token>();
            Token t = scanner.Scan();
            if (t.val != "\0") { ts.Add(t); }
            while (t.val != "\0")
            {
                t = scanner.Scan();
                ts.Add(t);
            }
            return ts;
        }

        public CSS ParseText(string content)
        {
            MemoryStream mem = new MemoryStream();
            byte[] bytes = ASCIIEncoding.ASCII.GetBytes(content);
            mem.Write(bytes, 0, bytes.Length);
            return ParseStream(mem);
        }

        public CSS ParseStream(Stream stream)
        {
            StringBuilder sb = new StringBuilder();
            TextWriter errorCatch = new StringWriter(sb);
            Scanner scanner = new Scanner(stream);
            Parser parser = new Parser(scanner);
            parser.errors.errorStream = errorCatch;
            parser.Parse();
            return parser.CSSDocument;
        }
    }
}
