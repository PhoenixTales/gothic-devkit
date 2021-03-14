// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageSender.cs" >
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
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.InteropServices;

    internal class MessageSender
    {
        private static readonly uint WM_COMMAND = 0x0111;

        public bool SendCommand(string processName, int resourceId)
        {
            Process process = Process.GetProcessesByName(processName).FirstOrDefault();
            if (process != null)
            {
                if (process.MainWindowHandle != IntPtr.Zero)
                {
                    SendMessage(process.MainWindowHandle, WM_COMMAND, (IntPtr)resourceId, (IntPtr)0);
                    return true;
                }
            }

            return false;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
    }
}