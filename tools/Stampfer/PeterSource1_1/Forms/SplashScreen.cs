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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Peter
{
    public partial class SplashScreen : Form
    {
        public SplashScreen(bool closeButton)
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
            //this.BackColor = SystemColors.Control;
            //this.TransparencyKey = SystemColors.Control;
            this.pictureBox1.BackColor = Color.Transparent;
           
            this._btnClose.Visible = closeButton;
        }

        private void _btnClose_Click (object sender, EventArgs e)
        {
            this.Close();
        }

        private void SplashScreen_Load(object sender, EventArgs e)
        {
            int exstyle = Win32.GetWindowLong(this.Handle, Win32.GWL_EXSTYLE);
            exstyle |= Win32.WS_EX_TRANSPARENT;
            Win32.SetWindowLong(this.Handle, Win32.GWL_EXSTYLE, exstyle);
            
        }

        private void SplashScreen_Paint(object sender, PaintEventArgs e)
        {
            Rectangle rect = this.RectangleToScreen(this.ClientRectangle);
            e.Graphics.CopyFromScreen(new Point(rect.Location.X + e.ClipRectangle.Location.X, rect.Location.Y + e.ClipRectangle.Location.Y), e.ClipRectangle.Location, this.ClientRectangle.Size);
            
        }

       

        

        

        
        

        
       

        
    }
    public class Win32
    {


        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int vKey);
        [DllImport("user32.dll")]
        public static extern bool GetKeyboardState(byte[] lpKeyState);

        public const int GWL_EXSTYLE = -20;
        public const int WS_EX_TRANSPARENT = 0x20;
    }
}