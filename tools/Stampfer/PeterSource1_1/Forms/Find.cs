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
using System.Data;
using System.Drawing;
using WeifenLuo.WinFormsUI.Docking;
using PeterInterface;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Collections;
using System.IO;
using System.Threading;

namespace Peter
{
    public delegate void AddTreeNodeDelegate(TreeNode node);
    public delegate void TraceDelegate(string text);

    public partial class Find : Form
    {
        private IPeterPluginHost m_Host;
        private const string OPEN_DOC = "In allen offenen Dateien";
        private const string BROWSE = "Durchsuchen...";
        private const string GOTHIC = "Im Gothic-Script-Verzeichnis...";
        private ArrayList m_FindLocations;
        private FindResults m_Results;
        private AddTreeNodeDelegate m_AddTreeNode;
        private TraceDelegate m_Trace;
        private MainForm m_MainForm;
        private Thread m_thrdFind;
        private Thread m_thrdReplace;

        struct FindInfo
        {
            public int Index;
            public int Line;
            public string FindString;
            public string FilePath;

            public FindInfo(int index, int line, string find, string file)
            {
                this.FindString = find;
                this.Line = line;
                this.Index = index;
                this.FilePath = file;
            }
        };

        public Find(MainForm main)
        {
            InitializeComponent();

            this.m_MainForm = main;
            this.m_Trace = new TraceDelegate(this.Trace);
            this.m_AddTreeNode = new AddTreeNodeDelegate(this.AddNode);
            this.m_FindLocations = new ArrayList();
            this.m_Results = new FindResults();
            this.m_Results.Tree.DoubleClick += new EventHandler(Tree_DoubleClick);
            this.cmbFindText.KeyDown += new KeyEventHandler(cmbFindText_KeyDown);
        }

        /// <summary>
        /// Gets or Sets the Host Application...
        /// </summary>
        public IPeterPluginHost Host
        {
            get { return this.m_Host; }

            set { this.m_Host = value; }
        }

        private void AddNode(TreeNode node)
        {
            string file = node.ToolTipText;
            string ext = Path.GetExtension(file);
            if (ext.Trim() == string.Empty)
            {
                ext = "none";
            }
            if (!this.m_Results.Images.Images.ContainsKey(ext))
            {
                this.m_Results.Images.Images.Add(ext, Common.GetFileIcon(file, false).ToBitmap());
            }

            node.ImageIndex = node.SelectedImageIndex = this.m_Results.Images.Images.IndexOfKey(ext);

            this.m_Results.Tree.Nodes.Add(node);
            node.Expand();

            // Update count...
            if (this.m_Results.TabText.IndexOf("-") != -1)
            {
                string[] txt = this.m_Results.TabText.Split('-');
                string matchCnt = txt[1].Trim();
                matchCnt = matchCnt.Split(' ')[0].Trim();
                matchCnt = Convert.ToString(Convert.ToInt32(matchCnt) + node.Nodes.Count);
                this.m_Results.TabText = "Suchergebnisse - " + matchCnt;
                //this.m_Results.TabText += (matchCnt.Equals("1")) ? " Ergebnis" : " Ergebnisse";
            }
            else
            {
                this.m_Results.TabText += " - " + node.Nodes.Count.ToString();
                //this.m_Results.TabText += (node.Nodes.Count == 1) ? " Ergebnis" : " Ergebnisse";
            }
        }

        private void Trace(string text)
        {
            if (text.Equals("Suchvorgang beendet."))
            {
                this.btnStop.Visible = false;
            }
            this.m_Host.Trace(text);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            base.OnClosing(e);
        }

        private void tsbFind_Click(object sender, EventArgs e)
        {
            tsbFind.Checked = true;
            tsbReplace.Checked = false;
            lblReplace.Enabled = false;
            cmbReplace.Enabled = false;
            this.btnFind.Text = "Weitersuchen";
            this.btnMark.Text = "Alles markieren";

            string text = this.cmbFindIn.SelectedText;
            this.cmbFindIn.Items.Clear();
            this.cmbFindIn.Items.Add(OPEN_DOC);
            this.cmbFindIn.Items.Add(BROWSE);
            if (this.m_MainForm.m_ScriptsPath!="")
            {
                this.cmbFindIn.Items.Add(GOTHIC);
            }
            this.cmbFindIn.Text = OPEN_DOC;
            for (int a = 0; a < this.m_FindLocations.Count; a++)
            {
                this.cmbFindIn.Items.Add(this.m_FindLocations[a]);
            }
            this.cmbFindIn.Text = text;
            if (this.cmbFindIn.Text == "")
            {
                this.cmbFindIn.Text = OPEN_DOC;
            }

            ckbFindIn_CheckedChanged(null, null);
            this.cmbFindText.Focus();
        }

