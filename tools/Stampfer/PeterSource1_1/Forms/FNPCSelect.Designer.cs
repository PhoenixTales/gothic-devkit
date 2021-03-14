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
namespace Peter.Forms
{
    partial class FNPCSelect
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.LbNPCs = new System.Windows.Forms.ListBox();
            this.BtCreate = new System.Windows.Forms.Button();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.TFind = new System.Windows.Forms.ToolStripTextBox();
            this.BtNext = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "NPC";
            // 
            // LbNPCs
            // 
            this.LbNPCs.FormattingEnabled = true;
            this.LbNPCs.Location = new System.Drawing.Point(3, 51);
            this.LbNPCs.Name = "LbNPCs";
            this.LbNPCs.Size = new System.Drawing.Size(276, 407);
            this.LbNPCs.TabIndex = 1;
            this.LbNPCs.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.LbNPCs_MouseDoubleClick);
            // 
            // BtCreate
            // 
            this.BtCreate.Location = new System.Drawing.Point(3, 468);
            this.BtCreate.Name = "BtCreate";
            this.BtCreate.Size = new System.Drawing.Size(275, 33);
            this.BtCreate.TabIndex = 2;
            this.BtCreate.Text = "Verwenden";
            this.BtCreate.UseVisualStyleBackColor = true;
            this.BtCreate.Click += new System.EventHandler(this.BtCreate_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TFind,
            this.toolStripSeparator1,
            this.BtNext});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(282, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // TFind
            // 
            this.TFind.Name = "TFind";
            this.TFind.Size = new System.Drawing.Size(200, 25);
            this.TFind.TextChanged += new System.EventHandler(this.TFind_TextChanged);
            // 
            // BtNext
            // 
            this.BtNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtNext.Image = global::Peter.InternalImages.Next;
            this.BtNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtNext.Name = "BtNext";
            this.BtNext.Size = new System.Drawing.Size(23, 22);
            this.BtNext.Text = "Weiter";
            this.BtNext.Click += new System.EventHandler(this.BtNext_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // FNPCSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 505);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.BtCreate);
            this.Controls.Add(this.LbNPCs);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Menu = this.mainMenu1;
            this.Name = "FNPCSelect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "NPC";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox LbNPCs;
        private System.Windows.Forms.Button BtCreate;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripTextBox TFind;
        private System.Windows.Forms.ToolStripButton BtNext;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}