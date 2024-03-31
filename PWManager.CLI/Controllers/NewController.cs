using PWManager.Application.Services.Interfaces;
using PWManager.CLI.Abstractions;
using PWManager.CLI.Attributes;
using PWManager.CLI.Enums;
using PWManager.CLI.Interfaces;
using PWManager.Domain.Services.Interfaces;

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

        var identifier = PromptHelper.GetInput(UIstrings.PROMPT_NAME_WEBSITE);
        var loginName = PromptHelper.GetInput(UIstrings.PROMPT_LOGIN_NAME);

        var genPassword = ConsoleInteraction.Confirm(UIstrings.PROMPT_GENERATE_PW);
        string password;
        if (genPassword) {
            password = _passwordGeneratorService.GeneratePassword();
        }else {
            var input = PromptHelper.TryPasswordInput();
            if (input is null) {
                Console.WriteLine(UIstrings.PASSWORD_PROVISION_FAILED);
                return ExitCondition.CONTINUE;
            }
            password = input;
        }

        _accountService.AddNewAccount(identifier, loginName, password);
        var defaultColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(UIstrings.ACCOUNT_CREATION_CONFIRM);
        Console.ForegroundColor = defaultColor;
        
        return ExitCondition.CONTINUE;
    }
}