using System.Text.RegularExpressions;
using PWManager.Application.Services.Interfaces;
using PWManager.CLI.Abstractions;
using PWManager.CLI.Attributes;
using PWManager.CLI.Enums;
using PWManager.CLI.Interfaces;

namespace PWManager.CLI.Controllers; 

[NoSession]
public class InitController : IController {

    private readonly IDatabaseInitializerService _dbInit;

    public InitController(IDatabaseInitializerService dbInit) {

        _dbInit = dbInit;
    }

    public ExitCondition Handle(string[] args) {
        var path = ConsoleInteraction.Input<string>(UIstrings.DESIRED_PATH);
        while(!Path.Exists(path)) {
            Console.WriteLine(UIstrings.PATH_DOES_NOT_EXIST);
            path = ConsoleInteraction.Input<string>(UIstrings.DESIRED_PATH);
        }

        _dbInit.CheckIfDataBaseExistsOnPath(path);

        var name = ConsoleInteraction.Input<string>(UIstrings.DESIRED_NAME);
        while (name.Length <= 1 || !Regex.IsMatch(name, @"^[a-zA-Z]+$")) {
            Console.WriteLine(UIstrings.INVALID_NAME);
            name = ConsoleInteraction.Input<string>(UIstrings.DESIRED_NAME);
        }

        var password = PromptHelper.InputNewPassword();
        
        _dbInit.InitDatabase(path, name, password);
        ConfigFileHandler.WriteDefaultFile(name, path);
        Console.WriteLine(UIstrings.CREATED_DATABASE);

        return ExitCondition.EXIT;
    }
}