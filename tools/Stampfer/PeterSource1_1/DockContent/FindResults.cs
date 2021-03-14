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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using PeterInterface;

namespace Peter
{
    public partial class FindResults : DockContent, IPeterPluginTab
    {
        private IPeterPluginHost m_Host;

        public FindResults()
        {
            InitializeComponent(); 
            System.Resources.ResourceManager mngr = new System.Resources.ResourceManager("Peter.InternalImages", this.GetType().Assembly);
            this.imgMain.Images.Add("Next", (Image)mngr.GetObject("Next"));
            this.TabText = "Suchergebnisse";
        }

        /// <summary>
        /// Gets or Sets the Tree View...
        /// </summary>
        public TreeView Tree
        {
            get { return this.treeMain; }

            set { this.treeMain = value; }
        }

        /// <summary>
        /// Gets or Sets the Image List...
        /// </summary>
        public ImageList Images
        {
            get { return this.imgMain; }

            set { this.imgMain = value; }
        }

        /// <summary>
        /// Overrides the Close Action...
        /// </summary>
        /// <param name="e">Cancel Events.</param>
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            base.OnClosing(e);
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
                Clipboard.SetText(this.treeMain.SelectedNode.Text);
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

        public void Duplicate()
        {
        }

        public void Delete()
        {
            if (this.treeMain.SelectedNode != null)
            {
                if (this.treeMain.SelectedNode.Parent == null)
                {
                    this.treeMain.Nodes.Remove(this.treeMain.SelectedNode);
                }
                else
                {
                    this.treeMain.SelectedNode.Parent.Nodes.Remove(this.treeMain.SelectedNode);
                }
            }
        }

        public void SelectAll()
        {
        }

        public bool CloseTab()
        {
            this.Hide();
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
            get { return true; }
        }

        public bool NeedsSaving
        {
            get { return false; }
        }

        #endregion

        private void treeMain_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }
    }
}