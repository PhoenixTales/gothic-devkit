using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Peter
{
    public class XmlParser
    {
        public static void ParseToTree (string fileName, TreeNodeCollection nodes)
        {
            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(fileName);

                FillXMLTree(xDoc.DocumentElement, nodes);
                foreach (TreeNode n in nodes)
                {
                    n.Expand();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void FillXMLTree (XmlNode node, TreeNodeCollection parentNode)
        {
            // End recursion if the node is a text type
            if (node != null && node.NodeType != XmlNodeType.Text && node.NodeType != XmlNodeType.CDATA && node.NodeType != XmlNodeType.Comment)
            {
                TreeNodeCollection tmptreenodecollection = AddNodeToTree(node, parentNode);

                // Add all the children of the current node to the treeview
                foreach (XmlNode tmpchildnode in node.ChildNodes)
                {
                    FillXMLTree(tmpchildnode, tmptreenodecollection);
                }
            }
        }


        private static TreeNodeCollection AddNodeToTree (XmlNode node, TreeNodeCollection parentnode)
        {
            TreeNode newchildnode = CreateTreeNodeFromXmlNode(node);

            // if nothing to add, return the parent item
            if (newchildnode == null) return parentnode;

            // add the newly created tree node to its parent
            if (parentnode != null) parentnode.Add(newchildnode);

            return newchildnode.Nodes;
        }


        private static TreeNode CreateTreeNodeFromXmlNode (XmlNode node)
        {
            TreeNode tmptreenode = new TreeNode();

            if ((node.HasChildNodes) && (node.FirstChild.Value != null))
            {
                tmptreenode = new TreeNode(node.Name);
                TreeNode tmptreenode2 = new TreeNode(node.FirstChild.Value);
                tmptreenode.Nodes.Add(tmptreenode2);
            }
            else if (node.NodeType != XmlNodeType.CDATA)
            {
                tmptreenode = new TreeNode(node.Name);
            }

            if (node.Attributes.Count > 0)
            {
                tmptreenode.Text += " (";
                foreach (XmlAttribute att in node.Attributes)
                {
                    tmptreenode.Text += att.Name + "=\"" + att.Value + "\" ";
                }
                tmptreenode.Text += ")";
            }

            return tmptreenode;
        }
    }
}
