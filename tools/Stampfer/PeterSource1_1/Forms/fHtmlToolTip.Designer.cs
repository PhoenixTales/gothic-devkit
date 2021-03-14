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
namespace Peter
{
    partial class fHtmlToolTip
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
            this.WebMain = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // WebMain
            // 
            this.WebMain.AllowNavigation = false;
            this.WebMain.AllowWebBrowserDrop = false;
            this.WebMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WebMain.IsWebBrowserContextMenuEnabled = false;
            this.WebMain.Location = new System.Drawing.Point(0, 0);
            this.WebMain.MinimumSize = new System.Drawing.Size(20, 20);
            this.WebMain.Name = "WebMain";
            this.WebMain.ScriptErrorsSuppressed = true;
            this.WebMain.ScrollBarsEnabled = false;
            this.WebMain.Size = new System.Drawing.Size(202, 49);
            this.WebMain.TabIndex = 0;
            // 
            // HtmlToolTip
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Info;
            this.ClientSize = new System.Drawing.Size(202, 49);
            this.ControlBox = false;
            this.Controls.Add(this.WebMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "HtmlToolTip";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser WebMain;
    }
}