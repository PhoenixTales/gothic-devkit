using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using System.Xml;
using System.Globalization;
using System.Threading;
namespace GVE
{
    public partial class FMain : Form
    {

        List<Var> Variablen = new List<Var>();  //Speichert gefundene Variablen(Name, Wert, Position)
        List<int> Matches = new List<int>();//Speichert beim suchen die Indizes der Matches


        int wait = -1;
       
        private string currentFileName = "";  //Aktuell geöffnete Datei
        public string OPTIONS_XML = "Options.xml"; //Speichert Pfad zum GothicOrdner
        //public string DEFAUL_BAK = "backup.txt"; //Speichert ob Backup angelegt werden soll
        private int currentCellContentSave = 0;  //speichert Cell-Wert (zum wiederherstellen bei falscher Eingabe)

        private bool dialogMode = true;  //Steuert die Extrawurst Dialoge
        private bool dialogBegin = false;
        private bool dialogEnd = false;

        public bool Backup = false;//soll vor dem Speichern ein Backup angelegt werden?
        public string path = "";  //Pfad zum GothicOrdner
        public bool autoload = true;
        public bool blank = false;
        public bool autoupdate = true;
        public string language = "default";

        string TITLE = "GVE - Gothic Variablen Editor";
        string SAVEGAMEMENU="&Savegame";
        string HELPMENU="&?";
        string OPTIONSMENU = "&Einstellungen";
        string BTSEARCH="Suchen";
        string BTPREV="<";
        string BTNEXT=">";
        string CKBAUSBLENDEN="Ausblenden";
        string CKBAUTOSEARCH = "Automatische Aktualisierung";
        string DATAVAR="Variable";
        string DATAVAL="Wert";
        string BTBACKUP="Backup anlegen";
        string BTRESTORE="Backup einspielen";
        string BTSAVE="Speichern";
        string LOADNEWESTMENU="&Aktuellstes laden";
        string LOADMENU="&Manuell laden";
        string LISTALLMENU="Alle &Savegames auflisten";
        string HELPM="&Hilfe";
        string INFO="&Info";

        public string GBPATH = "Pfad zum Gothic-Ordner";
        public string CKBBACKUP="Beim Speichern automatisch Backup anlegen";
        public string CKBAUTOLOAD="Automatisches Laden beim Start";
        public string LBLANGUAGE="Sprache";
        public string BTOK="Übernehmen";
        public string FOPTIONSTITLE="Einstellungen";

        string MBTINPUTERROR = "Eingabefehler";
        string MBINPUTERROR = "Ungülte Eingabe.\nDie Eingabe muss eine Zahl zwischen −2.147.483.648 und 2.147.483.647 sein.";
        string MBTFILEERROR = "Dateifehler";
        string MBFILENOTFOUND = "Datei nicht gefunden!";
        string MBFILENOTREAD = "Datei konnte nicht gelesen werden!";
        string MBFOLDERERROR = "Ungültiger Pfad! \nBitte ändern sie Den Pfad zum Gothic Ordner in den Einstellungen.";
        string MBNOTHINGFOUND = "Keine Savegames gefunden!\nVergewissern sie sich, dass der Pfad zum Gothic-Ordner führt.";
        string MBNOACCESS = "Kein Zugriff auf die Dateien!\nVergewissern sie sich, dass auf die Savegames zugegriffen werden kann.";
        string MBREGISTRY = "Es ist entweder keine Pfadangabe in der Konfigurationsdatei vorhanden \noder der angegebene Pfad existiert nicht.\nEs wird versucht den Pfad zum Gothic 2 Ordner über die Registry zu ermitteln.";
        string MBREGISTRYFOUND = "Es wurde ein Pfad erkannt, bitte überprüfen Sie,\nob der Pfad korrekt ist und ändern Sie ihn gegebenenfalls.";
        string MBREGISTRYNOTFOUND = "Es wurde kein Eintrag in der Registry gefunden.\nBitte geben sie den Pfad manuell an.";
        string MBWRONGINPUT = "Ungültige Eingabe.";
        string MBVARNOTFOUND = "Variable nicht gefunden.";
        string MBTNOTFOUND = "Nicht gefunden";
        string MBBACKUPCREATEERROR = "Backup kann nicht erzeugt werden.";
        string MBLOADBACKUP = "Soll das Backup eingespielt werden? Die Änderungen am aktuellen Savegame gehen dadurch verloren.";
        string MBBACKUPLOADERROR = "Backup kann nicht eingespielt werden.";
        string MBBACKUPNOTFOUND = "Backupatei nicht gefunden. Möglicherweise wurde noch kein Backup dieses Savegames erstellt.";
        string MBSAVEERROR = "Speichern fehlgeschlagen. Möglicherweise fehlt die Berechtigung.";
        string MBHELPNOTFOUND ="Hilfedatei nicht gefunden!";

        int Akt_Match = 0;  //Aktuelle Position beim Durchblättern der Suchergebnisse
        public FMain()
        {
            InitializeComponent();
            this.Data.CellEndEdit += new DataGridViewCellEventHandler(Data_CellEndEdit);
            this.Data.CellBeginEdit += new DataGridViewCellCancelEventHandler(Data_CellBeginEdit);
            FMain_Resize(null, new EventArgs());
            ReadOptions();//versucht an den Pfad zum GothicOrdner dranzukommen
            //Backup = ReadBackup();//Guckt, ob automatische Backups gemacht werden sollen.


            if (path.Trim().Length== 0 || (!Directory.Exists(path)))//Gibts den Pfad und ist er nicht leer, bzw wurde überhaupt einer gefunden?
            {

                GetPathByUser();//Pfad über Usereingabe
            }
            if (autoload)
            {
                LoadSave(GetNewestFile());//Lädt automatisch das neueste Savegame.
            }

           
        }

