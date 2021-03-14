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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Peter.Forms
{
    public partial class FAddQuest : Form
    {
        FQuest MainF;
        
        ArrayList Quests;
        int mode = 0;
        TreeNode t;
        Quest Q;
        public FAddQuest(TreeNode tn, ArrayList quests)
        {
            t = tn;
            Quests = quests;
            mode=t.ImageIndex;
            InitializeComponent();

            Q = (Quest)(t.Parent.Tag);       
            if (mode == 3)//von...
            {
                              
                foreach (Quest qu in Quests)
                {
                    this.LbQuestsAll.Items.Add(qu);
                }
                this.Text += " (beeinflusst von ...)";
                foreach (Quest qu in Q.PrevQuests)
                {
                    if (LbQuestsAll.Items.Contains(qu))
                    {
                        LbQuestsAll.Items.Remove(qu);
                    }
                    LbQuestsAdded.Items.Add(qu);
                }
            }
            else if (mode==4)
            {
                  
                foreach (Quest qu in Quests)
                {
                    this.LbQuestsAll.Items.Add(qu);
                }
                this.Text += " (beeinflusst ...)";
                foreach (Quest qu in Q.NextQuests)
                {
                    if (LbQuestsAll.Items.Contains(qu))
                    {
                        LbQuestsAll.Items.Remove(qu);
                    }
                    LbQuestsAdded.Items.Add(qu);
                }
            }
            

        }
        public FAddQuest(TreeNode tn, MainForm  m)
        {
            t = tn;
            Q = (Quest)(t.Parent.Tag);       
            mode = t.ImageIndex;
            InitializeComponent();

           
            if (mode == 8)
            {
                this.Text = "NPCs";
                if (m.m_GothicStructure!=null)
                {
                    foreach (Instance i in m.m_GothicStructure.NPCList.Values)
                    {                       
                        LbQuestsAll.Items.Add(i);
                    }
                    foreach (Instance i in Q.NPCs)
                    {
                        if (LbQuestsAll.Items.Contains(i))
                        {
                            LbQuestsAll.Items.Remove(i);
                        }
                        LbQuestsAdded.Items.Add(i);
                    }

                }


            }

        }
        public FAddQuest(FQuest f, int  m, ArrayList quest)
        {
            MainF = f;
            mode = m;
            Quests = quest;
            InitializeComponent();
            foreach (Quest q in Quests)
            {
                this.LbQuestsAll.Items.Add(q);
            }

            if (mode == 0)
            {
                this.Text += " (beeinflusst von ...)";
                foreach (Quest q in MainF.LbQuests1.Items)
                {
                    if (LbQuestsAll.Items.Contains(q))
                    {
                        LbQuestsAll.Items.Remove(q);
                    }
                    LbQuestsAdded.Items.Add(q);
                }
                
            }
            else if (mode ==1)
            {
                this.Text += " (beeinflusst ...)";
                foreach (Quest q in MainF.LbQuests2.Items)
                {
                    if (LbQuestsAll.Items.Contains(q))
                    {
                        LbQuestsAll.Items.Remove(q);
                    }
                    LbQuestsAdded.Items.Add(q);
                }
                
                    
                
            }


            
        }

        private void BtAdd_Click(object sender, EventArgs e)
        {
            while (LbQuestsAll.SelectedItems.Count > 0)
            {
                LbQuestsAdded.Items.Add(LbQuestsAll.SelectedItems[0]);
                LbQuestsAll.Items.Remove(LbQuestsAll.SelectedItems[0]);

            }
            
        }

        private void BtRemove_Click(object sender, EventArgs e)
        {
            while (LbQuestsAdded.SelectedItems.Count > 0)
            {
                LbQuestsAll.Items.Add(LbQuestsAdded.SelectedItems[0]);
                LbQuestsAdded.Items.Remove(LbQuestsAdded.SelectedItems[0]);

            }
        }

        private void BtCreate_Click(object sender, EventArgs e)
        {
            
            if (mode == 0)
            {
                MainF.LbQuests1.Items.Clear();
                foreach (Quest q in LbQuestsAdded.Items)
                {
                    MainF.LbQuests1.Items.Add(q);
                }
            }
            else if (mode ==1)
            {
                MainF.LbQuests2.Items.Clear();
                foreach (Quest q in LbQuestsAdded.Items)
                {
                    MainF.LbQuests2.Items.Add(q);
                }
            }
            else if (mode == 3)
            {
                Q.PrevQuests.Clear();
                Q.PrevQuestsList.Clear();
                
                foreach (Quest q in LbQuestsAdded.Items)
                {
                    Q.PrevQuestsList.Add(q.InternName);
                    Q.PrevQuests.Add(q);
                }
                Q.CreateTrees();
                SaveQuest(Q);
               

            }
            else if (mode == 4)
            {
                Q.NextQuests.Clear();
                Q.NextQuestsList.Clear();
                
                foreach (Quest q in LbQuestsAdded.Items)
                {
                    Q.NextQuestsList.Add(q.InternName);
                    Q.NextQuests.Add(q);
                }
                Q.CreateTrees();
                SaveQuest(Q);
            }
            else if (mode == 8)
            {
                Q.NPCs.Clear();
                foreach (Instance i in LbQuestsAdded.Items)
                {
                    Q.NPCs.Add(i);                    
                }
                Q.CreateTrees();
                SaveQuest(Q);
            }
            this.Close();
            this.Dispose();

            
        }
        private void SaveQuest(Quest q)
        {
            string sf = q.QuestTree.Parent.Tag.ToString() + "\\" + q.InternName + ".quest";
            FileStream fs;
            fs = new FileStream(sf, FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, q);
            fs.Close();
            fs.Dispose();
        }
    }
   
}