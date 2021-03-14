// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" >
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
    using System.Collections.ObjectModel;
    using System.Linq;

    internal class MainWindowViewModel
    {
        private readonly Config config;

        private readonly MessageSender winMessageSender;

        private string selectedEntry;

        private RelayCommand sendCommand;

        private string spacerProcessName;

        public MainWindowViewModel(MessageSender winMessageSender, Config config)
        {
            if (winMessageSender == null)
            {
                throw new ArgumentNullException(nameof(winMessageSender));
            }

            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            this.winMessageSender = winMessageSender;
            this.config = config;

            if (DataProvider.SpacerMenuResources.ContainsKey(config.LastSelectedItem ?? string.Empty))
            {
                this.SelectedEntry = this.config.LastSelectedItem;
            }

            this.SpacerProcessName = config.LastProcessName ?? string.Empty;

            // Prepare commands
            this.sendCommand =
                new RelayCommand(
                    (o) =>
                    winMessageSender.SendCommand(
                        this.SpacerProcessName, 
                        DataProvider.SpacerMenuResources[this.selectedEntry]));
        }

        public ObservableCollection<string> ListEntries
            => new ObservableCollection<string>(DataProvider.SpacerMenuResources.Keys.OrderBy(i => i));

        public string SelectedEntry
        {
            get
            {
                return this.selectedEntry;
            }

            set
            {
                this.selectedEntry = value;
                this.config.LastSelectedItem = value;
            }
        }

        public RelayCommand SendCommand
        {
            get
            {
                return this.sendCommand;
            }

            set
            {
                this.sendCommand = value;
            }
        }

        public string SpacerProcessName
        {
            get
            {
                return this.spacerProcessName;
            }

            set
            {
                this.spacerProcessName = value;
                this.config.LastProcessName = value;
            }
        }
    }
}