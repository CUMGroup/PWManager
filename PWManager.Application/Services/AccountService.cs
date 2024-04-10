using System.Diagnostics.CodeAnalysis;
using PWManager.Application.Context;
using PWManager.Application.Exceptions;
using PWManager.Application.Services.Interfaces;
using PWManager.Domain.Entities;
using PWManager.Domain.Repositories;
using PWManager.Domain.Services.Interfaces;

namespace PWManager.Application.Services; 

public class AccountService : IAccountService {

    private IUserEnvironment _environment;
    private IGroupRepository _groupRepo;
    private IClipboard _clipboard;
    private IPasswordGeneratorService _passGen;
    
    public AccountService(IUserEnvironment environment, IGroupRepository groupRepo, IClipboard clipboard, IPasswordGeneratorService passGen) {
        _environment = environment;
        _groupRepo = groupRepo;
        _clipboard = clipboard;
        _passGen = passGen;
    }

    public List<string> GetCurrentAccountNames() {
        ThrowIfGroupIsNull(_environment.CurrentGroup);
        return _environment.CurrentGroup.Accounts.Select(e => e.Identifier).ToList();
    }

    public void AddNewAccount(string identifier, string loginname, string password) {
        ThrowIfGroupIsNull(_environment.CurrentGroup);

        var account = GetAccountByIdentifier(identifier);
        if (account is not null) {
            throw new UserFeedbackException(MessageStrings.AccountAlreadyExist(identifier));
        }

        account = new Account(identifier, loginname, password);

        _environment.CurrentGroup.AddAccount(account);

        var saved = _groupRepo.AddAccountToGroup(account, _environment.CurrentGroup);
        if (!saved) {
            throw new UserFeedbackException(MessageStrings.FAILED_ADDING_ACCOUNT);
        }
    }

    public Account? GetAccountByIdentifier(string identifier) {
        ThrowIfGroupIsNull(_environment.CurrentGroup);
        
        return _environment.CurrentGroup.FindByIdentifier(identifier);
    }
    
    public void CopyPasswordToClipboard(string identifier) {
        var acc = GetAccountByIdentifier(identifier);
        if (acc is null) {
            throw new UserFeedbackException(MessageStrings.ACCOUNT_NOT_FOUND);
        }
        _clipboard.WriteClipboard(acc.Password);
    }

    public void CopyLoginnameToClipboard(string identifier) {
        var acc = GetAccountByIdentifier(identifier);
        if (acc is null) {
            throw new UserFeedbackException(MessageStrings.ACCOUNT_NOT_FOUND);
        }
        _clipboard.WriteClipboard(acc.LoginName);
    }

    public void RegeneratePassword(string identifier) {
        ThrowIfGroupIsNull(_environment.CurrentGroup);

        var acc = GetAccountByIdentifier(identifier);
        if (acc is null) {
            throw new UserFeedbackException(MessageStrings.ACCOUNT_NOT_FOUND);
        }

        acc.Password = _passGen.GeneratePassword();
        _groupRepo.UpdateAccountInGroup(acc, _environment.CurrentGroup);
    }

    public void DeleteAccount(string identifier) {
        ThrowIfGroupIsNull(_environment.CurrentGroup);
        
        var acc = GetAccountByIdentifier(identifier);
        if (acc is null) {
            throw new UserFeedbackException(MessageStrings.ACCOUNT_NOT_FOUND);
        }

        _environment.CurrentGroup.RemoveAccount(acc);
        _groupRepo.DeleteAccountInGroup(acc, _environment.CurrentGroup);
    }


    private static void ThrowIfGroupIsNull([NotNull]Group? group) {
        if (group is null) {
            throw new UserFeedbackException(MessageStrings.NO_ACTIVE_GROUP);
        }
    }
}