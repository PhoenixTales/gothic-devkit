/**************************************************************************************
    Stampfer - Gothic Script Editor
    Copyright (C) 2008  Jpmon1

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
using System.Collections;
using System.IO;

namespace Peter
{
    public class cDirectorySorter : IComparer
    {
        public int Compare(object x, object y)
        {
            DirectoryInfo dir1 = (DirectoryInfo)x;
            DirectoryInfo dir2 = (DirectoryInfo)y;
            return dir1.Name.CompareTo(dir2.Name);
        }
    }

    public class cFileSorter : IComparer
    {
        public int Compare(object x, object y)
        {
            FileInfo file1 = (FileInfo)x;
            FileInfo file2 = (FileInfo)y;
            return file1.Name.CompareTo(file2.Name);
        }
    }
}
