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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace Peter
{
    public partial class Project : Form
    {
        private string m_ProjectFile;

        public Project()
        {
            InitializeComponent();

            this.m_ProjectFile = null;
            this.cmbType.SelectedIndex = 0;
        }

        public void SetProjectFile(string filePath)
        {
            try
            {
                this.m_ProjectFile = filePath;
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(filePath);
                XmlNodeList nodes = xDoc.GetElementsByTagName("name");
                if (nodes.Count > 0)
                {
                    this.txtProjectName.Text = nodes[0].InnerText;
                }

                nodes = xDoc.GetElementsByTagName("type");
                if (nodes.Count > 0)
                {
                    this.cmbType.SelectedItem = nodes[0].InnerText;
                }

                nodes = xDoc.GetElementsByTagName("folder");
                foreach (XmlNode node in nodes)
                {
                    this.lstFolders.Items.Add(node.InnerText);
                }

                nodes = xDoc.GetElementsByTagName("file");
                foreach (XmlNode node in nodes)
                {
                    this.lstFiles.Items.Add(node.InnerText);
                }

                nodes = xDoc.GetElementsByTagName("WorkingDir");
                if (nodes.Count > 0)
                {
                    this.txtWorkingDir.Text = nodes[0].InnerText;
                }

                nodes = xDoc.GetElementsByTagName("BuildScript");
                if (nodes.Count > 0)
                {
                    this.richTextBox1.Text = nodes[0].InnerText;
                }

                nodes = xDoc.GetElementsByTagName("ExcludeFolderFilter");
                if (nodes.Count > 0)
                {
                    this.txtExcludeFilterFolder.Text = nodes[0].InnerText;
                }

                nodes = xDoc.GetElementsByTagName("ExcludeFileFilter");
                if (nodes.Count > 0)
                {
                    this.txtExcludeFilterFile.Text = nodes[0].InnerText;
                }

                nodes = xDoc.GetElementsByTagName("BuildFile");
                foreach (XmlNode node in nodes)
                {
                    string file = "";
                    bool check = false;

                    foreach (XmlNode cNode in node.ChildNodes)
                    {
                        if (cNode.Name.ToLower() == "file")
                        {
                            file = cNode.InnerText;
                        }
                        if (cNode.Name.ToLower() == "prebuild")
                        {
                            check = Convert.ToBoolean(cNode.InnerText);
                        }
                    }

                    this.clbBuildFiles.Items.Add(file, check);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Gets the location of the project...
        /// </summary>
        public string ProjectFile
        {
            get { return this.m_ProjectFile; }
        }

        private void btnAddFolder_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.lstFolders.Items.Add(this.folderBrowserDialog1.SelectedPath);
            }
        }

        private void btnRemoveFolder_Click(object sender, EventArgs e)
        {
            if (this.lstFolders.SelectedIndex >= 0)
            {
                this.lstFolders.Items.Remove(this.lstFolders.SelectedItem);
            }
        }

        private void btnAddFile_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in this.openFileDialog1.FileNames)
                {
                    this.lstFiles.Items.Add(file);
                }
            }
        }

        private void btnRemoveFile_Click(object sender, EventArgs e)
        {
            if (this.lstFiles.SelectedIndex >= 0)
            {
                this.lstFiles.Items.Remove(this.lstFiles.SelectedItem);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtProjectName.Text.Trim()))
            {
                MessageBox.Show("Bitte geben sie einen Projektnamen ein.", "Stmapfer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (string.IsNullOrEmpty(this.m_ProjectFile))
                {
                    if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        this.WriteXML(this.saveFileDialog1.FileName);
                        this.m_ProjectFile = this.saveFileDialog1.FileName;
                        this.Hide();
                    }
                }
                else
                {
                    if (File.Exists(this.m_ProjectFile))
                    {
                        File.Delete(this.m_ProjectFile);
                    }

                    this.WriteXML(this.m_ProjectFile);
                    this.Close();
                }
            }
        }

        private void WriteXML(string fileName)
        {
            XmlTextWriter writer = new XmlTextWriter(fileName, Encoding.Unicode);
            try
            {
                writer.Indentation = 4;
                writer.Formatting = Formatting.Indented;
                writer.WriteStartElement("PeterProject");

                // Project Name...
                writer.WriteStartElement("name");
                writer.WriteValue(this.txtProjectName.Text);
                writer.WriteEndElement();

                // Project Type...
                writer.WriteStartElement("type");
                writer.WriteValue(this.cmbType.SelectedItem.ToString());
                writer.WriteEndElement();

                // Folders...
                for (int a = 0; a < this.lstFolders.Items.Count; a++)
                {
                    writer.WriteStartElement("folder");
                    writer.WriteValue(this.lstFolders.Items[a].ToString());
                    writer.WriteEndElement();
                }

                // Files...
                for (int a = 0; a < this.lstFiles.Items.Count; a++)
                {
                    writer.WriteStartElement("file");
                    writer.WriteValue(this.lstFiles.Items[a].ToString());
                    writer.WriteEndElement();
                }

                // Filter Settings...
                writer.WriteStartElement("ExcludeFolderFilter");
                writer.WriteValue(this.txtExcludeFilterFolder.Text);
                writer.WriteEndElement();
                writer.WriteStartElement("ExcludeFileFilter");
                writer.WriteValue(this.txtExcludeFilterFile.Text);
                writer.WriteEndElement();

                // Build Settings...
                writer.WriteStartElement("BuildSettings");
                // Working Directory...
                writer.WriteStartElement("WorkingDir");
                writer.WriteValue(this.txtWorkingDir.Text.Trim());
                writer.WriteEndElement();
                // Build Script..
                writer.WriteStartElement("BuildScript");
                writer.WriteValue(this.richTextBox1.Text);
                writer.WriteEndElement();
                // Build Files..
                for (int a = 0; a < this.clbBuildFiles.Items.Count; a++)
                {
                    writer.WriteStartElement("BuildFile");
                    writer.WriteStartElement("PreBuild");
                    writer.WriteValue(this.clbBuildFiles.GetItemChecked(a).ToString());
                    writer.WriteEndElement();
                    writer.WriteStartElement("file");
                    writer.WriteValue(this.clbBuildFiles.Items[a].ToString());
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();

                writer.WriteEndElement();
            }
            finally
            {
                writer.Flush();
                writer.Close();
            }
        }

        private void btnBrowseDir_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.txtWorkingDir.Text = this.folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnMoveBuildFileUp_Click(object sender, EventArgs e)
        {
            if (this.clbBuildFiles.SelectedIndex == 0)
            {
                return;
            }

            int index = this.clbBuildFiles.SelectedIndex;
            object item = this.clbBuildFiles.Items[index];
            this.clbBuildFiles.Items.Remove(item);
            this.clbBuildFiles.Items.Insert(index - 1, item);
            this.clbBuildFiles.SelectedIndex = index - 1;
        }

        private void btnMoveBuildFileDown_Click(object sender, EventArgs e)
        {
            if (this.clbBuildFiles.SelectedIndex == this.clbBuildFiles.Items.Count - 1)
            {
                return;
            }

            int index = this.clbBuildFiles.SelectedIndex;
            object item = this.clbBuildFiles.Items[index];
            this.clbBuildFiles.Items.Remove(item);
            this.clbBuildFiles.Items.Insert(index + 1, item);
            this.clbBuildFiles.SelectedIndex = index + 1;
        }

        private void btnAddBuildfile_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in this.openFileDialog1.FileNames)
                {
                    this.clbBuildFiles.Items.Add(file);
                }
            }
        }
    }
}