using PWManager.Application.Services.Interfaces;
using PWManager.CLI.Abstractions;
using PWManager.CLI.Attributes;
using PWManager.CLI.Enums;
using PWManager.CLI.Interfaces;

namespace PWManager.CLI.Controllers; 

[SessionOnly]
public class ListController : IController {

    private readonly IAccountService _accountService;
    public ListController(IAccountService accountService) {
        _accountService = accountService;
    }

    public ExitCondition Handle(string[] args) {

        var accounts = _accountService.GetCurrentAccountNames();

        PromptHelper.PrintPaginated(accounts);
        Console.WriteLine();
        
        return ExitCondition.CONTINUE;
    }
}