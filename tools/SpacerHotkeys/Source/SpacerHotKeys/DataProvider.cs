// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataProvider.cs" >
// -
// Copyright (C) 2017 J.Vogel
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// -
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
// GNU General Public License for more details.
// -
// You should have received a copy of the GNU General Public License
// along with this program.If not, see<http://www.gnu.org/licenses/>.
// -
// </copyright>
// <summary>
// </summary>
//  -------------------------------------------------------------------------------------------------------------------
// http://www.gnu.org/licenses/>.
namespace SpacerHotKeys
{
    using System;
    using System.Collections.Generic;

    internal static class DataProvider
    {
        /// <summary>
        /// Gets the a dictionary with the Spacer Menu Resources.
        /// </summary>
        public static Dictionary<string, int> SpacerMenuResources
        {
            get
            {
                var dict = new Dictionary<string, int>();
                string[] lines = Properties.Resources.MenuResources.Split(
                    new[] { Environment.NewLine }, 
                    StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    string[] parts = line.Split(';');
                    int id = int.Parse(parts[1]);
                    dict.Add(parts[0], id);
                }

                return dict;
            }
        }
    }
}