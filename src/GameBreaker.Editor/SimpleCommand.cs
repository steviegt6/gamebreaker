using System;
using System.Windows.Input;

namespace GameBreaker.Editor;

public sealed class SimpleCommand<T> : ICommand {
    private readonly Action<T> action;

    public SimpleCommand(Action<T> action) {
        this.action = action;
    }

    public bool CanExecute(object? parameter) {
        return true;
    }

    public void Execute(object? parameter) {
        action((T)parameter!);
    }

    public event EventHandler? CanExecuteChanged;
}
