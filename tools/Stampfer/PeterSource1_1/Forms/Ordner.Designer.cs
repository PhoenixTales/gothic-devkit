/**************************************************************************************
    Stampfer - Gothic Script Editor
    Copyright (C)  2009 Alexander "Sumpfkrautjunkie" Ruppert

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
**************************************************************************************/namespace Peter.Forms
{
    partial class FOrdner
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
            this.BtCreate = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.TbBeschreibung = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Ordnername";
            // 
            // TbName
            // 
            this.TbName.Dock = System.Windows.Forms.DockStyle.Top;
            this.TbName.Location = new System.Drawing.Point(0, 13);
            this.TbName.Name = "TbName";
            this.TbName.Size = new System.Drawing.Size(292, 20);
            this.TbName.TabIndex = 1;
            // 
            // BtCreate
            // 
            this.BtCreate.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BtCreate.Location = new System.Drawing.Point(0, 233);
            this.BtCreate.Name = "BtCreate";
            this.BtCreate.Size = new System.Drawing.Size(292, 27);
            this.BtCreate.TabIndex = 2;
            this.BtCreate.Text = "Erstellen";
            this.BtCreate.UseVisualStyleBackColor = true;
            this.BtCreate.Click += new System.EventHandler(this.BtCreate_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Beschreibung";
            // 
            // TbBeschreibung
            // 
            this.TbBeschreibung.AcceptsReturn = true;
            this.TbBeschreibung.AcceptsTab = true;
            this.TbBeschreibung.Dock = System.Windows.Forms.DockStyle.Top;
            this.TbBeschreibung.Location = new System.Drawing.Point(0, 46);
            this.TbBeschreibung.MaxLength = 65534;
            this.TbBeschreibung.Multiline = true;
            this.TbBeschreibung.Name = "TbBeschreibung";
            this.TbBeschreibung.Size = new System.Drawing.Size(292, 187);
            this.TbBeschreibung.TabIndex = 4;
            // 
            // FOrdner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 260);
            this.Controls.Add(this.BtCreate);
            this.Controls.Add(this.TbBeschreibung);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TbName);
            this.Controls.Add(this.label1);
            this.Name = "FOrdner";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ordner";
            this.TopMost = true;
            this.Resize += new System.EventHandler(this.FOrdner_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TbName;
        private System.Windows.Forms.Button BtCreate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TbBeschreibung;
    }
}