namespace GVE
{
    partial class FMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FMain));
            this.MMenu = new System.Windows.Forms.MenuStrip();
            this.savegameMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadNewestMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.ListAllMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.OptionsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpM = new System.Windows.Forms.ToolStripMenuItem();
            this.Info = new System.Windows.Forms.ToolStripMenuItem();
            this.OFDOpen = new System.Windows.Forms.OpenFileDialog();
            this.BtBackup = new System.Windows.Forms.Button();
            this.BtSave = new System.Windows.Forms.Button();
            this.TSearch = new System.Windows.Forms.TextBox();
            this.BtSearch = new System.Windows.Forms.Button();
            this.CKBAusblenden = new System.Windows.Forms.CheckBox();
            this.BtPrev = new System.Windows.Forms.Button();
            this.BtNext = new System.Windows.Forms.Button();
            this.Data = new System.Windows.Forms.DataGridView();
            this.Variable = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Wert = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BtRestore = new System.Windows.Forms.Button();
            this.PButtons = new System.Windows.Forms.Panel();
            this.CKBAutoSearch = new System.Windows.Forms.CheckBox();
            this.WaitTimer = new System.Windows.Forms.Timer(this.components);
            this.MMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Data)).BeginInit();
            this.PButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // MMenu
            // 
            this.MMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.savegameMenu,
            this.OptionsMenu,
            this.HelpMenu});
            resources.ApplyResources(this.MMenu, "MMenu");
            this.MMenu.Name = "MMenu";
            // 
            // savegameMenu
            // 
            this.savegameMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LoadNewestMenu,
            this.LoadMenu,
            this.ListAllMenu});
            this.savegameMenu.Name = "savegameMenu";
            resources.ApplyResources(this.savegameMenu, "savegameMenu");
            // 
            // LoadNewestMenu
            // 
            this.LoadNewestMenu.Name = "LoadNewestMenu";
            resources.ApplyResources(this.LoadNewestMenu, "LoadNewestMenu");
            this.LoadNewestMenu.Click += new System.EventHandler(this.aktuellstesLadenToolStripMenuItem_Click);
            // 
            // LoadMenu
            // 
            this.LoadMenu.Name = "LoadMenu";
            resources.ApplyResources(this.LoadMenu, "LoadMenu");
            this.LoadMenu.Click += new System.EventHandler(this.manuellLadenToolStripMenuItem_Click);
            // 
            // ListAllMenu
            // 
            this.ListAllMenu.Name = "ListAllMenu";
            resources.ApplyResources(this.ListAllMenu, "ListAllMenu");
            this.ListAllMenu.Click += new System.EventHandler(this.alleSavegamesAuflistenToolStripMenuItem_Click);
            // 
            // OptionsMenu
            // 
            this.OptionsMenu.Name = "OptionsMenu";
            resources.ApplyResources(this.OptionsMenu, "OptionsMenu");
            this.OptionsMenu.Click += new System.EventHandler(this.einstellungenToolStripMenuItem_Click);
            // 
            // HelpMenu
            // 
            this.HelpMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.HelpM,
            this.Info});
            this.HelpMenu.Name = "HelpMenu";
            resources.ApplyResources(this.HelpMenu, "HelpMenu");
            // 
            // HelpM
            // 
            this.HelpM.Name = "HelpM";
            resources.ApplyResources(this.HelpM, "HelpM");
            this.HelpM.Click += new System.EventHandler(this.hilfeToolStripMenuItem_Click);
            // 
            // Info
            // 
            this.Info.Name = "Info";
            resources.ApplyResources(this.Info, "Info");
            this.Info.Click += new System.EventHandler(this.infoToolStripMenuItem_Click);
            // 
            // OFDOpen
            // 
            this.OFDOpen.DefaultExt = "\"SAV\"";
            resources.ApplyResources(this.OFDOpen, "OFDOpen");
            // 
            // BtBackup
            // 
            resources.ApplyResources(this.BtBackup, "BtBackup");
            this.BtBackup.Name = "BtBackup";
            this.BtBackup.UseVisualStyleBackColor = true;
            this.BtBackup.Click += new System.EventHandler(this.BtBackup_Click);
            // 
            // BtSave
            // 
            resources.ApplyResources(this.BtSave, "BtSave");
            this.BtSave.Name = "BtSave";
            this.BtSave.UseVisualStyleBackColor = true;
            this.BtSave.Click += new System.EventHandler(this.BtSave_Click);
            // 
            // TSearch
            // 
            resources.ApplyResources(this.TSearch, "TSearch");
            this.TSearch.Name = "TSearch";
            this.TSearch.TextChanged += new System.EventHandler(this.TSearch_TextChanged);
            this.TSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TSearch_KeyDown);
            // 
            // BtSearch
            // 
            resources.ApplyResources(this.BtSearch, "BtSearch");
            this.BtSearch.Name = "BtSearch";
            this.BtSearch.UseVisualStyleBackColor = true;
            this.BtSearch.Click += new System.EventHandler(this.BtSearch_Click);
            // 
            // CKBAusblenden
            // 
            resources.ApplyResources(this.CKBAusblenden, "CKBAusblenden");
            this.CKBAusblenden.Name = "CKBAusblenden";
            this.CKBAusblenden.UseVisualStyleBackColor = true;
            this.CKBAusblenden.CheckedChanged += new System.EventHandler(this.CKBAusblenden_CheckedChanged);
            // 
            // BtPrev
            // 
            resources.ApplyResources(this.BtPrev, "BtPrev");
            this.BtPrev.Name = "BtPrev";
            this.BtPrev.UseVisualStyleBackColor = true;
            this.BtPrev.Click += new System.EventHandler(this.BtPrev_Click);
            // 
            // BtNext
            // 
            resources.ApplyResources(this.BtNext, "BtNext");
            this.BtNext.Name = "BtNext";
            this.BtNext.UseVisualStyleBackColor = true;
            this.BtNext.Click += new System.EventHandler(this.BtNext_Click);
            // 
            // Data
            // 
            this.Data.AllowUserToAddRows = false;
            this.Data.AllowUserToDeleteRows = false;
            this.Data.AllowUserToOrderColumns = true;
            this.Data.AllowUserToResizeRows = false;
            this.Data.BackgroundColor = System.Drawing.SystemColors.Control;
            this.Data.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Data.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.Data.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Variable,
            this.Wert});
            resources.ApplyResources(this.Data, "Data");
            this.Data.MultiSelect = false;
            this.Data.Name = "Data";
            this.Data.RowHeadersVisible = false;
            this.Data.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            // 
            // Variable
            // 
            this.Variable.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            resources.ApplyResources(this.Variable, "Variable");
            this.Variable.Name = "Variable";
            // 
            // Wert
            // 
            this.Wert.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            resources.ApplyResources(this.Wert, "Wert");
            this.Wert.Name = "Wert";
            // 
            // BtRestore
            // 
            resources.ApplyResources(this.BtRestore, "BtRestore");
            this.BtRestore.Name = "BtRestore";
            this.BtRestore.UseVisualStyleBackColor = true;
            this.BtRestore.Click += new System.EventHandler(this.BtRestore_Click);
            // 
            // PButtons
            // 
            this.PButtons.Controls.Add(this.BtSave);
            this.PButtons.Controls.Add(this.BtRestore);
            this.PButtons.Controls.Add(this.BtBackup);
            resources.ApplyResources(this.PButtons, "PButtons");
            this.PButtons.Name = "PButtons";
            // 
            // CKBAutoSearch
            // 
            resources.ApplyResources(this.CKBAutoSearch, "CKBAutoSearch");
            this.CKBAutoSearch.Checked = true;
            this.CKBAutoSearch.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CKBAutoSearch.Name = "CKBAutoSearch";
            this.CKBAutoSearch.UseVisualStyleBackColor = true;
            this.CKBAutoSearch.CheckedChanged += new System.EventHandler(this.CKBAutoSearch_CheckedChanged);
            // 
            // WaitTimer
            // 
            this.WaitTimer.Enabled = true;
            this.WaitTimer.Tick += new System.EventHandler(this.WaitTimer_Tick);
            // 
            // FMain
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.CKBAutoSearch);
            this.Controls.Add(this.PButtons);
            this.Controls.Add(this.Data);
            this.Controls.Add(this.BtNext);
            this.Controls.Add(this.BtPrev);
            this.Controls.Add(this.CKBAusblenden);
            this.Controls.Add(this.BtSearch);
            this.Controls.Add(this.TSearch);
            this.Controls.Add(this.MMenu);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.MMenu;
            this.Name = "FMain";
            this.Load += new System.EventHandler(this.FMain_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FMain_FormClosing);
            this.Resize += new System.EventHandler(this.FMain_Resize);
            this.MMenu.ResumeLayout(false);
            this.MMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Data)).EndInit();
            this.PButtons.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MMenu;
        private System.Windows.Forms.ToolStripMenuItem savegameMenu;
        private System.Windows.Forms.ToolStripMenuItem LoadNewestMenu;
        private System.Windows.Forms.ToolStripMenuItem LoadMenu;
        private System.Windows.Forms.ToolStripMenuItem OptionsMenu;
        private System.Windows.Forms.OpenFileDialog OFDOpen;
        private System.Windows.Forms.Button BtBackup;
        private System.Windows.Forms.Button BtSave;
        private System.Windows.Forms.TextBox TSearch;
        private System.Windows.Forms.Button BtSearch;
        private System.Windows.Forms.CheckBox CKBAusblenden;
        private System.Windows.Forms.Button BtPrev;
        private System.Windows.Forms.Button BtNext;
        private System.Windows.Forms.DataGridView Data;
        private System.Windows.Forms.Button BtRestore;
        private System.Windows.Forms.Panel PButtons;
        private System.Windows.Forms.CheckBox CKBAutoSearch;
        private System.Windows.Forms.ToolStripMenuItem ListAllMenu;
        private System.Windows.Forms.ToolStripMenuItem HelpMenu;
        private System.Windows.Forms.ToolStripMenuItem HelpM;
        private System.Windows.Forms.ToolStripMenuItem Info;
        private System.Windows.Forms.DataGridViewTextBoxColumn Variable;
        private System.Windows.Forms.DataGridViewTextBoxColumn Wert;
        private System.Windows.Forms.Timer WaitTimer;
    }
}

