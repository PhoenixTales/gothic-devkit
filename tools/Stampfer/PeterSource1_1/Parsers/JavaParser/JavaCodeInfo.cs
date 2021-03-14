using System;
using System.Collections;

namespace Peter.JavaParser
{
    public class JavaCodeInfo
    {
        private ArrayList m_Imports;
        private ArrayList m_NameSpaces;
        private ArrayList m_Fields;
        private ArrayList m_Methods;
        private ArrayList m_Constructors;

        public JavaCodeInfo()
        {
            this.m_Imports = new ArrayList();
            this.m_NameSpaces = new ArrayList();
            this.m_Fields = new ArrayList();
            this.m_Methods = new ArrayList();
            this.m_Constructors = new ArrayList();
        }

        /// <summary>
        /// Gets or Sets the List of 'using ...' in the code...
        /// </summary>
        public ArrayList Imports
        {
            get { return this.m_Imports; }

            set { this.m_Imports = value; }
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
        /// Gets or Sets the List of Constructors in the code...
        /// </summary>
        public ArrayList Constructors
        {
            get { return this.m_Constructors; }

            set { this.m_Constructors = value; }
        }
    }
}
