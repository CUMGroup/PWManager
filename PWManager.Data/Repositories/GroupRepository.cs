using Microsoft.EntityFrameworkCore;
using PWManager.Application.Context;
using PWManager.Application.Exceptions;
using PWManager.Data.Abstraction;
using PWManager.Data.Abstraction.Interfaces;
using PWManager.Data.Models;
using PWManager.Data.Persistance;
using PWManager.Domain.Entities;
using PWManager.Domain.Repositories;
using PWManager.Domain.Services.Interfaces;

namespace PWManager.Data.Repositories; 

public class GroupRepository : IGroupRepository {

    private readonly IHaveDataContext _dataContext;
    private ApplicationDbContext _dbContext => _dataContext.GetDbContext();
    private readonly ICryptService _cryptService;
    private readonly IUserEnvironment _environment;

    public GroupRepository(ICryptService cryptService, IUserEnvironment environment) : this(cryptService, environment, HaveDataContextFactory.Create()) { }

    internal GroupRepository(ICryptService cryptService, IUserEnvironment environment, IHaveDataContext dataContext) {
        _cryptService = cryptService;
        _environment = environment;
        _dataContext = dataContext;
    }
    
    public Group GetGroup(string groupName) {
        if (_environment.CurrentUser is null) {
            throw new UserFeedbackException("No user found! Are you in a session?");
        }
        var groups = _dbContext.Groups.Where(e => e.UserId == _environment.CurrentUser.Id).AsNoTracking().ToList();
        
        var group = groups.FirstOrDefault(e => _cryptService.Decrypt(e.IdentifierCrypt).Equals(groupName));

        if (group is null) {
            return null!;
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
        var groupModel = GroupEntityToModel(group, false);

        _dbContext.Groups.Add(groupModel);
        return _dbContext.SaveChanges() > 0;
    }

    public bool UpdateGroup(Group group) {
        var groupModel = GroupEntityToModel(group, false);

        _dbContext.Groups.Update(groupModel);
        return _dbContext.SaveChanges() > 0;
    }

    public bool AddAccountToGroup(Account account, Group group) {
        var accountModel = AccountEntityToModel(account, group.Id);
        _dbContext.Accounts.Add(accountModel);
        return _dbContext.SaveChanges() > 0;
    }

    public bool UpdateAccountInGroup(Account account, Group group) {
        var accountModel = AccountEntityToModel(account, group.Id);
        _dbContext.Accounts.Update(accountModel);
        return _dbContext.SaveChanges() > 0;
    }

    public bool DeleteAccountInGroup(Account account, Group group) {
        group.RemoveAccount(account);
        var accountModel = AccountEntityToModel(account, group.Id);
        _dbContext.Accounts.Remove(accountModel);
        return _dbContext.SaveChanges() > 0;
    }

    public bool RemoveGroup(string groupName) {
        var group = GroupEntityToModel(GetGroup(groupName), false);

        _dbContext.Groups.Remove(group);
        return _dbContext.SaveChanges() > 0;
    }



    private GroupModel GroupEntityToModel(Group e, bool findChildren) {
        if (_environment.CurrentUser is null) {
            throw new UserFeedbackException("No user found! Are you in a session?");
        }

        var group = _dbContext.Groups.Find(e.Id) ?? new GroupModel() {
            Id = e.Id,
            Created = e.Created,
            Updated = e.Updated,
            UserId = _environment.CurrentUser.Id,
        };
        group.IdentifierCrypt = _cryptService.Encrypt(e.Identifier);
        if (findChildren) {
            group.Accounts = e.Accounts.Select(acc => AccountEntityToModel(acc, e.Id)).ToList();
        }
        return group;
    }

    private AccountModel AccountEntityToModel(Account e, string groupId) {
        var account = _dbContext.Accounts.Find(e.Id) ?? new AccountModel {
            Id = e.Id,
            Created = e.Created,
            Updated = e.Updated,
        };

        account.GroupId = groupId;
        account.IdentifierCrypt = _cryptService.Encrypt(e.Identifier);
        account.PasswordCrypt = _cryptService.Encrypt(e.Password);
        account.LoginNameCrypt = _cryptService.Encrypt(e.LoginName);

        return account;
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