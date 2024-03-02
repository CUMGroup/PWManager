using System.Diagnostics.CodeAnalysis;
using PWManager.Application.Context;
using PWManager.Application.Exceptions;
using PWManager.Application.Services.Interfaces;
using PWManager.Domain.Entities;
using PWManager.Domain.Repositories;

namespace PWManager.Application.Services; 

public class AccountService : IAccountService {

    private IUserEnvironment _environment;
    private IGroupRepository _groupRepo;
    public AccountService(IUserEnvironment environment, IGroupRepository groupRepo) {
        _environment = environment;
        _groupRepo = groupRepo;
    }

    public List<string> GetCurrentAccountNames() {
        ThrowIfGroupIsNull(_environment.CurrentGroup);
        return _environment.CurrentGroup.Accounts.Select(e => e.Identifier).ToList();
    }

    public void AddNewAccount(string identifier, string loginname, string password) {
        ThrowIfGroupIsNull(_environment.CurrentGroup);

        var account = _environment.CurrentGroup.FindByIdentifier(identifier);
        if (account is not null) {
            throw new UserFeedbackException("Account with identifier '" + identifier +
                                            "' does already exist in your group!");
        }

        account = new Account(identifier, loginname, password);

        _environment.CurrentGroup.AddAccount(account);

        var saved = _groupRepo.AddAccountToGroup(account, _environment.CurrentGroup);
        if (!saved) {
            throw new UserFeedbackException("Failed adding the account!");
        }
    }

    
    private static void ThrowIfGroupIsNull([NotNull]Group? group) {
        if (group is null) {
            throw new UserFeedbackException("No active group found. Are you in a session?");
        }
    }
}