        private void tsbReplace_Click(object sender, EventArgs e)
        {
            this.tsbFind.Checked = false;
            this.tsbReplace.Checked = true;
            this.lblReplace.Enabled = true;
            this.cmbReplace.Enabled = true;

            this.cmbFindIn.Items.Clear();
            this.cmbFindIn.Items.Add(OPEN_DOC);
            this.cmbFindIn.Items.Add(BROWSE);
            if (this.m_MainForm.m_ScriptsPath != "")
            {
                this.cmbFindIn.Items.Add(GOTHIC);
            }
            this.cmbFindIn.Text = OPEN_DOC;
            if (this.cmbFindIn.Text == "")
            {
                this.cmbFindIn.Text = OPEN_DOC;
            }

            this.btnFind.Text = "Weiter ersetzen";
            this.btnMark.Text = "Alles ersetzen";

            ckbFindIn_CheckedChanged(null, null);
            this.cmbReplace.Focus();
        }

        private void cmbFindText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnFind_Click(null, null);
            }
        }

        /// <summary>
        /// Gets the Regular Expression to search for...
        /// </summary>
        /// <returns>RegEx</returns>
        public Regex GetRegEx()
        {
            string regExString = this.cmbFindText.Text;

            if (this.ckbUseRegEx.Checked)
            {
                // We dont need to do anything...
            }
            else if (this.ckbWildCard.Checked)
            {
                regExString = regExString.Replace("*", @"\w*");
                regExString = regExString.Replace("?", @"\w");

                regExString = String.Format("{0}{1}{0}", @"\b", regExString);
            }
            else
            {
                regExString = Regex.Escape(regExString);
            }

            if (this.ckbMatchWord.Checked)
            {
                regExString = String.Format("{0}{1}{0}", @"\b", regExString);
            }

            try
            {

                if (this.ckbMatchCase.Checked)
                {
                    return new Regex(regExString);
                }
                else
                {
                    return new Regex(regExString, RegexOptions.IgnoreCase);
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the Find Results DockControl...
        /// </summary>
        public FindResults Results
        {
            get { return this.m_Results; }
        }

        /// <summary>
        /// Gets if we should search up or not...
        /// </summary>
        public bool FindUp
        {
            get { return this.ckbSearchUp.Checked; }
        }

        /// <summary>
        /// Gets or Sets the Text to Find...
        /// </summary>
        public string FindText
        {
            get { return this.cmbFindText.Text; }

            set { this.cmbFindText.Text = value; }
        }

        /// <summary>
        /// Gets the Text to Replace..
        /// </summary>
        public string ReplaceText
        {
            get { return this.cmbReplace.Text; }
        }

        private void ckbFindIn_CheckedChanged(object sender, EventArgs e)
        {
            if (tsbFind.Checked)
            {
                if (ckbFindIn.Checked)
                {
                    this.cmbFindIn.Enabled = true;
                    this.btnMark.Text = "Alles suchen";
                    this.btnFind.Enabled = false;
                    this.lblFilter.Enabled = true;
                    this.cmbFilter.Enabled = true;
                    this.ckbmarked.Enabled = false;
                }
                else
                {
                    this.cmbFindIn.Enabled = false;
                    this.btnMark.Text = "Alles markieren";
                    this.btnFind.Enabled = true;
                    this.lblFilter.Enabled = false;
                    this.cmbFilter.Enabled = false;
                    this.ckbmarked.Enabled = false;
                }
            }
            else
            {
                if (ckbFindIn.Checked)
                {
                    this.cmbFindIn.Enabled = true;
                    this.btnFind.Enabled = false;
                    this.lblFilter.Enabled = true;
                    this.cmbFilter.Enabled = true;
                    this.ckbmarked.Enabled = true;
                }
                else
                {
                    this.cmbFindIn.Enabled = false;
                    this.btnFind.Enabled = true;
                    this.lblFilter.Enabled = false;
                    this.cmbFilter.Enabled = false;
                    this.ckbmarked.Enabled = true;
                }
            }
        }

        public void SetFind(bool files)
        {
            this.ckbFindIn.Checked = files;
            tsbFind_Click(null, null);
        }

        public void SetReplace(bool files)
        {
            this.ckbFindIn.Checked = files;
            tsbReplace_Click(null, null);
        }

        private void UpdateComboBoxes()
        {
            if (!this.cmbFindText.Items.Contains(cmbFindText.Text))
            {
                this.cmbFindText.Items.Add(cmbFindText.Text);
            }
            if (this.cmbReplace.Enabled)
            {
                if (!this.cmbReplace.Items.Contains(this.cmbReplace.Text))
                {
                    this.cmbReplace.Items.Add(this.cmbReplace.Text);
                }
            }
            if (this.cmbFilter.Enabled)
            {
                if (!this.cmbFilter.Items.Contains(this.cmbFilter.Text))
                {
                    this.cmbFilter.Items.Add(this.cmbFilter.Text);
                }
            }
            if (this.cmbFindIn.Enabled)
            {
                if (!this.cmbFindIn.Items.Contains(this.cmbFindIn.Text))
                {
                    this.cmbFindIn.Items.Add(this.cmbFindIn.Text);
                }
                if (!this.m_FindLocations.Contains(this.cmbFindIn.Text))
                {
                    this.m_FindLocations.Add(this.cmbFindIn.Text);
                }
            }
        }

        private void cmbFindIn_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbFindIn.Text == BROWSE)
            {
                if (fbdMain.ShowDialog() == DialogResult.OK)
                {
                    if (!this.cmbFindIn.Items.Contains(fbdMain.SelectedPath))
                    {
                        this.cmbFindIn.Items.Add(fbdMain.SelectedPath);
                        this.m_FindLocations.Add(fbdMain.SelectedPath);
                    }
                    this.cmbFindIn.Text = fbdMain.SelectedPath;
                }
                else
                {
                    this.cmbFindIn.Text = OPEN_DOC;
                }
            }
            else if (this.cmbFindIn.Text == GOTHIC)
            {
                if (!this.cmbFindIn.Items.Contains(this.m_MainForm.m_ScriptsPath))
                {
                    this.cmbFindIn.Items.Add(this.m_MainForm.m_ScriptsPath);
                    this.m_FindLocations.Add(this.m_MainForm.m_ScriptsPath);
                    this.cmbFilter.Items.Add("*.d");
                }
                this.cmbFilter.Text = "*.d";
                this.cmbFindIn.Text = this.m_MainForm.m_ScriptsPath;
            }
        }
        public void ReplaceInFiles(object dirObj)
        {
            ArrayList info = (ArrayList)dirObj;
            DirectoryInfo dirInfo = new DirectoryInfo(info[1].ToString());
            string[] filters = info[2].ToString().Split('|');
            foreach (string filter in filters)
            {
                if (filter.Trim() != string.Empty)
                {
                    foreach (FileInfo file in dirInfo.GetFiles(filter.Trim(), SearchOption.AllDirectories))
                    {
                        this.ReplaceInFile(file, info[0].ToString(),info[4].ToString(), (Regex)info[3]);
                    }
                }
            }

            this.Invoke(this.m_Trace, new object[] { "Suchvorgang beendet." });
        }
        public void ReplaceInFile(FileInfo file, string searchText, string replaceText, Regex reg)
        {
            this.Invoke(this.m_Trace, new object[] { "Suche in " + file.FullName });
            StreamReader sr = new StreamReader(file.FullName,System.Text.Encoding.Default);
           
            string text1 = "";
            
           
            
            /*/while ((line = sr.ReadLine()) != null)
            {
               // Match m = reg.Match(line);
               // if (m.Success)
               // {
               //     line.Replace(m.
               // }
          
                sr.r
               // line=reg.Replace(line, replaceText);
               
            }*/
            text1 = sr.ReadToEnd();
            sr.Close();
            sr.Dispose();
            StreamWriter sw = new StreamWriter(file.FullName,false,System.Text.Encoding.Default);
            text1 = reg.Replace(text1, replaceText);
            sw.Write(text1);
            sw.Close();
            sw.Dispose();
          
            
        }
        public void FindInFiles(object dirObj)
        {
            ArrayList info = (ArrayList)dirObj;
            DirectoryInfo dirInfo = new DirectoryInfo(info[1].ToString());
            string[] filters = info[2].ToString().Split('|');
            foreach (string filter in filters)
            {
                if (filter.Trim() != string.Empty)
                {
                    foreach (FileInfo file in dirInfo.GetFiles(filter.Trim(), SearchOption.AllDirectories))
                    {
                        this.FindInFile(file, info[0].ToString(), (Regex)info[3]);
                    }
                }
            }

            this.Invoke(this.m_Trace, new object[] { "Suchvorgang beendet." });
        }

        public void FindInFile(FileInfo file, string searchText, Regex reg)
        {
            this.Invoke(this.m_Trace, new object[] { "Suche in " + file.FullName });
            StreamReader sr = new StreamReader(file.FullName, System.Text.Encoding.Default);
            string line;
            int lineNum = 1;
            TreeNode nodeRoot = new TreeNode(file.Name);
            nodeRoot.ToolTipText = file.FullName;
            while ((line = sr.ReadLine()) != null)
            {
                Match m = reg.Match(line);
                if (m.Success)
                {
                    TreeNode node = new TreeNode(lineNum.ToString() + " - " + line.Replace("\t"," ").Trim());
                    node.Tag = new FindInfo(m.Index, lineNum, searchText, file.FullName);
                    nodeRoot.Nodes.Add(node);
                }
                lineNum++;
            }
            if (nodeRoot.Nodes.Count > 0)
            {
                this.Invoke(this.m_AddTreeNode, new object[] { nodeRoot });
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            this.UpdateComboBoxes();
           
            if (this.tsbFind.Checked)
            {
                
                if (!this.ckbFindIn.Checked)
                {
                    // Find Next...
                    
                    if(this.m_MainForm.FindNext(this.FindUp))
                        this.Hide();
                }
            }
            else
            {
                if (!this.ckbFindIn.Checked)
                {
                    // Replace Next..
                   
                        this.m_MainForm.ReplaceNext();
                    
                }
            }
        }

        private void btnMark_Click(object sender, EventArgs e)
        {
            this.UpdateComboBoxes();
            if (this.tsbFind.Checked)
            {
                if (this.ckbFindIn.Checked)
                {
                    this.m_Results.Tree.Nodes.Clear();
                    this.m_Results.TabText = "Suchergebnisse";
                    if (this.m_Results.DockState == DockState.Unknown)
                    {
                        this.m_Host.AddDockContent(this.m_Results, DockState.DockBottom);
                    }
                    else if (this.m_Results.DockState == DockState.Hidden)
                    {
                        this.m_Results.Show();
                    }
                    // Find All in Files...
                    if (this.cmbFindIn.Text == OPEN_DOC)
                    {
                        this.m_MainForm.FindInOpenFiles();
                    }
                    else
                    {
                        this.m_MainForm.Trace("Erstelle Liste...");
                        ArrayList info = new ArrayList();
                        info.Add(this.cmbFindText.Text);
                        info.Add(this.cmbFindIn.Text);
                        info.Add(this.cmbFilter.Text);
                        info.Add(this.GetRegEx());
                        this.btnStop.Visible = true;

                        if(this.m_thrdFind != null)
                            if (this.m_thrdFind.IsAlive)
                                this.m_thrdFind.Abort();

                        this.m_thrdFind = null;
                        this.m_thrdFind = new Thread(new ParameterizedThreadStart(FindInFiles));
                        this.m_thrdFind.Start(info);
                    }
                }
                else
                {
                    // Mark All...
                    this.m_MainForm.MarkAll();
                }
            }
            else
            {
                if ((!this.ckbFindIn.Checked) && (!this.ckbmarked.Checked))
                {
                    // Replace All..
                    this.m_MainForm.ReplaceAll();
                }
                else if(this.ckbmarked.Checked)
                {
                    this.m_MainForm.ReplaceAllMarked();
                }
                else
                {
                    // Replace All in Files...
                    if (this.cmbFindIn.Text == OPEN_DOC)
                    {
                        this.m_MainForm.ReplaceInOpenFiles();
                    }
                    else
                    {
                      
                        this.btnStop.Visible = true;
                        ArrayList info = new ArrayList();
                        info.Add(this.cmbFindText.Text);
                        info.Add(this.cmbFindIn.Text);
                        info.Add(this.cmbFilter.Text);
                        info.Add(this.GetRegEx());
                        info.Add(this.cmbReplace.Text);
                        if (this.m_thrdReplace != null)
                            if (this.m_thrdReplace.IsAlive)
                                this.m_thrdReplace.Abort();

                        this.m_thrdReplace = null;
                        this.m_thrdReplace = new Thread(new ParameterizedThreadStart(ReplaceInFiles));
                        this.m_thrdReplace.Start(info);

                    }
                }
            }
        }

        private void Tree_DoubleClick(object sender, EventArgs e)
        {
            if (this.m_Results.Tree.SelectedNode != null
                && this.m_Results.Tree.SelectedNode.Tag != null)
            {
                FindInfo fi = (FindInfo)this.m_Results.Tree.SelectedNode.Tag;
                this.m_MainForm.CreateEditor(fi.FilePath, Path.GetFileName(fi.FilePath));
                this.m_MainForm.SelectWord(fi.Line - 1, fi.Index, fi.FindString.Length);
            }
        }

        private void btnStop_Click (object sender, EventArgs e)
        {
            if (this.m_thrdFind.IsAlive)
            {
                this.btnStop.Visible = false;
                this.m_thrdFind.Abort();
                this.m_thrdFind = null;
                Application.DoEvents();
                this.Trace("Suchvorgang abgebrochen...");
            }
        }

        private void ckbmarked_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbmarked.Checked)
            {
                this.btnFind.Enabled = false;
            }
            else
            {
                this.btnFind.Enabled = true;
            }
        }

       
    }
}