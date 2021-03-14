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
using System.Xml;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using PeterInterface;
using WeifenLuo.WinFormsUI.Docking;
using System.ComponentModel;
using ICSharpCode.TextEditor.Document;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Threading;


using System.Runtime.Remoting.Channels.Ipc;    //Importing IPC
//channel
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting;


namespace Peter
{
    public partial class MainForm : Form, IPeterPluginHost
    { 
        
       

        private const string CONFIG_FILE = "Config.dat";
        private const string DOCK_CONFIG_FILE = "DockConfig.dat";
        private const string SCHEME_FOLDER = "HighLightSchemes\\";
        private const string PLUGIN_FOLDER = "Plugins\\";
        private const string VERSION = "1.0";
        private const int WM_COPYDATA = 0x4A;

        private int m_NewCount;
        private int m_RecentProjectCount;
        private int m_RecentFileCount;
        private bool m_SaveonExit;
        private string m_DockConfigFile;
        private string m_ConfigFile;
        public string m_ScriptsPath;
        public string m_BilderPath;
        public bool m_ParserMessageBox;
        public string m_BackupFolder;
        public int m_BackupEach;
        public bool m_BackupFolderOnly;
        public bool m_AutoBrackets;
        public Classes.AutoComplete m_AutoComplete; //= new Peter.Classes.AutoComplete();
       
        
        public Common.EditorConfig m_EditorConfig;
        private cPluginCollection m_Plugins;
        private Find m_FindControl;
        private GoToLine m_GotoControl;
        private ProjectManager m_ProjMan;
        private IDockContent m_ActiveContent;
        private ctrlCodeStructure m_CodeStructure;
        public ctrlGothicInstances m_GothicStructure;
        private ctrlQuestManager m_QuestManager;
        private Thread m_OUThread;
        public bool TabCloseBlock=false;
        SplashScreen ss= new SplashScreen(false);
        public Editor MyActiveEditor;
        Editor LastEditor;
        public bool initialized = false;
        public string autocompletemenuauto="Auto-Ergänzung > auto";
        public string autocompletemenumanu = "Auto-Ergänzung > manuell";
        #region -= Constructor =-

        public MainForm(string[] args)
        {


            ss.Show();
            ss.Update();            
            Init(args);           
            
            
        }

        #endregion
        public void Init(string[] args)
        {
             
            
            this.m_SaveonExit = true;

            InitializeComponent();
            LoadTools();
            m_AutoComplete = new Peter.Classes.AutoComplete(this.ImgList, sslMain);
            this.Deactivate += new EventHandler(MainForm_Deactivate);

            // Set the Config File...
            this.m_DockConfigFile = Path.GetDirectoryName(Application.ExecutablePath) + "\\" + DOCK_CONFIG_FILE;
            this.m_ConfigFile = Path.GetDirectoryName(Application.ExecutablePath) + "\\" + CONFIG_FILE;
            this.m_EditorConfig = new Common.EditorConfig();

            // Load Any Configuration from Config File...
            if (File.Exists(this.m_ConfigFile))
            {
                // Load Config File...
                LoadConfigFile(false);
            }

            // Set Variabales...
            this.m_NewCount = 0;
            this.m_Plugins = new cPluginCollection();
            this.mnuHighlighting.Enabled = false;
            this.bookMarksToolStripMenuItem.Enabled = false;
            this.mnuCode.Enabled = false;
            this.m_ActiveContent = null;

            
            // Set up Find Control...
            this.m_FindControl = new Find(this);
            this.m_FindControl.Host = this;

            //this.m_GotoControl = new GoToLine(this);
            // this.m_GotoControl.Host = this;
            this.m_FindControl.Icon = Icon.FromHandle(((Bitmap)this.GetInternalImage("Find")).GetHicon());

            // Set up Project Manager...
            this.m_ProjMan = new ProjectManager(this);
            this.m_ProjMan.Host = this;
            this.m_ProjMan.TabPageContextMenuStrip = this.ctxTab;
            this.m_ProjMan.Icon = Icon.FromHandle(((Bitmap)this.GetInternalImage("Project")).GetHicon());

            // Set up Code Structure...
            this.m_CodeStructure = new ctrlCodeStructure(this);
            this.m_CodeStructure.Host = this;
            this.m_CodeStructure.Icon = Icon.FromHandle(((Bitmap)this.GetInternalImage("Code")).GetHicon());

            //Set up GothicInstances
            this.m_GothicStructure = new ctrlGothicInstances(this);
            this.m_GothicStructure.Host = this;
            this.m_QuestManager = new ctrlQuestManager(this);
            this.m_QuestManager.Host = this;
            // Set Events...
            this.ctxEditor.Opening += new CancelEventHandler(ctxEditor_Opening);
            this.ctxTab.Opening += new CancelEventHandler(ctxTab_Opening);
            this.mnuEdit.DropDownOpening += new EventHandler(mnuEdit_DropDownOpening);
            this.fileToolStripMenuItem.DropDownOpening += new EventHandler(fileToolStripMenuItem_DropDownOpening);
            this.txtFindNext.KeyDown += new KeyEventHandler(txtFindNext_KeyDown);

            // Setup The Dock Panel...
            this.DockMain.ShowDocumentIcon = false;
           
           /* m_buttonWindowList = new InertButton(ImageButtonWindowList, ImageButtonWindowListOverflow);
            m_toolTip.SetToolTip(m_buttonWindowList, ToolTipSelect);
            m_buttonWindowList.Click += new EventHandler(WindowList_Click);
            Controls.Add(m_buttonWindowList);*/
            
//Skin
            this.DockMain.Skin.DockPaneStripSkin.DocumentGradient.ActiveTabGradient.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.DockMain.Skin.DockPaneStripSkin.DocumentGradient.ActiveTabGradient.StartColor = Color.White;
            this.DockMain.Skin.DockPaneStripSkin.DocumentGradient.ActiveTabGradient.EndColor = Color.FromArgb(215,235,255);

            this.DockMain.Skin.DockPaneStripSkin.DocumentGradient.InactiveTabGradient.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.DockMain.Skin.DockPaneStripSkin.DocumentGradient.InactiveTabGradient.StartColor = Color.White;
            this.DockMain.Skin.DockPaneStripSkin.DocumentGradient.InactiveTabGradient.EndColor = Color.FromArgb(152, 180, 210);

            this.DockMain.Skin.DockPaneStripSkin.DocumentGradient.DockStripGradient.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.DockMain.Skin.DockPaneStripSkin.DocumentGradient.DockStripGradient.StartColor = Color.White;
            this.DockMain.Skin.DockPaneStripSkin.DocumentGradient.DockStripGradient.EndColor = SystemColors.ControlDark;

            this.DockMain.Skin.DockPaneStripSkin.ToolWindowGradient.ActiveCaptionGradient.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.DockMain.Skin.DockPaneStripSkin.ToolWindowGradient.ActiveCaptionGradient.StartColor = Color.FromArgb(215, 235, 255);
            this.DockMain.Skin.DockPaneStripSkin.ToolWindowGradient.ActiveCaptionGradient.EndColor = Color.White;

            this.DockMain.Skin.DockPaneStripSkin.ToolWindowGradient.InactiveCaptionGradient.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.DockMain.Skin.DockPaneStripSkin.ToolWindowGradient.InactiveCaptionGradient.StartColor = Color.FromArgb(152, 180, 210);
            this.DockMain.Skin.DockPaneStripSkin.ToolWindowGradient.InactiveCaptionGradient.EndColor =  Color.White;

            this.DockMain.Skin.DockPaneStripSkin.ToolWindowGradient.DockStripGradient.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.DockMain.Skin.DockPaneStripSkin.ToolWindowGradient.DockStripGradient.StartColor = SystemColors.ControlDark;
            this.DockMain.Skin.DockPaneStripSkin.ToolWindowGradient.DockStripGradient.EndColor =  Color.White;

            this.DockMain.Skin.DockPaneStripSkin.ToolWindowGradient.ActiveTabGradient.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.DockMain.Skin.DockPaneStripSkin.ToolWindowGradient.ActiveTabGradient.StartColor =  Color.FromArgb(215, 235, 255);
            this.DockMain.Skin.DockPaneStripSkin.ToolWindowGradient.ActiveTabGradient.EndColor =Color.White;

            this.DockMain.Skin.DockPaneStripSkin.ToolWindowGradient.InactiveTabGradient.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.DockMain.Skin.DockPaneStripSkin.ToolWindowGradient.InactiveTabGradient.StartColor =Color.FromArgb(152, 180, 210);
            this.DockMain.Skin.DockPaneStripSkin.ToolWindowGradient.InactiveTabGradient.EndColor =  Color.White;
            
            this.DockMain.Skin.AutoHideStripSkin.TabGradient.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.DockMain.Skin.AutoHideStripSkin.TabGradient.StartColor = Color.White;
            this.DockMain.Skin.AutoHideStripSkin.TabGradient.EndColor = Color.FromArgb(152, 180, 210);
            this.DockMain.Skin.AutoHideStripSkin.TabGradient.TextColor = SystemColors.ControlText;
            
           //Endskin


            this.DockMain.ActiveContentChanged += new EventHandler(DockMain_ActiveContentChanged);
            
            this.DockMain.ContentRemoved += new EventHandler<DockContentEventArgs>(DockMain_ContentRemoved);
            this.DockMain.ActiveDocumentChanged += new EventHandler(DockMain_ActiveDocumentChanged);


            // Drag N Drop...
            this.DockMain.AllowDrop = true;
            this.DockMain.DragEnter += new DragEventHandler(DockMain_DragEnter);
            this.DockMain.DragDrop += new DragEventHandler(DockMain_DragDrop);
            
            // Load Highlighting Files...
            this.LoadHighlighting();

            // Load Plugins...
            this.LoadPlugins();

            // Load Configuration...
            if (File.Exists(this.m_DockConfigFile))
            {
                this.DockMain.LoadFromXml(this.m_DockConfigFile, new DeserializeDockContent(this.GetContent));
            }
            
            
           

            // Load Files passed by arguments...
            
            foreach (string s in args)
            {
                if (File.Exists(s))
                {
                    if (Path.GetExtension(s).ToLower().Equals(".pproj"))
                    {
                        this.OpenProject(s);
                    }
                    else
                    {
                        this.CreateEditor(s, Path.GetFileName(s), Common.GetFileIcon(s, false));
                        
                    }
                }
            }
            List<Editor> edl = new List<Editor>();
            try//TODO
            {
                foreach (Editor ed in DockMain.Documents)
                {
                    edl.Add(ed);
                }
                MyActiveEditor=edl[0];
            }
            catch
            {
            }
            //MessageBox.Show(edl[edl.Count-1].TabText);
            //edl[edl.Count - 1].Show(DockMain, DockState.Document);
           
            if (edl.Count>0)  edl[edl.Count - 1].Focus();
            
           /* foreach (Editor ed in DockMain.Documents)
            {
                ed.CloseTab();
            }*/
            
            m_AutoComplete.ScriptsPath = m_ScriptsPath;
            Application.Idle += new EventHandler(OnIdle);
            
        }
        protected void OnIdle(object sender, EventArgs e)
        {
            Application.Idle -= new EventHandler(OnIdle);
            initialized = true;
            if (m_ScriptsPath.Length == 0)
            {
                MessageBox.Show("Der Pfad zu den Scripten ist noch nicht gesetzt, bitte stellen Sie den Pfad zum Ordner '_work\\Data\\Scripts' in den Einstellungen ein und starten Sie anschließend Stampfer neu.", "Pfad zu den Scripten unbekannt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Options frm = new Options(this);
                frm.Show();
                

                for (int a = 0; a < frm.m_OptionPanels.Count; a++)
                {
                    Control ctrl = (Control)frm.m_OptionPanels[a];

                    if (ctrl.Name == "Gothic")
                    {
                        frm.splitContainer1.Panel2.Controls.Clear();
                        ctrl.Dock = DockStyle.Fill;
                        frm.splitContainer1.Panel2.Controls.Add(ctrl);
                        break;
                    }
                }
                frm.BtBrowseScripts_Click(null, new EventArgs());

            }

            //Easteregg!!!
            if (MousePosition.X < 10
                && MousePosition.Y < 10)
            {
                FighterGame fg = new FighterGame();
                fg.Show();
            }
            
            
        }
        public int ToolSort(FileInfo obj1, FileInfo obj2)
        {
            return String.Compare(obj1.Name, obj2.Name, true) ;
        }
        void LoadTools()
        {
            string fl = Path.GetDirectoryName(Application.ExecutablePath);
            fl+=@"\Tools";
            if (Directory.Exists(fl))
            {

                DirectoryInfo di = new DirectoryInfo(fl);
                List<FileInfo> finfo = new List<FileInfo>();
                finfo.AddRange(di.GetFiles("*.lnk", SearchOption.AllDirectories));
                finfo.AddRange(di.GetFiles("*.exe", SearchOption.AllDirectories));
                finfo.Sort(new Comparison<FileInfo> (ToolSort));
                foreach (FileInfo f in finfo)
                {
                    ToolStripMenuItem m = new ToolStripMenuItem(f.Name.Remove(f.Name.LastIndexOf(".")), Common.GetFileIcon(f.FullName, false).ToBitmap(), new EventHandler(SelectProgram),f.FullName);
                    toolFolderToolStripMenuItem.DropDownItems.Add(m);

                }
            }
            
        }

        void SelectProgram(object sender, EventArgs e)
        {
            ToolStripMenuItem s = (ToolStripMenuItem)sender;
            System.Diagnostics.Process n = new System.Diagnostics.Process();
            n.StartInfo.FileName = s.Name;            
            n.Start();
        }
        void MainForm_Deactivate(object sender, EventArgs e)
        {
            m_AutoComplete.AHide();
        }

        
        #region -= Load Config file =-

        /// <summary>
        /// Loads the Configuration File...
        /// </summary>
        /// <param name="reload">Reloaded or Not.</param>
        public void LoadConfigFile(bool reload)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(this.m_ConfigFile);

            // Get Editor Config...
            XmlNodeList nodes = xDoc.GetElementsByTagName("Editor");
            if (nodes.Count == 1)
            {
                #region -= Parse Editor Config =-
                foreach (XmlNode node in nodes[0].ChildNodes)
                {
                    switch (node.Name.ToLower())
                    {
                        case "showeol":
                            this.m_EditorConfig.ShowEOL = Convert.ToBoolean(node.InnerText);
                            break;
                        case "showinvalidlines":
                            this.m_EditorConfig.ShowInvalidLines = Convert.ToBoolean(node.InnerText);
                            break;
                        case "showspaces":
                            this.m_EditorConfig.ShowSpaces = Convert.ToBoolean(node.InnerText);
                            break;
                        case "showtabs":
                            this.m_EditorConfig.ShowTabs = Convert.ToBoolean(node.InnerText);
                            break;
                        case "showmatchbracket":
                            this.m_EditorConfig.ShowMatchingBracket = Convert.ToBoolean(node.InnerText);
                            break;
                        case "showlinenumbers":
                            this.m_EditorConfig.ShowLineNumbers = Convert.ToBoolean(node.InnerText);
                            break;
                        case "showhruler":
                            this.m_EditorConfig.ShowHRuler = Convert.ToBoolean(node.InnerText);
                            break;
                        case "showvruler":
                            this.m_EditorConfig.ShowVRuler = Convert.ToBoolean(node.InnerText);
                            break;
                        case "enablecodefolding":
                            this.m_EditorConfig.EnableCodeFolding = Convert.ToBoolean(node.InnerText);
                            break;
                        case "converttabs":
                            this.m_EditorConfig.ConvertTabs = Convert.ToBoolean(node.InnerText);
                            break;
                        case "useantialias":
                            this.m_EditorConfig.UseAntiAlias = Convert.ToBoolean(node.InnerText);
                            break;
                        case "allowcaretbeyondeol":
                            this.m_EditorConfig.AllowCaretBeyondEOL = Convert.ToBoolean(node.InnerText);
                            break;
                        case "highlightcurrentline":
                            this.m_EditorConfig.HighlightCurrentLine = Convert.ToBoolean(node.InnerText);
                            break;
                        case "autoinsertbracket":
                            this.m_EditorConfig.AutoInsertBracket = Convert.ToBoolean(node.InnerText);
                            break;
                        case "tabindent":
                            this.m_EditorConfig.TabIndent = Convert.ToInt32(node.InnerText);
                            break;
                        case "verticalrulercol":
                            this.m_EditorConfig.VerticalRulerCol = Convert.ToInt32(node.InnerText);
                            break;
                        case "indentstyle":
                            this.m_EditorConfig.IndentStyle = node.InnerText;
                            break;
                        case "bracketmatchingstyle":
                            this.m_EditorConfig.BracketMatchingStyle = node.InnerText;
                            break;
                        case "font":
                            string[] font = node.InnerText.Split(';');
                            Font f = new Font(font[0], Convert.ToSingle(font[1]));
                            this.m_EditorConfig.EditorFont = f;
                            break;
                        case "scripts":
                            this.m_ScriptsPath = node.InnerText;
                            break;
                        case "bilder":
                            this.m_BilderPath = node.InnerText;
                            break;
                        case "parser":
                            this.m_ParserMessageBox = Convert.ToBoolean(node.InnerText);
                            break;
                        case "backup":
                            this.m_EditorConfig.Backup = Convert.ToBoolean(node.InnerText);
                            break;
                        case "autocomplete":
                            this.m_EditorConfig.AutoCompleteAuto = Convert.ToBoolean(node.InnerText);
                            break;
                        case "backupfolder":
                            this.m_BackupFolder = node.InnerText;
                            break;
                        case "backupeach":
                            this.m_BackupEach = Convert.ToInt32(node.InnerText);
                            break;
                        case "backupfolderonly":
                            this.m_BackupFolderOnly = Convert.ToBoolean(node.InnerText);
                            break;
                        case "autobrackets":
                            this.m_AutoBrackets = Convert.ToBoolean(node.InnerText);
                            break;
                    }
                }
                #endregion
            }

            // Get Application Config...
            nodes = xDoc.GetElementsByTagName("Application");
            if (nodes.Count == 1)
            {
                #region -= Parse Application Config =-
                foreach (XmlNode n in nodes[0].ChildNodes)
                {
                    switch (n.Name.ToLower())
                    {
                        case "top":
                            if (!reload)
                            {
                                this.Top = Convert.ToInt32(n.InnerText);
                            }
                            break;
                        case "left":
                            if (!reload)
                            {
                                this.Left = Convert.ToInt32(n.InnerText);

                                // Re-Position if off Screen...
                                int w = 0;
                                foreach (Screen screen in Screen.AllScreens)
                                {
                                    w += screen.Bounds.Width;
                                }
                                if (this.Left > w)
                                {
                                    this.Left = this.Left - w;
                                }
                            }
                            break;
                        case "width":
                            if (!reload)
                            {
                                this.Width = Convert.ToInt32(n.InnerText);
                            }
                            break;
                        case "height":
                            if (!reload)
                            {
                                this.Height = Convert.ToInt32(n.InnerText);
                            }
                            break;
                        case "saveonexit":
                            this.m_SaveonExit = Convert.ToBoolean(n.InnerText);
                            break;
                        case "recentfilecount":
                            this.m_RecentFileCount = Convert.ToInt32(n.InnerText);
                            break;
                        case "recentprojectcount":
                            this.m_RecentProjectCount = Convert.ToInt32(n.InnerText);
                            break;
                    }
                }
                #endregion
            }
            SetAutocompleteMenu();
            if (!reload)
            {
                nodes = xDoc.GetElementsByTagName("RecentProjects");
                if (nodes.Count == 1)
                {
                    #region -= Parse Recent Projects =-
                    foreach (XmlNode n in nodes[0].ChildNodes)
                    {
                        ToolStripMenuItem tsmi = new ToolStripMenuItem();
                        foreach (XmlNode nC in n.ChildNodes)
                        {
                            if (nC.Name.ToLower() == "name")
                            {
                                tsmi.Text = nC.InnerText;
                            }
                            if (nC.Name.ToLower() == "file")
                            {
                                tsmi.Name = nC.InnerText;
                            }
                        }

                        tsmi.Click += new EventHandler(ReopenProject);
                        this.mnuProjectReopen.DropDownItems.Add(tsmi);
                    }
                    #endregion
                }

                nodes = xDoc.GetElementsByTagName("RecentFiles");
                if (nodes.Count == 1)
                {
                    #region -= Parse Recent Files =-
                    foreach (XmlNode n in nodes[0].ChildNodes)
                    {
                        ToolStripMenuItem tsmi = new ToolStripMenuItem(n.InnerText);

                        tsmi.Click += new EventHandler(ReopenFile);
                        this.mnuFileOpenRecent.DropDownItems.Add(tsmi);
                    }
                    #endregion
                }
            }

            if (reload)
            {
                for (int a = 0; a < this.DockMain.Contents.Count; a++)
                {
                    if (this.DockMain.Contents[a].GetType() == typeof(Editor))
                    {
                        ((Editor)this.DockMain.Contents[a]).SetupEditor(this.m_EditorConfig);
                    }
                }

                foreach (IPeterPlugin plugin in this.m_Plugins)
                {
                    plugin.ApplyOptions();
                }
                
            }
            //Set Autosave
            if (m_BackupEach > 0)
            {
                tSaveTimer.Enabled = true;
                tSaveTimer.Interval = 60000 * m_BackupEach;
            }
            else
            {
                tSaveTimer.Enabled = false;
            }
        }
        void SetAutocompleteMenu()
        {
            if (m_EditorConfig.AutoCompleteAuto)
            {
                autocompletemenu.Text = autocompletemenuauto;
            }
            else
            {
                autocompletemenu.Text = autocompletemenumanu;
            }
        }

