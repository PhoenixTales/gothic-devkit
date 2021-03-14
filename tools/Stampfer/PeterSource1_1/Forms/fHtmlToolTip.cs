/**************************************************************************************
    Stampfer - Gothic Script Editor
    Copyright (C) 2008 Jpmon1

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
//using mshtml;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Peter
{
    public partial class fHtmlToolTip : Form
    {
        public fHtmlToolTip()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or Sets the WebBrowser Control...
        /// </summary>
        public WebBrowser WebBrowser
        {
            get { return this.WebMain; }
            set { this.WebMain = value; }
        }

        protected override bool ShowWithoutActivation
        {
            get
            {
                return true;// base.ShowWithoutActivation;
            }
        }

        public bool CanClose ()
        {
            return this.DesktopBounds.Contains(MousePosition);
        }
    }
}