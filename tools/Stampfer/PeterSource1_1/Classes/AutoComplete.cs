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
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;


namespace Peter.Classes
{
    public class AutoComplete: Label
    {
      
        const string KEYWORDDATEI="KeyWords\\KeyWords";
        const string PROPDATEI = "KeyWords\\Properties";
        const string FUNCDIR = "DialogCreator\\Funcs.txt";
        public List<KeyWord> KW = new List<KeyWord>();
        public List<KeyWord> MyItems = new List<KeyWord>();
        ToolTip TT;
        public ListView2 listView1;
        const int ItemHeight = 16;
        public int LastItemIndex;

        public int CaretPos=0;
        Editor currentEditor = null;
        public Hashtable ShortFuncList = new Hashtable();
        public List<KeyWord> ShortFuncs = new List<KeyWord>();
        public List<KeyWord> Properties = new List<KeyWord>();
        Hashtable DokuTable = new Hashtable();
       // private System.ComponentModel.IContainer components;
        public string Extension = ".d";
        ImageList ImgList;
        ToolStripStatusLabel Trace;
        public string ScriptsPath;

       
        public AutoComplete(ImageList img, ToolStripStatusLabel tlt)
        {
            listView1=  new ListView2(this);
            TT= new ToolTip();
          //  listView1.SelectedIndexChanged += new EventHandler(listView1_SelectedIndexChanged);
            
            ImgList = img;
            Trace = tlt;
            LastItemIndex = 0;
           // this.Sorted = true;            
            TT.Hide(this);
            M_AutoCompleteDa.Active = true;
            UpdateContent();
            this.Width = Sizes.Width;
            //this.listView1.Enabled = false;
            this.Enter += new EventHandler(AutoComplete_Enter);
            
            this.Height = Sizes.Height;
            this.SizeChanged += new EventHandler(AutoComplete_SizeChanged);
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            SetupListview(MyItems.Count);
            //this.listView1.MouseDoubleClick += new MouseEventHandler(AutoComplete_MouseDown);
            //ReadShortFunc();
            Read_KeyWords();
            
            
            
        }






        public void SelectedChanged()
        {
            listView1_SelectedIndexChanged(null, new EventArgs());
        }
        public void MouseInsert()
        {
            
            if (listView1.SelectedIndices.Count > 0
                && currentEditor!=null)
            {
                currentEditor.InsertACText();
                AHide();
            }
        }
        public void MouseRemove()
        {
            currentEditor.m_Editor.Focus();
            AHide();
        }
        void AutoComplete_Enter(object sender, EventArgs e)
        {
            //AHide();
            
        }
 
