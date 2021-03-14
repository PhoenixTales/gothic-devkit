/**************************************************************************************
    Stampfer - Gothic Script Editor
    Copyright (C)  2009 Alexander "Sumpfkrautjunkie" Ruppert

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
using System.IO;

namespace Peter.Forms
{
    public partial class FOrdner : Form
    {
        TreeNode t;
        ContextMenuStrip cms;
        bool editmode;
        public FOrdner(TreeNode tn, ContextMenuStrip maincms, bool ed)
        {
            editmode = ed;
            t = tn;
            cms = maincms;
            this.Text+= "   "+t.Text;
            InitializeComponent();
            if (!editmode)
            {
                BtCreate.Text = "Erstellen";
            }
            else
            {
                BtCreate.Text = "Ändern";
                TbName.Text = t.Text;
                TbBeschreibung.Text = t.ToolTipText;
                   
            }
            
        }

        private void BtCreate_Click(object sender, EventArgs e)
        {
            if (TbName.Text.Length > 0)
            {
                try
                {
                    foreach (TreeNode trn in t.Nodes)
                    {
                        if (trn.Text == TbName.Text)
                        {
                            MessageBox.Show(TbName.Text + " bereits vorhanden", "Ungültiger Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }


                    if (!editmode)
                    {
                        TreeNode tn = new TreeNode();
                        tn.Text = TbName.Text;
                        tn.ContextMenuStrip = cms;
                        tn.ToolTipText = TbBeschreibung.Text;
                        tn.ImageIndex = 2;
                        tn.SelectedImageIndex = tn.ImageIndex;

                       
                        string dir = t.Tag.ToString() + "\\" + TbName.Text;
                        tn.Tag = dir;
                        t.Nodes.Add(tn);
                        Directory.CreateDirectory(dir);
                        using (StreamWriter sw = new StreamWriter(dir + "\\" + TbName.Text + ".dfo", false, Encoding.Default))
                        {
                            sw.Write(TbBeschreibung.Text);
                        }
                    }
                    else
                    {
                        
                        t.ToolTipText = TbBeschreibung.Text;
                        string dirBase = t.Tag.ToString().Remove(t.Tag.ToString().Length - t.Text.Length);
                        string dir = dirBase + "\\" + TbName.Text;
                        string dirOld = dirBase + "\\" + t.Text;
                        if (dirOld.ToLower() != dir.ToLower())
                        {
                            Directory.Move(dirOld, dir);
                        }
                        if (File.Exists(dir +"\\"+ t.Text + ".dfo"))
                        {
                            File.Delete(dir +"\\"+ t.Text + ".dfo");
                        }
                        using (StreamWriter sw = new StreamWriter(dir + "\\" + TbName.Text + ".dfo", false, Encoding.Default))
                        {
                            sw.Write(TbBeschreibung.Text);
                        }
                        t.Text = TbName.Text;
                        t.Tag = dirBase + TbName.Text;
                        
                        
                    }
                }
                catch(IOException m)
                {
                    MessageBox.Show(m.ToString());
                }
            }
            this.Close();
            this.Dispose();
        }

        private void FOrdner_Resize(object sender, EventArgs e)
        {
            try { TbBeschreibung.Height=BtCreate.Top-TbBeschreibung.Top; }
            catch { }
        }
    }
}