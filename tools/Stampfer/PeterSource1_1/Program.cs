/**************************************************************************************
    Stampfer - Gothic Script Editor
    Copyright (C) 2008, 2009 Jpmon1, Alexander "Sumpfkrautjunkie" Ruppert

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
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;

using System.Runtime.Remoting.Channels.Ipc;    //Importing IPC
//channel
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting;

namespace Peter
{

    public interface ISharedAssemblyInterface
    {
        void Write(string a);
        void SetHandle(IntPtr h);
        IntPtr GetHandle();
        string Read();

    }
    public class MyRemoteObject : MarshalByRefObject,
               ISharedAssemblyInterface
    {
        private string content="";
        private IntPtr handle = IntPtr.Zero;
        public MyRemoteObject()
        {

        }
        public override object InitializeLifetimeService()
        {
            return null;
        }
        public void Write(string a)
        {
            content = a;
        }
        public string Read()
        {
            return content;
        }

        public void SetHandle(IntPtr h)
        {
            handle = h;
        }
        public IntPtr GetHandle()
        {
            return handle;
        }

    }
    

    static class Program
    {
        static Mutex mutex;
        const int SW_RESTORE = 9;
        private const int WM_COPYDATA = 0x4A;

        [DllImport("user32.dll")]
        private static extern int ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern int SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern int IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        private static extern int PostMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        /// <summary>
        /// GetCurrentInstanceWindowHandle
        /// </summary>
        /// <returns></returns>
        private static IntPtr GetCurrentInstanceWindowHandle()
        {
            IntPtr hWnd = IntPtr.Zero;
            /*//MessageBox.Show(hWnd.ToString());
            Process process = Process.GetCurrentProcess();
            
            Process[] processes = Process.GetProcessesByName(process.ProcessName);
            DateTime smallest=process.StartTime;
            foreach (Process _process in processes)
            {
                // Get the first instance that is not this instance, has the
                // same process name and was started from the same file name
                // and location. Also check that the process has a valid
                // window handle in this session to filter out other user's
                // processes.
                /*if (_process.Id != process.Id &&
                    _process.StartTime < smallest &&
                    _process.MainModule.FileName == process.MainModule.FileName &&
                    _process.MainWindowHandle != IntPtr.Zero)*
                if (_process.StartTime < smallest &&
                    _process.MainModule.FileName == process.MainModule.FileName &&
                    _process.MainWindowHandle != IntPtr.Zero)
                {
                    smallest = _process.StartTime;
                    hWnd = _process.MainWindowHandle;                    
                    //break;
                }
            }*/
            Process[] currentProcesses =
        Process.GetProcessesByName("Stampfer");
            hWnd = currentProcesses[0].MainWindowHandle;
            //MessageBox.Show(hWnd.ToString());           
            return hWnd;
        }

        /// <summary>
        /// SwitchToCurrentInstance
        /// </summary>

        private static bool ReadIPC(string data,ref IntPtr hWnd)
        {
            try
            {

                IpcChannel ipcCh = new IpcChannel("myClient");
                ChannelServices.RegisterChannel(ipcCh, false);

                ISharedAssemblyInterface obj =
                   (ISharedAssemblyInterface)Activator.GetObject
                   (typeof(ISharedAssemblyInterface),
                    "ipc://IPChannelName/SreeniRemoteObj");
                obj.Write(data);
                hWnd = obj.GetHandle();
                ChannelServices.UnregisterChannel(ipcCh);
                //Thread.Sleep(100);
                PostMessage(hWnd, 10001, IntPtr.Zero, IntPtr.Zero);
                //MessageBox.Show(data);
                return true;
            }
            catch
            {
                
                //Application.Exit();
                return false;
            }
        }
        private static void SwitchToCurrentInstance(string[] args)
        {
            
            IntPtr hWnd = IntPtr.Zero;// = /*(IntPtr)0x0FFFF;*/ GetCurrentInstanceWindowHandle();
           
            
            //if (hWnd != IntPtr.Zero)
            {
                
                // Send arguments...
                if (args.Length > 0)
                {
                    string data = args[0];
                    /*foreach (string arg in args)
                    {
                        data += arg + "|";

                        
                       
                    }
                    /*CopyDataStruct str = new CopyDataStruct(data);
                    IntPtr pnt = Marshal.AllocHGlobal(Marshal.SizeOf(str));
                    Marshal.StructureToPtr(str, pnt, true);
                    MessageBox.Show(data);                    
                    SendMessage(hWnd, 10001, IntPtr.Zero, pnt);*/

                    ReadIPC(data, ref hWnd);                    
                        Thread.Sleep(10);
                    
                    
                }

                // Restore window if minimised. Do not restore if already in
                // normal or maximised window state, since we don't want to
                // change the current state of the window.
                if (hWnd != IntPtr.Zero)
                {
                    if (IsIconic(hWnd) != 0)
                    {
                        ShowWindow(hWnd, SW_RESTORE);
                    }

                    // Set foreground window.
                    SetForegroundWindow(hWnd);
                }
                Application.Exit();
            }
        }

        /// <summary>
        /// check if given exe alread running or not
        /// </summary>
        /// <returns>returns true if already running</returns>
        private static bool IsAlreadyRunning()
        {
            
            string strLoc = Assembly.GetExecutingAssembly().Location;
            FileSystemInfo fileInfo = new FileInfo(strLoc);
            string sExeName = fileInfo.Name;
            bool bCreatedNew;
            
            mutex = new Mutex(true, "Global\\" + sExeName, out bCreatedNew);
            
            if (bCreatedNew)
                mutex.ReleaseMutex();
            
            return !bCreatedNew;
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 

        
        [STAThread]
        static void Main(string[] args)
        {
            
            if (IsAlreadyRunning()/* && args.Length > 0*/)
            {
                
                SwitchToCurrentInstance(args);
            }
            else
            {
                IpcChannel ipcCh = new IpcChannel("IPChannelName");

                ChannelServices.RegisterChannel(ipcCh,false);
                RemotingConfiguration.RegisterWellKnownServiceType
                   (typeof(MyRemoteObject),
                           "SreeniRemoteObj",
                           WellKnownObjectMode.Singleton);

                Application.EnableVisualStyles();
               
                Application.Run(new MainForm(args));
            }
        }

        static void myReceive(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                Console.WriteLine("MSG " + i.ToString() + ": " + args[i]);
            }
        }
    }

    public struct CopyDataStruct
    {
        public int dwData;
        public int cbData;
        public string lpData;

        public CopyDataStruct(string data)
        {
            this.lpData = data + "\0";
            this.cbData = data.Length;
            this.dwData = 0;
        }
    }
}
