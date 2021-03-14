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
using System.Windows.Forms;
using System.Drawing;

namespace Peter
{
    public class GListBox : ListBox
    {
        private ImageList _myImageList;

        public ImageList ImageList
        {
            get { return _myImageList; }
            set { _myImageList = value; }
        }

        public GListBox ()
        {
            // Set owner draw mode
            this.DrawMode = DrawMode.OwnerDrawFixed;
            this.BorderStyle = BorderStyle.FixedSingle;
        }

        protected override void OnDrawItem (System.Windows.Forms.DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.DrawFocusRectangle();
            if (this.Items.Count > 0)
            {
                GListBoxItem item;
                Rectangle bounds = e.Bounds;
                Size imageSize = _myImageList.ImageSize;
                try
                {
                    item = (GListBoxItem)Items[e.Index];
                    if (item.ImageIndex != -1)
                    {
                        ImageList.Draw(e.Graphics, bounds.Left, bounds.Top, item.ImageIndex);
                        e.Graphics.DrawString(item.Text, e.Font, new SolidBrush(e.ForeColor),
                            bounds.Left + imageSize.Width, bounds.Top);
                    }
                    else
                    {
                        e.Graphics.DrawString(item.Text, e.Font, new SolidBrush(e.ForeColor),
                            bounds.Left, bounds.Top);
                    }
                }
                catch
                {
                    if (e.Index != -1)
                    {
                        e.Graphics.DrawString(Items[e.Index].ToString(), e.Font,
                            new SolidBrush(e.ForeColor), bounds.Left, bounds.Top);
                    }
                    else
                    {
                        e.Graphics.DrawString(Text, e.Font, new SolidBrush(e.ForeColor),
                            bounds.Left, bounds.Top);
                    }
                }
            }
            base.OnDrawItem(e);
        }
    }

    public class GListBoxItem
    {
        private string _myText;
        private int _myImageIndex;
        // properties 
        public string Text
        {
            get { return _myText; }
            set { _myText = value; }
        }
        public int ImageIndex
        {
            get { return _myImageIndex; }
            set { _myImageIndex = value; }
        }
        //constructor
        public GListBoxItem (string text, int index)
        {
            _myText = text;
            _myImageIndex = index;
        }
        public GListBoxItem (string text) : this(text, -1) { }
        public GListBoxItem () : this("") { }
        public override string ToString ()
        {
            return _myText;
        }
    }
}