        void AutoComplete_SizeChanged(object sender, EventArgs e)
        {
            if (this.listView1!=null)
            {
                this.listView1.Height = this.Height;
            }
        }

        
        void listView_RetrieveVirtualItem(object sender,
           RetrieveVirtualItemEventArgs e)
        {
            //e.Item = lvi[e.ItemIndex];

            ListViewItem item = new ListViewItem(MyItems[e.ItemIndex].ToString(), ((KeyWord)MyItems[e.ItemIndex]).Type);
            
            e.Item = item;
            
            
            //e.Item.ImageIndex = 0;
        }
        private void SetupListview(int lenght)
        {
            if (listView1!=null &&  this.Controls.Contains(listView1))
            {
                this.Controls.Remove(listView1);
            }
            this.listView1 = new ListView2(this);
            listView1.SmallImageList = ImgList;
            
                
            
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.ShowGroups = true;
            
            this.listView1.Dock = DockStyle.Fill;
            //this.listView1.View = View.List;
            this.listView1.RetrieveVirtualItem +=
                    new RetrieveVirtualItemEventHandler(
                    listView_RetrieveVirtualItem);
            this.listView1.VirtualListSize = lenght;
            this.listView1.VirtualMode = true;
            this.listView1.HideSelection = false;
            
            this.Controls.Add(this.listView1);
            this.listView1.BringToFront();
            this.listView1.MultiSelect = false;
            listView1.View = View.Details;
            listView1.HeaderStyle = ColumnHeaderStyle.None;
            ColumnHeader h = new ColumnHeader();

            h.Width = listView1.ClientSize.Width-SystemInformation.VerticalScrollBarWidth;
            listView1.Columns.Add(h);
           


        }
        private void SetupListview2(int lenght)
        {
            
            this.listView1.VirtualListSize = lenght;
            ColumnHeader h = new ColumnHeader();
            h.Width = listView1.ClientSize.Width;
           
        }
        public void NewKeyWordFile(string s)
        {
            MyItems.Clear();
            //KW.Clear();
            TT.Hide(this);
            if (File.Exists(KEYWORDDATEI + s))
            {
                //this.BeginUpdate();
                Extension = s;
                //this.Items.Clear();
                
                //UpdateContent();
                LastItemIndex = 0;
               
                
                listView1_SelectedIndexChanged(null,new EventArgs());
                //this.EndUpdate();
            }
        }
        public void UpdatePos(Editor e)
        {
            
            this.Left =  10+e.GetCaretPos().X;
            this.Top = e.GetCaretPos().Y + (int)(e.Font.Size * 4f);
            if (e.DiaC != null)
            {
                if (e.DiaC.Ed_Active == 1)
                {
                    this.Top += e.DiaC.pCondition.Top + e.DiaC._grprCondition.Top;
                    this.Left += e.DiaC.pCondition.Left + e.DiaC._grprCondition.Left;
                }
                else
                {
                    this.Top += e.DiaC.pInfo.Top + e.DiaC._grprInfo.Top;
                    this.Left += e.DiaC.pInfo.Left + e.DiaC._grprInfo.Left;
                }
                
            }
            listView1_SelectedIndexChanged(null,new EventArgs());
        }
        public void UpdateSize()
        {
            //this.BeginUpdate();
            /*if (MyItems.Count*20 < this.Height)
            {
                this.Height = MyItems.Count * 20;
                
            }
            else if (((MyItems.Count * 20 + 10 < Sizes.Height)) && ((MyItems.Count * 20 > this.Height + 10)))
            {
                this.Height = ItemHeight*20;
            }
            else if ((this.Height + 10 < Sizes.Height) && ((MyItems.Count * 20 >= Sizes.Height)))
            {

                this.Height = Sizes.Height;
            }*/
            if (MyItems.Count * 16 < Sizes.Height)
            {
                this.Height = MyItems.Count * 16+16;

            }
            else
            {
                this.Height = Sizes.Height;
            }
            //this.EndUpdate();
           
        }
        string ck;
        public int GetFistKeyWordMatch(int i1, int i2, string s)
        {

            int center = (i1 + i2) / 2;
            //MessageBox.Show(i1.ToString() + "  " + i2.ToString() + "\n" + ((KeyWord)KW[center]).Name + "\n" + s);
            if (((i2 - i1) > 3))
            {

                ck = KW[center].Name;
                if (ck.Length > s.Length)
                {
                    ck = ck.Substring(0, s.Length);
                }



                if (String.Compare(ck, s, true) > 0)//Wenn KW kleiner s
                {
                    return GetFistKeyWordMatch(i1, center + 1, s);
                }
                else if (String.Compare(ck, s, true) < 0)//Wenn KW größer s
                {
                    return GetFistKeyWordMatch(center - 1, i2, s);
                }
                else
                {



                    return center;

                }
            }
            else
            {
                //MessageBox.Show("Test");
                for (int i = i1; i < i2; i++)
                {
                    if (((KeyWord)KW[i]).Name.ToLower().StartsWith(s.ToLower()))
                    {
                        return i;
                    }
                }
                return -1;
            }

        }
       
