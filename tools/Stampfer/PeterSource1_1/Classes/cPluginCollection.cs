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
using System.Collections;
using PeterInterface;

namespace Peter
{
    public class cPluginCollection : CollectionBase
    {
        public cPluginCollection()
        {
        }

        #region Add

        /// <summary>
        /// Adds a IPeterPlugin to the collection...
        /// </summary>
        /// <param name="page">IPeterPlugin to add...</param>
        /// <returns>Added IPeterPlugin's index</returns>
        public int Add(IPeterPlugin plugin)
        {
            // Make sure the plugin is not null...
            if (plugin == null)
            {
                throw new ArgumentNullException("IPeterPlugin", "IPeterPlugin is null!");
            }

            return this.List.Add(plugin);
        }

        #endregion

        #region Remove

        /// <summary>
        /// Removes a IPeterPlugin from the Collection...
        /// </summary>
        /// <param name="page">IPeterPlugin to remove...</param>
        public void Remove(IPeterPlugin plugin)
        {
            // Get the index of the plugin...
            int index = this.IndexOf(plugin);

            // Make sure it is a valid index...
            if (index != -1)
            {
                // Remove Page...
                this.RemoveAt(index);
            }
        }

        /// <summary>
        /// Removes all IPeterPlugins from the collection...
        /// </summary>
        public new void Clear()
        {
            if (this.Count == 0)
            {
                // Nothing to do, return...
                return;
            }

            // Clear the list...
            base.Clear();

            // Rest the Capacity...
            this.InnerList.Capacity = 0;
        }

        /// <summary>
        /// Gets the index of a specified IPeterPlugin...
        /// </summary>
        /// <param name="page">IPeterPlugin to get index for...</param>
        /// <returns>Index of IPeterPlugin</returns>
        public int IndexOf(IPeterPlugin plugin)
        {
            // Look for the IPeterPlugin...
            for (int a = 0; a < this.Count; a++)
            {
                if (this[a] == plugin)
                {
                    // Found it...
                    return a;
                }
            }

            // Not found...
            return -1;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a IPeterPlugin at the requested Index...
        /// </summary>
        /// <param name="index">Index of IPeterPlugin...</param>
        /// <returns>IPeterPlugin at requested Index...</returns>
        public IPeterPlugin this[int index]
        {
            get
            {
                // Make sure it is a valid index...
                if (index < 0 || index >= this.Count)
                {
                    return null;
                }

                // Return the tabber page...
                return this.List[index] as IPeterPlugin;
            }
        }

        #endregion
    }
}