        #endregion

        #region -= Properties =-

        /// <summary>
        /// Gets the path the Application started in...
        /// </summary>
        public string ApplicationExeStartPath
        {
            get { return Path.GetDirectoryName(Application.ExecutablePath); }
        }

        /// <summary>
        /// Gets the Active Tab Interface...
        /// </summary>
        private IPeterPluginTab ActiveTab
        {

            get { return (IPeterPluginTab)this.m_ActiveContent; }
        }

        /// <summary>
        /// Gets the Active Editor...
        /// </summary>
        public Editor ActiveEditor
        {
            get
            {
                if (this.ActiveTab != null)
                {
                    if (this.m_ActiveContent.GetType() == typeof(Editor))
                    {
                        return (Editor)this.m_ActiveContent;
                    }
                }
               

                return null;
            }
        }

        /// <summary>
        /// Gets the Active Content...
        /// </summary>
        public IDockContent ActiveContent
        {
            get { return this.m_ActiveContent; }
        }

        /// <summary>
        /// Gets the Type for a Editor in string format (typeof(Editor))...
        /// </summary>
        public string EditorType
        {
            get { return typeof(Editor).ToString(); }
        }

        /// <summary>
        /// Gets the Location of the Application Config File...
        /// </summary>
        public string ConfigFile
        {
            get { return this.m_ConfigFile; }
        }

        /// <summary>
        /// Gets the location of the Dock Config File...
        /// </summary>
        public string DockConfigFile
        {
            get { return this.m_DockConfigFile; }
        }

        #endregion

        #region -= Add Dock Content =-

        /// <summary>
        /// Adds the given Dock Content to the form...
        /// </summary>
        /// <param name="content">Content to Add.</param>
        public void AddDockContent(DockContent content)
        {
            if (this.CheckContent(content))
            {
                content.Show(this.DockMain);
                content.TabPageContextMenuStrip = this.ctxTab;
                
            }
        }

     

        

        /// <summary>
        /// Adds the given Dock Content to the form...
        /// </summary>
        /// <param name="content">Content to Add.</param>
        /// <param name="addTo">Content to Add new Tab to.</param>
        public void AddDockContent(DockContent content, IDockContent addTo)
        {
            
            if (this.CheckContent(content))
            {
                content.Show(addTo.DockHandler.Pane, addTo);
                content.TabPageContextMenuStrip = this.ctxTab;                
            }
        }

        /// <summary>
        /// Adds the given Dock Content to the form...
        /// </summary>
        /// <param name="content">Content to Add.</param>
        /// <param name="state">State of Content</param>
        public void AddDockContent(DockContent content, DockState state)
        {
            
            if (this.CheckContent(content))
            {
               


                    content.Show(this.DockMain, state);
                    content.TabPageContextMenuStrip = this.ctxTab;
               
                
            }
        }

