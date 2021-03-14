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
using System.Text;
using System.Windows.Forms;
using ICSharpCode.TextEditor.Document;

namespace Peter
{
    public partial class GoToLine : Form
    {
        private MainForm frm;
        public GoToLine(MainForm fr)
        {
            InitializeComponent();
            frm = fr;
        }
        
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)           
            {
                button1_Click(null, null);
            }
            else if (e.KeyCode==Keys.Escape)
            {
                 Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (frm.ActiveEditor == null)
            {
                Close();
               
                return;
            }
            int line;

           if (textBox1.Text != "")
            {


                try
                {
                    line = Convert.ToInt32(textBox1.Text) - 1;

                    if (line >= 0)
                    {
                        frm.ActiveEditor.JumpTo(line);

                    }
                }
                catch
                {
                }
            }
            Close();
          
            
        }

        private void GoToLine_Load(object sender, EventArgs e)
        {
            textBox1.Focus();
        }

       
    }
}