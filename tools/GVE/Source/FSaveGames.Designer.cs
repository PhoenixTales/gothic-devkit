namespace GVE
{
    partial class FSaveGames
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FSaveGames));
            this.TFiles = new System.Windows.Forms.TreeView();
            this.BtAbort = new System.Windows.Forms.Button();
            this.BtOk = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lbDatum = new System.Windows.Forms.Label();
            this.lbWelt = new System.Windows.Forms.Label();
            this.lbZeit = new System.Windows.Forms.Label();
            this.lbSekunden = new System.Windows.Forms.Label();
            this.images1 = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // TFiles
            // 
            this.TFiles.Location = new System.Drawing.Point(12, 12);
            this.TFiles.Name = "TFiles";
            this.TFiles.Size = new System.Drawing.Size(257, 276);
            this.TFiles.TabIndex = 0;
            this.TFiles.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TFiles_NodeMouseDoubleClick);
            this.TFiles.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TFiles_AfterSelect);
            // 
            // BtAbort
            // 
            this.BtAbort.Location = new System.Drawing.Point(143, 345);
            this.BtAbort.Name = "BtAbort";
            this.BtAbort.Size = new System.Drawing.Size(125, 23);
            this.BtAbort.TabIndex = 1;
            this.BtAbort.Text = "Abbrechen";
            this.BtAbort.UseVisualStyleBackColor = true;
            this.BtAbort.Click += new System.EventHandler(this.BtAbort_Click);
            // 
            // BtOk
            // 
            this.BtOk.Location = new System.Drawing.Point(11, 345);
            this.BtOk.Name = "BtOk";
            this.BtOk.Size = new System.Drawing.Size(126, 23);
            this.BtOk.TabIndex = 1;
            this.BtOk.Text = "Öffnen";
            this.BtOk.UseVisualStyleBackColor = true;
            this.BtOk.Click += new System.EventHandler(this.BtOk_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 291);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Datum:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 304);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Welt:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 317);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Ingame-Zeit:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 330);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(125, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Gespielte Sekunden:";
            // 
            // lbDatum
            // 
            this.lbDatum.AutoSize = true;
            this.lbDatum.Location = new System.Drawing.Point(141, 291);
            this.lbDatum.Name = "lbDatum";
            this.lbDatum.Size = new System.Drawing.Size(0, 13);
            this.lbDatum.TabIndex = 3;
            // 
            // lbWelt
            // 
            this.lbWelt.AutoSize = true;
            this.lbWelt.Location = new System.Drawing.Point(141, 304);
            this.lbWelt.Name = "lbWelt";
            this.lbWelt.Size = new System.Drawing.Size(0, 13);
            this.lbWelt.TabIndex = 3;
            // 
            // lbZeit
            // 
            this.lbZeit.AutoSize = true;
            this.lbZeit.Location = new System.Drawing.Point(141, 317);
            this.lbZeit.Name = "lbZeit";
            this.lbZeit.Size = new System.Drawing.Size(0, 13);
            this.lbZeit.TabIndex = 3;
            // 
            // lbSekunden
            // 
            this.lbSekunden.AutoSize = true;
            this.lbSekunden.Location = new System.Drawing.Point(141, 330);
            this.lbSekunden.Name = "lbSekunden";
            this.lbSekunden.Size = new System.Drawing.Size(0, 13);
            this.lbSekunden.TabIndex = 3;
            // 
            // images1
            // 
            this.images1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("images1.ImageStream")));
            this.images1.TransparentColor = System.Drawing.Color.Transparent;
            this.images1.Images.SetKeyName(0, "Folder.png");
            this.images1.Images.SetKeyName(1, "File.png");
            // 
            // FSaveGames
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(281, 380);
            this.Controls.Add(this.lbSekunden);
            this.Controls.Add(this.lbZeit);
            this.Controls.Add(this.lbWelt);
            this.Controls.Add(this.lbDatum);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtOk);
            this.Controls.Add(this.BtAbort);
            this.Controls.Add(this.TFiles);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(297, 416);
            this.MinimumSize = new System.Drawing.Size(297, 416);
            this.Name = "FSaveGames";
            this.Text = "Savegames";
            this.Load += new System.EventHandler(this.FSaveGames_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView TFiles;
        private System.Windows.Forms.Button BtAbort;
        private System.Windows.Forms.Button BtOk;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbDatum;
        private System.Windows.Forms.Label lbWelt;
        private System.Windows.Forms.Label lbZeit;
        private System.Windows.Forms.Label lbSekunden;
        private System.Windows.Forms.ImageList images1;
    }
}