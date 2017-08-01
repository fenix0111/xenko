// Copyright (c) 2014-2017 Silicon Studio Corp. All rights reserved. (https://www.siliconstudio.co.jp)
// See LICENSE.md for full license information.
using System;
using System.Windows;
using System.Windows.Input;
using SiliconStudio.Core.Annotations;

namespace SiliconStudio.Presentation.Commands
{
    internal class SystemCommand : ICommand
    {
        private readonly Func<Window, bool> canExecute;
        private readonly Action<Window> execute;

        internal SystemCommand([NotNull] Func<Window, bool> canExecute, [NotNull] Action<Window> execute)
        {
            if (canExecute == null) throw new ArgumentNullException(nameof(canExecute));
            if (execute == null) throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
            this.execute = execute;
        }

        public bool CanExecute(object parameter)
        {
            var window = parameter as Window;
            return window != null && canExecute(window);
        }

        public void Execute([NotNull] object parameter)
        {
            var window = parameter as Window;
            if (window == null) throw new ArgumentException("The parameter of this command must be an instance of \'Window\'.");
            execute(window);
        }

        // We provide an empty `add' and `remove' to avoid a warning about unused events that we have
        // to implement as they are part of the ICommand definition.
        public event EventHandler CanExecuteChanged { add { } remove { } }
    }
}