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
using System.Text.RegularExpressions;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;

namespace Peter
{
    public partial class ctrlGothicInstances : DockContent, IPeterPluginTab
    {
  
        
        private IPeterPluginHost m_Host;
        private MainForm MainF;
        private  Regex NoSpaces=new Regex(@"  ");
        private bool readfromfile=false;
        private bool m_CanScroll;
      //  private TextEditorControl m_Editor;
       
        private string ScriptsPath;

        private TreeNode DialogTree=new TreeNode("Dialoge");
        private TreeNode NPCTree = new TreeNode("NPCs");
        private TreeNode ItemTree = new TreeNode("Items");

        private TreeNode FuncTree = new TreeNode("Funktionen");
        private TreeNode VarTree = new TreeNode("Variablen");
        public TreeNode ConstTree = new TreeNode("Konstanten");


        public Dictionary<string, Instance> FuncList = new Dictionary<string, Instance>();
        public Dictionary<string, Instance> VarList = new Dictionary<string, Instance>();
        public Dictionary<string, Instance> ConstList = new Dictionary<string, Instance>();
        public Dictionary<string, Instance> ItemList = new Dictionary<string, Instance>();
        public Dictionary<string, Instance> NPCList = new Dictionary<string, Instance>();
        public Dictionary<string, Instance> DialogList = new Dictionary<string, Instance>();

        const int DIALOGIMG = 0;
        const int NPCIMG = 1;
        const int ITEMIMG = 2;
        const int FUNCIMG = 3;
        const int VARIMG = 4;
        const int CONSTIMG = 5;
        

        public ctrlGothicInstances(MainForm m)
        {
            
            this.MainF = m;  
            InitializeComponent();
            this.ScriptsPath = m.m_ScriptsPath;
            this.m_CanScroll = true;
            this.treeMain.ImageList = m.ImgList;    
            this.TabText = "Gothic Bezeichner";
            this.treeMain.NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(treeMain_NodeMouseDoubleClick);
           if (this.ScriptsPath != "")
            {
                this.treeMain.BeginUpdate();
                ReadInstancesFromFile();
                
                this.treeMain.EndUpdate();
            }
            
            DialogTree.ImageIndex = DIALOGIMG;
            DialogTree.SelectedImageIndex = DialogTree.ImageIndex;
            

            NPCTree.ImageIndex = NPCIMG;
            NPCTree.SelectedImageIndex = NPCTree.ImageIndex;
            

            ItemTree.ImageIndex = ITEMIMG;
            ItemTree.SelectedImageIndex = ItemTree.ImageIndex;
            

            FuncTree.ImageIndex = FUNCIMG;
            FuncTree.SelectedImageIndex = FuncTree.ImageIndex;
           

            VarTree.ImageIndex = VARIMG;
            VarTree.SelectedImageIndex = VarTree.ImageIndex;
           

            ConstTree.ImageIndex = CONSTIMG;
            ConstTree.SelectedImageIndex = ConstTree.ImageIndex;

            DialogTree.Nodes.Add("fakenode");
            NPCTree.Nodes.Add("fakenode");
            ItemTree.Nodes.Add("fakenode");
            FuncTree.Nodes.Add("fakenode");
            VarTree.Nodes.Add("fakenode");
            ConstTree.Nodes.Add("fakenode");
            treeMain.Nodes.Add(DialogTree);
            treeMain.Nodes.Add(NPCTree);
            treeMain.Nodes.Add(ItemTree);
            treeMain.Nodes.Add(FuncTree);
            treeMain.Nodes.Add(VarTree);
            treeMain.Nodes.Add(ConstTree);
            treeMain.BeforeExpand += new TreeViewCancelEventHandler(treeMain_BeforeExpand);
            treeMain.BeforeCollapse += new TreeViewCancelEventHandler(treeMain_BeforeCollapse);
            //treeMain.TreeViewNodeSorter = new InstanceNodeSorter();
        }

        void treeMain_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            treeMain.BeginUpdate();
            if (e.Node.ImageIndex == DIALOGIMG)//Dialogtree
            {
                DialogTree.Nodes.Clear();
                DialogTree.Nodes.Add("fakenode");
            }
            else if (e.Node.ImageIndex == NPCIMG)//Dialogtree
            {
                NPCTree.Nodes.Clear();
                NPCTree.Nodes.Add("fakenode");
            }
            else if (e.Node.ImageIndex == ITEMIMG)//Dialogtree
            {
                ItemTree.Nodes.Clear();
                ItemTree.Nodes.Add("fakenode");
            }
            else if (e.Node.ImageIndex == FUNCIMG)//Dialogtree
            {
                FuncTree.Nodes.Clear();
                FuncTree.Nodes.Add("fakenode");
            }
            else if (e.Node.ImageIndex == VARIMG)//Dialogtree
            {
                VarTree.Nodes.Clear();
                VarTree.Nodes.Add("fakenode");
            }
            else if (e.Node.ImageIndex == CONSTIMG)//Dialogtree
            {
                ConstTree.Nodes.Clear();
                ConstTree.Nodes.Add("fakenode");
            }
             
            treeMain.EndUpdate();
        }

        void treeMain_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            treeMain.BeginUpdate();
            if (e.Node.ImageIndex == DIALOGIMG)//Dialogtree
            {
                if (DialogList.Count == 0) {treeMain.EndUpdate(); return;}
                UpdateDialogTree();
            }
            else if (e.Node.ImageIndex == NPCIMG)//Dialogtree
            {
                if (NPCList.Count == 0) { treeMain.EndUpdate(); return; }
                UpdateNPCTree();
            }
            else if (e.Node.ImageIndex == ITEMIMG)//Dialogtree
            {
                if (ItemList.Count == 0) { treeMain.EndUpdate(); return; }
                UpdateItemTree();
            }
            else if (e.Node.ImageIndex == FUNCIMG)//Dialogtree
            {
                if (FuncList.Count == 0) { treeMain.EndUpdate(); return; }
                UpdateFuncTree();
            }
            else if (e.Node.ImageIndex == VARIMG)//Dialogtree
            {
                if (VarList.Count == 0) { treeMain.EndUpdate(); return; }
                UpdateVarTree();
            }
            else if (e.Node.ImageIndex == CONSTIMG)//Dialogtree
            {
                if (ConstList.Count == 0) { treeMain.EndUpdate(); return; }
                UpdateConstTree();
            }
            
            treeMain.EndUpdate();
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
        /// After an item is selected, scroll to it...
        /// </summary>
        /// <param name="sender">TreeView</param>
        /// <param name="e">TreeViewEventArgs</param>

