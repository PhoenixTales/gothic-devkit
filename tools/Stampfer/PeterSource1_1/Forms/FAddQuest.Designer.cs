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
    partial class FAddQuest
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.LbQuestsAll = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.LbQuestsAdded = new System.Windows.Forms.ListBox();
            this.BtAdd = new System.Windows.Forms.Button();
            this.BtRemove = new System.Windows.Forms.Button();
            this.BtCreate = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.LbQuestsAll);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(216, 216);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Pool";
            // 
            // LbQuestsAll
            // 
            this.LbQuestsAll.FormattingEnabled = true;
            this.LbQuestsAll.Location = new System.Drawing.Point(6, 19);
            this.LbQuestsAll.Name = "LbQuestsAll";
            this.LbQuestsAll.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.LbQuestsAll.Size = new System.Drawing.Size(204, 186);
            this.LbQuestsAll.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.LbQuestsAdded);
            this.groupBox2.Location = new System.Drawing.Point(400, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(216, 216);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Auswahl";
            // 
            // LbQuestsAdded
            // 
            this.LbQuestsAdded.FormattingEnabled = true;
            this.LbQuestsAdded.Location = new System.Drawing.Point(6, 19);
            this.LbQuestsAdded.Name = "LbQuestsAdded";
            this.LbQuestsAdded.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.LbQuestsAdded.Size = new System.Drawing.Size(204, 186);
            this.LbQuestsAdded.TabIndex = 0;
            // 
            // BtAdd
            // 
            this.BtAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtAdd.Location = new System.Drawing.Point(234, 89);
            this.BtAdd.Name = "BtAdd";
            this.BtAdd.Size = new System.Drawing.Size(160, 25);
            this.BtAdd.TabIndex = 1;
            this.BtAdd.Text = "===>";
            this.BtAdd.UseVisualStyleBackColor = true;
            this.BtAdd.Click += new System.EventHandler(this.BtAdd_Click);
            // 
            // BtRemove
            // 
            this.BtRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtRemove.Location = new System.Drawing.Point(234, 120);
            this.BtRemove.Name = "BtRemove";
            this.BtRemove.Size = new System.Drawing.Size(160, 25);
            this.BtRemove.TabIndex = 1;
            this.BtRemove.Text = "<===";
            this.BtRemove.UseVisualStyleBackColor = true;
            this.BtRemove.Click += new System.EventHandler(this.BtRemove_Click);
            // 
            // BtCreate
            // 
            this.BtCreate.Location = new System.Drawing.Point(234, 203);
            this.BtCreate.Name = "BtCreate";
            this.BtCreate.Size = new System.Drawing.Size(159, 24);
            this.BtCreate.TabIndex = 2;
            this.BtCreate.Text = "Übernehmen";
            this.BtCreate.UseVisualStyleBackColor = true;
            this.BtCreate.Click += new System.EventHandler(this.BtCreate_Click);
            // 
            // FAddQuest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(628, 240);
            this.Controls.Add(this.BtCreate);
            this.Controls.Add(this.BtRemove);
            this.Controls.Add(this.BtAdd);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FAddQuest";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quests hinzufügen";
            this.TopMost = true;
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox LbQuestsAll;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox LbQuestsAdded;
        private System.Windows.Forms.Button BtAdd;
        private System.Windows.Forms.Button BtRemove;
        private System.Windows.Forms.Button BtCreate;
    }
}