        /// <summary>
        /// Adds the given Dock Content to the form...
        /// </summary>
        /// <param name="content">Content to Add.</param>
        /// <param name="floatingRec">Floating Rectangle</param>
        public void AddDockContent(DockContent content, Rectangle floatingRec)
        {
            
            if (this.CheckContent(content))
            {
                content.Show(this.DockMain, floatingRec);
                content.TabPageContextMenuStrip = this.ctxTab;
               
            }
        }

        /// <summary>
        /// Checks the given content to see if it implements the IPeterPluginTab Interface...
        /// </summary>
        /// <param name="content">DockContent</param>
        /// <returns>True or False</returns>
        private bool CheckContent(DockContent content)
        {
            
            Type[] types = content.GetType().GetInterfaces();
            foreach (Type t in types)
            {
                if (t == typeof(IPeterPluginTab))
                {
                   
                    return true;
                }
            }

            MessageBox.Show("'" + content.GetType().ToString() + " kann nicht hinzugefügt werden.' Es implementiert nicht das IPeterPluginTab Interface",
                "Stampfer", MessageBoxButtons.OK, MessageBoxIcon.Error);

            return false;
        }

        

        #endregion

        #region -= Get Content =-

        /// <summary>
        /// Sets up the Content from last Session...
        /// </summary>
        /// <param name="contentString">Content String</param>
        /// <returns>IDockContent</returns>
        private IDockContent GetContent(string contentString)
        {
            
            // Find Results...
            if (contentString == typeof(FindResults).ToString())
            {
                this.m_FindControl.Results.TabPageContextMenuStrip = this.ctxTab;
                return this.m_FindControl.Results;
            }

            // Command Prompt...
            if (contentString == typeof(CommandPrompt).ToString())
            {
                CommandPrompt cmd = new CommandPrompt();
                cmd.Icon = Icon.FromHandle(((Bitmap)this.GetInternalImage("cmd")).GetHicon());
                cmd.TabPageContextMenuStrip = this.ctxTab;
                return cmd;
            }

            // File Difference...
            if (contentString == typeof(FileDifference).ToString())
            {
                FileDifference diff = this.GetNewFileDifference();
                diff.TabPageContextMenuStrip = this.ctxTab;
                return diff;
            }
            if (contentString == typeof(DialogCreator).ToString())
            {
                DialogCreator diff = new DialogCreator(this);
                diff.TabPageContextMenuStrip = this.ctxTab;
                return diff;
            }

            // Code Structure...
            if (contentString == typeof(ctrlCodeStructure).ToString())
            {
                return this.m_CodeStructure;
            }
            if (contentString == typeof(ctrlGothicInstances).ToString())
            {
                return this.m_GothicStructure;
            }
            if (contentString == typeof(ctrlQuestManager).ToString())
            {
                return this.m_QuestManager;
            }


            // Editor...
            string[] pSplit = contentString.Split('|');
            if (pSplit.Length == 5)
            {
                if (pSplit[0] == typeof(Editor).ToString())
                {
                    if (File.Exists(pSplit[2]))
                    {
                        Editor e = this.CreateNewEditor(pSplit[1]);
                        e.LoadFile(pSplit[2]);
                        this.UpdateRecentFileList(pSplit[2]);
                        // We Should'nt need to check for Duplicates...
                        e.Icon = Common.GetFileIcon(pSplit[2], false);
                        e.ScrollTo(Convert.ToInt32(pSplit[3]));
                        e.Project = pSplit[4];
                        return e;
                    }
                    return this.CreateNewEditor(pSplit[1]);
                }
            }

            if (pSplit.Length == 2)
            {
                // File Explorer
                if (pSplit[0] == typeof(ctrlFileExplorer).ToString())
                {
                    ctrlFileExplorer fe = new ctrlFileExplorer(this);
                    fe.Icon = Icon.FromHandle(((Bitmap)this.GetInternalImage("FEIcon")).GetHicon());
                    fe.LoadTree(pSplit[1]);
                    fe.TabPageContextMenuStrip = this.ctxTab;
                    return fe;
                }

                // Project Manager...
                if (pSplit[0] == typeof(ProjectManager).ToString())
                {
                    string[] projs = pSplit[1].Split(';');
                    foreach (string proj in projs)
                    {
                        this.OpenProject(proj);
                    }
                    return this.m_ProjMan;
                }
            }

            // Plugin...
            foreach (IPeterPlugin plugin in this.m_Plugins)
            {
                
                if (plugin.CheckContentString(contentString))
                {
                    DockContent dc = (DockContent)plugin.GetContent(contentString);
                    
                    dc.TabPageContextMenuStrip = this.ctxTab;
                    return dc;
                }
            }

            // If we return null, the program will crash, so just create an editor...
            this.m_NewCount++;
            return this.CreateNewEditor("Neu" + Convert.ToString(this.m_NewCount - 1));
        }

        

        #endregion

        #region -= Plugins =-

        /// <summary>
        /// Loads the Plugins in the Plugin Directory...
        /// </summary>
        private void LoadPlugins()
        {
            string[] files = Directory.GetFiles(Path.GetDirectoryName(Application.ExecutablePath) + "\\" + PLUGIN_FOLDER, "*.dll");
            foreach (string file in files)
            {
                this.LoadPlugin(file);
            }
        }

