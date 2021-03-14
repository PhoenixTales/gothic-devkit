using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Drawing.Imaging;

namespace Font2Targa
{
    public partial class Font2Targa_Main : Form
    {
        private const byte ConfigVersion = 1;

        BinaryWriter BW, BW2;
        readonly bool[] CharActive = new bool[256];
        readonly int[,] Coords = new int[256, 2];

        private readonly Int16[] CP1251 = new Int16[] { 0x0000, 0x0001, 0x0002, 0x0003, 0x0004, 0x0005, 0x0006, 0x0007, 0x0008, 0x0009, 0x000a, 0x000b, 0x000c, 0x000d, 0x000e, 0x000f, 0x0010, 0x0011, 0x0012, 0x0013, 0x0014, 0x0015, 0x0016, 0x0017, 0x0018, 0x0019, 0x001a, 0x001b, 0x001c, 0x001d, 0x001e, 0x001f, 0x0020, 0x0021, 0x0022, 0x0023, 0x0024, 0x0025, 0x0026, 0x0027, 0x0028, 0x0029, 0x002a, 0x002b, 0x002c, 0x002d, 0x002e, 0x002f, 0x0030, 0x0031, 0x0032, 0x0033, 0x0034, 0x0035, 0x0036, 0x0037, 0x0038, 0x0039, 0x003a, 0x003b, 0x003c, 0x003d, 0x003e, 0x003f, 0x0040, 0x0041, 0x0042, 0x0043, 0x0044, 0x0045, 0x0046, 0x0047, 0x0048, 0x0049, 0x004a, 0x004b, 0x004c, 0x004d, 0x004e, 0x004f, 0x0050, 0x0051, 0x0052, 0x0053, 0x0054, 0x0055, 0x0056, 0x0057, 0x0058, 0x0059, 0x005a, 0x005b, 0x005c, 0x005d, 0x005e, 0x005f, 0x0060, 0x0061, 0x0062, 0x0063, 0x0064, 0x0065, 0x0066, 0x0067, 0x0068, 0x0069, 0x006a, 0x006b, 0x006c, 0x006d, 0x006e, 0x006f, 0x0070, 0x0071, 0x0072, 0x0073, 0x0074, 0x0075, 0x0076, 0x0077, 0x0078, 0x0079, 0x007a, 0x007b, 0x007c, 0x007d, 0x007e, 0x007f, 0x0402, 0x0403, 0x201a, 0x0453, 0x201e, 0x2026, 0x2020, 0x2021, 0x20ac, 0x2030, 0x0409, 0x2039, 0x040a, 0x040c, 0x040b, 0x040f, 0x0452, 0x2018, 0x2019, 0x201c, 0x201d, 0x2022, 0x2013, 0x2014, 0x0098, 0x2122, 0x0459, 0x203a, 0x045a, 0x045c, 0x045b, 0x045f, 0x00a0, 0x040e, 0x045e, 0x0408, 0x00a4, 0x0490, 0x00a6, 0x00a7, 0x0401, 0x00a9, 0x0404, 0x00ab, 0x00ac, 0x00ad, 0x00ae, 0x0407, 0x00b0, 0x00b1, 0x0406, 0x0456, 0x0491, 0x00b5, 0x00b6, 0x00b7, 0x0451, 0x2116, 0x0454, 0x00bb, 0x0458, 0x0405, 0x0455, 0x0457, 0x0410, 0x0411, 0x0412, 0x0413, 0x0414, 0x0415, 0x0416, 0x0417, 0x0418, 0x0419, 0x041a, 0x041b, 0x041c, 0x041d, 0x041e, 0x041f, 0x0420, 0x0421, 0x0422, 0x0423, 0x0424, 0x0425, 0x0426, 0x0427, 0x0428, 0x0429, 0x042a, 0x042b, 0x042c, 0x042d, 0x042e, 0x042f, 0x0430, 0x0431, 0x0432, 0x0433, 0x0434, 0x0435, 0x0436, 0x0437, 0x0438, 0x0439, 0x043a, 0x043b, 0x043c, 0x043d, 0x043e, 0x043f, 0x0440, 0x0441, 0x0442, 0x0443, 0x0444, 0x0445, 0x0446, 0x0447, 0x0448, 0x0449, 0x044a, 0x044b, 0x044c, 0x044d, 0x044e, 0x044f }; 

