/**************************************************************************************
    Stampfer - Gothic Script Editor
    Copyright (C) 2008 Jpmon1

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
**************************************************************************************/namespace Peter
{
    partial class FileDifference
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
            this.ofdMain = new System.Windows.Forms.OpenFileDialog();
            this._ckbIgnoreCase = new System.Windows.Forms.CheckBox();
            this._cmbFile2 = new System.Windows.Forms.ComboBox();
            this._cmbFile1 = new System.Windows.Forms.ComboBox();
            this._ckbIgnoreSpace = new System.Windows.Forms.CheckBox();
            this._ckbTrimSpace = new System.Windows.Forms.CheckBox();
            //this.webber1 = new Peter.Webber();
            this._grprOptions = new Peter.Grouper();
            this._btnBrowseFile2 = new Peter.VistaButton();
            this._btnBrowseFile1 = new Peter.VistaButton();
            this._btnCompare = new Peter.VistaButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this._grprOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // ofdMain
            // 
            this.ofdMain.Title = "Please Select a File";
            // 
            // _ckbIgnoreCase
            // 
            this._ckbIgnoreCase.AutoSize = true;
            this._ckbIgnoreCase.Location = new System.Drawing.Point(300, 28);
            this._ckbIgnoreCase.Name = "_ckbIgnoreCase";
            this._ckbIgnoreCase.Size = new System.Drawing.Size(181, 17);
            this._ckbIgnoreCase.TabIndex = 7;
            this._ckbIgnoreCase.Text = "Groﬂ/-Kleinschreibung ignorieren";
            this._ckbIgnoreCase.UseVisualStyleBackColor = true;
            // 
            // _cmbFile2
            // 
            this._cmbFile2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._cmbFile2.FormattingEnabled = true;
            this._cmbFile2.Location = new System.Drawing.Point(23, 104);
            this._cmbFile2.Name = "_cmbFile2";
            this._cmbFile2.Size = new System.Drawing.Size(660, 21);
            this._cmbFile2.TabIndex = 1;
            // 
            // _cmbFile1
            // 
            this._cmbFile1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._cmbFile1.FormattingEnabled = true;
            this._cmbFile1.Location = new System.Drawing.Point(23, 64);
            this._cmbFile1.Name = "_cmbFile1";
            this._cmbFile1.Size = new System.Drawing.Size(660, 21);
            this._cmbFile1.TabIndex = 0;
            // 
            // _ckbIgnoreSpace
            // 
            this._ckbIgnoreSpace.AutoSize = true;
            this._ckbIgnoreSpace.Location = new System.Drawing.Point(161, 28);
            this._ckbIgnoreSpace.Name = "_ckbIgnoreSpace";
            this._ckbIgnoreSpace.Size = new System.Drawing.Size(133, 17);
            this._ckbIgnoreSpace.TabIndex = 6;
            this._ckbIgnoreSpace.Text = "Leerzeichen ignorieren";
            this._ckbIgnoreSpace.UseVisualStyleBackColor = true;
            // 
            // _ckbTrimSpace
            // 
            this._ckbTrimSpace.AutoSize = true;
            this._ckbTrimSpace.Location = new System.Drawing.Point(23, 28);
            this._ckbTrimSpace.Name = "_ckbTrimSpace";
            this._ckbTrimSpace.Size = new System.Drawing.Size(132, 17);
            this._ckbTrimSpace.TabIndex = 5;
            this._ckbTrimSpace.Text = "Leerzeichen entfernen";
            this._ckbTrimSpace.UseVisualStyleBackColor = true;
            // 
            // webber1
            // 
           /* this.webber1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webber1.Location = new System.Drawing.Point(0, 135);
            this.webber1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webber1.Name = "webber1";
            this.webber1.Size = new System.Drawing.Size(737, 374);
            this.webber1.TabIndex = 3;
            this.webber1.Url = new System.Uri("about:Blank", System.UriKind.Absolute);
           */ // 
            // _grprOptions
            // 
            this._grprOptions.BackgroundColor = System.Drawing.Color.White;
            this._grprOptions.BackgroundGradientColor = System.Drawing.Color.White;
            this._grprOptions.BackgroundGradientMode = Peter.Grouper.GroupBoxGradientMode.None;
            this._grprOptions.BorderColor = System.Drawing.Color.Black;
            this._grprOptions.BorderThickness = 1F;
            this._grprOptions.Controls.Add(this._btnBrowseFile2);
            this._grprOptions.Controls.Add(this._btnBrowseFile1);
            this._grprOptions.Controls.Add(this._btnCompare);
            this._grprOptions.Controls.Add(this.label2);
            this._grprOptions.Controls.Add(this.label1);
            this._grprOptions.Controls.Add(this._ckbIgnoreCase);
            this._grprOptions.Controls.Add(this._cmbFile2);
            this._grprOptions.Controls.Add(this._ckbTrimSpace);
            this._grprOptions.Controls.Add(this._ckbIgnoreSpace);
            this._grprOptions.Controls.Add(this._cmbFile1);
            this._grprOptions.CustomGroupBoxColor = System.Drawing.Color.White;
            this._grprOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this._grprOptions.GroupImage = null;
            this._grprOptions.GroupTitle = "Optionen";
            this._grprOptions.Location = new System.Drawing.Point(0, 0);
            this._grprOptions.Name = "_grprOptions";
            this._grprOptions.Padding = new System.Windows.Forms.Padding(20);
            this._grprOptions.PaintGroupBox = false;
            this._grprOptions.RoundCorners = 10;
            this._grprOptions.ShadowColor = System.Drawing.Color.DarkGray;
            this._grprOptions.ShadowControl = true;
            this._grprOptions.ShadowThickness = 3;
            this._grprOptions.Size = new System.Drawing.Size(737, 135);
            this._grprOptions.TabIndex = 6;
            // 
            // _btnBrowseFile2
            // 
            this._btnBrowseFile2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._btnBrowseFile2.BackColor = System.Drawing.Color.Transparent;
            this._btnBrowseFile2.BaseColor = System.Drawing.Color.Transparent;
            this._btnBrowseFile2.ButtonColor = System.Drawing.Color.Silver;
            this._btnBrowseFile2.ButtonText = "...";
            this._btnBrowseFile2.CornerRadius = 0;
            this._btnBrowseFile2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._btnBrowseFile2.ForeColor = System.Drawing.Color.Black;
            this._btnBrowseFile2.Location = new System.Drawing.Point(689, 99);
            this._btnBrowseFile2.Name = "_btnBrowseFile2";
            this._btnBrowseFile2.Size = new System.Drawing.Size(31, 26);
            this._btnBrowseFile2.TabIndex = 12;
            this._btnBrowseFile2.Click += new System.EventHandler(this._btnBrowseFile2_Click);
            // 
            // _btnBrowseFile1
            // 
            this._btnBrowseFile1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._btnBrowseFile1.BackColor = System.Drawing.Color.Transparent;
            this._btnBrowseFile1.BaseColor = System.Drawing.Color.Transparent;
            this._btnBrowseFile1.ButtonColor = System.Drawing.Color.Silver;
            this._btnBrowseFile1.ButtonText = "...";
            this._btnBrowseFile1.CornerRadius = 0;
            this._btnBrowseFile1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._btnBrowseFile1.ForeColor = System.Drawing.Color.Black;
            this._btnBrowseFile1.Location = new System.Drawing.Point(689, 61);
            this._btnBrowseFile1.Name = "_btnBrowseFile1";
            this._btnBrowseFile1.Size = new System.Drawing.Size(31, 26);
            this._btnBrowseFile1.TabIndex = 11;
            this._btnBrowseFile1.Click += new System.EventHandler(this._btnBrowseFile1_Click);
            // 
            // _btnCompare
            // 
            this._btnCompare.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._btnCompare.BackColor = System.Drawing.Color.Transparent;
            this._btnCompare.BaseColor = System.Drawing.Color.Transparent;
            this._btnCompare.ButtonColor = System.Drawing.Color.Silver;
            this._btnCompare.ButtonText = "Vergleichen";
            this._btnCompare.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._btnCompare.ForeColor = System.Drawing.Color.Black;
            this._btnCompare.Location = new System.Drawing.Point(620, 23);
            this._btnCompare.Name = "_btnCompare";
            this._btnCompare.Size = new System.Drawing.Size(100, 32);
            this._btnCompare.TabIndex = 10;
            this._btnCompare.Click += new System.EventHandler(this._btnCompare_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Datei 2:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Datei 1:";
            // 
            // FileDifference
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(737, 509);
           // this.Controls.Add(this.webber1);
            this.Controls.Add(this._grprOptions);
            this.Name = "FileDifference";
            this.TabText = "FileDifference";
            this.Text = "Dateivergleich";
            this._grprOptions.ResumeLayout(false);
            this._grprOptions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog ofdMain;
        private System.Windows.Forms.ComboBox _cmbFile2;
        private System.Windows.Forms.ComboBox _cmbFile1;
        private System.Windows.Forms.CheckBox _ckbIgnoreCase;
        private System.Windows.Forms.CheckBox _ckbIgnoreSpace;
        private System.Windows.Forms.CheckBox _ckbTrimSpace;
       // private Webber webber1;
        private Grouper _grprOptions;
        private VistaButton _btnCompare;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private VistaButton _btnBrowseFile2;
        private VistaButton _btnBrowseFile1;
    }
}