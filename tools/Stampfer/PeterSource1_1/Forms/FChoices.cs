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
    public partial class FChoices : Form
    {
        DialogCreator MainF;
        bool EditMode;
        string cont="";
        public FChoices(DialogCreator d, bool m)
        {
            MainF = d;
            EditMode = m;
            InitializeComponent();
            if (EditMode)
            {
                TbName.Text = d.LbChoices.SelectedItem.ToString();
                TbText.Text = ((Choice)d.LbChoices.SelectedItem).Text;
            }
        }

        private void TbName_TextChanged(object sender, EventArgs e)
        {
            string s = TbName.Text;
            if (s.Length == 0)
            {
                
                return;
            }
            

            int i = 0;
            while (i < s.Length)
            {
                if (!Char.IsLetterOrDigit(s[i]))
                {
                    s = s.Remove(i, 1);
                    s = s.Insert(i, "_");
                }
                i++;
            }
            TbName.Text = s;
            TbName.Select(TbName.Text.Length, 0);
        }

        private void BtOk_Click(object sender, EventArgs e)
        {
            bool doppelt = false;
            foreach (Choice c in MainF.LbChoices.Items)
            {
                if (c.Name == TbName.Text)
                {
                    doppelt = true;
                    break;
                }
            }
            if (doppelt)
            {
                MessageBox.Show("Choice mit gleichem Namen exisiteiert bereits. Bitte geben sie einen anderen Namen ein.", "Ungültiger Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (EditMode)
            {
                cont = ((Choice)MainF.LbChoices.SelectedItem).Content;
                if (((Choice)MainF.LbChoices.SelectedItem) == MainF.ActualChoice)
                {
                    MainF._grprInfo.GroupTitle = TbText.Text;
                }
                MainF.LbChoices.Items.Remove(MainF.LbChoices.SelectedItem);
                MainF.LbChoices.SelectedItem = null;
                MainF.LbChoices.Items.Add(new Choice(TbName.Text, TbText.Text,cont));
            }
            else
            {
                MainF.LbChoices.Items.Add(new Choice(TbName.Text,TbText.Text,cont));
            }
            this.Close();
            this.Dispose();
        }

        private void BtBack_Click(object sender, EventArgs e)
        {
            TbName.Text = "Back";
            TbText.Text = "Zurück";
            cont = "@@";

        }

        private void FChoices_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtOk_Click(null, new EventArgs());
            }
        }

        private void TbName_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                BtOk_Click(null, new EventArgs());
            }
        }

        private void TbText_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                BtOk_Click(null, new EventArgs());
            }
        }
    }
}