using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace GVE
{
    public partial class FSaveGames : Form
    {
        FMain mainF;
        public FSaveGames(FMain f)
        {
            mainF = f;
            InitializeComponent();
        }

        private void FSaveGames_Load(object sender, EventArgs e)
        {
            const string TITLE="Title=string:";
            const string WORLD="WorldName=string:";
            const string DAY="TimeDay=int:";
            const string HOUR="TimeHour=int:";
            const string MIN="TimeMin=int:";
            const string DATE="SaveDate=string:";
            const string SECONDS="PlayTimeSeconds=int:";
            TFiles.ImageList = images1;
            //DirectoryInfo di = new DirectoryInfo(mainF.path);
            string [] savedirs = Directory.GetDirectories(mainF.path, "saves*", SearchOption.TopDirectoryOnly);
            List<string> svdrs = new List<string>();
            svdrs.AddRange(savedirs);
            svdrs.Sort();
           // FileInfo[] fileinfossave = di.GetFiles("SAVEDAT.SAV", SearchOption.AllDirectories);
            //FileInfo[] fileinfosinfo= di.GetFiles("SAVEINFO.SAV", SearchOption.AllDirectories);
            List<SaveData> Saves = new List<SaveData>();
            string[] s;
            SaveData sv;
            int k;
            string temp;
            foreach (string str in svdrs)
            {
                
                //MessageBox.Show(str);
                Saves = new List<SaveData>();
                DirectoryInfo di = new DirectoryInfo(str);
                FileInfo[] fileinfosinfo = di.GetFiles("SAVEINFO.SAV", SearchOption.AllDirectories);
                TreeNode n = new TreeNode(str.Substring(str.LastIndexOf("\\")+1));
                n.ImageIndex = 0;
                n.SelectedImageIndex = 0;
                TFiles.Nodes.Add(n);
                Saves = new List<SaveData>();
                foreach (FileInfo fi in fileinfosinfo)
                {
                    temp = fi.FullName;
                    temp=temp.Remove(temp.LastIndexOf("\\"));
                    temp = temp.Substring(temp.LastIndexOf("\\")+1).ToLower();

                    if (temp == "current")
                    {
                        continue;
                    }
                    sv = new SaveData();
                    s = File.ReadAllLines(fi.FullName,Encoding.Default);//.GetEncoding(1251));                    
                    for (int i = 10; i < s.Length - 1; i++)
                    {
                        if ((k = s[i].IndexOf(TITLE)) > -1)
                        {
                            k += TITLE.Length;
                            sv.name = s[i].Substring(k);
                        }
                        else if ((k = s[i].IndexOf(WORLD)) > -1)
                        {
                            k += WORLD.Length;
                            sv.welt = s[i].Substring(k);
                        }
                        else if ((k = s[i].IndexOf(DAY)) > -1)
                        {
                            k += DAY.Length;
                            sv.zeit = s[i].Substring(k) + ". Tag ";
                        }
                        else if ((k = s[i].IndexOf(HOUR)) > -1)
                        {
                            k += HOUR.Length;
                            sv.zeit += s[i].Substring(k) + ":";
                        }
                        else if ((k = s[i].IndexOf(MIN)) > -1)
                        {
                            k += MIN.Length;
                            if (Convert.ToInt32(s[i].Substring(k)) < 10)
                            {
                                sv.zeit +="0"+ s[i].Substring(k);
                            }
                            else
                            {
                                sv.zeit += s[i].Substring(k);
                            }
                           
                        }
                        else if ((k = s[i].IndexOf(DATE)) > -1)
                        {
                            k += DATE.Length;
                            sv.datum = s[i].Substring(k).Replace("- ","")+":00";
                        }
                        else if ((k = s[i].IndexOf(SECONDS)) > -1)
                        {
                            k += SECONDS.Length;
                            sv.sekunden = s[i].Substring(k);
                        }

                    }
                    sv.file = fi.FullName.Remove(fi.FullName.LastIndexOf("\\")) + "\\SAVEDAT.SAV";
                    Saves.Add(sv);
                    
                }
                Saves.Sort();
                foreach (SaveData sav in Saves)
                {
                    TreeNode t = new TreeNode(sav.name);
                    t.ImageIndex = 1;
                    t.SelectedImageIndex = 1;
                    t.Tag = sav;
                    n.Nodes.Add(t);
                    //MessageBox.Show(sav.file);
                }


            }

        }

        private void TFiles_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SaveData sav;
            if (e.Node.Tag != null)
            {
                sav = (SaveData)e.Node.Tag;
                lbDatum.Text = sav.datum;
                lbSekunden.Text = sav.sekunden;
                lbWelt.Text = sav.welt;
                lbZeit.Text = sav.zeit;
            }
            else
            {
                lbDatum.Text = "";
                lbSekunden.Text = "";
                lbWelt.Text = "";
                lbZeit.Text = "";
            }
        }

        private void BtAbort_Click(object sender, EventArgs e)
        {
            Close();
           // Dispose();
        }

        private void BtOk_Click(object sender, EventArgs e)
        {
            if (TFiles.SelectedNode != null)
            {
                if (TFiles.SelectedNode.Tag != null)
                {
                   mainF.LoadSave(((SaveData)TFiles.SelectedNode.Tag).file);
                   BtAbort_Click(null, new EventArgs());
                }
            }
        }

        private void TFiles_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            BtOk_Click(null, new EventArgs());
        }
    }

    public class SaveData:IComparable
    {
        public string name;
        public string datum;
        public string welt;
        public string zeit;
        public string sekunden;
        public string file;

        public SaveData()
        {
        }
        int IComparable.CompareTo(object obj)
        {
            SaveData p = obj as SaveData;
            
           // return Convert.ToInt32(p.sekunden)-Convert.ToInt32(sekunden);
            //MessageBox.Show(p.datum);
           return Convert.ToDateTime(p.datum).CompareTo(Convert.ToDateTime(datum));
        }
    }
}
