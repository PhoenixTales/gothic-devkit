/**************************************************************************************
    Stampfer - Gothic Script Editor
    Copyright (C) 2009 Alexander "Sumpfkrautjunkie" Ruppert

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
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace Peter
{
    public partial class Info : Form
    {
        public Info()
        {
            InitializeComponent();
        }

        private void BtLicense_Click(object sender, EventArgs e)
        {
            if (File.Exists(Path.GetDirectoryName(Application.ExecutablePath)+"\\License.rtf"))
            {               
                Process.Start(Path.GetDirectoryName(Application.ExecutablePath)+"\\License.rtf");
            }
        }

        private void BtClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Process.Start(e.LinkText);
        }

       
    }
}
