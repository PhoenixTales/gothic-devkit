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

namespace Peter.Forms
{
    public partial class FNPCSelect : Form
    {
        DialogCreator DC;
        public FNPCSelect(MainForm m, DialogCreator d)
        {
            DC = d;
            InitializeComponent();
            
            if (m.m_GothicStructure != null)
            {
                List<Instance> inst = new List<Instance>();
                foreach (Instance i in m.m_GothicStructure.NPCList.Values)
                {
                    inst.Add(i);
                }
                inst.Sort();
                foreach(Instance n in inst)
                {
                    LbNPCs.Items.Add(n);
                }
                
            }
        }

        private void BtCreate_Click(object sender, EventArgs e)
        {
            if (LbNPCs.SelectedItem != null)
            {
                DC.npc = (Instance)LbNPCs.SelectedItem;
                DC.BtNPC.Text = DC.npc.Name;
                DC.GetNPCInformation();
                DC.DiaName = "DIA_" + DC.npc_name + "_" + DC.TbDialogName.Text;
                this.Close();
                 this.Dispose();

            }
            else
            {
                MessageBox.Show("Kein NPC ausgewählt!", "Ungültige Auswahl", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
           
        }

        private void LbNPCs_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (LbNPCs.SelectedItem != null)
            {
                BtCreate_Click(null, new EventArgs());
            }
        }
        int loc = 0;
        private void TFind_TextChanged(object sender, EventArgs e)
        {

            loc = -1;
            Find();
            
            
        }
        
        private void Find()
        {

            string searchstring = TFind.Text.ToLower().Trim();
            if (searchstring.Length == 0) return;
            int i = loc;
            bool found = false; ;
            if (i < LbNPCs.Items.Count)
            {
                i++;
            }
            else
            {
                i = 0;
                loc = 0;
            }
            for (i = i; i < LbNPCs.Items.Count; i++)
            {
                if (LbNPCs.Items[i].ToString().ToLower().Contains(searchstring))
                {
                    loc=LbNPCs.SelectedIndex = i;
                    found = true;
                    break;
                }
            }
            if (!found&&loc>0)
            {
                loc = -1;
                Find();
            }
        }

        private void BtNext_Click(object sender, EventArgs e)
        {
            Find();
        }
    }
}