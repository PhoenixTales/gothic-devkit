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
namespace Peter
{
    partial class Find
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Find));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbFind = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbReplace = new System.Windows.Forms.ToolStripButton();
            this.lblFilter = new System.Windows.Forms.Label();
            this.cmbFilter = new System.Windows.Forms.ComboBox();
            this.btnMark = new System.Windows.Forms.Button();
            this.btnFind = new System.Windows.Forms.Button();
            this.lblReplace = new System.Windows.Forms.Label();
            this.cmbReplace = new System.Windows.Forms.ComboBox();
            this.ckbFindIn = new System.Windows.Forms.CheckBox();
            this.cmbFindIn = new System.Windows.Forms.ComboBox();
            this.lblFind = new System.Windows.Forms.Label();
            this.cmbFindText = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ckbmarked = new System.Windows.Forms.CheckBox();
            this.ckbWildCard = new System.Windows.Forms.CheckBox();
            this.ckbUseRegEx = new System.Windows.Forms.CheckBox();
            this.ckbSearchUp = new System.Windows.Forms.CheckBox();
            this.ckbMatchWord = new System.Windows.Forms.CheckBox();
            this.ckbMatchCase = new System.Windows.Forms.CheckBox();
            this.fbdMain = new System.Windows.Forms.FolderBrowserDialog();
            this.btnStop = new System.Windows.Forms.Button();
            this.toolStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbFind,
            this.toolStripSeparator1,
            this.tsbReplace});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(466, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbFind
            // 
            this.tsbFind.Checked = true;
            this.tsbFind.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsbFind.Image = ((System.Drawing.Image)(resources.GetObject("tsbFind.Image")));
            this.tsbFind.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbFind.Name = "tsbFind";
            this.tsbFind.Size = new System.Drawing.Size(62, 22);
            this.tsbFind.Text = "Suchen";
            this.tsbFind.Click += new System.EventHandler(this.tsbFind_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbReplace
            // 
            this.tsbReplace.Image = ((System.Drawing.Image)(resources.GetObject("tsbReplace.Image")));
            this.tsbReplace.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbReplace.Name = "tsbReplace";
            this.tsbReplace.Size = new System.Drawing.Size(69, 22);
            this.tsbReplace.Text = "Ersetzen";
            this.tsbReplace.Click += new System.EventHandler(this.tsbReplace_Click);
            // 
            // lblFilter
            // 
            this.lblFilter.AutoSize = true;
            this.lblFilter.Enabled = false;
            this.lblFilter.Location = new System.Drawing.Point(12, 155);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(32, 13);
            this.lblFilter.TabIndex = 31;
            this.lblFilter.Text = "Filter:";
            // 
            // cmbFilter
            // 
            this.cmbFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbFilter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbFilter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbFilter.Enabled = false;
            this.cmbFilter.FormattingEnabled = true;
            this.cmbFilter.Items.AddRange(new object[] {
            "*.*"});
            this.cmbFilter.Location = new System.Drawing.Point(12, 202);
            this.cmbFilter.Name = "cmbFilter";
            this.cmbFilter.Size = new System.Drawing.Size(58, 21);
            this.cmbFilter.TabIndex = 30;
            this.cmbFilter.Text = "*.*";
            // 
            // btnMark
            // 
            this.btnMark.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMark.Location = new System.Drawing.Point(179, 200);
            this.btnMark.Name = "btnMark";
            this.btnMark.Size = new System.Drawing.Size(92, 23);
            this.btnMark.TabIndex = 26;
            this.btnMark.Text = "Alles markieren";
            this.btnMark.UseVisualStyleBackColor = true;
            this.btnMark.Click += new System.EventHandler(this.btnMark_Click);
            // 
            // btnFind
            // 
            this.btnFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFind.Location = new System.Drawing.Point(76, 200);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(94, 23);
            this.btnFind.TabIndex = 25;
            this.btnFind.Text = "Weitersuchen";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // lblReplace
            // 
            this.lblReplace.AutoSize = true;
            this.lblReplace.Enabled = false;
            this.lblReplace.Location = new System.Drawing.Point(12, 65);
            this.lblReplace.Name = "lblReplace";
            this.lblReplace.Size = new System.Drawing.Size(81, 13);
            this.lblReplace.TabIndex = 29;
            this.lblReplace.Text = "Ersetzen durch:";
            // 
            // cmbReplace
            // 
            this.cmbReplace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbReplace.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbReplace.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbReplace.Enabled = false;
            this.cmbReplace.FormattingEnabled = true;
            this.cmbReplace.Location = new System.Drawing.Point(12, 81);
            this.cmbReplace.Name = "cmbReplace";
            this.cmbReplace.Size = new System.Drawing.Size(259, 21);
            this.cmbReplace.TabIndex = 22;
            // 
            // ckbFindIn
            // 
            this.ckbFindIn.AutoSize = true;
            this.ckbFindIn.Location = new System.Drawing.Point(12, 108);
            this.ckbFindIn.Name = "ckbFindIn";
            this.ckbFindIn.Size = new System.Drawing.Size(77, 17);
            this.ckbFindIn.TabIndex = 23;
            this.ckbFindIn.Text = "Suchen in:";
            this.ckbFindIn.UseVisualStyleBackColor = true;
            this.ckbFindIn.CheckedChanged += new System.EventHandler(this.ckbFindIn_CheckedChanged);
            // 
            // cmbFindIn
            // 
            this.cmbFindIn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbFindIn.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbFindIn.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbFindIn.Enabled = false;
            this.cmbFindIn.FormattingEnabled = true;
            this.cmbFindIn.Items.AddRange(new object[] {
            "Open Documents"});
            this.cmbFindIn.Location = new System.Drawing.Point(12, 131);
            this.cmbFindIn.Name = "cmbFindIn";
            this.cmbFindIn.Size = new System.Drawing.Size(259, 21);
            this.cmbFindIn.TabIndex = 24;
            this.cmbFindIn.Text = "Offenen Dateien";
            this.cmbFindIn.SelectedIndexChanged += new System.EventHandler(this.cmbFindIn_SelectedIndexChanged);
            // 
            // lblFind
            // 
            this.lblFind.AutoSize = true;
            this.lblFind.Location = new System.Drawing.Point(12, 25);
            this.lblFind.Name = "lblFind";
            this.lblFind.Size = new System.Drawing.Size(47, 13);
            this.lblFind.TabIndex = 28;
            this.lblFind.Text = "Suchen:";
            // 
            // cmbFindText
            // 
            this.cmbFindText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbFindText.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbFindText.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbFindText.FormattingEnabled = true;
            this.cmbFindText.Location = new System.Drawing.Point(12, 41);
            this.cmbFindText.Name = "cmbFindText";
            this.cmbFindText.Size = new System.Drawing.Size(259, 21);
            this.cmbFindText.TabIndex = 21;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.ckbmarked);
            this.groupBox1.Controls.Add(this.ckbWildCard);
            this.groupBox1.Controls.Add(this.ckbUseRegEx);
            this.groupBox1.Controls.Add(this.ckbSearchUp);
            this.groupBox1.Controls.Add(this.ckbMatchWord);
            this.groupBox1.Controls.Add(this.ckbMatchCase);
            this.groupBox1.Location = new System.Drawing.Point(293, 28);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(161, 167);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Options";
            // 
            // ckbmarked
            // 
            this.ckbmarked.AutoSize = true;
            this.ckbmarked.Location = new System.Drawing.Point(15, 134);
            this.ckbmarked.Name = "ckbmarked";
            this.ckbmarked.Size = new System.Drawing.Size(116, 17);
            this.ckbmarked.TabIndex = 11;
            this.ckbmarked.Text = "Nur markierter Text";
            this.ckbmarked.UseVisualStyleBackColor = true;
            this.ckbmarked.CheckedChanged += new System.EventHandler(this.ckbmarked_CheckedChanged);
            // 
            // ckbWildCard
            // 
            this.ckbWildCard.AutoSize = true;
            this.ckbWildCard.Location = new System.Drawing.Point(15, 111);
            this.ckbWildCard.Name = "ckbWildCard";
            this.ckbWildCard.Size = new System.Drawing.Size(77, 17);
            this.ckbWildCard.TabIndex = 10;
            this.ckbWildCard.Text = "Wild Cards";
            this.ckbWildCard.UseVisualStyleBackColor = true;
            // 
            // ckbUseRegEx
            // 
            this.ckbUseRegEx.AutoSize = true;
            this.ckbUseRegEx.Location = new System.Drawing.Point(15, 88);
            this.ckbUseRegEx.Name = "ckbUseRegEx";
            this.ckbUseRegEx.Size = new System.Drawing.Size(123, 17);
            this.ckbUseRegEx.TabIndex = 9;
            this.ckbUseRegEx.Text = "Reguläre Ausdrücke";
            this.ckbUseRegEx.UseVisualStyleBackColor = true;
            // 
            // ckbSearchUp
            // 
            this.ckbSearchUp.AutoSize = true;
            this.ckbSearchUp.Location = new System.Drawing.Point(15, 65);
            this.ckbSearchUp.Name = "ckbSearchUp";
            this.ckbSearchUp.Size = new System.Drawing.Size(105, 17);
            this.ckbSearchUp.TabIndex = 8;
            this.ckbSearchUp.Text = "Aufwärts suchen";
            this.ckbSearchUp.UseVisualStyleBackColor = true;
            // 
            // ckbMatchWord
            // 
            this.ckbMatchWord.AutoSize = true;
            this.ckbMatchWord.Location = new System.Drawing.Point(15, 42);
            this.ckbMatchWord.Name = "ckbMatchWord";
            this.ckbMatchWord.Size = new System.Drawing.Size(144, 17);
            this.ckbMatchWord.TabIndex = 7;
            this.ckbMatchWord.Text = "Nur ganzes Wort suchen";
            this.ckbMatchWord.UseVisualStyleBackColor = true;
            // 
            // ckbMatchCase
            // 
            this.ckbMatchCase.AutoSize = true;
            this.ckbMatchCase.Location = new System.Drawing.Point(15, 19);
            this.ckbMatchCase.Name = "ckbMatchCase";
            this.ckbMatchCase.Size = new System.Drawing.Size(132, 17);
            this.ckbMatchCase.TabIndex = 6;
            this.ckbMatchCase.Text = "Groß-/Kleinschreibung";
            this.ckbMatchCase.UseVisualStyleBackColor = true;
            // 
            // btnStop
            // 
            this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStop.Location = new System.Drawing.Point(338, 200);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 32;
            this.btnStop.Text = "Abbrechen";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Visible = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // Find
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(466, 235);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.lblFilter);
            this.Controls.Add(this.cmbFilter);
            this.Controls.Add(this.btnMark);
            this.Controls.Add(this.btnFind);
            this.Controls.Add(this.lblReplace);
            this.Controls.Add(this.cmbReplace);
            this.Controls.Add(this.ckbFindIn);
            this.Controls.Add(this.cmbFindIn);
            this.Controls.Add(this.lblFind);
            this.Controls.Add(this.cmbFindText);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Find";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Suchen";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbFind;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbReplace;
        private System.Windows.Forms.Label lblFilter;
        private System.Windows.Forms.ComboBox cmbFilter;
        private System.Windows.Forms.Button btnMark;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.Label lblReplace;
        private System.Windows.Forms.ComboBox cmbReplace;
        private System.Windows.Forms.CheckBox ckbFindIn;
        private System.Windows.Forms.ComboBox cmbFindIn;
        private System.Windows.Forms.Label lblFind;
        private System.Windows.Forms.ComboBox cmbFindText;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox ckbWildCard;
        private System.Windows.Forms.CheckBox ckbUseRegEx;
        private System.Windows.Forms.CheckBox ckbSearchUp;
        private System.Windows.Forms.CheckBox ckbMatchWord;
        private System.Windows.Forms.CheckBox ckbMatchCase;
        private System.Windows.Forms.FolderBrowserDialog fbdMain;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.CheckBox ckbmarked;
    }
}