        /// <summary>
        /// Loads a Plugin...
        /// </summary>
        /// <param name="pluginPath">Full Path to Plugin</param>
        /// <returns>True if Plugin Loaded, otherwise false</returns>
        public bool LoadPlugin(string pluginPath)
        {
            Assembly asm;

            if (!File.Exists(pluginPath))
            {
                return false;
            }

            asm = Assembly.LoadFile(pluginPath);
            if (asm != null)
            {
                foreach (Type type in asm.GetTypes())
                {
                    if (type.IsAbstract)
                        continue;
                    object[] attrs = type.GetCustomAttributes(typeof(PeterPluginAttribute), true);
                    if (attrs.Length > 0)
                    {
                        IPeterPlugin plugin = Activator.CreateInstance(type) as IPeterPlugin;
                        plugin.Host = this;
                        if (plugin.HasMenu)
                        {
                            this.mnuPlugins.DropDownItems.Add(plugin.GetMenu());
                        }

                        if (plugin.HasTabMenu)
                        {
                            this.ctxTab.Items.Add(new ToolStripSeparator());
                            foreach (ToolStripMenuItem tsmi in plugin.GetTabMenu())
                            {
                                this.ctxTab.Items.Add(tsmi);
                            }
                        }

                        if (plugin.HasContextMenu)
                        {
                            this.ctxEditor.Items.Add(new ToolStripSeparator());
                            foreach (ToolStripMenuItem tsmi in plugin.GetContextMenu())
                            {
                                this.ctxEditor.Items.Add(tsmi);
                            }
                        }
                        this.m_Plugins.Add(plugin);
                        plugin.Start();
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region -= Active Content Changed =-

        /// <summary>
        /// Occurs when the Active Contents has Changed...
        /// </summary>
        /// <param name="sender">Content</param>
        /// <param name="e">Events</param>
        
        private void DockMain_ActiveContentChanged(object sender, EventArgs e)
        {

            
            this.mnuHighlighting.Enabled = false;
            this.bookMarksToolStripMenuItem.Enabled = false;
            this.mnuCode.Enabled = false;

            if (this.DockMain.ActiveContent != null)
            {
                // Set the Active content...
               
                
                this.m_ActiveContent = this.DockMain.ActiveContent;
                
                //MessageBox.Show(m_ActiveContent.DockHandler.TabText);
                if (this.DockMain.ActiveContent.GetType() == typeof(Editor))
                {
                    MyActiveEditor = (Editor)(DockMain.ActiveContent);
                    Editor edit = (Editor)this.DockMain.ActiveContent;
                    this.RemoveHighlightChecks();
                    this.mnuHighlighting.Enabled = true;
                    this.mnuCode.Enabled = true;
                    this.bookMarksToolStripMenuItem.Enabled = true;
                    for (int a = 0; a < this.mnuHighlighting.DropDown.Items.Count; a++)
                    {
                        ToolStripMenuItem tsmi = (ToolStripMenuItem)this.mnuHighlighting.DropDown.Items[a];
                        if (tsmi.Text == edit.Highlighting)
                        {
                            tsmi.Checked = true;
                            break;
                        }
                    }
                   

                    edit.UpdateCaretPos();
                   
                   // this.m_CodeStructure.ActiveContentChanged(this.DockMain.ActiveContent);
                    // this.m_CodeStructure.ActiveContentChanged(edit);
                }
                else
                {
                    if (this.DockMain.ActiveDocument == null || this.DockMain.ActiveDocument.GetType() != typeof(Editor))
                    {
                        this.UpdateCaretPos(0, 0, 0, null);
                        
                    }
                }

                foreach (IPeterPlugin plugin in this.m_Plugins)
                {
                    plugin.ActiveContentChanged(this.DockMain.ActiveContent);
                }

                this.UpdateToolBar();
            }
            else
            {
                this.UpdateCaretPos(0, 0, 0, null);
                
                
            }
            this.UpdateTitleBar();
            
        }

        public void UpdateTitleBar()
        {
            if (this.ActiveContent != null)
            {
                if (this.ActiveContent.GetType() == typeof(Editor))
                {
                    
                    this.Text = ((DockContent)this.ActiveContent).TabText + " - Stampfer";
                    if (this.m_CodeStructure.TEXT != this.Text && "*" + this.m_CodeStructure.TEXT != this.Text)
                    {
                        try
                        {
                            this.m_CodeStructure.TEXT = this.Text;
                            this.m_CodeStructure.Clear();
                            this.m_CodeStructure.ActiveContentChanged((Editor)this.ActiveContent,true);
                        }
                        catch
                        {
                        }
                    }
                   
                    
                }
                else if (this.DockMain.ActiveDocument == null || this.DockMain.ActiveDocument.GetType() != typeof(Editor))
                {
                    this.Text = "Stampfer";
                }
            }
            else
            {
                this.Text = "Stampfer";
                
            }
        }

        void DockMain_ActiveDocumentChanged(object sender, EventArgs e)
        {
            /*if (this.DockMain.ActiveDocument != null)
            {
                this.Text = ((DockContent)this.DockMain.ActiveDocument).TabText + " - Peter";
            }*/
           /* if (GetAsyncKeyState(0x04) < 0)
            {
                if (this.DockMain.ActiveDocument != null)
                {
                    ((DockContent)this.DockMain.ActiveDocument).Close();
                     
                }
            }*/
           
        }

        void DockMain_ContentRemoved(object sender, DockContentEventArgs e)
        {
            Type type = e.Content.GetType().GetInterface("IPeterPluginTab", true);//.GetInterfaces();
            if (type != null)
            {
                
                ((IPeterPluginTab)e.Content).CloseTab();
            }
            this.UpdateTitleBar();
        }

        #endregion

        #region -= Highlighting =-

        /// <summary>
        /// Loads the Highlighting Files...
        /// </summary>
        private void LoadHighlighting()
        {
            string path = Path.GetDirectoryName(Application.ExecutablePath) + "\\" + SCHEME_FOLDER;
            if (Directory.Exists(path))
            {
                HighlightingManager.Manager.AddSyntaxModeFileProvider(new FileSyntaxModeProvider(path));

                ICollection keys = HighlightingManager.Manager.HighlightingDefinitions.Keys;
                string[] keyArray = new string[keys.Count];
                keys.CopyTo(keyArray, 0);
                Array.Sort(keyArray);
                this.mnuHighlighting.DropDownItems.Clear();
                foreach (string key in keyArray)
                {
                    ToolStripMenuItem tsi = new ToolStripMenuItem(key);
                    tsi.Click += new EventHandler(Highlighter_Click);
                    this.mnuHighlighting.DropDown.Items.Add(tsi);
                }
            }
            else
            {
                MessageBox.Show("Highlighting Schemes können nicht geladen werden!", "Stampfer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Removes all the Check from the Highlighting Menu...
        /// </summary>
        private void RemoveHighlightChecks()
        {
            for (int a = 0; a < this.mnuHighlighting.DropDown.Items.Count; a++)
            {
                ToolStripMenuItem tsmi = (ToolStripMenuItem)this.mnuHighlighting.DropDown.Items[a];
                tsmi.Checked = false;
            }
        }

        /// <summary>
        /// Highlighting menu selection...
        /// </summary>
        /// <param name="sender">Highlighting Menu ToolStripMenuItem</param>
        /// <param name="e">Events</param>
        private void Highlighter_Click(object sender, EventArgs e)
        {
            this.RemoveHighlightChecks();
            if (this.ActiveContent != null)
            {
                if (this.ActiveContent.GetType() == typeof(Editor))
                {
                    ToolStripMenuItem tsmi = sender as ToolStripMenuItem;
                    tsmi.Checked = true;
                    Editor edit = (Editor)this.ActiveContent;
                    edit.Highlighting = tsmi.Text;
                }
            }
        }

        #endregion

        #region -= Drag N Drop =-

        /// <summary>
        /// Enables files to be dropped in the dock window...
        /// </summary>
        /// <param name="sender">DockPanel</param>
        /// <param name="e">Events</param>
        private void DockMain_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
            {
                e.Effect = DragDropEffects.All;
            }
        }

        /// <summary>
        /// Grabs the files dropped in the Dock Window...
        /// </summary>
        /// <param name="sender">DockPanel</param>
        /// <param name="e">Events</param>
        private void DockMain_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
            {
                this.CreateEditor(file, Path.GetFileName(file));
            }
        }

        #endregion

        #region -= New Document =-

        /// <summary>
        /// Creates a new blank editor...
        /// </summary>
        public void NewDocument()
        {
            this.m_NewCount++;
            Editor e = this.CreateNewEditor("Neu" + this.m_NewCount.ToString());
            e.Show(this.DockMain);
            
        }

        

       
       

        #endregion

        #region -= Open =-

        /// <summary>
        /// Displays the Open file Dialog to get files to edit...
        /// </summary>
        private void Open()
        {
            try { this.ofdMain.FileName = ((Editor)this.DockMain.ActiveContent).FileName; }
            catch { };
            if (this.ofdMain.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in this.ofdMain.FileNames)
                {
                    this.CreateEditor(file, Path.GetFileName(file));
                }
            }
        }

        #endregion

        #region -= Save =-

        /// <summary>
        /// Save the Current Pane...
        /// </summary>
        private void Save()
        {
            Editor current = null;
            int l = 0;
            IPeterPluginTab tab = this.ActiveTab;
            if (tab.FileName == null)
            {
                this.SaveAs(tab);
                this.UpdateTitleBar();
                return;
            }
            else
            {
                tab.Save();
                
            }
            if (this.m_CodeStructure != null) m_CodeStructure.treeMain.BeginUpdate();
            //::m_GothicStructure.BeginTreeUpdate();
            l = UpdateInstances(tab, ref current);
            if (current != null && this.m_CodeStructure != null)
            {
                this.m_CodeStructure.ActiveContentChanged(current, true);
                m_CodeStructure.treeMain.EndUpdate();
            }

            if ((this.m_GothicStructure != null))
            {

                //::m_GothicStructure.EndTreeUpdate();
                if (l > 0)
                    this.m_GothicStructure.SetAutoCompleteContent();

            }
           

            this.UpdateTitleBar();
        }

        /// <summary>
        /// Saves the Given Content As...
        /// </summary>
        /// <param name="tab">Content to Save</param>
        public void SaveAs(IPeterPluginTab tab)
        {
            Editor current = null;
            int l = 0;
            if (this.sfdMain.ShowDialog() == DialogResult.OK)
            {
                tab.SaveAs(this.sfdMain.FileName);
            }
            //::m_GothicStructure.BeginTreeUpdate();
            if (this.m_CodeStructure != null) m_CodeStructure.treeMain.BeginUpdate();
            l=UpdateInstances(tab, ref current);
            if (current != null && this.m_CodeStructure != null)
            {
                
                this.m_CodeStructure.ActiveContentChanged(current, true);
                m_CodeStructure.treeMain.EndUpdate();
            }

            if ((this.m_GothicStructure != null))
            {
                //m_GothicStructure.UpdateAllTrees(k);
                //::m_GothicStructure.EndTreeUpdate();
                if (l > 0)
                    this.m_GothicStructure.SetAutoCompleteContent();

            }
            
            this.UpdateTitleBar();
            
        }

        private int UpdateInstances(IPeterPluginTab tab, ref Editor current)
        {
            int k = 0;
            if (this.m_CodeStructure != null
                    && this.m_GothicStructure != null
                    && tab is Editor)
            {
                this.m_CodeStructure.Clear();

                if (this.MyActiveEditor.TabText == tab.TabText)
                //(((Editor)(this.ActiveContent)).TabText == ((Editor)(tab)).TabText))
                {

                    current = (Editor)tab;
                }
                if (tab.FileName == null) return 0;
                this.m_CodeStructure.ActiveContentChanged((Editor)tab, false);
                if (tab.FileName.Contains(FilePaths.ContentItems))
                {
                    foreach (Instance i in m_CodeStructure.lInstances)
                    {
                        k |= m_GothicStructure.AddItem(i.Name, tab.FileName);
                        //::m_GothicStructure.AddToItemTree(i.Name, tab.FileName, k);
                    }

                }
                else if (tab.FileName.Contains(FilePaths.ContentNPC))
                {
                    foreach (Instance i in m_CodeStructure.lInstances)
                    {
                        k |= m_GothicStructure.AddNPC(i.Name, tab.FileName);
                        //::m_GothicStructure.AddToNPCTree(i.Name, tab.FileName, k);
                    }
                }
                else if (tab.FileName.Contains(FilePaths.ContentDialoge))
                {
                    foreach (Instance i in m_CodeStructure.lInstances)
                    {
                        k |= m_GothicStructure.AddDia(i.Name, tab.FileName);
                        //::m_GothicStructure.AddToDialogTree(i.Name, tab.FileName, k);
                    }
                }

                foreach (Instance i in m_CodeStructure.lFuncs)//TODO
                {
                    k |= m_GothicStructure.AddFunc(i.Name, tab.FileName);
                    //::m_GothicStructure.AddToFuncTree(i.Name + "()", tab.FileName, k);
                }
                foreach (Instance i in m_CodeStructure.lVars)
                {
                    k |= m_GothicStructure.AddVar(i.Name, tab.FileName);
                    //::m_GothicStructure.AddToVarTree(i.Name, tab.FileName, k);
                }
                //MessageBox.Show(m_CodeStructure.lConsts.Count.ToString());
                foreach (Instance i in m_CodeStructure.lConsts)
                {
                    k |= m_GothicStructure.AddConst(i.Name, tab.FileName);
                    //::m_GothicStructure.AddToConstTree(i.Name, tab.FileName, k);
                }

                
            }
            return k;
        }
        /// <summary>
        /// Saves all of the Contents...
        /// </summary>
        private void SaveAll()
        {
            
            int l = 0;
            Editor current=null;
            //::if (this.m_GothicStructure != null) m_GothicStructure.BeginTreeUpdate();
            if (this.m_CodeStructure != null) m_CodeStructure.treeMain.BeginUpdate();

            for (int a = 0; a < this.DockMain.Contents.Count; a++)
            {
                IPeterPluginTab tab = (IPeterPluginTab)this.DockMain.Contents[a];
                
                if (tab.FileName == null)
                {
                    this.SaveAs(tab);
                }
                else
                {
                    tab.Save();
                }
                
                l |= UpdateInstances(tab, ref current);
                
                      
               /* if (m_GothicStructure != null
                    && tab is Editor)
                {
                    //MessageBox.Show(tab.FileName);
                    k |= m_GothicStructure.GetAll(tab.FileName);                    
                }*/
            }
            if (current != null)
            {
                this.m_CodeStructure.ActiveContentChanged(current, true);                
            }

            if (this.m_CodeStructure != null) m_CodeStructure.treeMain.EndUpdate();

            if ((this.m_GothicStructure != null))
            {
                //m_GothicStructure.UpdateAllTrees(k);
                //::m_GothicStructure.EndTreeUpdate();
                if (l>0)
                    this.m_GothicStructure.SetAutoCompleteContent();
                
            }
            //MessageBox.Show(k.ToString());
/*
            if (m_GothicStructure != null)
            {
                m_GothicStructure.UpdateAllTrees(k);
            }
            MessageBox.Show("!");*/
            /*if (this.m_CodeStructure !=null)
            {
                this.m_CodeStructure.Clear();
                this.m_CodeStructure.ActiveContentChanged((Editor)this.ActiveContent);
            }*/
            
            

        }

        #endregion

        #region -= Edit =-

        /// <summary>
        /// Clipboard Cut Action...
        /// </summary>
        private void Cut()
        {
            this.ActiveTab.Cut();
        }

        /// <summary>
        /// Clipboard Copy Action...
        /// </summary>
        private void Copy()
        {
            this.ActiveTab.Copy();
        }

        /// <summary>
        /// Clipboard Paste Action...
        /// </summary>
        private void Paste()
        {
            
            this.ActiveTab.Paste();
        }

        /// <summary>
        /// Clipboard Delete Action...
        /// </summary>
        private void Delete()
        {
            this.ActiveTab.Delete();
        }

        /// <summary>
        /// Select All Action...
        /// </summary>
        private void SelectAll()
        {
            this.ActiveTab.SelectAll();
        }

        /// <summary>
        /// Edit Undo Action...
        /// </summary>
        private void Undo()
        {
            this.ActiveTab.Undo();
        }

        /// <summary>
        /// Edit Redo Action...
        /// </summary>
        private void Redo()
        {
            this.ActiveTab.Redo();
        }

        #endregion

        #region -= Create Editor =-

        /// <summary>
        /// Creates a new Editor with the given file...
        /// </summary>
        /// <param name="fileName">File to load in Editor.</param>
        /// <param name="tabName">Name of Tab.</param>
        public void CreateEditor(string fileName, string tabName)
        {
            this.CreateEditor(fileName, tabName, Common.GetFileIcon(fileName, false));
        }

        /// <summary>
        /// Creates a new Editor with the given file...
        /// </summary>
        /// <param name="fileName">File to load in Editor.</param>
        /// <param name="tabName">Name of Tab.</param>
        /// <param name="image">Icon for Tab.</param>
        public void CreateEditor(string fileName, string tabName, Icon image)
        {
            this.CreateEditor(fileName, tabName, image, null);
        }

        /// <summary>
        /// Creates a new Editor with the given file...
        /// </summary>
        /// <param name="fileName">File to load in Editor.</param>
        /// <param name="tabName">Name of Tab.</param>
        /// <param name="image">Icon for Tab.</param>
        public void CreateEditor(string fileName, string tabName, Icon image, IDockContent addToContent)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    // Add to Recent Files...
                    this.UpdateRecentFileList(fileName);

                    // Let the plugins try to load the file first...
                    foreach (IPeterPlugin plugin in this.m_Plugins)
                    {
                        if (plugin.AbleToLoadFiles)
                        {
                            if (plugin.LoadFile(fileName))
                            {
                                return;
                            }
                        }
                    }

                    // No plugins want the file, we can load it...
                    if (!this.IsFileOpen(fileName))
                    {
                        Editor e = this.CreateNewEditor(tabName);
                        e.ShowIcon = true;
                        e.Icon = image;
                        e.LoadFile(fileName);
                        if (addToContent == null)
                        {
                            if (this.DockMain.ActiveDocumentPane != null)
                            {
                                e.Show(this.DockMain.ActiveDocumentPane, null);
                            }
                            else
                            {
                                e.Show(this.DockMain);
                            }
                           // MessageBox.Show(e.Text);
                           
                        }
                        else
                        {
                            e.Show(addToContent.DockHandler.Pane, null);
                            
                        }
                        
                        e.Activate();
                        
                       // this.DockMain.ActiveContent.DockHandler.Activate();
                        //MessageBox.Show(e.TabText);
                       
                        

                            
                            
                            
                    }
                    else
                    {
                        for (int a = this.DockMain.Contents.Count - 1; a >= 0; a--)
                        {
                            IPeterPluginTab tab = (IPeterPluginTab)this.DockMain.Contents[a];
                            if (tab.FileName == fileName)
                            {
                                this.DockMain.Contents[a].DockHandler.Show();
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show(fileName + " existiert nicht", "Stampfer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message, "Stampfer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public Editor GetEditor (string fileName)
        {
            for (int a = this.DockMain.Contents.Count - 1; a >= 0; a--)
            {
                IPeterPluginTab tab = (IPeterPluginTab)this.DockMain.Contents[a];
                if (tab.FileName == fileName)
                {
                    return (Editor)tab;
                }
            }

            return null;
        }

        /// <summary>
        /// Checks to see if a file is alread open...
        /// </summary>
        /// <param name="file">File to check.</param>
        /// <returns>True if open, else false</returns>
        private bool IsFileOpen(string file)
        {
            for (int a = 0; a < this.DockMain.Contents.Count; a++)
            {
                IPeterPluginTab tab = (IPeterPluginTab)this.DockMain.Contents[a];
                if (file.Equals(tab.FileName, StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        private void UpdateRecentFileList(string filePath)
        {
            // Add Menu Item if Needed...
            for (int a = 0; a < this.mnuFileOpenRecent.DropDownItems.Count; a++)
            {
                if (this.mnuFileOpenRecent.DropDownItems[a].Text == filePath)
                {
                    // File is already in list...
                    return;
                }
            }

            ToolStripMenuItem tsmi = new ToolStripMenuItem(filePath);
            tsmi.Click += new EventHandler(ReopenFile);
            this.mnuFileOpenRecent.DropDownItems.Add(tsmi);

            // Update Config file...
            bool inList = false;
            int count = 0;
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(this.m_ConfigFile);
            XmlNodeList recent = xDoc.GetElementsByTagName("RecentFiles");
            if (recent.Count == 1)
            {
                XmlNode recentNode = recent[0];
                foreach (XmlNode n in recentNode.ChildNodes)
                {
                    count++;
                    if (n.InnerText == this.ofdMain.FileName)
                    {
                        inList = true;
                        break;
                    }
                }

                if (!inList)
                {
                    if (count == this.m_RecentFileCount)
                    {
                        // Remove the First Node...
                        recentNode.RemoveChild(recentNode.ChildNodes[0]);
                    }

                    // Add the new file...
                    XmlNode newFileNode = xDoc.CreateNode(XmlNodeType.Element, "file", null);
                    newFileNode.InnerText = filePath;
                    recentNode.AppendChild(newFileNode);
                    xDoc.Save(this.m_ConfigFile);
                }
            }
        }

        #endregion

        #region -= Create New Editor =-

        /// <summary>
        /// Creates a new Editor with the given tab Name...
        /// </summary>
        /// <param name="tabName">Name to put on tab.</param>
        /// <returns>Newly created Editor.</returns>
        public Editor CreateNewEditor(string tabName)
        {
            Editor e = new Editor(tabName, this);
            e.Host = this;
            e.TabPageContextMenuStrip = this.ctxTab;
            e.SetContextMenuStrip(this.ctxEditor);
            e.SetupEditor(this.m_EditorConfig);
            

            return e;
        }

        #endregion

        #region -= Get File Icon =-

        /// <summary>
        /// Gets the Shell Icon for the given file...
        /// </summary>
        /// <param name="filePath">Path to File.</param>
        /// <param name="linkOverlay">Link Overlay or not.</param>
        /// <returns>Shell Icon for File.</returns>
        public Icon GetFileIcon(string filePath, bool linkOverlay)
        {
            return Common.GetFileIcon(filePath, linkOverlay);
        }

        #endregion

        #region -= Get Internal Image =-

        /// <summary>
        /// Gets an Image from the InternalImages resource file...
        /// </summary>
        /// <param name="imageName">Name of Image</param>
        /// <returns>Image</returns>
        public Image GetInternalImage(string imageName)
        {
            System.Resources.ResourceManager mngr = new System.Resources.ResourceManager("Peter.InternalImages", this.GetType().Assembly);
            return (Image)mngr.GetObject(imageName);
        }

        #endregion

        #region -= Trace =-

        /// <summary>
        /// Writes the given text in the status bar...
        /// </summary>
        /// <param name="text">Text to Write.</param>
        public void Trace(string text)
        {
            this.sslMain.Text = text;
        }

        #endregion

        #region -= Tool Bar =-

        /// <summary>
        /// Creates a new Blank Editor...
        /// </summary>
        /// <param name="sender">ToolStripButton</param>
        /// <param name="e">Events</param>
        private void tsbNew_Click(object sender, EventArgs e)
        {
            this.NewDocument();
        }

        /// <summary>
        /// Opens a Documnet...
        /// </summary>
        /// <param name="sender">ToolStripButton</param>
        /// <param name="e">Events</param>
        private void tsbOpen_Click(object sender, EventArgs e)
        {
            this.Open();
        }

        /// <summary>
        /// Save the Current Document...
        /// </summary>
        /// <param name="sender">ToolStripButton</param>
        /// <param name="e">Events</param>
        private void tsbSave_Click(object sender, EventArgs e)
        {
            
            this.Save();
        }

        /// <summary>
        /// Saves all of the Open Documents...
        /// </summary>
        /// <param name="sender">ToolStripButton</param>
        /// <param name="e">Events</param>
        private void tsbSaveAll_Click(object sender, EventArgs e)
        {
            this.SaveAll();
        }

        /// <summary>
        /// Clipboard Cut Action...
        /// </summary>
        /// <param name="sender">ToolStripButton</param>
        /// <param name="e">Events</param>
        private void tsbCut_Click(object sender, EventArgs e)
        {
            this.Cut();
        }

        /// <summary>
        /// Clipboard Copy Action...
        /// </summary>
        /// <param name="sender">ToolStripButton</param>
        /// <param name="e">Events</param>
        private void tsbCopy_Click(object sender, EventArgs e)
        {
            this.Copy();
        }

        /// <summary>
        /// Clipboard Paste Action...
        /// </summary>
        /// <param name="sender">ToolStripButton</param>
        /// <param name="e">Events</param>
        private void tsbPaste_Click(object sender, EventArgs e)
        {
            this.Paste();
        }

        /// <summary>
        /// Edit Undo Action...
        /// </summary>
        /// <param name="sender">ToolStripButton</param>
        /// <param name="e">Events</param>
        private void tsbUndo_Click(object sender, EventArgs e)
        {
            this.Undo();
        }

        /// <summary>
        /// Edit Redo Action...
        /// </summary>
        /// <param name="sender">ToolStripButton</param>
        /// <param name="e">Events</param>
        private void tsbRedo_Click(object sender, EventArgs e)
        {
            this.Redo();
        }

        /// <summary>
        /// Prints the Active Content...
        /// </summary>
        /// <param name="sender">ToolStripButton</param>
        /// <param name="e">Events</param>
        private void tsbPrint_Click(object sender, EventArgs e)
        {
            this.ActiveTab.Print();
        }

        #endregion

        #region -= File Menu =-

        /// <summary>
        /// Show the file Difference Dialog...
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">Events</param>
        private void mnuFileDifference_Click(object sender, EventArgs e)
        {

            this.AddDockContent(this.GetNewFileDifference(), DockState.Document);
        }

        /// <summary>
        /// Creates a new Difference Content...
        /// </summary>
        /// <returns>New Difference Content.</returns>
        private FileDifference GetNewFileDifference()
        {
            string fileList = "";
            string selectedFile = (this.ActiveTab != null) ? (!string.IsNullOrEmpty(this.ActiveTab.FileName)) ? this.ActiveTab.FileName : "" : "";
            for (int a = 0; a < this.DockMain.Contents.Count; a++)
            {
                IPeterPluginTab tab = (IPeterPluginTab)this.DockMain.Contents[a];
                if (!string.IsNullOrEmpty(tab.FileName))
                {
                    fileList += tab.FileName + ";";
                }
            }

            FileDifference diff = new FileDifference(fileList.Split(';'), selectedFile);
            diff.Icon = Icon.FromHandle(((Bitmap)this.GetInternalImage("Diff")).GetHicon());

            return diff;
        }

        /// <summary>
        /// Creates a new Blank Editor...
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">Events</param>
        private void mnuFileNew_Click(object sender, EventArgs e)
        {
            this.NewDocument();
        }

        /// <summary>
        /// Opens a Documnet...
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">Events</param>
        private void mnuFileOpen_Click(object sender, EventArgs e)
        {
            this.Open();
        }

        /// <summary>
        /// Save the Current Document...
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">Events</param>
        private void mnuFileSave_Click(object sender, EventArgs e)
        {
            this.Save();
        }

        /// <summary>
        /// Save the Current Document As...
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">Events</param>
        private void mnuFileSaveAs_Click(object sender, EventArgs e)
        {
            this.SaveAs((IPeterPluginTab)this.ActiveContent);
        }

        /// <summary>
        /// Saves all of the Open Documents...
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">Events</param>
        private void mnuFileSaveAll_Click(object sender, EventArgs e)
        {
            this.SaveAll();
        }

        /// <summary>
        /// Exits the Program
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">Events</param>
        private void mnuFileExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Reopens the Given File...
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">Events</param>
        void ReopenFile(object sender, EventArgs e)
        {
            
            ToolStripMenuItem tsmi = sender as ToolStripMenuItem;
            this.CreateEditor(tsmi.Text, Path.GetFileName(tsmi.Text));
        }

        #endregion

        #region -= Edit Menu =-

        /// <summary>
        /// Edit Undo Action...
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">Events</param>
        private void mnuEditUndo_Click(object sender, EventArgs e)
        {
            this.Undo();
        }

        /// <summary>
        /// Edit Redo Action...
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">Events</param>
        private void mnuEditRedo_Click(object sender, EventArgs e)
        {
            this.Redo();
        }

        /// <summary>
        /// Clipboard Cut Action...
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">Events</param>
        private void mnuEditCut_Click(object sender, EventArgs e)
        {
            this.Cut();
        }

        /// <summary>
        /// Clipboard Copy Action...
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">Events</param>
        private void mnuEditCopy_Click(object sender, EventArgs e)
        {
            this.Copy();
        }

        /// <summary>
        /// Clipboard Paste Action...
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">Events</param>
        private void mnuEditPaste_Click(object sender, EventArgs e)
        {
            this.Paste();
        }

        /// <summary>
        /// Duplicates the current selection...
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">Events</param>
        private void duplicateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.ActiveTab!=null)
                this.ActiveTab.Duplicate();
        }

        /// <summary>
        /// Clipboard Delete Action...
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">Events</param>
        private void mnuEditDelete_Click(object sender, EventArgs e)
        {
            this.Delete();
        }

        /// <summary>
        /// Selects All of the Text in the Current Document...
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">Events</param>
        private void mnuEditSelectAll_Click(object sender, EventArgs e)
        {
            this.SelectAll();
        }

        #endregion

        #region -= Editor Context Menu =-

        /// <summary>
        /// Edit Undo Action...
        /// </summary>
        /// <param name="sender">ToolStirpMenuItem</param>
        /// <param name="e">Events</param>
        private void ctxUndo_Click(object sender, EventArgs e)
        {
            this.Undo();
        }

        /// <summary>
        /// Edit Redo Action...
        /// </summary>
        /// <param name="sender">ToolStirpMenuItem</param>
        /// <param name="e">Events</param>
        private void ctxRedo_Click(object sender, EventArgs e)
        {
            this.Redo();
        }

        /// <summary>
        /// Clipboard Cut Action...
        /// </summary>
        /// <param name="sender">ToolStirpMenuItem</param>
        /// <param name="e">Events</param>
        private void ctxCut_Click(object sender, EventArgs e)
        {
            this.Cut();
        }

        /// <summary>
        /// Clipboard Copy Action...
        /// </summary>
        /// <param name="sender">ToolStirpMenuItem</param>
        /// <param name="e">Events</param>
        private void ctxCopy_Click(object sender, EventArgs e)
        {
            this.Copy();
        }

        /// <summary>
        /// Clipboard Paste Action...
        /// </summary>
        /// <param name="sender">ToolStirpMenuItem</param>
        /// <param name="e">Events</param>
        private void ctxPaste_Click(object sender, EventArgs e)
        {
            
            this.Paste();
        }

        /// <summary>
        /// Clipboard Delete Action...
        /// </summary>
        /// <param name="sender">ToolStirpMenuItem</param>
        /// <param name="e">Events</param>
        private void ctxDelete_Click(object sender, EventArgs e)
        {
            this.Delete();
        }

        /// <summary>
        /// Selects All Text in the Current Document...
        /// </summary>
        /// <param name="sender">ToolStirpMenuItem</param>
        /// <param name="e">Events</param>
        private void ctxSelectAll_Click(object sender, EventArgs e)
        {
            this.SelectAll();
        }

        /// <summary>
        /// Action before Editor Menu Opens...
        /// </summary>
        /// <param name="sender">Editor Context Menu</param>
        /// <param name="e">Events</param>
        private void ctxEditor_Opening(object sender, CancelEventArgs e)
        {
           /* this.ctxCut.Enabled = ((this.ActiveTab.Selection.Length > 0) && this.ActiveTab.AbleToCut);
            this.ctxCopy.Enabled = ((this.ActiveTab.Selection.Length > 0) && this.ActiveTab.AbleToCopy);
            this.ctxDelete.Enabled = ((this.ActiveTab.Selection.Length > 0) && this.ActiveTab.AbleToDelete);
            this.ctxPaste.Enabled = (Clipboard.ContainsText() && this.ActiveTab.AbleToPaste);
            this.ctxRedo.Enabled = this.ActiveTab.AbleToRedo;
            this.ctxUndo.Enabled = this.ActiveTab.AbleToUndo;
            this.ctxSelectAll.Enabled = this.ActiveTab.AbleToSelectAll;*/
        }

        /// <summary>
        /// Action before Edit Menu Opens...
        /// </summary>
        /// <param name="sender">Edit Menu</param>
        /// <param name="e">Events</param>
        private void mnuEdit_DropDownOpening(object sender, EventArgs e)
        {
            if (this.ActiveContent != null)
            {
                this.mnuEditCut.Enabled = ((this.ActiveTab.Selection.Length > 0) && this.ActiveTab.AbleToCut);
                this.mnuEditCopy.Enabled = ((this.ActiveTab.Selection.Length > 0) && this.ActiveTab.AbleToCopy);
                this.mnuEditDelete.Enabled = ((this.ActiveTab.Selection.Length > 0) && this.ActiveTab.AbleToDelete);
                this.mnuEditPaste.Enabled = this.ActiveTab.AbleToPaste;
                this.mnuEditRedo.Enabled = this.ActiveTab.AbleToRedo;
                this.mnuEditUndo.Enabled = this.ActiveTab.AbleToUndo;
                this.mnuEditSelectAll.Enabled = this.ActiveTab.AbleToSelectAll;
            }
            else
            {
                this.mnuEditCut.Enabled = this.mnuEditCopy.Enabled = this.mnuEditDelete.Enabled = false;
                this.mnuEditPaste.Enabled = false;
                this.mnuEditRedo.Enabled = false;
                this.mnuEditUndo.Enabled = false;
                this.mnuEditSelectAll.Enabled = false;
            }
        }

        /// <summary>
        /// Action before File Menu Opens...
        /// </summary>
        /// <param name="sender">File Menu</param>
        /// <param name="e">Events</param>
        private void fileToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            if (this.ActiveTab != null)
            {
                this.mnuFileSave.Enabled = this.mnuFileSaveAs.Enabled = this.ActiveTab.AbleToSave;
            }
            else
            {
                this.mnuFileSave.Enabled = this.mnuFileSaveAs.Enabled = false;
            }
        }

        #endregion

        #region -= Tab Context Menu =-

        /// <summary>
        /// Saves the Current Document...
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">Events</param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Save();
        }

        /// <summary>
        /// Closes the Current Document...
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">Events</param>
        private void ctsClose_Click(object sender, EventArgs e)
        {
            
            this.ActiveTab.CloseTab();
        }

        /// <summary>
        /// Closes all but the Current Document...
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">Events</param>
        private void ctxCloseAllBut_Click(object sender, EventArgs e)
        {
            for (int a = this.DockMain.Contents.Count - 1; a >= 0; a--)
            {
                if (this.DockMain.Contents[a].DockHandler.DockState == DockState.Document)
                {
                    IPeterPluginTab tab = (IPeterPluginTab)this.DockMain.Contents[a];
                    if (tab != this.ActiveTab)
                    {
                        tab.CloseTab();
                    }
                }
            }
        }

        /// <summary>
        /// Copys the path of the Current Document...
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">Events</param>
        private void copyPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.ActiveTab.FileName != null && this.ActiveTab.FileName != string.Empty)
            {
                Clipboard.SetText(this.ActiveTab.FileName);
            }
        }

        /// <summary>
        /// Opens the Folder containing the Current Document...
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">Events</param>
        private void openFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.ActiveTab.FileName != null && this.ActiveTab.FileName != string.Empty)
            {
                System.Diagnostics.Process.Start("explorer.exe",
                    Path.GetDirectoryName(this.ActiveTab.FileName));
            }
        }

        /// <summary>
        /// Event Before Tab Context Menu is opened
        /// </summary>
        /// <param name="sender">Tab Context Menu Strip</param>
        /// <param name="e">Events</param>
        private void ctxTab_Opening(object sender, CancelEventArgs e)
        {
            this.copyPathToolStripMenuItem.Enabled = 
                this.openFolderToolStripMenuItem.Enabled = (this.ActiveTab.FileName != null);
        }

        #endregion

        #region -= On Closing =-

        /// <summary>
        /// Intercepts the Closing Action to do some clean up items...
        /// </summary>
        /// <param name="e">Cancel Events</param>
        protected override void OnClosing(CancelEventArgs e)
        {
            
            for (int a = this.DockMain.Contents.Count - 1; a >= 0; a--)
            {
                IPeterPluginTab tab = (IPeterPluginTab)this.DockMain.Contents[a];
                if (tab.AbleToSave && tab.NeedsSaving)
                {
                    e.Cancel = !tab.CloseTab();
                }
            }

            if (!e.Cancel)
            {
               
                
                
                // Save Dock Layout...
                if (this.m_SaveonExit)
                {
                    this.DockMain.SaveAsXml(this.m_DockConfigFile);
                }
                else
                {
                    if (File.Exists(this.m_DockConfigFile))
                    {
                        File.Delete(this.m_DockConfigFile);
                    }
                }

                // Save Location...
                if (File.Exists(this.m_ConfigFile) && this.WindowState != FormWindowState.Minimized)
                {
                    // Load Config File...
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.Load(this.m_ConfigFile);
                    XmlNodeList nodes = xDoc.GetElementsByTagName("Application");
                    if (nodes.Count == 1)
                    {
                        foreach (XmlNode n in nodes[0].ChildNodes)
                        {
                            switch (n.Name.ToLower())
                            {
                                case "top":
                                    n.InnerText = this.Top.ToString();
                                    break;
                                case "left":
                                    n.InnerText = this.Left.ToString();
                                    break;
                                case "width":
                                    n.InnerText = this.Width.ToString();
                                    break;
                                case "height":
                                    n.InnerText = this.Height.ToString();
                                    break;
                                case "saveonexit":
                                    n.InnerText = this.m_SaveonExit.ToString();
                                    break;
                            }
                        }
                    }
                    xDoc.Save(this.m_ConfigFile);
                }

                foreach (IPeterPlugin plugin in this.m_Plugins)
                {
                    plugin.Close();
                }
            }
            m_AutoComplete.SaveKW();
            base.OnClosing(e);
            
        }

        #endregion

        #region -= Update Tool Bar =-

        /// <summary>
        /// Updates the Buttons on the tool bar...
        /// </summary>
        public void UpdateToolBar()
        {
            this.tsbSave.Enabled = this.ActiveTab.AbleToSave;
            this.tsbCut.Enabled = this.ActiveTab.AbleToCut;
            this.tsbCopy.Enabled = this.ActiveTab.AbleToCopy;
            this.tsbPaste.Enabled = this.ActiveTab.AbleToPaste;
        }

        #endregion

        #region -= Book Mark Menu =-

        /// <summary>
        /// Toggles a book mark on the active editor...
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">Events</param>
        private void mnuBookMarkToggle_Click(object sender, EventArgs e)
        {
            if (this.ActiveEditor != null)
            {
                this.ActiveEditor.ToggleMark();
            }
        }

        /// <summary>
        /// Removes all book marks on the active editor...
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">Events</param>
        private void mnuBookMarkRemoveAll_Click(object sender, EventArgs e)
        {
            if (this.ActiveEditor != null)
            {
                this.ActiveEditor.RemoveAllMarks();
            }
        }

        /// <summary>
        /// Goes to the next book mark on the active editor...
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">Events</param>
        private void mnuBookMarkNext_Click(object sender, EventArgs e)
        {
            if (this.ActiveEditor != null)
            {
                this.ActiveEditor.GotoMark(true);
            }
        }

        /// <summary>
        /// Goes to the Previous book mark on the active editor...
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">Events</param>
        private void mnuBookMarkPrevious_Click(object sender, EventArgs e)
        {
            if (this.ActiveEditor != null)
            {
                this.ActiveEditor.GotoMark(false);
            }
        }

        #endregion

        #region -= Help Menu =-

        /// <summary>
        /// Shows the About Form...
        /// </summary>
        /// <param name="sender">ToolStipMenuItem</param>
        /// <param name="e">Events</param>
        private void mnuHelpAbout_Click(object sender, EventArgs e)
        {
            //SplashScreen ss = new SplashScreen(true);
            //ss.ShowDialog();
            Info i = new Info();
            i.ShowDialog();
        }

        #endregion

        #region -= Find/Replace =-

        /// <summary>
        /// Activates the Find Dialog...
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">Events</param>
        private void mnuSearchFind_Click(object sender, EventArgs e)
        {
            // Set Dialog for Find...
            this.ShowFindDialog();
            this.m_FindControl.SetFind(false);
        }

        /// <summary>
        /// Finds the Next Occurance of the given Pattern
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">Events</param>
        private void mnuSearchFindNext_Click(object sender, EventArgs e)
        {
            this.FindNext(false);
        }

        private void findPreviousToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.FindNext(true);
        }

        /// <summary>
        /// Finds the Next Occurance of the given Pattern
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">Events</param>
        private void findInFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Set Dialog for Find...
            this.ShowFindDialog();
            this.m_FindControl.SetFind(true);
        }

        /// <summary>
        /// Finds the Next Occurance of the given Pattern
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">Events</param>
        private void mnuSearchReplace_Click(object sender, EventArgs e)
        {
            this.ShowFindDialog();
            this.m_FindControl.SetReplace(false);
        }

        /// <summary>
        /// Finds the Next Occurance of the given Pattern
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">Events</param>
        private void replaceNextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ReplaceNext();
        }

        /// <summary>
        /// Finds the Next Occurance of the given Pattern
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">Events</param>
        private void replaceInFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ShowFindDialog();
            this.m_FindControl.SetReplace(true);
        }

