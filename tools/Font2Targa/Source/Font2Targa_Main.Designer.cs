namespace Font2Targa
{
    partial class Font2Targa_Main
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
            System.Windows.Forms.GroupBox groupBox2;
            System.Windows.Forms.GroupBox groupBox3;
            System.Windows.Forms.GroupBox groupBox4;
            System.Windows.Forms.GroupBox groupBox1;
            System.Windows.Forms.Panel panel1;
            System.Windows.Forms.GroupBox groupBox5;
            System.Windows.Forms.GroupBox groupBox7;
            System.Windows.Forms.GroupBox groupBox8;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Font2Targa_Main));
            this.rbt_Italic = new System.Windows.Forms.RadioButton();
            this.rbt_Bold = new System.Windows.Forms.RadioButton();
            this.rbn_Regular = new System.Windows.Forms.RadioButton();
            this.nud_Fontsize = new System.Windows.Forms.NumericUpDown();
            this.cbx_Alias = new System.Windows.Forms.ComboBox();
            this.cbx_TexWidth = new System.Windows.Forms.ComboBox();
            this.cbx_TexHeight = new System.Windows.Forms.ComboBox();
            this.nud_CharHeight = new System.Windows.Forms.NumericUpDown();
            this.nud_CharWidth = new System.Windows.Forms.NumericUpDown();
            this.btn_Textcolor = new System.Windows.Forms.Button();
            this.btn_Bgcol = new System.Windows.Forms.Button();
            this.pbx_Targa = new System.Windows.Forms.PictureBox();
            this.nud_PosX = new System.Windows.Forms.NumericUpDown();
            this.nud_PosCorrect = new System.Windows.Forms.NumericUpDown();
            this.lbx_Console = new System.Windows.Forms.ListBox();
            this.btn_LoadConfig = new System.Windows.Forms.Button();
            this.btn_SaveConfig = new System.Windows.Forms.Button();
            this.btn_Export = new System.Windows.Forms.Button();
            this.lbx_Fonts = new System.Windows.Forms.ListBox();
            this.CLD = new System.Windows.Forms.ColorDialog();
            this.SFD = new System.Windows.Forms.SaveFileDialog();
            this.rtb_Chars = new System.Windows.Forms.RichTextBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.rbn_Following = new System.Windows.Forms.RadioButton();
            this.rbt_All = new System.Windows.Forms.RadioButton();
            this.lbl_Preview = new System.Windows.Forms.TextBox();
            this.SFDCon = new System.Windows.Forms.SaveFileDialog();
            this.OFDCon = new System.Windows.Forms.OpenFileDialog();
            this.cbx_CP1251 = new System.Windows.Forms.CheckBox();
            groupBox2 = new System.Windows.Forms.GroupBox();
            groupBox3 = new System.Windows.Forms.GroupBox();
            groupBox4 = new System.Windows.Forms.GroupBox();
            groupBox1 = new System.Windows.Forms.GroupBox();
            panel1 = new System.Windows.Forms.Panel();
            groupBox5 = new System.Windows.Forms.GroupBox();
            groupBox7 = new System.Windows.Forms.GroupBox();
            groupBox8 = new System.Windows.Forms.GroupBox();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Fontsize)).BeginInit();
            groupBox3.SuspendLayout();
            groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_CharHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_CharWidth)).BeginInit();
            groupBox1.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_Targa)).BeginInit();
            groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_PosX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_PosCorrect)).BeginInit();
            groupBox7.SuspendLayout();
            groupBox8.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(this.rbt_Italic);
            groupBox2.Controls.Add(this.rbt_Bold);
            groupBox2.Controls.Add(this.rbn_Regular);
            groupBox2.Controls.Add(this.nud_Fontsize);
            groupBox2.Controls.Add(this.cbx_Alias);
            groupBox2.Location = new System.Drawing.Point(373, 299);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new System.Drawing.Size(175, 72);
            groupBox2.TabIndex = 16;
            groupBox2.TabStop = false;
            groupBox2.Text = "Schrift";
            // 
            // rbt_Italic
            // 
            this.rbt_Italic.AutoSize = true;
            this.rbt_Italic.Location = new System.Drawing.Point(101, 46);
            this.rbt_Italic.Name = "rbt_Italic";
            this.rbt_Italic.Size = new System.Drawing.Size(47, 17);
            this.rbt_Italic.TabIndex = 12;
            this.rbt_Italic.Text = "Italic";
            this.rbt_Italic.UseVisualStyleBackColor = true;
            this.rbt_Italic.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
            // 
            // rbt_Bold
            // 
            this.rbt_Bold.AutoSize = true;
            this.rbt_Bold.Location = new System.Drawing.Point(101, 31);
            this.rbt_Bold.Name = "rbt_Bold";
            this.rbt_Bold.Size = new System.Drawing.Size(46, 17);
            this.rbt_Bold.TabIndex = 11;
            this.rbt_Bold.Text = "Bold";
            this.rbt_Bold.UseVisualStyleBackColor = true;
            this.rbt_Bold.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // rbn_Regular
            // 
            this.rbn_Regular.AutoSize = true;
            this.rbn_Regular.Checked = true;
            this.rbn_Regular.Location = new System.Drawing.Point(101, 16);
            this.rbn_Regular.Name = "rbn_Regular";
            this.rbn_Regular.Size = new System.Drawing.Size(62, 17);
            this.rbn_Regular.TabIndex = 10;
            this.rbn_Regular.TabStop = true;
            this.rbn_Regular.Text = "Regular";
            this.rbn_Regular.UseVisualStyleBackColor = true;
            this.rbn_Regular.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // nud_Fontsize
            // 
            this.nud_Fontsize.Location = new System.Drawing.Point(6, 17);
            this.nud_Fontsize.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nud_Fontsize.Minimum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.nud_Fontsize.Name = "nud_Fontsize";
            this.nud_Fontsize.Size = new System.Drawing.Size(89, 20);
            this.nud_Fontsize.TabIndex = 6;
            this.nud_Fontsize.Value = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.nud_Fontsize.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // cbx_Alias
            // 
            this.cbx_Alias.FormattingEnabled = true;
            this.cbx_Alias.Items.AddRange(new object[] {
            "AntiAlias",
            "Default"});
            this.cbx_Alias.Location = new System.Drawing.Point(6, 43);
            this.cbx_Alias.Name = "cbx_Alias";
            this.cbx_Alias.Size = new System.Drawing.Size(89, 21);
            this.cbx_Alias.TabIndex = 9;
            this.cbx_Alias.Text = "AntiAlias";
            this.cbx_Alias.SelectedIndexChanged += new System.EventHandler(this.cbx_Alias_SelectedIndexChanged);
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(this.cbx_TexWidth);
            groupBox3.Controls.Add(this.cbx_TexHeight);
            groupBox3.Location = new System.Drawing.Point(554, 299);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new System.Drawing.Size(74, 72);
            groupBox3.TabIndex = 17;
            groupBox3.TabStop = false;
            groupBox3.Text = "Textur";
            // 
            // cbx_TexWidth
            // 
            this.cbx_TexWidth.FormattingEnabled = true;
            this.cbx_TexWidth.Items.AddRange(new object[] {
            "64",
            "128",
            "256",
            "512",
            "1024",
            "2048",
            "4096"});
            this.cbx_TexWidth.Location = new System.Drawing.Point(6, 16);
            this.cbx_TexWidth.Name = "cbx_TexWidth";
            this.cbx_TexWidth.Size = new System.Drawing.Size(62, 21);
            this.cbx_TexWidth.TabIndex = 11;
            this.cbx_TexWidth.Text = "512";
            this.cbx_TexWidth.SelectedIndexChanged += new System.EventHandler(this.cbx_TexWidth_SelectedIndexChanged);
            // 
            // cbx_TexHeight
            // 
            this.cbx_TexHeight.FormattingEnabled = true;
            this.cbx_TexHeight.Items.AddRange(new object[] {
            "64",
            "128",
            "256",
            "512",
            "1024",
            "2048",
            "4096"});
            this.cbx_TexHeight.Location = new System.Drawing.Point(6, 43);
            this.cbx_TexHeight.Name = "cbx_TexHeight";
            this.cbx_TexHeight.Size = new System.Drawing.Size(62, 21);
            this.cbx_TexHeight.TabIndex = 12;
            this.cbx_TexHeight.Text = "256";
            this.cbx_TexHeight.SelectedIndexChanged += new System.EventHandler(this.cbx_TexHeight_SelectedIndexChanged);
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(this.nud_CharHeight);
            groupBox4.Controls.Add(this.nud_CharWidth);
            groupBox4.Location = new System.Drawing.Point(634, 299);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new System.Drawing.Size(70, 72);
            groupBox4.TabIndex = 18;
            groupBox4.TabStop = false;
            groupBox4.Text = "Zeichen";
            // 
            // nud_CharHeight
            // 
            this.nud_CharHeight.Location = new System.Drawing.Point(6, 43);
            this.nud_CharHeight.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nud_CharHeight.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.nud_CharHeight.Name = "nud_CharHeight";
            this.nud_CharHeight.Size = new System.Drawing.Size(58, 20);
            this.nud_CharHeight.TabIndex = 11;
            this.nud_CharHeight.Value = new decimal(new int[] {
            21,
            0,
            0,
            0});
            this.nud_CharHeight.ValueChanged += new System.EventHandler(this.nud_CharHeight_ValueChanged);
            // 
            // nud_CharWidth
            // 
            this.nud_CharWidth.Location = new System.Drawing.Point(6, 17);
            this.nud_CharWidth.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nud_CharWidth.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.nud_CharWidth.Name = "nud_CharWidth";
            this.nud_CharWidth.Size = new System.Drawing.Size(58, 20);
            this.nud_CharWidth.TabIndex = 10;
            this.nud_CharWidth.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nud_CharWidth.ValueChanged += new System.EventHandler(this.nud_CharWidth_ValueChanged);
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(this.btn_Textcolor);
            groupBox1.Controls.Add(this.btn_Bgcol);
            groupBox1.Location = new System.Drawing.Point(247, 299);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new System.Drawing.Size(120, 72);
            groupBox1.TabIndex = 15;
            groupBox1.TabStop = false;
            groupBox1.Text = "Farben";
            // 
            // btn_Textcolor
            // 
            this.btn_Textcolor.BackColor = System.Drawing.Color.Black;
            this.btn_Textcolor.ForeColor = System.Drawing.Color.White;
            this.btn_Textcolor.Location = new System.Drawing.Point(7, 16);
            this.btn_Textcolor.Name = "btn_Textcolor";
            this.btn_Textcolor.Size = new System.Drawing.Size(107, 24);
            this.btn_Textcolor.TabIndex = 4;
            this.btn_Textcolor.Text = "Text";
            this.btn_Textcolor.UseVisualStyleBackColor = false;
            this.btn_Textcolor.Click += new System.EventHandler(this.btn_Textcolor_Click);
            // 
            // btn_Bgcol
            // 
            this.btn_Bgcol.BackColor = System.Drawing.Color.Transparent;
            this.btn_Bgcol.ForeColor = System.Drawing.Color.Black;
            this.btn_Bgcol.Location = new System.Drawing.Point(7, 42);
            this.btn_Bgcol.Name = "btn_Bgcol";
            this.btn_Bgcol.Size = new System.Drawing.Size(107, 24);
            this.btn_Bgcol.TabIndex = 5;
            this.btn_Bgcol.Text = "Hintergrund";
            this.btn_Bgcol.UseVisualStyleBackColor = false;
            this.btn_Bgcol.Click += new System.EventHandler(this.btn_Bgcol_Click);
            // 
            // panel1
            // 
            panel1.AutoScroll = true;
            panel1.Controls.Add(this.pbx_Targa);
            panel1.Location = new System.Drawing.Point(247, 12);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(531, 281);
            panel1.TabIndex = 20;
            // 
            // pbx_Targa
            // 
            this.pbx_Targa.BackColor = System.Drawing.Color.Transparent;
            this.pbx_Targa.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pbx_Targa.Location = new System.Drawing.Point(0, 0);
            this.pbx_Targa.Name = "pbx_Targa";
            this.pbx_Targa.Size = new System.Drawing.Size(512, 256);
            this.pbx_Targa.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbx_Targa.TabIndex = 1;
            this.pbx_Targa.TabStop = false;
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(this.nud_PosX);
            groupBox5.Controls.Add(this.nud_PosCorrect);
            groupBox5.Location = new System.Drawing.Point(713, 299);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new System.Drawing.Size(65, 72);
            groupBox5.TabIndex = 22;
            groupBox5.TabStop = false;
            groupBox5.Text = "Position";
            // 
            // nud_PosX
            // 
            this.nud_PosX.Location = new System.Drawing.Point(6, 43);
            this.nud_PosX.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.nud_PosX.Name = "nud_PosX";
            this.nud_PosX.Size = new System.Drawing.Size(53, 20);
            this.nud_PosX.TabIndex = 13;
            this.nud_PosX.ValueChanged += new System.EventHandler(this.nud_PosX_ValueChanged);
            // 
            // nud_PosCorrect
            // 
            this.nud_PosCorrect.Location = new System.Drawing.Point(6, 17);
            this.nud_PosCorrect.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.nud_PosCorrect.Name = "nud_PosCorrect";
            this.nud_PosCorrect.Size = new System.Drawing.Size(53, 20);
            this.nud_PosCorrect.TabIndex = 12;
            this.nud_PosCorrect.ValueChanged += new System.EventHandler(this.nud_PosCorrect_ValueChanged);
            // 
            // groupBox7
            // 
            groupBox7.Controls.Add(this.lbx_Console);
            groupBox7.Location = new System.Drawing.Point(254, 377);
            groupBox7.Name = "groupBox7";
            groupBox7.Size = new System.Drawing.Size(294, 83);
            groupBox7.TabIndex = 26;
            groupBox7.TabStop = false;
            groupBox7.Text = "Benachrichtigungen";
            // 
            // lbx_Console
            // 
            this.lbx_Console.FormattingEnabled = true;
            this.lbx_Console.Location = new System.Drawing.Point(6, 19);
            this.lbx_Console.Name = "lbx_Console";
            this.lbx_Console.Size = new System.Drawing.Size(282, 56);
            this.lbx_Console.TabIndex = 0;
            // 
            // groupBox8
            // 
            groupBox8.Controls.Add(this.btn_LoadConfig);
            groupBox8.Controls.Add(this.btn_SaveConfig);
            groupBox8.Controls.Add(this.btn_Export);
            groupBox8.Location = new System.Drawing.Point(554, 377);
            groupBox8.Name = "groupBox8";
            groupBox8.Size = new System.Drawing.Size(224, 83);
            groupBox8.TabIndex = 27;
            groupBox8.TabStop = false;
            groupBox8.Text = "Sonstiges";
            // 
            // btn_LoadConfig
            // 
            this.btn_LoadConfig.Location = new System.Drawing.Point(6, 19);
            this.btn_LoadConfig.Name = "btn_LoadConfig";
            this.btn_LoadConfig.Size = new System.Drawing.Size(103, 21);
            this.btn_LoadConfig.TabIndex = 12;
            this.btn_LoadConfig.Text = "Laden";
            this.btn_LoadConfig.UseVisualStyleBackColor = true;
            this.btn_LoadConfig.Click += new System.EventHandler(this.btn_LoadConfig_Click);
            // 
            // btn_SaveConfig
            // 
            this.btn_SaveConfig.Location = new System.Drawing.Point(115, 19);
            this.btn_SaveConfig.Name = "btn_SaveConfig";
            this.btn_SaveConfig.Size = new System.Drawing.Size(103, 21);
            this.btn_SaveConfig.TabIndex = 11;
            this.btn_SaveConfig.Text = "Speichern";
            this.btn_SaveConfig.UseVisualStyleBackColor = true;
            this.btn_SaveConfig.Click += new System.EventHandler(this.btn_SaveConfig_Click);
            // 
            // btn_Export
            // 
            this.btn_Export.Location = new System.Drawing.Point(6, 46);
            this.btn_Export.Name = "btn_Export";
            this.btn_Export.Size = new System.Drawing.Size(212, 31);
            this.btn_Export.TabIndex = 10;
            this.btn_Export.Text = "Exportieren";
            this.btn_Export.UseVisualStyleBackColor = true;
            this.btn_Export.Click += new System.EventHandler(this.btn_Export_Click);
            // 
            // lbx_Fonts
            // 
            this.lbx_Fonts.FormattingEnabled = true;
            this.lbx_Fonts.IntegralHeight = false;
            this.lbx_Fonts.Location = new System.Drawing.Point(12, 12);
            this.lbx_Fonts.Name = "lbx_Fonts";
            this.lbx_Fonts.Size = new System.Drawing.Size(229, 256);
            this.lbx_Fonts.TabIndex = 0;
            this.lbx_Fonts.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // CLD
            // 
            this.CLD.AnyColor = true;
            this.CLD.FullOpen = true;
            // 
            // SFD
            // 
            this.SFD.DefaultExt = "*.tga";
            this.SFD.Filter = "Targa (*.tga)|*.tga";
            this.SFD.SupportMultiDottedExtensions = true;
            this.SFD.Title = "Targa exportieren";
            // 
            // rtb_Chars
            // 
            this.rtb_Chars.Enabled = false;
            this.rtb_Chars.Location = new System.Drawing.Point(6, 41);
            this.rtb_Chars.MaxLength = 255;
            this.rtb_Chars.Name = "rtb_Chars";
            this.rtb_Chars.Size = new System.Drawing.Size(217, 89);
            this.rtb_Chars.TabIndex = 23;
            this.rtb_Chars.Text = "";
            this.rtb_Chars.TextChanged += new System.EventHandler(this.rtb_Chars_TextChanged);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.cbx_CP1251);
            this.groupBox6.Controls.Add(this.rbn_Following);
            this.groupBox6.Controls.Add(this.rbt_All);
            this.groupBox6.Controls.Add(this.rtb_Chars);
            this.groupBox6.Location = new System.Drawing.Point(12, 299);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(229, 161);
            this.groupBox6.TabIndex = 24;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Exportliste";
            // 
            // rbn_Following
            // 
            this.rbn_Following.AutoSize = true;
            this.rbn_Following.Location = new System.Drawing.Point(54, 19);
            this.rbn_Following.Name = "rbn_Following";
            this.rbn_Following.Size = new System.Drawing.Size(115, 17);
            this.rbn_Following.TabIndex = 25;
            this.rbn_Following.Text = "Nur die Folgenden:";
            this.rbn_Following.UseVisualStyleBackColor = true;
            this.rbn_Following.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged_1);
            // 
            // rbt_All
            // 
            this.rbt_All.AutoSize = true;
            this.rbt_All.Checked = true;
            this.rbt_All.Location = new System.Drawing.Point(6, 19);
            this.rbt_All.Name = "rbt_All";
            this.rbt_All.Size = new System.Drawing.Size(42, 17);
            this.rbt_All.TabIndex = 24;
            this.rbt_All.TabStop = true;
            this.rbt_All.Text = "Alle";
            this.rbt_All.UseVisualStyleBackColor = true;
            // 
            // lbl_Preview
            // 
            this.lbl_Preview.Location = new System.Drawing.Point(12, 273);
            this.lbl_Preview.Name = "lbl_Preview";
            this.lbl_Preview.Size = new System.Drawing.Size(229, 20);
            this.lbl_Preview.TabIndex = 25;
            this.lbl_Preview.Text = "Sample";
            // 
            // SFDCon
            // 
            this.SFDCon.DefaultExt = "*.f2t.zip";
            this.SFDCon.Filter = "F2TConfig (*.f2t)|*.f2t";
            this.SFDCon.Title = "F2TConfig speichern";
            // 
            // OFDCon
            // 
            this.OFDCon.DefaultExt = "*.f2t";
            this.OFDCon.Filter = "F2TConfig (*.f2t)|*.f2t";
            this.OFDCon.Title = "F2TConfig laden";
            // 
            // cbx_CP1251
            // 
            this.cbx_CP1251.AutoSize = true;
            this.cbx_CP1251.Location = new System.Drawing.Point(6, 136);
            this.cbx_CP1251.Name = "cbx_CP1251";
            this.cbx_CP1251.Size = new System.Drawing.Size(67, 17);
            this.cbx_CP1251.TabIndex = 26;
            this.cbx_CP1251.Text = "CP-1251";
            this.cbx_CP1251.UseVisualStyleBackColor = true;
            this.cbx_CP1251.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // Font2Targa_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(790, 468);
            this.Controls.Add(groupBox8);
            this.Controls.Add(groupBox7);
            this.Controls.Add(this.lbl_Preview);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(groupBox5);
            this.Controls.Add(panel1);
            this.Controls.Add(groupBox4);
            this.Controls.Add(groupBox3);
            this.Controls.Add(groupBox2);
            this.Controls.Add(groupBox1);
            this.Controls.Add(this.lbx_Fonts);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Font2Targa_Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Font2Targa";
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Fontsize)).EndInit();
            groupBox3.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nud_CharHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_CharWidth)).EndInit();
            groupBox1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbx_Targa)).EndInit();
            groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nud_PosX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_PosCorrect)).EndInit();
            groupBox7.ResumeLayout(false);
            groupBox8.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbx_Fonts;
        private System.Windows.Forms.PictureBox pbx_Targa;
        private System.Windows.Forms.ColorDialog CLD;
        private System.Windows.Forms.Button btn_Textcolor;
        private System.Windows.Forms.Button btn_Bgcol;
        private System.Windows.Forms.NumericUpDown nud_Fontsize;
        private System.Windows.Forms.ComboBox cbx_Alias;
        private System.Windows.Forms.Button btn_Export;
        private System.Windows.Forms.SaveFileDialog SFD;
        private System.Windows.Forms.ComboBox cbx_TexWidth;
        private System.Windows.Forms.ComboBox cbx_TexHeight;
        private System.Windows.Forms.NumericUpDown nud_CharHeight;
        private System.Windows.Forms.NumericUpDown nud_CharWidth;
        private System.Windows.Forms.RadioButton rbt_Italic;
        private System.Windows.Forms.RadioButton rbt_Bold;
        private System.Windows.Forms.RadioButton rbn_Regular;
        private System.Windows.Forms.NumericUpDown nud_PosX;
        private System.Windows.Forms.NumericUpDown nud_PosCorrect;
        private System.Windows.Forms.RichTextBox rtb_Chars;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.RadioButton rbn_Following;
        private System.Windows.Forms.RadioButton rbt_All;
        private System.Windows.Forms.TextBox lbl_Preview;
        private System.Windows.Forms.ListBox lbx_Console;
        private System.Windows.Forms.Button btn_LoadConfig;
        private System.Windows.Forms.Button btn_SaveConfig;
        private System.Windows.Forms.SaveFileDialog SFDCon;
        private System.Windows.Forms.OpenFileDialog OFDCon;
        private System.Windows.Forms.CheckBox cbx_CP1251;
    }
}