        public Point FindKeyWords(string s)
        {
            Point p = new Point(-1, -1);
            Point p2 = new Point(-1, -1);
            Point p3 = new Point(-1, -1);
            int l1;
            //int temp=0;
            
            

            //RekRuns = KW.Count;
            l1=GetFistKeyWordMatch(0, KW.Count-1,s);
            
            //MessageBox.Show(l1.ToString());
            if (l1>=0)
            {
                //MessageBox.Show(l1.ToString());
                p = new Point(l1, l1);
                //MessageBox.Show(p.X + "  " + p.Y);
                if (l1>0)                
                {
                    p2.X = p.X;
                    while (p2.X > 0)
                    {
                        p2.X = GetFistKeyWordMatch(0, p2.X, s);
                        if (p2.X >= 0)
                        {
                            p3.X = p2.X;
                        }

                    }
                    p2.X = p3.X;

                    /*while ((p2.X > 0) && ((KeyWord)KW[p2.X-1]).Name.ToLower().StartsWith(s))
                    {
                        p2.X--;
                    }*/
                }
                if (l1 < KW.Count - 1)
                {
                    p2.Y = p.Y; 
                    while (p2.Y >= 0 && p2.Y<KW.Count-1)
                    {

                        p2.Y = GetFistKeyWordMatch(p2.Y+1, KW.Count-1, s);
                       
                        if (p2.Y >= 0)
                        {
                            p3.Y = p2.Y;
                        }

                    }
                    p2.Y = p3.Y;
                    
                       /* while ((p2.Y<KW.Count-1)&&((KeyWord)KW[p2.Y+1]).Name.ToLower().StartsWith(s))
                        {
                            p2.Y++;
                        }*/
                    

                }
                
            }
            if (p2.X>=0)
            {
                p.X=p2.X;
            }
            if (p2.Y>=0)
            {
                p.Y=p2.Y;
            }
            //MessageBox.Show(p.X + "  " + p.Y);
            return p;
        }
        public void UpdateContent(string s)
        {
            Point StartEnd = new Point(-1,-1);
            //this.BeginUpdate();
           // this.Items.Clear();
            this.MyItems.Clear();
            
            this.listView1.BeginUpdate();
            //int i = 0;
            
                


                
            /*while (i < KW.Count)
                {
                    if (((KeyWord)KW[i]).Name.ToLower().StartsWith(s.ToLower()))
                    {
                        //this.Items.Add(KW[i]);
                        this.MyItems.Add(KW[i]);
                    }
                    i++;

                }*/
            if (s.Trim().Length > 0)
            {
                StartEnd = FindKeyWords(s);
            }
            //MessageBox.Show(StartEnd.X.ToString() + "   " + StartEnd.Y.ToString());
            if (StartEnd.X >= 0 && StartEnd.Y>=0)
            {
               
                for (int z = StartEnd.X; z <= StartEnd.Y; z++)
                {
                    this.MyItems.Add(KW[z]);
                }
                
            
                
                //listView1_SelectedIndexChanged(null, new EventArgs());
                /*if (this.Items.Count > 0) { this.SelectedIndex = 0; } else { TT.Hide(this); 

                this.EndUpdate();*/
                
            }
            SetupListview2(MyItems.Count);
            UpdateSize();
            this.listView1.EndUpdate();

            if(this.listView1!=null)
            {
                if (this.MyItems.Count > 0) 
                {
                    
                    this.listView1.Items[0].Selected = true;
                    this.listView1.TopItem = listView1.Items[0];
                    listView1_SelectedIndexChanged(null, new EventArgs());
                } 
                else 
                { 
                    TT.Hide(this); 
                }
            }
            
    
            
            
        }
        public void UpdateContent()
        {
           // this.BeginUpdate();
            
            if (this.listView1 != null)
            {
                if (this.MyItems.Count > 0)
                {
                   
                    this.listView1.Items[0].Selected = true;
                    this.listView1.TopItem = listView1.Items[0];
                    listView1_SelectedIndexChanged(null, new EventArgs());
                }
            }
           
            //this.SelectedIndex = 0;

            //this.EndUpdate();
           
        }
        public void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if ((this.MyItems.Count <= 0)                
                ||(this.listView1.SelectedIndices.Count <= 0)
                ||(this.MyItems[listView1.SelectedIndices[0]] == null))
            {
                return;
            }
            
           
            /*if (LastItemIndex < this.listView1.SelectedIndices[0])
            {
                TTPos += ItemHeight;
                if (TTPos > this.Height - ItemHeight) TTPos = this.Height - ItemHeight;
            }
            else if (LastItemIndex > this.listView1.SelectedIndices[0])
            {
                TTPos -= ItemHeight;
                if (TTPos < 0) TTPos = 0;
            }*/
           
            KeyWord key = (MyItems[this.listView1.SelectedIndices[0]]);
            string s;
            // foreach (KeyWord m in KW)
            // {

