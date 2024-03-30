using Sharprompt;

namespace PWManager.CLI.Abstractions; 

public static class ConsoleInteraction {

    public static event Action OnConsoleInput;
    
    public static T Input<T>(string prompt) {
        return Prompt.Input<T>(prompt);
    }

    public static bool Confirm(string prompt) {
        return Prompt.Confirm(prompt);
    }

    public static T Select<T>(string message, IEnumerable<T>? items = null, object? defaultValue = null) where T : notnull {
        return Prompt.Select<T>(message, items, defaultValue: defaultValue);
    }

    public static IEnumerable<T> MultiSelect<T>(string message, IEnumerable<T>? items = null, IEnumerable<T>? defaultValues = null)
        where T : notnull {
        return Prompt.MultiSelect<T>(message, items, defaultValues: defaultValues);
    }

    public static string Password(string message) {
        return Prompt.Password(message);
    }

    public static string? ReadLine() {
        return Console.ReadLine();
    }
}