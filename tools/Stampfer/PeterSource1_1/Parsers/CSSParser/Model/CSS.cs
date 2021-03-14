using System;
using System.Collections.Generic;
using System.Text;

namespace Peter.CSSParser
{
    /// <summary></summary>
    public class CSS
    {
        private string filename;
        private List<Selector> selectors = new List<Selector>();

        /// <summary></summary>
        public string FileName
        {
            get { return filename; }
            set { filename = value; }
        }

        /// <summary></summary>
        public List<Selector> Selectors
        {
            get { return selectors; }
            set { selectors = value; }
        }
    }
}
