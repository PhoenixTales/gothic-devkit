using System;
using System.Collections.Generic;
using System.Text;

namespace Peter.CSSParser
{
    /// <summary></summary>
    public class Tag
    {
        private TagType tagtype;
        private string name;
        private string cls;
        private string pseudo;
        private string id;
        private List<Tag> subtags = new List<Tag>();

        /// <summary></summary>
        public TagType TagType
        {
            get { return tagtype; }
            set { tagtype = value; }
        }

        /// <summary></summary>
        public bool IsIDSelector
        {
            //get { return (int)(this.tagtype & TagType.IDed) > 0; }
            get { return id != null; }
        }

        /// <summary></summary>
        public bool HasName
        {
            get { return name != null; }
        }

        /// <summary></summary>
        public bool HasClass
        {
            get { return cls != null; }
        }

        /// <summary></summary>
        public bool HasPseudoClass
        {
            get { return pseudo != null; }
        }

        /// <summary></summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary></summary>
        public string Class
        {
            get { return cls; }
            set { cls = value; }
        }

        /// <summary></summary>
        public string Pseudo
        {
            get { return pseudo; }
            set { pseudo = value; }
        }

        /// <summary></summary>
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary></summary>
        public List<Tag> SubTags
        {
            get { return subtags; }
            set { subtags = value; }
        }

        /// <summary></summary>
        /// <returns></returns>
        public override string ToString()
        {
            System.Text.StringBuilder txt = new System.Text.StringBuilder(ToShortString());

            foreach (Tag t in subtags)
            {
                txt.Append(" ");
                txt.Append(t.ToString());
            }
            return txt.ToString();
        }

        /// <summary></summary>
        /// <returns></returns>
        public string ToShortString()
        {
            System.Text.StringBuilder txt = new System.Text.StringBuilder();
            if (HasName)
            {
                txt.Append(name);
            }
            if (HasClass)
            {
                txt.Append(".");
                txt.Append(cls);
            }
            if (IsIDSelector)
            {
                txt.Append("#");
                txt.Append(id);
            }
            if (HasPseudoClass)
            {
                txt.Append(":");
                txt.Append(pseudo);
            }
            return txt.ToString();
        }
    }
}
