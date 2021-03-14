using System;
using System.Collections.Generic;
using System.Text;

namespace Peter.CSSParser
{
    /// <summary></summary>
    public class Selector
    {
        private List<Tag> tags = new List<Tag>();
        private List<Property> properties = new List<Property>();

        /// <summary></summary>
        public List<Tag> Tags
        {
            get { return tags; }
            set { tags = value; }
        }

        /// <summary></summary>
        public List<Property> Properties
        {
            get { return properties; }
            set { properties = value; }
        }
    }
}
