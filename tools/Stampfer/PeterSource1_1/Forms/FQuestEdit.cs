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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;

namespace Peter.Forms
{
    public partial class FQuestEdit : Form
    {
        TreeNode t;
        Quest quest;
        ArrayList Quests;
        
        public FQuestEdit(TreeNode tn,ArrayList quests)
        {
            t = tn;
            Quests = quests;
            quest = (Quest)t.Tag;            
            InitializeComponent();
            TbName.Text = quest.Name;
            TbBeschreibung.Text = quest.Description;
            TbTitle.Text = quest.Title;
            LbName.Text = quest.InternName;
        }
        bool found = false;
        private void BtCreate_Click(object sender, EventArgs e)
        {
            if (TbName.Text == "")
            {
                MessageBox.Show("Die Quest braucht einen Namen", "Ungültige Quest", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string oldSaveFile = t.Parent.Tag.ToString() + "\\" + quest.InternName + ".quest";
            string SaveFile = t.Parent.Tag.ToString() + "\\" + LbName.Text + ".quest";
            if (File.Exists(oldSaveFile)) 
            {
                //MessageBox.Show("1");
                File.Delete(oldSaveFile);
            }
            //Check bei Verweisen...

            
            string OldInternName = quest.InternName;
            quest.Name = TbName.Text;
            quest.InternName = LbName.Text;
            quest.Title = TbTitle.Text;
            quest.Description = TbBeschreibung.Text;
            quest.TDiaryEntries.ToolTipText = TbTitle.Text;

            FileStream myStream;
            myStream = new FileStream(SaveFile, FileMode.Create);
            BinaryFormatter binFormatter = new BinaryFormatter();
            binFormatter.Serialize(myStream, quest);
            myStream.Close();
            myStream.Dispose();

            t.ToolTipText = LbName.Text + "\n" + TbTitle.Text + "\n\n" + TbBeschreibung.Text;
            t.Text = TbName.Text;


            string sf;
            foreach (Quest q in Quests)
            {
                found = false;
                if (q.PrevQuestsList.Contains(OldInternName))
                {
                    q.PrevQuestsList.Remove(OldInternName);
                    q.PrevQuestsList.Add(LbName.Text);
                    q.TPrevQuests.Nodes.Clear();
                    foreach (Quest pq in q.PrevQuests)
                    {
                        TreeNode tn = new TreeNode(pq.Name);
                        tn.Tag = pq;
                        tn.ToolTipText = pq.InternName;
                        tn.ImageIndex = 3;
                        tn.SelectedImageIndex = tn.ImageIndex;
                        q.TPrevQuests.Nodes.Add(tn);
                    }
                    found = true;
                }
                if (q.NextQuestsList.Contains(OldInternName))
                {
                    q.NextQuestsList.Remove(OldInternName);
                    q.NextQuestsList.Add(LbName.Text);
                    q.TNextQuests.Nodes.Clear();
                    foreach (Quest nq in q.NextQuests)
                    {
                        TreeNode tn = new TreeNode(nq.Name);
                        tn.Tag = nq;
                        tn.ToolTipText = nq.InternName;
                        tn.ImageIndex = 4;
                        tn.SelectedImageIndex = tn.ImageIndex;
                        q.TNextQuests.Nodes.Add(tn);
                    }
                    found = true;
                }
                if (found == true)
                {

                    sf = q.QuestTree.Parent.Tag.ToString() + "\\" + q.InternName + ".quest";
                    FileStream fs;
                    fs = new FileStream(sf, FileMode.Create);
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(fs, q);
                    fs.Close();
                    fs.Dispose();
                }

            }

            
            this.Close();
            this.Dispose();
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
                s = s.Insert(0, "_");
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
            LbName.Text = s;
        }
    }
}