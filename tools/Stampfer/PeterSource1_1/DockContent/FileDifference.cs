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
**************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using PeterInterface;
using System.Drawing;
using WeifenLuo.WinFormsUI.Docking;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Text;

namespace Peter
{
    public partial class FileDifference : DockContent, IPeterPluginTab
    {
        private IPeterPluginHost m_Host;

        public FileDifference(string[] fileList, string activeFile)
        {
            InitializeComponent();
            this.TabText = "Dateivergleich";

            foreach (string file in fileList)
            {
                if (!string.IsNullOrEmpty(file))
                {
                    this._cmbFile1.Items.Add(file);
                    this._cmbFile2.Items.Add(file);
                }
            }

            if (!string.IsNullOrEmpty(activeFile))
            {
                this._cmbFile1.Text = activeFile;
            }
        }

        #region IPeterPluginTab Members

        public void Save()
        {
        }

        public void SaveAs(string filePath)
        {
        }

        public void Cut()
        {
        }

        public void Copy()
        {
        }

        public void Paste()
        {
        }

        public void Undo()
        {
        }

        public void Redo()
        {
        }

        public void Delete()
        {
        }

        public void Duplicate()
        {
        }

        public void Print()
        {
        }

        public void SelectAll()
        {
        }

        public bool CloseTab()
        {
            this.Close();
            return true;
        }

        public IPeterPluginHost Host
        {
            get { return this.m_Host; }

            set { this.m_Host = value; }
        }

        public string FileName
        {
            get { return ""; }
        }

        public string Selection
        {
            get { return ""; }
        }

        public bool AbleToUndo
        {
            get { return false; }
        }

        public bool AbleToRedo
        {
            get { return false; }
        }

        public bool AbleToPaste
        {
            get { return false; }
        }

        public bool AbleToCut
        {
            get { return false; }
        }

        public bool AbleToCopy
        {
            get { return false; }
        }

        public bool AbleToSelectAll
        {
            get { return false; }
        }

        public bool AbleToSave
        {
            get { return false; }
        }

        public bool AbleToDelete
        {
            get { return false; }
        }

        public bool NeedsSaving
        {
            get { return false; }
        }

        public void MarkAll(System.Text.RegularExpressions.Regex reg)
        {
        }

        public bool FindNext(System.Text.RegularExpressions.Regex reg, bool searchUp)
        {
            return false ; //this.webber1.FindNext(reg.ToString());
        }

        public void ReplaceNext(System.Text.RegularExpressions.Regex reg, string replaceWith, bool searchUp)
        {
        }

        public void ReplaceAll(System.Text.RegularExpressions.Regex reg, string replaceWith)
        {
        }

        public void SelectWord(int line, int offset, int wordLeng)
        {
        }

        #endregion

        private string GetFileName()
        {
            if (this.ofdMain.ShowDialog() == DialogResult.OK)
            {
                return ofdMain.FileName;
            }

            return "";
        }

        private bool ValidFile(string file)
        {
            if (!string.IsNullOrEmpty(file))
            {
                if (File.Exists(file))
                {
                    return true;
                }
            }
            return false;
        }

        private void _btnCompare_Click (object sender, EventArgs e)
        {
           

            string file1 = this._cmbFile1.Text.Trim();
            string file2 = this._cmbFile2.Text.Trim();

            if (!this.ValidFile(file1))
            {
                MessageBox.Show("Die Datei 1 exisitert nicht.", "Stampfer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!this.ValidFile(file2))
            {
                MessageBox.Show("Die Datei 2 exisitert nicht.", "Stampfer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            StreamReader sr1 = new StreamReader(file1, Encoding.Default);
            StreamReader sr2 = new StreamReader(file2,Encoding.Default);
            string text1 = sr1.ReadToEnd();
            string text2 = sr2.ReadToEnd();
          
            sr1.Close();
            sr2.Close();

            cDiff.Item[] changes = cDiff.DiffText(text1, text2, this._ckbTrimSpace.Checked, this._ckbIgnoreSpace.Checked, this._ckbIgnoreCase.Checked);
            string[] lines1 = text1.Split('\n');
            string[] lines2 = text2.Split('\n');
            int cnt = 0;
            StringBuilder table = new StringBuilder();
            table.Append("<table cellspacing=\"0\" cellpadding=\"0\">");
            foreach (cDiff.Item item in changes)
            {
                // unchanged...
                while ((cnt < item.StartB) && (cnt < lines2.Length))
                {
                    table.Append("<tr><td>");
                    string text = lines2[cnt];
                    table.Append(cnt.ToString());
                    table.Append("</td><td>");
                    table.Append(System.Web.HttpUtility.HtmlEncode(text).Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;"));
                    table.Append("</td><td>");
                    table.Append(cnt.ToString());
                    table.Append("</td><td>");
                    table.Append(System.Web.HttpUtility.HtmlEncode(text).Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;"));
                    cnt++;
                    table.Append("</td></tr>");
                }

                // Deleted...
                for (int temp = 0; temp < item.deletedA; temp++)
                {
                    table.Append("<tr><td style=\"background-color:red;\">(");
                    string text = lines1[item.StartA + temp];
                    table.Append(Convert.ToString(item.StartA + temp + 1));
                    table.Append(")</td><td style=\"background-color:red;\">");
                    table.Append(System.Web.HttpUtility.HtmlEncode(text).Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;"));
                    table.Append("</td><td>");
                    table.Append("</td><td>");
                    table.Append("</td></tr>");
                }

                // Inserted...
                while (cnt < item.StartB + item.insertedB)
                {
                    table.Append("<tr><td>");
                    string text = lines2[cnt];
                    table.Append("</td><td>");
                    table.Append("</td><td style=\"background-color:green;\">");
                    table.Append(cnt.ToString());
                    table.Append("</td><td style=\"background-color:green;\">");
                    table.Append(System.Web.HttpUtility.HtmlEncode(text).Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;"));
                    cnt++;
                    table.Append("</td></tr>");
                }
            }

            // unchanged...
            while (cnt < lines2.Length)
            {
                table.Append("<tr><td>");
                string text = lines2[cnt];
                table.Append(cnt.ToString());
                table.Append("</td><td>");
                table.Append(System.Web.HttpUtility.HtmlEncode(text).Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;"));
                table.Append("</td><td>");
                table.Append(cnt.ToString());
                table.Append("</td><td>");
                table.Append(System.Web.HttpUtility.HtmlEncode(text).Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;"));
                cnt++;
                table.Append("</td></tr>");
            }

            table.Append("</table>");
            //this.webber1.HTML = table.ToString();
        }

        private void _btnBrowseFile1_Click (object sender, EventArgs e)
        {
            string file = this.GetFileName();
            if (!string.IsNullOrEmpty(file))
            {
                this._cmbFile1.Text = file;
                if (!this._cmbFile1.Items.Contains(file))
                {
                    this._cmbFile1.Items.Add(file);
                }
            }
        }

        private void _btnBrowseFile2_Click (object sender, EventArgs e)
        {
            string file = this.GetFileName();
            if (!string.IsNullOrEmpty(file))
            {
                this._cmbFile2.Text = file;
                if (!this._cmbFile2.Items.Contains(file))
                {
                    this._cmbFile2.Items.Add(file);
                }
            }
        }

        private void _btnBrowseFile2_Click_1 (object sender, EventArgs e)
        {

        }
    }
}