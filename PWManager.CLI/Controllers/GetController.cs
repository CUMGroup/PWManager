using PWManager.Application.Context;
using PWManager.Application.Services.Interfaces;
using PWManager.CLI.Abstractions;
using PWManager.CLI.Attributes;
using PWManager.CLI.Enums;
using PWManager.CLI.Interfaces;
using Sharprompt;
using System.IO;

namespace PWManager.CLI.Controllers; 

[SessionOnly]
public class GetController : IController {

    private readonly IAccountService _accountService;
    private readonly ILoginService _loginService;
    private readonly IUserEnvironment _userEnvironment;
    
    public GetController(IAccountService accountService, IUserEnvironment userEnvironment, ILoginService loginService) {
        _accountService = accountService;
        _userEnvironment = userEnvironment;
        _loginService = loginService;
    }

    public ExitCondition Handle(string[] args) {

        var selection = GetAccountSelection();
        
        if (selection is null) {
            return ExitCondition.CONTINUE;
        }

        AccountAction action;
        var executed = false;
        do {
            action = GetAccountAction();
            executed = ExecuteAction(selection, action);
        } while ((action != AccountAction.DELETE || !executed) && action != AccountAction.RETURN);

        return ExitCondition.CONTINUE;
    }

    private bool ExecuteAction(string accountIdentifier, AccountAction action) {
        return action switch {
            AccountAction.COPY_PASSWORD => HandleCopyPassword(accountIdentifier),
            AccountAction.COPY_LOGINNAME => HandleCopyLoginName(accountIdentifier),
            AccountAction.REGENERATE_PASSWORD => HandleRegeneration(accountIdentifier),
            AccountAction.DELETE => HandleDeletion(accountIdentifier),
            AccountAction.RETURN => true,
            _ => false
        };
    }

    private bool HandleCopyPassword(string identifier) {
        _accountService.CopyPasswordToClipboard(identifier);
        PromptHelper.PrintColoredText(ConsoleColor.Green, "Copied the password to your clipboard!");
        return true;
    }

    private bool HandleCopyLoginName(string identifier) {
        _accountService.CopyLoginnameToClipboard(identifier);
        PromptHelper.PrintColoredText(ConsoleColor.Green, "Copied the login-name to your clipboard!");
        return true;
    }

    private bool HandleRegeneration(string identifier) {
        if (!ConfirmRegeneration(identifier)) {
            PromptHelper.PrintColoredText(ConsoleColor.Red, "Regeneration aborted!");
            return false;
        }
        _accountService.RegeneratePassword(identifier);
        PromptHelper.PrintColoredText(ConsoleColor.Green, "Regenerated your password!");
        return true;
    }

    private bool HandleDeletion(string identifier) {
        var username = _userEnvironment.CurrentUser!.UserName;
        
        if (!PromptHelper.ConfirmDeletion(identifier, (pT) => _loginService.CheckPassword(username, pT))) {
            PromptHelper.PrintColoredText(ConsoleColor.Red, "Delete aborted!");
            return false;
        }
        _accountService.DeleteAccount(identifier);
        PromptHelper.PrintColoredText(ConsoleColor.Green, "Account deleted!");
        return true;
    }
    
    private bool ConfirmRegeneration(string identifier) {
        return ConsoleInteraction.Confirm($"Are you sure you want to update the password of {identifier}?");
    }
    
    private AccountAction GetAccountAction() {
        return ConsoleInteraction.Select<AccountAction>("Select an Action");
    }
    
    private string? GetAccountSelection() {
        var names = _accountService.GetCurrentAccountNames();
        if (names.Any()) {
            return ConsoleInteraction.Select("Search an Account", names);
        }
        PromptHelper.PrintColoredText(ConsoleColor.Red, "There are no accounts in this group!");
        return null;
    }
}