/**************************************************************************************
    Stampfer - Gothic Script Editor
    Copyright (C) 2008, 2009 Jpmon1, Alexander "Sumpfkrautjunkie" Ruppert

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
**************************************************************************************/
using System;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.IO;
using WeifenLuo.WinFormsUI.Docking;
using System.Xml;
using System.Text;
using System.Threading;
using System.Collections;

namespace Peter
{
    public delegate void ExpandDelegate(string node, string parent);

    public partial class ctrlFileExplorer : DockContent, PeterInterface.IPeterPluginTab
    {
        private PeterInterface.IPeterPluginHost m_Host;
        private MainForm MainF;
        private ExpandDelegate m_Expand;
        private string m_XML;
        private bool m_Folder;
        ArrayList Extensions = new ArrayList();
       

        public ctrlFileExplorer(MainForm f)
        {
            InitializeComponent();


            MainF = f;
            this.m_Host = f;
            this.TabText = "Datei Explorer";
            this.m_Expand = new ExpandDelegate(this.ExpandNode);

            TreeNode rootDesktop = new TreeNode("Desktop", 2, 2);
            this.treeMain.Nodes.Add(rootDesktop);
            rootDesktop.Name = "Desktop";
            rootDesktop.Nodes.Add("");
            TreeNode myComputer = new TreeNode("Arbeitsplatz", 4, 4);
            myComputer.Name = "Arbeitsplatz";
            this.treeMain.Nodes.Add(myComputer);
           if (MainF.m_ScriptsPath!="")
           {
               TreeNode Scripts = new TreeNode("Scripte", 9, 9);
            this.treeMain.Nodes.Add(Scripts);
            Scripts.Name = "Scripte";
            Scripts.Nodes.Add("");
           }
            this.treeMain.AfterSelect += new TreeViewEventHandler(treeMain_AfterSelect);
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                TreeNode driveNode = new TreeNode(drive.Name);
                driveNode.Name = drive.Name;
                switch (drive.DriveType)
                {
                    case DriveType.CDRom:
                        driveNode.SelectedImageIndex = 1;
                        driveNode.ImageIndex = 1;
                        break;
                    case DriveType.Network:
                        driveNode.SelectedImageIndex = 5;
                        driveNode.ImageIndex = 5;
                        break;
                    case DriveType.Removable:
                        driveNode.SelectedImageIndex = 0;
                        driveNode.ImageIndex = 0;
                        break;
                    default:
                        driveNode.SelectedImageIndex = 3;
                        driveNode.ImageIndex = 3;
                        break;
                }
                driveNode.Nodes.Add("");
                myComputer.Nodes.Add(driveNode);
            }

            this.treeMain.AfterLabelEdit += new NodeLabelEditEventHandler(treeMain_AfterLabelEdit);
            this.treeMain.BeforeExpand += new TreeViewCancelEventHandler(treeMain_BeforeExpand);
            this.treeMain.DoubleClick += new EventHandler(treeMain_DoubleClick);
            this.treeMain.KeyDown += new KeyEventHandler(treeMain_KeyDown);
            this.treeMain.MouseDown += new MouseEventHandler(treeMain_MouseDown);
           
        }

        void treeMain_AfterSelect(object sender, TreeViewEventArgs e)
        {
           e.Node.EnsureVisible();
            
        }

