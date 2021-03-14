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
using PeterInterface;
using System.Drawing;
using WeifenLuo.WinFormsUI.Docking;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

namespace Peter
{
    public partial class DialogCreator : DockContent, IPeterPluginTab
    {
        private IPeterPluginHost m_Host;
       
        MainForm MainF;

        private int DialogVariable=0;

        public Editor EdInfo;
        public Editor EdCond;
        public int Ed_Active=0;
        public Instance npc;
        public string npc_name="";
        public string npc_voice="";
        public string DiaName = "";
        string LastClipboardText;
        string MainContent = "";
        public Regex r;
        public Quest quest;
        public Choice ActualChoice=null;
        List<string> FuncNames = new List<string>();

        public DialogCreator(MainForm m)
        {
            MainF = m;
            InitializeComponent();
            this.TabText = "Dialog-Assistent";
        
            EdCond = new Editor("ConditionEditor", MainF, this);
            EdCond.SetupEditor(MainF.m_EditorConfig);
            EdCond.Host = MainF;

            EdCond.m_Editor.ActiveTextAreaControl.TextArea.Enter += new EventHandler(TextArea_Enter_Cond);
            EdCond.m_Editor.ActiveTextAreaControl.TextArea.LostFocus += new EventHandler(TextArea_LostFocus_Cond);
            EdCond.m_Editor.SetHighlighting("Daedalus");
            EdCond.TopLevel = false;
            EdCond.SetContextMenuStrip(MainF.ctxEditor);
            EdCond.FormBorderStyle = FormBorderStyle.None;
            EdCond.Parent = pCondition;
            EdCond.Dock = DockStyle.Fill;
            EdCond.Show();
          

            EdInfo = new Editor("InfoEditor", MainF, this);
            EdInfo.SetupEditor(MainF.m_EditorConfig);
            EdInfo.Host = MainF;
            EdInfo.m_Editor.ActiveTextAreaControl.TextArea.Enter+=new EventHandler(TextArea_Enter_Info);

            EdInfo.m_Editor.ActiveTextAreaControl.TextArea.LostFocus += new EventHandler(TextArea_LostFocus_Info);
            EdInfo.m_Editor.SetHighlighting("Daedalus");
            EdInfo.TopLevel = false;
            EdInfo.SetContextMenuStrip(MainF.ctxEditor);
            EdInfo.FormBorderStyle = FormBorderStyle.None;
            EdInfo.Parent = pInfo;
            EdInfo.Dock = DockStyle.Fill;
            EdInfo.Show();


            //EdCond.m_Editor.Text = "return TRUE;";
            EdCond.m_Editor.ActiveTextAreaControl.Document.Replace(0, EdCond.m_Editor.ActiveTextAreaControl.Document.TextContent.Length, "return TRUE;");
            LbChoices.Sorted = true;
            CbStandard.SelectedIndex = 0;
           // ReadShortFunc();
        }

        void TextArea_LostFocus_Cond(object sender, EventArgs e)
        {
            //Ed_Active = 0;
        }
        void TextArea_LostFocus_Info(object sender, EventArgs e)
        {
            //Ed_Active = 0;
        }

        void TextArea_Enter_Cond(object sender, EventArgs e)
        {
            Ed_Active = 1;
        }
        void TextArea_Enter_Info(object sender, EventArgs e)
        {
            Ed_Active = 2;
        }

       

        
        

        #region IPeterPluginTab Members

        public void Save()
        {
        }

        public void SaveAs(string filePath)
        {
        }

        public void Cut()
        {
            if (Ed_Active==1)
            {
                EdCond.Cut();
            }
            else if (Ed_Active==2)
            {
                EdInfo.Cut();
            }
        }

        public void Copy()
        {
            if (Ed_Active == 1)
            {
                EdCond.Copy();
            }
            else if (Ed_Active == 2)
            {
                EdInfo.Copy();
            }
        }

