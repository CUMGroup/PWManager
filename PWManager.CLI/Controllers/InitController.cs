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

        var password = PromptHelper.InputPassword();
        
        _dbInit.InitDatabase(path, name, password);
        ConfigFileHandler.WriteDefaultFile(name, path);
        Console.WriteLine("Created your database! Enjoy");

        return ExitCondition.EXIT;
    }
}