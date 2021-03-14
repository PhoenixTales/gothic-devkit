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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Xml;
using ICSharpCode.TextEditor.Document;
using System.IO;

namespace Peter
{
    public partial class Options : Form
    {
        public ArrayList m_OptionPanels;
        private MainForm m_MainForm;
        private bool m_EditorChanged;
        bool newscriptspath=false;
        public Options(MainForm main)
        {
            InitializeComponent();
            this.m_MainForm = main;
            this.m_OptionPanels = new ArrayList();
            this.m_OptionPanels.Add(this.Allgemein);
            this.m_OptionPanels.Add(this.Editor);
            this.m_OptionPanels.Add(this.Gothic);

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(this.m_MainForm.ConfigFile);

            XmlNodeList nodes = xDoc.GetElementsByTagName("SaveOnExit");
            if (nodes.Count > 0)
            {
                this.ckbSaveOnExt.Checked = Convert.ToBoolean(nodes[0].InnerText);
            }

            nodes = xDoc.GetElementsByTagName("RecentFileCount");
            if (nodes.Count > 0)
            {
                this.nudRecentFile.Value = Convert.ToInt32(nodes[0].InnerText);
            }

            nodes = xDoc.GetElementsByTagName("RecentProjectCount");
            if (nodes.Count > 0)
            {
                this.nudRecentProject.Value = Convert.ToInt32(nodes[0].InnerText);
            }

            nodes = xDoc.GetElementsByTagName("Editor");
            if(nodes.Count > 0)
            {
                foreach (XmlNode node in nodes[0].ChildNodes)
                {
                    switch (node.Name.ToLower())
                    {
                        case "showeol":
                            this.ckbShowEol.Checked = Convert.ToBoolean(node.InnerText);
                            break;
                        case "showinvalidlines":
                            this.ckbShowInvalidLines.Checked = Convert.ToBoolean(node.InnerText);
                            break;
                        case "showspaces":
                            this.ckbShowSpaces.Checked = Convert.ToBoolean(node.InnerText);
                            break;
                        case "showtabs":
                            this.ckbShowTabs.Checked = Convert.ToBoolean(node.InnerText);
                            break;
                        case "showmatchbracket":
                            this.ckbShowMatchingBracket.Checked = Convert.ToBoolean(node.InnerText);
                            break;
                        case "showlinenumbers":
                            this.ckbShowLineNumbes.Checked = Convert.ToBoolean(node.InnerText);
                            break;
                        case "showhruler":
                            this.ckbShowHRuler.Checked = Convert.ToBoolean(node.InnerText);
                            break;
                        case "showvruler":
                            this.ckbShowVRuler.Checked = Convert.ToBoolean(node.InnerText);
                            break;
                        case "enablecodefolding":
                            this.ckbEnableCodeFolding.Checked = Convert.ToBoolean(node.InnerText);
                            break;
                        case "converttabs":
                            this.ckbConvertTabs.Checked = Convert.ToBoolean(node.InnerText);
                            break;
                        case "useantialias":
                            this.ckbUseAntiAlias.Checked = Convert.ToBoolean(node.InnerText);
                            break;
                        case "allowcaretbeyondeol":
                            this.ckbAllowCaretBeyondEol.Checked = Convert.ToBoolean(node.InnerText);
                            break;
                        case "highlightcurrentline":
                            this.ckbHighlightCurrentLine.Checked= Convert.ToBoolean(node.InnerText);
                            break;
                        case "autoinsertbracket":
                            this.ckbAutoInsertBracket.Checked = Convert.ToBoolean(node.InnerText);
                            break;
                        case "tabindent":
                            this.nudTabIndent.Value = Convert.ToInt32(node.InnerText);
                            break;
                        case "verticalrulercol":
                            this.nudVRuler.Value = Convert.ToInt32(node.InnerText);
                            break;
                        case "indentstyle":
                            this.cmbIndentStyle.Text = node.InnerText;
                            break;
                        case "bracketmatchingstyle":
                            this.cmbBracketStyle.Text = node.InnerText;
                            break;
                        case "font":
                            string[] font = node.InnerText.Split(';');
                            Font f = new Font(font[0], Convert.ToSingle(font[1]));
                            this.fontDialog1.Font = f;
                            this.textEditorControl1.Font = f;
                            break;
                        case "scripts":
                            this.TScriptsPatch.Text = node.InnerText;
                            break;
                        case "bilder":
                            this.TBilderPatch.Text = node.InnerText;
                            break;
                        case "parser":                            
                            this.ckbMessageBox.Checked = Convert.ToBoolean(node.InnerText);
                            break;
                        case "backup":
                            this.ckbBackup.Checked = Convert.ToBoolean(node.InnerText);
                            break;
                        case "autocomplete":
                            this.ckbAutoCompleteAuto.Checked = Convert.ToBoolean(node.InnerText);
                            break;
                        case "backupfolder":
                            this.TBakPatch.Text=node.InnerText;
                            break;
                        case "backupeach":
                            this.nBakMin.Value=Convert.ToInt32(node.InnerText);
                            break;
                        case "backupfolderonly":
                            this.ckbInFolderOnly.Checked = Convert.ToBoolean(node.InnerText);
                            break;
                        case "autobrackets":
                            this.ckbAutoBrackets.Checked = Convert.ToBoolean(node.InnerText);
                            break;
                    }
                }

               
            }
            if (TBakPatch.Text != "")
            {
                ckbInFolderOnly.Enabled = true;
                nBakMin.Enabled = true;
               
            }
            else
            {
                ckbInFolderOnly.Enabled = false;
                nBakMin.Enabled = false;
                nBakMin.Value = 0;
                ckbInFolderOnly.Checked = false;
            }

            this.textEditorControl1.SetHighlighting("Java");
            this.textEditorControl1.Text = "public class Foo\r\n" +
                                            "{\r\n" +
                                            "	public int[] X = new int[]{1, 3, 5\r\n" +
                                            "		7, 9, 11};\r\n" +
                                            "\r\n" +
                                            "	public void foo(boolean a, int x,\r\n" +
                                            "	                int y, int z)\r\n" +
                                            "	{\r\n" +
                                            "		label1:\r\n" +
                                            "		do\r\n" +
                                            "		{\r\n" +
                                            "			try\r\n" +
                                            "			{\r\n" +
                                            "				if (x > 0)\r\n" +
                                            "				{\r\n" +
                                            "					int someVariable = a ?\r\n" +
                                            "						x :\r\n" +
                                            "						y;\r\n" +
                                            "				}\r\n" +
                                            "				else if (x < 0)\r\n" +
                                            "				{\r\n" +
                                            "					int someVariable = (y +\r\n" +
                                            "						z\r\n" +
                                            "					);\r\n" +
                                            "					someVariable = x =\r\n" +
                                            "						x +\r\n" +
                                            "							y;\r\n" +
                                            "				}\r\n" +
                                            "				else\r\n" +
                                            "				{\r\n" +
                                            "					label2:\r\n" +
                                            "					for (int i = 0;\r\n" +
                                            "					     i < 5;\r\n" +
                                            "					     i++)\r\n" +
                                            "						doSomething(i);\r\n" +
                                            "				}\r\n" +
                                            "				switch (a)\r\n" +
                                            "				{\r\n" +
                                            "					case 0:\r\n" +
                                            "						doCase0()\r\n;" +
                                            "						break;\r\n" +
                                            "					default:\r\n" +
                                            "						doDefault();\r\n" +
                                            "				}\r\n" +
                                            "			}\r\n" +
                                            "			catch (Exception e)\r\n" +
                                            "			{\r\n" +
                                            "				processException(e.getMessage(),\r\n" +
                                            "					x + y, z, a);\r\n" +
                                            "			\r\n}" +
                                            "			finally\r\n" +
                                            "			{\r\n" +
                                            "				processFinally();\r\n" +
                                            "			}\r\n" +
                                            "		}\r\n" +
                                            "		while (true);\r\n" +
                                            "\r\n" +
                                            "		if (2 < 3) return;\r\n" +
                                            "		if (3 < 4)\r\n" +
                                            "			return;\r\n" +
                                            "		do x++ while (x < 10000);\r\n" +
                                            "		while (x < 50000) x++;\r\n" +
                                            "		for (int i = 0; i < 5; i++) System.out.println(i);\r\n" +
                                            "	}\r\n" +
                                            "}";
            this.UpdateEditor();
            this.lstMain.Items[0].Selected = true;
            this.m_EditorChanged = false;
        }

