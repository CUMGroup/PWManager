﻿using Sharprompt;

namespace PWManager.CLI.Abstractions; 

public static class PromptHelper {

    public static string InputPassword() {
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
}