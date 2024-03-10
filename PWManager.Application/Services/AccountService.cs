﻿using System.Diagnostics.CodeAnalysis;
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

    public Account? GetAccountByIdentifier(string identifier) {
        ThrowIfGroupIsNull(_environment.CurrentGroup);
        
        return _environment.CurrentGroup.FindByIdentifier(identifier);
    }
    
    public void CopyPasswordToClipboard(string identifier) {
        var acc = GetAccountByIdentifier(identifier);
        if (acc is null) {
            throw new UserFeedbackException("Could not find the account!");
        }
        _clipboard.WriteClipboard(acc.Password);
    }

    public void CopyLoginnameToClipboard(string identifier) {
        var acc = GetAccountByIdentifier(identifier);
        if (acc is null) {
            throw new UserFeedbackException("Could not find the account!");
        }
        _clipboard.WriteClipboard(acc.LoginName);
    }

    public void RegeneratePassword(string identifier) {
        ThrowIfGroupIsNull(_environment.CurrentGroup);

        var acc = GetAccountByIdentifier(identifier);
        if (acc is null) {
            throw new UserFeedbackException("Could not find the account!");
        }

        acc.Password = _passGen.GeneratePassword();
        _groupRepo.UpdateAccountInGroup(acc, _environment.CurrentGroup);
    }

    public void DeleteAccount(string identifier) {
        ThrowIfGroupIsNull(_environment.CurrentGroup);
        
        var acc = GetAccountByIdentifier(identifier);
        if (acc is null) {
            throw new UserFeedbackException("Could not find the account!");
        }

        _groupRepo.DeleteAccountInGroup(acc, _environment.CurrentGroup);
    }


    private static void ThrowIfGroupIsNull([NotNull]Group? group) {
        if (group is null) {
            throw new UserFeedbackException("No active group found. Are you in a session?");
        }
    }
}