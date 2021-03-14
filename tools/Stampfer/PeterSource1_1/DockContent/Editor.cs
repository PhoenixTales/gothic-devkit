/**************************************************************************************
    Stampfer - Gothic Script Editor
    Copyright (C) 2008, 2009 Jpmon1, Alexander "Sumpfkrautjunkie" Ruppert

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
using WeifenLuo.WinFormsUI.Docking;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using PeterInterface;
using System.Xml;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Collections;
using System.Text;
using System.Runtime.InteropServices;
namespace Peter
{
    public delegate void LoadFileDelegate(string text);
    public delegate void CloseDelegate();
    
   
    public class Editor : DockContent, IPeterPluginTab, IHtmlInterface
    {
        public TextEditorControl m_Editor;
        public IPeterPluginHost m_Host;
        public bool m_Changed;
        public int m_FindPos;
        public MainForm m_MainForm;
        public FileSystemWatcher m_FSW;
        public LoadFileDelegate m_LoadFile;
        public CloseDelegate m_CloseDel;
       // public cHtmlToolTip m_ToolTip;
        private bool DialogMode=false;
        private string m_Project;
        public DialogCreator DiaC;
        public bool AutoCompleteAuto;
        public bool ignoreclose = false;

        const int COOLDOWN = 2;
        Timer mytimer = new Timer();
        int cooldowntimer = COOLDOWN;

        #region -= Constructor =-

        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int vKey);
        [DllImport("user32.dll")]
        public static extern int SendMessage(
              int hWnd,      // handle to destination window
              uint Msg,       // message
              long wParam,  // first message parameter
              long lParam   // second message parameter
              );
        public Editor(string tabTitle, MainForm main)
        {
            Init(tabTitle, main);
            
        }
        public Editor(string tabTitle, MainForm main, DialogCreator d)
        {
            Init(tabTitle, main);
            DiaC = d;
            DialogMode = true;
        }
        private void Init(string tabTitle, MainForm main)
        {
            // Set up the Parent Tab...
            this.TabText = tabTitle;
            this.m_FindPos = -1;
            this.m_MainForm = main;
           // this.m_ToolTip = new cHtmlToolTip(this);
            this.m_Project = "";
            
            M_AutoCompleteDa.Active = false;

            // Set up the Editor...
            this.m_Editor = new TextEditorControl();
            this.m_Editor.Dock = System.Windows.Forms.DockStyle.Fill;

            // Add the Editor...
            this.Controls.Add(this.m_Editor);
            this.m_Changed = false;

            // Delegates...
            this.m_LoadFile = new LoadFileDelegate(this.DelReLoad);
            this.m_CloseDel = new CloseDelegate(this.DelClose);

            // Drag N Drop...
            this.m_Editor.ActiveTextAreaControl.TextArea.AllowDrop = true;
            this.m_Editor.ActiveTextAreaControl.TextArea.DragEnter += new System.Windows.Forms.DragEventHandler(TextArea_DragEnter);
            this.m_Editor.ActiveTextAreaControl.TextArea.DragDrop += new System.Windows.Forms.DragEventHandler(TextArea_DragDrop);
            this.m_Editor.ActiveTextAreaControl.TextArea.MouseDown += new MouseEventHandler(TextArea_MouseDown);
            this.m_Editor.ActiveTextAreaControl.Caret.PositionChanged += new EventHandler(Caret_Change);
            this.m_Editor.ActiveTextAreaControl.Caret.CaretModeChanged += new EventHandler(Caret_CaretModeChanged);
            this.m_Editor.ActiveTextAreaControl.TextArea.LostFocus += new EventHandler(TextArea_LostFocus);
            
            this.m_Editor.ActiveTextAreaControl.TextArea.KeyDown += new System.Windows.Forms.KeyEventHandler(TextArea_KeyDown);
            this.m_Editor.ActiveTextAreaControl.TextArea.KeyPress += new KeyPressEventHandler(TextArea_KeyPress);

            this.m_Editor.ActiveTextAreaControl.TextArea.DoProcessDialogKey += new DialogKeyProcessor(TextArea_DoProcessDialogKey);

            this.m_Editor.Document.DocumentChanged += new DocumentEventHandler(Document_DocumentChanged);
            this.m_Editor.Document.UndoStack.ActionRedone += new EventHandler(UndoStack_ActionRedone);
            this.m_Editor.Document.UndoStack.ActionUndone += new EventHandler(UndoStack_ActionRedone);
            
            this.m_Editor.ActiveTextAreaControl.Document.DocumentChanged+=new DocumentEventHandler(Document_DocumentChanged2);

            mytimer.Enabled = true;
            mytimer.Interval = 500;
            mytimer.Tick += new EventHandler(mytimer_Tick);
        }





        void Document_DocumentChanged2(object sender, DocumentEventArgs e)
        {
            cooldowntimer = COOLDOWN;
        }

        void mytimer_Tick(object sender, EventArgs e)
        {
            if (cooldowntimer>0)
            {
                cooldowntimer--;
                if (cooldowntimer <= 0)
                {
                    UpdateFolding();                    
                }
            }
            
        }
        
        protected override void OnEnter(EventArgs e)
        {
            if (!m_MainForm.TabCloseBlock)
            {
                if (GetAsyncKeyState(0x04) < 0)
                {
                    m_MainForm.TabCloseBlock = true;
                    Close();
                }
            }
            else
            {
                m_MainForm.TabCloseBlock = false;
            }
            
            base.OnEnter(e);
        }
        protected override void OnLostFocus(EventArgs e)
        {
            this.m_MainForm.m_AutoComplete.AHide();
            base.OnLostFocus(e);
        }
        

       
        
        public void InsertACText()
        {
           
            Point p = GetBaseWord();
            /*if (AutoCompleteAuto)
            {

            }*/
            if (this.m_MainForm.m_AutoComplete.MyItems.Count == 0) return;
            if (this.m_MainForm.m_AutoComplete.MyItems[this.m_MainForm.m_AutoComplete.listView1.SelectedIndices[0]] == null) return;
            string s = this.m_MainForm.m_AutoComplete.MyItems[this.m_MainForm.m_AutoComplete.listView1.SelectedIndices[0]].ToString();
            /*if (this.m_Editor.ActiveTextAreaControl.Caret.Offset > p.X)
            {
                s = s.Remove(0, this.m_Editor.ActiveTextAreaControl.Caret.Offset - p.X);
            }
           // s = s.Remove(0, 2);
            //this.m_Editor.ActiveTextAreaControl.Document.Remove(p.X,p.Y);
            this.m_Editor.ActiveTextAreaControl.Document.Insert(this.m_Editor.ActiveTextAreaControl.Caret.Offset, s);
            if ((this.m_Editor.ActiveTextAreaControl.Caret.Offset + s.Length) <= this.m_Editor.ActiveTextAreaControl.Document.TextContent.Length)
            {
                this.m_Editor.ActiveTextAreaControl.Caret.Position = this.m_Editor.Document.OffsetToPosition(this.m_Editor.ActiveTextAreaControl.Caret.Offset + s.Length);
            }
            else
            {
                this.m_Editor.ActiveTextAreaControl.Caret.Position = this.m_Editor.Document.OffsetToPosition(this.m_Editor.ActiveTextAreaControl.Document.TextContent.Length);
            }*/
            int saveoffset = this.m_Editor.ActiveTextAreaControl.Caret.Offset;
            this.m_Editor.ActiveTextAreaControl.Document.Remove(p.X, saveoffset-p.X);
            this.m_Editor.ActiveTextAreaControl.Caret.Position = this.m_Editor.Document.OffsetToPosition(this.m_Editor.ActiveTextAreaControl.Caret.Offset - (this.m_Editor.ActiveTextAreaControl.Caret.Offset - p.X));
            this.m_Editor.ActiveTextAreaControl.Document.Insert(this.m_Editor.ActiveTextAreaControl.Caret.Offset, s);
            if ((this.m_Editor.ActiveTextAreaControl.Caret.Offset + s.Length-(this.m_Editor.ActiveTextAreaControl.Caret.Offset - p.X)) <= this.m_Editor.ActiveTextAreaControl.Document.TextContent.Length)
            {
                this.m_Editor.ActiveTextAreaControl.Caret.Position = this.m_Editor.Document.OffsetToPosition(this.m_Editor.ActiveTextAreaControl.Caret.Offset + s.Length);
            }
            else
            {
                this.m_Editor.ActiveTextAreaControl.Caret.Position = this.m_Editor.Document.OffsetToPosition(this.m_Editor.ActiveTextAreaControl.Document.TextContent.Length);
            }
        }
        private Point GetBaseWord()
        {
            int Endpos = this.m_Editor.ActiveTextAreaControl.TextArea.Caret.Offset;
           
            int Startpos = Endpos;

           
            while (Startpos >= 1)
            {
                /*if (this.m_MainForm.m_AutoComplete.mode != 1)
                {
                    if (!Char.IsLetterOrDigit(this.m_Editor.ActiveTextAreaControl.Document.TextContent[Startpos - 1]) && this.m_Editor.ActiveTextAreaControl.Document.TextContent[Startpos - 1] != '_')
                        break;
                }
                else
                {*/
                if (!Char.IsLetterOrDigit(this.m_Editor.ActiveTextAreaControl.Document.TextContent[Startpos - 1]) && this.m_Editor.ActiveTextAreaControl.Document.TextContent[Startpos - 1] != '_' && this.m_Editor.ActiveTextAreaControl.Document.TextContent[Startpos - 1] != '.' && this.m_Editor.ActiveTextAreaControl.Document.TextContent[Startpos - 1] != '#')
                        break;
                //};


                Startpos--;
                if (/*this.m_MainForm.m_AutoComplete.mode == 1 &&*/ this.m_Editor.ActiveTextAreaControl.Document.TextContent[Startpos] == '.'||this.m_Editor.ActiveTextAreaControl.Document.TextContent[Startpos] == '#')
                {
                    break;
                }
            }
            Point p = new Point(Startpos, Endpos - Startpos);
            return p;
        }
        bool firstupdate = false;
        private bool CreateACUpdtate()
        {

            Point p =GetBaseWord();
            
            string s = this.m_Editor.ActiveTextAreaControl.Document.TextContent.Substring(p.X, p.Y);
            
            
           /* if (/*(AutoCompleteAuto) && (s.Trim().Length == 0))
            {

                if (M_AutoCompleteDa.Active) RemoveAutoComplete();
               return false;
            }*/
           
            //MessageBox.Show(s);
            
            if (firstupdate == true && AutoCompleteAuto && (s.Trim()==""))
            {
                
                RemoveAutoComplete();
            }
            this.m_MainForm.m_AutoComplete.UpdateContent(s);
            if (firstupdate == true && AutoCompleteAuto && this.m_MainForm.m_AutoComplete.MyItems.Count == 0)
            {
                
                RemoveAutoComplete();
            }
            firstupdate = true;
            return true;   
        }
        //const string brstr = "{\n" + "\t" + "\n}";
        bool TextArea_DoProcessDialogKey(Keys keyData)
        {
            
            if (M_AutoCompleteDa.Active)
            {
                /*if ((this.m_MainForm.m_AutoComplete.mode==1)&&(this.m_Editor.ActiveTextAreaControl.Caret.Offset>0)&&((keyData == Keys.Back) || (keyData == Keys.Delete)) && (this.m_Editor.ActiveTextAreaControl.Document.TextContent[this.m_Editor.ActiveTextAreaControl.Caret.Offset-1]=='.'))
                {
                    this.m_MainForm.m_AutoComplete.mode = 0;
                }*/
               /* if ((keyData == Keys.Escape) || ((keyData ==(Keys.Alt))))// &&(keyData == (Keys.Control | Keys.ControlKey)))))
                {
                    RemoveAutoComplete();

                    this.m_MainForm.m_AutoComplete.mode = 0;
                    return true;
                }*/

                if ((keyData == Keys.Space)||(keyData == Keys.Tab) || (keyData == Keys.Oemcomma) || (keyData == Keys.OemMinus) || (keyData == (Keys.Shift | Keys.D0)) ||
                    (keyData == (Keys.Control|Keys.Alt | Keys.D8))||(keyData == (Keys.Shift | Keys.D8)) || (keyData == Keys.OemPeriod) || (keyData == Keys.Oemplus) || (keyData == (Keys.Shift | Keys.Oemcomma))||
                    (keyData == (Keys.Control | Keys.Alt | Keys.D9)) || (keyData == (Keys.Shift | Keys.D9)))
                {
                    if (!((keyData == Keys.Space) && (AutoCompleteAuto)))
                    {
                        InsertACText();
                    } 
                    
                    if ((keyData == Keys.OemPeriod))
                    {
                        //this.m_MainForm.m_AutoComplete.PropertyMode();
                        
                    }
                    else
                    {



                        RemoveAutoComplete();
                        
                    }
                    

                }
                else if (keyData == (Keys.Control | Keys.Space))
                {

                    

                    RemoveAutoComplete();
                    return true;

                }
                else if (keyData == Keys.Enter)
                {
                    
                        InsertACText();
                   
                        RemoveAutoComplete();
                        return true;
                    
                }

                if (keyData == Keys.Up)
                {
                    if (this.m_MainForm.m_AutoComplete.MyItems.Count == 0) return true;
                    if (this.m_MainForm.m_AutoComplete.listView1.SelectedIndices[0] > 0)
                    {

                        this.m_MainForm.m_AutoComplete.listView1.Items[this.m_MainForm.m_AutoComplete.listView1.SelectedIndices[0] - 1].Selected = true;
                        this.m_MainForm.m_AutoComplete.listView1.EnsureVisible(this.m_MainForm.m_AutoComplete.listView1.SelectedIndices[0]);//.TopItem = this.m_MainForm.m_AutoComplete.listView1.Items[this.m_MainForm.m_AutoComplete.listView1.SelectedIndices[0]];
                        this.m_MainForm.m_AutoComplete.listView1_SelectedIndexChanged(null, new EventArgs());
                    }
                    return true;
                }
                else if (keyData == Keys.Down)
                {
                    if (this.m_MainForm.m_AutoComplete.MyItems.Count == 0) return true;
                    if (this.m_MainForm.m_AutoComplete.listView1.SelectedIndices[0] < this.m_MainForm.m_AutoComplete.MyItems.Count - 1)
                    {
                        //int tempsel=this.m_MainForm.m_AutoComplete.listView1.SelectedIndices[0] +1;
                        this.m_MainForm.m_AutoComplete.listView1.Items[this.m_MainForm.m_AutoComplete.listView1.SelectedIndices[0] + 1].Selected = true;
                        this.m_MainForm.m_AutoComplete.listView1.EnsureVisible(this.m_MainForm.m_AutoComplete.listView1.SelectedIndices[0]);//.TopItem = this.m_MainForm.m_AutoComplete.listView1.Items[tempsel];

                        this.m_MainForm.m_AutoComplete.listView1_SelectedIndexChanged(null, new EventArgs());
                    }
                    return true;

                }
                else if (keyData == (Keys.Left))
                {
                    RemoveAutoComplete();
                    return true;
                }
                else if (keyData == (Keys.Right))
                {
                    RemoveAutoComplete();
                    return true;
                }

                if (keyData == (Keys.PageUp))
                {
                    bool scrolled = false;
                    if (this.m_MainForm.m_AutoComplete.MyItems.Count == 0) return true;
                    for (int kl = this.m_MainForm.m_AutoComplete.listView1.SelectedIndices[0]; kl > 0; kl--)
                    {
                        if (this.m_MainForm.m_AutoComplete.listView1.Items[kl].Position.Y < -this.m_MainForm.m_AutoComplete.listView1.Height)
                        {
                            this.m_MainForm.m_AutoComplete.listView1.TopItem = this.m_MainForm.m_AutoComplete.listView1.Items[kl];
                            this.m_MainForm.m_AutoComplete.listView1.Items[kl].Selected = true;
                            scrolled = true;
                            break;
                        }
                    }
                    if (!scrolled)
                    {
                        this.m_MainForm.m_AutoComplete.listView1.TopItem = this.m_MainForm.m_AutoComplete.listView1.Items[0];
                        this.m_MainForm.m_AutoComplete.listView1.Items[0].Selected = true;
                    }
                    return true;
                }
                else if (keyData == (Keys.PageDown))
                {
                    bool scrolled = false;
                    if (this.m_MainForm.m_AutoComplete.MyItems.Count == 0) return true;
                    for (int kl = this.m_MainForm.m_AutoComplete.listView1.SelectedIndices[0]; kl < this.m_MainForm.m_AutoComplete.listView1.VirtualListSize; kl++)
                    {
                        if (this.m_MainForm.m_AutoComplete.listView1.Items[kl].Position.Y > this.m_MainForm.m_AutoComplete.listView1.Height)
                        {
                            this.m_MainForm.m_AutoComplete.listView1.TopItem = this.m_MainForm.m_AutoComplete.listView1.Items[kl];
                            this.m_MainForm.m_AutoComplete.listView1.Items[kl - 1].Selected = true;
                            scrolled = true;
                            break;
                        }
                    }
                    if (!scrolled)
                    {
                        this.m_MainForm.m_AutoComplete.listView1.TopItem = this.m_MainForm.m_AutoComplete.listView1.Items[this.m_MainForm.m_AutoComplete.listView1.VirtualListSize - 1];
                        this.m_MainForm.m_AutoComplete.listView1.Items[this.m_MainForm.m_AutoComplete.listView1.VirtualListSize - 1].Selected = true;
                    }


                    return true;
                }
                


               /* if (keyData != Keys.Alt && keyData == Keys.ShiftKey)
                {
                    CreateACUpdtate();
                }*/
                /*if (AutoCompleteAuto)
                {
                    CreateACUpdtate();
                }*/
                if (M_AutoCompleteDa.Active && (keyData == (Keys.Escape)))
                {
                    RemoveAutoComplete();
                }

            }
            else  {
              
                if (keyData == (Keys.Control | Keys.Space))
                {
                    //this.m_MainForm.m_AutoComplete.listView1.SmallImageList = m_MainForm.ImgList;
                    ShowAutoComplete();
                    CreateACUpdtate();
                    return true;
                }
                if (keyData == (Keys.Shift | Keys.Escape))
                {
                    Close();
                    return true;
                }
                
                
            }
            /*if (M_AutoCompleteDa.Active == false && AutoCompleteAuto)
            {
                if ((keyData != (Keys.Control | Keys.ControlKey) && (keyData != (Keys.Shift | Keys.ShiftKey)) && (keyData != (Keys.Alt)) && (keyData != (Keys.LWin)) && (keyData != (Keys.RWin))))
                {
                    ShowAutoComplete();
                    CreateACUpdtate();
                 }
            }*/
            if (M_AutoCompleteDa.Active == false && AutoCompleteAuto)
            {
                if (keyData==Keys.Delete||keyData==Keys.Back)
                {
                    ShowAutoComplete();
                    CreateACUpdtate();
                }
            }
            if ((keyData == (Keys.Control | Keys.Alt | Keys.D7)))
            {
                if (m_MainForm.m_AutoBrackets)
                {
                    string s = GetWhiteSpace(m_Editor.ActiveTextAreaControl.Caret.Offset, false);                  
                    int x = m_Editor.ActiveTextAreaControl.Caret.Offset;

                    string s2;

                    while (x > 1)
                    {
                        if (m_Editor.ActiveTextAreaControl.Document.TextContent[x - 1] == '\n')
                        {
                            break;
                        }
                        else if (!Char.IsWhiteSpace(m_Editor.ActiveTextAreaControl.Document.TextContent[x - 1]))
                        {
                            return false;
                        }
                        x--;
                    }

                    if (IsAnIf())
                    {
                        m_Editor.ActiveTextAreaControl.Caret.Position = new Point(0, m_Editor.ActiveTextAreaControl.Caret.Position.Y);
                        m_Editor.ActiveTextAreaControl.TextArea.Document.Insert(m_Editor.ActiveTextAreaControl.TextArea.Caret.Offset, s);
                        m_Editor.ActiveTextAreaControl.Caret.Position = this.m_Editor.Document.OffsetToPosition(m_Editor.ActiveTextAreaControl.Caret.Offset + s.Length);
                        s2 = "{\r\n" + s + "\t\r\n" + s + "}";
                        m_Editor.ActiveTextAreaControl.TextArea.Document.Insert(m_Editor.ActiveTextAreaControl.TextArea.Caret.Offset, s2);
                        m_Editor.ActiveTextAreaControl.Caret.Position = this.m_Editor.Document.OffsetToPosition(m_Editor.ActiveTextAreaControl.Caret.Offset + s2.Length);
                        
                        
                    }
                    else
                    {

                        m_Editor.ActiveTextAreaControl.Caret.Position = new Point(0, m_Editor.ActiveTextAreaControl.Caret.Position.Y);
                        m_Editor.ActiveTextAreaControl.TextArea.Document.Insert(m_Editor.ActiveTextAreaControl.TextArea.Caret.Offset, s);
                        m_Editor.ActiveTextAreaControl.Caret.Position = this.m_Editor.Document.OffsetToPosition(m_Editor.ActiveTextAreaControl.Caret.Offset + s.Length);
                        s2 = "{\r\n" + s + "\t\r\n" + s + "};";
                        m_Editor.ActiveTextAreaControl.TextArea.Document.Insert(m_Editor.ActiveTextAreaControl.TextArea.Caret.Offset, s2);
                        m_Editor.ActiveTextAreaControl.Caret.Position = this.m_Editor.Document.OffsetToPosition(m_Editor.ActiveTextAreaControl.Caret.Offset + s2.Length-s.Length-3);
                        
                        
                        

                      
                    }
                    m_Editor.Refresh();
                   /* m_Editor.ActiveTextAreaControl.TextArea.Document.Insert(m_Editor.ActiveTextAreaControl.TextArea.Caret.Offset, brstr);
                    m_Editor.ActiveTextAreaControl.Caret.Position = this.m_Editor.Document.OffsetToPosition(m_Editor.ActiveTextAreaControl.Caret.Offset + brstr.Length);
                    m_Editor.Refresh();*/
                    return true;
                }
            }


            if ((keyData == (Keys.Control | Keys.Alt | Keys.D0)))
            {
                int i=m_Editor.ActiveTextAreaControl.Caret.Offset;
                int closedbraces=0;
                int x = m_Editor.ActiveTextAreaControl.Caret.Position.X;
                
                

                
                while (i > 0)
                {
                    if (m_Editor.ActiveTextAreaControl.Document.TextContent[i - 1] == '}')
                    {
                        closedbraces++;
                    }
                    else if (m_Editor.ActiveTextAreaControl.Document.TextContent[i - 1] == '{')
                    {
                        
                        if (closedbraces<=0)
                        {
                            i--;
                            break;
                        }
                        closedbraces--;
                    }
                    i--;
                }

                if (i == 0)
                    return false;


                string s = GetWhiteSpace(i,true);
                string s3="";
                for (int z = 0; z < m_Editor.TabIndent; z++)
                {
                    s3 += " ";
                }
                s = s.Replace("\t", s3);

                s = s.Replace(s3, "\t");
                if (s.Trim().Length > 0)
                {
                    return false;
                }
                else
                {
                    string s2 = GetWhiteSpace(m_Editor.ActiveTextAreaControl.TextArea.Caret.Offset, true);
                    if (s2.Trim().Length >0)
                    {
                        return false;
                    }
                    else 
                    {
                        m_Editor.ActiveTextAreaControl.TextArea.Document.Remove(m_Editor.ActiveTextAreaControl.TextArea.Caret.Offset - s2.Length, s2.Length);
                        m_Editor.ActiveTextAreaControl.Caret.Position = new Point(0, m_Editor.ActiveTextAreaControl.Caret.Position.Y);
                        m_Editor.ActiveTextAreaControl.TextArea.Document.Insert(m_Editor.ActiveTextAreaControl.TextArea.Caret.Offset,s + "}");

                        //m_Editor.ActiveTextAreaControl.Document.LineSegmentCollection[m_Editor.ActiveTextAreaControl.Caret.Line].Length;

                        m_Editor.ActiveTextAreaControl.Caret.Position = this.m_Editor.Document.OffsetToPosition(m_Editor.ActiveTextAreaControl.Caret.Offset+s.Length+1);
                        //m_Editor.ActiveTextAreaControl.Caret.Position = this.m_Editor.Document.OffsetToPosition(m_Editor.ActiveTextAreaControl.Caret.Offset);
                        m_Editor.Refresh();

                       
                    }

                    
                }

                
                return true;
            }
           return false;
        }
        public string GetWhiteSpace(int i, bool mode)
        {
            //int i = m_Editor.ActiveTextAreaControl.Caret.Offset;
            string s = "";
            i -= 1;
            while (i > 0)
            {
                if (m_Editor.ActiveTextAreaControl.Document.TextContent[i/* - 1*/] == '\n')
                {
                    break;
                }               
                else if (!mode&&!Char.IsWhiteSpace(m_Editor.ActiveTextAreaControl.Document.TextContent[i]))
                {
                    s += " ";
                   
                }
                else
                {
                    s += m_Editor.ActiveTextAreaControl.Document.TextContent[i];
                }


                i--;
            }
            return s;
        }
        Regex ifreg =new Regex(@"\sif(\s|\()",RegexOptions.IgnoreCase);
        public bool IsAnIf()
        {
            int k = m_Editor.ActiveTextAreaControl.Caret.Offset;
            int l = m_Editor.ActiveTextAreaControl.Caret.Offset;
            
            while (l > 0)
            {
                if (m_Editor.ActiveTextAreaControl.Document.TextContent[l-1] == '{' || m_Editor.ActiveTextAreaControl.Document.TextContent[l-1] == '}')
                {
                    break;
                }
                l--;
            }
            Match m= ifreg.Match(m_Editor.ActiveTextAreaControl.Document.TextContent,l,k-l);
            if (m.Success)
            {
                return true;
            }

            return false;
        }


        public void RemoveAutoComplete()
        {
                this.Controls.Remove(this.m_MainForm.m_AutoComplete);
                        this.m_MainForm.m_AutoComplete.AHide();
        }
        public void ShowAutoComplete()
        {
            firstupdate = false;
            if (M_AutoCompleteDa.Active)
            {
                if (DialogMode == false)
                {
                    if (!this.Controls.Contains(this.m_MainForm.m_AutoComplete))
                    {
                        this.Controls.Add(this.m_MainForm.m_AutoComplete);
                    }

                }
                else
                {
                    if (!this.DiaC.Controls.Contains(this.m_MainForm.m_AutoComplete))
                    {
                        this.DiaC.Controls.Add(this.m_MainForm.m_AutoComplete);
                    }

                }
                if (!string.IsNullOrEmpty(this.FileName))
                {

                    string ext = Path.GetExtension(this.m_Editor.FileName).ToLower();
                    if (ext != this.m_MainForm.m_AutoComplete.Extension.ToLower())
                    {
                        this.m_MainForm.m_AutoComplete.NewKeyWordFile(ext);
                    }
                }
                this.m_MainForm.m_AutoComplete.AShow(this);
            }
            else
            {

                if (!string.IsNullOrEmpty(this.FileName))
                {

                    string ext = Path.GetExtension(this.m_Editor.FileName).ToLower();
                    if (ext != this.m_MainForm.m_AutoComplete.Extension.ToLower())
                    {
                        this.m_MainForm.m_AutoComplete.NewKeyWordFile(ext);
                    }
                }
                if (DialogMode == false)
                {
                    this.Controls.Add(this.m_MainForm.m_AutoComplete);
                }
                else
                {
                    this.DiaC.Controls.Add(this.m_MainForm.m_AutoComplete);
                }

                this.m_MainForm.m_AutoComplete.AShow(this);
            }

            //MessageBox.Show(m_MainForm.ImgList.ToString());
            //CreateACUpdtate();

            this.m_MainForm.m_AutoComplete.BringToFront();
           
        }
        void TextArea_KeyPress(object sender, KeyPressEventArgs e)
        {

           
            
               // MessageBox.Show("b", "Stampfer",
               //     MessageBoxButtons.OK, MessageBoxIcon.Question);
            
           
            
        }

        #endregion

        #region -= Key Press =-




       
        void TextArea_KeyDown(object sender, KeyEventArgs e)
        {

            
            if (M_AutoCompleteDa.Active == false && AutoCompleteAuto)
            {
               
                /*if (e.Shift)
                {
                    if ((e.KeyData == (Keys.Shift | Keys.A))
                    || (e.KeyData == (Keys.Shift | Keys.B))
                    || (e.KeyData == (Keys.Shift | Keys.C))
                    || (e.KeyData == (Keys.Shift | Keys.D))
                    || (e.KeyData == (Keys.Shift | Keys.E))
                    || (e.KeyData == (Keys.Shift | Keys.F))
                    || (e.KeyData == (Keys.Shift | Keys.G))
                    || (e.KeyData == (Keys.Shift | Keys.H))
                    || (e.KeyData == (Keys.Shift | Keys.I))
                    || (e.KeyData == (Keys.Shift | Keys.J))
                    || (e.KeyData == (Keys.Shift | Keys.K))
                    || (e.KeyData == (Keys.Shift | Keys.L))
                    || (e.KeyData == (Keys.Shift | Keys.M))
                    || (e.KeyData == (Keys.Shift | Keys.N))
                    || (e.KeyData == (Keys.Shift | Keys.O))
                    || (e.KeyData == (Keys.Shift | Keys.P))
                    || (e.KeyData == (Keys.Shift | Keys.Q))
                    || (e.KeyData == (Keys.Shift | Keys.R))
                    || (e.KeyData == (Keys.Shift | Keys.S))
                    || (e.KeyData == (Keys.Shift | Keys.T))
                    || (e.KeyData == (Keys.Shift | Keys.U))
                    || (e.KeyData == (Keys.Shift | Keys.V))
                    || (e.KeyData == (Keys.Shift | Keys.W))
                    || (e.KeyData == (Keys.Shift | Keys.X))
                    || (e.KeyData == (Keys.Shift | Keys.Y))
                    || (e.KeyData == (Keys.Shift | Keys.Z))
                    || (e.KeyData == (Keys.Shift | Keys.OemMinus)))
                    {
                    }
                    else
                    {
                        return;
                    }
                }*/
                
                if (!((e.KeyCode == Keys.A) || (e.KeyCode == Keys.B) || (e.KeyCode == Keys.C) || (e.KeyCode == Keys.D) || (e.KeyCode == Keys.E) ||
                (e.KeyCode == Keys.F) || (e.KeyCode == Keys.G) || (e.KeyCode == Keys.H) || (e.KeyCode == Keys.I) || (e.KeyCode == Keys.J) ||
                (e.KeyCode == Keys.K) || (e.KeyCode == Keys.L) || (e.KeyCode == Keys.M) || (e.KeyCode == Keys.N) || (e.KeyCode == Keys.O) ||
                (e.KeyCode == Keys.P) || (e.KeyCode == Keys.Q) || (e.KeyCode == Keys.R) || (e.KeyCode == Keys.S) || (e.KeyCode == Keys.T)||
                (e.KeyCode == Keys.U) || (e.KeyCode == Keys.V) || (e.KeyCode == Keys.W) || (e.KeyCode == Keys.X) || (e.KeyCode == Keys.Y) || (e.KeyCode == Keys.Z)))
                {
                    return;
                }
           
                if ((!e.Control && (e.KeyCode != (Keys.Space)) && (!e.Alt) && (e.KeyCode != (Keys.LWin)) && (e.KeyCode != (Keys.RWin))))
                {
                    ShowAutoComplete();
                    CreateACUpdtate();
                }
            }
            
               
            
           
            
        }

        #endregion

        #region -= Caret =-

        void Caret_CaretModeChanged(object sender, EventArgs e)
        {
            
            this.UpdateCaretPos();

        }

        void Caret_Change(object sender, EventArgs e)
        {
            if ((M_AutoCompleteDa.Active)/*&&(!AutoCompleteAuto)*/)
            {
                CreateACUpdtate();
            } 
            this.UpdateCaretPos();
            
        }

        public void UpdateCaretPos()
        {
          
            this.m_MainForm.UpdateCaretPos(this.m_Editor.ActiveTextAreaControl.Caret.Offset, this.m_Editor.ActiveTextAreaControl.Caret.Line + 1,
                this.m_Editor.ActiveTextAreaControl.Caret.Column + 1, this.m_Editor.ActiveTextAreaControl.Caret.CaretMode.ToString());
        }

        #endregion

        #region -= Mouse Down =-

        void TextArea_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.m_MainForm.m_AutoComplete != null)
            {
                RemoveAutoComplete();
            }
           // if (this.m_ToolTip.Visible) this.m_ToolTip.Hide();
            if (e.Button == MouseButtons.Left )
            {
                if (Control.ModifierKeys == Keys.Control || Control.ModifierKeys == Keys.Alt )
                {
                    int pos = this.m_Editor.ActiveTextAreaControl.Caret.Offset;
                    int before = pos;
                    int after = pos;
                   // try
                   // {
                    if (after <= 0)
                        return;
                    if (before >= this.m_Editor.Document.TextContent.Length - 1)
                        return;
                        while (CheckStop(this.m_Editor.Document.TextContent[before]))
                        {
                            
                            before--;
                            if (before <= 0)
                                return;
                        }
                        before++;
                        while (CheckStop(this.m_Editor.Document.TextContent[after]))
                        {
                            after++;
                            if (after >= this.m_Editor.Document.TextContent.Length-1)
                                return;
                        }
                   // }
                   // catch
                   // {
                  //  }

                    if (after > before)
                    {
                        if (Control.ModifierKeys == Keys.Control)
                        {
                            this.m_Editor.ActiveTextAreaControl.SelectionManager.SetSelection(
                                this.m_Editor.Document.OffsetToPosition(before), this.m_Editor.Document.OffsetToPosition(after));
                        }
                        else if (Control.ModifierKeys == Keys.Alt )
                        {
                            #region -= Look Up Word =-
                            
                            string text = this.m_Editor.Document.TextContent.Substring(before, after - before);
                            string t1,t2 = text;
                            
                            if (m_MainForm.m_GothicStructure != null)
                            {
                               
                                foreach (Instance inst in m_MainForm.m_GothicStructure.FuncList.Values)
                                {
                                    t1 = inst.Name;
                                    m_MainForm.m_GothicStructure.ConvertFuncForAutoComplete(ref t1, ref t2);
                                    //MessageBox.Show(text.Replace(' ', '#') + " " + t1.Replace(' ', '#'));

                                    if (string.Compare(t1.Trim(),text, true)==0)
                                    {
                                        m_MainForm.m_GothicStructure.OpenFile(inst.File, inst.Name);                                       
                                        return;
                                    }
                                }
                                foreach (Instance inst in m_MainForm.m_GothicStructure.NPCList.Values)
                                {
                                    if (string.Compare(inst.Name, text, true) == 0)
                                    {
                                        m_MainForm.m_GothicStructure.OpenFile(inst.File, inst.Name);
                                        return;
                                    }
                                }
                                foreach (Instance inst in m_MainForm.m_GothicStructure.DialogList.Values)
                                {
                                    if (string.Compare(inst.Name, text, true) == 0)
                                    {
                                        m_MainForm.m_GothicStructure.OpenFile(inst.File, inst.Name);
                                        return;
                                    }
                                }
                                foreach (Instance inst in m_MainForm.m_GothicStructure.VarList.Values)
                                {
                                    t1 = inst.Name;
                                    m_MainForm.m_GothicStructure.ConvertVarForAutoComplete(ref t1, ref t2);
                                    if (string.Compare(t1, text,true) == 0)
                                    {
                                        m_MainForm.m_GothicStructure.OpenFile(inst.File, inst.Name);
                                        return;
                                    }
                                }
                                foreach (Instance inst in m_MainForm.m_GothicStructure.ConstList.Values)
                                {
                                    t1 = inst.Name;
                                    m_MainForm.m_GothicStructure.ConvertConstForAutoComplete(ref t1, ref t2);
                                    
                                    if (string.Compare(t1.Trim(), text,true) == 0)
                                    {
                                        m_MainForm.m_GothicStructure.OpenFile(inst.File, inst.Name);
                                        return;
                                    }
                                }

                                foreach (Instance inst in m_MainForm.m_GothicStructure.ItemList.Values)
                                {
                                    if (string.Compare(inst.Name, text,true) == 0)
                                    {
                                        m_MainForm.m_GothicStructure.OpenFile(inst.File, inst.Name);
                                        return;
                                    }
                                }
                                
                                
                            }
                           /* string html = "<table><tr><th colspan=\"2\">" + text + "</th></tr><tr>";
                            int count = 0;
                            if (!string.IsNullOrEmpty(this.m_Project))
                            {
                                string lookup = this.m_MainForm.LookUpProject(text, this.m_Project);
                                if (!lookup.Equals("<ul></ul>"))
                                {
                                    count++;
                                }
                                html += "<tr><td colspan=\"2\">" + lookup + "</td></tr>";
                            }
                            Regex reg = new Regex(text,RegexOptions.IgnoreCase);
                            MatchCollection mc = reg.Matches(this.m_Editor.Document.TextContent);
                            if (mc.Count > 1)
                            {
                                ArrayList lines = new ArrayList();
                                html += "<tr><td colspan=\"2\">Im Dokument:</td></tr>";
                                foreach (Match m in mc)
                                {
                                    int lineNum = this.m_Editor.Document.GetLineNumberForOffset(m.Index);
                                    if (lineNum != this.m_Editor.ActiveTextAreaControl.Caret.Line && !lines.Contains(lineNum))
                                    {
                                        lines.Add(lineNum);
                                        lineNum++;
                                        count++;
                                        LineSegment line = this.m_Editor.Document.GetLineSegmentForOffset(m.Index);

                                        html += "<tr><td valign=\"top\" align=\"right\"><a href=\"\">" + lineNum.ToString() + "</a></td><td>" +
                                            this.m_Editor.Document.TextContent.Substring(line.Offset, line.TotalLength).Replace('<',' ').Replace('>',' ') + "</td></tr>";
                                    }
                                }
                                html += "</table>";
                            }
                            if (count > 0)
                            {
                               /* this.m_ToolTip.HTML = html;
                                this.m_ToolTip.Location = MousePosition;
                                this.m_ToolTip.Show();*/
                            //}

                            #endregion
                        }
                    }
                }
            }
        }

        #endregion

        #region -= Check Stop =-

        private bool CheckStop(char val)
        {
            switch (val)
            {
                case '(':
                case ')':
                case ' ':
                case '-':
                case '+':
                case '=':
                case '*':
                case '&':
                case '^':
                case '%':
                case '$':
                case '#':
                case '@':
                case '!':
                case '{':
                case '}':
                case '[':
                case ']':
                case '|':
                case '\\':
                case ';':
                case '\'':
                case ':':
                case '\"':
                case ',':
                case '.':
                case '<':
                case '>':
                case '/':
                case '?':
                case '`':
                case '~':
                case '\t':
                case '\r':
                case '\n':
                    return false;
                default:
                    return true;
            }
        }

        #endregion

        #region -= Misc =-

        void UndoStack_ActionRedone(object sender, EventArgs e)
        {
            this.m_Editor.ActiveTextAreaControl.TextArea.Invalidate();
        }

        /// <summary>
        /// Sets the Context Menu for the Editor
        /// </summary>
        /// <param name="ctx">Context Menu to use.</param>
        public void SetContextMenuStrip(ContextMenuStrip ctx)
        {
            this.m_Editor.ActiveTextAreaControl.TextArea.ContextMenuStrip = ctx;
        }

        public void Print()
        {
            PrintPreviewDialog dlg = new PrintPreviewDialog();
            dlg.Document = this.m_Editor.PrintDocument;
            dlg.ShowDialog();
        }

        public void ScrollTo(int offset)
        {
            if (offset > this.m_Editor.Document.TextLength)
            {
                return;
            }
            int line = this.m_Editor.Document.GetLineNumberForOffset(offset);
            this.m_Editor.ActiveTextAreaControl.Caret.Position = this.m_Editor.Document.OffsetToPosition(offset);
            this.m_Editor.ActiveTextAreaControl.ScrollTo(line);//.CenterViewOn(line, 0);
        }

        void TextArea_LostFocus(object sender, EventArgs e)
        {
            //if (this.m_ToolTip.Visible) this.m_ToolTip.Hide();
           
        }

        /// <summary>
        /// Gets or Sets the Project related to the file...
        /// </summary>
        public string Project
        {
            get { return this.m_Project; }

            set { this.m_Project = value; }
        }

        #endregion

        #region -= File Watcher =-

        /// <summary>
        /// Sets up the file watcher...
        /// </summary>
        public void SetupFileWatcher()
        {
            // Remove Events...
            if (this.m_FSW != null)
            {
                this.m_FSW.Changed -= new FileSystemEventHandler(m_FSW_Changed);
                this.m_FSW.Deleted -= new FileSystemEventHandler(m_FSW_Deleted);
                this.m_FSW.Renamed -= new RenamedEventHandler(m_FSW_Renamed);
            }

            // Create new FileWatcher...
            this.m_FSW = new FileSystemWatcher(Path.GetDirectoryName(this.m_Editor.FileName), Path.GetFileName(this.m_Editor.FileName));
            this.m_FSW.EnableRaisingEvents = true;
            this.m_FSW.IncludeSubdirectories = false;

            // Add Events...
            this.m_FSW.Changed += new FileSystemEventHandler(m_FSW_Changed);
            this.m_FSW.Deleted += new FileSystemEventHandler(m_FSW_Deleted);
            this.m_FSW.Renamed += new RenamedEventHandler(m_FSW_Renamed);
        }

        /// <summary>
        /// Occurs when the file is Renamed...
        /// </summary>
        /// <param name="sender">FileSystemWatcher</param>
        /// <param name="e">Events</param>
        void m_FSW_Renamed(object sender, RenamedEventArgs e)
        {
            this.Invoke(this.m_LoadFile, new object[] { e.FullPath });
        }

        /// <summary>
        /// Occurs when the file is Changed...
        /// </summary>
        /// <param name="sender">FileSystemWatcher</param>
        /// <param name="e">Events</param>
        void m_FSW_Changed(object sender, FileSystemEventArgs e)
        {
            this.Invoke(this.m_LoadFile, new object[] { e.FullPath });
        }

        /// <summary>
        /// Occurs when the file is deleted...
        /// </summary>
        /// <param name="sender">FileSystemWatcher</param>
        /// <param name="e">Events</param>
        void m_FSW_Deleted(object sender, FileSystemEventArgs e)
        {
            try { this.Invoke(this.m_CloseDel); }
            catch { }
        }

        /// <summary>
        /// Close Delegate Method...
        /// </summary>
        private void DelClose()
        {
            if (ignoreclose) return;
            if (MessageBox.Show(this.m_MainForm, this.m_Editor.FileName + " wurde gelöscht. Soll die Datei geschlossen werden?",
                "Stampfer", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                this.m_Changed = false;
                this.CloseTab();
            }
        }

        /// <summary>
        /// Reload File Delegate...
        /// </summary>
        /// <param name="file">Path to File.</param>
        private void DelReLoad(string file)
        {
            if (MessageBox.Show(this.m_MainForm, this.m_Editor.FileName + " wurde geändert. Soll die Datei erneut geladen werden?",
                "Stampfer", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                this.LoadFile(file);
            }
        }

        #endregion

        #region -= Book Marks =-

        /// <summary>
        /// Toggles a mark at the current line...
        /// </summary>
        public void ToggleMark()
        {
            this.m_Editor.Document.BookmarkManager.ToggleMarkAt(this.m_Editor.ActiveTextAreaControl.Caret.Line);
            this.m_Editor.ActiveTextAreaControl.Invalidate(true);
        }

        /// <summary>
        /// Removes all the Marks from the Edior...
        /// </summary>
        public void RemoveAllMarks()
        {
            this.m_Editor.Document.BookmarkManager.Clear();
            this.m_Editor.ActiveTextAreaControl.Invalidate(true);
        }

        /// <summary>
        /// Goes to the next(true) or previous(false) mark...
        /// </summary>
        /// <param name="forward">True(next) or Previous(false)</param>
        public void GotoMark(bool forward)
        {
            Bookmark b = (forward) ?
                this.m_Editor.Document.BookmarkManager.GetNextMark(this.m_Editor.ActiveTextAreaControl.Caret.Line) :
                this.m_Editor.Document.BookmarkManager.GetPrevMark(this.m_Editor.ActiveTextAreaControl.Caret.Line);
            if (b == null) return;
            this.m_Editor.ActiveTextAreaControl.CenterViewOn(b.LineNumber, 0);
            this.m_Editor.ActiveTextAreaControl.Caret.Line = b.LineNumber;
        }

        #endregion

        #region -= Setup Editor =-

        /// <summary>
        /// Sets up the Editor...
        /// </summary>
        /// <param name="node">XmlNode that holds the Configuration.</param>
        public void SetupEditor(Common.EditorConfig config)
        {
            this.m_Editor.ShowEOLMarkers = config.ShowEOL;
            this.m_Editor.ShowInvalidLines = config.ShowInvalidLines;
            this.m_Editor.ShowSpaces = config.ShowSpaces;
            this.m_Editor.ShowTabs = config.ShowTabs;
            this.m_Editor.ShowMatchingBracket = config.ShowMatchingBracket;
            this.m_Editor.ShowLineNumbers = config.ShowLineNumbers;
            this.m_Editor.ShowHRuler = config.ShowHRuler;
            this.m_Editor.ShowVRuler = config.ShowVRuler;
            this.m_Editor.EnableFolding = config.EnableCodeFolding;
            this.m_Editor.TextEditorProperties.CreateBackupCopy = config.Backup;
            this.m_Editor.Font = config.EditorFont;
            this.m_Editor.ConvertTabsToSpaces = config.ConvertTabs;
            this.m_Editor.TabIndent = config.TabIndent;
            this.m_Editor.VRulerRow = config.VerticalRulerCol;
            this.m_Editor.UseAntiAliasFont = config.UseAntiAlias; // #develop 2
            this.AutoCompleteAuto = config.AutoCompleteAuto;
            /* // #develop 3
            if (config.UseAntiAlias)
            {
                this.m_Editor.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            }*/
            this.m_Editor.AllowCaretBeyondEOL = config.AllowCaretBeyondEOL;
            this.m_Editor.TextEditorProperties.AutoInsertCurlyBracket = config.AutoInsertBracket;
            this.m_Editor.LineViewerStyle = (config.HighlightCurrentLine) ? LineViewerStyle.FullRow : LineViewerStyle.None;




            this.m_Editor.Document.FoldingManager.FoldingStrategy = new cDFoldingStrategy();
          
            switch (config.BracketMatchingStyle.ToLower())
            {
                case "before":
                    this.m_Editor.BracketMatchingStyle = BracketMatchingStyle.Before;
                    break;
                case "after":
                    this.m_Editor.BracketMatchingStyle = BracketMatchingStyle.After;
                    break;
            }
            switch (config.IndentStyle.ToLower())
            {
                case "auto":
                    this.m_Editor.IndentStyle = IndentStyle.Auto;
                    break;
                case "none":
                    this.m_Editor.IndentStyle = IndentStyle.None;
                    break;
                case "smart":
                    this.m_Editor.IndentStyle = IndentStyle.Smart;
                    break;
            }
        }

        #endregion

        #region -= Drag N Drop =-

        /// <summary>
        /// Enables files to be dropped in the dock window...
        /// </summary>
        /// <param name="sender">Text Area</param>
        /// <param name="e">Events</param>
        private void TextArea_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
            {
                e.Effect = DragDropEffects.All;
            }
        }

        /// <summary>
        /// Grabs the files dropped in the Editor...
        /// </summary>
        /// <param name="sender">Text Area</param>
        /// <param name="e">Events</param>
        private void TextArea_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
            {
                this.m_Host.CreateEditor(file, Path.GetFileName(file), this.Host.GetFileIcon(file, false), this);
            }
            this.m_MainForm.Focus();
        }

        #endregion

        #region -= Document Changed =-

        /// <summary>
        /// Occurs when the document changes...
        /// </summary>
        /// <param name="sender">Document</param>
        /// <param name="e">DocumentEvents</param>
        private void Document_DocumentChanged(object sender, DocumentEventArgs e)
        {
            if (!this.m_Changed)
            {
                this.TabText = "*" + this.TabText;
                this.m_Changed = true;
                this.m_MainForm.UpdateTitleBar();
            }
            
        }

        #endregion

        #region -= Highlighting =-

        /// <summary>
        /// Gets or Sets the Highligthing for the Editor...
        /// </summary>
        public string Highlighting
        {
            get { return this.m_Editor.Document.HighlightingStrategy.Name; }

            set
            {
                this.m_Editor.Document.HighlightingStrategy = HighlightingManager.Manager.FindHighlighter(value);
            }
        }

        #endregion

        #region -= Set/Get Text =-

        /// <summary>
        /// Sets the Text of the Editor...
        /// </summary>
        /// <param name="text">Text for the Editor</param>
        public void SetText(string text)
        {
            this.m_Editor.Document.TextContent = text;
            this.m_Changed = false;
            this.RemoveChangeStar();
        }

        /// <summary>
        /// Sets the Text of the Editor...
        /// </summary>
        /// <param name="text">Text for the Editor</param>
        public void SetTextChanged(string text)
        {
            this.m_Editor.Document.TextContent = text;
        }

        /// <summary>
        /// Gets the Text of the Editor...
        /// </summary>
        /// <returns>Text in the Editor</returns>
        public string GetText()
        {
            return this.m_Editor.Document.TextContent;
        }

        #endregion

        #region -= Get Persist String =-

        /// <summary>
        /// Overrides the Persist String for the Docking Control...
        /// </summary>
        /// <returns>Newly Formed Persist String.</returns>
        protected override string GetPersistString()
        {
            this.RemoveChangeStar();

            if (this.FileName == null)
            {
                return this.GetType().ToString() + "|" + this.TabText + "|none|" + this.m_Editor.Document.TextContent;
            }
            else
            {
                return this.GetType().ToString() + "|" + this.TabText + "|" + this.FileName + "|" +
                    this.m_Editor.ActiveTextAreaControl.Caret.Offset.ToString() + "|" + this.m_Project;
            }
        }

        #endregion

        #region IPeterPluginTab Members

        /// <summary>
        /// Closes this Editor...
        /// </summary>
        public bool CloseTab()
        {
            
            
            if (this.m_Changed)
            {
                string file = (this.FileName == null) ? this.TabText : this.FileName;
                switch (MessageBox.Show(this.m_MainForm, "Soll " + file + " gespeichert werden?", "Stampfer",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                {
                    case DialogResult.Yes:
                        if (this.FileName == null)
                        {
                            this.m_Host.SaveAs(this);
                        }
                        else
                        {
                            this.Save();
                        }
                        break;
                    case DialogResult.No:
                        this.m_Changed = false;
                        break;
                    case DialogResult.Cancel:
                        return false;
                }
            }
            RemoveAutoComplete();
            if (this.m_FSW != null)
                this.m_FSW.EnableRaisingEvents = false;
            this.Close();
            this.m_MainForm.UpdateCaretPos(0, 0, 0, null);
            this.Dispose(true);
            return true;
        }

        /// <summary>
        /// Saves the Current Document...
        /// </summary>
        public void Save()
        {
            this.SaveAs(this.m_Editor.FileName);
        }

        /// <summary>
        /// Saves the Document As...
        /// </summary>
        /// <param name="filePath">Path to File to Save As.</param>
        public void SaveAs(string filePath)
        {
            if (this.m_FSW != null)
                this.m_FSW.EnableRaisingEvents = false;

            if (string.IsNullOrEmpty(Path.GetExtension(filePath)))
                filePath += ".txt";

            FileInfo f = new FileInfo(filePath);
            if (f.Exists && f.IsReadOnly)
            {
                if (MessageBox.Show(this.m_MainForm, "Die Datei \"" + filePath + "\" ist nur zum Lesen freigegeben.\nSoll die Datei unter anderem Namen gespeichert werden?",
                    "Stampfer", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.m_Host.SaveAs(this);
                    return;
                }
                else
                {
                    return;
                }
            }

            
            this.ToolTipText = filePath;
            this.TabText = Path.GetFileName(filePath);
            this.Icon = this.m_Host.GetFileIcon(filePath, false);
            try
            {
                if (Path.GetExtension(filePath) != ".d")
                {
                    this.m_Editor.SaveFile(filePath);
                }
                else
                {
                    //Backup
                    if (this.m_Editor.TextEditorProperties.CreateBackupCopy)
                    {
                        try
                        {
                            if (File.Exists(filePath))
                            {
                                string backupName;
                                try
                                {
                                    if (m_MainForm.m_BackupFolder != ""
                                        && Directory.Exists(m_MainForm.m_BackupFolder))
                                    {
                                        string Datum = m_MainForm.m_BackupFolder + "\\" + DateTime.Today.Day.ToString() + "." + DateTime.Today.Month.ToString();
                                        if (!Directory.Exists(Datum))
                                        {
                                            Directory.CreateDirectory(Datum);
                                        }
                                        
                                        backupName = Datum + "\\" + Path.GetFileName(filePath);
                                        if (File.Exists(backupName))
                                        {
                                            File.Delete(backupName);
                                        }
                                        File.Copy(filePath, backupName);
                                    }
                                }
                                catch//ta
                                {
                                }


                                try
                                {
                                    if (!m_MainForm.m_BackupFolderOnly)
                                    {
                                        backupName = filePath + ".bak";
                                        if (File.Exists(backupName))
                                        {
                                            File.Delete(backupName);
                                        }
                                        File.Copy(filePath, backupName);
                                    }
                                }
                                catch
                                {
                                }
                                
                                
                                
                            }
                        }
                        catch (Exception)
                        {
                            //				IMessageService messageService =(IMessageService)ServiceManager.Services.GetService(typeof(IMessageService));
                            //				messageService.ShowError(e, "Can not create backup copy of " + fileName);
                        }
                    }


			        StreamWriter stream;

                    stream = new StreamWriter(filePath, false, Encoding.GetEncoding(1252));


                    stream.Write(this.m_Editor.Document.TextContent);
        			
			        stream.Close();

                    this.m_Editor.FileName = filePath;
		        }
		
            }
           
            catch (IOException m)
            {
                
                MessageBox.Show(m.Message,"Dateifehler",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            this.m_Editor.Document.HighlightingStrategy = HighlightingManager.Manager.FindHighlighterForFile(filePath);
            bool reload = false;
            if (string.IsNullOrEmpty(this.m_Editor.FileName))
            {
                reload = true;
            }
            else
            {
                if (Path.GetExtension(filePath) != Path.GetExtension(this.m_Editor.FileName))
                {
                    reload = true;
                }
            }

            if (reload)
            {
                this.m_Editor.Document.HighlightingStrategy = HighlightingManager.Manager.FindHighlighterForFile(filePath);
            }
            this.m_Changed = false;
            this.RemoveChangeStar();

            this.SetupFileWatcher();
            UpdateFolding();
        }


        public void UpdateFolding()
        {
            //if (m_Editor.FileName.EndsWith(".d"))
            
            {
                this.m_Editor.Document.FoldingManager.UpdateFoldings(null, null);
               
            }
            m_Editor.Refresh();
        }

        public void FoldingExpand()
        {
            foreach (FoldMarker f in this.m_Editor.Document.FoldingManager.FoldMarker)
            {
                f.IsFolded = false;
            }
            m_Editor.Refresh();
        }

        public void RegionsExpand()
        {
            foreach (FoldMarker f in this.m_Editor.Document.FoldingManager.FoldMarker)
            {
                if (f.FoldType==FoldType.Region)
                    f.IsFolded = false;
            }
            m_Editor.Refresh();
        }

        public void RegionsCollapse()
        {
            foreach (FoldMarker f in this.m_Editor.Document.FoldingManager.FoldMarker)
            {
                if (f.FoldType == FoldType.Region)
                    f.IsFolded = true;
            }
            m_Editor.Refresh();
        }

        public void FoldingCollapse()
        {
            foreach (FoldMarker f in this.m_Editor.Document.FoldingManager.FoldMarker)
            {
                f.IsFolded = true;
            }
            m_Editor.Refresh();
        }

        /// <summary>
        /// Clipboard Cut Action...
        /// </summary>
        public void Cut()
        {
            this.m_Editor.ActiveTextAreaControl.TextArea.ClipboardHandler.Cut(null, null);
        }

        /// <summary>
        /// Clipboard Copy Action...
        /// </summary>
        public void Copy()
        {
            this.m_Editor.ActiveTextAreaControl.TextArea.ClipboardHandler.Copy(null, null);
        }

        /// <summary>
        /// Clipboard Paste Action...
        /// </summary>
        public void Paste()
        {
            try
            {
                this.m_Editor.ActiveTextAreaControl.TextArea.ClipboardHandler.Paste(null, null);
               
                //this.m_Editor.ActiveTextAreaControl.Document.Insert(this.m_Editor.ActiveTextAreaControl.Caret.Offset, Clipboard.GetText());
            }
            catch
            {
            }
        }

        /// <summary>
        /// Edit Undo Action...
        /// </summary>
        public void Undo()
        {
            this.m_Editor.Document.UndoStack.Undo();
        }

        /// <summary>
        /// Edit Redo Action...
        /// </summary>
        public void Redo()
        {
            this.m_Editor.Document.UndoStack.Redo();
        }

        /// <summary>
        /// Clipboard Delete Action...
        /// </summary>
        public void Delete()
        {
            this.m_Editor.ActiveTextAreaControl.TextArea.ClipboardHandler.Delete(null, null);
        }

        public void Duplicate()
        {
            if (this.m_Editor.ActiveTextAreaControl.SelectionManager.HasSomethingSelected)
            {
                string selection = this.m_Editor.ActiveTextAreaControl.SelectionManager.SelectedText;
                int pos = this.m_Editor.ActiveTextAreaControl.SelectionManager.SelectionCollection[0].EndOffset;
                this.m_Editor.Document.Insert(pos, selection);

                this.m_Editor.ActiveTextAreaControl.TextArea.Invalidate();
            }
        }

        /// <summary>
        /// Selects All the Text of the Document...
        /// </summary>
        public void SelectAll()
        {
            try
            {
                this.m_Editor.ActiveTextAreaControl.TextArea.ClipboardHandler.SelectAll(null, null);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Loads the Given File...
        /// </summary>
        /// <param name="filePath">Path to File.</param>
        public void LoadFile(string filePath)
        {
            FileInfo f = new FileInfo(filePath);
            this.TabText = f.Name;
            this.ToolTipText = filePath;
            this.m_Editor.LoadFile(filePath, true, true);
            this.m_Changed = false;
            this.RemoveChangeStar();

            if (string.IsNullOrEmpty(f.Extension))
            {
                this.m_Editor.Document.HighlightingStrategy = HighlightingManager.Manager.FindHighlighter("HTML");
            }
            if (f.IsReadOnly)
            {
                this.m_Editor.Document.ReadOnly = true;
                Bitmap b = this.Icon.ToBitmap();
                using (Graphics g = Graphics.FromImage(b))
                {
                    Image img = this.m_MainForm.GetInternalImage("_lock");
                    g.DrawImage(img, new Point(0, 0));
                    this.Icon = Icon.FromHandle(b.GetHicon());
                }
            }
            this.SetupFileWatcher();
            this.m_Editor.ActiveTextAreaControl.TextArea.Focus();
            UpdateFolding();
        }

        /// <summary>
        /// Marks all of the Occurances of the given Pattern...
        /// </summary>
        /// <param name="reg">Pattern to Mark.</param>
        public void MarkAll(Regex reg)
        {
            MatchCollection mc = reg.Matches(this.m_Editor.Document.TextContent);
            foreach (Match m in mc)
            {
                int line = this.m_Editor.Document.GetLineNumberForOffset(m.Index);
                this.m_Editor.Document.BookmarkManager.AddMark(new Bookmark(this.m_Editor.Document, line));
            }
            this.m_Editor.ActiveTextAreaControl.Invalidate(true);
            this.m_Editor.ActiveTextAreaControl.TextArea.Focus();
        }
        
       

        public void JumpTo(int line)
        {
            this.m_Editor.ActiveTextAreaControl.CenterViewOn(line, 0);
            this.m_Editor.ActiveTextAreaControl.Caret.Line = line;

        }
        public void JumpToError(int line, int line2)
        {
            this.m_Editor.ActiveTextAreaControl.CenterViewOn(line-1, 0);
            this.m_Editor.ActiveTextAreaControl.Caret.Line = line-1;
            this.m_Editor.ActiveTextAreaControl.Caret.Column= line2-1;

        }
        public void JumpToPos(int offset,int pos)
        {
           
            int line = this.m_Editor.Document.GetLineNumberForOffset(offset);
            this.m_Editor.ActiveTextAreaControl.Caret.Position = this.m_Editor.Document.OffsetToPosition(offset);
            this.m_Editor.ActiveTextAreaControl.SelectionManager.SetSelection(
                this.m_Editor.Document.OffsetToPosition(offset), this.m_Editor.Document.OffsetToPosition(pos));
            this.m_Editor.ActiveTextAreaControl.CenterViewOn(line, 0);
            this.m_Editor.ActiveTextAreaControl.TextArea.Focus();
        }
        /// <summary>
        /// Finds the Next Occurance of the given Pattern...
        /// </summary>
        /// <param name="reg">Pattern to Find.</param>
        public bool FindNext(Regex reg, bool searchUp)
        {
            this.m_Host.Trace("Suche nach " + reg.ToString());
            Match m = null;
            try
            {
                if (searchUp)
                {
                    MatchCollection mc = reg.Matches(this.m_Editor.Document.TextContent.Substring(0, this.m_Editor.ActiveTextAreaControl.Caret.Offset));
                    if (mc.Count > 0)
                    {
                        m = mc[mc.Count - 1];
                        HighlightMatch(m, searchUp);
                    }
                    else
                    {
                        mc = reg.Matches(this.m_Editor.Document.TextContent);
                        if (mc.Count > 0)
                        {
                            m = mc[mc.Count - 1];
                            HighlightMatch(m, searchUp);
                        }
                        else
                        {
                            CouldNotFind(reg);
                            return false;
                        }
                    }
                }
                else
                {
                    m = reg.Match(this.m_Editor.Document.TextContent, this.m_Editor.ActiveTextAreaControl.Caret.Offset);
                    if (!m.Success)
                    {
                        m = reg.Match(this.m_Editor.Document.TextContent);
                    }

                    if (m.Success)
                    {
                        HighlightMatch(m, searchUp);
                    }
                    else
                    {
                        CouldNotFind(reg);
                        return false;
                    }
                    this.m_Editor.ActiveTextAreaControl.TextArea.Focus();
                }
            }
            catch
            {
            }

            return true;
        }



        public bool FindText(Regex reg)
        {
            this.m_Host.Trace("Suche nach " + reg.ToString());
            //Match m = null;
            try
            {
                
                    MatchCollection mc = reg.Matches(this.m_Editor.Document.TextContent);
                    if (mc.Count > 0)
                    {
                       
                        ScrollTo(mc[0].Index);
                    }
                    
               
               
            }
            catch
            {
            }

            return true;
        }



        private void CouldNotFind(Regex reg)
        {

            this.m_FindPos = -1;
            MessageBox.Show(this.m_MainForm, "Keine Ergebnisse '" + reg.ToString() + "' gefunden.", "Stampfer", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.m_Host.Trace("");
        }

        private void HighlightMatch(Match m, bool searchUp)
        {
            this.m_Editor.ActiveTextAreaControl.SelectionManager.SetSelection(
                this.m_Editor.Document.OffsetToPosition(m.Index), this.m_Editor.Document.OffsetToPosition(m.Index + m.Length));
            if (searchUp)
            {
                this.m_Editor.ActiveTextAreaControl.Caret.Position = this.m_Editor.Document.OffsetToPosition(m.Index);
            }
            else
            {
                this.m_Editor.ActiveTextAreaControl.Caret.Position = this.m_Editor.Document.OffsetToPosition(m.Index + m.Length);
            }
            this.m_Host.Trace("");
            this.m_FindPos = m.Index;
        }

        /// <summary>
        /// Replaces the Next Occurance of the Given Pattern...
        /// </summary>
        /// <param name="reg">Pattern to Replace.</param>
        public void ReplaceNext(Regex reg, string replaceWith, bool searchUp)
        {
            this.FindNext(reg, searchUp);
            if (this.m_FindPos != -1)
            {
                this.m_Editor.ActiveTextAreaControl.Caret.Position = this.m_Editor.Document.OffsetToPosition(this.m_FindPos);
                string replaced = reg.Replace(this.m_Editor.Document.TextContent, replaceWith, 1, this.m_FindPos);
                this.m_Editor.Document.Replace(0, this.m_Editor.Document.TextLength, replaced);
                this.m_Editor.ActiveTextAreaControl.TextArea.Focus();
            }
            this.m_Editor.ActiveTextAreaControl.TextArea.Focus();
        }
        /// <summary>
        /// Gets the Caret Position
        /// </summary>
        /// <param name="reg">Pattern to Replace.</param>
        public Point GetCaretPos()
        {
            return this.m_Editor.ActiveTextAreaControl.Caret.ScreenPosition;
        }


        /// <summary>
        /// Replaces all Occurances of the Given Pattern...
        /// </summary>
        /// <param name="reg">Pattern to Replace.</param>
        public void ReplaceAll(Regex reg, string replaceWith)
        {
            string replaced = reg.Replace(this.m_Editor.Document.TextContent, replaceWith);
            if (replaced != this.m_Editor.Document.TextContent)
            {
                this.m_Editor.Document.Replace(0, this.m_Editor.Document.TextLength, replaced);
            }
            else
            {
                this.m_Host.Trace("Keine Ergebnisse '" + reg.ToString() + "' in " + this.TabText + "gefunden.");
            }
            this.m_Editor.ActiveTextAreaControl.TextArea.Focus();
        }

        public void ReplaceAllMarked(Regex reg, string replaceWith)
        {
            string replaced = reg.Replace(Selection, replaceWith);
            if (replaced != Selection)
            {
                this.m_Editor.Document.Replace(this.m_Editor.ActiveTextAreaControl.SelectionManager.SelectionCollection[0].Offset, this.m_Editor.ActiveTextAreaControl.SelectionManager.SelectionCollection[0].EndOffset - this.m_Editor.ActiveTextAreaControl.SelectionManager.SelectionCollection[0].Offset, replaced);
            }
            else
            {
                this.m_Host.Trace("Keine Ergebnisse '" + reg.ToString() + "' in " + this.TabText + "gefunden.");
            }
            this.m_Editor.ActiveTextAreaControl.TextArea.Focus();
        }
        Regex nreg = new Regex("\n");
        public void Indent()
        {
            string[] s = new string[this.m_Editor.Document.TotalNumberOfLines];
            string text;
            int k = 0;
            int indent=0;
            int l;
            int brace = 0;
            string tempstring= ""; 
            for (int i = 0; i < this.m_Editor.Document.TotalNumberOfLines; i++)
            {
                text = this.m_Editor.Document.GetText(this.m_Editor.Document.GetLineSegment(i));
                k = 0;
                l = 0;
               
                while (k < text.Length)
                {
                    if (Char.IsWhiteSpace(text[k]))
                    {
                        l++;
                    }
                    else
                    {
                        break;
                    }
                    k++;
                }
                text = text.Remove(0, l);                
                s[i]=text;
                
               
            }
            //this.m_Editor.Document.Replace(0, this.m_Editor.Document.TextContent.Length, s.ToString());
            StringBuilder s2 = new StringBuilder(this.m_Editor.Document.TextContent.Length * 5);
            for (int i = 0; i < s.Length; i++)
            {
                tempstring = "";
                text = s[i];
                    k = 0;
                    
                   
                    while (k < text.Length)
                    {
                        if (text[k]=='{')
                        {
                            indent++;
                        }
                        else if (text[k] == '}')
                        {
                            indent--;
                        }
                        k++;
                    }

                    if (text.Contains("{"))
                    {
                        brace = 1;
                        //text = text.Replace("{", "{ ");
                    }
                    else
                    {
                        brace = 0;
                    }
                    for (int z = 0; z < indent - brace; z++)
                    {
                        tempstring += "\t";
                    }
                    text = text.Insert(0, tempstring);
                    s2.Append(text + "\r\n");
            }


            s2.Remove(s2.Length - 1, 1);

            this.m_Editor.Document.Replace(0, this.m_Editor.Document.TextContent.Length, s2.ToString());
            UpdateFolding();
            this.m_Editor.Refresh();

        }
        public void Enclose(string s1, string s2)
        {
            int Leer=0;
            char t = Convert.ToChar("\t");
            if (this.m_Editor.ActiveTextAreaControl.SelectionManager.SelectionCollection.Count < 1)
            {
                this.m_Editor.Document.Insert(this.m_Editor.ActiveTextAreaControl.Caret.Offset, s1);
                this.m_Editor.ActiveTextAreaControl.TextArea.Caret.Position = this.m_Editor.Document.OffsetToPosition(this.m_Editor.ActiveTextAreaControl.Caret.Offset + s1.Length);
                this.m_Editor.Document.Insert(this.m_Editor.ActiveTextAreaControl.Caret.Offset, s2);
               

            }
            else if (s2.Length > 0)
            {

                this.m_Editor.Document.Insert(this.m_Editor.ActiveTextAreaControl.SelectionManager.SelectionCollection[0].EndOffset, s2);


                for (int i = this.m_Editor.Document.LineSegmentCollection[this.m_Editor.ActiveTextAreaControl.SelectionManager.SelectionCollection[0].StartPosition.Y].Offset; i < this.m_Editor.ActiveTextAreaControl.SelectionManager.SelectionCollection[0].Offset; i++)
                {
                    if (this.m_Editor.Document.TextContent[i] == ' ' || this.m_Editor.Document.TextContent[i] == t)
                    {
                        Leer++;
                    }
                    else
                    {
                        break;
                    }
                }
                this.m_Editor.ActiveTextAreaControl.SelectionManager.SetSelection(new Point(this.m_Editor.ActiveTextAreaControl.SelectionManager.SelectionCollection[0].StartPosition.X - Leer,
                    this.m_Editor.ActiveTextAreaControl.SelectionManager.SelectionCollection[0].StartPosition.Y), new Point(this.m_Editor.ActiveTextAreaControl.SelectionManager.SelectionCollection[0].EndPosition.X,
                    this.m_Editor.ActiveTextAreaControl.SelectionManager.SelectionCollection[0].EndPosition.Y));
                this.m_Editor.Document.Insert(this.m_Editor.ActiveTextAreaControl.SelectionManager.SelectionCollection[0].Offset, s1);

                this.m_Editor.ActiveTextAreaControl.SelectionManager.ClearSelection();
            }
            else
            {
                int i = this.m_Editor.ActiveTextAreaControl.SelectionManager.SelectionCollection[0].Offset;
                string[] s = this.m_Editor.Document.TextContent.Substring(i,this.m_Editor.ActiveTextAreaControl.SelectionManager.SelectionCollection[0].EndOffset-i).Split('\n');
                StringBuilder sb = new StringBuilder();
                for (int j = 0; j < s.Length; j++)
                {
                    sb.Append(s1 + s[j]);
                }
                
                
                this.m_Editor.Document.Remove(i, this.m_Editor.ActiveTextAreaControl.SelectionManager.SelectionCollection[0].EndOffset - i);
                this.m_Editor.Document.Insert(i, sb.ToString());
                this.m_Editor.ActiveTextAreaControl.SelectionManager.ClearSelection();
                
            }
        }
        /// <summary>
        /// Selects the word at the given offset...
        /// </summary>
        /// <param name="line">Line Word is on.</param>
        /// <param name="offset">Offset Word is at.</param>
        /// <param name="wordLeng">Length of Word.</param>
        public void SelectWord(int line, int offset, int wordLen)
        {
            this.m_Editor.ActiveTextAreaControl.ScrollTo(line);
            this.m_Editor.ActiveTextAreaControl.Caret.Line = line;
            this.m_Editor.ActiveTextAreaControl.SelectionManager.SetSelection(
                this.m_Editor.Document.OffsetToPosition(this.m_Editor.ActiveTextAreaControl.Caret.Offset + offset),
                this.m_Editor.Document.OffsetToPosition(this.m_Editor.ActiveTextAreaControl.Caret.Offset + offset + wordLen));
            this.m_Editor.ActiveTextAreaControl.TextArea.Focus();
        }

        /// <summary>
        /// Removes the * at the front of a changed document...
        /// </summary>
        private void RemoveChangeStar()
        {
            if (this.TabText.IndexOf('*') == 0)
            {
                this.TabText = this.TabText.Substring(1);
            }
        }

        /// <summary>
        /// Sets the Host Control...
        /// </summary>
        public IPeterPluginHost Host
        {
            get { return this.m_Host; }

            set { this.m_Host = value; }
        }

        /// <summary>
        /// Gets the File Name of the Document...
        /// </summary>
        public string FileName
        {
            get { return this.m_Editor.FileName; }
        }

        /// <summary>
        /// Gets the Selected Text...
        /// </summary>
        public string Selection
        {
            get
            {
                if (this.m_Editor.ActiveTextAreaControl.SelectionManager.HasSomethingSelected)
                {
                    return this.m_Editor.ActiveTextAreaControl.SelectionManager.SelectedText;
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// Gets if we are able to do an Undo Action...
        /// </summary>
        public bool AbleToUndo
        {
            get { return this.m_Editor.EnableUndo; }
        }

        /// <summary>
        /// Gets if we are able to do a Redo Action...
        /// </summary>
        public bool AbleToRedo
        {
            get { return this.m_Editor.EnableRedo; }
        }

        /// <summary>
        /// Gets if the Editor needs to be Saved...
        /// </summary>
        public bool NeedsSaving
        {
            get { return this.m_Changed; }
        }

        /// <summary>
        /// Gets if this Control can Paste...
        /// </summary>
        public bool AbleToPaste
        {
            get { return true; }
        }

        /// <summary>
        /// Gets if this Control can Cut...
        /// </summary>
        public bool AbleToCut
        {
            get { return true; }
        }

        /// <summary>
        /// Gets if this Control can Cut...
        /// </summary>
        public bool AbleToCopy
        {
            get { return true; }
        }

        /// <summary>
        /// Gets if this Control can Select All...
        /// </summary>
        public bool AbleToSelectAll
        {
            get { return true; }
        }

        /// <summary>
        /// Gets if this Control can Save...
        /// </summary>
        public bool AbleToSave
        {
            get { return true; }
        }

        /// <summary>
        /// Gets if this Control can Delete...
        /// </summary>
        public bool AbleToDelete
        {
            get { return true; }
        }

        #endregion

        #region IHtmlInterface Members

        public void LinkClick(HtmlElement activeElement)
        {
            int line = 0;
            if (Int32.TryParse(activeElement.InnerText, out line))
            {
                --line;
                LineSegment lineSeg = this.m_Editor.Document.GetLineSegment(line);
                this.m_Editor.ActiveTextAreaControl.Caret.Position = this.m_Editor.Document.OffsetToPosition(lineSeg.Offset);
                this.m_Editor.ActiveTextAreaControl.CenterViewOn(line, 0);
            }
            else
            {
                string file = activeElement.GetAttribute("href");
                file = System.Web.HttpUtility.UrlDecode(file);
                file = file.Substring(8);
                file = file.Replace('/', '\\');
                string offset = activeElement.GetAttribute("offset");
                this.m_Host.CreateEditor(file, Path.GetFileName(file), this.Host.GetFileIcon(file, false), this);
                this.m_MainForm.GetEditor(file).ScrollTo(Convert.ToInt32(offset));
                this.m_MainForm.GetEditor(file).Project = this.m_Project;
            }
        }

        #endregion

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Editor
            // 
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Name = "Editor";
            this.ResumeLayout(false);

        }

    }
    public static class M_AutoCompleteDa
    {
        public static bool Active;
    }
}