        /// <summary>
        /// Adds an Option panel...
        /// </summary>
        /// <param name="panel">Panel to Add</param>
        public void AddOptionPanel(Control panel, Image image)
        {
            this.m_OptionPanels.Add(panel);
            ListViewItem lvi = new ListViewItem(panel.Name);
            if (image != null)
            {
                int index = this.imgMain.Images.Add(image, Color.Transparent);
                lvi.ImageIndex = index;
            }
            this.lstMain.Items.Add(lvi);
        }

        private void lstMain_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            if (this.lstMain.SelectedItems.Count == 1)
            {
               for (int a = 0; a < this.m_OptionPanels.Count; a++)
                {
                    Control ctrl = (Control)this.m_OptionPanels[a];
                 
                  if (ctrl.Name == lstMain.SelectedItems[0].Text)
                  {
                      this.splitContainer1.Panel2.Controls.Clear();
                      ctrl.Dock = DockStyle.Fill;
                      this.splitContainer1.Panel2.Controls.Add(ctrl);
                      break;
                  }
                }
            }
           
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.btnApply_Click(sender, e);
            this.Close();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.m_MainForm.Trace("Übernehme allgemeine Einstellungen");
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(this.m_MainForm.ConfigFile);

            XmlNodeList nodes = xDoc.GetElementsByTagName("SaveOnExit");
            if (nodes.Count > 0)
            {
                nodes[0].InnerText = this.ckbSaveOnExt.Checked.ToString();
            }

