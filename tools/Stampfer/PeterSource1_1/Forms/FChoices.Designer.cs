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
    partial class FChoices
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
            this.label1 = new System.Windows.Forms.Label();
            this.TbName = new System.Windows.Forms.TextBox();
            this.BtOk = new System.Windows.Forms.Button();
            this.TbText = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.BtBack = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Name";
            // 
            // TbName
            // 
            this.TbName.Location = new System.Drawing.Point(12, 25);
            this.TbName.Name = "TbName";
            this.TbName.Size = new System.Drawing.Size(332, 20);
            this.TbName.TabIndex = 0;
            this.TbName.TextChanged += new System.EventHandler(this.TbName_TextChanged);
            this.TbName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TbName_KeyDown);
            // 
            // BtOk
            // 
            this.BtOk.Location = new System.Drawing.Point(350, 25);
            this.BtOk.Name = "BtOk";
            this.BtOk.Size = new System.Drawing.Size(75, 20);
            this.BtOk.TabIndex = 2;
            this.BtOk.Text = "OK";
            this.BtOk.UseVisualStyleBackColor = true;
            this.BtOk.Click += new System.EventHandler(this.BtOk_Click);
            // 
            // TbText
            // 
            this.TbText.Location = new System.Drawing.Point(12, 64);
            this.TbText.Name = "TbText";
            this.TbText.Size = new System.Drawing.Size(330, 20);
            this.TbText.TabIndex = 1;
            this.TbText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TbText_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Text";
            // 
            // BtBack
            // 
            this.BtBack.Location = new System.Drawing.Point(350, 64);
            this.BtBack.Name = "BtBack";
            this.BtBack.Size = new System.Drawing.Size(75, 20);
            this.BtBack.TabIndex = 3;
            this.BtBack.Text = "\"Zurück\"";
            this.BtBack.UseVisualStyleBackColor = true;
            this.BtBack.Click += new System.EventHandler(this.BtBack_Click);
            // 
            // FChoices
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(433, 104);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TbText);
            this.Controls.Add(this.BtBack);
            this.Controls.Add(this.BtOk);
            this.Controls.Add(this.TbName);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FChoices";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Choices";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FChoices_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TbName;
        private System.Windows.Forms.Button BtOk;
        private System.Windows.Forms.TextBox TbText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button BtBack;
    }
}