        void treeMain_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            
                if (this.m_CanScroll)
                {
                    if (e.Node.Parent != null)
                    {
                        if (e.Node.Tag != null)
                        {
                            string file = (e.Node.Tag.ToString());
                            OpenFile(file, e.Node.Text);


                        }
                    }
                }
            
        }
        public void OpenFile(string file, string txt)
        {
            if (File.Exists(file))
            {

                string searchstring = "";
                

                this.MainF.CreateEditor(file, Path.GetFileName(file));
                foreach (char c in txt)
                {

                    if (c == '=' || c == '(')
                    {
                        break;
                    }
                    searchstring += c.ToString();

                }
                if (txt.ToLower().StartsWith("void"))
                {
                    this.MainF.FindText(new Regex(@"void(\s)*" + RemoveType(ref searchstring, ref file), RegexOptions.IgnoreCase), true);
                }
                else if (txt.ToLower().StartsWith("int"))
                {
                    this.MainF.FindText(new Regex(@"int(\s)*" + RemoveType(ref searchstring, ref file), RegexOptions.IgnoreCase), true);
                }
                else if (txt.ToLower().StartsWith("string"))
                {
                    this.MainF.FindText(new Regex(@"string(\s)*" + RemoveType(ref searchstring, ref file), RegexOptions.IgnoreCase), true);
                }
                else if (txt.ToLower().StartsWith("c_npc"))
                {
                    this.MainF.FindText(new Regex(@"c_npc(\s)*" + RemoveType(ref searchstring, ref file), RegexOptions.IgnoreCase), true);
                }
                else if (txt.ToLower().StartsWith("c_item"))
                {
                    this.MainF.FindText(new Regex(@"c_item(\s)*" + RemoveType(ref searchstring, ref file), RegexOptions.IgnoreCase), true);
                }
                else if (txt.ToLower().StartsWith("float"))
                {
                    this.MainF.FindText(new Regex(@"float(\s)*" + RemoveType(ref searchstring, ref file), RegexOptions.IgnoreCase), true);
                }
                else
                {
                    this.MainF.FindText(new Regex(@"\s" + RemoveType(ref searchstring, ref file), RegexOptions.IgnoreCase), true);
                }

            }
        }

        /// <summary>
        /// Occurs when the active content is changed...
        /// </summary>
        /// <param name="content">New Content</param>
       // public void ActiveContentChanged (IDockContent content)
        public void ActiveContentChanged2(Editor e)
        {
            //
            

          //  DockContent d = (DockContent)content;
          //  TextEditorControl edit = null;
           /* foreach (Control c in d.Controls)
            {
              

                if (c.GetType().ToString().Equals(typeof(TextEditorControl).ToString()))
                {
                   
                    edit = (TextEditorControl)c;
                    break;
                }
            }*/
            //if (edit != this.m_Editor)
            
         
           
        }

        /// <summary>
        /// Scrolls the active editor to the given offset...
        /// </summary>
        /// <param name="offset">offset to start at</param>
        /// <param name="pos">offset to end at</param>
        public void ScrollToOffset (int offset, int pos)
        {

            //m_Editor.JumpToPos(offset, pos);
           
           
            
        }

        /// <summary>
        /// Clears the list view's items and groups...
        /// </summary>
        public void Clear()
        {
            try
            {
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
            this.treeMain.BeginUpdate();
            for (int a = 0; a < this.treeMain.Nodes.Count; a++)
            {
                this.treeMain.Nodes[a].ExpandAll();
            }
            this.treeMain.EndUpdate();
        }

        private void tsbCollapseAll_Click(object sender, EventArgs e)
        {
            this.treeMain.BeginUpdate();
            for (int a = 0; a < this.treeMain.Nodes.Count; a++)
            {
                this.treeMain.Nodes[a].Collapse();
            }
            this.treeMain.EndUpdate();
        }

        #endregion

        public void BeginTreeUpdate()
        {
            treeMain.BeginUpdate();
        }
        public void EndTreeUpdate()
        {
            treeMain.EndUpdate();
        }
        private void ClearArrays()
        {
            //this.treeMain.Nodes.Clear();
            this.DialogTree.Nodes.Clear();
            this.NPCTree.Nodes.Clear();
            this.ItemTree.Nodes.Clear();
            this.FuncTree.Nodes.Clear();
            this.VarTree.Nodes.Clear();
            this.ConstTree.Nodes.Clear();
            ItemList.Clear();
            DialogList.Clear();
            NPCList.Clear();
            FuncList.Clear();
            VarList.Clear();
            ConstList.Clear();
        }

        public void UpdateAllTrees(int mode)
        {
            if (mode > 0)
            {
                treeMain.BeginUpdate();
                if ((mode & 32) == 32)
                {
                    UpdateItemTree();
                }

                if ((mode & 16) == 16)
                    UpdateNPCTree();

                if ((mode & 8) == 8)
                    UpdateDialogTree();

                if ((mode & 2) == 2)
                    UpdateVarTree();

                if ((mode & 4) == 4)
                    UpdateFuncTree();

                if ((mode & 1) == 1)
                    UpdateConstTree();
                treeMain.EndUpdate();
                
                SetAutoCompleteContent();
                this.MainF.Trace("Gothic-Bezeichner wurden erfolgreich aktualisert.");
            }
        }

        public void AddToItemTree(string name, string path, int mode)
        {
            if ((mode & 32) == 32)
            {
                TreeNode node = new TreeNode(RemoveDoubleSpaces(name));
                node.ImageIndex = ITEMIMG;
                node.SelectedImageIndex = node.ImageIndex;
                node.Tag = path;
                ItemTree.Nodes.Add(node);
               
            }
        }
        public void AddToNPCTree(string name, string path, int mode)
        {
            if ((mode & 16) == 16)
            {
                TreeNode node = new TreeNode(RemoveDoubleSpaces(name));
                node.ImageIndex = NPCIMG;
                node.SelectedImageIndex = node.ImageIndex;
                node.Tag = path;
                NPCTree.Nodes.Add(node);

            }
        }
        public void AddToDialogTree(string name, string path, int mode)
        {
            if ((mode & 8) == 8)
            {
                TreeNode node = new TreeNode(RemoveDoubleSpaces(name));
                node.ImageIndex = DIALOGIMG;
                node.SelectedImageIndex = node.ImageIndex;
                node.Tag = path;
                DialogTree.Nodes.Add(node);

            }
        }
        public void AddToFuncTree(string name, string path, int mode)
        {
            if ((mode & 4) == 4)
            {
                TreeNode node = new TreeNode(RemoveDoubleSpaces(name));
                node.ImageIndex = FUNCIMG;
                node.SelectedImageIndex = node.ImageIndex;
                node.Tag = path;
                FuncTree.Nodes.Add(node);

            }
        }
        public void AddToVarTree(string name, string path, int mode)
        {
            if ((mode & 2) == 2)
            {
                
                TreeNode node = new TreeNode(RemoveDoubleSpaces(name));
                node.ImageIndex = VARIMG;
                node.SelectedImageIndex = node.ImageIndex;
                node.Tag = path;
                VarTree.Nodes.Add(node);

            }
        }
        public void AddToConstTree(string name, string path, int mode)
        {
            if ((mode & 1) == 32)
            {
                
                TreeNode node = new TreeNode(RemoveDoubleSpaces(name));
                node.ImageIndex = CONSTIMG;
                node.SelectedImageIndex = node.ImageIndex;
                node.Tag = path;
                ConstTree.Nodes.Add(node);
            }
        }


        public int GetAll(string path)
        {
            
            return GetDias(path) | GetFuncs(path) | GetItems(path) | GetNPCs(path);
           
        }
        Regex r = new Regex(@"((^)|(\s))instance ", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        Regex r2 = new Regex(@"((^)|(\s))func ", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        Regex r3 = new Regex(@"((^)|(\s))var ", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        Regex r4 = new Regex(@"((^)|(\s))const ", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        const int SB_LENGTH=256;
        public int GetItems(string path)
        {
            StreamReader sr = new StreamReader(path, Encoding.Default);
            string s = sr.ReadToEnd();
            MatchCollection m = r.Matches(s);
            StringBuilder sb1;
            int i = 0;
            int k = 0;
            foreach (Match match in m)
            {
                sb1 = new StringBuilder(SB_LENGTH);
                i = match.Index + match.Length;
                while ((i < s.Length) && (s[i] != '(') && (s[i] != ' ') && (s[i] != '\t'))
                {


                    sb1.Append(s[i]);

                    i++;
                }
                k|=AddItem(sb1.ToString(), path);
            }
            sr.Close();
            sr.Dispose();
            
            return k;
        }
        public int AddItem(string sb1, string path)
        {
            if (sb1.Length > 0
                    && !ItemList.ContainsKey(sb1.ToString()))
            {
                
                ItemList.Add(sb1, new Instance(sb1.ToString(), path));
                return 32;
            }
            return 0;
        }
        public int GetNPCs(string path)
        {
            StreamReader sr = new StreamReader(path, Encoding.Default);
            string s = sr.ReadToEnd();
            MatchCollection m = r.Matches(s);
            StringBuilder sb1;
            int i = 0;
            int k = 0;
            foreach (Match match in m)
            {
                sb1 = new StringBuilder(SB_LENGTH);
                i = match.Index + match.Length;
                while ((i < s.Length) && (s[i] != '(') && (s[i] != ' ') && (s[i] != '\t'))
                {


                    sb1.Append(s[i]);

                    i++;
                }
                k |= AddNPC(sb1.ToString(), path);
            }
            sr.Close();
            sr.Dispose();
            
            return k;
        }
        public int AddNPC(string sb1, string path)
        {
            if (sb1.Length > 0
                    && !NPCList.ContainsKey(sb1.ToString()))
            {

                NPCList.Add(sb1, new Instance(sb1.ToString(), path));
                return 16;
            }
            return 0;
        }
        public int GetDias(string path)
        {
            StreamReader sr = new StreamReader(path, Encoding.Default);
            string s = sr.ReadToEnd();
            MatchCollection m = r.Matches(s);
            StringBuilder sb1;
            int i = 0;
            int k=0;
            foreach (Match match in m)
            {
                sb1 = new StringBuilder(SB_LENGTH);
                i = match.Index + match.Length;
                while ((i < s.Length) && (s[i] != '(') && (s[i] != ' ') && (s[i] != '\t'))
                {


                    sb1.Append(s[i]);

                    i++;
                }
                k |= AddDia(sb1.ToString(), path);
            }
            sr.Close();
            sr.Dispose();
            
            return k;
        }
        public int AddDia(string sb1, string path)
        {
            if (sb1.Length > 0
                    && !DialogList.ContainsKey(sb1.ToString()))
            {

                DialogList.Add(sb1, new Instance(sb1.ToString(), path));
                return 8;
            }
            return 0;
        }
        public int GetFuncs(string path)
        {
            StreamReader sr = new StreamReader(path, Encoding.Default);
            bool externals = Path.GetFileName(path).ToLower()=="externals.d";
            string s = sr.ReadToEnd();
            MatchCollection m;
            StringBuilder sb1;
            StringBuilder sb2;
            int i = 0;
            int k = 0;
            
            
                
                m = r2.Matches(s);
                

                foreach (Match match in m)
                {

                    sb1 = new StringBuilder(SB_LENGTH*2);
                    sb2 = new StringBuilder(SB_LENGTH*2);
                    i = match.Index + match.Length;
                    while (s[i] != '{' && s[i] != '\n' && s[i] != '/')
                    {

                        if (s[i] == '(')
                        {
                            sb1.Append(" "+s[i]);
                            i++;
                            continue;
                        }
                        if (s[i] == ',')
                        {
                            sb1.Append(s[i] + " ");
                            i++;
                            continue;
                        }
                       
                        sb1.Append(s[i]);
                        if (s[i] == ')')
                            break;
                        i++;
                    }                   
                    k |= AddFunc(sb1.ToString(), path);
                                           
                }

                if (externals) return k;
                
                m = r3.Matches(s);
                foreach (Match match in m)
                {

                    sb1 = new StringBuilder(512);

                    i = match.Index + match.Length;
                    while (s[i] != '\n' && s[i] != ')')
                    {


                        

                        if (s[i] == ';')
                            break;
                        sb1.Append(s[i]);

                        i++;
                    }
                    
                        k|=AddVar(sb1.ToString(), path);
                    
                }
                
                m = r4.Matches(s);
                foreach (Match match in m)
                {

                    sb1 = new StringBuilder(512);

                    i = match.Index + match.Length;
                    while (s[i] != ';' && s[i] != '\n')
                    {

                        if (s[i] == '=')
                        {
                            sb1.Append(" "+s[i]+" ");
                            i++;
                            continue;
                        }
                        sb1.Append(s[i]);
                        i++;
                    }
                    
                        k |= AddConst(sb1.ToString(), path);
                    
                }
                
            
            sr.Close();
            sr.Dispose();
            return k;
        }
        public int AddFunc(string sb1, string path)
        {
            if (sb1.Length > 0)
            {
                        string tempstring = sb1.Trim().Replace('\t', ' ');
                        string tempstring2;
                        string temp="";
                        if (tempstring.Length == 0) return 0;
                        tempstring = RemoveDoubleSpaces(tempstring);
                        try
                        {
                            int y=tempstring.IndexOf(" ");
                            if (y > 0)
                            {
                                temp = tempstring.Substring(0, y);
                                tempstring = temp.ToLower() + tempstring.Substring(y);
                            }
                        }catch { }
                        tempstring2 = tempstring.ToLower();
                        if (tempstring2.StartsWith("void") || tempstring2.StartsWith("int") || tempstring2.StartsWith("c_npc") || tempstring2.StartsWith("c_item") || tempstring2.StartsWith("string"))
                        {
                            
                            if( !FuncList.ContainsKey(tempstring))
                            {

                                FuncList.Add(tempstring, new Instance(tempstring, path));
                                return 4;
                            }
                        }
            }
            return 0;
        }
        public int AddVar(string sb1, string path)
        {
            if (sb1.Length > 0)
            {

                string tempstring = sb1.Trim().Replace('\t', ' ');
                string temp;
                if (tempstring.Length == 0) return 0;
                tempstring = RemoveDoubleSpaces(tempstring);
                try
                {
                    int y = tempstring.IndexOf(" ");
                    if (y > 0)
                    {
                        temp = tempstring.Substring(0, y);
                        tempstring = temp.ToLower() + tempstring.Substring(y);
                    }
                }
                catch { }

                if (!VarList.ContainsKey(tempstring))
                {

                    VarList.Add(tempstring, new Instance(tempstring, path));
                    return 2;
                }
            }
            return 0;
        }
        public int AddConst(string sb1, string path)
        {
            if (sb1.Length > 0)
            {


                string tempstring = sb1.Trim().Replace('\t', ' ');
                string temp;
                if (tempstring.Length == 0) return 0;
                tempstring = RemoveDoubleSpaces(tempstring);
                try
                {
                    int y = tempstring.IndexOf(" ");
                    if (y > 0)
                    {
                        temp = tempstring.Substring(0, y);
                        tempstring = temp.ToLower() + tempstring.Substring(y);
                    }
                }
                catch { }
                if (!ConstList.ContainsKey(tempstring))
                {

                    ConstList.Add(tempstring, new Instance(tempstring, path));
                    return 1;
                }
            }
            return 0;
        }
        private void GetInstancesToFile(bool d, bool np, bool it, bool fu)
        {
            //ClearArrays();
            //this.treeMain.Nodes.Clear();
            //::treeMain.BeginUpdate();
            #region Treeclear
            if (d)
            {
                DialogTree.Collapse();
                DialogList.Clear();

            }
           
            if (np)
            {
                NPCTree.Collapse();
                NPCList.Clear();
            }
           
            if (it)
            {
                ItemTree.Collapse();
                ItemList.Clear();
            }
            
            if (fu)
            {
                FuncTree.Collapse();
                VarTree.Collapse();
                ConstTree.Collapse();
                FuncList.Clear();
                VarList.Clear();
                ConstList.Clear();

            }
            #endregion



            this.MainF.Trace("Gothic-Bezeichner werden aktualisert (kann einige Sekunden dauern).");
                DirectoryInfo dig;// = new DirectoryInfo(Path.GetDirectoryName(this.ScriptsPath + @"\Content\Items\"));
                FileInfo[] rgFilesg;// = dig.GetFiles("*.d", SearchOption.AllDirectories);
                




           if (it)
           {
                dig = new DirectoryInfo(Path.GetDirectoryName(this.ScriptsPath + FilePaths.ContentItems));
                rgFilesg = dig.GetFiles("*.d", SearchOption.AllDirectories);
                    if (rgFilesg.Length > 0)
                    {
                        
                        //StreamReader sr = new StreamReader(rgFilesg[0].FullName, Encoding.Default);
                        //i = 0;
                        
                        foreach (FileInfo fi in rgFilesg)
                        {
                            
                            GetItems(fi.FullName);
                            
                        }

                        //::UpdateItemTree();
                        //DialogTree=new TreeNode("Dialoge");

                        //::this.AddNode(this.ItemTree);
                }
            }
            if (np)
            {
                dig = new DirectoryInfo(Path.GetDirectoryName(this.ScriptsPath + FilePaths.ContentNPC));
                rgFilesg = dig.GetFiles("*.d", SearchOption.AllDirectories);
                if (rgFilesg.Length > 0)
                {


                    
                    
                    foreach (FileInfo fi in rgFilesg)
                    {
                        
                        GetNPCs(fi.FullName);
                        
                    }


                    //::UpdateNPCTree();
                    //DialogTree=new TreeNode("Dialoge");
                    //::this.AddNode(this.NPCTree);
                }
            }
            if (d)
            {
                
                dig = new DirectoryInfo(Path.GetDirectoryName(this.ScriptsPath + FilePaths.ContentDialoge));
                rgFilesg = dig.GetFiles("*.d", SearchOption.AllDirectories);
                if (rgFilesg.Length > 0)
                {
                    
                    foreach (FileInfo fi in rgFilesg)
                    {
                        
                        GetDias(fi.FullName);
                        
                    }


                    //::UpdateDialogTree();

                    //::this.AddNode(this.DialogTree);
                }
               
            }


            if (fu)
            {


                dig = new DirectoryInfo(Path.GetDirectoryName(this.ScriptsPath + FilePaths.ContentDir));
                rgFilesg = dig.GetFiles("*.d", SearchOption.AllDirectories);
                if (rgFilesg.Length > 0)
                {

                    foreach (FileInfo fi in rgFilesg)
                    {
                        
                        GetFuncs(fi.FullName);                        
                    }



                    //::UpdateFuncTree();
                    //::UpdateVarTree();
                    //::UpdateConstTree();

                    //::this.AddNode(this.FuncTree);
                    //::this.AddNode(this.VarTree);
                    //::this.AddNode(this.ConstTree);
                }
            }
           
            //treeMain.TreeViewNodeSorter = new InstanceNodeSorter();
            //::treeMain.EndUpdate();
            this.MainF.Trace("Gothic-Bezeichner wurden erfolgreich aktualisert.");
            SetAutoCompleteContent();
           
        }
        
        private void UpdateConstTree()
        {
            List<Instance> t = new List<Instance>();
            t.AddRange(ConstList.Values);
            t.Sort();
            this.ConstTree.Nodes.Clear();
            foreach (Instance sl in t)
            {

                TreeNode node = new TreeNode(sl.ToString());
                node.ImageIndex = CONSTIMG;
                node.SelectedImageIndex = node.ImageIndex;
                node.Tag = sl.File;
                this.ConstTree.Nodes.Add(node);
            }
            t.Clear();
        }
        private void UpdateVarTree()
        {
            List<Instance> t = new List<Instance>();
            t.AddRange(VarList.Values);
            t.Sort();
            this.VarTree.Nodes.Clear();
            foreach (Instance sl in t)
            {

                TreeNode node = new TreeNode(sl.ToString());
                node.ImageIndex = VARIMG;
                node.SelectedImageIndex = node.ImageIndex;
                node.Tag = sl.File;
                this.VarTree.Nodes.Add(node);
            }
            t.Clear();
        }
        private void UpdateFuncTree()
        {
            List<Instance> t = new List<Instance>();
            t.AddRange(FuncList.Values);
            t.Sort();
            this.FuncTree.Nodes.Clear();
            foreach (Instance sl in t)
            {

                TreeNode node = new TreeNode(sl.ToString());
                node.ImageIndex = FUNCIMG;
                node.SelectedImageIndex = node.ImageIndex;
                node.Tag = sl.File;
                this.FuncTree.Nodes.Add(node);
            }
            t.Clear();
        }
        private void UpdateItemTree()
        {
            List<Instance> t = new List<Instance>();
            t.AddRange(ItemList.Values);
            t.Sort();
            this.ItemTree.Nodes.Clear();
            foreach (Instance sl in t)
            {

                TreeNode node = new TreeNode(sl.ToString());
                node.ImageIndex = ITEMIMG;
                node.SelectedImageIndex = node.ImageIndex;
                node.Tag = sl.File;
                this.ItemTree.Nodes.Add(node);
            }
            t.Clear();
        }
        private void UpdateNPCTree()
        {
            List<Instance> t = new List<Instance>();
            t.AddRange(NPCList.Values);
            t.Sort();
            this.NPCTree.Nodes.Clear();
            foreach (Instance sl in t)
            {

                TreeNode node = new TreeNode(sl.ToString());
                node.ImageIndex = NPCIMG;
                node.SelectedImageIndex = node.ImageIndex;
                node.Tag = sl.File;
                this.NPCTree.Nodes.Add(node);
            }
            t.Clear();
        }
        private void UpdateDialogTree()
        {
            List<Instance> t = new List<Instance>();
            t.AddRange(DialogList.Values);
            t.Sort();
            this.DialogTree.Nodes.Clear();
            foreach (Instance sl in t)
            {

                TreeNode node = new TreeNode(sl.ToString());
                node.ImageIndex = DIALOGIMG;
                node.SelectedImageIndex = node.ImageIndex;
                node.Tag = sl.File;
                this.DialogTree.Nodes.Add(node);
            }
            t.Clear();
        }
        
        private void ReadInstancesFromFile()
        {
            //::treeMain.BeginUpdate();
            ClearArrays();

            if (File.Exists(this.ScriptsPath + FilePaths.DIALOGE) && File.Exists(this.ScriptsPath + FilePaths.ITEMS) && File.Exists(this.ScriptsPath + FilePaths.NPCS) && File.Exists(this.ScriptsPath + FilePaths.FUNC) && File.Exists(this.ScriptsPath + FilePaths.VARS) && File.Exists(this.ScriptsPath + FilePaths.CONSTS))
            {
                //::treeMain.BeginUpdate();
                try
                {
                    this.MainF.Trace("Gothic-Bezeichner werden ausgelesen.");
                    string line = "";
                    string line2 = "";
                    
                    StreamReader sr = new StreamReader(this.ScriptsPath + FilePaths.DIALOGE, Encoding.Default);
                    while ((line = sr.ReadLine()) != null)
                    {
                        line2 = sr.ReadLine();
                        DialogList.Add(line,new Instance(line, line2));
                        
                    }
                    sr.Close();
                    //::UpdateDialogTree();
                    //::this.AddNode(this.DialogTree);

                    sr = new StreamReader(this.ScriptsPath + FilePaths.NPCS, Encoding.Default);
                    while ((line = sr.ReadLine()) != null)
                    {
                        line2 = sr.ReadLine();
                        NPCList.Add(line,new Instance(line, line2));
                        
                    }
                    sr.Close();
                    //::UpdateNPCTree();
                    //::this.AddNode(this.NPCTree);

                    sr = new StreamReader(this.ScriptsPath + FilePaths.ITEMS, Encoding.Default);
                    while ((line = sr.ReadLine()) != null)
                    {
                        line2 = sr.ReadLine();
                        ItemList.Add(line,new Instance(line, line2));
                        
                    }
                    sr.Close();
                    //::UpdateItemTree();
                    //::this.AddNode(this.ItemTree);

                    sr = new StreamReader(this.ScriptsPath + FilePaths.FUNC, Encoding.Default);
                    while ((line = sr.ReadLine()) != null)
                    {
                        line2 = sr.ReadLine();
                        FuncList.Add(line,new Instance(line, line2));
                       
                    }
                    sr.Close();
                    //::UpdateFuncTree();
                    //::this.AddNode(this.FuncTree);

                    sr = new StreamReader(this.ScriptsPath + FilePaths.VARS, Encoding.Default);
                    while ((line = sr.ReadLine()) != null)
                    {
                        line2 = sr.ReadLine();
                        VarList.Add(line,new Instance(line, line2));
                        
                    }
                    sr.Close();
                    //::UpdateVarTree();
                    //::this.AddNode(this.VarTree);

                    sr = new StreamReader(this.ScriptsPath + FilePaths.CONSTS, Encoding.Default);
                    while ((line = sr.ReadLine()) != null)
                    {
                        line2 = sr.ReadLine();
                        ConstList.Add(line,new Instance(line, line2));
                       
                    }
                    sr.Close();
                    //::UpdateConstTree();
                    //::this.AddNode(this.ConstTree);
                    sr.Dispose();



                    /*this.DialogTree.Nodes.Add("leer");
                    this.NPCTree.Nodes.Add("leer");
                    this.ItemTree.Nodes.Add("leer");
                    this.FuncTree.Nodes.Add("leer");
                    this.VarTree.Nodes.Add("leer");
                    this.ConstTree.Nodes.Add("leer");*/

                    //::treeMain.EndUpdate();
                    this.MainF.Trace("Gothic-Bezeichner erfolgreich ausgelesen.");
                }
                catch
                {
                    GetInstancesToFile(true, true, true, true);
                }
                readfromfile = true;
            }
            else
            {
                GetInstancesToFile(true, true, true, true);
            }
            //::treeMain.EndUpdate();
            SetAutoCompleteContent();
        }
        public string RemoveDoubleSpaces(string s)
        {
            /*s =  NoSpaces.Replace(s, " ");
            while (s.Contains("  "))
            {
                s = NoSpaces.Replace(s, " ");
*/
            string[] ts = s.Split(' ');
            StringBuilder sb = new StringBuilder(s.Length);
            
            foreach (string st in ts)
            {
                s = st.Trim();
                if (s.Length > 0)
                    sb.Append(s + " ");
            }
            return sb.ToString().Remove(sb.Length - 1);
        }
        private void BtGothicUpdate_Click(object sender, EventArgs e)
        {
            GetInstancesToFile(true, true, true, true);
        }
        public void SetAutoCompleteContent()
        {
           
            if (this.MainF.m_AutoComplete != null)
            {
                if (this.MainF.m_AutoComplete.Extension == ".d")
                {
                    this.MainF.m_AutoComplete.KW.Clear();
                   /* if (readfromfile&&File.Exists(ScriptsPath+Global.KW))
                    {
                        
                        BinaryFormatter binFormatter = new BinaryFormatter();
                        FileStream fls = new FileStream(ScriptsPath + Global.KW, FileMode.Open);
                        Classes.KWSave k = (Classes.KWSave)binFormatter.Deserialize(fls);
                        fls.Close();
                        fls.Dispose();
                        this.MainF.m_AutoComplete.KW.AddRange(k.KW);
                        return;
                    }*/
                    
                    //this.MainF.m_AutoComplete.UpdateContent();

                    
                    
                    string s1, s2 = "";
                    
                    foreach (Instance i in VarList.Values)
                    {
                        
                        
                        s1 = i.Name;
                        ConvertVarForAutoComplete(ref s1, ref s2);
                       
                        Classes.KeyWord k = new Classes.KeyWord(s1,4,s2,"Variable");
                        
                        if (this.MainF.m_AutoComplete.KW.BinarySearch(k) >= 0)
                        {
                            continue;
                        }
                        
                            this.MainF.m_AutoComplete.KW.Add(k);
                        
                       // this.MainF.m_AutoComplete.KW.Add(k);
                    }
                   /* for (int i = 0; i < this.MainF.m_AutoComplete.KW.Count; i++)// (Classes.KeyWord k in this.MainF.m_AutoComplete.KW)
                    {
                        while (this.MainF.m_AutoComplete.KW.BinarySearch(k) >= 0)
                        {

                        }
                    }*/



                    foreach (Instance i in FuncList.Values)
                    {
                        
                        s1=i.Name;
                        ConvertFuncForAutoComplete(ref s1, ref s2);
                        
                        Classes.KeyWord k = new Classes.KeyWord(s1, 3, s2, " ");
                       
                        this.MainF.m_AutoComplete.KW.Add(k);

                    }
                    
                    foreach (Instance i in ConstList.Values)
                    {
                        
                        s1 = i.Name;
                        ConvertConstForAutoComplete(ref s1, ref s2);
                       
                        Classes.KeyWord k = new Classes.KeyWord(s1, 5, s2, "Konstante");
                        if (this.MainF.m_AutoComplete.KW.BinarySearch(k) >= 0)
                        {
                            continue;
                        }
                        this.MainF.m_AutoComplete.KW.Add(k);
                    }
                    //MessageBox.Show(ConstList.Values.Count.ToString());
                    foreach (Instance i in DialogList.Values)
                    {
                        
                        Classes.KeyWord k = new Classes.KeyWord(i.Name, 0, "Dialog", " ");

                        this.MainF.m_AutoComplete.KW.Add(k);
                    }

                    foreach (Instance i in ItemList.Values)
                    {
                        
                        Classes.KeyWord k = new Classes.KeyWord(i.Name, 2, "Item", " ");

                        //ListViewItem lvi = new ListViewItem(
                        this.MainF.m_AutoComplete.KW.Add(k);
                    }

                    foreach (Instance i in NPCList.Values)
                    {
                      
                        Classes.KeyWord k = new Classes.KeyWord(i.Name, 1, "NPC", " ");


                        this.MainF.m_AutoComplete.KW.Add(k);
                    }



                    this.MainF.m_AutoComplete.KW.AddRange(this.MainF.m_AutoComplete.Properties);
                    this.MainF.m_AutoComplete.KW.AddRange(this.MainF.m_AutoComplete.ShortFuncs);
                    this.MainF.m_AutoComplete.KW.Sort();

                    
                    
                }
            }
        }
        private string RemoveType(ref string ts,ref string t)
        {
            
            int y = ts.IndexOf(" ");
            if (y > 0)
            {
                t = ts.Substring(0, y);
                ts = ts.Substring(y+1);
            }
            /*
            string tempstring = ts;
            if (tempstring.StartsWith("int "))
            {
                t = "int";
                ts = ts.Remove(0, 4);
            }
            else if (tempstring.StartsWith("void "))
            {
                t = "void";
                ts = ts.Remove(0, 5);
            }
            else if (tempstring.StartsWith("string "))
            {
                t = "string";
                ts = ts.Remove(0, 7);
            }
            else if (tempstring.StartsWith("float "))
            {
                t = "float";
                ts = ts.Remove(0, 6);
            }
            else if (tempstring.StartsWith("c_item "))
            {
                t = "c_item";
                ts = ts.Remove(0, 7);
            }
            else if (tempstring.StartsWith("c_npc "))
            {
                t = "c_npc";
                ts = ts.Remove(0, 6);
            }
            else if (tempstring.StartsWith("func "))
            {
                t = "func";
                ts = ts.Remove(0, 5);
            }
            else if (tempstring.StartsWith("c_spell "))
            {
                t = "c_spell";
                ts = ts.Remove(0, 8);
            }
            else if (tempstring.StartsWith("c_info "))
            {
                t = "c_info";
                ts = ts.Remove(0, 7);
            }
            ts=ts.Trim();*/
            return ts;
        }
        public void ConvertFuncForAutoComplete(ref string s1, ref string s2)
        {
            
            string [] sa;
            try
            {
                if (!s1.Contains("("))
                {
                    s1 = RemoveType(ref s1, ref s2);
                    
                }
                else
                {
                    sa = s1.Split('(');
                    sa[1] = "(" + sa[1];
                    s1 = RemoveType(ref sa[0], ref s2);
                    s2 = s2 + " " + sa[1];
                }

            }
            catch
            {
            }

           
        }
        public void ConvertVarForAutoComplete(ref string s1, ref string s2)
        {

          try
            {                
                RemoveType(ref s1, ref s2);               

            }
            catch
            {
            }
        }
        public void ConvertConstForAutoComplete(ref string s1, ref string s2)
        {

            try
            {
                if (!s1.Contains('='))
                {
                    s1 = RemoveType(ref s1, ref s2);
                }
                else
                {

                    string[] sa;

                    sa = s1.Split('=');
                    sa[1] = "=" + sa[1];
                    s1 = RemoveType(ref sa[0], ref s2).Trim();
                    s2 = s2 + " " + sa[1];
                }



            }
            catch
            {
            }


        }

        ArrayList TreeMatches = new ArrayList();
        private int currentMatch = 0;
        private void TxtSuchString_TextChanged(object sender, EventArgs e)
        {
            FindInTree(false);
           
        }
        Regex rg;
        private void FindInTreeSub(TreeNode i, string Temp, bool mode)
        {
            int lokIndex;
            try
            {
                if (mode == false)
                {
                    if (i.Text.ToLower().Contains(Temp))
                    {
                        TreeNode tempnode = i.Parent;
                        lokIndex = i.Index;
                        while (tempnode.PrevNode != null)
                        {
                            lokIndex += tempnode.PrevNode.Nodes.Count;
                            tempnode = tempnode.PrevNode;
                        }
                        i.StateImageKey = Convert.ToString(lokIndex);
                        TreeMatches.Add(i);
                        i.BackColor = Color.Yellow;
                    }
                }
                else if (rg!=null)
                {
                    //MessageBox.Show(rg.ToString());
                    if (rg.IsMatch(i.Text.ToLower()))
                    {
                        TreeNode tempnode = i.Parent;
                        lokIndex = i.Index;
                        while (tempnode.PrevNode != null)
                        {
                            lokIndex += tempnode.PrevNode.Nodes.Count;
                            tempnode = tempnode.PrevNode;
                        }
                        i.StateImageKey = Convert.ToString(lokIndex);
                        TreeMatches.Add(i);
                        i.BackColor = Color.Yellow;
                    }
                }
            }
            catch
            {

            }  
        }
        private void FindInTree(bool mode)
        {
           
            
            foreach (TreeNode k in TreeMatches)
            {
                k.BackColor = treeMain.BackColor;
            }
            TreeMatches.Clear();
            currentMatch = 0;
            if (mode==false && TxtSuchString.Text.Length < 3)
            {
                LbFound.Text = "0";
                return;
            }
            string Temp = TxtSuchString.Text.ToLower();
            try
            {
                rg = new Regex(Temp);
            }
            catch
            {
            }
            if (ItemTree.IsExpanded)
            {
                
                foreach (TreeNode i in ItemTree.Nodes)
                {
                    
                    FindInTreeSub(i, Temp, mode);
                }

            }
            if (DialogTree.IsExpanded)
            {
                
                foreach (TreeNode i in DialogTree.Nodes)
                {

                    FindInTreeSub(i, Temp, mode);
                }

            }
            if (NPCTree.IsExpanded)
            {
                
                foreach (TreeNode i in NPCTree.Nodes)
                {
                    FindInTreeSub(i, Temp, mode);
                }
            }
            if (FuncTree.IsExpanded)
            {
                
                foreach (TreeNode i in FuncTree.Nodes)
                {
                    FindInTreeSub(i, Temp,  mode);
                }
            }
            if (VarTree.IsExpanded)
            {
                
                foreach (TreeNode i in VarTree.Nodes)
                {
                    FindInTreeSub(i, Temp,  mode);
                }
            }
            if (ConstTree.IsExpanded)
            {
                
                foreach (TreeNode i in ConstTree.Nodes)
                {
                    FindInTreeSub(i, Temp,  mode);
                }
            }
            if (TreeMatches.Count > 0)
            {
                treeMain.SelectedNode = (TreeNode)TreeMatches[0];
                TxtFoundIndex.Text = "1";

            }
            else
            {
                TxtFoundIndex.Text = "";
            }
            LbFound.Text = TreeMatches.Count.ToString();
            
        }

       
            private void treeMain_KeyDown(object sender, KeyEventArgs e)
        {
            
            if (e.KeyCode==Keys.Enter && treeMain.SelectedNode!=null)
            {
                
                treeMain_NodeMouseDoubleClick(null, new TreeNodeMouseClickEventArgs(treeMain.SelectedNode,MouseButtons.Left,0,0,0));
            }
        }

        private void treeMain_AfterSelect(object sender, TreeViewEventArgs e)
        {
           /* if (e.Node.Tag != null)
            {
                MessageBox.Show(e.Node.Tag.ToString());
            }*/
            e.Node.EnsureVisible();
        }

        private void BtLeft_Click(object sender, EventArgs e)
        {
            if(TreeMatches.Count > 0)
            {
               /* if (currentMatch > 1)
                {
                    currentMatch--;
                    
                }
                else
                {
                    currentMatch = TreeMatches.Count - 1;
                }*/
                

                if (treeMain.SelectedNode != null)
                {
                    int globIndex = treeMain.SelectedNode.Index;
                    TreeNode tempnode = treeMain.SelectedNode.Parent;

                    while (tempnode.PrevNode != null)
                    {

                        globIndex += tempnode.PrevNode.Nodes.Count;
                        tempnode = tempnode.PrevNode;
                    }

                    int z;
                    if ((globIndex > Convert.ToInt32(((TreeNode)TreeMatches[currentMatch]).StateImageKey)))
                    {
                        z = TreeMatches.Count-1;
                    }
                    else
                    {
                        z = currentMatch;
                    }
                    for (int i = z; i>-1 ; i--)
                    {
                        if (globIndex > Convert.ToInt32(((TreeNode)TreeMatches[i]).StateImageKey))
                        {
                            currentMatch = i;
                            break;
                        }
                        else
                        {
                            currentMatch = TreeMatches.Count - 1;
                        }
                    }
                   
                }
                TxtFoundIndex.Text = ((int)(currentMatch + 1)).ToString(); 
                treeMain.SelectedNode = (TreeNode)TreeMatches[currentMatch];
                treeMain.Focus();

            }
        }

        private void BtRight_Click(object sender, EventArgs e)
        {
            if (TreeMatches.Count > 0)
            {
                                
               /* if (currentMatch < TreeMatches.Count-1)
                {
                    currentMatch++;
                   
                }
                else
                {
                    currentMatch = 0;
                }*/

                if (treeMain.SelectedNode!=null)
                {
                    int globIndex = treeMain.SelectedNode.Index;
                    TreeNode tempnode = treeMain.SelectedNode.Parent;

                    while (tempnode.PrevNode!=null)
                    {
                        
                        globIndex += tempnode.PrevNode.Nodes.Count;
                        tempnode = tempnode.PrevNode;
                    }
                    //MessageBox.Show((((TreeNode)TreeMatches[currentMatch]).StateImageKey)+ "   " + globIndex);

                    int z;
                    if ((globIndex < Convert.ToInt32(((TreeNode)TreeMatches[currentMatch]).StateImageKey)))
                    {
                        z = 0;
                       
                    }
                    else
                    {
                        z = currentMatch;
                    }
                    for (int i = z; i < TreeMatches.Count; i++)
                    {
                       
                        if (globIndex < Convert.ToInt32(((TreeNode)TreeMatches[i]).StateImageKey))
                        {
                            currentMatch = i;
                            break;
                        }
                        else
                        {
                            currentMatch = 0;
                        }
                    }
                    
                }
                TxtFoundIndex.Text = ((int)(currentMatch+1)).ToString(); 
                treeMain.SelectedNode = (TreeNode)TreeMatches[currentMatch];
                treeMain.Focus();

            }
        }
        Regex DigitOnly = new Regex(@"\d{1}");

        private void TxtFoundIndex_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                if (TreeMatches.Count > 0)
                {
                    DigitOnly = new Regex(@"\d{" + TxtFoundIndex.Text.Length + "}");
                    if (!DigitOnly.IsMatch(TxtFoundIndex.Text))
                    {
                        return;
                    }
                    int FoundIndex = Convert.ToInt32(TxtFoundIndex.Text);
                    if (FoundIndex < 1)
                    {
                        FoundIndex = 1;
                    }
                    else if (FoundIndex > TreeMatches.Count)
                    {
                        FoundIndex = TreeMatches.Count;
                    }
                        TxtFoundIndex.Text = FoundIndex.ToString();
                        currentMatch = FoundIndex-1;
                        treeMain.SelectedNode = (TreeNode)TreeMatches[currentMatch];
                        treeMain.Focus();
                    
                }
            }
        }

        private void tsbGotoCode_Click(object sender, EventArgs e)
        {

        }

        private void BtCopyTreeItem_Click(object sender, EventArgs e)
        {
            if (treeMain.SelectedNode!=null)
            {
                Clipboard.SetText(treeMain.SelectedNode.Text);

            }
        }

        private void treeMain_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.Node != null)
                {
                    /* 
                     string s1, s2 = "";
                    foreach (Instance i in FuncList)
                    {
                       // MessageBox.Show("ASD");
                        Classes.KeyWord k = new Classes.KeyWord();
                        s1=i.Name;
                        ConvertFuncForAutoComplete(ref s1, ref s2);
                        k.Name = s1;
                        k.Text1 = s2;
                        k.Text2 = " ";

                        this.MainF.m_AutoComplete.KW.Add(k);
                    }
                    
                     */
                     string Temp = "";
                     string s2 = "";
                     Temp = e.Node.Text;
                    if (e.Node.Parent==FuncTree)
                    {
                        ConvertFuncForAutoComplete(ref Temp, ref s2);
                    }
                    else if (e.Node.Parent == VarTree)
                    {
                        ConvertVarForAutoComplete(ref Temp, ref s2);
                    }
                    else if (e.Node.Parent == ConstTree)
                    {
                        ConvertConstForAutoComplete(ref Temp, ref s2);
                    }

                    Clipboard.SetText(Temp);

                }
            }
        }

        private void TxtSuchString_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control&&e.KeyCode==Keys.V)
            {   
                TxtSuchString.Text = Clipboard.GetText();
            }

            if (e.Control && e.Alt && e.KeyCode==Keys.D9)
            {
                TxtSuchString.Text += "]";
            }
            if (e.Control && e.Alt && e.KeyCode == Keys.OemBackslash)
            {
                TxtSuchString.Text += "\\";
            }
        }

        private void mRrefresh_all_Click(object sender, EventArgs e)
        {
            GetInstancesToFile(true, true, true, true);
        }

        private void mRrefresh_dia_Click(object sender, EventArgs e)
        {
            GetInstancesToFile(true, false, false, false);
        }

        private void mRrefresh_npc_Click(object sender, EventArgs e)
        {
            GetInstancesToFile(false, true, false, false);
        }

        private void mRrefresh_items_Click(object sender, EventArgs e)
        {
            GetInstancesToFile(false, false, true, false);
        }

        private void mRrefresh_Func_Click(object sender, EventArgs e)
        {
            GetInstancesToFile(false, true, false, true);
        }

        private void BtRegex_Click(object sender, EventArgs e)
        {
            FindInTree(true);
        }

        

        
        
        
        
    }
    [Serializable()]
    public class Instance:IComparable
    {
        public string Name;
        public string File;
        public string Params;
        public int Line;
        public Instance(string s1, string s2)
        {
            Name = s1;
            File = s2;

        }
        public Instance(string s1, string s2, string s3)
        {
            Name = s1;
            File = s2;
            Params = s3;

        }
        public Instance(string s1, int i1)
        {
            Name = s1;
            Line = i1;

        }
        public override string ToString()
        {
            return Name;
        }
        int IComparable.CompareTo(object obj)
        {
            Instance p = obj as Instance;
            
            return String.Compare(this.ToString(), p.ToString(), true);
           

        }
    }


    public class InstanceNodeSorter : IComparer
    {
        // Compare the length of the strings, or the strings
        // themselves, if they are the same length.
        public int Compare(object x, object y)
        {
            TreeNode tx = x as TreeNode;
            TreeNode ty = y as TreeNode;

            

            // If they are the same length, call Compare.
            return string.Compare(tx.Text, ty.Text,true);
        }
    }


    /*public class InstanceComparer : IComparer
    {

        public InstanceComparer()
        {
           
        }
        int IComparer.Compare(Instance x, Instance y)
        {
            /*if (x == null && y == null) return 0;
            else if (x == null) return -1;
            else if (y == null) return 1;
            if (x == y) return 0;

            //Instance f = x as Instance;

            //Instance g = y as Instance;


            return String.Compare(((Instance)(x)).ToString(), ((Instance)(y)).ToString(), true);
        }
    }*/
    
   
}
