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
    partial class FBeklauen
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.nDex = new System.Windows.Forms.NumericUpDown();
            this.nGold = new System.Windows.Forms.NumericUpDown();
            this.TbDescription = new System.Windows.Forms.TextBox();
            this.BtCreate = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nDex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nGold)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Geschick";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Gold";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Description";
            // 
            // nDex
            // 
            this.nDex.Location = new System.Drawing.Point(15, 25);
            this.nDex.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nDex.Name = "nDex";
            this.nDex.Size = new System.Drawing.Size(188, 20);
            this.nDex.TabIndex = 0;
            this.nDex.ValueChanged += new System.EventHandler(this.nDex_ValueChanged);
            // 
            // nGold
            // 
            this.nGold.Location = new System.Drawing.Point(15, 64);
            this.nGold.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nGold.Name = "nGold";
            this.nGold.Size = new System.Drawing.Size(188, 20);
            this.nGold.TabIndex = 1;
            this.nGold.KeyDown += new System.Windows.Forms.KeyEventHandler(this.nGold_KeyDown);
            // 
            // TbDescription
            // 
            this.TbDescription.Location = new System.Drawing.Point(15, 103);
            this.TbDescription.Name = "TbDescription";
            this.TbDescription.Size = new System.Drawing.Size(188, 20);
            this.TbDescription.TabIndex = 2;
            // 
            // BtCreate
            // 
            this.BtCreate.Location = new System.Drawing.Point(15, 130);
            this.BtCreate.Name = "BtCreate";
            this.BtCreate.Size = new System.Drawing.Size(188, 34);
            this.BtCreate.TabIndex = 3;
            this.BtCreate.Text = "Erstellen";
            this.BtCreate.UseVisualStyleBackColor = true;
            this.BtCreate.Click += new System.EventHandler(this.BtCreate_Click);
            // 
            // FBeklauen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(215, 173);
            this.Controls.Add(this.BtCreate);
            this.Controls.Add(this.TbDescription);
            this.Controls.Add(this.nGold);
            this.Controls.Add(this.nDex);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FBeklauen";
            this.Text = "Beklauen";
            ((System.ComponentModel.ISupportInitialize)(this.nDex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nGold)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nDex;
        private System.Windows.Forms.NumericUpDown nGold;
        private System.Windows.Forms.TextBox TbDescription;
        private System.Windows.Forms.Button BtCreate;
    }
}