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
//using mshtml;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Peter
{
   /* public class Webber : WebBrowser
    {
        /*private const int WM_HSCROLL = 0x114;
        private const int WM_VSCROLL = 0x115;
        private const int WM_MOUSEWHEEL = 0x020A;
        private const int WM_NCCALCSIZE = 0x0083;
        private const int WM_PAINT = 0x000F;
        private const int WM_SIZE = 0x0005;
        private const int WM_PARENTNOTIFY = 0x210;
        private const int WM_IME_SETCONTEXT = 0x281;
        private const int WM_MOUSEACTIVATE = 0x21;

        public Webber()
        {
            this.Navigate("about:Blank");
            while (this.Document.Body == null) Application.DoEvents();
        }

        /// <summary>
        /// Get the current selection.
        /// </summary>
        public string Selection
        {
            get
            {
                IHTMLDocument2 doc = (IHTMLDocument2)this.Document.DomDocument;
                IHTMLSelectionObject sel = doc.selection;
                IHTMLTxtRange range = (IHTMLTxtRange)sel.createRange();
                if (range.text == null)
                    return "";
                else
                    return range.text.Trim();
            }
        }

        /// <summary>
        /// Sets the HTML of the document giving the ability to use CSS and scripts.
        /// </summary>
        public string HTML
        {
            set
            {
                IHTMLDocument2 doc = (IHTMLDocument2)this.Document.DomDocument;
                doc.write(value);
                doc.close();
            }
        }

        /// <summary>
        /// Select the word clicked.
        /// </summary>
        public void SelectWord()
        {
            IHTMLDocument2 doc = (IHTMLDocument2)this.Document.DomDocument;
            IHTMLSelectionObject sel = (IHTMLSelectionObject)doc.selection;
            IHTMLTxtRange rng = (IHTMLTxtRange)sel.createRange();
            rng.expand("word");
            rng.select();
        }

        /// <summary>
        /// Clear any selection.
        /// </summary>
        public void ClearSelection()
        {
            IHTMLDocument2 doc = (IHTMLDocument2)this.Document.DomDocument;
            IHTMLSelectionObject sel = (IHTMLSelectionObject)doc.selection;
            sel.empty();
        }

        /// <summary>
        /// Copy Current Selection.
        /// </summary>
        public void Copy()
        {
            this.Document.ExecCommand("Copy", false, null);
            ClearSelection();
        }

        /// <summary>
        /// Paste data from the clipboard.
        /// </summary>
        public void Paste()
        {
            this.Document.ExecCommand("Paste", false, null);
        }

        /// <summary>
        /// Cut Current Selection.
        /// </summary>
        public void Cut()
        {
            this.Document.ExecCommand("Cut", false, null);
        }

        /// <summary>
        /// Select Everything in the Document.
        /// </summary>
        public void SelectAll()
        {
            this.Document.ExecCommand("SelectAll", false, null);
        }

        /// <summary>
        /// Deletes the Current Selection...
        /// </summary>
        public void Delete()
        {
            this.Document.ExecCommand("Delete", false, null);
        }

        /// <summary>
        /// Find the first occurance of the given text.
        /// </summary>
        /// <param name="text">Text to find.</param>
        /// <returns>Found or not Found</returns>
        public bool FindFirst(string text)
        {
            IHTMLDocument2 doc = (IHTMLDocument2)this.Document.DomDocument;
            IHTMLSelectionObject sel = (IHTMLSelectionObject)doc.selection;
            sel.empty(); // get an empty selection, so we start from the beginning
            IHTMLTxtRange rng = (IHTMLTxtRange)sel.createRange();
            if (rng.findText(text, 1000000000, 0))
            {
                rng.select();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Finds occurances of the given text.
        /// </summary>
        /// <param name="text">Text to find.</param>
        /// <returns>Found or not Found</returns>
        public bool FindNext(string text)
        {
            IHTMLDocument2 doc = (IHTMLDocument2)this.Document.DomDocument;
            IHTMLSelectionObject sel = (IHTMLSelectionObject)doc.selection;
            IHTMLTxtRange rng = (IHTMLTxtRange)sel.createRange();
            rng.collapse(false); // collapse the current selection so we start from the end of the previous range
            if (rng.findText(text, 1000000000, 0))
            {
                rng.select();
                return true;
            }
            else
            {
                return false;
            }
        }
    }*/
}
