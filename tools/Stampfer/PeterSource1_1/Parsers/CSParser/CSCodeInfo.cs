using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Peter.CSParser
{
    struct TokenMatch
    {
        public string Value;
        public int Position;

        public TokenMatch(string val, int pos)
        {
            this.Value = val;
            this.Position = pos;
        }
    }

    public class CSCodeInfo
    {
        private ArrayList m_Usings;
        private ArrayList m_NameSpaces;
        private ArrayList m_Fields;
        private ArrayList m_Methods;
        private ArrayList m_Properties;
        private ArrayList m_Constructors;

        public CSCodeInfo()
        {
            this.m_Usings = new ArrayList();
            this.m_NameSpaces = new ArrayList();
            this.m_Fields = new ArrayList();
            this.m_Methods = new ArrayList();
            this.m_Properties = new ArrayList();
            this.m_Constructors = new ArrayList();
        }

        /// <summary>
        /// Gets or Sets the List of 'using ...' in the code...
        /// </summary>
        public ArrayList Usings
        {
            get { return this.m_Usings; }

            set { this.m_Usings = value; }
        }

        /// <summary>
        /// Gets or Sets the List of 'namespace ...' in the code...
        /// </summary>
        public ArrayList NameSpaces
        {
            get { return this.m_NameSpaces; }

            set { this.m_NameSpaces = value; }
        }

        /// <summary>
        /// Gets or Sets the List of Fields in the code...
        /// </summary>
        public ArrayList Fields
        {
            get { return this.m_Fields; }

            set { this.m_Fields = value; }
        }

        /// <summary>
        /// Gets or Sets the List of Methods in the code...
        /// </summary>
        public ArrayList Methods
        {
            get { return this.m_Methods; }

            set { this.m_Methods = value; }
        }

        /// <summary>
        /// Gets or Sets the List of Properties in the code...
        /// </summary>
        public ArrayList Properties
        {
            get { return this.m_Properties; }

            set { this.m_Properties = value; }
        }

        /// <summary>
        /// Gets or Sets the List of Constructors in the code...
        /// </summary>
        public ArrayList Constructors
        {
            get { return this.m_Constructors; }

            set { this.m_Constructors = value; }
        }
    }
}
