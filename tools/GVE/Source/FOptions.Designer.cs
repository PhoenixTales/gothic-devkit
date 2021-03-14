namespace GVE
{
    partial class FOptions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FOptions));
            this.GbPath = new System.Windows.Forms.GroupBox();
            this.BtBrowse = new System.Windows.Forms.Button();
            this.TPath = new System.Windows.Forms.TextBox();
            this.BtOk = new System.Windows.Forms.Button();
            this.FBD = new System.Windows.Forms.FolderBrowserDialog();
            this.CKBBackup = new System.Windows.Forms.CheckBox();
            this.CKBAutoload = new System.Windows.Forms.CheckBox();
            this.CbLanguage = new System.Windows.Forms.ComboBox();
            this.LbLanguage = new System.Windows.Forms.Label();
            this.GbPath.SuspendLayout();
            this.SuspendLayout();
            // 
            // GbPath
            // 
            this.GbPath.Controls.Add(this.BtBrowse);
            this.GbPath.Controls.Add(this.TPath);
            this.GbPath.Location = new System.Drawing.Point(10, 15);
            this.GbPath.Name = "GbPath";
            this.GbPath.Size = new System.Drawing.Size(462, 52);
            this.GbPath.TabIndex = 0;
            this.GbPath.TabStop = false;
            this.GbPath.Text = "Pfad zum Gothic-Ordner";
            // 
            // BtBrowse
            // 
            this.BtBrowse.Location = new System.Drawing.Point(424, 17);
            this.BtBrowse.Name = "BtBrowse";
            this.BtBrowse.Size = new System.Drawing.Size(25, 23);
            this.BtBrowse.TabIndex = 2;
            this.BtBrowse.Text = "...";
            this.BtBrowse.UseVisualStyleBackColor = true;
            this.BtBrowse.Click += new System.EventHandler(this.BtBrowse_Click);
            // 
            // TPath
            // 
            this.TPath.Location = new System.Drawing.Point(6, 19);
            this.TPath.MaxLength = 2;
            this.TPath.Name = "TPath";
            this.TPath.Size = new System.Drawing.Size(412, 20);
            this.TPath.TabIndex = 0;
            // 
            // BtOk
            // 
            this.BtOk.Location = new System.Drawing.Point(280, 114);
            this.BtOk.Name = "BtOk";
            this.BtOk.Size = new System.Drawing.Size(192, 45);
            this.BtOk.TabIndex = 1;
            this.BtOk.Text = "Übernehmen";
            this.BtOk.UseVisualStyleBackColor = true;
            this.BtOk.Click += new System.EventHandler(this.BtOk_Click);
            // 
            // CKBBackup
            // 
            this.CKBBackup.AutoSize = true;
            this.CKBBackup.Location = new System.Drawing.Point(16, 73);
            this.CKBBackup.Name = "CKBBackup";
            this.CKBBackup.Size = new System.Drawing.Size(241, 17);
            this.CKBBackup.TabIndex = 2;
            this.CKBBackup.Text = "Beim Speichern automatisch Backup anlegen";
            this.CKBBackup.UseVisualStyleBackColor = true;
            this.CKBBackup.CheckedChanged += new System.EventHandler(this.CKBBackup_CheckedChanged);
            // 
            // CKBAutoload
            // 
            this.CKBAutoload.AutoSize = true;
            this.CKBAutoload.Location = new System.Drawing.Point(16, 96);
            this.CKBAutoload.Name = "CKBAutoload";
            this.CKBAutoload.Size = new System.Drawing.Size(178, 17);
            this.CKBAutoload.TabIndex = 3;
            this.CKBAutoload.Text = "Automatisches Laden beim Start";
            this.CKBAutoload.UseVisualStyleBackColor = true;
            this.CKBAutoload.CheckedChanged += new System.EventHandler(this.CKBAutoload_CheckedChanged);
            // 
            // CbLanguage
            // 
            this.CbLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CbLanguage.FormattingEnabled = true;
            this.CbLanguage.Location = new System.Drawing.Point(16, 138);
            this.CbLanguage.Name = "CbLanguage";
            this.CbLanguage.Size = new System.Drawing.Size(178, 21);
            this.CbLanguage.TabIndex = 4;
            this.CbLanguage.SelectedIndexChanged += new System.EventHandler(this.CbLanguage_SelectedIndexChanged);
            // 
            // LbLanguage
            // 
            this.LbLanguage.AutoSize = true;
            this.LbLanguage.Location = new System.Drawing.Point(13, 122);
            this.LbLanguage.Name = "LbLanguage";
            this.LbLanguage.Size = new System.Drawing.Size(50, 13);
            this.LbLanguage.TabIndex = 5;
            this.LbLanguage.Text = "Sprache:";
            // 
            // FOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 171);
            this.Controls.Add(this.LbLanguage);
            this.Controls.Add(this.CbLanguage);
            this.Controls.Add(this.CKBAutoload);
            this.Controls.Add(this.CKBBackup);
            this.Controls.Add(this.BtOk);
            this.Controls.Add(this.GbPath);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FOptions";
            this.Text = "Einstellungen";
            this.Load += new System.EventHandler(this.FOptions_Load);
            this.GbPath.ResumeLayout(false);
            this.GbPath.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox GbPath;
        private System.Windows.Forms.TextBox TPath;
        private System.Windows.Forms.Button BtOk;
        private System.Windows.Forms.Button BtBrowse;
        private System.Windows.Forms.FolderBrowserDialog FBD;
        private System.Windows.Forms.CheckBox CKBBackup;
        private System.Windows.Forms.CheckBox CKBAutoload;
        private System.Windows.Forms.ComboBox CbLanguage;
        private System.Windows.Forms.Label LbLanguage;
    }
}