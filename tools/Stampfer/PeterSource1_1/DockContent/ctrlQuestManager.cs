/**************************************************************************************
    Stampfer - Gothic Script Editor
    Copyright (C) 2009 Alexander "Sumpfkrautjunkie" Ruppert

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
using ICSharpCode.TextEditor;
using System.IO;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;

namespace Peter
{
    public partial class ctrlQuestManager : DockContent, IPeterPluginTab
    {
        private IPeterPluginHost m_Host;
        MainForm MainF;
        private bool m_CanScroll;
      //  private TextEditorControl m_Editor;
        
        public string TEXT = "";
        public ArrayList Quests = new ArrayList();
        Hashtable hs = new Hashtable();
        ToolTip tt = new ToolTip();
        Regex ToolTipRegex = new Regex("\n");
        public ctrlQuestManager(MainForm f)
        {
           
            InitializeComponent();
            MainF = f;
            this.m_CanScroll = true;
            this.TabText = "Quest Manager";
            //this.treeMain.AfterSelect += new TreeViewEventHandler(treeMain_AfterSelect);
            this.treeMain.NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(treeMain_NodeMouseDoubleClick);
            this.treeMain.NodeMouseClick += new TreeNodeMouseClickEventHandler(treeMain_NodeMouseClick);
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.treeMain.ImageList = QuestImages;
            
            Clear();
            GetDrectories();
        }

        

        void treeMain_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node != null)
            {
                treeMain.SelectedNode = e.Node;
            }
        }

        void treeMain_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            
        }

        #region -= Helpers =-

        /// <summary>
        /// Gets or Sets if we are able to scroll to the selected item...
        /// </summary>
        public bool CanScroll
        {
            get { return this.m_CanScroll; }

            set { this.m_CanScroll = value; }
        }

        /// <summary>
        /// Scrolls the active editor to the given offset...
        /// </summary>
        /// <param name="offset">offset to start at</param>
        /// <param name="pos">offset to end at</param>
        public void ScrollToOffset (int offset, int pos)
        {

           // m_Editor.JumpToPos(offset, offset);
          
           
            
        }

        /// <summary>
        /// Clears the list view's items and groups...
        /// </summary>
        public void Clear()
        {
            try
            {
                this.Quests.Clear();
                this.treeMain.Nodes.Clear();
                
            }
            catch { }
        }

        /// <summary>
        /// Adds a node to the tree...
        /// </summary>
        /// <param name="node">Node to Add</param>
        public void AddNode(TreeNode node)
        {
            this.treeMain.Nodes.Add(node);
        }

        /// <summary>
        /// Gets the number of Items in the TreeView...
        /// </summary>
        public int ItemCount
        {
            get { return this.treeMain.Nodes.Count; }
        }

        /// <summary>
        /// Gets the Nodes for the tree...
        /// </summary>
        /// <returns>TreeNodeCollection</returns>
        public TreeNodeCollection TreeNodes()
        {
            return this.treeMain.Nodes;
        }

        /// <summary>
        /// Sets the image list of the tree...
        /// </summary>
        /// <param name="fileExt">Image list name to set...</param>
        public void SetImageList(string fileExt)
        {
            switch (fileExt)
            {
                case ".css":
                    this.treeMain.ImageList = imgCSS;
                    break;
                default:
                    this.treeMain.ImageList =QuestImages;
                    break;
            }
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

        public void Delete()
        {
        }

        public void Duplicate()
        {
        }

        public void Print()
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

        public IPeterPluginHost Host
        {
            get { return this.m_Host; }

            set { this.m_Host = value; }
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
            get { return false; }
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

        #region -= Tool Bar =-

        private void tsbExpandAll_Click(object sender, EventArgs e)
        {
            for (int a = 0; a < this.treeMain.Nodes.Count; a++)
            {
                this.treeMain.Nodes[a].ExpandAll();
            }
        }

        private void tsbCollapseAll_Click(object sender, EventArgs e)
        {
            for (int a = 0; a < this.treeMain.Nodes.Count; a++)
            {
                this.treeMain.Nodes[a].Collapse();
            }
        }

        #endregion

        
        

        private void GetDrectories()
        {
            
            string path=MainF.m_ScriptsPath+"\\Quests\\";
            this.Clear();
            if (!Directory.Exists(path)) return;
            hs.Clear();
            Quests.Clear();
            DirectoryInfo dig = new DirectoryInfo(Path.GetDirectoryName(path));
            DirectoryInfo[] directories = dig.GetDirectories();
            TreeNode trn = new TreeNode();
            trn.Text ="Quests";
            trn.ContextMenuStrip = ContextOrdner;
            trn.Tag = path;
            trn.ImageIndex = 0;
            
            treeMain.Nodes.Add(trn);
            foreach (DirectoryInfo d2 in directories)
            {
                TreeNode tn = new TreeNode();
                tn.Text = d2.Name;
                tn.ContextMenuStrip = ContextOrdner;
                tn.ToolTipText=GetDirInfo(d2);
                tn.Tag = d2.FullName;
                GetQuests(d2, tn);
                tn.ImageIndex = 2;
                tn.SelectedImageIndex = tn.ImageIndex;
                trn.Nodes.Add(tn);
                SubDirectories(d2, tn);
            }
            if (trn.Nodes.Count > 0)
                trn.Expand();
            treeMain.TreeViewNodeSorter = new QuestSort();
            treeMain.Sort();

            foreach (Quest qu in Quests)
            {
                qu.ClearArrays();
                foreach(string p in qu.PrevQuestsList)
                {
                    if (hs.Contains(p))
                    {
                        qu.PrevQuests.Add(hs[p]);
                    }
                }
                foreach (string n in qu.NextQuestsList)
                {
                    if (hs.Contains(n))
                    {
                        qu.NextQuests.Add(hs[n]);
                    }
                }
                qu.CreateTrees();
            }
            
            
           
            
        }
        
        private void GetQuests(DirectoryInfo d, TreeNode t)
        {
            FileInfo[] f = d.GetFiles("*.quest");
            if (f.Length == 0) return;
            BinaryFormatter binFormatter = new BinaryFormatter();
            FileStream fls= new FileStream(f[0].FullName, FileMode.Open);
            fls.Close();
            Quest q;
            foreach (FileInfo fi in f)
            {
                
                fls = new FileStream(fi.FullName, FileMode.Open);
                q = (Quest)binFormatter.Deserialize(fls);
                Quests.Add(q);
                q.AddToTree(t).ContextMenuStrip = QuestStrip;
                q.TPrevQuests.ContextMenuStrip = pnQuests;
                q.TNextQuests.ContextMenuStrip = pnQuests;
                q.TDialoge.ContextMenuStrip = Dias;
                q.TDiaryEntries.ContextMenuStrip = DiaryEntries;
                q.TNPCs.ContextMenuStrip = NPCs;
                q.TXP.ContextMenuStrip = XP_Menu;
                hs.Add(q.InternName, q);
               // MessageBox.Show(q.QuestTree.Parent.ToString());                
                fls.Close();               
            }
            

            fls.Dispose();
        }
        private string GetDirInfo(DirectoryInfo d)
        {
            FileInfo[] f = d.GetFiles("*.dfo");
           // MessageBox.Show(f.Length.ToString());
            if (f.Length==0) return "";
            using (StreamReader sr=new StreamReader(f[0].FullName,Encoding.Default))
            {
               
               return sr.ReadToEnd();
            }
        }
        private void SubDirectories(DirectoryInfo d,TreeNode t)
        {
            
            DirectoryInfo[] directories = d.GetDirectories();
            foreach (DirectoryInfo d2 in directories)
            {
                TreeNode tn = new TreeNode();
                tn.Text = d2.Name;
                tn.ContextMenuStrip = ContextOrdner;
                tn.ToolTipText = GetDirInfo(d2);
                tn.Tag = d2.FullName;
                tn.ImageIndex = 2;
                tn.SelectedImageIndex = tn.ImageIndex;
                GetQuests(d2,tn);
                t.Nodes.Add(tn);  
              
                SubDirectories(d2,tn);
            }
        }







        //Context Menü
        private void hoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(treeMain.SelectedNode==null) return;
            Forms.FOrdner fo = new Peter.Forms.FOrdner(treeMain.SelectedNode, this.ContextOrdner, false);
            fo.Show();
        }

        private void ordnerBearbeitenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (treeMain.SelectedNode == null) return;
            Forms.FOrdner fo = new Peter.Forms.FOrdner(treeMain.SelectedNode, this.ContextOrdner, true);
            fo.Show();
        }

        private void ordnerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(treeMain.SelectedNode==null) return;
            TreeNode t = treeMain.SelectedNode;
            if (MessageBox.Show("Möchten Sie den Questordner "+t.Tag.ToString()+" wirklick löschen?","Löschen bestätigen",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
            {
                if (Directory.Exists(t.Tag.ToString())) Directory.Delete(t.Tag.ToString(),true);
                treeMain.Nodes.Remove(t);
            }
            
        }

        private void questToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeMain.SelectedNode == null) return;
            Forms.FQuest q = new Peter.Forms.FQuest(treeMain.SelectedNode, Quests, this);
            q.Show(this);
           
        }

        private void questsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeMain.SelectedNode == null || treeMain.SelectedNode.Nodes.Count==0) return;
            TreeNode t = treeMain.SelectedNode;
            if (MessageBox.Show("Möchten Sie alle Quests im Ordner " + t.Tag.ToString() + " wirklich löschen?", "Löschen bestätigen", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                
                
                bool cont=true;
                while (cont==true)
                {
                    
                    if (t.Nodes[0].Tag is Quest)
                    {
                        RemoveQuestFile(t.Tag.ToString(), ((Quest)(t.Nodes[0].Tag)).InternName);
                        Quests.Remove(t.Nodes[0].Tag);
                        t.Nodes.Remove(t.Nodes[0]);
                        if (t.Nodes.Count == 0)
                        {
                            cont = false;
                        }
                    }
                    else
                    {
                        cont = false;
                    }
                    
                }
            }
            

        }

        private void loeschenToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (treeMain.SelectedNode == null) return;
            TreeNode t = treeMain.SelectedNode;
            Quests.Remove(t.Tag);
            RemoveQuestFile(t.Parent.Tag.ToString(),((Quest)(t.Tag)).InternName);
            t.Parent.Nodes.Remove(t);
        }
        private void RemoveQuestFile(string d, string f)
        {
            string s = d + "\\" + f + ".quest";
            if (File.Exists(s))
            {
                File.Delete(s);
            }
        }

        private void bearbeitenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeMain.SelectedNode == null) return;
            Forms.FQuestEdit fq = new Peter.Forms.FQuestEdit(treeMain.SelectedNode,Quests);
            fq.Show(this);

        }

        private void treeMain_NodeMouseHover(object sender, TreeNodeMouseHoverEventArgs e)
        {
            
            /*if (e.Node.ToolTipText == "")
            {
                tt.Hide(this);
                return;
            }*/
            //MessageBox.Show("s");
            
            string s = e.Node.ToolTipText;
            string s2 = "";
            int m = ToolTipRegex.Match(s).Index;
            if (m > 0)
            {
                s2 = s.Substring(m);
                s = s.Substring(0, m);
            }
            else
            {
                s2 = s;
                s = "";
            }
            tt.ToolTipTitle = s;
            tt.Show(s2, this, e.Node.Bounds.Left-16, e.Node.Bounds.Bottom + e.Node.Bounds.Height * 3);
            //tt.Hide(this);
            //tt.Show(s2, this, 0, e.Node.TreeView.Height);
            

        }

        private void treeMain_MouseLeave(object sender, EventArgs e)
        {
            tt.Hide(this);
        }

        private void treeMain_NodeMouseDoubleClick_1(object sender, TreeNodeMouseClickEventArgs e)
        {
            
            if ((e.Node.SelectedImageIndex == 3 || e.Node.SelectedImageIndex == 4)
                &&e.Node.Tag!=null)
            {
                
                Quest q=((Quest)e.Node.Tag);
                //MessageBox.Show(q.QuestTree.Parent.Text);

                treeMain.SelectedNode = q.QuestTree;
                q.QuestTree.Expand();
                q.QuestTree.EnsureVisible();
            }
            else if (e.Node.SelectedImageIndex == 8)
            {
               string file = (e.Node.Tag.ToString());
               if (File.Exists(file))
               {
                   this.MainF.CreateEditor(file, Path.GetFileName(file));
                   this.MainF.FindText(new Regex(@"\s" + e.Node.Text, RegexOptions.IgnoreCase), true);
               }
            }
        }

        private void BTParse_Click(object sender, EventArgs e)
        {
            Clear();
            GetDrectories();
            treeMain.Sort();
        }

        private void bearbeitenToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (treeMain.SelectedNode == null) return;
            Forms.FAddQuest aq = new Forms.FAddQuest(treeMain.SelectedNode, Quests);
            aq.Show(this);

        }

        private void bearbeitenToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            if (treeMain.SelectedNode == null) return;
            Forms.FAddQuest aq = new Forms.FAddQuest(treeMain.SelectedNode, MainF);
            aq.Show(this);
        }
       

        

        

        

        
    }
    [Serializable()]
    public class Quest
    {
        
        public string Name;
        public string InternName;
        public string Title;
        public string Description;
        public ArrayList PrevQuestsList=new ArrayList();
        public ArrayList NextQuestsList = new ArrayList();
        public ArrayList PrevQuests = new ArrayList();
        public ArrayList NextQuests = new ArrayList();
        public ArrayList XP = new ArrayList();
        public ArrayList DiaryEntries = new ArrayList();
        public ArrayList StartDialoge = new ArrayList();
        public ArrayList Dialoge = new ArrayList();
        public ArrayList EndDialoge = new ArrayList();
        public ArrayList NPCs = new ArrayList();
        public TreeNode QuestTree=new TreeNode();
        public TreeNode TPrevQuests=new TreeNode("Beeinflusst von...");
        public TreeNode TNextQuests = new TreeNode("Beeinflusst...");
        public TreeNode TDialoge = new TreeNode("Dialoge");
        public TreeNode TXP = new TreeNode("Erfahrungspunkte");
        public TreeNode TDiaryEntries = new TreeNode("Tagebucheinträge");
        public TreeNode TNPCs = new TreeNode("NPCs");        
        public Quest()
        {
        }
        public Quest(string n, string i, string t, string d,
            Quest[] pq, Quest[] nq, object[] x/*, ArrayList de,
            ArrayList sd, ArrayList di, ArrayList ed, ArrayList npc*/
                                                                     )
        {
            
            Name = n;
            InternName = i;
            Title = t;
            Description = d;

            foreach(Quest q in pq)
            {
                PrevQuestsList.Add(q.InternName);
                PrevQuests.Add(q);
            }
            foreach (Quest q in nq)
            {
                NextQuestsList.Add(q.InternName);
                NextQuests.Add(q);
            }
            foreach (object q in x)
            {
                
                XP.Add(Convert.ToInt32(q));
            }
            /*DiaryEntries = de;
            StartDialoge = sd;
            Dialoge = di;
            EndDialoge = ed;
            NPCs = npc;*/
            CreateTrees();
        }
        public Quest(string n, string i, string t, string d)
        {
            
            Name = n;
            InternName = i;
            Title = t;
            Description = d;            
           // CreateTrees();
        }
        public void ClearArrays()
        {
            PrevQuests.Clear();
            NextQuests.Clear();
        }
        private void ClearTrees()
        {
            TPrevQuests.Nodes.Clear();
            TNextQuests.Nodes.Clear();
            TDialoge.Nodes.Clear();
            TXP.Nodes.Clear();
            TDiaryEntries.Nodes.Clear();
            TNPCs.Nodes.Clear();            
        }
        public void CreateTrees()
        {
            ClearTrees();
            
            if (PrevQuests != null)
            {
                foreach (Quest pq in PrevQuests)
                {
                    TreeNode t = new TreeNode(pq.Name);
                    
                    t.Tag = pq;
                    t.ToolTipText = pq.InternName;
                    t.ImageIndex = 3;
                    t.SelectedImageIndex = t.ImageIndex;
                    TPrevQuests.Nodes.Add(t);
                }
            }
            if (NextQuests != null)
            {
                foreach (Quest nq in NextQuests)
                {
                    TreeNode t = new TreeNode(nq.Name);
                    t.Tag = nq;
                    t.ToolTipText = nq.InternName;
                    t.ImageIndex = 4;
                    t.SelectedImageIndex = t.ImageIndex;
                    TNextQuests.Nodes.Add(t);
                }
            }
            if (StartDialoge != null)
            {
                foreach (Instance i in StartDialoge)
                {
                    TreeNode t = new TreeNode(i.Name);
                    t.Tag = i.File;
                    t.ImageIndex = 5;
                    t.SelectedImageIndex = t.ImageIndex;
                    TDialoge.Nodes.Add(t);
                }
            }
            if (Dialoge != null)
            {
                foreach (Instance i in Dialoge)
                {
                    TreeNode t = new TreeNode(i.Name);
                    t.Tag = i.File;
                    t.ImageIndex = 6;
                    t.SelectedImageIndex = t.ImageIndex;
                    TDialoge.Nodes.Add(t);
                }
            }
            if (EndDialoge != null)
            {
                foreach (Instance i in EndDialoge)
                {
                    TreeNode t = new TreeNode(i.Name);
                    t.Tag = i.File;
                    t.ImageIndex = 7;
                    t.SelectedImageIndex = t.ImageIndex;
                    TDialoge.Nodes.Add(t);
                }
            }
            if (NPCs != null)
            {
                foreach (Instance i in NPCs)
                {
                    TreeNode t = new TreeNode(i.Name);
                    t.Tag = i.File;
                    t.ImageIndex = 8;
                    t.SelectedImageIndex = t.ImageIndex;
                    TNPCs.Nodes.Add(t);
                }
            }
            if (XP != null)
            {
                foreach (int i in XP)
                {
                    TreeNode t = new TreeNode(i.ToString());
                    t.ImageIndex = 10;
                    t.SelectedImageIndex = t.ImageIndex;
                    TXP.Nodes.Add(t);
                }
            }

            if (DiaryEntries != null)
            {
                foreach (string i in DiaryEntries)
                {
                    TreeNode t = new TreeNode(i);
                    t.ImageIndex = 9;
                    t.SelectedImageIndex = t.ImageIndex;
                    TDiaryEntries.Nodes.Add(t);
                }
            }
            //AddToTree(MainTree);
           
        }
        public override string ToString()
        {
            return Name;
        }
        public TreeNode  AddToTree(TreeNode t)
        {
            QuestTree = new TreeNode(Name);
            QuestTree.ToolTipText = InternName +"\n"+ Title+"\n\n" + Description;
            QuestTree.Tag = this;

           /* TPrevQuests=new TreeNode("Beeinflusst von...");
            TNextQuests = new TreeNode("Beeinflusst...");
        TDialoge = new TreeNode("Dialoge");
        TXP = new TreeNode("Erfahrungspunkte");
        TDiaryEntries = new TreeNode("Tagebucheinträge");
        TNPCs = new TreeNode("NPCs");   */
            QuestTree.ImageIndex = 1;
            QuestTree.SelectedImageIndex = QuestTree.ImageIndex;
            TPrevQuests.ImageIndex = 3;
            TPrevQuests.SelectedImageIndex = TPrevQuests.ImageIndex;
            TNextQuests.ImageIndex = 4;
            TNextQuests.SelectedImageIndex = TNextQuests.ImageIndex;
            TDialoge.ImageIndex = 6;
            TDialoge.SelectedImageIndex = TDialoge.ImageIndex;
            TNPCs.ImageIndex = 8;
            TNPCs.SelectedImageIndex = TNPCs.ImageIndex;
            TDiaryEntries.ImageIndex = 9;
            TDiaryEntries.SelectedImageIndex = TDiaryEntries.ImageIndex;
            TXP.ImageIndex = 10;
            TXP.SelectedImageIndex = TXP.ImageIndex;    
        
            QuestTree.Nodes.Add(TPrevQuests);
            QuestTree.Nodes.Add(TNextQuests);
            QuestTree.Nodes.Add(TDialoge);
            QuestTree.Nodes.Add(TNPCs);
            QuestTree.Nodes.Add(TXP);
            QuestTree.Nodes.Add(TDiaryEntries);
            TDiaryEntries.ToolTipText = Title;

            t.Nodes.Add(QuestTree);            
            return QuestTree;
        }
    }
    public class QuestSort : IComparer
    {

        public QuestSort()
        {

        }
        int IComparer.Compare(object x, object y)
        {
            if (x == null && y == null) return 0;
            else if (x == null) return -1;
            else if (y == null) return 1;
            if (x == y) return 0;

            TreeNode f = x as TreeNode;

            TreeNode g = y as TreeNode;


            return f.ImageIndex - g.ImageIndex; ;
        }
    }
    
}
