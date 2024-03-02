using PWManager.Application.Services.Interfaces;
using PWManager.CLI.Abstractions;
using PWManager.CLI.Attributes;
using PWManager.CLI.Enums;
using PWManager.CLI.Interfaces;
using PWManager.Domain.Services.Interfaces;
using Sharprompt;

namespace PWManager.CLI.Controllers; 

[SessionOnly]
public class NewController : IController {

    private readonly IAccountService _accountService;
    private readonly IPasswordGeneratorService _passwordGeneratorService;
    
    public NewController(IAccountService accountService, IPasswordGeneratorService passwordGeneratorService) {
        _accountService = accountService;
        _passwordGeneratorService = passwordGeneratorService;
    }

    public ExitCondition Handle(string[] args) {

        var identifier = GetInput("What's the name of the website?");
        var loginName = GetInput("What's your name or email for the login?");

        var genPassword = Prompt.Confirm("Do you want to generate a random password?");
        string password;
        if (genPassword) {
            password = _passwordGeneratorService.GeneratePassword();
        }else {
            var input = PromptHelper.TryPasswordInput();
            if (input is null) {
                Console.WriteLine("You failed to provide a password. Please try again creating your account!");
                return ExitCondition.CONTINUE;
            }
            password = input;
        }

        _accountService.AddNewAccount(identifier, loginName, password);
        var defaultColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Created the account!");
        Console.ForegroundColor = defaultColor;
        
        return ExitCondition.CONTINUE;
    }

    private string GetInput(string prompt) {
        var input = Prompt.Input<string>(prompt);
        while (string.IsNullOrWhiteSpace(input)) {
            Console.WriteLine("Your input was empty! Try again!");
            input = Prompt.Input<string>(prompt);
        }

        return input;
    }
}