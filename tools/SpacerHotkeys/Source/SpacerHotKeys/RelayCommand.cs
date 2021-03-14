// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RelayCommand.cs" >
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
    using System.Windows.Input;

    /// <summary>
    ///     A generic command class which uses delegates for the interface implementation.
    /// </summary>
    public class RelayCommand : ICommand
    {
        /// <summary>
        ///     The action.
        /// </summary>
        private readonly Action<object> action;

        /// <summary>
        ///     The can execute check.
        /// </summary>
        private readonly Func<object, bool> canExecuteCheck;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RelayCommand" /> class that can always be executed.
        /// </summary>
        /// <param name="action">Action that should be invoked when the command is executed.</param>
        public RelayCommand(Action<object> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            this.canExecuteCheck = ob => true;
            this.action = action;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="RelayCommand" /> class with a check if the command can be executed and
        ///     a <see cref="Action" /> that should be executed i the command is executed.
        /// </summary>
        /// <param name="canExecuteCheck">Function that can be used to check if the command can be executed.</param>
        /// <param name="action">Action that should be invoked when the command is executed.</param>
        public RelayCommand(Func<object, bool> canExecuteCheck, Action<object> action)
        {
            if (canExecuteCheck == null)
            {
                throw new ArgumentNullException(nameof(canExecuteCheck));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            this.canExecuteCheck = canExecuteCheck;
            this.action = action;
        }

        /// <summary>
        ///     The can execute changed event.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        /// <summary>
        ///     Checks if the command can be executed.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        /// <returns>True if the command can be executed.</returns>
        public bool CanExecute(object parameter)
        {
            return this.canExecuteCheck(parameter);
        }

        /// <summary>
        ///     Executes the command.
        /// </summary>
        /// <param name="parameter">Command parameter.</param>
        public void Execute(object parameter)
        {
            this.action(parameter);
        }
    }
}