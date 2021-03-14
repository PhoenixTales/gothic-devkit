using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
namespace GVE
{
    public partial class FOptions : Form
    {
        FMain fmain;
        string FOPTIONSTITLE;
        string GBPATH;
        string CKBBACKUP;
        string CKBAUTOLOAD;
        string LBLANGUAGE;
        string BTOK;
        public FOptions(FMain f)
        {
            InitializeComponent();
            fmain = f;
            this.TPath.Text = f.path;
        }
       

        private void FOptions_Load(object sender, EventArgs e)
        {
            this.MaximumSize = this.MinimumSize = this.Size;
            CKBBackup.Checked = fmain.Backup;
            CKBAutoload.Checked = fmain.autoload;
            LoadLanguageSelection();
            LoadLanguage(fmain.language + ".lang");
        }
        private void LoadLanguageSelection()
        {
            DirectoryInfo di = new DirectoryInfo(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location));
            FileInfo[] fi = di.GetFiles("*.lang", SearchOption.TopDirectoryOnly);
            this.CbLanguage.Items.Clear();
            foreach (FileInfo f in fi)
            {
                this.CbLanguage.Items.Add(Path.GetFileNameWithoutExtension(f.Name));
            }
            if (fi.Length > 0)
            {
                if (fmain.language.Length == 0 || !CbLanguage.Items.Contains(fmain.language))
                {
                    CbLanguage.SelectedIndex = 0;
                }
                else
                {
                    CbLanguage.SelectedIndex = CbLanguage.Items.IndexOf(fmain.language);
                }

            }
        }
        private void BtOk_Click(object sender, EventArgs e)
        {
            if (this.TPath.Text == "" || !Directory.Exists(this.TPath.Text))
            {
                MessageBox.Show("Der angegebene Pfad existiert nicht.", "Fehler");
            }
            else
            {
                fmain.path=TPath.Text;
                fmain.LoadLanguage(fmain.language + ".lang");
                this.Close();
                this.Dispose();
            }
        }

        private void BtBrowse_Click(object sender, EventArgs e)
        {
            if (FBD.ShowDialog() == DialogResult.OK)
            {
                this.TPath.Text = FBD.SelectedPath;
            }
        }

        private void CKBBackup_CheckedChanged(object sender, EventArgs e)
        {
           
            fmain.Backup = CKBBackup.Checked;
        }

        private void CKBAutoload_CheckedChanged(object sender, EventArgs e)
        {
            fmain.autoload = CKBAutoload.Checked;
        }

        private void CbLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                fmain.language = CbLanguage.SelectedItem.ToString();
                
            }
            catch
            {
            }
            
        }
        public void LoadLanguage(string file)
        {
            if (!File.Exists(file))
            {

                return;
            }

            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(file);
                XmlNodeList nodes = xDoc.GetElementsByTagName("FOPTIONSTITLE");
                if (nodes.Count > 0)
                {
                    FOPTIONSTITLE = nodes[0].InnerText;
                }
                nodes = xDoc.GetElementsByTagName("GBPATH");
                if (nodes.Count > 0)
                {
                    GBPATH = nodes[0].InnerText;
                }
                nodes = xDoc.GetElementsByTagName("CKBBACKUP");
                if (nodes.Count > 0)
                {
                    CKBBACKUP = nodes[0].InnerText;
                }
                nodes = xDoc.GetElementsByTagName("CKBAUTOLOAD");
                if (nodes.Count > 0)
                {
                    CKBAUTOLOAD = nodes[0].InnerText;
                }
                nodes = xDoc.GetElementsByTagName("LBLANGUAGE");
                if (nodes.Count > 0)
                {
                    LBLANGUAGE = nodes[0].InnerText;
                }
                nodes = xDoc.GetElementsByTagName("BTOK");
                if (nodes.Count > 0)
                {
                    BTOK = nodes[0].InnerText;
                }
               
            }
            catch (Exception e)
            {
                throw e;
            }
            ApplyLanguage();
        }
        private void ApplyLanguage()
        {
            this.Text = FOPTIONSTITLE;
            this.GbPath.Text = GBPATH;
            this.CKBAutoload.Text = CKBAUTOLOAD;
            this.CKBBackup.Text = CKBBACKUP;
            this.LbLanguage.Text = LBLANGUAGE;
            this.BtOk.Text = BTOK;
           
        }
    }
}
