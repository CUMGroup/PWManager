﻿using Microsoft.EntityFrameworkCore;
using PWManager.Application.Context;
using PWManager.Application.Exceptions;
using PWManager.Data.Models;
using PWManager.Data.Persistance;
using PWManager.Domain.Entities;
using PWManager.Domain.Repositories;
using PWManager.Domain.Services.Interfaces;

namespace PWManager.Data.Repositories; 

public class GroupRepository : IGroupRepository {

    private ApplicationDbContext _dbContext => DataContext.GetDbContext();
    private readonly ICryptService _cryptService;
    private readonly IUserEnvironment _environment;
    
    public GroupRepository(ICryptService cryptService, IUserEnvironment environment) {
        _cryptService = cryptService;
        _environment = environment;
    }
    
    public Group GetGroup(string groupName) {
        if (_environment.CurrentUser is null) {
            throw new UserFeedbackException("No user found! Are you in a session?");
        }
        var groups = _dbContext.Groups.Where(e => e.UserId == _environment.CurrentUser.Id).AsNoTracking().ToList();
        
        var group = groups.FirstOrDefault(e => _cryptService.Decrypt(e.IdentifierCrypt).Equals(groupName));

        if (group is null) {
            throw new UserFeedbackException("Could not find group with name " + groupName);
        }

        var groupEntity = GroupModelToEntity(group);
        var accounts = _dbContext.Accounts.Where(e => e.GroupId == group.Id).AsNoTracking().ToList();

        var accountEntities = accounts.Select(AccountModelToEntity).OrderBy(e => e.Identifier).ToList();

        accountEntities.ForEach(e => groupEntity.AddAccount(e));

        return groupEntity;
    }

    public List<string> GetAllGroupNames() {
        if (_environment.CurrentUser is null) {
            throw new UserFeedbackException("No user found! Are you in a session?");
        }
        var groupNames = _dbContext.Groups
            .Where(e => e.UserId == _environment.CurrentUser.Id)
            .Select(e => e.IdentifierCrypt)
            .AsNoTracking()
            .ToList();

        return groupNames.Select(_cryptService.Decrypt).ToList();
    }

    public bool AddGroup(Group group) {
        var groupModel = GroupEntityToModel(group);

        _dbContext.Groups.Add(groupModel);
        return _dbContext.SaveChanges() > 0;
    }

    public bool UpdateGroup(Group group) {
        var groupModel = GroupEntityToModel(group);

        _dbContext.Groups.Update(groupModel);
        return _dbContext.SaveChanges() > 0;
    }

    public bool AddAccountToGroup(Account account, Group group) {
        var accountModel = AccountEntityToModel(account, group.Id);
        _dbContext.Accounts.Add(accountModel);
        return _dbContext.SaveChanges() > 0;
    }

    public bool RemoveGroup(string groupName) {
        var group = GroupEntityToModel(GetGroup(groupName));

        _dbContext.Groups.Remove(group);
        return _dbContext.SaveChanges() > 0;
    }



    private GroupModel GroupEntityToModel(Group e) {
        if (_environment.CurrentUser is null) {
            throw new UserFeedbackException("No user found! Are you in a session?");
        }
        return new GroupModel {
            Id = e.Id,
            Created = e.Created,
            Updated = e.Updated,
            UserId = _environment.CurrentUser.Id,
            IdentifierCrypt = _cryptService.Encrypt(e.Identifier),
            Accounts = e.Accounts.Select(acc => AccountEntityToModel(acc, e.Id)).ToList()
        };
    }

    private AccountModel AccountEntityToModel(Account e, string groupId) {
        return new AccountModel {
            Id = e.Id,
            Created = e.Created,
            Updated = e.Updated,
            GroupId = groupId,
            IdentifierCrypt = _cryptService.Encrypt(e.Identifier),
            PasswordCrypt = _cryptService.Encrypt(e.Password),
            LoginNameCrypt = _cryptService.Encrypt(e.LoginName)
        };
    }
    
    private Group GroupModelToEntity(GroupModel e) {
        return new Group(
            e.Id,
            e.Created,
            e.Updated,
            e.UserId,
            _cryptService.Decrypt(e.IdentifierCrypt));
    }

    private Account AccountModelToEntity(AccountModel e) {
        return new Account(
            e.Id,
            e.Created,
            e.Updated,
            _cryptService.Decrypt(e.IdentifierCrypt),
            _cryptService.Decrypt(e.LoginNameCrypt),
            _cryptService.Decrypt(e.PasswordCrypt));
    }
}