        public void Paste()
        {
            if (LastClipboardText != Clipboard.GetText())
            {
                Regex rg = new Regex("\n\t");
                string s=rg.Replace(Clipboard.GetText(),"\n");
                LastClipboardText = s;
                Clipboard.SetText(s);
            }
            if (Ed_Active == 1)
            {
                EdCond.Paste();
            }
            else if (Ed_Active == 2)
            {
                EdInfo.Paste();
            }
        }

        public void Undo()
        {
            if (Ed_Active == 1)
            {
                EdCond.Undo();
            }
            else if (Ed_Active == 2)
            {
                EdInfo.Undo();
            }
        }

        public void Redo()
        {
            if (Ed_Active == 1)
            {
                EdCond.Redo();
            }
            else if (Ed_Active == 2)
            {
                EdInfo.Redo();
            }
        }

        public void Delete()
        {
            if (Ed_Active == 1)
            {
                EdCond.Delete();
            }
            else if (Ed_Active == 2)
            {
                EdInfo.Delete();
            }
        }

        public void Duplicate()
        {
            if (Ed_Active == 1)
            {
                EdCond.Duplicate();
            }
            else if (Ed_Active == 2)
            {
                EdInfo.Duplicate();
            }
        }

        public void Print()
        {
        }

        public void SelectAll()
        {
            if (Ed_Active == 1)
            {
                EdCond.SelectAll();
            }
            else if (Ed_Active == 2)
            {
                EdInfo.SelectAll();
            }
        }

        public bool CloseTab()
        {
            this.Close();
            return true;
        }

        public IPeterPluginHost Host
        {
            get { return this.m_Host; }

            set { this.m_Host = value; }
        }

        public string FileName
        {
            get { return ""; }
        }

        public string Selection
        {
            get { return ""; }
        }

        public bool AbleToUndo
        {
            get { return false; }
        }

        public bool AbleToRedo
        {
            get { return false; }
        }

        public bool AbleToPaste
        {
            get { return false; }
        }

        public bool AbleToCut
        {
            get { return false; }
        }

        public bool AbleToCopy
        {
            get { return false; }
        }

        public bool AbleToSelectAll
        {
            get { return false; }
        }

        public bool AbleToSave
        {
            get { return false; }
        }

        public bool AbleToDelete
        {
            get { return false; }
        }

        public bool NeedsSaving
        {
            get { return false; }
        }

        public void MarkAll(System.Text.RegularExpressions.Regex reg)
        {
        }

        public bool FindNext(System.Text.RegularExpressions.Regex reg, bool searchUp)
        {
            return false ; //this.webber1.FindNext(reg.ToString());
        }

        public void ReplaceNext(System.Text.RegularExpressions.Regex reg, string replaceWith, bool searchUp)
        {
        }

        public void ReplaceAll(System.Text.RegularExpressions.Regex reg, string replaceWith)
        {
        }

        public void SelectWord(int line, int offset, int wordLeng)
        {
        }

        #endregion

        private void BtChoicesAdd_Click(object sender, EventArgs e)
        {
            Forms.FChoices ch = new Peter.Forms.FChoices(this, false);
            ch.ShowDialog(this);
        }

        private void BtChoicesDelete_Click(object sender, EventArgs e)
        {
            if (LbChoices.SelectedItem != null)
                LbChoices.Items.Remove(LbChoices.SelectedItem);
        }

        private void BtChoicesEdit_Click(object sender, EventArgs e)
        {
            if (LbChoices.SelectedItem == null) return;
            Forms.FChoices ch = new Peter.Forms.FChoices(this, true);
            if (ActualChoice != null)
            {
                ActualChoice.Content = EdInfo.m_Editor.Text;
            }
            ch.ShowDialog(this);
        }

        public void BtBack_Click(object sender, EventArgs e)
        {
            LbChoices.SelectedItem = null;
            if (ActualChoice != null)
            {
                ActualChoice.Content = EdInfo.m_Editor.ActiveTextAreaControl.Document.TextContent;
                EdInfo.m_Editor.ActiveTextAreaControl.Document.TextContent=MainContent;
                this._grprCondition.Visible = true;
                ActualChoice = null;
                this._grprInfo.GroupTitle = "Info";
            }

        }