            s = key.Name.ToLower();
            if (DokuTable[s] != null)
            {

                key.Text2 = Convert.ToString(DokuTable[s]);
            }
            // }

            TT.ToolTipTitle = (key).Text1;
            if (this.Left + (this.Width / 2) < Screen.PrimaryScreen.WorkingArea.Width / 2)
            {
                TT.Show((key).Text2, this, this.Width, listView1.Items[listView1.SelectedIndices[0]].Position.Y);


            }
            else
            {

                Label Templabel = new Label();
                Label Templabel2 = new Label();
                this.Controls.Add(Templabel);
                this.Controls.Add(Templabel2);
                Templabel.AutoSize = true;
                Templabel2.AutoSize = true;


                Templabel.Font = new System.Drawing.Font(Templabel.Font, System.Drawing.FontStyle.Bold);
                Templabel.Text = (key).Text1;

                Templabel2.Text = (key).Text2;
                if (Templabel.Width > Templabel2.Width)
                {
                    TT.Show((key).Text2, this, -(Templabel.Width) - 25, listView1.Items[listView1.SelectedIndices[0]].Position.Y);
                }
                else
                {
                    TT.Show((key).Text2, this, -(Templabel2.Width) - 25, listView1.Items[listView1.SelectedIndices[0]].Position.Y);
                }


                this.Controls.Remove(Templabel);
                this.Controls.Remove(Templabel2);
                Templabel.Dispose();
                Templabel2.Dispose();

            }


            
           
            //listView1.Items[LastItemIndex].BackColor = System.Drawing.SystemColors.Window;
            LastItemIndex = this.listView1.SelectedIndices[0];
            //base.OnSelectedIndexChanged(e);
            KeyWord TraceWord =((KeyWord)MyItems[this.listView1.SelectedIndices[0]]);
            if ( TraceWord.Type== 3)
            {
                Trace.Text = TraceWord.Name+"   "+TraceWord.Text1;
            }
            else if (TraceWord.Type == 6)
            {
                Trace.Text = TraceWord.Name+"   "+TraceWord.Text2.Replace("\n", " ");
            }
        }
        /*protected override void OnSelectedIndexChanged(EventArgs e)
        {
            
            if (LastItemIndex < this.SelectedIndex)
            {
                TTPos += this.ItemHeight;
                if (TTPos > this.Height - this.ItemHeight) TTPos = this.Height - this.ItemHeight;
            }
            else if (LastItemIndex > this.SelectedIndex)
            {
                TTPos -= this.ItemHeight;
                if (TTPos < 0 ) TTPos = 0;
            }
            if (this.SelectedItem == null) return;
           TT.ToolTipTitle=((KeyWord)(this.SelectedItem)).Text1;
            if(this.Left+(this.Width/2)<Screen.PrimaryScreen.WorkingArea.Width/2)
            {
                 TT.Show( ((KeyWord)(this.SelectedItem)).Text2, this,this.Width,TTPos );
               

            }
            else
            {

                Label Templabel = new Label();
                Label Templabel2 = new Label();
                this.Controls.Add(Templabel);
                this.Controls.Add(Templabel2);
                Templabel.AutoSize=true;
                Templabel2.AutoSize= true;
                
              
                    Templabel.Font = new System.Drawing.Font(Templabel.Font, System.Drawing.FontStyle.Bold);
                    Templabel.Text=((KeyWord)(this.SelectedItem)).Text1;
               
                    Templabel2.Text=((KeyWord)(this.SelectedItem)).Text2;
                   if(Templabel.Width>Templabel2.Width)
                   {
                       TT.Show(((KeyWord)(this.SelectedItem)).Text2, this, -(Templabel.Width) - 25, TTPos);
                   }
                   else
                   {
                       TT.Show(((KeyWord)(this.SelectedItem)).Text2, this, -(Templabel2.Width) - 25, TTPos);
                   }
                
               
                this.Controls.Remove(Templabel);
                 this.Controls.Remove(Templabel2);
                Templabel.Dispose();
                Templabel2.Dispose();
                
            }



            LastItemIndex = this.SelectedIndex;
            base.OnSelectedIndexChanged(e);
        }*/
        private KeyWord Deconstruct_Line(string l)
        {
            KeyWord kw=new KeyWord();
           
           string [] s= l.Split('@');
          
           try
           {
                kw.Name = s[0];
                //kw.Text1 = s[1];
                kw.Text2 = " ";
                for (int i = 1; i < s.Length; i++)
                {
                    if(i==s.Length-1)
                    {
                        kw.Text1 += s[i] + "";
                    }
                    else
                    {
                        kw.Text1 +=s[i] +"\n";
                    }
                }
                
                
           }
           catch
           {
           }
           
           return kw;

        }
        private void Read_KeyWords()
        {
            //this.Items.Clear();
            List<KeyWord> Doku = new List<KeyWord>();
            this.listView1.Clear();
            this.MyItems.Clear();
            this.KW.Clear();

            if (Extension==".d")
            {
                KeyWord k = new KeyWord();
                Doku.Clear();
                
                if (File.Exists(KEYWORDDATEI + Extension))
                {
                    using (StreamReader sr = new StreamReader(KEYWORDDATEI + Extension, Encoding.Default))
                    {
                        String line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            k = Deconstruct_Line(line);
                            //this.Items.Add(k);
                            Doku.Add(k);

                        }

                    }
                }
                if (File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + "\\" + PROPDATEI + Extension))
                {
                    using (StreamReader sr = new StreamReader(Path.GetDirectoryName(Application.ExecutablePath) + "\\" + PROPDATEI + Extension, Encoding.Default))
                    {
                        String line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            k = Deconstruct_Line(line) ;
                            //k.Text2=" ";
                           
                            k.Type = 7;
                            //this.Items.Add(k);
                            Properties.Add(k);

                        }

                    }
                   
