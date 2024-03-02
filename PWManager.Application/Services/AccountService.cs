using PWManager.Application.Context;
using PWManager.Application.Exceptions;
using PWManager.Application.Services.Interfaces;
using PWManager.Domain.Repositories;

namespace PWManager.Application.Services; 

public class AccountService : IAccountService {

    private IUserEnvironment _environment;
    public AccountService(IUserEnvironment environment) {
        _environment = environment;
    }

    public List<string> GetCurrentAccountNames() {
        if (_environment.CurrentGroup is null) {
            throw new UserFeedbackException("No active group found. Are you in a session?");
        }
        return _environment.CurrentGroup.Accounts.Select(e => e.Identifier).ToList();
    }
}