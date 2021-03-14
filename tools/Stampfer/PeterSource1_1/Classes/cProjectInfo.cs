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
using System.Text;
using System.Windows.Forms;

namespace Peter
{
    /// <summary>
    /// This class Holds Information about a specific project...
    /// </summary>
    public class cProjectInfo
    {
        /// <summary>
        /// Path of Project on Disk...
        /// </summary>
        private string m_ProjectPath;

        /// <summary>
        /// Name of Project....
        /// </summary>
        private string m_ProjectName;

        /// <summary>
        /// Type of Project...
        /// </summary>
        private string m_ProjectType;

        /// <summary>
        /// TreeNode list of Project Data...
        /// </summary>
        private TreeNode m_ProjectData;

        /// <summary>
        /// Creates a new Project Info Class...
        /// </summary>
        public cProjectInfo ()
        {
            this.m_ProjectPath = "";
            this.m_ProjectName = "";
            this.m_ProjectType = "";
            this.m_ProjectData = null;
        }

        /// <summary>
        /// Gets or Sets the Path of the Project...
        /// </summary>
        public string Path
        {
            get { return this.m_ProjectPath; }
            set { this.m_ProjectPath = value; }
        }

        /// <summary>
        /// Gets or Sets the Name of the Project...
        /// </summary>
        public string Name
        {
            get { return this.m_ProjectName; }
            set { this.m_ProjectName = value; }
        }

        /// <summary>
        /// Gets or Sets the Type of the Project...
        /// </summary>
        public string Type
        {
            get { return this.m_ProjectType; }
            set { this.m_ProjectType = value; }
        }

        /// <summary>
        /// Gets or Sets the Data of the Project...
        /// </summary>
        public TreeNode Data
        {
            get { return this.m_ProjectData; }
            set { this.m_ProjectData = value; }
        }
    }
}
