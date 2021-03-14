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
namespace Peter
{
    partial class ctrlGothicInstances
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ctrlGothicInstances));
            this.treeMain = new System.Windows.Forms.TreeView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbExpandAll = new System.Windows.Forms.ToolStripButton();
            this.tsbCollapseAll = new System.Windows.Forms.ToolStripButton();
            this.TxtSuchString = new System.Windows.Forms.ToolStripTextBox();
            this.BtLeft = new System.Windows.Forms.ToolStripButton();
            this.BtRight = new System.Windows.Forms.ToolStripButton();
            this.TxtFoundIndex = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.LbFound = new System.Windows.Forms.ToolStripLabel();
            this.BtCopyTreeItem = new System.Windows.Forms.ToolStripButton();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.mRrefresh_all = new System.Windows.Forms.ToolStripMenuItem();
            this.mRrefresh_dia = new System.Windows.Forms.ToolStripMenuItem();
            this.mRrefresh_npc = new System.Windows.Forms.ToolStripMenuItem();
            this.mRrefresh_items = new System.Windows.Forms.ToolStripMenuItem();
            this.mRrefresh_Func = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeMain
            // 
            this.treeMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeMain.Location = new System.Drawing.Point(0, 25);
            this.treeMain.Name = "treeMain";
            this.treeMain.Size = new System.Drawing.Size(403, 634);
            this.treeMain.TabIndex = 1;
            this.treeMain.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeMain_AfterSelect);
            this.treeMain.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeMain_NodeMouseClick);
            this.treeMain.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeMain_KeyDown);
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1,
            this.toolStripSeparator2,
            this.tsbExpandAll,
            this.tsbCollapseAll,
            this.TxtSuchString,
            this.BtLeft,
            this.BtRight,
            this.TxtFoundIndex,
            this.toolStripSeparator1,
            this.LbFound,
            this.BtCopyTreeItem});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(403, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbExpandAll
            // 
            this.tsbExpandAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbExpandAll.Image = ((System.Drawing.Image)(resources.GetObject("tsbExpandAll.Image")));
            this.tsbExpandAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbExpandAll.Name = "tsbExpandAll";
            this.tsbExpandAll.Size = new System.Drawing.Size(23, 22);
            this.tsbExpandAll.Text = "Alle ausklappen";
            this.tsbExpandAll.Click += new System.EventHandler(this.tsbExpandAll_Click);
            // 
            // tsbCollapseAll
            // 
            this.tsbCollapseAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbCollapseAll.Image = ((System.Drawing.Image)(resources.GetObject("tsbCollapseAll.Image")));
            this.tsbCollapseAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCollapseAll.Name = "tsbCollapseAll";
            this.tsbCollapseAll.Size = new System.Drawing.Size(23, 22);
            this.tsbCollapseAll.Text = "Alle einklappen";
            this.tsbCollapseAll.Click += new System.EventHandler(this.tsbCollapseAll_Click);
            // 
            // TxtSuchString
            // 
            this.TxtSuchString.AcceptsReturn = true;
            this.TxtSuchString.AcceptsTab = true;
            this.TxtSuchString.Name = "TxtSuchString";
            this.TxtSuchString.ShortcutsEnabled = false;
            this.TxtSuchString.Size = new System.Drawing.Size(100, 25);
            this.TxtSuchString.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtSuchString_KeyDown);
            this.TxtSuchString.TextChanged += new System.EventHandler(this.TxtSuchString_TextChanged);
            // 
            // BtLeft
            // 
            this.BtLeft.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtLeft.Image = global::Peter.Properties.Resources.Left;
            this.BtLeft.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtLeft.Name = "BtLeft";
            this.BtLeft.Size = new System.Drawing.Size(23, 22);
            this.BtLeft.Text = "Vorheriges Element";
            this.BtLeft.Click += new System.EventHandler(this.BtLeft_Click);
            // 
            // BtRight
            // 
            this.BtRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtRight.Image = global::Peter.Properties.Resources.GoToNextHS;
            this.BtRight.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtRight.Name = "BtRight";
            this.BtRight.Size = new System.Drawing.Size(23, 22);
            this.BtRight.Text = "Nächstes Element";
            this.BtRight.Click += new System.EventHandler(this.BtRight_Click);
            // 
            // TxtFoundIndex
            // 
            this.TxtFoundIndex.AutoSize = false;
            this.TxtFoundIndex.Name = "TxtFoundIndex";
            this.TxtFoundIndex.Size = new System.Drawing.Size(30, 25);
            this.TxtFoundIndex.ToolTipText = "Aktueller Suchindex";
            this.TxtFoundIndex.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtFoundIndex_KeyDown);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // LbFound
            // 
            this.LbFound.Name = "LbFound";
            this.LbFound.Size = new System.Drawing.Size(13, 22);
            this.LbFound.Text = "0";
            this.LbFound.ToolTipText = "Gefundene Einträge";
            // 
            // BtCopyTreeItem
            // 
            this.BtCopyTreeItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtCopyTreeItem.Image = global::Peter.Properties.Resources.downarrow;
            this.BtCopyTreeItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtCopyTreeItem.Name = "BtCopyTreeItem";
            this.BtCopyTreeItem.Size = new System.Drawing.Size(23, 22);
            this.BtCopyTreeItem.Text = "Eintrag Kopieren";
            this.BtCopyTreeItem.Click += new System.EventHandler(this.BtCopyTreeItem_Click);
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mRrefresh_all,
            this.mRrefresh_dia,
            this.mRrefresh_npc,
            this.mRrefresh_items,
            this.mRrefresh_Func});
            this.toolStripDropDownButton1.Image = global::Peter.Properties.Resources.analyze;
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(29, 22);
            this.toolStripDropDownButton1.Text = "Aktualisieren";
            // 
            // mRrefresh_all
            // 
            this.mRrefresh_all.Image = global::Peter.Properties.Resources.analyze;
            this.mRrefresh_all.Name = "mRrefresh_all";
            this.mRrefresh_all.Size = new System.Drawing.Size(158, 22);
            this.mRrefresh_all.Text = "Alles";
            this.mRrefresh_all.Click += new System.EventHandler(this.mRrefresh_all_Click);
            // 
            // mRrefresh_dia
            // 
            this.mRrefresh_dia.Image = global::Peter.Properties.Resources.Dialog;
            this.mRrefresh_dia.Name = "mRrefresh_dia";
            this.mRrefresh_dia.Size = new System.Drawing.Size(158, 22);
            this.mRrefresh_dia.Text = "Dialoge";
            this.mRrefresh_dia.Click += new System.EventHandler(this.mRrefresh_dia_Click);
            // 
            // mRrefresh_npc
            // 
            this.mRrefresh_npc.Image = global::Peter.Properties.Resources.NPC;
            this.mRrefresh_npc.Name = "mRrefresh_npc";
            this.mRrefresh_npc.Size = new System.Drawing.Size(158, 22);
            this.mRrefresh_npc.Text = "NPCs";
            this.mRrefresh_npc.Click += new System.EventHandler(this.mRrefresh_npc_Click);
            // 
            // mRrefresh_items
            // 
            this.mRrefresh_items.Image = global::Peter.Properties.Resources.Items;
            this.mRrefresh_items.Name = "mRrefresh_items";
            this.mRrefresh_items.Size = new System.Drawing.Size(158, 22);
            this.mRrefresh_items.Text = "Items";
            this.mRrefresh_items.Click += new System.EventHandler(this.mRrefresh_items_Click);
            // 
            // mRrefresh_Func
            // 
            this.mRrefresh_Func.Image = global::Peter.Properties.Resources.Funtionen;
            this.mRrefresh_Func.Name = "mRrefresh_Func";
            this.mRrefresh_Func.Size = new System.Drawing.Size(158, 22);
            this.mRrefresh_Func.Text = "Func/Var/Const";
            this.mRrefresh_Func.Click += new System.EventHandler(this.mRrefresh_Func_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // ctrlGothicInstances
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.ClientSize = new System.Drawing.Size(403, 659);
            this.Controls.Add(this.treeMain);
            this.Controls.Add(this.toolStrip1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeyPreview = true;
            this.Name = "ctrlGothicInstances";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeMain;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbExpandAll;
        private System.Windows.Forms.ToolStripButton tsbCollapseAll;
        private System.Windows.Forms.ToolStripTextBox TxtSuchString;
        private System.Windows.Forms.ToolStripButton BtLeft;
        private System.Windows.Forms.ToolStripButton BtRight;
        private System.Windows.Forms.ToolStripTextBox TxtFoundIndex;
        private System.Windows.Forms.ToolStripLabel LbFound;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton BtCopyTreeItem;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem mRrefresh_all;
        private System.Windows.Forms.ToolStripMenuItem mRrefresh_dia;
        private System.Windows.Forms.ToolStripMenuItem mRrefresh_npc;
        private System.Windows.Forms.ToolStripMenuItem mRrefresh_items;
        private System.Windows.Forms.ToolStripMenuItem mRrefresh_Func;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;


    }
}
