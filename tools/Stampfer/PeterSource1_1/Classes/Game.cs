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
using System.Diagnostics;

namespace Peter
{
    public partial class Game : Form
    {
        Timer t = new Timer();

        List<Point> pixelup = new List<Point>();
        List<Point> pixeldown = new List<Point>();
        int ship = 0;
        int speed = 5;
        const int MOVESPEED = 4;
        int pos = 0;
        int size = 5;
        Random r = new Random();
        bool up = false;
        int score = 0;
        int multi = 1;
        int scorecounter = 0;
        const int PWSIZE = 6;
        const int SCOREPOWERUP = 100;
        List<Point> powerups = new List<Point>();
        List<Point> badup = new List<Point>();
        Stopwatch stopwatch = new Stopwatch();
        public int Score
        {
            set
            {
                score = value;
                Text = IntToBin(score);
            }
            get
            {
                return score;
            }

        }

        public Game()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            t.Interval = 30;
            t.Tick += new EventHandler(Game_Update);
            t.Enabled = true;
            stopwatch.Start();
            Init();



        }
        string IntToBin(int i)
        {
            return Convert.ToString(i, 2);
        }
        void Init()
        {

            stopwatch.Reset();
            stopwatch.Start();
            pixelup.Clear();
            pixeldown.Clear();
            powerups.Clear();
            badup.Clear();
            ship = this.Height / 2 - 5;
            pos = this.Width / 10;
            size = 4;
            speed = 5;
            Score = 0;
            scorecounter = 0;
            up = false;
            multi = 1;
            pixelup.Add(new Point(0, this.Height / 4));
            pixeldown.Add(new Point(0, this.Height - (this.Height / 4)));
            pixelup.Add(new Point(this.Width, this.Height / 4));
            pixeldown.Add(new Point(this.Width, this.Height - (this.Height / 4)));
            pixelup.Add(new Point(this.Width + 30, this.Height / 4));
            pixeldown.Add(new Point(this.Width + 30, this.Height - (this.Height / 4)));
            pixelup.Add(new Point(this.Width + 200, this.Height / 4));
            pixeldown.Add(new Point(this.Width + 200, this.Height - (this.Height / 4)));

        }
        int random = 0;
        void Game_Update(object sender, EventArgs e)
        {
            if (stopwatch.ElapsedMilliseconds > 60000)
            {
                speed++;
                stopwatch.Reset();
                stopwatch.Start();
            }
            scorecounter++;
            if (scorecounter > 30)
            {
                Score += multi;
                scorecounter = 0;
            }
            if (up)
            {
                if (ship > 0)
                    ship -= (int)(MOVESPEED * 1.5);
            }
            else
            {
                if (ship < this.Height)
                    ship += MOVESPEED;
            }
            for (int i = 0; i < pixelup.Count; i++)
            {
                pixelup[i] = new Point(pixelup[i].X - speed, pixelup[i].Y);

            }
            for (int i = 0; i < pixeldown.Count; i++)
            {
                pixeldown[i] = new Point(pixeldown[i].X - speed, pixeldown[i].Y);
            }
            for (int i = 0; i < powerups.Count; i++)
            {
                powerups[i] = new Point(powerups[i].X - speed, powerups[i].Y);
            }
            for (int i = 0; i < badup.Count; i++)
            {
                badup[i] = new Point(badup[i].X - speed, badup[i].Y);
            }
            if (pixelup[1].X < 0)
            {
                pixelup.RemoveAt(0);
            }
            if (pixeldown[1].X < 0)
            {
                pixeldown.RemoveAt(0);
            }
            if (powerups.Count > 0 && powerups[0].X < 0)
            {
                powerups.RemoveAt(0);
            }
            if (badup.Count > 0 && badup[0].X < 0)
            {
                badup.RemoveAt(0);
            }
            if (pixelup[pixelup.Count - 1].X < this.Width)
            {
                random = r.Next(100);
                CreateNewPixelUp();

                if (random > 50)
                {
                    AddPowerUp();
                }
                else if (random < 40)
                {
                    AddBadUp();
                }


            }

            if (pixeldown[pixeldown.Count - 1].X < this.Width)
            {
                CreateNewPixelDown();
            }

            Colli();
            Display.Invalidate();
        }
        Rectangle rec = new Rectangle();
        void Colli()
        {

            rec = new Rectangle(pos - size - PWSIZE, ship - size - PWSIZE, size * 2 + PWSIZE * 2, size * 2 + PWSIZE * 2);
            for (int i = 0; i < powerups.Count; i++)
            {
                if (rec.Contains(powerups[i]))
                {
                    powerups.RemoveAt(i);
                    Score += SCOREPOWERUP + (SCOREPOWERUP / 3) * multi;
                }

            }
            for (int i = 0; i < badup.Count; i++)
            {
                if (rec.Contains(badup[i]))
                {
                    badup.RemoveAt(i);
                    size++;

                }

            }
            if (ship < this.Height / 2)
            {
                ColliUp();
            }
            else
            {
                ColliDown();
            }

        }
        void ColliUp()
        {
            int i = 0;
            float m = 0f;
            int n = 0;
            for (i = 0; i < pixelup.Count; i++)
            {
                if (pixelup[i].X >= pos)
                {
                    break;
                }
            }
            m = ((pixelup[i].Y - pixelup[i - 1].Y) / (float)((pixelup[i].X) - (pixelup[i - 1].X)));
            n = (pixelup[i - 1].Y - (int)(m * pixelup[i - 1].X));//(int)(pixelup[i-1].Y / (float)(m * pixelup[i-1].X));


            if (ship - size < (m * pos) + n)
            {
                Die();
            }




        }
        float m = 0f;
        int n = 0;
        void ColliDown()
        {
            int i = 0;


            for (i = 0; i < pixeldown.Count; i++)
            {
                if (pixeldown[i].X > pos)
                {
                    break;
                }
            }
            m = ((pixeldown[i].Y - pixeldown[i - 1].Y) / (float)((pixeldown[i].X) - (pixeldown[i - 1].X)));
            n = (pixeldown[i - 1].Y - (int)(m * pixeldown[i - 1].X));//(int)(pixelup[i-1].Y / (float)(m * pixelup[i-1].X));


            if (ship + size > (m * pos) + n)
            {
                Die();
            }
        }
        void Die()
        {

            t.Enabled = false;
            MessageBox.Show("Haha!");
            Init();
            t.Enabled = true;
        }
        void AddPowerUp()
        {
            m = ((pixeldown[pixeldown.Count - 1].Y - pixeldown[pixeldown.Count - 2].Y) / (float)((pixeldown[pixeldown.Count - 1].X) - (pixeldown[pixeldown.Count - 2].X)));
            n = (pixeldown[pixeldown.Count - 2].Y - (int)(m * pixeldown[pixeldown.Count - 2].X));//(int)(pixelup[i-1].Y / (float)(m * pixelup[i-1].X));
            powerups.Add(new Point(pixelup[pixelup.Count - 2].X, pixelup[pixelup.Count - 2].Y + PWSIZE + r.Next((int)(m * pixelup[pixelup.Count - 2].X) + n - pixelup[pixelup.Count - 2].Y - PWSIZE)));

        }
        void AddBadUp()
        {
            m = ((pixeldown[pixeldown.Count - 1].Y - pixeldown[pixeldown.Count - 2].Y) / (float)((pixeldown[pixeldown.Count - 1].X) - (pixeldown[pixeldown.Count - 2].X)));
            n = (pixeldown[pixeldown.Count - 2].Y - (int)(m * pixeldown[pixeldown.Count - 2].X));//(int)(pixelup[i-1].Y / (float)(m * pixelup[i-1].X));
            badup.Add(new Point(pixelup[pixelup.Count - 2].X, pixelup[pixelup.Count - 2].Y + PWSIZE + r.Next((int)(m * pixelup[pixelup.Count - 2].X) + n - pixelup[pixelup.Count - 2].Y - PWSIZE)));

        }
        void CreateNewPixelUp()
        {
            pixelup.Add(new Point(pixelup[pixelup.Count - 1].X + (this.Width / 8) + (this.Width / 20) * r.Next(6), this.Height / 10 + r.Next(this.Height / 2 - this.Height / 10)));
        }
        void CreateNewPixelDown()
        {
            pixeldown.Add(new Point(pixelup[pixelup.Count - 1].X + (this.Width / 8) + (this.Width / 20) * r.Next(6), this.Height - (this.Height / 10 + r.Next(this.Height / 2 - this.Height / 10))));
        }

