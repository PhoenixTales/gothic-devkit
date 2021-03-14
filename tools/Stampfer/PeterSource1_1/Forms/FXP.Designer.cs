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
    partial class FXP
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
            this.BtCreate = new System.Windows.Forms.Button();
            this.NXP = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.NXP)).BeginInit();
            this.SuspendLayout();
            // 
            // BtCreate
            // 
            this.BtCreate.Location = new System.Drawing.Point(12, 38);
            this.BtCreate.Name = "BtCreate";
            this.BtCreate.Size = new System.Drawing.Size(120, 23);
            this.BtCreate.TabIndex = 1;
            this.BtCreate.Text = "Erstellen";
            this.BtCreate.UseVisualStyleBackColor = true;
            this.BtCreate.Click += new System.EventHandler(this.BtCreate_Click);
            // 
            // NXP
            // 
            this.NXP.Location = new System.Drawing.Point(12, 12);
            this.NXP.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.NXP.Name = "NXP";
            this.NXP.Size = new System.Drawing.Size(120, 20);
            this.NXP.TabIndex = 0;
            this.NXP.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NXP_KeyDown);
            // 
            // FXP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(141, 70);
            this.Controls.Add(this.NXP);
            this.Controls.Add(this.BtCreate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FXP";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "XP";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.NXP)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtCreate;
        private System.Windows.Forms.NumericUpDown NXP;
    }
}