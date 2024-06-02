namespace PWManager.CLI.Abstractions; 

public static class PromptHelper {
    public static string GetInput(string prompt) {
        var input = ConsoleInteraction.Input<string>(prompt);
        while (string.IsNullOrWhiteSpace(input)) {
            Console.WriteLine(UIstrings.EMPTY_INPUT);
            input = ConsoleInteraction.Input<string>(prompt);
        }

        return input;
    }
    
    public static int GetInputGreaterThan(string message, int greaterThan) {
        var val = ConsoleInteraction.Input<int>(message);

        while(val <= greaterThan) {
            PrintColoredText(ConsoleColor.Red, UIstrings.ValueCannotBeLessThan(greaterThan + 1));
            val = ConsoleInteraction.Input<int>(message);
        }

        return val;
    }

    public static bool InputPassword(Func<string, bool> passwordValidator) {
        var tryCount = 0;
        var pass = ConsoleInteraction.Password(UIstrings.ENTER_PASSWORD);
        var succ = passwordValidator(pass);

        while (!succ && tryCount < 3) {
            ++tryCount;
            Console.WriteLine($"{UIstrings.INVALID_PASSWORD} {UIstrings.TRY_AGAIN} ({tryCount}/3)");
            pass = ConsoleInteraction.Password(UIstrings.ENTER_PASSWORD);
            succ = passwordValidator(pass);
        }

        return succ;
    }

    public static string InputNewPassword() {
        var password = TryPasswordInput();
        while (password is null) {
            Console.WriteLine($"{UIstrings.REPEAT_PASSWORD_FAILED} {UIstrings.TRY_AGAIN}");
            password = TryPasswordInput();
        }

        return password;
    }
    
    public static string? TryPasswordInput() {
        var password = ConsoleInteraction.Password(UIstrings.ENTER_PASSWORD);
        while (string.IsNullOrWhiteSpace(password) || password.Length < 4) {
            Console.WriteLine(UIstrings.PASSWORD_TOO_SHORT);
            password = ConsoleInteraction.Password(UIstrings.ENTER_PASSWORD);
        }
    
        var repeat = ConsoleInteraction.Password(UIstrings.REPEAT_PASSWORD);
        var tryCount = 0;
        while (!password.Equals(repeat) && tryCount < 3) {
            ++tryCount;
            Console.WriteLine($"{UIstrings.REPEAT_PASSWORD_DOES_NOT_MATCH} {UIstrings.TRY_AGAIN} ({tryCount}/3)");
            repeat = ConsoleInteraction.Password(UIstrings.REPEAT_PASSWORD);
        }

        return password.Equals(repeat) ? password : null;
    }

    public static void PrintPaginated(List<string> lines) {
        var index = 0;
        while (index < lines.Count) {
            if (index >= Console.BufferHeight - 1) {
                Console.Write(UIstrings.PAGINATION_HINT);
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
        var areYouSure = ConsoleInteraction.Confirm(UIstrings.DeletionOf(identifier));
        if (!areYouSure) {
            return false;
        }

        var passwordTest = ConsoleInteraction.Password(UIstrings.ENTER_MASTER_PASSWORD);
        var passwordCorrect = passwordValidator(passwordTest);

        if (passwordCorrect) {
            return true;
        }
        Console.WriteLine(UIstrings.INVALID_PASSWORD);
        return false;
    }
}