        private void Display_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Pen p = new Pen(Brushes.GreenYellow, 3);

            for (int i = 0; i < pixelup.Count - 1; i++)
            {
                g.DrawLine(p, pixelup[i], pixelup[i + 1]);
            }
            for (int i = 0; i < pixeldown.Count - 1; i++)
            {
                g.DrawLine(p, pixeldown[i], pixeldown[i + 1]);
            }
            for (int i = 0; i < powerups.Count - 1; i++)
            {
                g.DrawEllipse(p, powerups[i].X - PWSIZE, powerups[i].Y - PWSIZE, PWSIZE * 2, PWSIZE * 2);
            }
            for (int i = 0; i < badup.Count - 1; i++)
            {
                g.DrawRectangle(p, badup[i].X - PWSIZE, badup[i].Y - PWSIZE, PWSIZE * 2, PWSIZE * 2);

            }
            g.DrawEllipse(p, pos - size, ship - size, size * 2, size * 2);
        }

        private void Display_MouseDown(object sender, MouseEventArgs e)
        {
            up = true;
        }

        private void Display_MouseUp(object sender, MouseEventArgs e)
        {
            up = false;
        }

        private void Game_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.t.Enabled = false;
        }





    }
}

namespace Peter
{
    partial class Game
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.Display = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.Display)).BeginInit();
            this.SuspendLayout();
            // 
            // Display
            // 
            this.Display.BackColor = System.Drawing.Color.Black;
            this.Display.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Display.Location = new System.Drawing.Point(0, 0);
            this.Display.Name = "Display";
            this.Display.Size = new System.Drawing.Size(696, 510);
            this.Display.TabIndex = 0;
            this.Display.TabStop = false;
            this.Display.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Display_MouseDown);
            this.Display.Paint += new System.Windows.Forms.PaintEventHandler(this.Display_Paint);
            this.Display.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Display_MouseUp);
            // 
            // Game
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(696, 510);
            this.Controls.Add(this.Display);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Game";
            this.Text = "Game";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Game_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.Display)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox Display;
    }
}