        private void BtNPC_Click(object sender, EventArgs e)
        {
            Forms.FNPCSelect npcf = new Peter.Forms.FNPCSelect(MainF, this);
            npcf.Show(this);
            
        }

        public void GetNPCInformation()
        {
            npc_name = npc.Name;
            npc_voice = "0";
            string s,s2;
            string temp="";
            int i;
            i=0;
           
            using (StreamReader sr = new StreamReader(npc.File, Encoding.Default))
            {
                s=sr.ReadToEnd();
            }
            try
            {
                r = new Regex(@"\s" + npc.Name, RegexOptions.IgnoreCase);
                Match m = r.Match(s);
                
                s = s.Remove(0, m.Index + m.Length);
                r = new Regex(@"\s" + "name", RegexOptions.IgnoreCase);
                m = r.Match(s);
                s2 = s.Remove(0, m.Index + m.Length);
                r = new Regex("\"");
                m = r.Match(s2);
                r=new Regex(";");
                i = m.Index + m.Length;
                if ((r.Match(s,0,m.Index)).Success)
                {
                    npc_name = npc.Name;
                }
                else
                {
                    while (s2[i] != '"')
                    {


                        temp += s2[i];
                        i++;
                    }
                    npc_name = temp;
                }

                
                temp = "";
                r = new Regex(@"\s" + "voice", RegexOptions.IgnoreCase);
                m = r.Match(s);
                s2 = s.Remove(0, m.Index + m.Length);
                r = new Regex("=");
                m = r.Match(s2);
                i = m.Index + m.Length;
                while (s2[i] != ';')
                {

                    temp += s2[i];
                    i++;
                }
                npc_voice = temp.Trim();
            }
            catch
            {
                
            }

        }
        private string GenerateInstance()
        {
            string s;
            s = @"// ************************************************************"+"\r\n" +
                "// 			  	" + TbDialogName.Text + "\r\n" +
                "// ************************************************************\r\n\r\n" +
                "Instance " + DiaName + " (C_INFO)\r\n" +
                "{\r\n" +
                "\tnpc = " + npc.Name + ";\r\n" +
                "\tnr = " + this.nNr.Value.ToString() + ";\r\n" +
                "\tcondition = " + DiaName + "_Condition;\r\n" +
                "\tinformation = " + DiaName + "_Info;\r\n" +
                "\tdescription = \"" + TbDescription.Text + "\";\r\n";
                if (ckbImportant.Checked)
                {
                    s += "\timportant = TRUE;\r\n";
                }
                if (ckbPermanent.Checked)
                {
                    s += "\tpermanent = TRUE;\r\n";
                }
                if (ckbTrade.Checked)
                {
                    s += "\ttrade = TRUE;\r\n";
                }
                s += "};\r\n\r\n";
            return s;
        }
        private string GenerateCondition(string st)
        {
            string s,t;
            t="int "+DiaName + "_Condition";

            FuncNames.Add(t);
            s = "Func "+t+"()\r\n" +
                "{\r\n\t";           
            s += st.Replace("\r\n", "\r\n\t");
            s+="\r\n};\r\n\r\n";

            return s;
        }
        private string GenerateInfo(string st)
        {
            string s,t;
            t = "void " + DiaName + "_Info";

            FuncNames.Add(t);
            s = "Func "+t+"()\r\n" +
                "{\r\n\t";
            s += st.Replace("\n", "\n\t");
            s += "\r\n};\r\n\r\n";

            return s;
        }
        public List<FoundFuncs> CheckForExistance(string file)
        {
            List<FoundFuncs> found = new List<FoundFuncs>();
            Regex r;
            MatchCollection mc;
            int k;
            int openbrace;
            int closebrace;
            int start;
            int end;
           // string file = File.ReadAllText(path, Encoding.Default);
            for (int i=0;i<FuncNames.Count;i++)
            {
                if (FuncNames[i].StartsWith("@"))
                {
                    r = new Regex("instance(\\s)*" + FuncNames[i].Remove(0,1) + "(\\s)*\\(", RegexOptions.IgnoreCase);

                }
                else
                {
                    r = new Regex("func(\\s)*" + FuncNames[i] + "(\\s)*\\(", RegexOptions.IgnoreCase);

                }
                mc = r.Matches(file);
                
                foreach (Match m in mc)
                {
                    k=0;
                    openbrace = 0;
                    closebrace = 0;
                    start = 0;
                    end = 0;
                    start = m.Index;                    
                    for (k = m.Index + m.Length; k < file.Length; k++)
                    {
                        if (file[k] == '{')
                        {
                            

                            openbrace++;
                        }
                        else if (file[k] == '}')
                        {
                            closebrace++;
                        }
                        else if ((openbrace == closebrace) && (openbrace > 0) && (file[k] == ';'))
                        {
                            
                            end = k+1;
                            //MessageBox.Show(file.Substring(start, end - start));
                            break;
                        }
                    }
                    if ((start > 0 && end > 0))
                        found.Add(new FoundFuncs(i, start, end));

                }
            }
            return found;
        }
        private void BtCreate_Click(object sender, EventArgs e)
        {
            FuncNames.Clear();
            
            if (npc == null)
            {
                MessageBox.Show("Kein NPC ausgewählt!", "Ungültige Auswahl", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (TbDialogName.Text=="")
            {
                MessageBox.Show("Kein Name eingegeben!", "Ungültiger Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            DialogVariable = 0;
            string ed1 = EdCond.m_Editor.ActiveTextAreaControl.Document.TextContent;
            string ed2 = EdInfo.m_Editor.ActiveTextAreaControl.Document.TextContent;
            if (ActualChoice != null)
            {
                ActualChoice.Content = EdInfo.m_Editor.ActiveTextAreaControl.Document.TextContent;
                ed2 = MainContent;
            }
            
            ed1 = TransformChoices(ed1);
            //ed2 = TransformChoices(ed2);
            /*ed1 = MainF.m_AutoComplete.TransformShortFunc(EdCond);
            ed2 = MainF.m_AutoComplete.TransformShortFunc(EdInfo);
            
            ed1 = TransformOutputs(ed1, null);
            ed2 = TransformOutputs(ed2, null);*/
            if (ActualChoice == null)
            {
                MainContent = EdInfo.m_Editor.ActiveTextAreaControl.Document.TextContent;
            }
            string mess,t;
            mess = GenerateInstance() + GenerateCondition(MainF.m_AutoComplete.TransformShortFunc(TransformOutputs(TransformChoices(ed1), null))) + GenerateInfo(MainF.m_AutoComplete.TransformShortFunc(TransformOutputs(TransformChoices(MainContent),null)));
            
            foreach (Choice ch in LbChoices.Items)
            {
                t="void " + DiaName + "_" + ch.Name;
                FuncNames.Add(t);
                mess += "Func "+t + "()\r\n{\r\n\t" +
                    MainF.m_AutoComplete.TransformShortFunc(TransformOutputs(TransformChoices(ch.Content.Replace("\n", "\n\t")),ch)) + "\r\n};\r\n\r\n";

            }
            string newfilepath=MainF.m_ScriptsPath+FilePaths.ContentDialoge+"DIA_" + npc_name + ".d";

            FuncNames.Add("@" + DiaName);
            bool filenew=false;

            if(!File.Exists(newfilepath))
            {
                using (StreamWriter sw = new StreamWriter(newfilepath, true, Encoding.Default))
                {
                    sw.Write("");
                    sw.Close();
                }
                filenew=true;
            }

            
            //Editor ned = MainF.CreateNewEditor("DIA_" + npc_name + ".d");
            //ned.Show(MainF.DockMain);
            //MainF.CreateEditor(newfilepath, Path.GetFileName(newfilepath),null);
            Editor alreadyopened = null; ;
            for (int x = 0; x < MainF.DockMain.Contents.Count; x++)
            {
                if (MainF.DockMain.Contents[x] is Editor)
                {
                    if (((Editor)MainF.DockMain.Contents[x]).FileName == newfilepath)
                    {

                        alreadyopened = ((Editor)MainF.DockMain.Contents[x]);
                        break;
                    }
                }                
            }
            Editor ned;
            if (alreadyopened == null)
            {
                
                    ned = MainF.CreateNewEditor(Path.GetFileName(newfilepath));
                    ned.LoadFile(newfilepath);
                    //ned.Show(MainF);
                    ned.Show(MainF.DockMain);
                    for (int x = 0; x < MainF.DockMain.Contents.Count; x++)
                    {
                        if (MainF.DockMain.Contents[x] is Editor)
                        {
                            if (((Editor)MainF.DockMain.Contents[x]).TabText==ned.TabText
                                && ((Editor)MainF.DockMain.Contents[x])!=ned)
                            {

                                ((Editor)MainF.DockMain.Contents[x]).CloseTab();
                                break;
                            }
                        }
                    }
                

                
            }
            else
            {
                ned = alreadyopened;
               
            }
            ned.Activate();
            
            if (filenew)
            {
                ned.ignoreclose = true;
                File.Delete(newfilepath);
                //ned.m_Editor.ActiveTextAreaControl.Document.TextContent += mess;
            }
            //else
            {
                
                List<FoundFuncs> found = CheckForExistance(ned.m_Editor.ActiveTextAreaControl.Document.TextContent);
                found.Sort();
                StringBuilder sb=new StringBuilder();
                foreach(FoundFuncs f in found)
                {
                    if (FuncNames[f.Id].StartsWith("@"))
                    {
                        sb.AppendLine("Instance "+ FuncNames[f.Id].Remove(0,1));
                    }
                    else
                    {
                        sb.AppendLine("Func " + FuncNames[f.Id]);
                    }
                    
                }
                if (found.Count > 0)
                {
                    if (MessageBox.Show(MainF, ned.m_Editor.FileName + " enhält bereits die Funktionen/Instanzen:\r\n" + sb + "\r\nSollen diese ersetzt werden?",
                    "Stampfer", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        int last = 0;
                        
                        string temp;
                        int k = 0;
                        if (found[0].Start > 0)
                        {
                            last = found[0].Start-1;
                            for (int i = found[0].Start-1; i >= 0; i--)//kommentar entfernen
                            {
                                
                               
                                if ((ned.m_Editor.ActiveTextAreaControl.Document.TextContent[i] == '\n')||(i == 0))
                                {
                                    temp = ned.m_Editor.ActiveTextAreaControl.Document.TextContent.Substring(i, last - i).Trim();
                                    if (!(temp.StartsWith("//") || temp.Length == 0))
                                    {

                                        ned.m_Editor.ActiveTextAreaControl.Document.TextContent = ned.m_Editor.ActiveTextAreaControl.Document.TextContent.Remove(last, found[0].Start - last);
                                        k = found[0].Start - last;
                                        break;
                                    }
                                    last = i;

                                }

                            }
                        }
                        for (int i = 0; i < found.Count; i++)
                        {
                            //MessageBox.Show(found[i].Start.ToString() + " " + found[i].End.ToString()+ "\r\n"+ned.m_Editor.ActiveTextAreaControl.Document.TextContent.Substring(found[i].Start,found[i].End-found[i].Start));
                            ned.m_Editor.ActiveTextAreaControl.Document.TextContent=ned.m_Editor.ActiveTextAreaControl.Document.TextContent.Remove(found[i].Start - k, (found[i].End - found[i].Start));
                            k += found[i].End - found[i].Start;
                            
                        }

                        ned.m_Editor.ActiveTextAreaControl.Document.TextContent = ned.m_Editor.ActiveTextAreaControl.Document.TextContent.Trim()+"\r\n\r\n"+ mess;
                    }                    
                }
                else
                {
                    ned.m_Editor.ActiveTextAreaControl.Document.TextContent += mess;
                }
            }

            
            //MessageBox.Show(mess);
            //Clipboard.SetText(mess);
           /* foreach (DictionaryEntry sfl in ShortFuncList)
            {
                ShortFunc sf = (ShortFunc)sfl.Value;
                string s = "";
                MessageBox.Show(sf.Prarams.Count.ToString());
                foreach (int i in sf.Prarams)
                {
                    s += i.ToString() + " ";
                }
                MessageBox.Show(sf.Short + " " + sf.FuncText+" "+s);
            }*/
        }

        private void TbDialogName_TextChanged(object sender, EventArgs e)
        {
            string s = TbDialogName.Text;
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
            TbDialogName.Text = s;
            TbDialogName.Select(TbDialogName.Text.Length, 0);
            DiaName = "DIA_" + npc_name + "_" + TbDialogName.Text;
        }
        private string TransformOutputs(string st, Choice ch)
        {
            string s = st;
            Regex os = new Regex(">>");
            Regex so = new Regex("<<");
            Regex n = new Regex("\n");
            Match m1,m2;
            string chstring="";
            string builder="";            
            string temp = "";
            int i = 0;
            while ((m1 = os.Match(s, i)).Success)
            {
                i = m1.Index + m1.Length;
                if ((m2 = n.Match(s, i)).Success)
                {
                    temp = s.Substring(i, m2.Index - i);                    
                }
                else
                {
                    temp = s.Substring(i, s.Length-i);
                }

                if (ch != null)
                {
                    chstring = "_"+ch.Name;
                }
                else
                {
                    chstring = "";
                }
                builder="AI_Output (other, self, \""+DiaName+chstring+"_15_"+DialogVariable.ToString()+"\"); //"+temp;
                DialogVariable++;
                s = s.Remove(m1.Index, m1.Length + temp.Length);
                s = s.Insert(m1.Index, builder);
                i += builder.Length - m1.Length;
                if (i >= s.Length) break;
            }
            i = 0;
            while ((m1 = so.Match(s, i)).Success)
            {
                i = m1.Index + 2;
                if ((m2 = n.Match(s, i)).Success)
                {
                    temp = s.Substring(i, m2.Index-i);
                }
                else
                {
                    temp = s.Substring(i, s.Length -i);
                }
                if (ch != null)
                {
                    chstring = "_"+ch.Name;
                }
                else
                {
                    chstring = "";
                }
                builder = "AI_Output (self, other, \"" + DiaName + chstring + "_" + npc_voice + "_" + DialogVariable.ToString() + "\"); //" + temp;
                DialogVariable++;
                s = s.Remove(m1.Index, m1.Length + temp.Length);
                s = s.Insert(m1.Index, builder);
                i += builder.Length - m1.Length;
                if (i >= s.Length) break;
            }

            return s;
        }
        private string TransformChoices(string st)
        {
            string s = st;
            Regex r = new Regex("@");
            Match m;
            int i=0;
            
            string temp="";
            string builder = "";
            while ((m = r.Match(s, i)).Success)
            {
                builder = "";
                i = m.Index + 1;
                if (s[i] == '@')
                {
                    s = s.Remove(m.Index, 2);
                    //s = s.Insert(m.Index, "Info_ClearChoices (" + DiaName + ");");
                    builder = "Info_ClearChoices (" + DiaName + ");";

                }
                else
                {
                    temp = "";
                    while (i < s.Length)
                    {
                        if (!Char.IsWhiteSpace(s[i]) && s[i] != ';' && s[i] != '=')
                        {
                            temp += s[i];
                            i++;
                        }
                        else
                        {
                            i++;
                            break;
                        }
                    }
                    s = s.Remove(m.Index, temp.Length + 1);
                    //MessageBox.Show(temp);
                    foreach (Choice c in LbChoices.Items)
                    {
                        if (c.Name.ToLower() == temp.ToLower())
                        {
                            builder = "Info_AddChoice (" + DiaName + " ,\"" + c.Text + "\", " + DiaName + "_" + c.Name + ");";
                            break;
                        }
                       
                    }
                    if (builder == "")
                    {
                        builder = "<unbekannte Choice!>";
                    }
                }
              //  builder = "\t" + builder;
                s = s.Insert(m.Index, builder);
                i = m.Index+builder.Length;
                if (i >= s.Length) break;

            }
            return s;
        }
        private string TransformShortFunc(string st)
        {
           string s = st; 

            return s;

        }


       

        private void LbChoices_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LbChoices.SelectedItem != null)
            {
                Choice ch = (Choice)LbChoices.SelectedItem;
                if (ActualChoice == null)
                {
                    MainContent = EdInfo.m_Editor.ActiveTextAreaControl.Document.TextContent;
                    
                }
                else
                {
                    ActualChoice.Content = EdInfo.m_Editor.ActiveTextAreaControl.Document.TextContent;
                }
                this._grprCondition.Visible = false; 
                if (ch.Content=="")
                {
                   // EdInfo.m_Editor.Text = "@@\n>>"+ch.Text+"\n";
                    EdInfo.m_Editor.ActiveTextAreaControl.Document.Replace(0, EdInfo.m_Editor.ActiveTextAreaControl.Document.TextContent.Length, "@@\n>>" + ch.Text + "\n");
            
                    
                }
                else
                {
                    //EdInfo.m_Editor.Text = ch.Content;
                    EdInfo.m_Editor.ActiveTextAreaControl.Document.Replace(0, EdInfo.m_Editor.ActiveTextAreaControl.Document.TextContent.Length, ch.Content);
                }
                this._grprInfo.GroupTitle = ch.Text;
                EdInfo.Refresh();              
                ActualChoice = ch;

            }
        }

        private void BtReset_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Wollen Sie eventuelle Änderungen verlieren und den Dialog-Assistenten zurücksetzen?","Achtung!",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
            {
                LbChoices.Items.Clear();
                BtBack_Click(null, new EventArgs());

                EdCond.m_Editor.ActiveTextAreaControl.Document.Replace(0, EdCond.m_Editor.ActiveTextAreaControl.Document.TextContent.Length, "return TRUE;");
                EdCond.Refresh();
                EdInfo.m_Editor.ActiveTextAreaControl.Document.Replace(0, EdInfo.m_Editor.ActiveTextAreaControl.Document.TextContent.Length, "");
                EdInfo.Refresh();



            }
        }
        string oldTbDescription;
        private void TbDescription_TextChanged(object sender, EventArgs e)
        {

            if (ActualChoice==null)
            {
               
                string t=TbDescription.Text;
                if (TbDescription.Text.Length > 0)
                {
                    if (EdInfo.m_Editor.Text.Length == 0)
                    {
                        //EdInfo.m_Editor.Text = ">>" + t;
                        EdInfo.m_Editor.ActiveTextAreaControl.Document.Replace(0, EdInfo.m_Editor.ActiveTextAreaControl.Document.TextContent.Length, ">>" + t);
                    }
                    else if (EdInfo.m_Editor.Text.StartsWith(">>"))
                    {
                        if (EdInfo.m_Editor.Text.Remove(0, 2) == oldTbDescription)
                        {
                            //EdInfo.m_Editor.Text = ">>" + t;
                            EdInfo.m_Editor.ActiveTextAreaControl.Document.Replace(0, EdInfo.m_Editor.ActiveTextAreaControl.Document.TextContent.Length, ">>" + t);
                        }
                    }
                }
                else
                {
                    if (EdInfo.m_Editor.Text.StartsWith(">>") && EdInfo.m_Editor.Text.Remove(0,2) == oldTbDescription)
                    {
                        //EdInfo.m_Editor.Text = ">>";
                        EdInfo.m_Editor.ActiveTextAreaControl.Document.Replace(0, EdInfo.m_Editor.ActiveTextAreaControl.Document.TextContent.Length, ">>");


                    }
                }
                
            }
            oldTbDescription = TbDescription.Text;
        }

        private void BtStandardCreate_Click(object sender, EventArgs e)
        {
            if (EdCond.m_Editor.Text.Length>100||EdInfo.m_Editor.Text.Length>100)
            {
                if(MessageBox.Show("Bereits getätigte Änderungen gehen verloren. Sind Sie sicher?","Achtung!",MessageBoxButtons.YesNo,MessageBoxIcon.Question)!=DialogResult.Yes)
                {
                    return;
                }
            }
            switch (CbStandard.Text.ToLower())
            {
                case "ende":
                    TbDialogName.Text = "EXIT";
                    TbDescription.Text = "ENDE";
                   // EdCond.m_Editor.Text = "return TRUE;";
                    EdCond.m_Editor.ActiveTextAreaControl.Document.Replace(0, EdCond.m_Editor.ActiveTextAreaControl.Document.TextContent.Length, "return TRUE;");

                    EdCond.Refresh();
                    //EdInfo.m_Editor.Text = "AI_StopProcessInfos	(self);";
                    EdInfo.m_Editor.ActiveTextAreaControl.Document.Replace(0, EdInfo.m_Editor.ActiveTextAreaControl.Document.TextContent.Length, "AI_StopProcessInfos	(self);");
                    EdInfo.Refresh();
                    nNr.Value = 999;
                    break;
                case "trade":
                    break;
                case "beklauen":
                    Forms.FBeklauen bkl = new Peter.Forms.FBeklauen(this);
                    bkl.ShowDialog(this);
                    break;
                case "teach":
                    break;

            }
        }

        
        
        
    }
    public class Choice
    {
        public string Name;
        public string Text;
        public string Content="";
        public Choice(string n, string t)
        {
            Name = n;
            Text = t;
        }
        public Choice(string n, string t, string c)
        {
            Name = n;
            Text = t;
            Content = c;
        }
        public override string ToString()
        {
            return Name;
        }
    }
    public struct ParameterPos:IComparable
    {
        public int pos;
        public string parameter;
        int IComparable.CompareTo(object obj)
        {
            ParameterPos p = (ParameterPos)(obj);
            return this.pos - p.pos;
        }
    }
    public class Parameter
    {
        public string ParamName = "";
        public ArrayList ParamPos = new ArrayList();
        public Parameter()
        {
        }
    }
    public class ShortFunc
    {
        public string Short = "";
        public string FuncText = "";
        //public string LoopText = "";
        public int loopstart = -1;
        public int loopend = -1;
        public ArrayList Params = new ArrayList();
        public ArrayList LoopParams = new ArrayList();
        public ShortFunc()
        {
        }
    }
    public class FoundFuncs: IComparable
    {
        public int Id = 0;
        public int Start = 0;
        public int End = 0;
        public FoundFuncs(int id, int s, int e)
        {
            Id = id;
            Start = s;
            End = e;
        }
        int IComparable.CompareTo(object x)
        {
           // if (x == null && y == null) return 0;
          //  else if (x == null) return -1;
           // else if (y == null) return 1;
           // if (x == y) return 0;

            return this.Start - ((FoundFuncs)x).Start;
            
        }
    }
    public class PosComparer : IComparer
    {

        public PosComparer()
        {

        }
        int IComparer.Compare(object x, object y)
        {
            if (x == null && y == null) return 0;
            else if (x == null) return -1;
            else if (y == null) return 1;
            if (x == y) return 0;


            ParameterPos f = (ParameterPos)x;

            ParameterPos g = (ParameterPos)y ;


            return f.pos-g.pos;
        }
    }

}