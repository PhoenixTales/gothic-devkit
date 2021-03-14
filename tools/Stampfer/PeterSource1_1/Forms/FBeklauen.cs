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
    public partial class FBeklauen : Form
    {
        DialogCreator MainF;
        bool NPCismale = true;
        public FBeklauen(DialogCreator d)
        {
            MainF = d;
            if (d.npc_voice == "16" || d.npc_voice == "17")
            {
                NPCismale = false;
            }
            InitializeComponent();
        }

        private void nDex_ValueChanged(object sender, EventArgs e)
        {
            if (NPCismale)
            {
                if (nDex.Value<=20)
                {
                    TbDescription.Text = "(Es wäre ein Kinderspiel seinen Geldbeutel zu stehlen)";
                }
                else if (nDex.Value <= 40)
                {
                    TbDescription.Text = "(Es wäre einfach seinen Geldbeutel zu stehlen)";
                }
                else if (nDex.Value <= 60)
                {
                    TbDescription.Text = "(Es wäre gewagt seinen Geldbeutel zu stehlen)";
                }
                else if (nDex.Value <= 80)
                {
                    TbDescription.Text = "(Es wäre schwierig seinen Geldbeutel zu stehlen)";
                }
                else if (nDex.Value <= 100)
                {
                    TbDescription.Text = "(Es wäre verdammt schwierig seinen Geldbeutel zu stehlen)";
                }
                else if (nDex.Value <= 120)
                {
                    TbDescription.Text = "(Es wäre fast unmöglich seinen Geldbeutel zu stehlen)";
                }
            }
            else
            {
                if (nDex.Value <= 20)
                {
                    TbDescription.Text = "(Es wäre ein Kinderspiel ihren Geldbeutel zu stehlen)";
                }
                else if (nDex.Value <= 40)
                {
                    TbDescription.Text = "(Es wäre einfach ihren Geldbeutel zu stehlen)";
                }
                else if (nDex.Value <= 60)
                {
                    TbDescription.Text = "(Es wäre gewagt ihren Geldbeutel zu stehlen)";
                }
                else if (nDex.Value <= 80)
                {
                    TbDescription.Text = "(Es wäre schwierig ihren Geldbeutel zu stehlen)";
                }
                else if (nDex.Value <= 100)
                {
                    TbDescription.Text = "(Es wäre verdammt schwierig ihren Geldbeutel zu stehlen)";
                }
                else if (nDex.Value <= 120)
                {
                    TbDescription.Text = "(Es wäre fast unmöglich ihren Geldbeutel zu stehlen)";
                }
            }
        }

        private void BtCreate_Click(object sender, EventArgs e)
        {
            string temp;
            MainF.TbDialogName.Text = "PICKPOCKET";
            MainF.TbDescription.Text = this.TbDescription.Text;
            MainF.LbChoices.Items.Clear();
            temp = "B_Beklauen ();\n@@";
            MainF.LbChoices.Items.Add(new Choice("DoIt","(Taschendiebstahl versuchen)",temp));
            temp = "@@";
            MainF.LbChoices.Items.Add(new Choice("BACK", "ZURÜCK", temp));
            MainF.BtBack_Click(null, new EventArgs());
            MainF.EdCond.m_Editor.Text = "C_Beklauen ("+nDex.Value.ToString()+", "+nGold.Value.ToString()+");";
            MainF.EdCond.Refresh();
            MainF.EdInfo.m_Editor.Text = "@@\n@BACK\n@DoIt";
            MainF.EdInfo.Refresh();
            this.Close();
            this.Dispose();


        }

        private void nGold_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtCreate_Click(null, new EventArgs());
            }
        }
    }
}