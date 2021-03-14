/**************************************************************************************
    Stampfer - Gothic Script Editor
    Copyright (C)  2009  Alexander "Sumpfkrautjunkie" Ruppert

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
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace Peter
{
    public partial class FighterGame : Form
    {
        Timer t = new Timer();
        Fighter player;
        List<BaseObject> objList = new List<BaseObject>();
        List<BaseObject> objListRunTime = new List<BaseObject>();
        Random r = new Random();
        Stopwatch stopwatch = new Stopwatch();
        int Enemycount;
        int EnemyHPcount;
        int EnemyPowerCount;
        public FighterGame()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);

            t.Interval = 20;
            t.Tick += new EventHandler(Game_Update);
            t.Enabled = true;
            Init();
        }


        void Init()
        {
            objList.Clear();
            System.Windows.Forms.Cursor.Hide();
            Enemycount = 970;
            EnemyHPcount = 10;
            EnemyPowerCount = 10;
            player = new Fighter(this.ClientRectangle, objList);
            AddObject(new Background(this.ClientRectangle, objList));
            AddObject(player);
            stopwatch.Reset();
            stopwatch.Start();
        }
        void AddObject(BaseObject b)
        {
            objList.Add(b);
        }
        void RemoveObject(BaseObject b)
        {
            try
            {
                objList.Remove(b);
            }
            catch
            {
            }
        }

        void Game_Update(object sender, EventArgs e)
        {
            int rand = r.Next(1000);
            if (rand > Enemycount)
            {
                AddObject(new Enemy(this.ClientRectangle, objList, new PointF(r.Next(this.ClientRectangle.Width - this.ClientRectangle.Width / 6) + this.ClientRectangle.Width / 6, -100), EnemyHPcount, EnemyPowerCount));
            }
            if (rand > 990)
            {
                AddObject(new PowerUp(this.ClientRectangle, objList, new PointF(r.Next(this.ClientRectangle.Width - this.ClientRectangle.Width / 6) + this.ClientRectangle.Width / 6, -100)));
            }
            objListRunTime.AddRange(objList);
            foreach (BaseObject b in objListRunTime)
            {
                b.Update();
            }

            if (!player.Visible)
            {
                t.Enabled = false;
                if (MessageBox.Show("Sie sind tot, Erdling!", "Game Over", MessageBoxButtons.OK) == DialogResult.OK)
                {
                    t.Enabled = true;
                    Init();
                }
            }

            if (stopwatch.ElapsedMilliseconds % 1000 == 0)
            {
                Enemycount--;
                EnemyHPcount++;
                EnemyPowerCount++;
            }

            Display.Invalidate();
        }

        private void Display_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;


            foreach (BaseObject b in objListRunTime)
            {
                b.Draw(g);
            }
            //g.DrawString(Enemycount.ToString(),new Font("Arial",8f),new SolidBrush(Color.White),new PointF(0,0));
            objListRunTime.Clear();
        }

        private void Display_MouseMove(object sender, MouseEventArgs e)
        {
            player.Position = PointToClient(MousePosition);
        }

        private void Display_Click(object sender, EventArgs e)
        {

            player.Shoot();

        }

        private void FighterGame_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Windows.Forms.Cursor.Show();
        }

        private void FighterGame_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }

    }


    public class BaseObject
    {
        protected Random r = new Random();
        protected PointF _pos = new PointF(-1000, 0);
        protected PointF _vec = new PointF(0, 0);
        protected Rectangle _bounds = new Rectangle(0, 0, 0, 0);
        protected Rectangle _screenbounds = new Rectangle(0, 0, 0, 0);
        protected float _rot = 0f;
        protected float _speed = 0f;
        protected Pen _pen = new Pen(Color.White);
        protected List<BaseObject> _list = new List<BaseObject>();
        protected const int STANDARDSIZEX = 10;
        protected const int STANDARDSIZEY = 10;
        protected bool _visible = true;
        protected float _ySpeed = 1f;
        protected int _hp = 10;
        public virtual int Hitpoints
        {
            get
            {
                return _hp;
            }
            set
            {
                _hp = value;
                if (_hp < 1) Kill(this);
            }
        }
        public virtual bool Visible
        {
            get
            {
                return _visible;
            }
            set
            {
                _visible = value;
            }
        }
        public virtual Rectangle Bounds
        {
            get
            {
                return _bounds;
            }
            set
            {
                _bounds = value;
            }
        }
        public virtual float Speed
        {
            get
            {
                return _speed;
            }
            set
            {
                _speed = value;
                RecalcVector();
            }
        }
        public virtual PointF Vector
        {
            get
            {
                return _vec;
            }
            set
            {
                _vec = value;

            }
        }
        public virtual PointF Position
        {
            get
            {
                return _pos;
            }
            set
            {
                _pos = value;
                _bounds.X = (int)(_pos.X - _bounds.Width / 2);
                _bounds.Y = (int)(_pos.Y - _bounds.Height / 2);
            }
        }
        public virtual float Rotation
        {
            get
            {
                return _rot;
            }
            set
            {
                _rot = value;
                RecalcVector();
            }
        }

        public BaseObject(Rectangle rec, List<BaseObject> list)
        {
            _screenbounds = rec;
            _list = list;
            _pen = new Pen(new SolidBrush(Color.LimeGreen), 2f);
            ChangeBounds(STANDARDSIZEX, STANDARDSIZEY);
        }
        public virtual void ChangePen(Color clr, float thickness)
        {
            _pen.Color = clr;
            _pen.Width = thickness;
        }
        public virtual void ChangeBounds(int w, int h)
        {
            _bounds.Width = w;
            _bounds.Height = h;
        }
        public void RecalcVector()
        {
            Vector = new PointF(_speed * (float)Math.Cos(_rot), _speed * (float)Math.Sin(_rot));
        }
        public virtual void Draw(Graphics g)
        {

        }
        public virtual void Update()
        {

        }
        public virtual void Move()
        {
            Position = new PointF(Position.X + Vector.X, Position.Y + Vector.Y + _ySpeed);
        }
        public virtual bool CheckBoundCollision(Rectangle r1, Rectangle r2)
        {
            Rectangle rec = CheckBoundCollisionRec(r1, r2);
            if (rec.Width > 0 || rec.Height > 0)
            {
                return true;
            }
            return false;
        }
        public virtual Rectangle CheckBoundCollisionRec(Rectangle r1, Rectangle r2)
        {
            Rectangle rec;
            rec = Rectangle.Intersect(r1, r2);
            if (rec.Width > 0 || rec.Height > 0)
            {
                return rec;
            }
            return rec;
        }
        public virtual bool IsInScreen()
        {
            return CheckBoundCollision(_bounds, _screenbounds);
        }
        public virtual void Remove()
        {
            _list.Remove(this);
        }
        public virtual BaseObject CheckCollision<T>()
        {

            foreach (BaseObject obj in _list)
            {
                if (obj is T)
                {
                    if (CheckBoundCollision(this.Bounds, obj.Bounds))
                    {
                        return obj;
                    }
                }

            }
            return null;
        }
        public virtual void Kill(BaseObject s)
        {

            _list.Remove(s);
        }

        public virtual void Damage(BaseObject s, int amount)
        {
            s.Hitpoints = s.Hitpoints - amount;
        }

    }
    public class Ship : BaseObject
    {
        protected bool isPlayer = false;
        protected int _firepower = 5;
        public virtual int FirePower
        {
            get
            {
                return _firepower;
            }
            set
            {
                _firepower = value;


            }

        }


        Bullet[] _bullets = new Bullet[50];
        public Ship(Rectangle rec, List<BaseObject> list, bool player)
            : base(rec, list)
        {
            for (int i = 0; i < _bullets.Length; i++)
            {
                isPlayer = player;
                _bullets[i] = new Bullet(rec, list, Position, player);
                _bullets[i].Dmg = _firepower;
                _bullets[i].Visible = false;

            }
        }
        public virtual void Shoot()
        {
            for (int i = 0; i < _bullets.Length; i++)
            {
                if (!_bullets[i].Visible)
                {
                    _bullets[i].Visible = true;
                    _bullets[i].Position = new PointF(Position.X, Position.Y);//+ (isPlayer ? (-Bounds.Height / 2) : (+Bounds.Height / 2)));
                    _bullets[i].Dmg = _firepower;
                    break;
                }
            }
        }
        public override void Update()
        {
            if (!_visible) return;
            base.Update();
            for (int i = 0; i < _bullets.Length; i++)
            {
                _bullets[i].Update();
            }
        }
        public override void Draw(Graphics g)
        {
            if (!_visible) return;
            base.Draw(g);
            for (int i = 0; i < _bullets.Length; i++)
            {
                _bullets[i].Draw(g);
            }
        }
        public virtual void CheckForRemove()
        {
            if (Bounds.Y > _screenbounds.Height)
            {
                Remove();
            }
        }
    }

    public class Fighter : Ship
    {
        Points pt;
        Life lf;
        protected int _pointCount = 0;

        public virtual int PointCount
        {
            get
            {
                return _pointCount;
            }
            set
            {
                pt.PointCount = _pointCount = value;

            }
        }
        public override bool Visible
        {
            get
            {
                return base.Visible;
            }
            set
            {
                base.Visible = value;
                PointCount = 0;
            }
        }
        public override int Hitpoints
        {
            get
            {
                return _hp;
            }
            set
            {
                lf.LifeCount = _hp = value;
                if (_hp < 1) Kill(this);
            }
        }
        public Fighter(Rectangle rec, List<BaseObject> list)
            : base(rec, list, true)
        {
            pt = new Points(_screenbounds, list, new PointF(_screenbounds.Width / 30, 0), "Punkte: ");
            lf = new Life(_screenbounds, list, new PointF(_screenbounds.Width / 30, _screenbounds.Height - _screenbounds.Height / 30), "Schild: ");
            Position = new PointF((_screenbounds.Width / 2), (_screenbounds.Width / 2));
            ChangeBounds(25, 25);
            Hitpoints = 100;
        }
        public override void Draw(Graphics g)
        {
            if (!_visible) return;
            base.Draw(g);
            //g.DrawEllipse(_pen, Bounds);
            g.DrawClosedCurve(_pen, new Point[]{new Point (Bounds.X+Bounds.Width/2,Bounds.Y),
                new Point(Bounds.X,Bounds.Y+Bounds.Height/2),
                new Point(Bounds.X+Bounds.Width/2,Bounds.Y+Bounds.Height/3),
                new Point(Bounds.X+Bounds.Width,Bounds.Y+Bounds.Height/2)
            });

            g.DrawLines(_pen, new Point[]{
                new Point(Bounds.X+Bounds.Width/4,Bounds.Y+Bounds.Height/2),
                new Point(Bounds.X+Bounds.Width/2,Bounds.Y+Bounds.Height),
                new Point(Bounds.X+Bounds.Width-(Bounds.Width/4),Bounds.Y+Bounds.Height/2)
                
            });
            g.DrawLines(_pen, new Point[]{
                new Point(Bounds.X+Bounds.Width/4,Bounds.Y+Bounds.Height/2),
                new Point(Bounds.X,Bounds.Y+Bounds.Height),
                new Point(Bounds.X+Bounds.Width-(Bounds.Width/4),Bounds.Y+Bounds.Height/2),
                new Point(Bounds.X+Bounds.Width,Bounds.Y+Bounds.Height),
                new Point(Bounds.X+Bounds.Width/4,Bounds.Y+Bounds.Height/2)
                
            });

            pt.Draw(g);
            lf.Draw(g);


        }
        public override void Update()
        {
            if (!_visible) return;
            base.Update();

            BaseObject o;
            if ((o = CheckCollision<Enemy>()) != null)
            {
                if (!(o is Fighter))
                {
                    Damage(this, o.Hitpoints);
                    Kill(o);
                }
            }
        }
        public override void Shoot()
        {
            if (!_visible) return;
            base.Shoot();
        }
        public override void Kill(BaseObject s)
        {

            if (s is Fighter)
            {
                Visible = false;
            }
            else
            {
                base.Kill(s);
            }
        }

    }
    public class Enemy : Ship
    {
        public Enemy(Rectangle rec, List<BaseObject> list, PointF pos, int hp, int fp)
            : base(rec, list, false)
        {
            Position = pos;
            ChangeBounds(25, 25);
            Rotation = (float)(Math.PI / 2);
            Speed = 2f;
            Hitpoints = hp;
            FirePower = fp;
        }
        public override void Draw(Graphics g)
        {
            if (!_visible) return;
            base.Draw(g);
            g.DrawLines(_pen, new Point[]{
                new Point(Bounds.X,Bounds.Y),
                new Point(Bounds.X+Bounds.Width/5,Bounds.Y+Bounds.Height),
                new Point(Bounds.X+Bounds.Width/5,Bounds.Y),
                new Point(Bounds.X,Bounds.Y)
            });
            g.DrawLines(_pen, new Point[]{
                new Point(Bounds.X+Bounds.Width,Bounds.Y),
                new Point(Bounds.X+Bounds.Width-(Bounds.Width/5),Bounds.Y+Bounds.Height),
                new Point(Bounds.X+Bounds.Width-(Bounds.Width/5),Bounds.Y),
                new Point(Bounds.X+Bounds.Width,Bounds.Y)
            });
            g.DrawLines(_pen, new Point[]{
                new Point(Bounds.X+Bounds.Width/5,Bounds.Y+Bounds.Height/4),
                new Point(Bounds.X+Bounds.Width/2,Bounds.Y+Bounds.Height-(Bounds.Height/4)),
                new Point(Bounds.X+Bounds.Width-(Bounds.Width/5),Bounds.Y+Bounds.Height/4),
                new Point(Bounds.X+Bounds.Width/5,Bounds.Y+Bounds.Height/4)
                
                
            });

        }
        public override void Update()
        {
            if (!_visible) return;
            base.Update();
            Move();
            if (r.Next(100) > 97) Shoot();
            CheckForRemove();
        }
        public override void Move()
        {
            base.Move();
            if (r.Next(100) > 95)
            {
                Speed = r.Next(6) + 1;
                Rotation = (float)(r.NextDouble() * Math.PI);
            }
        }
        public override void Kill(BaseObject s)
        {
            base.Kill(s);
            foreach (BaseObject b in _list)
            {
                if (b is Fighter)
                {
                    ((Fighter)b).PointCount += 5;
                }
            }
        }

    }



    public class Bullet : BaseObject
    {

        protected int _dmg;
        public virtual int Dmg
        {
            get
            {
                return _dmg;
            }
            set
            {
                _dmg = value;
            }
        }


        const float BASESPEED = 10f;
        public bool ownedByPlayer = false;
        public Bullet(Rectangle rec, List<BaseObject> list, PointF pos, bool owned)
            : base(rec, list)
        {
            ChangeBounds(2, 15);
            ownedByPlayer = owned;
            //Speed = 1f;
            Rotation = 0f;
            Position = pos;
            if (owned)
            {
                _ySpeed = -BASESPEED;
                ChangePen(Color.CornflowerBlue, 2f);
            }
            else
            {
                _ySpeed = BASESPEED;
                ChangePen(Color.Red, 2f);
            }

        }

        public override void Draw(Graphics g)
        {
            if (!_visible) return;
            base.Draw(g);
            g.DrawEllipse(_pen, Bounds);

        }
        public override void Update()
        {
            if (!_visible) return;
            base.Update();
            Move();
            if (!IsInScreen())
                Visible = false;

            BaseObject o;
            if ((o = CheckCollision<Ship>()) != null)
            {
                if (!((o is Fighter)
                    && this.ownedByPlayer == true)
                    && (!((o is Enemy)
                    && this.ownedByPlayer == false))
                    )
                {

                    Damage(o, Dmg);
                    Visible = false;
                }


            }

        }
        public override void Kill(BaseObject s)
        {
            Visible = false;
        }

    }
    public class Text : BaseObject
    {
        protected string _textvalue = "";
        protected Font fnt = new Font("Verdana", 10f);
        protected Brush brs = new SolidBrush(Color.LimeGreen);
        public virtual string Textvalue
        {
            get
            {
                return _textvalue;
            }
            set
            {
                _textvalue = value;
            }
        }
        public Text(Rectangle rec, List<BaseObject> list, PointF pos, string val)
            : base(rec, list)
        {
            Textvalue = val;
            Position = pos;
        }
        public override void Draw(Graphics g)
        {
            if (!_visible) return;
            base.Draw(g);
            //g.DrawString(Textvalue, fnt, brs, Position);

        }
        public override void Update()
        {
            if (!_visible) return;
            base.Update();
        }
    }
    public class Points : Text
    {
        protected int _pts = 0;
        public virtual int PointCount
        {
            get
            {
                return _pts;
            }
            set
            {
                _pts = value;
            }
        }
        public Points(Rectangle rec, List<BaseObject> list, PointF pos, string val)
            : base(rec, list, pos, val)
        {

        }

        public override void Draw(Graphics g)
        {
            if (!_visible) return;
            base.Draw(g);
            g.DrawString(Textvalue + PointCount.ToString(), fnt, brs, Position);

        }
    }
    public class Life : Text
    {
        protected int _life = 0;
        public virtual int LifeCount
        {
            get
            {
                return _life;
            }
            set
            {
                _life = value;
            }
        }
        public Life(Rectangle rec, List<BaseObject> list, PointF pos, string val)
            : base(rec, list, pos, val)
        {

        }

        public override void Draw(Graphics g)
        {
            if (!_visible) return;
            base.Draw(g);
            g.DrawString(Textvalue + LifeCount.ToString(), fnt, brs, Position);

        }
    }
    public class Particle : BaseObject
    {
        public Particle(Rectangle rec, List<BaseObject> list, float spd, Color clr)
            : base(rec, list)
        {
            Rotation = (float)(Math.PI / 2);
            _ySpeed = spd;
            ChangeBounds(1, 1);
            ChangePen(clr, 1f);
            //Position = new PointF(r.Next(_screenbounds.Width),-1);
        }
        public override void Update()
        {
            base.Update();
            Move();
            if (Position.Y > _screenbounds.Height)
            {
                Position = new PointF(Position.X, -1);
            }

        }
        public override void Draw(Graphics g)
        {
            base.Draw(g);
            //g.DrawEllipse(_pen, Bounds);
            g.DrawLine(_pen, new Point(Bounds.X + Bounds.Width / 2, Bounds.Y), new Point(Bounds.X + Bounds.Width / 2, Bounds.Y + Bounds.Height));
            g.DrawLine(_pen, new Point(Bounds.X, Bounds.Y + Bounds.Height / 2), new Point(Bounds.X + Bounds.Width, Bounds.Y + Bounds.Height / 2));
        }
    }
    public class Background : BaseObject
    {
        Particle[] layer1 = new Particle[50];
        Particle[] layer2 = new Particle[100];
        public Background(Rectangle rec, List<BaseObject> list)
            : base(rec, list)
        {

            Position = new PointF(0, 0);
            ChangeBounds(_screenbounds.Width, _screenbounds.Height);
            for (int i = 0; i < layer1.Length; i++)
            {
                layer1[i] = new Particle(_screenbounds, _list, r.Next(6) + 3, Color.White);
                layer1[i].Position = new PointF(r.Next(_screenbounds.Width), r.Next(_screenbounds.Height));
            }
            for (int i = 0; i < layer2.Length; i++)
            {
                layer2[i] = new Particle(_screenbounds, _list, r.Next(3) + 1, Color.Wheat);
                layer2[i].Position = new PointF(r.Next(_screenbounds.Width), r.Next(_screenbounds.Height));
            }

        }
        public override void Update()
        {
            if (!_visible) return;
            base.Update();
            for (int i = 0; i < layer1.Length; i++)
            {
                layer1[i].Update();
            }
            for (int i = 0; i < layer2.Length; i++)
            {
                layer2[i].Update();
            }
        }
        public override void Draw(Graphics g)
        {
            if (!_visible) return;
            base.Draw(g);
            for (int i = 0; i < layer1.Length; i++)
            {
                layer1[i].Draw(g);
            }
            for (int i = 0; i < layer2.Length; i++)
            {
                layer2[i].Draw(g);
            }
        }
    }
    public class PowerUp : Ship
    {

        public PowerUp(Rectangle rec, List<BaseObject> list, PointF pos)
            : base(rec, list, false)
        {
            ChangeBounds(10, 10);
            Rotation = (float)(Math.PI / 2);
            Speed = 2f;
            Position = pos;
            ChangePen(Color.Yellow, 3f);

        }
        public override void Draw(Graphics g)
        {
            if (!_visible) return;
            base.Draw(g);
            g.DrawEllipse(_pen, Bounds);

        }
        public override void Update()
        {
            if (!_visible) return;
            base.Update();
            Move();
            BaseObject o;
            if ((o = CheckCollision<Ship>()) != null)
            {
                if (o is Fighter)
                {
                    ((Fighter)o).PointCount += 20;

                    Remove();
                    o.Hitpoints += 10;
                    ((Fighter)o).FirePower += 1;
                }
            }

        }

    }

}


namespace Peter
{
    partial class FighterGame
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
            this.Display.Size = new System.Drawing.Size(590, 570);
            this.Display.TabIndex = 0;
            this.Display.TabStop = false;
            this.Display.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Display_MouseMove);
            this.Display.Click += new System.EventHandler(this.Display_Click);
            this.Display.Paint += new System.Windows.Forms.PaintEventHandler(this.Display_Paint);
            // 
            // FighterGame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 570);
            this.Controls.Add(this.Display);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FighterGame";
            this.Text = "FighterGame";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FighterGame_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FighterGame_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.Display)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox Display;
    }
}