            nodes = xDoc.GetElementsByTagName("RecentFileCount");
            if (nodes.Count > 0)
            {
                nodes[0].InnerText = this.nudRecentFile.Value.ToString();
            }

            nodes = xDoc.GetElementsByTagName("RecentProjectCount");
            if (nodes.Count > 0)
            {
                nodes[0].InnerText = this.nudRecentProject.Value.ToString();
            }

            if (this.m_EditorChanged)
            {
                this.m_MainForm.Trace("Übernehme Editor Einstellungen");
                nodes = xDoc.GetElementsByTagName("Editor");
                if (nodes.Count > 0)
                {
                    foreach (XmlNode node in nodes[0].ChildNodes)
                    {
                        switch (node.Name.ToLower())
                        {
                            case "showeol":
                                node.InnerText = this.ckbShowEol.Checked.ToString();
                                break;
                            case "showinvalidlines":
                                node.InnerText = this.ckbShowInvalidLines.Checked.ToString();
                                break;
                            case "showspaces":
                                node.InnerText = this.ckbShowSpaces.Checked.ToString();
                                break;
                            case "showtabs":
                                node.InnerText = this.ckbShowTabs.Checked.ToString();
                                break;
                            case "showmatchbracket":
                                node.InnerText = this.ckbShowMatchingBracket.Checked.ToString();
                                break;
                            case "showlinenumbers":
                                node.InnerText = this.ckbShowLineNumbes.Checked.ToString();
                                break;
                            case "showhruler":
                                node.InnerText = this.ckbShowHRuler.Checked.ToString();
                                break;
                            case "showvruler":
                                node.InnerText = this.ckbShowVRuler.Checked.ToString();
                                break;
                            case "enablecodefolding":
                                node.InnerText = this.ckbEnableCodeFolding.Checked.ToString();
                                break;
                            case "converttabs":
                                node.InnerText = this.ckbConvertTabs.Checked.ToString();
                                break;
                            case "useantialias":
                                node.InnerText = this.ckbUseAntiAlias.Checked.ToString();
                                break;
                            case "allowcaretbeyondeol":
                                node.InnerText = this.ckbAllowCaretBeyondEol.Checked.ToString();
                                break;
                            case "highlightcurrentline":
                                node.InnerText = this.ckbHighlightCurrentLine.Checked.ToString();
                                break;
                            case "autoinsertbracket":
                                node.InnerText = this.ckbAutoInsertBracket.Checked.ToString();
                                break;
                            case "tabindent":
                                node.InnerText = this.nudTabIndent.Value.ToString();
                                break;
                            case "verticalrulercol":
                                node.InnerText = this.nudVRuler.Value.ToString();
                                break;
                            case "indentstyle":
                                node.InnerText = this.cmbIndentStyle.Text.ToString();
                                break;
                            case "bracketmatchingstyle":
                                node.InnerText = this.cmbBracketStyle.Text.ToString();
                                break;
                            case "font":
                                string font = this.textEditorControl1.Font.FontFamily.Name + ";" + textEditorControl1.Font.Size.ToString();
                                node.InnerText = font;
                                break;
                            case "scripts":
                                node.InnerText = this.TScriptsPatch.Text.ToString();
                                break;
                            case "bilder":
                                node.InnerText = this.TBilderPatch.Text.ToString();
                                break;
                            case "parser":                               
                                node.InnerText = this.ckbMessageBox.Checked.ToString();// == true ? "1" : "0";
                                break;
                            case "backup":
                                node.InnerText = this.ckbBackup.Checked.ToString();// == true ? "1" : "0";
                                break;
                            case "autocomplete":
                                node.InnerText = this.ckbAutoCompleteAuto.Checked.ToString();
                                break;
                            case "backupfolder":
                                node.InnerText = this.TBakPatch.Text;
                                break;
                            case "backupeach":
                                node.InnerText = this.nBakMin.Value.ToString();
                                break;
                            case "backupfolderonly":
                                node.InnerText = this.ckbInFolderOnly.Checked.ToString();
                                break;
                            case "autobrackets":
                                node.InnerText = this.ckbAutoBrackets.Checked.ToString();
                                break;
                        }
                    }
                }
            }
           
