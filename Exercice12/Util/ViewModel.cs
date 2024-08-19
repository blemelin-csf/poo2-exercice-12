using System.ComponentModel;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;

namespace Exercice12;

public abstract class ViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected ICommand NewCommand(Action execute, Func<bool>? canExecute = null)
    {
        return new DelegateCommand<object>(_ => execute(), canExecute);
    }

    protected ICommand NewCommandWithParam<T>(Action<T> execute, Func<bool>? canExecute = null)
    {
        return new DelegateCommand<T>(execute, canExecute);
    }

    protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected void UpdateProperty<T>(T newValue, ref T currentValue, [CallerMemberName] string propertyName = null)
    {
        if (Equals(newValue, currentValue)) return;
        currentValue = newValue;
        NotifyPropertyChanged(propertyName);
    }
}

public sealed class DelegateCommand<T>(Action<T> execute, Func<bool>? canExecute = null) : ICommand
{
    public event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public void Execute(object parameter)
    {
        execute((T)parameter);
    }

    public bool CanExecute(object parameter)
    {
        return canExecute == null || canExecute();
    }
}

public class EventBindingExtension(string commandName) : MarkupExtension
{
    private static readonly MethodInfo OnEventMethod =
        typeof(EventBindingExtension).GetMethod(nameof(OnEvent), BindingFlags.Static | BindingFlags.NonPublic)!;

    private static void OnEvent(object sender, string commandName)
    {
        if (sender is not FrameworkElement control)
            throw new InvalidOperationException("Sender is not a WPF Control.");

        var context = control.DataContext;
        if (context?.GetType().GetProperty(commandName)?.GetValue(context) is not ICommand command)
            throw new InvalidOperationException("Target is not a WPF Command.");

        var parameter = (control as ICommandSource)?.CommandParameter;
        if (command.CanExecute(parameter)) command.Execute(parameter);
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        // Validations
        if (serviceProvider.GetService(typeof(IProvideValueTarget)) is not IProvideValueTarget targetProvider)
            throw new InvalidOperationException("Unexpected null target provider.");

        if (targetProvider.TargetObject is not FrameworkElement targetObject)
            throw new InvalidOperationException("Source is not a WPF Control.");

        if (targetProvider.TargetProperty is not EventInfo eventInfo)
            throw new InvalidOperationException("Property is not an WPF Event.");

        var handlerType = eventInfo.EventHandlerType!;
        var handler = handlerType.GetMethod("Invoke")!;

        var method = new DynamicMethod("", handler.ReturnType, [typeof(object), typeof(object)]);
        var generator = method.GetILGenerator();
        generator.Emit(OpCodes.Ldarg, 0);
        generator.Emit(OpCodes.Ldstr, commandName);
        generator.Emit(OpCodes.Call, OnEventMethod);
        generator.Emit(OpCodes.Ret);
        return method.CreateDelegate(handlerType);
    }
}