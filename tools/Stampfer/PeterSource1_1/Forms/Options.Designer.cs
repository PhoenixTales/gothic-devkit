/**************************************************************************************
    Stampfer - Gothic Script Editor
    Copyright (C) 2008, 2009 Jpmon1, Alexander "Sumpfkrautjunkie" Ruppert

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
namespace Peter
{
    partial class Options
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Options));
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("Allgemein", 0);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("Editor", 2);
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem("Gothic");
            this.imgMain = new System.Windows.Forms.ImageList(this.components);
            this.Editor = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.textEditorControl1 = new ICSharpCode.TextEditor.TextEditorControl();
            this.btnFont = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbBracketStyle = new System.Windows.Forms.ComboBox();
            this.cmbIndentStyle = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.nudVRuler = new System.Windows.Forms.NumericUpDown();
            this.nudTabIndent = new System.Windows.Forms.NumericUpDown();
            this.ckbAutoInsertBracket = new System.Windows.Forms.CheckBox();
            this.ckbHighlightCurrentLine = new System.Windows.Forms.CheckBox();
            this.ckbAllowCaretBeyondEol = new System.Windows.Forms.CheckBox();
            this.ckbUseAntiAlias = new System.Windows.Forms.CheckBox();
            this.ckbConvertTabs = new System.Windows.Forms.CheckBox();
            this.ckbEnableCodeFolding = new System.Windows.Forms.CheckBox();
            this.ckbShowVRuler = new System.Windows.Forms.CheckBox();
            this.ckbShowHRuler = new System.Windows.Forms.CheckBox();
            this.ckbShowLineNumbes = new System.Windows.Forms.CheckBox();
            this.ckbShowMatchingBracket = new System.Windows.Forms.CheckBox();
            this.ckbShowTabs = new System.Windows.Forms.CheckBox();
            this.ckbShowSpaces = new System.Windows.Forms.CheckBox();
            this.ckbShowInvalidLines = new System.Windows.Forms.CheckBox();
            this.ckbShowEol = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Allgemein = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ckbSaveOnExt = new System.Windows.Forms.CheckBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.ckbBackup = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.nudRecentFile = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.nudRecentProject = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.FBD = new System.Windows.Forms.FolderBrowserDialog();
            this.Gothic = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.TScriptsPatch = new System.Windows.Forms.TextBox();
            this.BtBrowseScripts = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.TBilderPatch = new System.Windows.Forms.TextBox();
            this.BtBrowseBilder = new System.Windows.Forms.Button();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.TBakPatch = new System.Windows.Forms.TextBox();
            this.BtBrowseBak = new System.Windows.Forms.Button();
            this.ckbInFolderOnly = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.nBakMin = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.ckbMessageBox = new System.Windows.Forms.CheckBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.ckbAutoCompleteAuto = new System.Windows.Forms.CheckBox();
            this.ckbAutoBrackets = new System.Windows.Forms.CheckBox();
            this.lstMain = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.Editor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudVRuler)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTabIndent)).BeginInit();
            this.Allgemein.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRecentFile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRecentProject)).BeginInit();
            this.Gothic.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nBakMin)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // imgMain
            // 
            this.imgMain.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgMain.ImageStream")));
            this.imgMain.TransparentColor = System.Drawing.Color.Transparent;
            this.imgMain.Images.SetKeyName(0, "OptionsHS.png");
            this.imgMain.Images.SetKeyName(1, "DisplayInColorHS.png");
            this.imgMain.Images.SetKeyName(2, "ShowRulelinesHS.png");
            // 
            // Editor
            // 
            this.Editor.Controls.Add(this.label9);
            this.Editor.Controls.Add(this.textEditorControl1);
            this.Editor.Controls.Add(this.btnFont);
            this.Editor.Controls.Add(this.label8);
            this.Editor.Controls.Add(this.label7);
            this.Editor.Controls.Add(this.cmbBracketStyle);
            this.Editor.Controls.Add(this.cmbIndentStyle);
            this.Editor.Controls.Add(this.label6);
            this.Editor.Controls.Add(this.label5);
            this.Editor.Controls.Add(this.nudVRuler);
            this.Editor.Controls.Add(this.nudTabIndent);
            this.Editor.Controls.Add(this.ckbAutoInsertBracket);
            this.Editor.Controls.Add(this.ckbHighlightCurrentLine);
            this.Editor.Controls.Add(this.ckbAllowCaretBeyondEol);
            this.Editor.Controls.Add(this.ckbUseAntiAlias);
            this.Editor.Controls.Add(this.ckbConvertTabs);
            this.Editor.Controls.Add(this.ckbEnableCodeFolding);
            this.Editor.Controls.Add(this.ckbShowVRuler);
            this.Editor.Controls.Add(this.ckbShowHRuler);
            this.Editor.Controls.Add(this.ckbShowLineNumbes);
            this.Editor.Controls.Add(this.ckbShowMatchingBracket);
            this.Editor.Controls.Add(this.ckbShowTabs);
            this.Editor.Controls.Add(this.ckbShowSpaces);
            this.Editor.Controls.Add(this.ckbShowInvalidLines);
            this.Editor.Controls.Add(this.ckbShowEol);
            this.Editor.Controls.Add(this.label4);
            this.Editor.Location = new System.Drawing.Point(2, 0);
            this.Editor.Name = "Editor";
            this.Editor.Size = new System.Drawing.Size(436, 312);
            this.Editor.TabIndex = 0;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(187, 172);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(55, 13);
            this.label9.TabIndex = 25;
            this.label9.Text = "Vorschau:";
            // 
            // textEditorControl1
            // 
            this.textEditorControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textEditorControl1.Location = new System.Drawing.Point(190, 188);
            this.textEditorControl1.Name = "textEditorControl1";
            this.textEditorControl1.ShowEOLMarkers = true;
            this.textEditorControl1.ShowSpaces = true;
            this.textEditorControl1.ShowTabs = true;
            this.textEditorControl1.ShowVRuler = true;
            this.textEditorControl1.Size = new System.Drawing.Size(232, 115);
            this.textEditorControl1.TabIndex = 24;
            // 
            // btnFont
            // 
            this.btnFont.Location = new System.Drawing.Point(347, 29);
            this.btnFont.Name = "btnFont";
            this.btnFont.Size = new System.Drawing.Size(75, 23);
            this.btnFont.TabIndex = 23;
            this.btnFont.Text = "Schriftart...";
            this.btnFont.UseVisualStyleBackColor = true;
            this.btnFont.Click += new System.EventHandler(this.btnFont_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(270, 126);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(78, 13);
            this.label8.TabIndex = 22;
            this.label8.Text = "Einrückungsstil";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(270, 147);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(87, 13);
            this.label7.TabIndex = 21;
            this.label7.Text = "Klammerkontrolle";
            // 
            // cmbBracketStyle
            // 
            this.cmbBracketStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBracketStyle.FormattingEnabled = true;
            this.cmbBracketStyle.Items.AddRange(new object[] {
            "Vorher",
            "Nachher"});
            this.cmbBracketStyle.Location = new System.Drawing.Point(190, 144);
            this.cmbBracketStyle.Name = "cmbBracketStyle";
            this.cmbBracketStyle.Size = new System.Drawing.Size(74, 21);
            this.cmbBracketStyle.TabIndex = 20;
            this.cmbBracketStyle.SelectedIndexChanged += new System.EventHandler(this.cmbBracketStyle_SelectedIndexChanged);
            // 
            // cmbIndentStyle
            // 
            this.cmbIndentStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbIndentStyle.FormattingEnabled = true;
            this.cmbIndentStyle.Items.AddRange(new object[] {
            "auto",
            "none",
            "smart"});
            this.cmbIndentStyle.Location = new System.Drawing.Point(190, 121);
            this.cmbIndentStyle.Name = "cmbIndentStyle";
            this.cmbIndentStyle.Size = new System.Drawing.Size(74, 21);
            this.cmbIndentStyle.TabIndex = 19;
            this.cmbIndentStyle.SelectedIndexChanged += new System.EventHandler(this.cmbIndentStyle_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(270, 103);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(84, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Vertikales Lineal";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(270, 80);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Tab Indent";
            // 
            // nudVRuler
            // 
            this.nudVRuler.Location = new System.Drawing.Point(190, 99);
            this.nudVRuler.Name = "nudVRuler";
            this.nudVRuler.Size = new System.Drawing.Size(74, 20);
            this.nudVRuler.TabIndex = 16;
            this.nudVRuler.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudVRuler.ValueChanged += new System.EventHandler(this.nudVRuler_ValueChanged);
            // 
            // nudTabIndent
            // 
            this.nudTabIndent.Location = new System.Drawing.Point(190, 76);
            this.nudTabIndent.Name = "nudTabIndent";
            this.nudTabIndent.Size = new System.Drawing.Size(74, 20);
            this.nudTabIndent.TabIndex = 15;
            this.nudTabIndent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudTabIndent.ValueChanged += new System.EventHandler(this.nudTabIndent_ValueChanged);
            // 
            // ckbAutoInsertBracket
            // 
            this.ckbAutoInsertBracket.AutoSize = true;
            this.ckbAutoInsertBracket.Location = new System.Drawing.Point(190, 55);
            this.ckbAutoInsertBracket.Name = "ckbAutoInsertBracket";
            this.ckbAutoInsertBracket.Size = new System.Drawing.Size(142, 17);
            this.ckbAutoInsertBracket.TabIndex = 14;
            this.ckbAutoInsertBracket.Text = "Auto Klammerplatzierung";
            this.ckbAutoInsertBracket.UseVisualStyleBackColor = true;
            this.ckbAutoInsertBracket.CheckedChanged += new System.EventHandler(this.ckbAutoInsertBracket_CheckedChanged);
            // 
            // ckbHighlightCurrentLine
            // 
            this.ckbHighlightCurrentLine.AutoSize = true;
            this.ckbHighlightCurrentLine.Location = new System.Drawing.Point(190, 33);
            this.ckbHighlightCurrentLine.Name = "ckbHighlightCurrentLine";
            this.ckbHighlightCurrentLine.Size = new System.Drawing.Size(153, 17);
            this.ckbHighlightCurrentLine.TabIndex = 13;
            this.ckbHighlightCurrentLine.Text = "Aktuelle Zeile hervorheben";
            this.ckbHighlightCurrentLine.UseVisualStyleBackColor = true;
            this.ckbHighlightCurrentLine.CheckedChanged += new System.EventHandler(this.ckbHighlightCurrentLine_CheckedChanged);
            // 
            // ckbAllowCaretBeyondEol
            // 
            this.ckbAllowCaretBeyondEol.AutoSize = true;
            this.ckbAllowCaretBeyondEol.Location = new System.Drawing.Point(7, 286);
            this.ckbAllowCaretBeyondEol.Name = "ckbAllowCaretBeyondEol";
            this.ckbAllowCaretBeyondEol.Size = new System.Drawing.Size(167, 17);
            this.ckbAllowCaretBeyondEol.TabIndex = 12;
            this.ckbAllowCaretBeyondEol.Text = "Marker hinter dem Zeilenende";
            this.ckbAllowCaretBeyondEol.UseVisualStyleBackColor = true;
            this.ckbAllowCaretBeyondEol.CheckedChanged += new System.EventHandler(this.ckbAllowCaretBeyondEol_CheckedChanged);
            // 
            // ckbUseAntiAlias
            // 
            this.ckbUseAntiAlias.AutoSize = true;
            this.ckbUseAntiAlias.Location = new System.Drawing.Point(7, 263);
            this.ckbUseAntiAlias.Name = "ckbUseAntiAlias";
            this.ckbUseAntiAlias.Size = new System.Drawing.Size(91, 17);
            this.ckbUseAntiAlias.TabIndex = 11;
            this.ckbUseAntiAlias.Text = "Schrift glätten";
            this.ckbUseAntiAlias.UseVisualStyleBackColor = true;
            this.ckbUseAntiAlias.CheckedChanged += new System.EventHandler(this.ckbUseAntiAlias_CheckedChanged);
            // 
            // ckbConvertTabs
            // 
            this.ckbConvertTabs.AutoSize = true;
            this.ckbConvertTabs.Location = new System.Drawing.Point(7, 240);
            this.ckbConvertTabs.Name = "ckbConvertTabs";
            this.ckbConvertTabs.Size = new System.Drawing.Size(157, 17);
            this.ckbConvertTabs.TabIndex = 10;
            this.ckbConvertTabs.Text = "Tabulatoren in Leerzeichen";
            this.ckbConvertTabs.UseVisualStyleBackColor = true;
            this.ckbConvertTabs.CheckedChanged += new System.EventHandler(this.ckbConvertTabs_CheckedChanged);
            // 
            // ckbEnableCodeFolding
            // 
            this.ckbEnableCodeFolding.AutoSize = true;
            this.ckbEnableCodeFolding.Location = new System.Drawing.Point(7, 217);
            this.ckbEnableCodeFolding.Name = "ckbEnableCodeFolding";
            this.ckbEnableCodeFolding.Size = new System.Drawing.Size(144, 17);
            this.ckbEnableCodeFolding.TabIndex = 9;
            this.ckbEnableCodeFolding.Text = "Code Folding verwenden";
            this.ckbEnableCodeFolding.UseVisualStyleBackColor = true;
            this.ckbEnableCodeFolding.CheckedChanged += new System.EventHandler(this.ckbEnableCodeFolding_CheckedChanged);
            // 
            // ckbShowVRuler
            // 
            this.ckbShowVRuler.AutoSize = true;
            this.ckbShowVRuler.Location = new System.Drawing.Point(7, 194);
            this.ckbShowVRuler.Name = "ckbShowVRuler";
            this.ckbShowVRuler.Size = new System.Drawing.Size(103, 17);
            this.ckbShowVRuler.TabIndex = 8;
            this.ckbShowVRuler.Text = "Vertikales Lineal";
            this.ckbShowVRuler.UseVisualStyleBackColor = true;
            this.ckbShowVRuler.CheckedChanged += new System.EventHandler(this.ckbShowVRuler_CheckedChanged);
            // 
            // ckbShowHRuler
            // 
            this.ckbShowHRuler.AutoSize = true;
            this.ckbShowHRuler.Location = new System.Drawing.Point(7, 171);
            this.ckbShowHRuler.Name = "ckbShowHRuler";
            this.ckbShowHRuler.Size = new System.Drawing.Size(115, 17);
            this.ckbShowHRuler.TabIndex = 7;
            this.ckbShowHRuler.Text = "Horizontales Lineal";
            this.ckbShowHRuler.UseVisualStyleBackColor = true;
            this.ckbShowHRuler.CheckedChanged += new System.EventHandler(this.ckbShowHRuler_CheckedChanged);
            // 
            // ckbShowLineNumbes
            // 
            this.ckbShowLineNumbes.AutoSize = true;
            this.ckbShowLineNumbes.Location = new System.Drawing.Point(7, 148);
            this.ckbShowLineNumbes.Name = "ckbShowLineNumbes";
            this.ckbShowLineNumbes.Size = new System.Drawing.Size(144, 17);
            this.ckbShowLineNumbes.TabIndex = 6;
            this.ckbShowLineNumbes.Text = "Zeilennummern anzeigen";
            this.ckbShowLineNumbes.UseVisualStyleBackColor = true;
            this.ckbShowLineNumbes.CheckedChanged += new System.EventHandler(this.ckbShowLineNumbes_CheckedChanged);
            // 
            // ckbShowMatchingBracket
            // 
            this.ckbShowMatchingBracket.AutoSize = true;
            this.ckbShowMatchingBracket.Location = new System.Drawing.Point(7, 125);
            this.ckbShowMatchingBracket.Name = "ckbShowMatchingBracket";
            this.ckbShowMatchingBracket.Size = new System.Drawing.Size(178, 17);
            this.ckbShowMatchingBracket.TabIndex = 5;
            this.ckbShowMatchingBracket.Text = "Zusammengehörende Klammern";
            this.ckbShowMatchingBracket.UseVisualStyleBackColor = true;
            this.ckbShowMatchingBracket.CheckedChanged += new System.EventHandler(this.ckbShowMatchingBracket_CheckedChanged);
            // 
            // ckbShowTabs
            // 
            this.ckbShowTabs.AutoSize = true;
            this.ckbShowTabs.Location = new System.Drawing.Point(7, 102);
            this.ckbShowTabs.Name = "ckbShowTabs";
            this.ckbShowTabs.Size = new System.Drawing.Size(127, 17);
            this.ckbShowTabs.TabIndex = 4;
            this.ckbShowTabs.Text = "Tab Marker anzeigen";
            this.ckbShowTabs.UseVisualStyleBackColor = true;
            this.ckbShowTabs.CheckedChanged += new System.EventHandler(this.ckbShowTabs_CheckedChanged);
            // 
            // ckbShowSpaces
            // 
            this.ckbShowSpaces.AutoSize = true;
            this.ckbShowSpaces.Location = new System.Drawing.Point(7, 79);
            this.ckbShowSpaces.Name = "ckbShowSpaces";
            this.ckbShowSpaces.Size = new System.Drawing.Size(139, 17);
            this.ckbShowSpaces.TabIndex = 3;
            this.ckbShowSpaces.Text = "Space Marker anzeigen";
            this.ckbShowSpaces.UseVisualStyleBackColor = true;
            this.ckbShowSpaces.CheckedChanged += new System.EventHandler(this.ckbShowSpaces_CheckedChanged);
            // 
            // ckbShowInvalidLines
            // 
            this.ckbShowInvalidLines.AutoSize = true;
            this.ckbShowInvalidLines.Location = new System.Drawing.Point(7, 56);
            this.ckbShowInvalidLines.Name = "ckbShowInvalidLines";
            this.ckbShowInvalidLines.Size = new System.Drawing.Size(160, 17);
            this.ckbShowInvalidLines.TabIndex = 2;
            this.ckbShowInvalidLines.Text = "Empty Line Marker anzeigen";
            this.ckbShowInvalidLines.UseVisualStyleBackColor = true;
            this.ckbShowInvalidLines.CheckedChanged += new System.EventHandler(this.ckbShowInvalidLines_CheckedChanged);
            // 
            // ckbShowEol
            // 
            this.ckbShowEol.AutoSize = true;
            this.ckbShowEol.Location = new System.Drawing.Point(7, 33);
            this.ckbShowEol.Name = "ckbShowEol";
            this.ckbShowEol.Size = new System.Drawing.Size(162, 17);
            this.ckbShowEol.TabIndex = 1;
            this.ckbShowEol.Text = "End of Line Marker anzeigen";
            this.ckbShowEol.UseVisualStyleBackColor = true;
            this.ckbShowEol.CheckedChanged += new System.EventHandler(this.ckbShowEol_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(3, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(157, 24);
            this.label4.TabIndex = 0;
            this.label4.Text = "Editor Optionen";
            // 
            // Allgemein
            // 
            this.Allgemein.Controls.Add(this.groupBox2);
            this.Allgemein.Controls.Add(this.groupBox6);
            this.Allgemein.Controls.Add(this.groupBox1);
            this.Allgemein.Controls.Add(this.label1);
            this.Allgemein.Location = new System.Drawing.Point(18, 12);
            this.Allgemein.Name = "Allgemein";
            this.Allgemein.Size = new System.Drawing.Size(378, 261);
            this.Allgemein.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.ckbSaveOnExt);
            this.groupBox2.Location = new System.Drawing.Point(7, 30);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(368, 87);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Layout";
            // 
            // ckbSaveOnExt
            // 
            this.ckbSaveOnExt.AutoSize = true;
            this.ckbSaveOnExt.Location = new System.Drawing.Point(6, 28);
            this.ckbSaveOnExt.Name = "ckbSaveOnExt";
            this.ckbSaveOnExt.Size = new System.Drawing.Size(178, 17);
            this.ckbSaveOnExt.TabIndex = 0;
            this.ckbSaveOnExt.Text = "Layout beim Beenden speichern";
            this.ckbSaveOnExt.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.ckbBackup);
            this.groupBox6.Location = new System.Drawing.Point(7, 230);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(422, 39);
            this.groupBox6.TabIndex = 4;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Speicheroptionen";
            // 
            // ckbBackup
            // 
            this.ckbBackup.AutoSize = true;
            this.ckbBackup.Location = new System.Drawing.Point(6, 16);
            this.ckbBackup.Name = "ckbBackup";
            this.ckbBackup.Size = new System.Drawing.Size(263, 17);
            this.ckbBackup.TabIndex = 0;
            this.ckbBackup.Text = "Backup der Datei vor dem Überschreiben anlegen";
            this.ckbBackup.UseVisualStyleBackColor = true;
            this.ckbBackup.CheckedChanged += new System.EventHandler(this.ckbBackup_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.nudRecentFile);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.nudRecentProject);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(7, 123);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(368, 100);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Zuletzt geöffnet";
            // 
            // nudRecentFile
            // 
            this.nudRecentFile.Location = new System.Drawing.Point(6, 30);
            this.nudRecentFile.Name = "nudRecentFile";
            this.nudRecentFile.Size = new System.Drawing.Size(72, 20);
            this.nudRecentFile.TabIndex = 3;
            this.nudRecentFile.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(84, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Letzte Projekte";
            // 
            // nudRecentProject
            // 
            this.nudRecentProject.Location = new System.Drawing.Point(6, 56);
            this.nudRecentProject.Name = "nudRecentProject";
            this.nudRecentProject.Size = new System.Drawing.Size(72, 20);
            this.nudRecentProject.TabIndex = 4;
            this.nudRecentProject.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(84, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Letzte Dateien";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(208, 24);
            this.label1.TabIndex = 1;
            this.label1.Text = "Allgemeine Optionen";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(386, 321);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "Ok";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(467, 321);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(80, 23);
            this.btnApply.TabIndex = 2;
            this.btnApply.Text = "Übernehmen";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(553, 321);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Abbrechen";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // fontDialog1
            // 
            this.fontDialog1.ShowEffects = false;
            // 
            // FBD
            // 
            this.FBD.Description = "Bitte den Ordner \"Scripts\" auswählen";
            // 
            // Gothic
            // 
            this.Gothic.Controls.Add(this.groupBox8);
            this.Gothic.Controls.Add(this.groupBox5);
            this.Gothic.Controls.Add(this.label10);
            this.Gothic.Controls.Add(this.groupBox7);
            this.Gothic.Controls.Add(this.groupBox4);
            this.Gothic.Controls.Add(this.groupBox3);
            this.Gothic.Location = new System.Drawing.Point(10, 11);
            this.Gothic.Name = "Gothic";
            this.Gothic.Size = new System.Drawing.Size(415, 301);
            this.Gothic.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.BtBrowseScripts);
            this.groupBox3.Controls.Add(this.TScriptsPatch);
            this.groupBox3.Location = new System.Drawing.Point(16, 47);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(394, 43);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Pfad zum Scripts Ordner";
            // 
            // TScriptsPatch
            // 
            this.TScriptsPatch.Location = new System.Drawing.Point(6, 16);
            this.TScriptsPatch.Name = "TScriptsPatch";
            this.TScriptsPatch.Size = new System.Drawing.Size(297, 20);
            this.TScriptsPatch.TabIndex = 0;
            // 
            // BtBrowseScripts
            // 
            this.BtBrowseScripts.Location = new System.Drawing.Point(311, 16);
            this.BtBrowseScripts.Name = "BtBrowseScripts";
            this.BtBrowseScripts.Size = new System.Drawing.Size(70, 20);
            this.BtBrowseScripts.TabIndex = 1;
            this.BtBrowseScripts.Text = "...";
            this.BtBrowseScripts.UseVisualStyleBackColor = true;
            this.BtBrowseScripts.Click += new System.EventHandler(this.BtBrowseScripts_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.BtBrowseBilder);
            this.groupBox4.Controls.Add(this.TBilderPatch);
            this.groupBox4.Location = new System.Drawing.Point(16, 96);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(394, 43);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Bilder-Ordner";
            this.groupBox4.Visible = false;
            // 
            // TBilderPatch
            // 
            this.TBilderPatch.Location = new System.Drawing.Point(6, 16);
            this.TBilderPatch.Name = "TBilderPatch";
            this.TBilderPatch.Size = new System.Drawing.Size(297, 20);
            this.TBilderPatch.TabIndex = 0;
            // 
            // BtBrowseBilder
            // 
            this.BtBrowseBilder.Location = new System.Drawing.Point(311, 15);
            this.BtBrowseBilder.Name = "BtBrowseBilder";
            this.BtBrowseBilder.Size = new System.Drawing.Size(70, 20);
            this.BtBrowseBilder.TabIndex = 1;
            this.BtBrowseBilder.Text = "...";
            this.BtBrowseBilder.UseVisualStyleBackColor = true;
            this.BtBrowseBilder.Click += new System.EventHandler(this.BtBrowseBilder_Click);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.nBakMin);
            this.groupBox7.Controls.Add(this.label11);
            this.groupBox7.Controls.Add(this.ckbInFolderOnly);
            this.groupBox7.Controls.Add(this.BtBrowseBak);
            this.groupBox7.Controls.Add(this.TBakPatch);
            this.groupBox7.Location = new System.Drawing.Point(16, 145);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(394, 62);
            this.groupBox7.TabIndex = 1;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Backup-Ordner";
            // 
            // TBakPatch
            // 
            this.TBakPatch.Location = new System.Drawing.Point(6, 16);
            this.TBakPatch.Name = "TBakPatch";
            this.TBakPatch.Size = new System.Drawing.Size(164, 20);
            this.TBakPatch.TabIndex = 0;
            this.TBakPatch.TextChanged += new System.EventHandler(this.TBakPatch_TextChanged);
            // 
            // BtBrowseBak
            // 
            this.BtBrowseBak.Location = new System.Drawing.Point(176, 15);
            this.BtBrowseBak.Name = "BtBrowseBak";
            this.BtBrowseBak.Size = new System.Drawing.Size(70, 20);
            this.BtBrowseBak.TabIndex = 1;
            this.BtBrowseBak.Text = "...";
            this.BtBrowseBak.UseVisualStyleBackColor = true;
            this.BtBrowseBak.Click += new System.EventHandler(this.BtBrowseBak_Click);
            // 
            // ckbInFolderOnly
            // 
            this.ckbInFolderOnly.AutoSize = true;
            this.ckbInFolderOnly.Enabled = false;
            this.ckbInFolderOnly.Location = new System.Drawing.Point(6, 42);
            this.ckbInFolderOnly.Name = "ckbInFolderOnly";
            this.ckbInFolderOnly.Size = new System.Drawing.Size(176, 17);
            this.ckbInFolderOnly.TabIndex = 2;
            this.ckbInFolderOnly.Text = "Nur Backups im Backup-Ordner";
            this.ckbInFolderOnly.UseVisualStyleBackColor = true;
            this.ckbInFolderOnly.CheckedChanged += new System.EventHandler(this.ckbInFolderOnly_CheckedChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(269, 15);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(112, 13);
            this.label11.TabIndex = 3;
            this.label11.Text = "Backup alle x Minuten";
            // 
            // nBakMin
            // 
            this.nBakMin.Enabled = false;
            this.nBakMin.Location = new System.Drawing.Point(272, 31);
            this.nBakMin.Maximum = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.nBakMin.Name = "nBakMin";
            this.nBakMin.Size = new System.Drawing.Size(109, 20);
            this.nBakMin.TabIndex = 4;
            this.nBakMin.ValueChanged += new System.EventHandler(this.nBakMin_ValueChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(10, 8);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(99, 31);
            this.label10.TabIndex = 2;
            this.label10.Text = "Gothic";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.ckbMessageBox);
            this.groupBox5.Location = new System.Drawing.Point(17, 210);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(393, 42);
            this.groupBox5.TabIndex = 3;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Parser";
            // 
            // ckbMessageBox
            // 
            this.ckbMessageBox.AutoSize = true;
            this.ckbMessageBox.Location = new System.Drawing.Point(6, 18);
            this.ckbMessageBox.Name = "ckbMessageBox";
            this.ckbMessageBox.Size = new System.Drawing.Size(232, 17);
            this.ckbMessageBox.TabIndex = 0;
            this.ckbMessageBox.Text = "Fehlermeldungen als Messagebox anzeigen";
            this.ckbMessageBox.UseVisualStyleBackColor = true;
            this.ckbMessageBox.CheckedChanged += new System.EventHandler(this.ckbMessageBox_CheckedChanged);
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.ckbAutoBrackets);
            this.groupBox8.Controls.Add(this.ckbAutoCompleteAuto);
            this.groupBox8.Location = new System.Drawing.Point(17, 258);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(394, 40);
            this.groupBox8.TabIndex = 4;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Auto-Ergänzung";
            // 
            // ckbAutoCompleteAuto
            // 
            this.ckbAutoCompleteAuto.AutoSize = true;
            this.ckbAutoCompleteAuto.Location = new System.Drawing.Point(7, 19);
            this.ckbAutoCompleteAuto.Name = "ckbAutoCompleteAuto";
            this.ckbAutoCompleteAuto.Size = new System.Drawing.Size(84, 17);
            this.ckbAutoCompleteAuto.TabIndex = 0;
            this.ckbAutoCompleteAuto.Text = "Automatisch";
            this.ckbAutoCompleteAuto.UseVisualStyleBackColor = true;
            this.ckbAutoCompleteAuto.CheckedChanged += new System.EventHandler(this.ckbAutoCompleteAuto_CheckedChanged);
            // 
            // ckbAutoBrackets
            // 
            this.ckbAutoBrackets.AutoSize = true;
            this.ckbAutoBrackets.Location = new System.Drawing.Point(184, 19);
            this.ckbAutoBrackets.Name = "ckbAutoBrackets";
            this.ckbAutoBrackets.Size = new System.Drawing.Size(97, 17);
            this.ckbAutoBrackets.TabIndex = 1;
            this.ckbAutoBrackets.Text = "Auto-Klammern";
            this.ckbAutoBrackets.UseVisualStyleBackColor = true;
            this.ckbAutoBrackets.CheckedChanged += new System.EventHandler(this.ckbAutoBrackets_CheckedChanged);
            // 
            // lstMain
            // 
            this.lstMain.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lstMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstMain.FullRowSelect = true;
            this.lstMain.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            listViewItem1.ToolTipText = "Allgemein";
            listViewItem2.ToolTipText = "Editor";
            listViewItem3.ToolTipText = "Gothic";
            this.lstMain.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3});
            this.lstMain.Location = new System.Drawing.Point(0, 0);
            this.lstMain.MultiSelect = false;
            this.lstMain.Name = "lstMain";
            this.lstMain.Size = new System.Drawing.Size(199, 315);
            this.lstMain.SmallImageList = this.imgMain;
            this.lstMain.TabIndex = 0;
            this.lstMain.UseCompatibleStateImageBehavior = false;
            this.lstMain.View = System.Windows.Forms.View.Details;
            this.lstMain.SelectedIndexChanged += new System.EventHandler(this.lstMain_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 173;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lstMain);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.Gothic);
            this.splitContainer1.Size = new System.Drawing.Size(635, 315);
            this.splitContainer1.SplitterDistance = 199;
            this.splitContainer1.TabIndex = 0;
            // 
            // Options
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(635, 347);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Options";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Optionen";
            this.Editor.ResumeLayout(false);
            this.Editor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudVRuler)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTabIndent)).EndInit();
            this.Allgemein.ResumeLayout(false);
            this.Allgemein.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRecentFile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRecentProject)).EndInit();
            this.Gothic.ResumeLayout(false);
            this.Gothic.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nBakMin)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel Allgemein;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudRecentProject;
        private System.Windows.Forms.NumericUpDown nudRecentFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox ckbSaveOnExt;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel Editor;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox ckbEnableCodeFolding;
        private System.Windows.Forms.CheckBox ckbShowVRuler;
        private System.Windows.Forms.CheckBox ckbShowHRuler;
        private System.Windows.Forms.CheckBox ckbShowLineNumbes;
        private System.Windows.Forms.CheckBox ckbShowMatchingBracket;
        private System.Windows.Forms.CheckBox ckbShowTabs;
        private System.Windows.Forms.CheckBox ckbShowSpaces;
        private System.Windows.Forms.CheckBox ckbShowInvalidLines;
        private System.Windows.Forms.CheckBox ckbShowEol;
        private System.Windows.Forms.CheckBox ckbConvertTabs;
        private System.Windows.Forms.CheckBox ckbUseAntiAlias;
        private System.Windows.Forms.CheckBox ckbAllowCaretBeyondEol;
        private System.Windows.Forms.CheckBox ckbAutoInsertBracket;
        private System.Windows.Forms.CheckBox ckbHighlightCurrentLine;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nudVRuler;
        private System.Windows.Forms.NumericUpDown nudTabIndent;
        private System.Windows.Forms.ComboBox cmbBracketStyle;
        private System.Windows.Forms.ComboBox cmbIndentStyle;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnFont;
        private ICSharpCode.TextEditor.TextEditorControl textEditorControl1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.ImageList imgMain;
        private System.Windows.Forms.FolderBrowserDialog FBD;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.CheckBox ckbBackup;
        private System.Windows.Forms.Panel Gothic;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.CheckBox ckbAutoBrackets;
        private System.Windows.Forms.CheckBox ckbAutoCompleteAuto;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox ckbMessageBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.NumericUpDown nBakMin;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox ckbInFolderOnly;
        private System.Windows.Forms.Button BtBrowseBak;
        private System.Windows.Forms.TextBox TBakPatch;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button BtBrowseBilder;
        private System.Windows.Forms.TextBox TBilderPatch;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button BtBrowseScripts;
        private System.Windows.Forms.TextBox TScriptsPatch;
        public System.Windows.Forms.ListView lstMain;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        public System.Windows.Forms.SplitContainer splitContainer1;
    }
}