        void Data_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            currentCellContentSave = Convert.ToInt32(Data.Rows[e.RowIndex].Cells[1].Value);//Sicherheitskopie des Cell-Wertes (Wiederherstellung bei falscher Eingabe)
        }

        void Data_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int content;
            int index;
            index = e.RowIndex;
          
            try
            {
                content = Convert.ToInt32(Data.Rows[index].Cells[1].Value);//Konvertierungsversuch (daher in trys)
                Data.Rows[index].Cells[1].Value = content;

                ((Var)Data.Rows[index].Cells[0].Value).value = content; //Variablendaten aktualisieren
                ((Var)Data.Rows[index].Cells[0].Value).changed = true; // als geändert markieren (damit beim Speichern nicht zu viel Herumgeschrieben wird und möglicherweise dabei etwas schiefgeht)

               
            }
            catch//falsche Eingabe
            {
                MessageBox.Show(MBINPUTERROR, MBTINPUTERROR);
                Data.Rows[index].Cells[1].Value = currentCellContentSave; //Wiederherstellung
            }
        }
       
        private void einstellungenToolStripMenuItem_Click(object sender, EventArgs e)
        {

            bool offen = false;
            foreach (Form f in Application.OpenForms)
            {
                if (f is FOptions)
                {
                    offen = true;
                    break;
                }
            }
            if (!offen)
            {
                FOptions FOptions = new FOptions(this);
                FOptions.Show(this);
            }
                
            
        }

       
        public void Reset()
        {
           
            Matches.Clear(); //Löscht die Suchergebnisse
           
        }
        public void LoadSave(string p)
        {
            if (p.Length == 0) return;
            if (!ReadSavegame(p))
            {
                return;
            }//List das Savegame
            SetupData();//Erzeugt Tabelle
            
        }
        bool tagebuchmode = false;

        public int JumpNr(ref byte[] b,ref int z, ref int k)//Liest die Werte aus
        {
            int d2 = 0;
            int r = 0;            
            while (b[z]==0x12) //die 12er sind ein guter Anker
            {
                /*if (b[z + 1] == 0x03)
                {
                    tagebuchmode = true;
                }*/
                //MessageBox.Show(z.ToString());
                z +=5;   //die Nummerierung interessiert uns nicht, also überspringen
                if (b[z] != 0x01)//die 1er interressieren uns auch nicht, da die den String einläuten
                {
                   /*if (b[z] == 0x02)
                    {
                        MessageBox.Show(dialogBegin.ToString());
                    } */
                    if (dialogBegin && b[z] == 0x02) //2er sind für Integer, da zuerst die dialoge gespeichert sind, wissen wir hier, dass nun nur noch Variablen kommen
                    {
                        tagebuchmode = true;
                        dialogMode = false;
                        dialogEnd = true;
                        dialogBegin = false;
                    }
                    z++;
                    k = z; //Position speichern, um später überschreiben zu können
                    if (dialogBegin == false && d2 < 2)
                    {
                        r = BitConverter.ToInt32(b, z);//auslesen des Integers
                        d2++;
                    }
                    z += 4;//Integer ist 4 Bytes lang
                    if (b[z - 5] != 2)
                    {
                     // MessageBox.Show(b[z - 5].ToString()+" " +z.ToString());
                    }
                    if (dialogMode && b[z-5] == 0x06) //Sollte es eine 6 als Typ sein, haben wir hier Dialoge, also beginnt hier das Dialogsegment
                    {
                        dialogBegin = true; 
                        return r;
                    }
                    //Wiese hier kein Return? Weil nur der letzte Block den Wert speichert (jaja, das Format der PBs ist sehr 'interessant')
                }
                 
                
            }
            return r;
        }
        public int ReadLength(ref byte[] b, ref int z)//Liest die Länge des Variablenstrings
        {
            int r = 0;
            z += 1;
            r = BitConverter.ToInt16(b, z);//ist ein short
            z += 2;
            return r;
        }
        
        public string ReadName(ref byte[] b, ref int z, int l)//liest den Namen der Variable aus
        {
            StringBuilder  r = new StringBuilder(2048); //ehem 65k:Ja, ist ja ein Short, daher mal maximal einen Short nehmen (sinnfrei so lange Variablennamen zu nehmen, aber sicher ist sicher)
            string s;
            //char[] t=new char[l-z+1];

           // string st = Convert.ToBase64String();
           //MessageBox.Show(st);
            //char[] k = (char[])b;
            int i;
            for ( i = z; i < z+l; i++)//alle Bytes duchgehen
            {
               // r.Append((char)b[i]);
                r.Append((char)(b[i]));
               
            }
            //MessageBox.Show(i.ToString());
            /*foreach (char c in t)
            {
                r.Append(c);
            }*/
          
            s = r.ToString();
            
            z += s.Length-1;//z aktualisieren
           // if (s.Trim().Length==0)
           //     return "";
            if (tagebuchmode && s==("[]"))
            {
                tagebuchmode = false;
                return "";
            }
            if (tagebuchmode)
            {
               // MessageBox.Show(s);
                return "";
            }
           /* if (!CheckString(ref s))//Nichtvariablen (Datenmüll) rausfiltern
            {
                //MessageBox.Show(s);       
                return "";
            }*/
            return s;
        }
        Regex reg = new Regex(@"[A-ZÄÖÜßåöü]|\d|_|\.");//Erlaubte Zeichen
        public bool CheckString(ref string s)
        {
            string s1;
            for (int i = 0; i < s.Length; i++)//Alle zeichen durchgehen und per Regex auf Gültigkeit überprüfen
            {
                s1=s[i].ToString();
                if (!reg.IsMatch(s[i].ToString()))
                {
                    return false;
                }
                else if (s[i]=='å')
                {
                    s=s.Replace('å','Ä');
                }
                else if (s[i] == 'ö')
                {
                    s = s.Replace('ö', 'Ö');
                }
                else if (s[i] == 'ü')
                {
                    s = s.Replace('ü', 'Ü');
                }
            }
            return true;
        }
        public bool ReadSavegame(string p)//Liest das Savegame ein
        {
            Variablen.Clear(); //Variablen resetten
            if (p.Trim().Length==0||!File.Exists(p)) //Gucken, ob die Datei da ist (ich bin paranoid)
            {
                MessageBox.Show(MBFILENOTFOUND, MBTFILEERROR);
                return false;
            }
            string Oldname = currentFileName;
            this.Text = TITLE +" - "+ p.Replace(path,"");//Dateiname in den Titel
            currentFileName = p; //geöffnete Datei aktualisieren
            dialogMode = true;  
            dialogBegin = false;
            byte[] ByteArray;
            try
            {
                ByteArray = File.ReadAllBytes(p);//alle Bytes einlesen
            }
            catch
            {
                MessageBox.Show(MBFILENOTREAD, MBTFILEERROR);
                return false;
            }
            
            int strlength = 0;
            int position = 0;
            string varname = "";
            int value=0;
            bool Readmode = false;
            int i = 0;
            //Eintrittspunkt
            int Count0A = 0;
            int MaxByte = 0;
            while (i < ByteArray.Length)//Überspringt den Einleutungskram und sucht einen guten Einstiegspunkt
            {
              
                if (ByteArray[i]==0x0A)
                {
                    Count0A++;
                    if (Count0A > 6)//Die Anzahl der Absätze 0A sollte fest sein.
                    {
                        i++;
                        
                       // break;
                    }
                }
                if (ByteArray[i] == 0x02 
                && ByteArray[i+1] == 0x00
                && ByteArray[i+2] == 0x00
                && ByteArray[i+3] == 0x00
                && ByteArray[i+4] == 0x01
                && ByteArray[i+5] == 0x00
                && ByteArray[i+6] == 0x00
                && ByteArray[i+7] == 0x00)
                {
                    break;
                }
                i++;
            }

            i += 8;//02 00 00 00 01 00 00 00 Dann kommt die Maxanzahl;
            MaxByte = BitConverter.ToInt32(ByteArray, i);//Bis wohin gelesen werden soll (danach kommt unbrauchbares Zeug)
            i += 3;
            
            while (i < MaxByte)//wir gehen die Bytes durch
            {
                
                try
                {
                    
                    if (ByteArray[i] == 0x12)//12er sind ein guter Anker
                    {
                        
                        if (dialogMode)//Beim Dialogmode muss erst der Wert und dann der Variablenstring gelesen werden, Dialoge sind leider Extrawürste
                        {
                            value = JumpNr(ref ByteArray, ref i, ref position);//alles per Referenz
                            
                        }
                        else
                        {
                            
                            if (Readmode == true && varname.Trim().Length > 0)//readmode gibt an, ob schon ein string eingelesen worden ist, da bei normalen Variablen nach dem Variablenstring der Wert kommt.
                            {
                                
                                value = JumpNr(ref ByteArray, ref i, ref position);//Wert auslesen.

                                if (dialogEnd)//Wenn Das ende erreicht ist, soll keine Variable gespeichert werden, da der Letzte Dia dann einen falschen Wert bekommt
                                {
                                    dialogEnd = false;
                                }
                                else
                                {
                                    
                                    Variablen.Add(new Var(varname, value, position));//Variable erzeugen
                                }
                                i--; //eins zurück wegen unten dem whilebedingten i++
                                Readmode = false;//es soll ein neuer Variablenstring eingelesen werden, bis ein wert eingelesen wird
                                

                            }
                        }
                    }
                    else if (ByteArray[i]== 0x01)//Wenn wir stattdessen auf eine 1 stoßen, gehts um einen String
                    {
                        //MessageBox.Show(i.ToString());
                        //MessageBox.Show("!");
                        strlength=ReadLength(ref ByteArray, ref i);//Länge des Variablenstrings bestimmen
                        if (strlength >0)//Gültig?
                        {
                            varname = ReadName(ref ByteArray, ref i, strlength);//Name Lesen
                        }

                        
                      
                            
                            if (varname.Trim().Length > 0)//gültig?
                            {
                                if (dialogMode && dialogBegin)//Im dialogmodus kommt der Variablentring zum Schluss, daher wird hier die Variable erzeugt
                                {

                                    Variablen.Add(new Var(varname, value, position));
                                }
                                else
                                {
                                    Readmode = true;//Name wurde eingelesen, jetzt kann der Wert eingelesen werden
                                }
                                
                            }
                        
                    }
                    

                }
                catch
                {
                    break;
                    
                }
                i++;
            }
            Variablen.Sort();
            if (Variablen.Count == 0)
            {
                MessageBox.Show(MBFILENOTREAD, MBTFILEERROR);
                currentFileName = Oldname;
                this.Text = TITLE + " - " + currentFileName.Replace(path, "");
                return false;
            }
            return true;
        }
        
        public string GetNewestFile()//sucht nach der neuesten Datei.
        {
            if (!Directory.Exists(path))//existiert der PFad noch?
            {
                MessageBox.Show(MBFOLDERERROR, MBTFILEERROR);
                return "";
            }

            DirectoryInfo di = new DirectoryInfo(path);
            try
            {
                FileInfo[] fileinfos2 = di.GetFiles("SAVEDAT.SAV", SearchOption.AllDirectories);//Files suchen
               // List<FileInfo> fileinfos = new List<FileInfo>();
                FileInfo newest;
                string temp;
                if (fileinfos2.Length > 0)
                {
                    newest = new FileInfo(fileinfos2[0].FullName);
                    foreach (FileInfo fi in fileinfos2)
                    {
                        temp = fi.FullName;
                        temp = temp.Remove(temp.LastIndexOf("\\"));
                        temp = temp.Substring(temp.LastIndexOf("\\") + 1).ToLower();
                        if (temp != "current")
                        {
                           // fileinfos.Add(fi);
                            if (fi.LastWriteTime > newest.LastWriteTime)
                            {
                                newest = fi;
                            }
                        }
                    }
                    return newest.FullName;
                }
                else//nix gefunden
                {
                    MessageBox.Show(MBNOTHINGFOUND, MBTFILEERROR);
                    return "";
                }
               
                /*int ki = 0;
                while (ki < fileinfos.Count)
                {
                    temp = fileinfos[ki].FullName;
                    temp = temp.Remove(temp.LastIndexOf("\\"));
                    temp = temp.Substring(temp.LastIndexOf("\\") + 1).ToLower();

                    if (temp == "current")
                    {
                        fileinfos.Remove(fileinfos[ki]);
                        ki--;
                    }
                    ki++;
                }*/
               /* if (fileinfos.Count > 0)
                {
                    newest = new FileInfo(fileinfos[0].FullName);
                    for (int i = 0; i < fileinfos.Count; i++)//den neuesten durch Vergleich bestimmen
                    {
                        if (fileinfos[i].LastWriteTime > newest.LastWriteTime)
                        {
                            newest = fileinfos[i];
                        }
                    }
                    //MessageBox.Show("!");
                    return newest.FullName;
                }
                else//nix gefunden
                {
                    MessageBox.Show(MBNOTHINGFOUND,MBTFILEERROR);
                    return "";
                }*/
            }
            catch
            {
                MessageBox.Show(MBNOACCESS, MBTFILEERROR);
                return "";
            }
           
        }
        public void GetPathByUser()//User muss Pfad manuell angeben
        {
           
            MessageBox.Show(MBREGISTRY, MBTFILEERROR);
            path = GetRegistryKey();//Sucht per Registry
            if (path != "")
            {
                MessageBox.Show(MBREGISTRYFOUND, MBTFILEERROR);
            }
            else//Fehlschlag
            {
                MessageBox.Show(MBREGISTRYNOTFOUND, "");
            }
            FOptions FOptions = new FOptions(this);//User soll Pfad angeben
            FOptions.ShowDialog(this);
        }
        public string GetRegistryKey()
        {
            string s = "";
            if (SubKeyExist(@"SOFTWARE\JoWood\Gothic II\"))//Gibts den Schlüssel?
            {
                RegistryKey myKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\JoWood\Gothic II\");
                string firstApp = (string)myKey.GetValue("RunCmd");//Schlüsselwert auslesen und formatieren
                string Pfad = firstApp.Replace(@"System\Gothic2.exe", "");
                s = String.Copy(Pfad);
               

            }
            else
            {
            }
            return s;
        }
        public static bool SubKeyExist(string Subkey)
        {
           
            RegistryKey myKey = Registry.LocalMachine.OpenSubKey(Subkey);
            if (myKey == null)
                return false;
            else
                return true;
        }
       /* public string ReadPath()//Liest aus der beiligenden Datei den Pfad aus
        {
            string r = "";
           
            if (!File.Exists(DEFAUL_TXT))
            {
                return r;
            }
            using (StreamReader sr=new StreamReader(DEFAUL_TXT,Encoding.Default))
            {
                try
                {
                    r = sr.ReadLine().Trim();
                    
                    
                }
                catch
                {
                    r = "";
                }

            }
            return r;
        }*/
        public void ReadOptions()
        {
            if (!File.Exists(OPTIONS_XML))            
            {

                return;
            }
            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(OPTIONS_XML);
                XmlNodeList nodes = xDoc.GetElementsByTagName("path");
                if (nodes.Count > 0)
                {
                   path=nodes[0].InnerText;
                }
                nodes = xDoc.GetElementsByTagName("backup");
                if (nodes.Count > 0)
                {
                    Backup=Convert.ToBoolean(nodes[0].InnerText);
                }
                nodes = xDoc.GetElementsByTagName("autoload");
                if (nodes.Count > 0)
                {
                    autoload = Convert.ToBoolean(nodes[0].InnerText);
                }
                nodes = xDoc.GetElementsByTagName("blank");
                if (nodes.Count > 0)
                {
                    blank = Convert.ToBoolean(nodes[0].InnerText);
                }
                nodes = xDoc.GetElementsByTagName("autoupdate");
                if (nodes.Count > 0)
                {
                    autoupdate = Convert.ToBoolean(nodes[0].InnerText);
                }
                nodes = xDoc.GetElementsByTagName("language");
                if (nodes.Count > 0)
                {
                    language=nodes[0].InnerText;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            CKBAusblenden.Checked = blank;
            CKBAutoSearch.Checked = autoupdate;
            
           
            

        }
        
        public void SaveOptions()
        {




            try
            {



                //if file is not found, create a new xml file
                XmlTextWriter xmlWriter = new XmlTextWriter(OPTIONS_XML, System.Text.Encoding.UTF8);
                xmlWriter.Formatting = Formatting.Indented;
                xmlWriter.WriteProcessingInstruction("xml", "version='1.0' encoding='UTF-8'");
                xmlWriter.WriteStartElement("Root");
                //If WriteProcessingInstruction is used as above,
                //Do not use WriteEndElement() here
                //xmlWriter.WriteEndElement();
                //it will cause the &ltRoot></Root> to be &ltRoot />
                xmlWriter.WriteElementString("path", path);
                xmlWriter.WriteElementString("backup", Backup.ToString());
                xmlWriter.WriteElementString("autoload", autoload.ToString());
                xmlWriter.WriteElementString("blank", blank.ToString());
                xmlWriter.WriteElementString("autoupdate", autoupdate.ToString());
                xmlWriter.WriteElementString("language", language);
                xmlWriter.Close();

            }
            catch(Exception e)
            {
                throw e;
            }

            

                
           
            

        }
        /*List<Control> Cntrls = new List<Control>();
        List<ToolStripItem> tms = new List<ToolStripItem>();

        public void CreateLang()
        {
            Cntrls.Clear();
            //if file is not found, create a new xml file
            XmlTextWriter xmlWriter = new XmlTextWriter("lang.xml", System.Text.Encoding.UTF8);
            xmlWriter.Formatting = Formatting.Indented;
            xmlWriter.WriteProcessingInstruction("xml", "version='1.0' encoding='UTF-8'");
            xmlWriter.WriteStartElement("Root");
           
            
            foreach (Control c in Cntrls)
            {
                MessageBox.Show(c.Name);
            }
            foreach (ToolStripItem c in tms)
            {
                MessageBox.Show(c.Name);
            }
            //If WriteProcessingInstruction is used as above,
            //Do not use WriteEndElement() here
            //xmlWriter.WriteEndElement();
            //it will cause the &ltRoot></Root> to be &ltRoot />
            xmlWriter.WriteElementString("path", path);
           
            xmlWriter.Close();
        }*/
       /* public void GetControls(Control c)
        {
            /*foreach (MenuStrip ms in c.Controls)
            {
                
                foreach (ToolStripItem t in ms.Items)
                {
                    tms.Add(t);
                }
            }
            foreach (object ct in c.Controls)
            {
               if (ct is MenuStrip)
                {
                    
                        foreach (ToolStripItem t in ((MenuStrip)ct).Items)
                        {
                            tms.Add(t);
                        }
                    
                }
               else
                   if (ct is Control)
                   {
                       if (c.Name.Length > 0)
                       {
                           Cntrls.Add((Control)ct);
                       }
                       GetControls((Control)ct);
                   }
            }
        }*/
        /*public bool ReadBackup()//Liest aus der beiliegenden Datei aus, ob ein Backup vor dem Speichern angelegt werden soll
        {
           if (!File.Exists(DEFAUL_BAK))
           {
               return false;
           }
            using (StreamReader sr = new StreamReader(DEFAUL_BAK, Encoding.Default))
            {
                try
                {
                    return Convert.ToBoolean(sr.ReadLine());
                }
                catch
                {
                   
                }
            }
            return false; 
        }*/
        public void SetColumnWidth()//Setzt automatisch das Spaltenverhältnis
        {
            this.Data.Columns[0].Width = (this.Data.Width / 5) * 4;

            this.Data.Columns[1].Width = this.Data.Width - this.Data.Columns[0].Width - SystemInformation.VerticalScrollBarWidth - 1;
         
        }
        public void SetupData()//Erzeugt Tabelle
        {
            Data.SuspendLayout();
            this.Data.RowHeadersVisible = false;//sieht doof aus
           
            this.Data.Columns[0].ReadOnly = true;//User soll die Namen nicht editieren können
            SetColumnWidth();
            
            this.Data.Rows.Clear();//Löscht falls vorhanden die Zeilen
            this.Data.Rows.Add(Variablen.Count-1);//legt Zeilen an
            
            for (int i = 0; i < Variablen.Count-1; i++) //Zellenbefüllung aus der Variablenliste
            {
               // for (int l = 0; l < this.Data.Columns.Count ; l++)
                //{
                 //   if (l == 0)
                 //   {
                        this.Data.Rows[i].Cells[0].Value = Variablen[i];
                       
                 //   }
                  //  else
                  //  {
                        this.Data.Rows[i].Cells[1].Value =  Variablen[i].value;
                       
                  //  }
               // }
            }
            foreach (DataGridViewColumn col in Data.Columns)
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            //Data.Sort(Variable, ListSortDirection.Ascending);
            this.Data.ResumeLayout();
            //MessageBox.Show(Variablen.Count.ToString());

        }
        Regex regx;
        private void Search(string s, bool button)
        {
            string searchstring = "";
            bool last = false;
            

            if (button)
            {
                if (CKBAusblenden.Checked)
                {
                    Einblenden();//falls ausgeblendet worden ist.
                }
            }
                Matches.Clear();//Match reset
                searchstring = s.Trim();//.Replace("*", ".*");//User soll gewohnte Sternchen benutzen können
                try
                {
                    regx = new Regex(searchstring, RegexOptions.IgnoreCase);
                }
                catch//ungültige Eingabe
                {
                    MessageBox.Show(MBWRONGINPUT, MBTINPUTERROR);
                    return;
                }

                for (int i = 0; i <= Data.Rows.Count - 1; i++)
                {

                    if (regx.IsMatch(Data.Rows[i].Cells[0].Value.ToString()))//guckt jede Variablenzelle durch
                    {

                        if (last == false)
                        {
                            Data.Rows[i].Cells[0].Selected = true;
                            last = true;

                        }
                        Matches.Add(i);
                    }
                }
                if (button)
                {
                    if (CKBAusblenden.Checked)
                    {
                        Ausblenden();//falls ausblenden aktiviert
                    }
                }
                Akt_Match = 0;
                if (Matches.Count == 0)
                {
                    MessageBox.Show(MBVARNOTFOUND, MBTNOTFOUND);
                }
            
        }
        private void BtSearch_Click(object sender, EventArgs e)
        {
            if (TSearch.Text.Length > 0)
            {
                Search(TSearch.Text, true);
            }
        }

        private void BtPrev_Click(object sender, EventArgs e)
        {
            
            if (Matches.Count > 0)
            {
                if (CKBAusblenden.Checked)
                {
                    int nr;
                    if (Data.SelectedCells.Count > 0)
                    {
                        nr = Data.SelectedCells[0].RowIndex;
                        if (nr > 0)
                        {
                            nr--;
                            this.Data.Rows[nr].Cells[0].Selected = true;
                        }
                        else
                        {
                            this.Data.Rows[Data.Rows.Count - 1].Cells[0].Selected = true;
                        }
                    }

                    return; 
                }
                if (Data.SelectedCells.Count > 0 && Data.SelectedCells[0].RowIndex != Matches[Akt_Match])
                {
                    int nr = -1;
                    for (int i = Data.SelectedCells[0].RowIndex; i > 0; i--)
                    {
                        if (Matches.Contains(i))
                        {
                            nr = i;
                            break;
                        }
                    }
                    if (nr > -1)
                    {
                        Akt_Match = Matches.IndexOf(nr);
                    }
                    else
                    {
                        Akt_Match = Matches.Count - 1;
                    }

                   
                }
                else
                {
                    if (Akt_Match > 0)
                    {
                        Akt_Match--;

                    }
                    else
                    {
                        Akt_Match = Matches.Count - 1;
                    }
                }
                this.Data.Rows[Matches[Akt_Match]].Cells[0].Selected = true;
            }
        }

        private void BtNext_Click(object sender, EventArgs e)
        {
          

            if (Matches.Count > 0)
            {
                if (CKBAusblenden.Checked)
                {
                    int nr;
                    if (Data.SelectedCells.Count > 0)
                    {
                        nr = Data.SelectedCells[0].RowIndex;
                        if (nr < Data.Rows.Count - 1)
                        {
                            nr++;
                            this.Data.Rows[nr].Cells[0].Selected = true;
                        }
                        else
                        {
                            this.Data.Rows[0].Cells[0].Selected = true;
                        }
                    }

                    return;
                }
                if (Data.SelectedCells.Count > 0 && Data.SelectedCells[0].RowIndex != Matches[Akt_Match])
                {
                    
                    int nr = -1;
                    for (int i = Data.SelectedCells[0].RowIndex; i < Data.Rows.Count; i++)
                    {
                        if (Matches.Contains(i))
                        {
                            nr = i;
                            break;
                        }
                    }
                    if (nr > -1)
                    {
                        Akt_Match = Matches.IndexOf(nr);
                    }
                    else
                    {
                        Akt_Match = 0;
                    }
                }
                else
                {

                    if (Akt_Match < Matches.Count - 1)
                    {
                        Akt_Match++;

                    }
                    else
                    {
                        Akt_Match = 0;
                    }
                }
                this.Data.Rows[Matches[Akt_Match]].Cells[0].Selected = true;
            }
          
        }
        
        private void CKBAusblenden_CheckedChanged(object sender, EventArgs e)
        {
            blank = CKBAusblenden.Checked;
            if (this.CKBAusblenden.Checked)//an
            {

                Ausblenden();
            }
            else
            {

                Einblenden();
                
            }
        }
        bool ausgeblendet=false;
        private void Einblenden()
        {
            int current = 0;
           
            if ((Matches.Count>0)&&(Data.SelectedCells.Count > 0))
            {

                if (ausgeblendet)
                {
                    current = Matches[Data.SelectedCells[0].RowIndex];
                }
                else
                {
                    current = Data.SelectedCells[0].RowIndex;
                }
                
            }
            Data.SuspendLayout();
            Data.Rows.Clear();
            Data.ResumeLayout();
            SetupData();
            //Data.Sort(Variable, ListSortDirection.Ascending);
            Data.Rows[current].Cells[0].Selected = true;
            Akt_Match = Matches.IndexOf(current);
            ausgeblendet = false;
        }
        private void Ausblenden()
        {
            int current = 0;
            if (Matches.Count > 0)
            {
                if (Data.SelectedCells.Count > 0)
                {

                    current = (Data.SelectedCells[0].RowIndex);
                    
                }
                Data.SuspendLayout();
                Data.Rows.Clear();
               
               
                    this.Data.Rows.Add(Matches.Count);
                
               
                for (int i = 0; i < Matches.Count; i++)
                {


                    for (int l = 0; l < this.Data.Columns.Count; l++)
                    {
                        if (l == 0)
                        {
                            this.Data.Rows[i].Cells[l].Value = Variablen[Matches[i]];

                        }
                        else
                        {
                            this.Data.Rows[i].Cells[l].Value = Variablen[Matches[i]].value;

                        }
                    }

                }
                
                int index = Matches.IndexOf(current);

                if (index >= 0)
                {
                    Data.Rows[index].Cells[0].Selected = true;
                    ausgeblendet = true;
                }
                Data.ResumeLayout();
            }

        }
        private void BtBackup_Click(object sender, EventArgs e)
        {
            
            MakeBackUp();
        }
        private void MakeBackUp()
        {
            if (currentFileName.Length > 0)
            {

                try
                {
                    File.Copy(currentFileName, currentFileName + ".bak", true);
                }
                catch
                {
                    MessageBox.Show(MBBACKUPCREATEERROR, MBTFILEERROR);
                }
            }
        }
        private void aktuellstesLadenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reset();
            LoadSave(GetNewestFile());
        }

        private void manuellLadenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OFDOpen.InitialDirectory = path;
            if (OFDOpen.ShowDialog() == DialogResult.OK)
            {
                Reset();
                LoadSave(OFDOpen.FileName);
            }
        }

        private void BtRestore_Click(object sender, EventArgs e)
        {
            if (currentFileName.Length == 0) return;
            if (currentFileName.Length > 0)
            {
                if (File.Exists(currentFileName + ".bak"))
                {
                    if (MessageBox.Show(MBLOADBACKUP, "?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        try
                        {
                            File.Copy(currentFileName + ".bak", currentFileName, true);
                            Reset();
                            LoadSave(currentFileName);
                        }
                        catch
                        {
                            MessageBox.Show(MBBACKUPLOADERROR, MBTFILEERROR);
                        }
                    }
                }
                else
                {
                    MessageBox.Show(MBBACKUPNOTFOUND, MBTFILEERROR);
                }
            }
        }

        private void BtSave_Click(object sender, EventArgs e)
        {
            if (currentFileName.Length == 0) return;
            if (Backup)
            {
                MakeBackUp();
            }
            try
            {
                FileStream fstr = new FileStream(currentFileName, FileMode.Open, FileAccess.Write);
                BinaryWriter w = new BinaryWriter(fstr);




                for (int i = 0; i < Variablen.Count - 1; i++)
                {
                    if (Variablen[i].changed)
                    {
                        // MessageBox.Show(i.ToString() + "  " + Variablen[i].pos.ToString() + "   " + Variablen[i].value);
                        w.Seek(Variablen[i].pos, 0);
                        w.Write(Variablen[i].value);
                    }
                }
                w.Close();
                fstr.Close();
            }
            catch
            {
                MessageBox.Show(MBSAVEERROR, MBTFILEERROR);
            }
        }

        private void TSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtSearch_Click(null, new EventArgs());
            }
        }

        private void FMain_Resize(object sender, EventArgs e)
        {
           
            PButtons.Top = this.ClientSize.Height - PButtons.Height - 3;
            this.Data.Width = this.ClientSize.Width - this.Data.Left - 16;
            SetColumnWidth();
            this.Data.Height = PButtons.Top - 3-Data.Top;
        }


        private void TSearch_TextChanged(object sender, EventArgs e)
        {
            wait = 4;
            
           
        }

        private void CKBAutoSearch_CheckedChanged(object sender, EventArgs e)
        {
            autoupdate = CKBAutoSearch.Checked;
            //Einblenden();
        }

        private void alleSavegamesAuflistenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FSaveGames saveform = new FSaveGames(this);
            saveform.Show();
        }

        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(TITLE + "\nvon Sumpfkrautjunkie\n\nCopyright ©  2009 Alexander Ruppert", TITLE);
        }

        private void hilfeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(language + ".chm"))
                {
                    Help.ShowHelp(this, language + ".chm");
                }
                else if (File.Exists("Default.chm"))
                {
                    Help.ShowHelp(this, "Default.chm");
                }
                else
                {
                    MessageBox.Show(MBHELPNOTFOUND, MBTFILEERROR);
                }
                
            }
            catch
            {
                MessageBox.Show(MBHELPNOTFOUND,MBTFILEERROR);
            }
        }

        private void FMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveOptions();
        }

        private void FMain_Load(object sender, EventArgs e)
        {
           // LoadLanguage("Default.xml");
            LoadLanguage(language + ".lang");
        }

        private void WaitTimer_Tick(object sender, EventArgs e)
        {
            if (wait > -1)
            {
                wait--;
            }
            if (wait ==0)
            {
                
                if (CKBAutoSearch.Checked)
                {


                    if (TSearch.Text.Length > 0)
                    {
                        Einblenden();
                        Search(TSearch.Text, false);
                        if (CKBAusblenden.Checked)
                        {
                            Ausblenden();
                        }
                    }
                    else
                    {
                        Einblenden();
                    }
                }

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
                XmlNodeList nodes = xDoc.GetElementsByTagName("TITLE");
                if (nodes.Count > 0)
                {
                    TITLE = nodes[0].InnerText;
                }
                nodes = xDoc.GetElementsByTagName("SAVEGAMEMENU");
                if (nodes.Count > 0)
                {
                    SAVEGAMEMENU = nodes[0].InnerText;
                }
                nodes = xDoc.GetElementsByTagName("HELPMENU");
                if (nodes.Count > 0)
                {
                    HELPMENU = nodes[0].InnerText;
                }
                nodes = xDoc.GetElementsByTagName("OPTIONSMENU");
                if (nodes.Count > 0)
                {
                    OPTIONSMENU = nodes[0].InnerText;
                }
                nodes = xDoc.GetElementsByTagName("BTSEARCH");
                if (nodes.Count > 0)
                {
                    BTSEARCH = nodes[0].InnerText;
                }
                nodes = xDoc.GetElementsByTagName("BTPREV");
                if (nodes.Count > 0)
                {
                    BTPREV = nodes[0].InnerText;
                }
                nodes = xDoc.GetElementsByTagName("BTNEXT");
                if (nodes.Count > 0)
                {
                    BTNEXT = nodes[0].InnerText;
                }
                nodes = xDoc.GetElementsByTagName("CKBAUSBLENDEN");
                if (nodes.Count > 0)
                {
                    CKBAUSBLENDEN = nodes[0].InnerText;
                }
                nodes = xDoc.GetElementsByTagName("DATAVAR");
                if (nodes.Count > 0)
                {
                    DATAVAR = nodes[0].InnerText;
                }
                nodes = xDoc.GetElementsByTagName("DATAVAL");
                if (nodes.Count > 0)
                {
                    DATAVAL = nodes[0].InnerText;
                }
                nodes = xDoc.GetElementsByTagName("BTBACKUP");
                if (nodes.Count > 0)
                {
                    BTBACKUP = nodes[0].InnerText;
                }
                nodes = xDoc.GetElementsByTagName("BTRESTORE");
                if (nodes.Count > 0)
                {
                    BTRESTORE = nodes[0].InnerText;
                }
                nodes = xDoc.GetElementsByTagName("BTSAVE");
                if (nodes.Count > 0)
                {
                    BTSAVE = nodes[0].InnerText;
                }
                nodes = xDoc.GetElementsByTagName("LOADNEWESTMENU");
                if (nodes.Count > 0)
                {
                    LOADNEWESTMENU = nodes[0].InnerText;
                }
                nodes = xDoc.GetElementsByTagName("LOADMENU");
                if (nodes.Count > 0)
                {
                    LOADMENU = nodes[0].InnerText;
                }
                nodes = xDoc.GetElementsByTagName("LISTALLMENU");
                if (nodes.Count > 0)
                {
                    LISTALLMENU = nodes[0].InnerText;
                }
                nodes = xDoc.GetElementsByTagName("HELPM");
                if (nodes.Count > 0)
                {
                    HELPM = nodes[0].InnerText;
                }
                nodes = xDoc.GetElementsByTagName("INFO");
                if (nodes.Count > 0)
                {
                    INFO = nodes[0].InnerText;
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
                nodes = xDoc.GetElementsByTagName("FOPTIONSTITLE");
                if (nodes.Count > 0)
                {
                    FOPTIONSTITLE = nodes[0].InnerText;
                }
                nodes = xDoc.GetElementsByTagName("MBTINPUTERROR");
                if (nodes.Count > 0)
                {
                    MBTINPUTERROR = nodes[0].InnerText;
                }
                nodes = xDoc.GetElementsByTagName("MBINPUTERROR");
                if (nodes.Count > 0)
                {
                    MBINPUTERROR = nodes[0].InnerText;
                }
                nodes = xDoc.GetElementsByTagName("MBTFILEERROR");
                if (nodes.Count > 0)
                {
                    MBTFILEERROR = nodes[0].InnerText;
                }
                nodes = xDoc.GetElementsByTagName("MBFILENOTFOUN");
                if (nodes.Count > 0)
                {
                    MBFILENOTFOUND = nodes[0].InnerText;
                }
                nodes = xDoc.GetElementsByTagName("MBFILENOTREAD");
                if (nodes.Count > 0)
                {
                    MBFILENOTREAD = nodes[0].InnerText;
                }
                nodes = xDoc.GetElementsByTagName("MBFOLDERERROR");
                if (nodes.Count > 0)
                {
                    MBFOLDERERROR = nodes[0].InnerText;
                }
                nodes = xDoc.GetElementsByTagName("MBNOTHINGFOUN");
                if (nodes.Count > 0)
                {
                    MBNOTHINGFOUND = nodes[0].InnerText;
                }
                nodes = xDoc.GetElementsByTagName("MBNOACCESS");
                if (nodes.Count > 0)
                {
                    MBNOACCESS = nodes[0].InnerText;
                }
                nodes = xDoc.GetElementsByTagName("MBREGISTRY");
                if (nodes.Count > 0)
                {
                    MBREGISTRY = nodes[0].InnerText;
                }
                nodes = xDoc.GetElementsByTagName("MBREGISTRYFOUND");
                if (nodes.Count > 0)
                {
                    MBREGISTRYFOUND = nodes[0].InnerText;
                }
                nodes = xDoc.GetElementsByTagName("MBREGISTRYNOTFOUND");
                if (nodes.Count > 0)
                {
                    MBREGISTRYNOTFOUND = nodes[0].InnerText;
                }
                nodes = xDoc.GetElementsByTagName("MBWRONGINPUT");
                if (nodes.Count > 0)
                {
                    MBWRONGINPUT = nodes[0].InnerText;
                }
                nodes = xDoc.GetElementsByTagName("MBVARNOTFOUND");
                if (nodes.Count > 0)
                {
                    MBVARNOTFOUND = nodes[0].InnerText;
                }
                nodes = xDoc.GetElementsByTagName("MBTNOTFOUND");
                if (nodes.Count > 0)
                {
                    MBTNOTFOUND = nodes[0].InnerText;
                }
                nodes = xDoc.GetElementsByTagName("MBBACKUPCREATEERROR");
                if (nodes.Count > 0)
                {
                    MBBACKUPCREATEERROR = nodes[0].InnerText;
                }
                nodes = xDoc.GetElementsByTagName("MBLOADBACKUP");
                if (nodes.Count > 0)
                {
                    MBLOADBACKUP = nodes[0].InnerText;
                }
                nodes = xDoc.GetElementsByTagName("MBBACKUPLOADERROR");
                if (nodes.Count > 0)
                {
                    MBBACKUPLOADERROR = nodes[0].InnerText;
                }
                nodes = xDoc.GetElementsByTagName("MBBACKUPNOTFOUND");
                if (nodes.Count > 0)
                {
                    MBBACKUPNOTFOUND = nodes[0].InnerText;
                }
                nodes = xDoc.GetElementsByTagName("MBSAVEERROR");
                if (nodes.Count > 0)
                {
                    MBSAVEERROR = nodes[0].InnerText;
                }
                nodes = xDoc.GetElementsByTagName("MBHELPNOTFOUND");
                if (nodes.Count > 0)
                {
                    MBHELPNOTFOUND = nodes[0].InnerText;
                }
                nodes = xDoc.GetElementsByTagName("CKBAUTOSEARCH");
                if (nodes.Count > 0)
                {
                    CKBAUTOSEARCH = nodes[0].InnerText;
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
            this.Text = TITLE;
            this.savegameMenu.Text = SAVEGAMEMENU;
            this.HelpMenu.Text = HELPMENU;
            this.OptionsMenu.Text = OPTIONSMENU;
            this.BtSearch.Text = BTSEARCH;
            this.BtPrev.Text = BTPREV;
            this.BtNext.Text = BTNEXT;
            this.CKBAutoSearch.Text = CKBAUTOSEARCH;
            this.CKBAusblenden.Text = CKBAUSBLENDEN;
            this.Data.Columns[0].HeaderText = DATAVAR;
            this.Data.Columns[1].HeaderText = DATAVAL;
            this.BtBackup.Text = BTBACKUP;
            this.BtRestore.Text = BTRESTORE;
            this.BtSave.Text = BTSAVE;
            this.LoadNewestMenu.Text = LOADNEWESTMENU;
            this.LoadMenu.Text = LOADMENU;
            this.ListAllMenu.Text = LISTALLMENU;
            this.HelpM.Text = HELPM;
            this.Info.Text = INFO;

        }

       

       

       

        




    }
    /*public class VarComparer<T> : IComparer
    {

        int IComparer.Compare(object x, object y)
        {
            

            Var v = x as Var;
           
            Var w = y as Var;
            
            return String.Compare(v.name,w.name);
        }
    }*/

}
