using PWManager.Application.Context;
using PWManager.Application.Services.Interfaces;
using PWManager.Domain.Repositories;

namespace PWManager.Application.Services; 

public class AccountService : IAccountService {

    private IApplicationEnvironment _environment;
    public AccountService(IApplicationEnvironment environment) {
        _environment = environment;
    }

    public List<string> GetCurrentAccounts() {
        return _environment.CurrentGroup.Accounts.Select(e => e.Identifier).ToList();
    }
}