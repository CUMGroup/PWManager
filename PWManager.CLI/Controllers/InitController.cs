using System.ComponentModel;
using System.Text.RegularExpressions;
using PWManager.Application.Context;
using PWManager.Application.Exceptions;
using PWManager.Application.Services.Interfaces;
using PWManager.CLI.Abstractions;
using PWManager.CLI.Attributes;
using PWManager.CLI.Enums;
using PWManager.CLI.Interfaces;
using Sharprompt;

namespace PWManager.CLI.Controllers; 

[NoSession]
public class InitController : IController {

    private readonly IDatabaseInitializerService _dbInit;
    
    public InitController(IDatabaseInitializerService dbInit) {
        _dbInit = dbInit;
    }

    public ExitCondition Handle(string[] args) {
        var path = Prompt.Input<string>("Where do you want to create your database file?");
        while(!Path.Exists(path)) {
            Console.WriteLine("The given path does not exist.");
            path = Prompt.Input<string>("Where do you want to create your database file?");
        }
        
        var name = Prompt.Input<string>("What's your desired user name?");
        while (name.Length <= 1 || !Regex.IsMatch(name, @"^[a-zA-Z]+$")) {
            Console.WriteLine("Invalid name! It mus be longer than 1 character and must include only letters!");
            name = Prompt.Input<string>("What's your desired user name?");
        }

        var password = InputPassword();
        while (password is null) {
            Console.WriteLine("You failed to repeat your password 3 times in a row! Please try again!");
            password = InputPassword();
        }
        
        _dbInit.InitDatabase(path, name, password);
        ConfigFileHandler.WriteDefaultFile(name, path);
        Console.WriteLine("Created your database! Enjoy");

        return ExitCondition.EXIT;
    }

    private string? InputPassword() {
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