        string ErrorMsg = "";

        private static void Warning(string text)
        {
            MessageBox.Show(text, @"Warnung", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private static void Error(string text)
        {
            MessageBox.Show(text, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public Font2Targa_Main()
        {
            InitializeComponent();

            SFD.InitialDirectory = Application.StartupPath;
            SFDCon.InitialDirectory = Application.StartupPath;
            OFDCon.InitialDirectory = Application.StartupPath;

            UpdateFontlist();
            lbx_Fonts.SelectedIndex = -1;
        }

        private void UpdateFontlist()
        {
            lbx_Fonts.Items.Clear();
            foreach (var family in new InstalledFontCollection().Families)
                lbx_Fonts.Items.Add(family.Name);
        }

        int tileW, tileH;
        private Bitmap CreateBitmapImage()
        {
            lbx_Console.Items.Clear();

            FontStyle style = FontStyle.Regular;
            if (rbt_Bold.Checked) style = FontStyle.Bold;
            if (rbt_Italic.Checked) style = FontStyle.Italic;

            Font objFont = new Font((string)lbx_Fonts.SelectedItem, (int)nud_Fontsize.Value, style, GraphicsUnit.Pixel);

            int w = Convert.ToInt32(cbx_TexWidth.Text), h = Convert.ToInt32(cbx_TexHeight.Text);

            Bitmap objBmpImage = new Bitmap(w, h);
            Graphics objGraphics = Graphics.FromImage(objBmpImage);
         
            objGraphics.Clear(Color.Transparent);

            if (cbx_Alias.SelectedIndex == 0)
            {
                objGraphics.SmoothingMode = SmoothingMode.AntiAlias;
                objGraphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            }
            else
            {
                objGraphics.SmoothingMode = SmoothingMode.None;
                objGraphics.TextRenderingHint = TextRenderingHint.SystemDefault;
            }

            tileW = (int)nud_CharWidth.Value - 1;
            tileH = (int)nud_CharHeight.Value - 1;

            int tileWh = tileW>>1;
            int tileHh = tileH>>1;

            SolidBrush fontBrush = new SolidBrush(btn_Textcolor.BackColor);
            
            int j = -1, k = 0;
            for (int i = 32; i < 256; ++i)
            {
                if (rtb_Chars.Enabled && !CharActive[i])
                {
                    Coords[i, 0] = -1;
                    Coords[i, 1] = -1;
                    continue;
                }

                Bitmap tile = new Bitmap(tileW-1, tileH-1);
                Graphics tileG = Graphics.FromImage(tile);
                string s = cbx_CP1251.Checked ? ((char) CP1251[i]).ToString() : ((char)i).ToString();

                SizeF size = objGraphics.MeasureString(s, objFont);
                Bitmap letter = new Bitmap((int)size.Width, (int)size.Height);
                Graphics letterG = Graphics.FromImage(letter);
                if (cbx_Alias.SelectedIndex == 0)
                {
                    letterG.SmoothingMode = SmoothingMode.AntiAlias;
                    letterG.TextRenderingHint = TextRenderingHint.AntiAlias;
                }
                else
                {
                    letterG.SmoothingMode = SmoothingMode.None;
                    letterG.TextRenderingHint = TextRenderingHint.SystemDefault;
                }
                letterG.DrawString(s, objFont, fontBrush, 0, 0);

                tileG.DrawImage(letter, -((int)size.Width >> 1) + tileWh + (int)nud_PosX.Value, -((int)size.Height >> 1) + tileHh + (int)nud_PosCorrect.Value);

                ++j;
                if ((j+1)*tileW > w)
                {
                    j = 0;
                    ++k;
                    if ((k + 1) * tileH > h)
                    {
                        lbx_Console.Items.Add("Nicht alle Buchstaben passen auf die Textur.");
                        lbx_Console.Items.Add("Vergrößere die Textur oder reduziere die Zeichengröße.");
                        ErrorMsg = "Nicht alle Buchstaben passen auf die Textur.";
                        break;
                    }
                }
                Coords[i, 0] = j * tileW + 1;
                Coords[i, 1] = k * tileH + 1;
                objGraphics.DrawImage(tile, Coords[i, 0], Coords[i, 1]);
                ErrorMsg = "";
            }
            int kmax = (k+1) * tileH;
            Pen pen = new Pen(new SolidBrush(Color.FromArgb(127, 255, 0, 0)));
            for (j = 0; j+1 <= w; j += tileW)
                objGraphics.DrawLine(pen, new Point(j, 0), new Point(j, kmax));
            for (k = 0; k <= kmax; k += tileH)
                objGraphics.DrawLine(pen, new Point(0, k), new Point(j - tileW, k));

            objGraphics.Flush();
         
            return (objBmpImage);
        }

        private void GenerateTarga()
        {
            if (lbx_Fonts.SelectedIndex == -1) return;
            if(pbx_Targa.Image != null)
                pbx_Targa.Image.Dispose();
            pbx_Targa.Image = CreateBitmapImage();
        }

        private void ExportTarga()
        {
            if (ErrorMsg != "")
            {
                Warning(ErrorMsg);
                return;
            }

            if (lbx_Fonts.SelectedIndex == -1)
            {
                Error("Keine Schriftart ausgewählt. Textur kann nicht exportiert werden.");
                return;
            }
            SFD.FileName = "Font_" + ((string)lbx_Fonts.SelectedItem).Replace(" ", "") + "_" + nud_Fontsize.Value + ".tga";
            if (SFD.ShowDialog() != DialogResult.OK) return;
            try
            {
                BW = new BinaryWriter(new FileStream(SFD.FileName, FileMode.Create));
                BW2 = new BinaryWriter(new FileStream(SFD.FileName.Substring(0, SFD.FileName.Length-3) + "fnt", FileMode.Create));
            }
            catch 
            {
                Error("Auf die gewählte Datei kann nicht zugegriffen werden. Schließe alle Programme und versuche es erneut.");
                BW = null;
                BW2 = null;
                return;
            }

            int w = Convert.ToInt32(cbx_TexWidth.Text), h = Convert.ToInt32(cbx_TexHeight.Text);

            // Header
            BW.Write((byte)0);
            BW.Write((byte)0);
            BW.Write((byte)2);   // Unkomprimiert
            BW.Write(0);
            BW.Write(0);
            BW.Write((byte)0);
            BW.Write((UInt16)w); // Breite
            BW.Write((UInt16)h); // Höhe
            BW.Write((byte)32);  // Bittiefe
            BW.Write((byte)0);

            Bitmap bmp = (Bitmap)pbx_Targa.Image;
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, bmp.PixelFormat);
            unsafe
            {
                for (int y = h-1; y >= 0; --y)
                    for (int x = 0; x < w; ++x)
                    {
                        int offs = x << 2;
                        byte* row = (byte*)data.Scan0 + (y * data.Stride);
                        BW.Write(row[offs++]);
                        BW.Write(row[offs++]);
                        offs++;
                        if ((row[offs] == 127 || row[offs] == 145 || row[offs] == 167 || row[offs] == 175 || row[offs] == 191 || row[offs] == 183) && (row[offs - 1] >= 254))
                        {
                            BW.Write((byte)255);
                            BW.Write((byte)0);
                        }
                        else
                        {
                            BW.Write(row[offs - 1]);
                            BW.Write(row[offs]);
                        }
                    }
            }
            bmp.UnlockBits(data);
            BW.Close();

            // Jetzt die .fnt beschreiben
            BW2.Write('1');
            BW2.Write('\n');
            string p = Path.GetFileName(SFD.FileName) ?? String.Empty;
            string fileName = p.ToUpper();
            for (int i = 0; i < fileName.Length; ++i)
                BW2.Write(fileName[i]);
            BW2.Write('\n');
            BW2.Write(tileH-2);
            BW2.Write(256);
            float[,] fnt = new float[256, 5];
            unsafe
            {
                for (int i = 0; i < 256; ++i)
                {
                    fnt[i, 0] = 0;
                    fnt[i, 1] = 0;
                    fnt[i, 2] = 0;
                    fnt[i, 3] = 0;
                    fnt[i, 4] = 0;
                }
                for (int i = 32; i < 256; ++i)
                {
                    if (Coords[i, 0] == -1) continue;

                    int oX = 0;

                    fnt[i, 0] = (float)(Coords[i, 0] / (double)w);
                    fnt[i, 1] = (float)(Coords[i, 1] / (double)h);
                    fnt[i, 2] = (float)((Coords[i, 0]+3) / (double)w);
                    fnt[i, 3] = (float)((Coords[i, 1]+tileH-2) / (double)h);
                    fnt[i, 4] = 3;

                    bool found = false;
                    for (int x = Coords[i, 0]; x < Coords[i, 0] + tileW - 1; ++x)
                    {
                        for (int y = Coords[i, 1]; y < Coords[i, 1] + tileH - 1; ++y)
                        {
                            byte* row = (byte*)(data.Scan0) + ((y * data.Stride) + (x<<2) + 3);
                            if (row[0] > 0)
                            {
                                found = true;
                                oX = x;
                                fnt[i, 0] = (float)(oX / (double)w);
                                break;
                            }
                        }
                        if (found) break;
                    }
                    if (!found) continue;
                    found = false;
                    for (int x = Coords[i, 0] + tileW - 2; x >= Coords[i, 0]; --x)
                    {
                        for (int y = Coords[i, 1]; y < Coords[i, 1] + tileH - 1; ++y)
                        {
                            byte* row = (byte*)(data.Scan0) + ((y * data.Stride) + (x << 2) + 3);
                            if (row[0] <= 0) continue;
                            found = true;
                            fnt[i, 2] = (float)((x + 2) / (double)w);
                            fnt[i, 4] = x - oX + 2;
                            break;
                        }
                        if (found) break;
                    }
                    if (found) continue;
                }
                if (Coords[32, 0] != -1 && fnt[32, 4] == 3.0 && Coords[97, 0] != -1) // Wenn ' ' und 'a' aktiv, dann ' ' mit Breite von 'a' überschreiben (So machts auch die Engine)
                {
                    fnt[32, 4] = fnt[97, 4];
                }
            }
            for (int i = 0; i < 256; ++i)
                BW2.Write((byte)(int)fnt[i, 4]);
            for (int i = 0; i < 256; ++i)
            {
                BW2.Write(fnt[i, 0]);
                BW2.Write(fnt[i, 1]);
            }
            for (int i = 0; i < 256; ++i)
            {
                BW2.Write(fnt[i, 2]);
                BW2.Write(fnt[i, 3]);
            }
            BW2.Close();
        }

        private void ParseRTB()
        {
            for (int i = 0; i < 256; ++i)
                CharActive[i] = false;
            for (int i = 0; i < rtb_Chars.TextLength; ++i)
                CharActive[(byte)rtb_Chars.Text[i]] = true;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lbl_Preview.Font = new Font((string)lbx_Fonts.SelectedItem, 12);
                lbl_Preview.Location = new Point(lbl_Preview.Location.X, 293 - lbl_Preview.Height);
                lbx_Fonts.Height = 277 - lbl_Preview.Height;
                GenerateTarga();
            }
            catch
            {
                Error("Die Schriftart \"" + (string)lbx_Fonts.SelectedItem + "\" kann nicht Regulär angezeigt werden");
                lbx_Fonts.Items.RemoveAt(lbx_Fonts.SelectedIndex);
                lbx_Fonts.SelectedIndex = -1;
            }
        }

        private void btn_Textcolor_Click(object sender, EventArgs e)
        {
            if (CLD.ShowDialog() != DialogResult.OK) return;
            btn_Textcolor.BackColor = CLD.Color;
            btn_Textcolor.ForeColor = CLD.Color.GetBrightness() < 0.5 ? Color.White : Color.Black;
            GenerateTarga();
        }

        private void btn_Bgcol_Click(object sender, EventArgs e)
        {
            if (CLD.ShowDialog() != DialogResult.OK) return;
            btn_Bgcol.BackColor = CLD.Color;
            btn_Bgcol.ForeColor = CLD.Color.GetBrightness() < 0.5 ? Color.White : Color.Black;
            pbx_Targa.BackColor = CLD.Color;
            GenerateTarga();
        }

        #region Targa-Input

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            GenerateTarga();
        }

        private void cbx_Alias_SelectedIndexChanged(object sender, EventArgs e)
        {
            GenerateTarga();
        }

        private void btn_Export_Click(object sender, EventArgs e)
        {
            ExportTarga();
        }

        private void nud_CharWidth_ValueChanged(object sender, EventArgs e)
        {
            GenerateTarga();
        }

        private void nud_CharHeight_ValueChanged(object sender, EventArgs e)
        {
            GenerateTarga();
        }

        private void cbx_TexWidth_SelectedIndexChanged(object sender, EventArgs e)
        {
            GenerateTarga();
        }

        private void cbx_TexHeight_SelectedIndexChanged(object sender, EventArgs e)
        {
            GenerateTarga();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            GenerateTarga();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            GenerateTarga();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            GenerateTarga();
        }

        private void nud_PosCorrect_ValueChanged(object sender, EventArgs e)
        {
            GenerateTarga();
        }

        private void nud_PosX_ValueChanged(object sender, EventArgs e)
        {
            GenerateTarga();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            GenerateTarga();
        }

        #endregion

        private void rtb_Chars_TextChanged(object sender, EventArgs e)
        {
            ParseRTB();
            GenerateTarga();
        }

        private void radioButton2_CheckedChanged_1(object sender, EventArgs e)
        {
            rtb_Chars.Enabled = rbn_Following.Checked;
            ParseRTB();
            GenerateTarga();
        }

        #region F2TConfig

        private void LoadConfig(string path)
        {
            BinaryReader BR;
            try
            {
                BR = new BinaryReader(new FileStream(path, FileMode.Open));
            }
            catch
            {
                Error("Auf die gewählte Datei kann nicht zugegriffen werden. Schließe alle Programme und versuche es erneut.");
                return;
            }

            try
            {

                if (BR.ReadString() != "F2T")
                {
                    Error("Die gewählte Datei beinhaltet keine gültige Font2Targa Konfiguration.");
                    return;
                }

                byte val = BR.ReadByte();
                if (val != ConfigVersion)
                {
                    Error((val > ConfigVersion) ? "Diese Datei ist zu neu für dieses Programm. Bitte installiere die neueste Font2Targa Version."
                                                : "Diese Datei ist veraltet und wird nicht mehr unterstützt.");
                    return;
                }

                if (BR.ReadByte() == 1)
                    lbx_Fonts.SelectedItem = BR.ReadString();
                else
                    lbx_Fonts.SelectedIndex = -1;

                btn_Bgcol.BackColor = Color.FromArgb(255, BR.ReadByte(), BR.ReadByte(), BR.ReadByte());
                btn_Textcolor.BackColor = Color.FromArgb(255, BR.ReadByte(), BR.ReadByte(), BR.ReadByte());

                btn_Bgcol.ForeColor = btn_Bgcol.BackColor.GetBrightness() < 0.5 ? Color.White : Color.Black;
                pbx_Targa.BackColor = btn_Bgcol.BackColor;

                btn_Textcolor.ForeColor = btn_Textcolor.BackColor.GetBrightness() < 0.5 ? Color.White : Color.Black;


                nud_Fontsize.Value = BR.ReadByte();
                val = BR.ReadByte();
                cbx_Alias.SelectedIndex = val & 0x0F;
                rbn_Regular.Checked = (val & (1 << 4)) > 0;
                rbt_Bold.Checked = (val & (1 << 5)) > 0;
                rbt_Italic.Checked = (val & (1 << 6)) > 0;
                val = BR.ReadByte();
                cbx_TexHeight.SelectedIndex = val & 0x0F;
                cbx_TexWidth.SelectedIndex = (val & 0xF0) >> 4;
                nud_CharHeight.Value = BR.ReadByte();
                nud_CharWidth.Value = BR.ReadByte();
                nud_PosCorrect.Value = BR.ReadByte() - 127;
                nud_PosX.Value = BR.ReadByte() - 127;

                if (BR.ReadByte() == 1)
                {
                    rbt_All.Checked = false;
                    rbn_Following.Checked = true;
                    for (int i = 0; i < 32; ++i)
                    {
                        val = BR.ReadByte();
                        for (int j = 0; j < 8; ++j)
                            CharActive[(i << 3) + j] = (val & (1 << j)) > 0;
                    }
                    StringBuilder b = new StringBuilder();
                    for (int i = 0; i < 256; ++i)
                        if (CharActive[i])
                            b.Append(((char)i).ToString());
                    rtb_Chars.Text = b.ToString();
                }
                else
                {
                    rbt_All.Checked = true;
                    rbn_Following.Checked = false;
                }

            }
            catch
            {
                Error("Beim Lesen der Datei ist ein Fehler aufgetreten. Möglicherweise ist sie unvollständig oder beschädigt.");
            }
            finally
            {
                BR.Close();
            }
        }

        private void btn_SaveConfig_Click(object sender, EventArgs e)
        {
            SFDCon.FileName = "Font_" + (lbx_Fonts.SelectedIndex != -1 ? (((string)lbx_Fonts.SelectedItem).Replace(" ", "")) : "Any") + "_" + nud_Fontsize.Value + ".f2t";
            if (SFDCon.ShowDialog() != DialogResult.OK) return;
            try
            {
                BW = new BinaryWriter(new FileStream(SFDCon.FileName, FileMode.Create));
            }
            catch
            {
                Error("Auf die gewählte Datei kann nicht zugegriffen werden. Schließe alle Programme und versuche es erneut.");
                return;
            }

            BW.Write("F2T");   // Identifier
            BW.Write(ConfigVersion); // Version

            if (lbx_Fonts.SelectedIndex == -1)
                BW.Write((byte)0);
            else
            {
                BW.Write((byte)1);
                BW.Write((string)lbx_Fonts.SelectedItem);
            }

            BW.Write(btn_Bgcol.BackColor.R); // Hintergrundfarbe
            BW.Write(btn_Bgcol.BackColor.G);
            BW.Write(btn_Bgcol.BackColor.B);

            BW.Write(btn_Textcolor.BackColor.R); // Schriftfarbe
            BW.Write(btn_Textcolor.BackColor.G);
            BW.Write(btn_Textcolor.BackColor.B);

            BW.Write((byte)nud_Fontsize.Value);
            BW.Write((byte)(cbx_Alias.SelectedIndex 
                | (Convert.ToInt32(rbn_Regular.Checked) << 4)
                | (Convert.ToInt32(rbt_Bold.Checked) << 5)
                | (Convert.ToInt32(rbt_Italic.Checked) << 6)));
            BW.Write((byte)(cbx_TexHeight.SelectedIndex 
                | (cbx_TexWidth.SelectedIndex<<4)));
            BW.Write((byte)nud_CharHeight.Value);
            BW.Write((byte)nud_CharWidth.Value);
            BW.Write((byte)(nud_PosCorrect.Value+127));
            BW.Write((byte)(nud_PosX.Value + 127));
            BW.Write(Convert.ToByte(rtb_Chars.Enabled));
            if (rtb_Chars.Enabled)
                for (int i = 0; i < 32; ++i)
                {
                    byte val = 0;
                    for (int j = 0; j < 8; ++j)
                        val |= (byte)(Convert.ToInt32(CharActive[(i << 3) + j])<<j);
                    BW.Write(val);
                }

            BW.Close();
        }

        private void btn_LoadConfig_Click(object sender, EventArgs e)
        {
            if (OFDCon.ShowDialog() != DialogResult.OK) return;
            LoadConfig(OFDCon.FileName);
        }

        #endregion
    }
}