                    //Properties.Sort(new Classes.KeyWordComparer());
                }


                DokuTable.Clear();
                foreach (KeyWord l in Doku)
                {

                    try
                    {
                        DokuTable.Add(l.Name.ToLower(), l.Text1);
                    }
                    catch
                    {
                    }
                }

                ReadShortFunc();
            }
            
            KeyWord k2 = new KeyWord();
            if (File.Exists(Path.GetDirectoryName(Application.ExecutablePath)+"\\"+KEYWORDDATEI + Extension))
            {
                
                using (StreamReader sr = new StreamReader(Path.GetDirectoryName(Application.ExecutablePath)+"\\"+KEYWORDDATEI + Extension, Encoding.Default))
                {
                    
                    String line;                
                    while ((line = sr.ReadLine()) != null) 
                    {
                        k2=Deconstruct_Line(line);
                       
                        this.KW.Add(k2);

                    }

                }
                SetupListview(MyItems.Count);
           }
            else
            {
                
                MessageBox.Show("Die Datei " +Path.GetDirectoryName(Application.ExecutablePath)+"\\"+ KEYWORDDATEI +Extension+ " konnte nicht gefunden werden!", "Fehler!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        public void ReadShortFunc()
        {
           



            if (File.Exists(FUNCDIR))
            {

                string line;
                int mode = 0;
                bool isloop = false;
                int startloop = -1;
                int endloop = -1;
                
                int i = 0;
                string temp = "";
                string name = "";
                List<Parameter> ParameterArray = new List<Parameter>();
                //ArrayList LoopParameterArray = new ArrayList();
                bool found = false;
                using (StreamReader sr = new StreamReader(FUNCDIR, Encoding.Default))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        isloop = false;
                        startloop = -1;
                        endloop = -1;
                        
                        name = "";
                        ParameterArray = new List<Parameter>();
                       // LoopParameterArray = new ArrayList();

                        while (i < line.Length)
                        {
                            if (mode == 0)
                            {
                                if (line[i] != '@')
                                {
                                    name += line[i];
                                }
                                else
                                {

                                    mode = 1;
                                    if (i < line.Length && line[i + 1] == '@')
                                    {
                                        isloop = true;
                                        
                                    }
                                    
                                    
                                    line = line.Remove(0, name.Length + (isloop?2:1));
                                    i = 0;
                                    break;
                                }
                            }  
                            i++;
                        }   
                        if (isloop)
                        {
                            i = 0;
                            while (i<line.Length)
                            {
                                if (line[i] == '|')
                                {
                                    if (startloop == -1)
                                    {
                                        startloop = i;
                                    }
                                    else if (endloop == -1)
                                    {
                                        endloop = i-1;
                                        
                                        line = line.Remove(startloop, 1);
                                        line = line.Remove(endloop, 1);
                                        break;
                                    }
                                }
                                i++;

                            }
                        }
                        i = 0;
                            while(i < line.Length)
                            {
                            
                                if (line[i] == '#')
                                {
                                    i++;
                                    temp = "";
                                    while (i < line.Length)
                                    {
                                        if (Char.IsDigit(line[i]))
                                        {
                                            temp += line[i];
                                        }
                                        else
                                        {
                                            break;
                                        }
                                        i++;
                                    }
                                    if (isloop)
                                    {
                                        if (i < startloop)
                                        {
                                            startloop -= temp.Length+1;
                                        }
                                        if (i < endloop)
                                        {
                                            endloop -= temp.Length+1;
                                        }
                                        ;
                                    }
                                    found = false;
                                    Parameter p1 = new Parameter();
                                    Parameter pl = new Parameter();

                                    /*if (startloop >= 0 && endloop == -1)
                                    {
                                        foreach (Parameter p in LoopParameterArray)
                                        {
                                            if (p.ParamName == temp)
                                            {
                                                found = true;
                                                pl = p;
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {*/
                                        foreach (Parameter p in ParameterArray)
                                        {
                                            if (p.ParamName == temp)
                                            {
                                                found = true;
                                                p1 = p;
                                                break;
                                            }
                                        }
                                    //}

                                    if (found == true)
                                    {
                                        /*if (startloop >= 0 && endloop == -1)
                                        {
                                            pl.ParamPos.Add(i - temp.Length - 1-startloop);
                                        }
                                        else
                                        {*/
                                            p1.ParamPos.Add(i - temp.Length - 1);
                                        //}

                                    }
                                    else
                                    {
                                        /*if (startloop >= 0 && endloop == -1)
                                        {
                                            pl.ParamName = temp;
                                            pl.ParamPos.Add(i - temp.Length - 1-startloop);
                                            LoopParameterArray.Add(pl);
                                        }
                                        else
                                        {*/
                                            p1.ParamName = temp;
                                            p1.ParamPos.Add(i - temp.Length - 1);
                                            ParameterArray.Add(p1);
                                        //}
                                    }

                                    line = line.Remove(i - (temp.Length + 1), temp.Length + 1);
                                    
                                    i = i - (temp.Length + 1);


                                }
                                
                            
                            
                            i++;
                        }
                        /*if (isloop)
                        {
                            i = 0;
                            while (i<line.Length)
                            {
                                if (isloop && line[i] == '|')
                                {
                                    if (startloop == -1)
                                    {
                                        startloop = i;
                                    }
                                    else if (endloop == -1)
                                    {
                                        endloop = i;
                                        mode = 1;
                                        i = 0;
                                        line = line.Remove(startloop, 1);
                                        line = line.Remove(endloop - 1, 1);
                                        
                                    }
                                }
                                i++;

                            }
                        }*/
                        
                        line = line.Replace("§", "\n");
                        ShortFunc sfu = new ShortFunc();
                        sfu.Short = name;
                        sfu.FuncText = line;
                        sfu.loopstart = startloop;
                        sfu.loopend = endloop;
                        

                        foreach (Parameter p in ParameterArray)
                        {
                            sfu.Params.Add(p);
                        }
                        /*if (startloop>=0)
                        {
                            foreach (Parameter p in LoopParameterArray)
                            {
                                sfu.LoopParams.Add(p);
                            }
                        }*/

                        if (!ShortFuncList.Contains(name))
                        {
                            ShortFuncList.Add(name,sfu);
                        }

                        KeyWord k = new KeyWord();
                        k.Name = "#"+name;
                        if (startloop >= 0)
                        {
                            k.Text1 = "Loop-Kurzfunktion";
                        }
                        else
                        {
                            k.Text1 = "Kurzfunktion";
                        }
                        k.Text2 = line;
                        k.Type = 6;
                        ShortFuncs.Add(k);
                        mode = 0;
                        i = 0;

                    }
                }
                //####

            }
                 
        }
        int i2;
        string s2;

        public string TransformShortFunc(Editor e)
        {
            string s = e.m_Editor.ActiveTextAreaControl.Document.TextContent;
            int oldcaretpos = e.m_Editor.ActiveTextAreaControl.Caret.Line;
            s=TransformShortFunc(s);
            e.m_Editor.ActiveTextAreaControl.Caret.Line = oldcaretpos;
            return s;
        }
        public string TransformShortFunc(string s)
        {
            //string s = e.m_Editor.ActiveTextAreaControl.Document.TextContent;
            //int oldcaretpos = e.m_Editor.ActiveTextAreaControl.Caret.Line;
            Regex r = new Regex("#");
            Match m;
            int i, l, l2, l3, n;
            string temp = "";
            string builder = "";
            string loopbuilder = "";
            List<string> ParameterArray = new List<string>();
            List<ParameterPos> PosArray = new List<ParameterPos>();
            ParameterPos ppos;
            bool isloop = false;
            n = 0;
            while ((m = r.Match(s, n)).Success)
            {

                i = m.Index + 1;
                temp = "";
                while (i < s.Length)
                {
                    if (!Char.IsWhiteSpace(s[i]))
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
                //MessageBox.Show("!" + temp + "!");
                temp = temp.ToLower();
                if (ShortFuncList.Contains(temp))
                {

                    ShortFunc sf = ((ShortFunc)ShortFuncList[temp]);
                   // if (sf.Params.Count > 0)
                   // {
                        ParameterArray.Clear();
                        temp = "";
                        while (i < s.Length)
                        {
                            if (!Char.IsWhiteSpace(s[i]) /*&& s[i] != ';' && s[i] != '='*/)
                            {
                                if (s[i] == ',')
                                {
                                    ParameterArray.Add(temp.Replace("§","\n"));
                                    temp = "";
                                }
                                else if (i == s.Length - 1)
                                {
                                    temp += s[i];
                                    ParameterArray.Add(temp.Replace("§","\n"));
                                    temp = "";
                                }
                                else
                                {
                                    temp += s[i];
                                }

                                i++;
                            }
                            else
                            {
                                ParameterArray.Add(temp);
                                i++;
                                break;
                            }

                        }
                        
                    //}
                    isloop = sf.loopstart != -1;
                   
                    
                    
                    builder = sf.FuncText;
                    
                    l = 0;
                    l2 = 0;
                    l3=0;
                    PosArray.Clear();
                    foreach(Parameter p in sf.Params)
                    {
                        foreach (int Pos in p.ParamPos)
                        {
                            try
                            {
                                ppos.pos = Pos;
                                ppos.parameter = ParameterArray[Convert.ToInt32(p.ParamName)].ToString();
                                PosArray.Add(ppos);
                            }
                            catch
                            {
                            }
                        }
                    }

                   


                    PosArray.Sort();
                    /*foreach (Parameter p in sf.Params)
                    {
                        MessageBox.Show(p.ParamName);
                    }*/
                    foreach (ParameterPos p in PosArray)
                    {

                        
                        builder = builder.Insert(p.pos + l, p.parameter);
                       

                        if (/*p.pos>=sf.loopstart+l &&*/ p.pos<= sf.loopstart)
                        {
                            //MessageBox.Show(p.pos.ToString() + " " + p.parameter+ "  "+ sf.loopstart.ToString());
                            l2 += p.parameter.Length;
                        }
                        else if (p.pos <= sf.loopend)
                        {
                            l3 += p.parameter.Length;
                        }
                       
                         l += p.parameter.Length;
                        
                        
                       
                            
                        
                    }
                    if (isloop)
                    {
                        loopbuilder = builder.Substring(sf.loopstart + l2, (sf.loopend + l2 + l3) - (sf.loopstart + l2));
                        builder = builder.Remove(sf.loopstart + l2, (sf.loopend + +l2 + l3) - (sf.loopstart + l2));
                        //MessageBox.Show(loopbuilder + " " + l2.ToString() + " " + l3.ToString());

                    }

                    i2 = m.Index;
                    s2 = "";
                    while (i2 > 0)
                    {
                        
                        if (s[i2]=='\t')
                        {
                            s2 += "\t";
                        }
                        else if (s[i2] == ' ')
                        {
                            s2 += " ";
                            
                        }
                        else if (s[i2] == '\n')
                        {
                            break;
                        }

                        i2--;
                    }
                    //MessageBox.Show(ParameterArray[ParameterArray.Count - 1].ToString());
                    temp = "";
                    if (isloop && ParameterArray.Count>1)
                    {
                        
                        try
                        {
                            for (int y = 0; y < Convert.ToInt32(ParameterArray[ParameterArray.Count - 2].ToString()); y++)
                            {
                                temp += loopbuilder.Replace("%", (y+Convert.ToInt32(ParameterArray[ParameterArray.Count - 1])).ToString());
                                
                            }
                            loopbuilder = temp;
                        }
                        catch
                        {
                        }
                        
                    }

                    if (isloop)
                    {
                        builder = builder.Insert(sf.loopstart + l2,loopbuilder);
                    }
                    builder = builder.Replace("\n","\n"+s2);
                    n = m.Index + builder.Length;
                    s = s.Remove(m.Index, i - m.Index);
                    s = s.Insert(m.Index, builder);


                }
                else
                {
                    n = m.Index + 1;
                }
                if (n > s.Length) break;

            }
            //e.m_Editor.ActiveTextAreaControl.Caret.Line = oldcaretpos;
            return s;
            //e.m_Editor.Text = s;
            /*e.m_Editor.ActiveTextAreaControl.Document.Replace(0, e.m_Editor.ActiveTextAreaControl.Document.TextContent.Length, s);
            e.m_Editor.ActiveTextAreaControl.Caret.Line = oldcaretpos;
            e.Refresh();*/
            
        }

        public void AHide()
        {
            this.Visible = false;
            try { TT.Hide(this); }
            catch { }
            M_AutoCompleteDa.Active = false;
            LastItemIndex = 0;
           
          
           
        }
        public void AShow(Editor e)
        {
            currentEditor = e;
            UpdatePos(e);
            CaretPos=e.m_Editor.ActiveTextAreaControl.Caret.Offset;
            this.Visible = true;
            M_AutoCompleteDa.Active = true;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);

        }
        public void SaveKW()
        {
            
            KWSave kwsave = new KWSave();
            kwsave.KW=new KeyWord[KW.Count];
            KW.CopyTo(kwsave.KW);
           

            
                FileStream myStream;
                myStream = new FileStream(ScriptsPath + Global.KW, FileMode.Create);
                BinaryFormatter binFormatter = new BinaryFormatter();
                
                binFormatter.Serialize(myStream, kwsave);
                
                myStream.Close();
                myStream.Dispose();
           
            return;
        }

    }
    [Serializable()]
    public class KeyWord:IComparable
    {
        public string Name;
        public string Text1;
        public string Text2;
        public int Type=0;
        public KeyWord()
        {

        }
        public KeyWord(string n, int typ, string t1, string t2)
        {
            Name = n;
            Type = typ;
            Text1 = t1;
            Text2 = t2;
        }
        public override string ToString()
        {
            return Name;
        }
        int IComparable.CompareTo(object obj)
        {
            KeyWord p = obj as KeyWord;
            return String.Compare(Name, p.Name, true);
        }
    }
    public class ListView2 : ListView
    {
        public AutoComplete auto;
        public ListView2(AutoComplete a)
        {
            auto = a;
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.Height = Sizes.Height;
            this.Width=Sizes.Width;
            
        }
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            
            base.OnMouseDoubleClick(e);
            auto.MouseInsert();
            
        }
        protected override void  OnSelectedIndexChanged(EventArgs e)
        {
 	         base.OnSelectedIndexChanged(e);
             auto.SelectedChanged();
        }
        
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Escape)
            {
                auto.MouseRemove();

            }
            else if (e.KeyCode == Keys.Enter)
            {
                auto.MouseInsert();
            }
        }
        
    }
    static class Sizes
    {
        public const int Height = 300;
        public const int Width = 250;
    }
    [Serializable()]
    public class KWSave
    {
        public KeyWord[] KW;
        
        public KWSave()
        {
        }
    }
   /* public class KeyWordComparer : IComparer
    {

        public KeyWordComparer()
        {

        }
        int IComparer.Compare(object x, object y)
        {
           


            return String.Compare(((KeyWord)x).Name, ((KeyWord)y).Name);
        }
    }*/
   
}