        void treeMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                treeMain_DoubleClick(null, new EventArgs());
            }
            

        }
        
        void treeMain_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    this.treeMain.SelectedNode = this.treeMain.GetNodeAt(e.X, e.Y);
                    if (this.treeMain.SelectedNode.Tag != null)
                    {
                        cShellContextMenu ctxMnu = new cShellContextMenu();
                        FileInfo[] arrFI = new FileInfo[1];
                        arrFI[0] = new FileInfo(this.treeMain.SelectedNode.Tag.ToString());
                        ctxMnu.ShowContextMenu(arrFI, this.PointToScreen(new Point(e.X, e.Y)));
                    }
                    else
                    {
                        cShellContextMenu ctxMnu = new cShellContextMenu();
                        DirectoryInfo[] dir = new DirectoryInfo[1];
                        dir[0] = new DirectoryInfo(GetFolderPath(this.treeMain.SelectedNode));
                        ctxMnu.ShowContextMenu(dir, this.PointToScreen(new Point(e.X, e.Y)));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void treeMain_DoubleClick(object sender, EventArgs e)
        {
            if (this.treeMain.SelectedNode != null)
            {
                if (this.treeMain.SelectedNode.Tag != null
                    && this.treeMain.SelectedNode.Tag.ToString().IndexOf('.')>0)
                {
                    if (this.treeMain.SelectedNode.ImageIndex != -1)
                    {
                        this.m_Host.CreateEditor(this.treeMain.SelectedNode.Tag.ToString(),
                            Path.GetFileName(this.treeMain.SelectedNode.Tag.ToString()),
                            Icon.FromHandle(((Bitmap)this.imgMain.Images[this.treeMain.SelectedNode.ImageIndex]).GetHicon()));
                    }
                    else
                    {
                        this.m_Host.CreateEditor(this.treeMain.SelectedNode.Tag.ToString(),
                            Path.GetFileName(this.treeMain.SelectedNode.Tag.ToString()));
                    }
                }
            }
        }

        private void treeMain_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Text != "Arbeitsplatz")
            {
                this.EnumerateDirectory(e.Node);
            }
        }

        private void EnumerateDirectory(TreeNode parentNode)
        {
            DirectoryInfo dirInfo;
            string path = GetFolderPath(parentNode);
            parentNode.Nodes.Clear();

            dirInfo = new DirectoryInfo(path);
            DirectoryInfo[] dirs = dirInfo.GetDirectories();
           // Array.Sort(dirs, new cDirectorySorter());
            //MessageBox.Show(dirs.Length.ToString());
            foreach (DirectoryInfo dirI in dirs)
            {
                TreeNode node = new TreeNode(dirI.Name, 6, 6);
                node.Name = dirI.Name;
                node.Nodes.Add("");
                parentNode.Nodes.Add(node);
            }

            FileInfo[] files = dirInfo.GetFiles();
           // Array.Sort(files, new cFileSorter());
           // string ext = "";
           // MessageBox.Show(this.imgMain.Images.Count.ToString());
            foreach (FileInfo file in files)
            {
                TreeNode node = new TreeNode(file.Name, 6, 6);
                node.Name = file.Name;
                /*ext = Path.GetExtension(file.Name);

                if (Extensions.Count == 0)
                {
                    this.imgMain.Images.Add(this.m_Host.GetFileIcon(file.FullName, false));
                    Extensions.Add(ext);
                }
                foreach(string s in Extensions)
                {
                    
                    if (String.Compare(s,ext)!=0)
                    {
                        this.imgMain.Images.Add(this.m_Host.GetFileIcon(file.FullName, false));
                        Extensions.Add(s);
                        break;
                    }
                }*/
               // this.imgMain.Images.Add(this.m_Host.GetFileIcon(file.FullName, false));
                
                // this.imgMain.Images.Add(this.m_Host.GetFileIcon(file.FullName, false));
              // this.imgMain.Images.Add(new Icon(@"d:\Dokumente und Einstellungen\Sumpfkrautjunkie\Eigene Dateien\Visual Studio 2005\Projects\PeterSource1_1\Icons\Icon.ico"));
                //this.imgMain.Images.SetKeyName(0, "RemovableDrive.png");
                //this.imgMain.Images.Add(IconHandler.IconFromExtension(Path.GetExtension(file.Name),IconSize.Small));

                int imgIndex=0;
                if (Path.GetExtension(file.Name)==".d"||Path.GetExtension(file.Name)==".src")
                {
                    imgIndex =9;

                }
                else
                {
                    imgIndex = 8;
                }
                node.Tag = file.FullName;
                node.ImageIndex = imgIndex;
                node.SelectedImageIndex = imgIndex;
                parentNode.Nodes.Add(node);
            }
           
            //MessageBox.Show(this.imgMain.Images.Count.ToString());
        }

        private string GetFolderPath(TreeNode parentNode)
        {
            string path = "";
            string[] pathSplit = parentNode.FullPath.Split('\\');
            if (pathSplit[0] == "Desktop")
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\";
                for (int a = 1; a < pathSplit.Length; a++)
                {
                    if (pathSplit[a].Trim() != string.Empty)
                    {
                        path += pathSplit[a] + "\\";
                    }
                }
            }
            else if (pathSplit[0] == "Scripte")
            {
                path = MainF.m_ScriptsPath + "\\";
                for (int a = 1; a < pathSplit.Length; a++)
                {
                    if (pathSplit[a].Trim() != string.Empty)
                    {
                        path += pathSplit[a] + "\\";
                    }
                }

            }
            else
            {
                for (int a = 1; a < pathSplit.Length; a++)
                {
                    if (pathSplit[a].Trim() != string.Empty)
                    {
                        path += pathSplit[a] + "\\";
                    }
                }

            }
            return path;
        }

        protected override string GetPersistString()
        {
            MemoryStream ms = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(ms, Encoding.Unicode);
            writer.WriteStartDocument();
            writer.WriteStartElement("FileExplorer");
            this.WriteNodes(this.treeMain.Nodes, writer);
            writer.WriteEndElement();
            writer.Flush();

            ms.Position = 0;
            StreamReader sr = new StreamReader(ms);
            string text = sr.ReadToEnd();
            writer.Close();
            sr.Close();


            return this.GetType().ToString() + "|" + text;
        }

        private void WriteNodes(TreeNodeCollection nodesCollection,
            XmlTextWriter textWriter)
        {
            for (int i = 0; i < nodesCollection.Count; i++)
            {
                TreeNode node = nodesCollection[i];
                if (node.IsExpanded)
                {
                    textWriter.WriteStartElement("node");
                    textWriter.WriteAttributeString("text", node.Text);
                    if (node.Tag != null) textWriter.WriteAttributeString("tag", node.Tag.ToString());

                    if (node.Nodes.Count > 0)
                    {
                        WriteNodes(node.Nodes, textWriter);
                    }
                    textWriter.WriteEndElement();
                }
            }
        }

        public void LoadTree(string xml)
        {
            this.m_XML = xml;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (this.m_XML != null && this.m_XML.Trim() != string.Empty)
            {
                Thread t = new Thread(new ParameterizedThreadStart(this.LoadTreeThread));
                t.Start(m_XML);
            }
        }

        private void ExpandNode(string nodeName, string parent)
        {
            TreeNode[] nodes = this.treeMain.Nodes.Find(nodeName, true);
            foreach (TreeNode node in nodes)
            {
                if (parent == "")
                {
                    node.Expand();
                }
                else
                {
                    if (node.Parent.Text == parent)
                    {
                        node.Expand();
                        break;
                    }
                }
            }
        }

        private void LoadTreeThread(object oxml)
        {
            string xml = oxml.ToString();
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(xml);
            XmlNodeList nodes = xDoc.GetElementsByTagName("node");
            foreach (XmlNode node in nodes)
            {
                string name = node.Attributes["text"].Value;
                XmlAttribute att = node.ParentNode.Attributes["text"];
                string parName = (att == null) ? "" : att.Value;
                if (this.m_Expand != null)
                {
                    //this.Invoke(this.m_Expand, new object[] { name, parName });
                }
            }
        }

        #region IPeterPluginTab Members

        public void Save()
        {
        }

        public void SaveAs(string filePath)
        {
        }

        public void Cut()
        {
        }

        public void Copy()
        {
            if (this.treeMain.SelectedNode != null)
            {
                if (this.treeMain.SelectedNode.Tag != null)
                {
                    Clipboard.SetText(this.treeMain.SelectedNode.Tag.ToString());
                }
            }
        }

        public void Paste()
        {
        }

        public void Undo()
        {
        }

        public void Redo()
        {
        }

        public void Print()
        {
        }

        public void Delete()
        {
        }

        public void Duplicate()
        {
        }

        public void SelectAll()
        {
        }

        public bool CloseTab()
        {
            this.Close();
            return true;
        }

        public void MarkAll(System.Text.RegularExpressions.Regex reg)
        {
        }

        public bool FindNext(System.Text.RegularExpressions.Regex reg, bool searchUp)
        {
            return false;
        }

        public void ReplaceNext(System.Text.RegularExpressions.Regex reg, string replaceWith, bool searchUp)
        {
        }

        public void ReplaceAll(System.Text.RegularExpressions.Regex reg, string replaceWith)
        {
        }

        public void SelectWord(int line, int offset, int wordLeng)
        {
        }

        public PeterInterface.IPeterPluginHost Host
        {
            get
            {
                return this.m_Host;
            }
            set
            {
                this.m_Host = value;
            }
        }

        public string FileName
        {
            get { return ""; }
        }

        public string Selection
        {
            get { return ""; }
        }

        public bool AbleToUndo
        {
            get { return false; }
        }

        public bool AbleToRedo
        {
            get { return false; }
        }

        public bool NeedsSaving
        {
            get { return false; }
        }

        public bool AbleToPaste
        {
            get { return false; }
        }

        public bool AbleToCut
        {
            get { return false; }
        }

        public bool AbleToCopy
        {
            get { return true; }
        }

        public bool AbleToSelectAll
        {
            get { return false; }
        }

        public bool AbleToSave
        {
            get { return false; }
        }

        public bool AbleToDelete
        {
            get { return false; }
        }

        #endregion

        private void tsbRefresh_Click(object sender, EventArgs e)
        {
            if (this.treeMain.SelectedNode != null)
            {
                if (this.treeMain.SelectedNode.Tag != null)
                {
                    this.treeMain.SelectedNode.Parent.Collapse();
                    this.treeMain.SelectedNode.Parent.Expand();
                }
                else
                {
                    this.treeMain.SelectedNode.Collapse();
                    this.treeMain.SelectedNode.Expand();
                }
            }
        }

        private void tsbNewFolder_Click(object sender, EventArgs e)
        {
            this.m_Folder = true;
            this.AddNode();
        }

        private void tsbNewFile_Click(object sender, EventArgs e)
        {
            this.m_Folder = false;
            this.AddNode();
        }

        private void AddNode()
        {
            this.treeMain.LabelEdit = true;
            if (this.treeMain.SelectedNode != null)
            {
                TreeNode node = new TreeNode();
                if (this.treeMain.SelectedNode.Tag != null)
                {
                    this.treeMain.SelectedNode.Parent.Nodes.Add(node);
                }
                else
                {
                    this.treeMain.SelectedNode.Nodes.Add(node);
                }
                node.Text = (this.m_Folder) ? "Neuer Ordner" : "Neue Textdokument";
                node.SelectedImageIndex = node.ImageIndex = (this.m_Folder) ? 6 : 7;
                node.BeginEdit();
            }
        }

        private void treeMain_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            try
            {
                e.Node.EndEdit(true);
                e.Node.Text = e.Label;
                this.treeMain.LabelEdit = false;
                string path = GetFolderPath(e.Node.Parent);

                if (this.m_Folder)
                {
                    string[] dirs = Directory.GetDirectories(path);
                    foreach (string dir in dirs)
                    {
                        if (dir.ToLower().Equals(e.Node.Text.ToLower()))
                        {
                            this.treeMain.LabelEdit = true;
                            MessageBox.Show(this, "Ein gleichnamiger Ordner existiert bereits. Bitte verwenden Sie einen anderen Namen.",
                                "Stampfer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            e.Node.BeginEdit();
                            return;
                        }
                    }

                    e.Node.SelectedImageIndex = e.Node.ImageIndex = 6;
                    Directory.CreateDirectory(path + "\\" + e.Label);
                }
                else
                {
                    string[] files = Directory.GetFiles(path);
                    foreach (string file in files)
                    {
                        if (Path.GetFileName(file).ToLower().Equals(e.Node.Text.ToLower()))
                        {
                            this.treeMain.LabelEdit = true;
                            MessageBox.Show(this, "Eine gleichnamige Datei existiert bereits. Bitte verwenden Sie einen anderen Namen.",
                                "Stampfer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            e.Node.BeginEdit();
                            return;
                        }
                    }
                    e.Node.Tag = path + e.Label;
                    FileStream f = File.Create(path + e.Label);
                    f.Close();

                    this.imgMain.Images.Add(this.m_Host.GetFileIcon(e.Node.Tag.ToString(), false));
                    e.Node.SelectedImageIndex = e.Node.ImageIndex = this.imgMain.Images.Count - 1;
                }
                this.treeMain.SelectedNode = e.Node;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
