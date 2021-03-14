/**************************************************************************************
    Stampfer - Gothic Script Editor
    Copyright (C) 2008 Jpmon1

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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using PeterInterface;
using System.Xml;
using System.IO;
using System.Collections;
using System.Threading;
using Peter.CSParser;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO.Compression;
using System.Runtime.Remoting.Messaging;

namespace Peter
{
    public delegate void StartParsing (ArrayList fileList, cProjectInfo projNode);

    public partial class ProjectManager : DockContent, IPeterPluginTab
    {
        private IPeterPluginHost m_Host;
        private ContextMenuStrip m_ctxRoot;
        private MainForm m_MainForm;
        private TraceDelegate m_delTrace;

        #region -= Constructor =-

        public ProjectManager(MainForm main)
        {
            InitializeComponent();

            this.m_MainForm = main;
            this.m_ctxRoot = new ContextMenuStrip();
            ToolStripMenuItem tsmiBuild = new ToolStripMenuItem("Build");
            tsmiBuild.Click += new EventHandler(Build);
            ToolStripMenuItem tsmiRmvProject = new ToolStripMenuItem("Remove Project");
            tsmiRmvProject.Click += new EventHandler(RemoveProject);

            this.m_ctxRoot.Items.Add(tsmiBuild);
            this.m_ctxRoot.Items.Add(tsmiRmvProject);

            this.TabText = "Projekte";
            this.m_delTrace = new TraceDelegate(this.m_MainForm.Trace);
            this.treeMain.BeforeExpand += new TreeViewCancelEventHandler(treeMain_BeforeExpand);
            this.treeMain.DoubleClick += new EventHandler(treeMain_DoubleClick);
            this.treeMain.AfterSelect += new TreeViewEventHandler(treeMain_AfterSelect);
        }

        #endregion

        #region -= Build =-

        void Build(object sender, EventArgs e)
        {
            string script = "";
            string workingDir = "";
            string proj = this.treeMain.SelectedNode.Tag.ToString();

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(proj);

            // Working Dir...
            XmlNodeList nodes = xDoc.GetElementsByTagName("WorkingDir");
            if (nodes.Count == 1)
            {
                workingDir = nodes[0].InnerText;
            }
            if (string.IsNullOrEmpty(workingDir))
            {
                workingDir = Application.StartupPath;
            }
            
            // Build Script...
            nodes = xDoc.GetElementsByTagName("BuildScript");
            if (nodes.Count == 1)
            {
                script = nodes[0].InnerText;
            }
            
            // Pre-Build/Post-Build Files...
            string preBuild = "", postBuild = System.Environment.NewLine;
            nodes = xDoc.GetElementsByTagName("BuildFile");
            foreach (XmlNode node in nodes)
            {
                bool bpreBuild = false;
                string file = "";
                foreach (XmlNode cNode in node.ChildNodes)
                {
                    if (cNode.Name.ToLower() == "file")
                    {
                        file = cNode.InnerText;
                    }

                    if (cNode.Name.ToLower() == "prebuild")
                    {
                        bpreBuild = Convert.ToBoolean(cNode.InnerText);
                    }
                }

                if (bpreBuild)
                {
                    preBuild += file + System.Environment.NewLine;
                }
                else
                {
                    postBuild += file + System.Environment.NewLine;
                }
            }

            script = preBuild + script + postBuild;
            this.m_MainForm.RunScript(script, workingDir);
        }

        #endregion

        #region -= Tree Selection =-

        void treeMain_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (this.treeMain.SelectedNode != null)
            {
                this.tsbDelete.Enabled = true;
                if (this.treeMain.SelectedNode.Parent == null)
                {
                    this.tsbAddFile.Enabled = this.tsbAddFolder.Enabled =
                        this.tsbProperties.Enabled = this.tsbRefresh.Enabled = true;
                    this.tsbOpenDir.Enabled = false;
                }
                else if (this.treeMain.SelectedNode.Nodes.Count == 0)
                {
                    this.tsbProperties.Enabled = this.tsbAddFolder.Enabled = this.tsbRefresh.Enabled =
                        this.tsbAddFile.Enabled = false;
                    this.tsbOpenDir.Enabled = true;
                }
                else
                {
                    this.tsbRefresh.Enabled = this.tsbOpenDir.Enabled = true;
                    this.tsbProperties.Enabled = this.tsbAddFolder.Enabled = this.tsbAddFile.Enabled = false;
                }
            }
            else
            {
                this.tsbAddFile.Enabled = this.tsbAddFolder.Enabled = this.tsbDelete.Enabled =
                    this.tsbProperties.Enabled = this.tsbRefresh.Enabled = this.tsbOpenDir.Enabled = false;
            }
        }

        void treeMain_DoubleClick(object sender, EventArgs e)
        {
            if (this.treeMain.SelectedNode != null)
            {
                if (this.treeMain.SelectedNode.Nodes.Count == 0)
                {
                    TreeNode n = this.treeMain.SelectedNode.Parent;
                    while (n.Parent != null)
                    {
                        n = n.Parent;
                    }
                    cProjectInfo pInfo = (cProjectInfo)n.Tag;
                    string file = this.treeMain.SelectedNode.Tag.ToString();
                    this.m_Host.CreateEditor(file, Path.GetFileName(file));
                    this.m_MainForm.GetEditor(file).Project = pInfo.Path;
                }
            }
        }

        void treeMain_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Tag.ToString().ToLower() != "vfolder")
            {
                if (!this.treeMain.Nodes.Contains(e.Node))
                {
                    this.GetDirContent(e.Node);
                }
            }
        }

        #endregion

        #region -= Get Directory Content =-

        private void GetDirContent(TreeNode node)
        {
            node.Nodes.Clear();
            DirectoryInfo dirInfo = new DirectoryInfo(node.Tag.ToString());
            if (dirInfo.Exists)
            {
                DirectoryInfo[] dirs = dirInfo.GetDirectories();
                Array.Sort(dirs, new cDirectorySorter());
                foreach (DirectoryInfo dir in dirs)
                {
                    TreeNode tNode = new TreeNode();
                    tNode.Tag = dir.FullName;
                    string folder = dir.FullName.Substring(dir.FullName.LastIndexOf('\\') + 1);
                    tNode.Text = tNode.Name = folder;
                    tNode.Nodes.Add("");

                    node.Nodes.Add(tNode);
                }

                FileInfo[] files = dirInfo.GetFiles();
                Array.Sort(files, new cFileSorter());
                foreach (FileInfo file in files)
                {
                    TreeNode tNode = new TreeNode();
                    tNode.Tag = file.FullName;
                    tNode.Text = tNode.Name = Path.GetFileName(file.FullName);
                    this.GetNodeImage(tNode);

                    node.Nodes.Add(tNode);
                }
            }
            else
            {
                MessageBox.Show("Fad nicht gefunden.", "Stampfer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region -= Load File =-

        /// <summary>
        /// Loads the given project...
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public string LoadFile(string filePath)
        {
            // Check to see if the project is open...
            foreach (TreeNode rN in this.treeMain.Nodes)
            {
                if (rN.Tag.ToString() == filePath)
                {
                    // The project is already open, leave...
                    return null;
                }
            }

            // Does the file Exist???
            if (File.Exists(filePath))
            {
                cProjectInfo pInfo = new cProjectInfo();

                // Load project file...
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(filePath);

                // Get Project Name...
                XmlNodeList nodes = xDoc.GetElementsByTagName("name");
                if (nodes.Count < 1)
                {
                    // No name, get out of here...
                    return null;
                }
                string proj = nodes[0].InnerText;

                // Set up Root Node...
                TreeNode root = new TreeNode(proj);
                root.Name = proj;
                root.ImageIndex = root.SelectedImageIndex = 1;
                root.ContextMenuStrip = this.m_ctxRoot;

                // Get Project Type...
                nodes = xDoc.GetElementsByTagName("type");
                if (nodes.Count > 0)
                {
                    pInfo.Type = nodes[0].InnerText;
                    if (string.IsNullOrEmpty(pInfo.Type)) pInfo.Type = "None";
                }
                else { pInfo.Type = "None"; }

                // Set up Project Info...
                pInfo.Name = proj;
                pInfo.Path = filePath;
                root.Tag = pInfo;

                // List of folders and files...
                ArrayList folders = new ArrayList();
                ArrayList files = new ArrayList();

                // Get Folders...
                nodes = xDoc.GetElementsByTagName("folder");
                foreach (XmlNode node in nodes)
                {
                    GetFolder(root, node);
                    folders.Add(node.InnerText);
                }

                // Get Files...
                nodes = xDoc.GetElementsByTagName("file");
                foreach (XmlNode node in nodes)
                {
                    GetFile(root, node);
                    files.Add(node.InnerText);
                }

                // Add Project to TreeView...
                this.treeMain.Nodes.Add(root);
                this.treeMain.SelectedNode = root;

                // Parse Project...
                if (!pInfo.Equals("None"))
                {
                    ArrayList fileList = new ArrayList();
                    string[] filters = new string[1];
                    filters[0] = "*.*";
                    switch (pInfo.Type.ToLower())
                    {
                        case "c project":
                            filters = new string[2];
                            filters[0] = "*.c";
                            filters[1] = "*.h";
                            break;
                    }

                    foreach (string folder in folders)
                    {
                        DirectoryInfo di = new DirectoryInfo(folder);
                        foreach (string filter in filters)
                        {
                            FileInfo[] dirFiles = di.GetFiles(filter, SearchOption.AllDirectories);
                            foreach (FileInfo fi in dirFiles)
                            {
                                fileList.Add(fi);
                            }
                        }
                    }
                    foreach (string file in files)
                    {
                        switch (pInfo.Type.ToLower())
                        {
                            case "c project":
                                foreach (string filter in filters)
                                {
                                    if (filter.Equals("*" + Path.GetExtension(file)))
                                    {
                                        fileList.Add(new FileInfo(file));
                                    }
                                }
                                break;
                        }
                    }

                    StartParsing del = new StartParsing(ParseProject);
                    del.BeginInvoke(fileList, pInfo, new AsyncCallback(CallBack), null);
                }

                return proj;
            }
            else
            {
                return null;
            }
        }

        #endregion

        public void CallBack (IAsyncResult ar)
        {
            StartParsing del = (StartParsing)((AsyncResult)ar).AsyncDelegate;
            Console.WriteLine("--- Done Parsing ---");
            del.EndInvoke(ar);
        }

        public void ParseProject (ArrayList fileList, cProjectInfo pInfo)
        {
            //cProjectInfo pInfo = (cProjectInfo)projNode.Tag;
            pInfo.Data = new TreeNode("root");

            switch(pInfo.Type.ToLower())
            {
                case "c project":
                    foreach (FileInfo fi in fileList)
                    {
                        ParseCFile(pInfo.Data, fi);
                    }
                    break;
            }
            this.m_delTrace.BeginInvoke(pInfo.Name + " Project Data Complete.", null, null);
        }

        private  void ParseCFile (TreeNode data, FileInfo fi)
        {
            this.m_delTrace.BeginInvoke("Gathering Project Data for file: " + fi.FullName, null, null);
            TreeNode file = new TreeNode(fi.FullName);
            file.Name = fi.FullName;

            TreeNode prototypes = new TreeNode("prototypes");
            prototypes.Name = "prototypes";

            TreeNode variables = new TreeNode("variables");
            variables.Name = "variables";

            TreeNode defines = new TreeNode("defines");
            defines.Name = "defines";

            Peter.CParser.Scanner scanner = new Peter.CParser.Scanner(fi.FullName);
            Peter.CParser.Parser parser = new Peter.CParser.Parser(scanner);
            parser.Parse();
            foreach (TokenMatch tm in parser.CodeInfo.Defines)
            {
                TreeNode node = new TreeNode(tm.Value);
                node.Name = tm.Value;
                node.Tag = tm.Position;
                defines.Nodes.Add(node);
            }
            foreach (TokenMatch tm in parser.CodeInfo.Prototypes)
            {
                TreeNode node = new TreeNode(tm.Value);
                node.Name = tm.Value;
                node.Tag = tm.Position;
                prototypes.Nodes.Add(node);
            }
            foreach (TokenMatch tm in parser.CodeInfo.Functions)
            {
                TreeNode node = new TreeNode(tm.Value);
                node.Name = tm.Value;
                node.Tag = tm.Position;
                prototypes.Nodes.Add(node);
            }
            foreach (TokenMatch tm in parser.CodeInfo.GlobalVariables)
            {
                TreeNode node = new TreeNode(tm.Value);
                node.Name = tm.Value;
                node.Tag = tm.Position;
                variables.Nodes.Add(node);
            }

            if (defines.Nodes.Count > 0) file.Nodes.Add(defines);
            if (prototypes.Nodes.Count > 0) file.Nodes.Add(prototypes);
            if (variables.Nodes.Count > 0) file.Nodes.Add(variables);

            if (file.Nodes.Count > 0) data.Nodes.Add(file);
        }

        public string LookUp (string word, string project)
        {
            string rtn = "<ul>";
            TreeNode pData = this.GetProjectData(project);
            if (pData != null)
            {
                foreach (TreeNode fileNode in pData.Nodes)
                {
                    foreach (TreeNode dataNode in fileNode.Nodes)
                    {
                        foreach (TreeNode infoNode in dataNode.Nodes)
                        {
                            if (infoNode.Text.Contains(word))
                            {
                                rtn += "<li><b><a href=\"" + fileNode.Text + "\" TITLE=\"" + fileNode.Text + "\" offset=\"" + 
                                    infoNode.Tag.ToString() + "\">" + Path.GetFileName(fileNode.Text);
                                rtn += "</a></b><ul><li>" + infoNode.Text + "</li></ul></li>";
                            }
                        }
                    }
                }
            }
            rtn += "</ul>";
            return rtn;
        }

        private TreeNode GetProjectData (string project)
        {
            foreach (TreeNode node in this.treeMain.Nodes)
            {
                cProjectInfo pInfo = (cProjectInfo)node.Tag;
                if (pInfo.Path.Equals(project))
                {
                    return pInfo.Data;
                }
            }

            return null;
        }

        public string CheckFileInProject (string filePath)
        {
            try
            {
                foreach (TreeNode node in this.treeMain.Nodes)
                {
                    cProjectInfo pInfo = (cProjectInfo)node.Tag;
                    if (pInfo.Data != null)
                    {
                        foreach (TreeNode n in pInfo.Data.Nodes["files"].Nodes)
                        {
                            if (n.Text.ToLower().Equals(n.Text.ToLower()))
                            {
                                return pInfo.Path;
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            return null;
        }

        #region -= Get Info =-

        private void GetFile(TreeNode root, XmlNode node)
        {

            TreeNode tNode = new TreeNode();
            string file = node.InnerText;
            tNode.Tag = file;
            file = Path.GetFileName(file);
            tNode.Text = tNode.Name = file;
            this.GetNodeImage(tNode);

            root.Nodes.Add(tNode);
        }

        private static void GetFolder(TreeNode root, XmlNode node)
        {
            TreeNode tNode = new TreeNode();
            string folder = node.InnerText;
            tNode.Tag = folder;
            folder = folder.Substring(folder.LastIndexOf('\\') + 1);
            tNode.Text = tNode.Name = folder;
            tNode.Nodes.Add("");

            root.Nodes.Add(tNode);
        }

        private void GetNodeImage(TreeNode node)
        {
            Image image = Common.GetFileIcon(node.Tag.ToString(), false).ToBitmap();
            int index = this.imgMain.Images.Add(image, Color.Transparent);

            node.SelectedImageIndex = node.ImageIndex = index;
        }

        #endregion

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

        public void Duplicate()
        {
        }

        public void Delete()
        {
            if (this.treeMain.SelectedNode.Parent == null)
            {
                // Delete Project...
                if (MessageBox.Show("Hierbei wird das Projekt '" + this.treeMain.SelectedNode.Text + "' gelöscht.\nSind Sie damit einverstanden?",
                    "Stampfer", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    if(File.Exists(this.treeMain.SelectedNode.Tag.ToString()))
                    {
                        File.Delete(this.treeMain.SelectedNode.Tag.ToString());
                        return;
                    }
                }
                return;
            }

            TreeNode root = this.treeMain.SelectedNode;
            while(root.Parent != null)
            {
                root = root.Parent;
            }
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(root.Tag.ToString());
            foreach (XmlNode node in xDoc.FirstChild.ChildNodes)
            {
                if (node.InnerText == this.treeMain.SelectedNode.Tag.ToString())
                {
                    xDoc.FirstChild.RemoveChild(node);

                    this.treeMain.SelectedNode.Parent.Nodes.Remove(this.treeMain.SelectedNode);
                    return;
                }
            }

            MessageBox.Show("Datei kann nicht entfernt werden.", "Stampfer", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void SelectAll()
        {
        }

        public bool CloseTab()
        {
            this.Close();
            return true;
        }

        public IPeterPluginHost Host
        {
            get {  return this.m_Host; }

            set { this.m_Host = value; }
        }

        public string FileName
        {
            get { return ""; }
        }

        public string Selection
        {
            get 
            {
                if (this.treeMain.SelectedNode != null)
                {
                    return this.treeMain.SelectedNode.Tag.ToString();
                }

                return "";
            }
        }

        public bool AbleToUndo
        {
            get { return false; }
        }

        public bool AbleToRedo
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
            get { return true; }
        }

        public bool AbleToDelete
        {
            get { return true; }
        }

        public bool NeedsSaving
        {
            get { return false; }
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

        #endregion

        #region -= Tool Bar =-

        private void tsbProperties_Click(object sender, EventArgs e)
        {
            Project prj = new Project();
            prj.SetProjectFile(this.treeMain.SelectedNode.Tag.ToString());
            prj.ShowDialog();
        }

        private void tsbAddFolder_Click(object sender, EventArgs e)
        {
            if (this.fbdMain.ShowDialog() == DialogResult.OK)
            {
                string path = this.fbdMain.SelectedPath;
                string proj = this.treeMain.SelectedNode.Tag.ToString();

                foreach (TreeNode node in this.treeMain.SelectedNode.Nodes)
                {
                    if (node.Tag.ToString() == path)
                    {
                        return;
                    }
                }

                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(proj);
                XmlNode addNode = xDoc.CreateNode(XmlNodeType.Element, "folder", null);
                addNode.InnerText = path;
                xDoc.FirstChild.AppendChild(addNode);
                xDoc.Save(proj);

                TreeNode newFolder = new TreeNode(path.Substring(path.LastIndexOf('\\') + 1));
                newFolder.Name = newFolder.Text;
                newFolder.Tag = path;
                newFolder.Nodes.Add("");
                this.treeMain.SelectedNode.Nodes.Add(newFolder);
            }
        }

        private void tsbAddFile_Click(object sender, EventArgs e)
        {
            if (this.ofdMain.ShowDialog() == DialogResult.OK)
            {
                string proj = this.treeMain.SelectedNode.Tag.ToString();
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(proj);

                foreach (string file in this.ofdMain.FileNames)
                {
                    foreach (TreeNode node in this.treeMain.SelectedNode.Nodes)
                    {
                        if (node.Tag.ToString() == file)
                        {
                            return;
                        }
                    }
                    XmlNode addNode = xDoc.CreateNode(XmlNodeType.Element, "file", null);
                    addNode.InnerText = file;
                    xDoc.FirstChild.AppendChild(addNode);

                    TreeNode newFile = new TreeNode(Path.GetFileName(file));
                    newFile.Name = newFile.Text;
                    newFile.Tag = file;
                    this.GetNodeImage(newFile);
                    this.treeMain.SelectedNode.Nodes.Add(newFile);
                }
                xDoc.Save(proj);
            }
        }

        private void tsbRefresh_Click(object sender, EventArgs e)
        {
            if (this.treeMain.SelectedNode.Parent == null)
            {
                // Refresh the whole project...
                foreach (TreeNode node in this.treeMain.SelectedNode.Nodes)
                {
                    node.Collapse();
                    node.Expand();
                }
            }
            else
            {
                // Refresh Folder...
                this.treeMain.SelectedNode.Collapse();
                this.treeMain.SelectedNode.Expand();
            }
        }

        private void tsbDelete_Click(object sender, EventArgs e)
        {
            this.Delete();
        }

        private void tsbOpenDir_Click(object sender, EventArgs e)
        {
            if (this.treeMain.SelectedNode != null)
            {
                if (this.treeMain.SelectedNode.Tag != null)
                {
                    if (this.treeMain.SelectedNode.Nodes.Count == 0)
                    {
                        System.Diagnostics.Process.Start("explorer.exe",
                            Path.GetDirectoryName(this.treeMain.SelectedNode.Tag.ToString()));
                    }
                    else
                    {
                        System.Diagnostics.Process.Start("explorer.exe", this.treeMain.SelectedNode.Tag.ToString());
                    }
                }
            }
        }

        #endregion

        #region -= Misc =-

        /// <summary>
        /// Gets or Sets the Context Menu used for the Root nodes...
        /// </summary>
        public ContextMenuStrip RootContextMenu
        {
            get { return this.m_ctxRoot; }

            set { this.m_ctxRoot = value; }
        }

        protected override string GetPersistString ()
        {
            string projs = "";
            foreach (TreeNode node in this.treeMain.Nodes)
            {
                cProjectInfo pInfo = (cProjectInfo)node.Tag;
                projs += pInfo.Path + ";";
            }

            return this.GetType().ToString() + "|" + projs.TrimEnd(new char[] { ';' });
        }

        void RemoveProject (object sender, EventArgs e)
        {
            try
            {
                cProjectInfo pInfo = (cProjectInfo)this.treeMain.SelectedNode.Tag;
                if (pInfo.Data != null)
                {
                    pInfo.Data.Nodes.Clear();
                }
                GC.Collect();
                this.treeMain.SelectedNode.Tag = null;
                this.treeMain.Nodes.Remove(this.treeMain.SelectedNode);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        #endregion
    }
}