            xDoc.Save(this.m_MainForm.ConfigFile);

            this.m_MainForm.Trace("Übernehme Einstellungen");
            this.m_MainForm.LoadConfigFile(true);
            this.Cursor = Cursors.Default;
            this.m_MainForm.Trace("");
            if (newscriptspath)
            {
                MessageBox.Show("Script - Pfad wurde geändert. Stampfer wird nun neu gestartet.", "Neustart", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
                Application.Restart();
            }
        }

        private void btnFont_Click(object sender, EventArgs e)
        {
            this.fontDialog1.Font = this.textEditorControl1.Font;
            if (this.fontDialog1.ShowDialog() == DialogResult.OK)
            {
                this.textEditorControl1.Font = this.fontDialog1.Font;
                this.m_EditorChanged = true;
            }
        }

        private void UpdateEditor()
        {
            this.textEditorControl1.ShowEOLMarkers = this.ckbShowEol.Checked;
            this.textEditorControl1.ShowInvalidLines = this.ckbShowInvalidLines.Checked;
            this.textEditorControl1.ShowSpaces = this.ckbShowSpaces.Checked;
            this.textEditorControl1.ShowTabs = this.ckbShowTabs.Checked;
            this.textEditorControl1.ShowMatchingBracket = this.ckbShowMatchingBracket.Checked;
            this.textEditorControl1.ShowLineNumbers = this.ckbShowLineNumbes.Checked;
            this.textEditorControl1.ShowHRuler = this.ckbShowHRuler.Checked;
            this.textEditorControl1.ShowVRuler = this.ckbShowVRuler.Checked;
            this.textEditorControl1.EnableFolding = this.ckbEnableCodeFolding.Checked;
            this.textEditorControl1.ConvertTabsToSpaces = this.ckbConvertTabs.Checked;
            this.textEditorControl1.UseAntiAliasFont = this.ckbUseAntiAlias.Checked; // #develop 2
            /* // #develop 3
            if (this.ckbUseAntiAlias.Checked)
            {
                this.textEditorControl1.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            }*/
            this.textEditorControl1.AllowCaretBeyondEOL = this.ckbAllowCaretBeyondEol.Checked;
            this.textEditorControl1.TextEditorProperties.AutoInsertCurlyBracket = this.ckbAutoInsertBracket.Checked;
            this.textEditorControl1.TabIndent = Convert.ToInt32(this.nudTabIndent.Value);
            this.textEditorControl1.VRulerRow = Convert.ToInt32(this.nudVRuler.Value);

            this.textEditorControl1.LineViewerStyle = (this.ckbHighlightCurrentLine.Checked) ? LineViewerStyle.FullRow : LineViewerStyle.None;
            switch (this.cmbBracketStyle.Text.ToLower())
            {
                case "vorher":
                    this.textEditorControl1.BracketMatchingStyle = BracketMatchingStyle.Before;
                    break;
                case "nachher":
                    this.textEditorControl1.BracketMatchingStyle = BracketMatchingStyle.After;
                    break;
            }
            switch (this.cmbIndentStyle.Text.ToLower())
            {
                case "auto":
                    this.textEditorControl1.IndentStyle = IndentStyle.Auto;
                    break;
                case "none":
                    this.textEditorControl1.IndentStyle = IndentStyle.None;
                    break;
                case "smart":
                    this.textEditorControl1.IndentStyle = IndentStyle.Smart;
                    break;
            }
            this.m_EditorChanged = true;
        }

        private void ckbShowEol_CheckedChanged(object sender, EventArgs e)
        {
            this.UpdateEditor();
        }

        private void ckbShowInvalidLines_CheckedChanged(object sender, EventArgs e)
        {
            this.UpdateEditor();
        }

        private void ckbShowSpaces_CheckedChanged(object sender, EventArgs e)
        {
            this.UpdateEditor();
        }

        private void ckbShowTabs_CheckedChanged(object sender, EventArgs e)
        {
            this.UpdateEditor();
        }

