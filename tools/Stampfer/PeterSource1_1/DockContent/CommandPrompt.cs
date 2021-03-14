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
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using WeifenLuo.WinFormsUI.Docking;
using PeterInterface;
using System.Text.RegularExpressions;

namespace Peter
{
    public partial class CommandPrompt : DockContent, IPeterPluginTab
    {
        public delegate void UpdateOutputCallback(string text);
        private StreamWriter m_Writer;
        private bool m_Command;
        private IPeterPluginHost m_Host;

        public CommandPrompt()
        {
            InitializeComponent();

            this.Load += new EventHandler(CommandPrompt_Load);
            this.TabText = "Command Prompt";
            this.m_Command = false;
            this.cmbInput.KeyDown += new KeyEventHandler(InputKeyPress);
            this.rtbOutput.GotFocus += new EventHandler(rtbOutput_GotFocus);
        }

        void rtbOutput_GotFocus(object sender, EventArgs e)
        {
            this.cmbInput.Focus();
        }

        void CommandPrompt_Load(object sender, EventArgs e)
        {
            this.StartCommandPrompt();
            this.cmbInput.Focus();
        }

        private void InputKeyPress(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.RunCommand(this.cmbInput.Text);
            }
        }

        private void StartCommandPrompt()
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.StartInfo.RedirectStandardError = true;
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;

            cmd.OutputDataReceived += new DataReceivedEventHandler(HandleOutput);
            cmd.ErrorDataReceived += new DataReceivedEventHandler(HandleError);

            cmd.Start();
            this.m_Writer = cmd.StandardInput;
            this.m_Writer.WriteLine("cd " + Path.GetDirectoryName(Application.ExecutablePath));
            cmd.BeginErrorReadLine();
            cmd.BeginOutputReadLine();
        }

        void HandleError(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                this.rtbOutput.Invoke(new UpdateOutputCallback(this.UpdateError), new object[] { e.Data });
            }
        }

        void HandleOutput(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                this.rtbOutput.Invoke(new UpdateOutputCallback(this.UpdateOutput), new object[] { e.Data });
            }
        }

        private void UpdateOutput(string text)
        {
            if (!this.m_Command && text.IndexOf(">cd") == text.Length - 3)
            {
                return;
            }

            int start = this.rtbOutput.TextLength;

            this.rtbOutput.AppendText(text + System.Environment.NewLine);

            if (this.m_Command)
            {
                this.rtbOutput.Select(start, text.Length);
                this.rtbOutput.SelectionColor = Color.Yellow;
                this.rtbOutput.Select(this.rtbOutput.TextLength, 0);
                this.m_Command = false;
            }

            this.rtbOutput.ScrollToCaret();
        }

        private void UpdateError(string text)
        {
            int start = this.rtbOutput.TextLength;
            this.rtbOutput.AppendText(text + System.Environment.NewLine);
            this.rtbOutput.Select(start, text.Length);
            this.rtbOutput.SelectionColor = Color.Red;
            this.rtbOutput.Select(this.rtbOutput.TextLength, 0);
            this.rtbOutput.ScrollToCaret();
        }

        /// <summary>
        /// Runs the given Script...
        /// </summary>
        /// <param name="script">Script to run (Commands should be separated by a new line '\n').</param>
        /// <param name="workingDirectory">Directory to run script.</param>
        public void RunScript(string script, string workingDirectory)
        {
            int index = workingDirectory.IndexOf(":\\");
            if (index > 0)
            {
                this.m_Writer.WriteLine(workingDirectory.Substring(0, index + 1));
            }
            this.m_Writer.WriteLine("cd " + workingDirectory);
            string[] commands = Regex.Split(script, System.Environment.NewLine);
            foreach (string command in commands)
            {
                if (!string.IsNullOrEmpty(command))
                {
                    this.m_Writer.WriteLine(command);
                }
            }
        }

        /// <summary>
        /// Runs the given command...
        /// </summary>
        /// <param name="command">Command to run.</param>
        public void RunCommand(string command)
        {
            this.m_Writer.WriteLine(command);
            this.m_Command = true;
            if (!this.cmbInput.Items.Contains(command))
            {
                this.cmbInput.Items.Add(command);
            }
            this.cmbInput.Text = "";
            this.m_Writer.WriteLine("cd");
        }

        #region IPeterPluginTab Members

        public void Save()
        {
            //this.m_Host.SaveAs(this);
        }

        public void SaveAs(string filePath)
        {
            this.rtbOutput.SaveFile(filePath);
        }

        public void Cut()
        {
        }

        public void Copy()
        {
            this.rtbOutput.Copy();
        }

        public void Paste()
        {
            if (Clipboard.ContainsText())
            {
                this.cmbInput.Text = Clipboard.GetText();
            }
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
            this.rtbOutput.SelectAll();
        }

        public bool CloseTab()
        {
            this.Close();
            return true;
        }

        public IPeterPluginHost Host
        {
            get {  return this.m_Host; }

            set { this.m_Host = value; }
        }

        public string FileName
        {
            get { return ""; }
        }

        public string Selection
        {
            get { return this.rtbOutput.SelectedText; }
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
            get { return true; }
        }

        public bool AbleToCut
        {
            get { return false; }
        }

        public bool AbleToCopy
        {
            get { return true; }
        }

        public bool AbleToSelectAll
        {
            get { return true; }
        }

        public bool AbleToSave
        {
            get { return true; }
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
            Match m = reg.Match(this.rtbOutput.Text, this.rtbOutput.SelectionStart);
            if (m.Success)
            {
                this.rtbOutput.Select(m.Index, m.Length);                
                return true;
            }
            return false;
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
    }
}
