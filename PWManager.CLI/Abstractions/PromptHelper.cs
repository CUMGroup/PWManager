using PWManager.Application.Services.Interfaces;
using Sharprompt;

namespace PWManager.CLI.Abstractions; 

public static class PromptHelper {
    public static string GetInput(string prompt) {
        var input = Prompt.Input<string>(prompt);
        while (string.IsNullOrWhiteSpace(input)) {
            Console.WriteLine("Your input was empty! Try again!");
            input = Prompt.Input<string>(prompt);
        }

        return input;
    }

    public static bool InputPassword(Func<string, bool> passwordValidator) {
        var tryCount = 0;
        var pass = Prompt.Password("Enter your password");
        var succ = passwordValidator(pass);

        while (!succ && tryCount < 3) {
            ++tryCount;
            Console.WriteLine($"Password incorrect! Please try again. ({tryCount}/3)");
            pass = Prompt.Password("Enter your password");
            succ = passwordValidator(pass);
        }

        return succ;
    }

    public static string InputNewPassword() {
        var password = TryPasswordInput();
        while (password is null) {
            Console.WriteLine("You failed to repeat your password 3 times in a row! Please try again!");
            password = TryPasswordInput();
        }

        return password;
    }
    
    public static string? TryPasswordInput() {
        var password = Prompt.Password("Enter your password");
        while (string.IsNullOrWhiteSpace(password) || password.Length < 8) {
            Console.WriteLine("Your password was too short. Please use a password with at least 8 characters!");
            password = Prompt.Password("Enter your password");
        }
    
        var repeat = Prompt.Password("Repeat your password");
        var tryCount = 0;
        while (!password.Equals(repeat) && tryCount < 3) {
            ++tryCount;
            Console.WriteLine($"The repeated password does not match your password! Please try again! ({tryCount}/3)");
            repeat = Prompt.Password("Repeat your password");
        }

        return password.Equals(repeat) ? password : null;
    }

    public static void PrintPaginated(List<string> lines) {
        var index = 0;
        while (index < lines.Count) {
            if (index >= Console.BufferHeight - 1) {
                Console.Write("(Press enter to view more, q to stop)");
                var input = Console.ReadKey(intercept: true);
                ClearConsoleLine();
                if (input.Key == ConsoleKey.Q) {
                    return;
                }

                if (input.Key is not (ConsoleKey.Enter or ConsoleKey.DownArrow)) {
                    continue;
                }
            }

            ClearConsoleLine();
            Console.WriteLine(lines[index]);
            index++;
        }
    }

    public static void ClearConsoleLine() {
        var currentLine = Console.CursorTop;
        Console.SetCursorPosition(0, Console.CursorTop);
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, currentLine);
    }
    
    public static void PrintColoredText(ConsoleColor color, string text) {
        var defaultColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ForegroundColor = defaultColor;

    }

    public static bool ConfirmDeletion(string identifier, Func<string, bool> passwordValidator) {
        var areYouSure = Prompt.Confirm($"Are you sure you want to delete {identifier}?");
        if (!areYouSure) {
            return false;
        }

        var passwordTest = Prompt.Password("Enter your master password to confirm");
        var passwordCorrect = passwordValidator(passwordTest);

        if (passwordCorrect) {
            return true;
        }
        Console.WriteLine("Invalid password.");
        return false;
    }
}