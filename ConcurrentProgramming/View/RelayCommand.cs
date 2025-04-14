﻿using System;
using System.Windows.Input;

public class RelayCommand : ICommand
{
    private readonly Action execute;
    public RelayCommand(Action execute) => this.execute = execute;
    public bool CanExecute(object parameter) => true;
    public void Execute(object parameter) => execute();
    public event EventHandler CanExecuteChanged;
}
