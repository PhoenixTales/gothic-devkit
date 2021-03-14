// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" >
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
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Interop;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int HotkeyId = 9000;

        private static readonly string ConfPath = Path.Combine(Environment.CurrentDirectory, "config.xml");

        private static Config conf;

        private readonly MessageSender sender;

        private readonly MainWindowViewModel viewModel;

        private HwndSource source;

        public MainWindow()
        {
            this.InitializeComponent();

            conf = Config.Load(ConfPath) ?? new Config();

            this.sender = new MessageSender();
            this.viewModel = new MainWindowViewModel(this.sender, conf);
            this.DataContext = this.viewModel;

            this.Loaded += this.MainWindowLoaded;
            this.Closing += this.MainWindowClosing;
        }

        [DllImport("User32.dll")]
        private static extern bool RegisterHotKey([In] IntPtr hWnd, [In] int id, [In] uint fsModifiers, [In] uint vk);

        [DllImport("User32.dll")]
        private static extern bool UnregisterHotKey([In] IntPtr hWnd, [In] int id);

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WmHotkey = 0x0312;
            switch (msg)
            {
                case WmHotkey:
                    switch (wParam.ToInt32())
                    {
                        case HotkeyId:
                            this.OnHotKeyPressed();
                            handled = true;
                            break;
                    }

                    break;
            }

            return IntPtr.Zero;
        }

        private void MainWindowClosing(object sender, CancelEventArgs e)
        {
            try
            {
                conf?.Save(ConfPath);
                this.source.RemoveHook(this.HwndHook);
                this.source = null;
                this.UnregisterHotKey();
            }
            catch
            {
                // Do nothing if we can not save the config.
                Environment.Exit(1);
            }
        }

        private void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            var helper = new WindowInteropHelper(this);
            this.source = HwndSource.FromHwnd(helper.Handle);
            this.source.AddHook(this.HwndHook);
            this.RegisterHotKey();
        }

        private void OnHotKeyPressed()
        {
            this.viewModel.SendCommand.Execute(null);
        }

        private void RegisterHotKey()
        {
            var helper = new WindowInteropHelper(this);
            const uint VkF1 = 0x70;
            const uint ModAlt = 0x0001;
            if (!RegisterHotKey(helper.Handle, HotkeyId, ModAlt, VkF1))
            {
                // handle error
            }
        }

        private void UnregisterHotKey()
        {
            var helper = new WindowInteropHelper(this);
            UnregisterHotKey(helper.Handle, HotkeyId);
        }
    }
}