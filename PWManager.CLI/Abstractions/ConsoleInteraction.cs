using Sharprompt;

namespace PWManager.CLI.Abstractions; 

public static class ConsoleInteraction {

    public static event Action OnConsoleInput;
    
    public static T Input<T>(string prompt) {
        OnConsoleInput();
        var res = Prompt.Input<T>(prompt);
        OnConsoleInput();
        return res;
    }

    public static bool Confirm(string prompt) {
        OnConsoleInput();
        var res = Prompt.Confirm(prompt);
        OnConsoleInput();
        return res;
    }

    public static T Select<T>(string message, IEnumerable<T>? items = null, object? defaultValue = null) where T : notnull {
        OnConsoleInput();
        var res = Prompt.Select<T>(message, items, defaultValue: defaultValue);
        OnConsoleInput();
        return res;
    }

    public static IEnumerable<T> MultiSelect<T>(string message, IEnumerable<T>? items = null, IEnumerable<T>? defaultValues = null)
        where T : notnull {
        OnConsoleInput();
        var res = Prompt.MultiSelect<T>(message, items, defaultValues: defaultValues);
        OnConsoleInput();
        return res;
    }

    public static string Password(string message) {
        OnConsoleInput();
        var res = Prompt.Password(message);
        OnConsoleInput();
        return res;
    }

    public static string? ReadLine() {
        OnConsoleInput();
        var res = Console.ReadLine();
        OnConsoleInput();
        return res;
    }

    public static void ResetConsole() {
        Console.CursorVisible = true;
        Console.ResetColor();
        Console.SetCursorPosition(0, Console.CursorTop);
        Console.WriteLine();
    }
}