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
using System.Collections;
using System.Reflection;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Peter.Forms
{
    public partial class FQuest : Form
    {
        TreeNode t;
        public ArrayList Quests;
        
        ctrlQuestManager QM;
        public FQuest(TreeNode tn, ArrayList q, ctrlQuestManager qm)
        {
            t = tn;
            QM = qm;
            Quests = q;
            InitializeComponent();
        }

        private void LbXPAdd_Click(object sender, EventArgs e)
        {
            FXP f = new FXP(this);
            f.ShowDialog(this);
        }

        private void BtXPRemove_Click(object sender, EventArgs e)
        {
                
                while (LbXP.SelectedItems.Count > 0)
                {
                    
                    LbXP.Items.Remove(LbXP.SelectedItems[0]);
                    
                }
            
        }

        private void BtQuest1Add_Click(object sender, EventArgs e)
        {
            FAddQuest aq = new FAddQuest(this, 0, Quests);
            aq.ShowDialog(this);
        }

        private void TbName_TextChanged(object sender, EventArgs e)
        {
            
            string s = TbName.Text;
            if (s.Length == 0)
            {
                LbName.Text = "";
                return;
            }
            if (Char.IsDigit(s[0]))
            {
                s=s.Insert(0, "_");
            }

            int i = 0;
            while(i<s.Length)
            {
                if (!Char.IsLetterOrDigit(s[i]))
                {
                    s=s.Remove(i,1);
                    s = s.Insert(i, "_");
                }
                i++;
            }
            LbName.Text = s;
        }

        private void BtCreate_Click(object sender, EventArgs e)
        {
            if (TbName.Text=="")
            {
                MessageBox.Show("Die Quest braucht einen Namen", "Ungültige Quest", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }           
            
            
            foreach (Quest qw in Quests)
            {
                if (String.Compare(qw.InternName.ToLower(),LbName.Text.ToLower())==0)
                {
                    MessageBox.Show("Interner Name " + LbName.Text + " bereits vergeben!", "Ungültige Quest", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            Quest[] QuestArray1 = new Quest[LbQuests1.Items.Count];
            Quest[] QuestArray2 = new Quest[LbQuests2.Items.Count];
            object[] XPArray = new object[LbXP.Items.Count];
            LbQuests1.Items.CopyTo(QuestArray1, 0);
            LbQuests2.Items.CopyTo(QuestArray2, 0);
            LbXP.Items.CopyTo(XPArray, 0);

            Quest q = new Quest(TbName.Text, LbName.Text, TbTagebuch.Text, TbBeschreibung.Text,QuestArray1,QuestArray2,XPArray);
            q.AddToTree(t).ContextMenuStrip = QM.QuestStrip;
            q.TPrevQuests.ContextMenuStrip = QM.pnQuests;
            q.TNextQuests.ContextMenuStrip = QM.pnQuests;

            q.TDialoge.ContextMenuStrip = QM.Dias;
            q.TDiaryEntries.ContextMenuStrip = QM.DiaryEntries;
            q.TNPCs.ContextMenuStrip = QM.NPCs;
            q.TXP.ContextMenuStrip = QM.XP_Menu;


            Quests.Add(q);
            string SaveFile = t.Tag.ToString() + "\\" + LbName.Text + ".quest";
            FileStream myStream;
            myStream = new FileStream(SaveFile, FileMode.Create);
            BinaryFormatter binFormatter = new BinaryFormatter();
            binFormatter.Serialize(myStream, q);
            myStream.Close();
            myStream.Dispose();

            this.Close();
            this.Dispose();
        }

        private void BtQuest2Add_Click(object sender, EventArgs e)
        {
            FAddQuest aq = new FAddQuest(this, 1, Quests);
            aq.ShowDialog(this);
        }
    }
}