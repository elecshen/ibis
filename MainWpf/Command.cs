﻿using System.Windows.Input;



namespace MainWpf
{
    public partial class MainWindow
    {
        public class Command(Action<object?> execute, Func<object?, bool>? canExecute = null) : ICommand
        {
            public event EventHandler? CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }

            public bool CanExecute(object? parameter) => canExecute == null || canExecute(parameter);

            public void Execute(object? parameter) => execute(parameter);
        }
    }
}