        private void ckbShowMatchingBracket_CheckedChanged(object sender, EventArgs e)
        {
            this.UpdateEditor();
        }

        private void ckbShowLineNumbes_CheckedChanged(object sender, EventArgs e)
        {
            this.UpdateEditor();
        }

        private void ckbShowHRuler_CheckedChanged(object sender, EventArgs e)
        {
            this.UpdateEditor();
        }

        private void ckbShowVRuler_CheckedChanged(object sender, EventArgs e)
        {
            this.UpdateEditor();
        }

        private void ckbEnableCodeFolding_CheckedChanged(object sender, EventArgs e)
        {
            this.UpdateEditor();
        }

        private void ckbConvertTabs_CheckedChanged(object sender, EventArgs e)
        {
            this.UpdateEditor();
        }

        private void ckbUseAntiAlias_CheckedChanged(object sender, EventArgs e)
        {
            this.UpdateEditor();
        }

        private void ckbAllowCaretBeyondEol_CheckedChanged(object sender, EventArgs e)
        {
            this.UpdateEditor();
        }

        private void ckbHighlightCurrentLine_CheckedChanged(object sender, EventArgs e)
        {
            this.UpdateEditor();
        }

        private void ckbAutoInsertBracket_CheckedChanged(object sender, EventArgs e)
        {
            this.UpdateEditor();
        }

        private void nudTabIndent_ValueChanged(object sender, EventArgs e)
        {
            this.UpdateEditor();
        }

        private void nudVRuler_ValueChanged(object sender, EventArgs e)
        {
            this.UpdateEditor();
        }

        private void cmbIndentStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.UpdateEditor();
        }

        private void cmbBracketStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.UpdateEditor();
        }

        public void BtBrowseScripts_Click(object sender, EventArgs e)
        {
            FBD = new FolderBrowserDialog();
            if (this.FBD.ShowDialog() == DialogResult.OK)
            {
                string s=FBD.SelectedPath.ToLower();
                if (Directory.Exists(s))
                {
                    if (!s.ToLower().EndsWith("scripts"))
                    {
                        MessageBox.Show("Der angegebene Pfad sollte mit 'Scripts' enden, sofern Sie die Gothic-Originalscripte und Eintellungen unverändert vorliegen haben.", "Pfad zu den Scripten", MessageBoxButtons.OK, MessageBoxIcon.Information);
               
                    }
                    this.TScriptsPatch.Text = s;
                    this.m_EditorChanged = true;
                    newscriptspath = true;
                }
                else
                {
                    MessageBox.Show("Der angegebene Pfad existiert nicht.", "Ungültiger Pfad", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            
        }

        private void BtBrowseBilder_Click(object sender, EventArgs e)
        {
            FBD = new FolderBrowserDialog();
            if (this.FBD.ShowDialog() == DialogResult.OK)
            {

                this.TBilderPatch.Text = FBD.SelectedPath.ToLower();
                this.m_EditorChanged = true;
            }
        }

        private void ckbMessageBox_CheckedChanged(object sender, EventArgs e)
        {
            this.m_EditorChanged = true;
        }

        private void ckbBackup_CheckedChanged(object sender, EventArgs e)
        {
            this.m_EditorChanged = true;
        }

        private void ckbAutoCompleteAuto_CheckedChanged(object sender, EventArgs e)
        {
            this.m_EditorChanged = true;
        }

        private void BtBrowseBak_Click(object sender, EventArgs e)
        {
            FBD = new FolderBrowserDialog();
            if (this.FBD.ShowDialog() == DialogResult.OK)
            {

                this.TBakPatch.Text = FBD.SelectedPath.ToLower();
                this.m_EditorChanged = true;
            }
        }

        private void nBakMin_ValueChanged(object sender, EventArgs e)
        {
            this.m_EditorChanged = true;
        }

        private void ckbInFolderOnly_CheckedChanged(object sender, EventArgs e)
        {
            this.m_EditorChanged = true;
        }

        private void TBakPatch_TextChanged(object sender, EventArgs e)
        {
            if (TBakPatch.Text != "")
            {
                ckbInFolderOnly.Enabled = true;
                nBakMin.Enabled = true;
               
            }
            else
            {
                ckbInFolderOnly.Enabled = false;
                nBakMin.Enabled = false;
                nBakMin.Value = 0;
                ckbInFolderOnly.Checked = false;
            }
        }

        private void ckbAutoBrackets_CheckedChanged(object sender, EventArgs e)
        {
            this.m_EditorChanged = true;
        }

        
    }
}
