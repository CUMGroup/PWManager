using PWManager.Application.Abstractions.Interfaces;
using PWManager.Application.Context;
using PWManager.Application.Exceptions;
using PWManager.Application.Services.Interfaces;
using PWManager.CLI.Abstractions;
using PWManager.CLI.Attributes;
using PWManager.CLI.Enums;
using PWManager.CLI.Interfaces;

namespace PWManager.CLI.Controllers; 

[SessionOnly]
public class DeleteDatabaseController : IController {

    private readonly IDeleteDataContext _dataContextDeleter;
    private readonly ILoginService _loginService;
    private readonly IUserEnvironment _userEnv;
    
    public DeleteDatabaseController(IDeleteDataContext dataContextDeleter, ILoginService loginService, IUserEnvironment userEnv) {
        _dataContextDeleter = dataContextDeleter;
        _loginService = loginService;
        _userEnv = userEnv;
    }

    public ExitCondition Handle(string[] args) {
        
        if (!ConfirmPassword()) {
            return ExitCondition.CONTINUE;
        }

        try {
            _dataContextDeleter.DeleteDataContext();
        }
        catch(UserFeedbackException ex) {
            PromptHelper.PrintColoredText(ConsoleColor.Red, ex.Message);
            return ExitCondition.EXIT;
        }

        try {
            ConfigFileHandler.DeleteDefaultFile();
        }
        catch (UserFeedbackException ex) {
            PromptHelper.PrintColoredText(ConsoleColor.Red, ex.Message);
        }

        PromptHelper.PrintColoredText(ConsoleColor.Cyan, UIstrings.DATABASE_DELETED);
        
        return ExitCondition.EXIT;
    }

    private bool ConfirmPassword() {
        if (_userEnv.CurrentUser is null) {
            throw new ApplicationException(MessageStrings.NO_ACTIVE_USER);
        } 
        var username = _userEnv.CurrentUser.UserName;
        return PromptHelper.ConfirmDeletion(UIstrings.YOUR_DATABASE, (pw) => _loginService.CheckPassword(username, pw));
    }
}