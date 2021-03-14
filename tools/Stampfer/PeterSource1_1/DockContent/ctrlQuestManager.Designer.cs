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
    partial class ctrlQuestManager
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ctrlQuestManager));
            this.treeMain = new System.Windows.Forms.TreeView();
            this.imgCSS = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbExpandAll = new System.Windows.Forms.ToolStripButton();
            this.tsbCollapseAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbGotoCode = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.BTParse = new System.Windows.Forms.ToolStripButton();
            this.ContextOrdner = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.hoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.neueQuestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.questToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.standardQuestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bringeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sprichMitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.töteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ordnerBearbeitenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.löschenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ordnerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.questsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.QuestStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.bearbeitenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loeschenToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.QuestImages = new System.Windows.Forms.ImageList(this.components);
            this.pnQuests = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.bearbeitenToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.Dias = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.bearbeitenToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.NPCs = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.bearbeitenToolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.XP_Menu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.bearbeitenToolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.DiaryEntries = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.bearbeitenToolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1.SuspendLayout();
            this.ContextOrdner.SuspendLayout();
            this.QuestStrip.SuspendLayout();
            this.pnQuests.SuspendLayout();
            this.Dias.SuspendLayout();
            this.NPCs.SuspendLayout();
            this.XP_Menu.SuspendLayout();
            this.DiaryEntries.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeMain
            // 
            this.treeMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeMain.Location = new System.Drawing.Point(0, 25);
            this.treeMain.Name = "treeMain";
            this.treeMain.Size = new System.Drawing.Size(255, 634);
            this.treeMain.TabIndex = 1;
            this.treeMain.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeMain_NodeMouseDoubleClick_1);
            this.treeMain.NodeMouseHover += new System.Windows.Forms.TreeNodeMouseHoverEventHandler(this.treeMain_NodeMouseHover);
            this.treeMain.MouseLeave += new System.EventHandler(this.treeMain_MouseLeave);
            // 
            // imgCSS
            // 
            this.imgCSS.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgCSS.ImageStream")));
            this.imgCSS.TransparentColor = System.Drawing.Color.Transparent;
            this.imgCSS.Images.SetKeyName(0, "Selector.ico");
            this.imgCSS.Images.SetKeyName(1, "Tag.ico");
            this.imgCSS.Images.SetKeyName(2, "Property.ico");
            this.imgCSS.Images.SetKeyName(3, "Value.ico");
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbExpandAll,
            this.tsbCollapseAll,
            this.toolStripSeparator2,
            this.tsbGotoCode,
            this.toolStripSeparator3,
            this.BTParse});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(255, 25);
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
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbGotoCode
            // 
            this.tsbGotoCode.Checked = true;
            this.tsbGotoCode.CheckOnClick = true;
            this.tsbGotoCode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsbGotoCode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbGotoCode.Image = ((System.Drawing.Image)(resources.GetObject("tsbGotoCode.Image")));
            this.tsbGotoCode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbGotoCode.Name = "tsbGotoCode";
            this.tsbGotoCode.Size = new System.Drawing.Size(23, 22);
            this.tsbGotoCode.Text = "Beim Klick zur Zeile springen";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // BTParse
            // 
            this.BTParse.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BTParse.Image = ((System.Drawing.Image)(resources.GetObject("BTParse.Image")));
            this.BTParse.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BTParse.Name = "BTParse";
            this.BTParse.Size = new System.Drawing.Size(23, 22);
            this.BTParse.Text = "Aktualisieren";
            this.BTParse.Click += new System.EventHandler(this.BTParse_Click);
            // 
            // ContextOrdner
            // 
            this.ContextOrdner.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hoToolStripMenuItem,
            this.neueQuestToolStripMenuItem,
            this.ordnerBearbeitenToolStripMenuItem,
            this.löschenToolStripMenuItem});
            this.ContextOrdner.Name = "ContextOrdner";
            this.ContextOrdner.Size = new System.Drawing.Size(175, 92);
            // 
            // hoToolStripMenuItem
            // 
            this.hoToolStripMenuItem.Name = "hoToolStripMenuItem";
            this.hoToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.hoToolStripMenuItem.Text = "Neuer Ordner";
            this.hoToolStripMenuItem.Click += new System.EventHandler(this.hoToolStripMenuItem_Click);
            // 
            // neueQuestToolStripMenuItem
            // 
            this.neueQuestToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.questToolStripMenuItem,
            this.standardQuestToolStripMenuItem});
            this.neueQuestToolStripMenuItem.Name = "neueQuestToolStripMenuItem";
            this.neueQuestToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.neueQuestToolStripMenuItem.Text = "Neue Quest";
            // 
            // questToolStripMenuItem
            // 
            this.questToolStripMenuItem.Name = "questToolStripMenuItem";
            this.questToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.questToolStripMenuItem.Text = "Quest";
            this.questToolStripMenuItem.Click += new System.EventHandler(this.questToolStripMenuItem_Click);
            // 
            // standardQuestToolStripMenuItem
            // 
            this.standardQuestToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bringeToolStripMenuItem,
            this.sprichMitToolStripMenuItem,
            this.töteToolStripMenuItem});
            this.standardQuestToolStripMenuItem.Name = "standardQuestToolStripMenuItem";
            this.standardQuestToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.standardQuestToolStripMenuItem.Text = "Standard Quest";
            // 
            // bringeToolStripMenuItem
            // 
            this.bringeToolStripMenuItem.Name = "bringeToolStripMenuItem";
            this.bringeToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.bringeToolStripMenuItem.Text = "Bringe...";
            // 
            // sprichMitToolStripMenuItem
            // 
            this.sprichMitToolStripMenuItem.Name = "sprichMitToolStripMenuItem";
            this.sprichMitToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.sprichMitToolStripMenuItem.Text = "Sprich mit...";
            // 
            // töteToolStripMenuItem
            // 
            this.töteToolStripMenuItem.Name = "töteToolStripMenuItem";
            this.töteToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.töteToolStripMenuItem.Text = "Töte...";
            // 
            // ordnerBearbeitenToolStripMenuItem
            // 
            this.ordnerBearbeitenToolStripMenuItem.Name = "ordnerBearbeitenToolStripMenuItem";
            this.ordnerBearbeitenToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.ordnerBearbeitenToolStripMenuItem.Text = "Ordner bearbeiten";
            this.ordnerBearbeitenToolStripMenuItem.Click += new System.EventHandler(this.ordnerBearbeitenToolStripMenuItem_Click);
            // 
            // löschenToolStripMenuItem
            // 
            this.löschenToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ordnerToolStripMenuItem,
            this.questsToolStripMenuItem});
            this.löschenToolStripMenuItem.Name = "löschenToolStripMenuItem";
            this.löschenToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.löschenToolStripMenuItem.Text = "Löschen";
            // 
            // ordnerToolStripMenuItem
            // 
            this.ordnerToolStripMenuItem.Name = "ordnerToolStripMenuItem";
            this.ordnerToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.ordnerToolStripMenuItem.Text = "Ordner";
            this.ordnerToolStripMenuItem.Click += new System.EventHandler(this.ordnerToolStripMenuItem_Click);
            // 
            // questsToolStripMenuItem
            // 
            this.questsToolStripMenuItem.Name = "questsToolStripMenuItem";
            this.questsToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.questsToolStripMenuItem.Text = "Quests";
            this.questsToolStripMenuItem.Click += new System.EventHandler(this.questsToolStripMenuItem_Click);
            // 
            // QuestStrip
            // 
            this.QuestStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bearbeitenToolStripMenuItem,
            this.loeschenToolStripMenuItem1});
            this.QuestStrip.Name = "QuestStrip";
            this.QuestStrip.Size = new System.Drawing.Size(138, 48);
            // 
            // bearbeitenToolStripMenuItem
            // 
            this.bearbeitenToolStripMenuItem.Name = "bearbeitenToolStripMenuItem";
            this.bearbeitenToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.bearbeitenToolStripMenuItem.Text = "Bearbeiten";
            this.bearbeitenToolStripMenuItem.Click += new System.EventHandler(this.bearbeitenToolStripMenuItem_Click);
            // 
            // loeschenToolStripMenuItem1
            // 
            this.loeschenToolStripMenuItem1.Name = "loeschenToolStripMenuItem1";
            this.loeschenToolStripMenuItem1.Size = new System.Drawing.Size(137, 22);
            this.loeschenToolStripMenuItem1.Text = "Löschen";
            this.loeschenToolStripMenuItem1.Click += new System.EventHandler(this.loeschenToolStripMenuItem1_Click);
            // 
            // QuestImages
            // 
            this.QuestImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("QuestImages.ImageStream")));
            this.QuestImages.TransparentColor = System.Drawing.Color.Fuchsia;
            this.QuestImages.Images.SetKeyName(0, "Folder.png");
            this.QuestImages.Images.SetKeyName(1, "Quest.png");
            this.QuestImages.Images.SetKeyName(2, "Questfolder.png");
            this.QuestImages.Images.SetKeyName(3, "pQuest.png");
            this.QuestImages.Images.SetKeyName(4, "nQuest.png");
            this.QuestImages.Images.SetKeyName(5, "StartDialog.png");
            this.QuestImages.Images.SetKeyName(6, "Dialog.png");
            this.QuestImages.Images.SetKeyName(7, "EndDialog.png");
            this.QuestImages.Images.SetKeyName(8, "NPC.png");
            this.QuestImages.Images.SetKeyName(9, "Book.png");
            this.QuestImages.Images.SetKeyName(10, "uparrow.png");
            // 
            // pnQuests
            // 
            this.pnQuests.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bearbeitenToolStripMenuItem1});
            this.pnQuests.Name = "pnQuests";
            this.pnQuests.Size = new System.Drawing.Size(138, 26);
            // 
            // bearbeitenToolStripMenuItem1
            // 
            this.bearbeitenToolStripMenuItem1.Name = "bearbeitenToolStripMenuItem1";
            this.bearbeitenToolStripMenuItem1.Size = new System.Drawing.Size(137, 22);
            this.bearbeitenToolStripMenuItem1.Text = "Bearbeiten";
            this.bearbeitenToolStripMenuItem1.Click += new System.EventHandler(this.bearbeitenToolStripMenuItem1_Click);
            // 
            // Dias
            // 
            this.Dias.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bearbeitenToolStripMenuItem3});
            this.Dias.Name = "Dias";
            this.Dias.Size = new System.Drawing.Size(138, 26);
            // 
            // bearbeitenToolStripMenuItem3
            // 
            this.bearbeitenToolStripMenuItem3.Name = "bearbeitenToolStripMenuItem3";
            this.bearbeitenToolStripMenuItem3.Size = new System.Drawing.Size(137, 22);
            this.bearbeitenToolStripMenuItem3.Text = "Bearbeiten";
            // 
            // NPCs
            // 
            this.NPCs.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bearbeitenToolStripMenuItem5});
            this.NPCs.Name = "NPCs";
            this.NPCs.Size = new System.Drawing.Size(153, 48);
            // 
            // bearbeitenToolStripMenuItem5
            // 
            this.bearbeitenToolStripMenuItem5.Name = "bearbeitenToolStripMenuItem5";
            this.bearbeitenToolStripMenuItem5.Size = new System.Drawing.Size(152, 22);
            this.bearbeitenToolStripMenuItem5.Text = "Bearbeiten";
            this.bearbeitenToolStripMenuItem5.Click += new System.EventHandler(this.bearbeitenToolStripMenuItem5_Click);
            // 
            // XP_Menu
            // 
            this.XP_Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bearbeitenToolStripMenuItem6});
            this.XP_Menu.Name = "XP_Menu";
            this.XP_Menu.Size = new System.Drawing.Size(138, 26);
            // 
            // bearbeitenToolStripMenuItem6
            // 
            this.bearbeitenToolStripMenuItem6.Name = "bearbeitenToolStripMenuItem6";
            this.bearbeitenToolStripMenuItem6.Size = new System.Drawing.Size(137, 22);
            this.bearbeitenToolStripMenuItem6.Text = "Bearbeiten";
            // 
            // DiaryEntries
            // 
            this.DiaryEntries.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bearbeitenToolStripMenuItem7});
            this.DiaryEntries.Name = "DiaryEntries";
            this.DiaryEntries.Size = new System.Drawing.Size(138, 26);
            // 
            // bearbeitenToolStripMenuItem7
            // 
            this.bearbeitenToolStripMenuItem7.Name = "bearbeitenToolStripMenuItem7";
            this.bearbeitenToolStripMenuItem7.Size = new System.Drawing.Size(152, 22);
            this.bearbeitenToolStripMenuItem7.Text = "Bearbeiten";
            // 
            // ctrlQuestManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(255, 659);
            this.Controls.Add(this.treeMain);
            this.Controls.Add(this.toolStrip1);
            this.Name = "ctrlQuestManager";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ContextOrdner.ResumeLayout(false);
            this.QuestStrip.ResumeLayout(false);
            this.pnQuests.ResumeLayout(false);
            this.Dias.ResumeLayout(false);
            this.NPCs.ResumeLayout(false);
            this.XP_Menu.ResumeLayout(false);
            this.DiaryEntries.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeMain;
        private System.Windows.Forms.ImageList imgCSS;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbExpandAll;
        private System.Windows.Forms.ToolStripButton tsbCollapseAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsbGotoCode;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton BTParse;
        public System.Windows.Forms.ContextMenuStrip ContextOrdner;
        private System.Windows.Forms.ToolStripMenuItem hoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem neueQuestToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem questToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem standardQuestToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bringeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sprichMitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem töteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ordnerBearbeitenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem löschenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ordnerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem questsToolStripMenuItem;
        public System.Windows.Forms.ContextMenuStrip QuestStrip;
        private System.Windows.Forms.ToolStripMenuItem loeschenToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem bearbeitenToolStripMenuItem;
        private System.Windows.Forms.ImageList QuestImages;
        public System.Windows.Forms.ContextMenuStrip pnQuests;
        private System.Windows.Forms.ToolStripMenuItem bearbeitenToolStripMenuItem1;
        public System.Windows.Forms.ContextMenuStrip Dias;
        private System.Windows.Forms.ToolStripMenuItem bearbeitenToolStripMenuItem3;
        public System.Windows.Forms.ContextMenuStrip NPCs;
        private System.Windows.Forms.ToolStripMenuItem bearbeitenToolStripMenuItem5;
        public System.Windows.Forms.ContextMenuStrip XP_Menu;
        private System.Windows.Forms.ToolStripMenuItem bearbeitenToolStripMenuItem6;
        public System.Windows.Forms.ContextMenuStrip DiaryEntries;
        private System.Windows.Forms.ToolStripMenuItem bearbeitenToolStripMenuItem7;

    }
}
