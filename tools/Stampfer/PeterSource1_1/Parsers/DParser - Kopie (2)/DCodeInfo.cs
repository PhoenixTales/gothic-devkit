using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Peter.DParser
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

    public class DCodeInfo
    {
        private ArrayList m_VarDeclarations;
        private ArrayList m_Functions;
        private ArrayList m_Instances;
        private ArrayList m_ConstDeclarations;
       

        public DCodeInfo()
        {
            this.m_VarDeclarations = new ArrayList();
            this.m_ConstDeclarations = new ArrayList();
            this.m_Functions = new ArrayList();
            this.m_Instances = new ArrayList();
           
        }

        /// <summary>
        /// Gets or Sets the List of 'using ...' in the code...
        /// </summary>
        

        public ArrayList VarDeclarations
        {
            get { return this.m_VarDeclarations; }

            set { this.m_VarDeclarations = value; }
        }
        public ArrayList ConstDeclarations
        {
            get { return this.m_ConstDeclarations; }

            set { this.m_ConstDeclarations = value; }
        }

        /// <summary>
        /// Gets or Sets the List of 'namespace ...' in the code...
        /// </summary>
        public ArrayList Functions
        {
            get { return this.m_Functions; }

            set { this.m_Functions = value; }
        }

        /// <summary>
        /// Gets or Sets the List of Fields in the code...
        /// </summary>
        public ArrayList Instances
        {
            get { return this.m_Instances; }

            set { this.m_Instances = value; }
        }

        
    }
}
