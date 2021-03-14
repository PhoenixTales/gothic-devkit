using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Peter.CParser
{
    public class CCodeInfo
    {
        private ArrayList m_Includes;
        private ArrayList m_GlobalVars;
        private ArrayList m_Functions;
        private ArrayList m_Prototypes;
        private ArrayList m_Defines;
        private ArrayList m_Structs;
        private ArrayList m_TypeDefs;

        public CCodeInfo()
        {
            this.m_Includes = new ArrayList();
            this.m_GlobalVars = new ArrayList();
            this.m_Functions = new ArrayList();
            this.m_Prototypes = new ArrayList();
            this.m_Defines = new ArrayList();
            this.m_Structs = new ArrayList();
            this.m_TypeDefs = new ArrayList();
        }

        /// <summary>
        /// Gets or Sets the List of 'includes ...' in the code...
        /// </summary>
        public ArrayList Includes
        {
            get { return this.m_Includes; }

            set { this.m_Includes = value; }
        }

        /// <summary>
        /// Gets or Sets the List of Global Variables in the code...
        /// </summary>
        public ArrayList GlobalVariables
        {
            get { return this.m_GlobalVars; }

            set { this.m_GlobalVars = value; }
        }

        /// <summary>
        /// Gets or Sets the List of Functions in the code...
        /// </summary>
        public ArrayList Functions
        {
            get { return this.m_Functions; }

            set { this.m_Functions = value; }
        }

        /// <summary>
        /// Gets or Sets the List of Prototypes in the code...
        /// </summary>
        public ArrayList Prototypes
        {
            get { return this.m_Prototypes; }

            set { this.m_Prototypes = value; }
        }

        /// <summary>
        /// Gets or Sets the List of Defines in the Code...
        /// </summary>
        public ArrayList Defines
        {
            get { return this.m_Defines; }

            set { this.m_Defines = value; }
        }

        /// <summary>
        /// Gets or Sets the List of Structs in the Code...
        /// </summary>
        public ArrayList Structs
        {
            get { return this.m_Structs; }

            set { this.m_Structs = value; }
        }

        /// <summary>
        /// Gets or Sets the List of TypeDefs in the Code...
        /// </summary>
        public ArrayList TypeDefs
        {
            get { return this.m_TypeDefs; }

            set { this.m_TypeDefs = value; }
        }
    }
}
