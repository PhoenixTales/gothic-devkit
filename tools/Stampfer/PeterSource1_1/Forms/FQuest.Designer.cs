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
    partial class FQuest
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
            this.TbName = new System.Windows.Forms.TextBox();
            this.LbQuests1 = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.BtQuest1Add = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.BtQuest2Add = new System.Windows.Forms.Button();
            this.LbQuests2 = new System.Windows.Forms.ListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.TbBeschreibung = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TbTagebuch = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.LbName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.BtXPRemove = new System.Windows.Forms.Button();
            this.BtXPAdd = new System.Windows.Forms.Button();
            this.LbXP = new System.Windows.Forms.ListBox();
            this.BtCreate = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.SuspendLayout();
            // 
            // TbName
            // 
            this.TbName.Location = new System.Drawing.Point(6, 19);
            this.TbName.Name = "TbName";
            this.TbName.Size = new System.Drawing.Size(304, 20);
            this.TbName.TabIndex = 1;
            this.TbName.TextChanged += new System.EventHandler(this.TbName_TextChanged);
            // 
            // LbQuests1
            // 
            this.LbQuests1.FormattingEnabled = true;
            this.LbQuests1.HorizontalScrollbar = true;
            this.LbQuests1.Location = new System.Drawing.Point(6, 19);
            this.LbQuests1.Name = "LbQuests1";
            this.LbQuests1.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.LbQuests1.Size = new System.Drawing.Size(159, 199);
            this.LbQuests1.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.BtQuest1Add);
            this.groupBox1.Controls.Add(this.LbQuests1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(171, 247);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Beeinflusst von ...";
            // 
            // BtQuest1Add
            // 
            this.BtQuest1Add.Location = new System.Drawing.Point(6, 218);
            this.BtQuest1Add.Name = "BtQuest1Add";
            this.BtQuest1Add.Size = new System.Drawing.Size(159, 23);
            this.BtQuest1Add.TabIndex = 5;
            this.BtQuest1Add.Text = "Bearbeiten";
            this.BtQuest1Add.UseVisualStyleBackColor = true;
            this.BtQuest1Add.Click += new System.EventHandler(this.BtQuest1Add_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.BtQuest2Add);
            this.groupBox2.Controls.Add(this.LbQuests2);
            this.groupBox2.Location = new System.Drawing.Point(511, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(171, 247);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Beeinflusst von ...";
            // 
            // BtQuest2Add
            // 
            this.BtQuest2Add.Location = new System.Drawing.Point(6, 218);
            this.BtQuest2Add.Name = "BtQuest2Add";
            this.BtQuest2Add.Size = new System.Drawing.Size(159, 23);
            this.BtQuest2Add.TabIndex = 5;
            this.BtQuest2Add.Text = "Bearbeiten";
            this.BtQuest2Add.UseVisualStyleBackColor = true;
            this.BtQuest2Add.Click += new System.EventHandler(this.BtQuest2Add_Click);
            // 
            // LbQuests2
            // 
            this.LbQuests2.FormattingEnabled = true;
            this.LbQuests2.HorizontalScrollbar = true;
            this.LbQuests2.Location = new System.Drawing.Point(6, 19);
            this.LbQuests2.Name = "LbQuests2";
            this.LbQuests2.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.LbQuests2.Size = new System.Drawing.Size(159, 199);
            this.LbQuests2.TabIndex = 4;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.TbBeschreibung);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.TbTagebuch);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.LbName);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.TbName);
            this.groupBox3.Location = new System.Drawing.Point(189, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(316, 394);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Questname";
            // 
            // TbBeschreibung
            // 
            this.TbBeschreibung.AcceptsReturn = true;
            this.TbBeschreibung.AcceptsTab = true;
            this.TbBeschreibung.Location = new System.Drawing.Point(6, 123);
            this.TbBeschreibung.MaxLength = 128000;
            this.TbBeschreibung.Multiline = true;
            this.TbBeschreibung.Name = "TbBeschreibung";
            this.TbBeschreibung.Size = new System.Drawing.Size(304, 263);
            this.TbBeschreibung.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Questbeschreibung";
            // 
            // TbTagebuch
            // 
            this.TbTagebuch.Location = new System.Drawing.Point(6, 84);
            this.TbTagebuch.Name = "TbTagebuch";
            this.TbTagebuch.Size = new System.Drawing.Size(304, 20);
            this.TbTagebuch.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Titel im Tagebuch";
            // 
            // LbName
            // 
            this.LbName.AutoSize = true;
            this.LbName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbName.Location = new System.Drawing.Point(6, 55);
            this.LbName.Name = "LbName";
            this.LbName.Size = new System.Drawing.Size(0, 13);
            this.LbName.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Interner Name";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.BtXPRemove);
            this.groupBox7.Controls.Add(this.BtXPAdd);
            this.groupBox7.Controls.Add(this.LbXP);
            this.groupBox7.Location = new System.Drawing.Point(12, 265);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(171, 141);
            this.groupBox7.TabIndex = 5;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "XP-Konstanten-Werte";
            // 
            // BtXPRemove
            // 
            this.BtXPRemove.Location = new System.Drawing.Point(6, 110);
            this.BtXPRemove.Name = "BtXPRemove";
            this.BtXPRemove.Size = new System.Drawing.Size(159, 23);
            this.BtXPRemove.TabIndex = 5;
            this.BtXPRemove.Text = "Entfernen";
            this.BtXPRemove.UseVisualStyleBackColor = true;
            this.BtXPRemove.Click += new System.EventHandler(this.BtXPRemove_Click);
            // 
            // BtXPAdd
            // 
            this.BtXPAdd.Location = new System.Drawing.Point(6, 81);
            this.BtXPAdd.Name = "BtXPAdd";
            this.BtXPAdd.Size = new System.Drawing.Size(159, 23);
            this.BtXPAdd.TabIndex = 5;
            this.BtXPAdd.Text = "Hinzufügen";
            this.BtXPAdd.UseVisualStyleBackColor = true;
            this.BtXPAdd.Click += new System.EventHandler(this.LbXPAdd_Click);
            // 
            // LbXP
            // 
            this.LbXP.FormattingEnabled = true;
            this.LbXP.HorizontalScrollbar = true;
            this.LbXP.Location = new System.Drawing.Point(6, 19);
            this.LbXP.Name = "LbXP";
            this.LbXP.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.LbXP.Size = new System.Drawing.Size(159, 56);
            this.LbXP.TabIndex = 4;
            // 
            // BtCreate
            // 
            this.BtCreate.Location = new System.Drawing.Point(517, 375);
            this.BtCreate.Name = "BtCreate";
            this.BtCreate.Size = new System.Drawing.Size(159, 23);
            this.BtCreate.TabIndex = 7;
            this.BtCreate.Text = "Erstellen";
            this.BtCreate.UseVisualStyleBackColor = true;
            this.BtCreate.Click += new System.EventHandler(this.BtCreate_Click);
            // 
            // FQuest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(687, 412);
            this.Controls.Add(this.BtCreate);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FQuest";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quest";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox TbName;
        public System.Windows.Forms.ListBox LbQuests1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        public System.Windows.Forms.ListBox LbQuests2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label LbName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BtQuest1Add;
        private System.Windows.Forms.Button BtQuest2Add;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Button BtXPRemove;
        private System.Windows.Forms.Button BtXPAdd;
        public System.Windows.Forms.ListBox LbXP;
        private System.Windows.Forms.TextBox TbTagebuch;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TbBeschreibung;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button BtCreate;
    }
}