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
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Forms;
//using mshtml;
using System.Text;

namespace Peter
{/*
    public class cHtmlToolTip
    {
        private string m_HTML;
        private Timer m_Timer;
        private bool m_Showing;
        private IHtmlInterface m_Parent;
        private fHtmlToolTip m_ToolTip;
        private Form m_Shadow;

        private const int SW_SHOWNOACTIVATE = 4;
        private const int HWND_TOPMOST = -1;
        private const int SWP_NOACTIVATE = 0x0010;

        [DllImport("User32")]
        private static extern int ShowWindow (int hwnd, int nCmdShow);

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        static extern bool SetWindowPos (
            int hWnd,               // handle to window
            int hWndInsertAfter,    // placement-order handle
            int X,                  // horizontal position
            int Y,                  // vertical position
            int cx,                 // width
            int cy,                 // height
            uint uFlags             // window-positioning options
        );

        public cHtmlToolTip(IHtmlInterface parent)
        {
            Startup(parent);
        }

        public cHtmlToolTip(string html, Point location, IHtmlInterface parent)
        {
            Startup(parent);
            this.HTML = html;
            this.m_ToolTip.Location = location;
        }

        private void Startup(IHtmlInterface parent)
        {
            this.m_Showing = false;
            this.m_ToolTip = new fHtmlToolTip();
            this.m_ToolTip.WebBrowser.Navigate("about:Blank");
            while (this.m_ToolTip.WebBrowser.Document.Body == null) Application.DoEvents();
            this.m_ToolTip.WebBrowser.Document.Click += new HtmlElementEventHandler(Document_Click);
            this.m_Timer = new Timer();
            this.m_Timer.Interval = 5000;
            this.m_Timer.Tick += new EventHandler(TimerTick);
            this.m_Parent = parent;

            this.m_Shadow = new Form();
            this.m_Shadow.ShowInTaskbar = false;
            this.m_Shadow.FormBorderStyle = FormBorderStyle.None;
            this.m_Shadow.BackColor = Color.Black;
            this.m_Shadow.GotFocus += new EventHandler(Shadow_GotFocus);
        }

        void Shadow_GotFocus (object sender, EventArgs e)
        {
            this.m_ToolTip.Focus();
        }

        void Document_Click (object sender, HtmlElementEventArgs e)
        {
            if (this.m_Parent != null)
            {
                if (this.m_ToolTip.WebBrowser.Document.ActiveElement.TagName.Equals("A"))
                {
                    this.m_Parent.LinkClick(this.m_ToolTip.WebBrowser.Document.ActiveElement);
                    this.m_Shadow.Hide();
                    this.m_ToolTip.Hide();
                }
            }
        }

        void TimerTick (object sender, EventArgs e)
        {
            if (!this.m_ToolTip.CanClose())
            {
                this.m_Shadow.Hide();
                this.m_ToolTip.Hide();
                this.m_Timer.Stop();
            }
        }

        /// <summary>
        /// Sets the HTML of the document giving the ability to use CSS and scripts.
        /// </summary>
        private string Doc_HTML
        {
            set
            {
                IHTMLDocument2 doc = (IHTMLDocument2)this.m_ToolTip.WebBrowser.Document.DomDocument;
                doc.write(value);
                doc.close();
            }
        }

        /// <summary>
        /// Gets or Sets the HTML of the ToolTip...
        /// </summary>
        public string HTML
        {
            get { return this.m_HTML; }

            set
            {
                this.m_HTML = value;
                StringBuilder sb = new StringBuilder();
                sb.Append("<html><head><title>ToolTip</title>");
                sb.Append("<style type=\"text/css\">");
                sb.Append("table {margin:1px;font-family: sans-serif, \"Microsoft Sans Serif\", Arial, Verdana, Tahoma; font-size:75%;}");
                sb.Append("a:link {color:black} a:visited {color:black} a:hover {color:black}");
                sb.Append("ul ul { list-style: none; padding-left: 15px; } ul{ margin-left: 0px; padding: 0px; }");
                sb.Append("</style>");
                sb.Append("</head><body style=\"background: #ffffe1;margin:3px;;font-size:85%\">");
                sb.Append(value);
                sb.Append("</body></html>");
                this.Doc_HTML = sb.ToString();
            }
        }

        /// <summary>
        /// Get or Sets the Delay of the Tool Tip
        /// </summary>
        public int Delay
        {
            get { return this.m_Timer.Interval; }

            set
            {
                if (this.m_Timer.Enabled)
                {
                    this.m_Timer.Stop();
                }
                this.m_Timer.Interval = value;
            }
        }

        /// <summary>
        /// Gets or Sets if the Tool Tip if Visible...
        /// </summary>
        public bool Visible
        {
            get { return this.m_ToolTip.Visible; }
            set { this.m_ToolTip.Visible = value; }
        }

        /// <summary>
        /// Gets or Sets the Location of the Tool Tip...
        /// </summary>
        public Point Location
        {
            get { return this.m_ToolTip.Location; }
            set { this.m_ToolTip.Location = value; }
        }

        /// <summary>
        /// Hides the Tool Tip...
        /// </summary>
        public void Hide ()
        {

            Timer tHide = new Timer();
            tHide.Interval = 10;
            tHide.Tick += new EventHandler(tHideTick);
            tHide.Start();
        }

        void tHideTick (object sender, EventArgs e)
        {
            if (this.m_Showing)
            {
                Timer t = (Timer)sender;
                t.Stop();
                return;
            }
            if (this.m_ToolTip.Opacity <= 0)
            {
                Timer t = (Timer)sender;
                t.Stop();
                this.m_Shadow.Hide();
                this.m_ToolTip.Hide();
            }
            else
            {
                this.m_Shadow.Opacity -= .10;
                this.m_ToolTip.Opacity -= .20;
            }
        }

        /// <summary>
        /// Shows the Tool Tip...
        /// </summary>
        public void Show ()
        {
            this.m_Showing = true;
            this.m_ToolTip.WebBrowser.ScrollBarsEnabled = false;
            this.m_ToolTip.Height = this.m_ToolTip.WebBrowser.Document.Body.ScrollRectangle.Height;
            this.m_ToolTip.Width = this.m_ToolTip.WebBrowser.Document.Body.ScrollRectangle.Width;

            int left = this.m_ToolTip.Location.X;
            int top = this.m_ToolTip.Location.Y;

            if (this.m_ToolTip.Height > 500)
            {
                this.m_ToolTip.Height = 500;
                this.m_ToolTip.WebBrowser.ScrollBarsEnabled = true;
            }

            if (this.m_Timer.Enabled)
            {
                this.m_Timer.Stop();
            }

            if (Screen.PrimaryScreen.Bounds.Right < (left + this.m_ToolTip.Width))
                left = Screen.PrimaryScreen.Bounds.Right - this.m_ToolTip.Width - 5;
            if (Screen.PrimaryScreen.Bounds.Bottom < (top + this.m_ToolTip.Height))
                top = Screen.PrimaryScreen.Bounds.Bottom - this.m_ToolTip.Height - 5;

            this.m_Timer.Start();

            this.m_ToolTip.Opacity = 0;
            this.m_Shadow.Opacity = 0;
            this.m_Shadow.Show();

            SetWindowPos(this.m_Shadow.Handle.ToInt32(), HWND_TOPMOST, left + 3,
                top + 3, this.m_ToolTip.Width, this.m_ToolTip.Height, SWP_NOACTIVATE);
            ShowWindow(this.m_Shadow.Handle.ToInt32(), SW_SHOWNOACTIVATE);

            SetWindowPos(this.m_ToolTip.Handle.ToInt32(), HWND_TOPMOST, left,
                top, this.m_ToolTip.Width, this.m_ToolTip.Height, SWP_NOACTIVATE);
            ShowWindow(this.m_ToolTip.Handle.ToInt32(), SW_SHOWNOACTIVATE);
            //this.m_ToolTip.Show();

            Timer tShow = new Timer();
            tShow.Interval = 20;
            tShow.Tick += new EventHandler(tShowTick);
            tShow.Start();
        }

        void tShowTick (object sender, EventArgs e)
        {
            if (this.m_ToolTip.Opacity >= 1)
            {
                Timer t = (Timer)sender;
                t.Stop();
                this.m_Showing = false;
            }
            else
            {
                this.m_ToolTip.Opacity += .10;
                this.m_Shadow.Opacity += .05;
            }
        }
    }*/
}