        /// <summary>
        /// Activates the Find Next Method...
        /// </summary>
        /// <param name="sender">ToolStripButton</param>
        /// <param name="e">Events</param>
        private void tsbFind_Click(object sender, EventArgs e)
        {
            //Easteregg!!!!
            if (!string.IsNullOrEmpty(this.txtFindNext.Text))
            {
                if (this.txtFindNext.Text == "Kakulukiam")
                {
                    Game gm = new Game();
                    gm.Show();
                    gm.BringToFront();
                    gm.Select();
                    return;
                }
                this.m_FindControl.SetFind(false);
                this.m_FindControl.FindText = this.txtFindNext.Text;
                this.FindNext(false);
            }
        }

        /// <summary>
        /// Check for enter being pressing in find box...
        /// </summary>
        /// <param name="sender">ToolStripTextBox</param>
        /// <param name="e">Events</param>
        void txtFindNext_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                tsbFind_Click(null, null);
            }
        }

        /// <summary>
        /// Makes the find Dialog Visible...
        /// </summary>
        private void ShowFindDialog()
        {
            // Grab the Selection...
            if(this.ActiveTab != null)
                if (this.ActiveTab.Selection.Length > 0)
                    this.m_FindControl.FindText = this.ActiveTab.Selection;

            if (!this.m_FindControl.Visible)
            {
                this.m_FindControl.Show(this);
            }
        }

        /// <summary>
        /// Finds the Next Occurance of the given Pattern in the Active Document...
        /// </summary>
        public bool FindNext(bool findUp)
        {
            
            if (this.ActiveContent != null)
            {
                
                if (this.ActiveContent.GetType()==typeof(DialogCreator))
                {
                   
                    DialogCreator DC=(DialogCreator)this.ActiveContent;
                    if (DC.Ed_Active==1)
                    {
                        return DC.EdCond.FindNext(this.m_FindControl.GetRegEx(), findUp);
                    }
                    else if (DC.Ed_Active == 2)
                    {
                        return DC.EdInfo.FindNext(this.m_FindControl.GetRegEx(), findUp);
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                      IPeterPluginTab tab = (IPeterPluginTab)this.ActiveContent;
                    return tab.FindNext(this.m_FindControl.GetRegEx(), findUp);
                }
              
            }
            return false;
        }
        public bool FindText(Regex r,bool findUp)
        {
            if (this.ActiveEditor != null)
            {
                
               // IPeterPluginTab tab = (IPeterPluginTab)this.ActiveContent;
                //return tab.FindNext(r, findUp);
                this.ActiveEditor.FindText(r);
              
                return true;
            }
            return false;
        }

        /// <summary>
        /// Finds the given Pattern in all of the Open Files...
        /// </summary>
        public void FindInOpenFiles()
        {
            for (int a = 0; a < this.DockMain.Contents.Count; a++)
            {
                IPeterPluginTab tab = (IPeterPluginTab)this.DockMain.Contents[a];
                if (!string.IsNullOrEmpty(tab.FileName))
                {
                    this.m_FindControl.FindInFile(new FileInfo(tab.FileName), this.m_FindControl.FindText, this.m_FindControl.GetRegEx());
                }
            }
        }

        /// <summary>
        /// Replaces the Next Occurance of the given Pattern in the Active Document...
        /// </summary>
        public void ReplaceNext()
        {
            if (this.ActiveContent != null)
            {
                if (this.ActiveContent.GetType() == typeof(DialogCreator))
                {

                    DialogCreator DC = (DialogCreator)this.ActiveContent;
                    if (DC.Ed_Active == 1)
                    {
                         DC.EdCond.ReplaceNext(this.m_FindControl.GetRegEx(), this.m_FindControl.ReplaceText, this.m_FindControl.FindUp);
                    }
                    else if (DC.Ed_Active == 2)
                    {
                         DC.EdInfo.ReplaceNext(this.m_FindControl.GetRegEx(), this.m_FindControl.ReplaceText, this.m_FindControl.FindUp);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    IPeterPluginTab tab = (IPeterPluginTab)this.ActiveContent;
                    tab.ReplaceNext(this.m_FindControl.GetRegEx(), this.m_FindControl.ReplaceText, this.m_FindControl.FindUp);
                }
          }
        }
        /// <summary>
        /// Ersetzt markiertes
        /// </summary>
        public void ReplaceAllMarked()
        {
            if (this.ActiveContent != null)
            {

                if (this.ActiveContent.GetType() == typeof(DialogCreator))
                {

                    DialogCreator DC = (DialogCreator)this.ActiveContent;
                    if (DC.Ed_Active == 1)
                    {
                        DC.EdCond.ReplaceAllMarked(this.m_FindControl.GetRegEx(), this.m_FindControl.ReplaceText);
                    }
                    else if (DC.Ed_Active == 2)
                    {
                        DC.EdInfo.ReplaceAllMarked(this.m_FindControl.GetRegEx(), this.m_FindControl.ReplaceText);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    this.ActiveEditor.ReplaceAllMarked(this.m_FindControl.GetRegEx(), this.m_FindControl.ReplaceText);
                }
                   
               
              
            }
        }
       
        /// <summary>
        /// Replaces all the Occurance of the given Pattern in the Active Document...
        /// </summary>
        public void ReplaceAll()
        {
            if (this.ActiveContent != null)
            {
                if (this.ActiveContent.GetType() == typeof(DialogCreator))
                {

                    DialogCreator DC = (DialogCreator)this.ActiveContent;
                    if (DC.Ed_Active == 1)
                    {
                        DC.EdCond.ReplaceAll(this.m_FindControl.GetRegEx(), this.m_FindControl.ReplaceText);
                    }
                    else if (DC.Ed_Active == 2)
                    {
                        DC.EdInfo.ReplaceAll(this.m_FindControl.GetRegEx(), this.m_FindControl.ReplaceText);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    IPeterPluginTab tab = (IPeterPluginTab)this.ActiveContent;
                    tab.ReplaceAll(this.m_FindControl.GetRegEx(), this.m_FindControl.ReplaceText);
                }
           }
        }

        /// <summary>
        /// Replaces all the Occurance of the given Pattern in all the Documents...
        /// </summary>
        public void ReplaceInOpenFiles()
        {
            for (int a = 0; a < this.DockMain.Contents.Count; a++)
            {
                IPeterPluginTab tab = (IPeterPluginTab)this.DockMain.Contents[a];
                tab.ReplaceAll(this.m_FindControl.GetRegEx(), this.m_FindControl.ReplaceText);
            }
        }

        /// <summary>
        /// Marks all occurances of the given Pattern in the active Document...
        /// </summary>
        public void MarkAll()
        {
            if (this.ActiveContent != null)
            {
                IPeterPluginTab tab = (IPeterPluginTab)this.ActiveContent;
                tab.MarkAll(this.m_FindControl.GetRegEx());
            }
        }

        #endregion

        #region -= Select Word =-

        /// <summary>
        /// Selects text at the offset with the give length...
        /// </summary>
        /// <param name="line">Line text is on.</param>
        /// <param name="offset">Offset Text is At.</param>
        /// <param name="wordLeng">Length of Text.</param>
        public void SelectWord(int line, int offset, int wordLeng)
        {
            this.ActiveTab.SelectWord(line, offset, wordLeng);
        }

        #endregion

        #region -= Project Menu =-

        /// <summary>
        /// Shows the project manager...
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">Events</param>
        private void mnuProjectShow_Click(object sender, EventArgs e)
        {
            if (this.m_ProjMan.DockState == DockState.Unknown)
            {
                this.m_ProjMan.Show(this.DockMain, DockState.DockLeft);
            }
            else
            {
                this.m_ProjMan.Show();
            }
        }

        /// <summary>
        /// Opens a project...
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">Events</param>
        private void mnuProjectOpen_Click(object sender, EventArgs e)
        {
            this.ofdMain.Multiselect = false;
            this.ofdMain.Filter = "Peter Project File (*.pproj)|*.pproj";
            if (this.ofdMain.ShowDialog() == DialogResult.OK)
            {
                OpenProject(this.ofdMain.FileName);
            }
            this.ofdMain.Multiselect = true;
            this.ofdMain.Filter = "";
        }

        /// <summary>
        /// Opens the given project...
        /// </summary>
        /// <param name="fileName">Path to project file.</param>
        public void OpenProject(string fileName)
        {
            // Is this a project file...
            if (Path.GetExtension(fileName).ToLower() == ".pproj")
            {
                this.mnuProjectShow_Click(null, null);
                // Load a Project...
                string proj = this.m_ProjMan.LoadFile(fileName);

                if (proj == null)
                {
                    // The project was already open...
                    return;
                }

                // Add Menu Item if Needed...
                if (!this.mnuProjectReopen.DropDownItems.ContainsKey(fileName))
                {
                    ToolStripMenuItem tsmi = new ToolStripMenuItem(proj);
                    tsmi.Click += new EventHandler(ReopenProject);
                    //tsmi.Tag = fileName;
                    tsmi.Name = fileName;

                    this.mnuProjectReopen.DropDownItems.Add(tsmi);
                }

                // Update Config file...
                bool inList = false;
                int count = 0;
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(this.m_ConfigFile);
                XmlNodeList recent = xDoc.GetElementsByTagName("RecentProjects");
                if (recent.Count == 1)
                {
                    XmlNode recentNode = recent[0];
                    foreach (XmlNode n in recentNode.ChildNodes)
                    {
                        count++;
                        foreach (XmlNode nC in n.ChildNodes)
                        {
                            if (nC.InnerText == fileName)
                            {
                                inList = true;
                                break;
                            }
                        }

                        if (inList) break;
                    }

                    if (!inList)
                    {
                        if (count == this.m_RecentProjectCount)
                        {
                            // Remove the First Node...
                            recentNode.RemoveChild(recentNode.ChildNodes[0]);
                        }

                        // Add the new project...
                        XmlNode newProjectNode = xDoc.CreateNode(XmlNodeType.Element, "project", null);
                        XmlNode newFileNode = xDoc.CreateNode(XmlNodeType.Element, "file", null);
                        newFileNode.InnerText = fileName;
                        XmlNode newNameNode = xDoc.CreateNode(XmlNodeType.Element, "name", null);
                        newNameNode.InnerText = proj;
                        newProjectNode.AppendChild(newNameNode);
                        newProjectNode.AppendChild(newFileNode);
                        recentNode.AppendChild(newProjectNode);
                        xDoc.Save(this.m_ConfigFile);
                    }
                }
            }
        }

        /// <summary>
        /// Reopens a project...
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">Events</param>
        void ReopenProject(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = sender as ToolStripMenuItem;
            this.mnuProjectShow_Click(null, null);
            // Load a Project...
            string proj = this.m_ProjMan.LoadFile(tsmi.Name);
        }

        /// <summary>
        /// Creates a new Project...
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">Events</param>
        private void mnuProjectNew_Click(object sender, EventArgs e)
        {
            Project prj = new Project();
            prj.ShowDialog();
            if (prj.ProjectFile != null)
            {
                this.OpenProject(prj.ProjectFile);
            }
            if (!prj.IsDisposed)
            {
                prj.Close();
            }
        }

        #endregion

        #region -= Options =-

        /// <summary>
        /// Shows the Options Dialog...
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">Events</param>
        private void mnuOptions_Click(object sender, EventArgs e)
        {
            Options frm = new Options(this);
            foreach (IPeterPlugin plugin in this.m_Plugins)
            {
                if (plugin.OptionPanel != null)
                {
                    frm.AddOptionPanel(plugin.OptionPanel, plugin.PluginImage);
                }
            }
            frm.ShowDialog();
        }

        #endregion

        #region -= Run Menu =-

        /// <summary>
        /// Starts a command Prompt...
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">Events</param>
        private void mnuRunCMD_Click(object sender, EventArgs e)
        {
            CommandPrompt cmd = new CommandPrompt();
            cmd.Icon = Icon.FromHandle(((Bitmap)this.GetInternalImage("cmd")).GetHicon());
            this.AddDockContent(new CommandPrompt(), DockState.DockBottom);
        }

        /// <summary>
        /// Runs a given command...
        /// </summary>
        /// <param name="command">Command to Run.</param>
        public void RunCommand(string command)
        {
            CommandPrompt cmd = this.GetCMD();
            cmd.RunCommand(command);
        }

        /// <summary>
        /// Runs the given Script in the given Directory...
        /// </summary>
        /// <param name="script">Script to Run (Commands are separated by new lines).</param>
        /// <param name="workingDir">Directory to run script.</param>
        public void RunScript(string script, string workingDir)
        {
            CommandPrompt cmd = this.GetCMD();
            cmd.RunScript(script, workingDir);
        }

        /// <summary>
        /// Finds an open Command Prompt, if none creates one.
        /// </summary>
        /// <returns>CommandPrompt</returns>
        public CommandPrompt GetCMD()
        {
            CommandPrompt cmd = null;
            foreach (IDockContent dc in this.DockMain.Contents)
            {
                if (dc.GetType() == typeof(CommandPrompt))
                {
                    cmd = (CommandPrompt)dc;
                    cmd.Show();
                    break;
                }
            }
            if (cmd == null)
            {
                this.mnuRunCMD_Click(null, null);
                cmd = (CommandPrompt)this.DockMain.Contents[this.DockMain.Contents.Count - 1];
            }

            return cmd;
        }

        #endregion

        #region -= Tools Menu =-

        /// <summary>
        /// Creates a new File Explorer...
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">Events</param>
        private void mnuToolsFileExplorer_Click(object sender, EventArgs e)
        {
            ctrlFileExplorer fe = new ctrlFileExplorer(this);
            fe.Icon = Icon.FromHandle(((Bitmap)this.GetInternalImage("FEIcon")).GetHicon());
            this.AddDockContent(fe, DockState.DockLeft);
        }

        /// <summary>
        /// Shows the Code Structure...
        /// </summary>
        /// <param name="sender">ToolStripMenuItem</param>
        /// <param name="e">Events</param>
        private void tsmiCodeStructure_Click (object sender, EventArgs e)
        {
          
           if (m_CodeStructure.VisibleState == DockState.Unknown)
            {
                ctrlCodeStructure m_CodeStructure2 = new ctrlCodeStructure(this);
                this.AddDockContent(m_CodeStructure2, DockState.DockRight);
                this.m_CodeStructure = m_CodeStructure2;
            }
            
        }

        #endregion

        #region -= Code Menu =-

        private void mnuCodeLineComment_Click(object sender, EventArgs e)
        {
            string text = this.ActiveEditor.GetText();
            text.Split('\n');
        }

        private void xMLToolStripMenuItem_Click (object sender, EventArgs e)
        {
            if (this.ActiveEditor != null)
            {
                //will hold formatted xml
                StringBuilder sb = new StringBuilder();
                //does the formatting
                XmlTextWriter xtw = null;
                try
                {
                    /*
                    & - &amp; 
                    < - &lt; 
                    > - &gt; 
                    " - &quot; 
                    ' - &#39; 
                    */
                    string xml = this.ActiveEditor.GetText();
                    xml = xml.Replace("&", "&amp;");

                    //load unformatted xml into a dom
                    XmlDocument xd = new XmlDocument();
                    xd.LoadXml(xml);

                    //pumps the formatted xml into the StringBuilder above
                    StringWriter sw = new StringWriter(sb);

                    //point the xtw at the StringWriter
                    xtw = new XmlTextWriter(sw);

                    //we want the output formatted
                    xtw.Formatting = Formatting.Indented;
                    xtw.Indentation = 4;

                    //get the dom to dump its contents into the xtw 
                    xd.WriteTo(xtw);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Fehler beim Parsen der XML.\n" + ex.Message, "Stampfer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    //clean up even if error
                    if (xtw != null)
                        xtw.Close();
                }

                //return the formatted xml
                if(!string.IsNullOrEmpty(sb.ToString()))
                    this.ActiveEditor.SetTextChanged(sb.ToString().Replace("&amp;", "&"));
            }
        }

        #endregion

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            IntPtr mainWindowHandle = this.Handle;
            try
            {
                lock (this)
                {
                    ISharedAssemblyInterface obj =
                            (ISharedAssemblyInterface)Activator.GetObject
                            (typeof(ISharedAssemblyInterface),
                             "ipc://IPChannelName/SreeniRemoteObj");
                    obj.SetHandle(mainWindowHandle);
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        protected override void WndProc(ref Message m)
        {
            
                switch (m.Msg)
                {

                    case 10001:

                        
                        //CopyDataStruct str = (CopyDataStruct)Marshal.PtrToStructure(m.LParam, typeof(CopyDataStruct));
                        //string[] files = str.lpData.Split('|');
                       

                        ISharedAssemblyInterface obj =
                           (ISharedAssemblyInterface)Activator.GetObject
                           (typeof(ISharedAssemblyInterface),
                            "ipc://IPChannelName/SreeniRemoteObj");
                        
                        //string[] files = obj.Read().Split('|');
                        //foreach (string file in files)
                        //{
                        string file = obj.Read();
                           
                            if (!string.IsNullOrEmpty(file))
                            {
                                if (File.Exists(file))
                                {
                                    this.CreateEditor(file, Path.GetFileName(file), Common.GetFileIcon(file, false));
                                }
                            }
                        //}
                        break;
                    
                }
            
            base.WndProc(ref m);
        }

        /// <summary>
        /// Updates the Status Bar's Caret Info...
        /// </summary>
        /// <param name="offset">Offset of Caret.</param>
        /// <param name="line">Line Carret is on.</param>
        /// <param name="col">Column Carret is at.</param>
        /// <param name="mode">Mode of Carret.</param>
        public void UpdateCaretPos(int offset, int line, int col, string mode)
        {
            if (string.IsNullOrEmpty(mode))
            {
                this.sslLine.Text = "";
                this.sslOther.Text = "";
                this.sslInsert.Text = "";
                this.sslColumn.Text = "";
            }
            else
            {
                this.sslLine.Text = "Zeile: " + line.ToString();
                this.sslOther.Text = mode;
                this.sslInsert.Text = "Zeichen: " + offset.ToString();
                this.sslColumn.Text = "Spalte: " + col.ToString();
            }
        }

        /// <summary>
        /// Looks up the given word in the given project...
        /// </summary>
        /// <param name="word">Word to look up.</param>
        /// <param name="project">Project to look in.</param>
        /// <returns>Any information if found.</returns>
        public string LookUpProject (string word, string project)
        {
            return this.m_ProjMan.LookUp(word, project);
        }

        private void DockMain_Paint(object sender, PaintEventArgs e)
        {
           
        }

        private void geheZuZeileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Grab the Selection...

            

            
                this.m_GotoControl = new GoToLine(this);
               
           
            this.m_GotoControl.Show(this);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            mnuCode.Visible = false;
            ss.Close();            
            this.WindowState = FormWindowState.Maximized;
        }

        

        private void gothicInstanzenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_GothicStructure.VisibleState == DockState.Unknown)
            {
                ctrlGothicInstances m_GothicStructure2 = new ctrlGothicInstances(this);
                
                this.AddDockContent(m_GothicStructure2, DockState.DockRight);
                this.m_GothicStructure = m_GothicStructure2;
            }
        }

        private void zeilenkommentarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.ActiveEditor!=null)
            {
                this.ActiveEditor.Enclose(@"//", "");
                this.ActiveEditor.Refresh();
            }
        }

        private void blockkommentarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.ActiveEditor != null)
            {
                this.ActiveEditor.Enclose(@"/*", @"*/");
                this.ActiveEditor.Refresh();
            }
        }

        private void kleinesKommentarfeldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.ActiveEditor != null)
            {
                this.ActiveEditor.Enclose("\r\n" + @"/*===================================================================" + "\r\n", "\r\n" + @"==================================================================*/"+"\r\n");
                this.ActiveEditor.Refresh();
            }
        }

        private void großesKommentarfeldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.ActiveEditor != null)
            {
                this.ActiveEditor.Enclose("\r\n"+"\r\n"+@"/*#################################################################" + "\r\n" + "###################################################################"+"\r\n", "\r\n" + "###################################################################"+"\r\n"+ @"#################################################################*/"+"\r\n"+"\r\n");
                this.ActiveEditor.Refresh();
            }
        }

        private void questManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_QuestManager.VisibleState == DockState.Unknown)
            {
                ctrlQuestManager m_QuestManager2 = new ctrlQuestManager(this);
                this.AddDockContent(m_QuestManager2, DockState.DockRight);
                this.m_QuestManager = m_QuestManager2;
            }
        }

        private void dialogAssistentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogCreator DC = new DialogCreator(this);
            this.AddDockContent(DC, DockState.Document);
        }

        private void BtShortFunc_Click(object sender, EventArgs e)
        {
            if (m_AutoComplete != null && ActiveEditor!=null)
            {

                ActiveEditor.m_Editor.ActiveTextAreaControl.Document.Replace(0, ActiveEditor.m_Editor.ActiveTextAreaControl.Document.TextContent.Length, m_AutoComplete.TransformShortFunc(ActiveEditor));

                ActiveEditor.UpdateFolding();
                //ActiveEditor.Refresh();
                
            }
        }

        private void kurzfunktionenUmwandelnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BtShortFunc_Click(null, new EventArgs());
        }

        private void tSaveTimer_Tick(object sender, EventArgs e)
        {
            if (m_BackupFolder != ""
                && Directory.Exists(m_BackupFolder))
            {
                Trace("Backup erstellt");
                string Current = m_BackupFolder + "\\Current\\" ;
                string filename = "";
                if (!Directory.Exists(Current))
                {
                    Directory.CreateDirectory(Current);
                }
                StreamWriter stream;
                Editor tab;
                for (int a = 0; a < this.DockMain.Contents.Count; a++)
                {
                    try
                    {
                        tab = (Editor)this.DockMain.Contents[a];
                    }
                    catch
                    {
                        continue;
                    }
                    
                    if (tab.FileName == null)
                    {
                        this.SaveAs(tab);
                    }
                    else if (Path.GetExtension(tab.FileName)!=".d")
                    {
                        continue;
                    }
                    else
                    {
                        filename = tab.FileName;
                        filename = Current + Path.GetFileName(filename);


                        try
                        {
                            stream = new StreamWriter(filename, false, Encoding.GetEncoding(1252));


                            stream.Write(tab.m_Editor.Document.TextContent);

                            stream.Close();
                        }
                        catch 
                        { }
                        //tab.Save();

                    }
                }
            }
        }

        private void mnuExpandAll_Click(object sender, EventArgs e)
        {
            if(ActiveEditor!=null)
            {
                ActiveEditor.FoldingExpand();                
            }
        }

        private void mnuCollapseAll_Click(object sender, EventArgs e)
        {
            if (ActiveEditor != null)
            {
                ActiveEditor.FoldingCollapse();
            }
        }

        private void autoEinrückenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveEditor != null)
            {
                ActiveEditor.Indent();
            }
        }

        private void regionenAusklappenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveEditor != null)
            {
                ActiveEditor.RegionsExpand();
            }
        }

        private void regionenEinklappenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveEditor != null)
            {
                ActiveEditor.RegionsCollapse();
            }
        }

        private void autocompletemenu_Click(object sender, EventArgs e)
        {
            if (autocompletemenu.Text == autocompletemenuauto)
            {
                m_EditorConfig.AutoCompleteAuto = false;
                autocompletemenu.Text = autocompletemenumanu;
                /*foreach (Editor ed in DockMain.Documents)
                {
                    ed.AutoCompleteAuto = false;
                }*/
                for (int x = 0; x < this.DockMain.Contents.Count; x++)
                {
                    if (this.DockMain.Contents[x] is Editor)
                    {
                        ((Editor)this.DockMain.Contents[x]).AutoCompleteAuto = false;                        
                    }
                    else if (this.DockMain.Contents[x] is DialogCreator)
                    {
                        ((DialogCreator)this.DockMain.Contents[x]).EdCond.AutoCompleteAuto = false;
                        ((DialogCreator)this.DockMain.Contents[x]).EdInfo.AutoCompleteAuto = false;
                    }
                }
            }
            else
            {
                m_EditorConfig.AutoCompleteAuto = true;
                autocompletemenu.Text = autocompletemenuauto;
                /*foreach (Editor ed in DockMain.Documents)
                {
                    ed.AutoCompleteAuto = true;
                }*/
                for (int x = 0; x < this.DockMain.Contents.Count; x++)
                {
                    if (this.DockMain.Contents[x] is Editor)
                    {
                        ((Editor)this.DockMain.Contents[x]).AutoCompleteAuto = true;
                    }
                    else if (this.DockMain.Contents[x] is DialogCreator)
                    {
                        ((DialogCreator)this.DockMain.Contents[x]).EdCond.AutoCompleteAuto = true;
                        ((DialogCreator)this.DockMain.Contents[x]).EdInfo.AutoCompleteAuto = true;
                    }
                }
            }
            

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
           
            //Speicherung der Instanzen
            initialized = false;
            if(m_GothicStructure!=null)
            {
                
                StreamWriter sw = new StreamWriter(this.m_ScriptsPath + FilePaths.ITEMS, false, Encoding.Default);
                foreach (Instance sl in m_GothicStructure.ItemList.Values)
                {
                    sw.WriteLine(sl.ToString() + "\r\n" + sl.File);
                   
                }
                sw.Close();
                sw = new StreamWriter(this.m_ScriptsPath + FilePaths.NPCS, false, Encoding.Default);
                foreach (Instance sl in m_GothicStructure.NPCList.Values)
                {
                    sw.WriteLine(sl.ToString() + "\r\n" + sl.File);
                }
                sw.Close();
                sw = new StreamWriter(this.m_ScriptsPath + FilePaths.DIALOGE, false, Encoding.Default);
                foreach (Instance sl in m_GothicStructure.DialogList.Values)
                {
                    sw.WriteLine(sl.ToString() + "\r\n" + sl.File);
                }
                sw.Close();
                sw = new StreamWriter(this.m_ScriptsPath + FilePaths.FUNC, false, Encoding.Default);
                foreach (Instance sl in m_GothicStructure.FuncList.Values)
                {
                    sw.WriteLine(sl.ToString() + "\r\n" + sl.File);
                }
                sw.Close();
                sw = new StreamWriter(this.m_ScriptsPath + FilePaths.VARS, false, Encoding.Default);
                foreach (Instance sl in m_GothicStructure.VarList.Values)
                {
                    sw.WriteLine(sl.ToString() + "\r\n" + sl.File);
                }
                sw.Close();
                sw = new StreamWriter(this.m_ScriptsPath + FilePaths.CONSTS, false, Encoding.Default);
                foreach (Instance sl in m_GothicStructure.ConstList.Values)
                {
                    sw.WriteLine(sl.ToString() + "\r\n" + sl.File);
                }
                sw.Close();
                sw.Dispose();


            }
            
        }

        private void druckenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ActiveTab.Print();
        }

        private void txtFindNext_Enter(object sender, EventArgs e)
        {
            this.Focus();
            ((ToolStripTextBox)sender).Focus();
        }

        

        

       

        

        

        
       

       
      

        
    }

    public struct OUInfo
    {
        public string Name;
        public string Text;
    }
    
}
