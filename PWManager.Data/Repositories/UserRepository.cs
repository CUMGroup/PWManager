using Microsoft.EntityFrameworkCore;
using PWManager.Application.Exceptions;
using PWManager.Data.Models;
using PWManager.Data.Persistance;
using PWManager.Domain.Entities;
using PWManager.Domain.Repositories;
using PWManager.Domain.Services.Interfaces;

namespace PWManager.Data.Repositories; 

public class UserRepository : IUserRepository {

    private ApplicationDbContext _dbContext => DataContext.GetDbContext();
    private readonly ICryptService _cryptService;

    public UserRepository(ICryptService cryptService) {
        _cryptService = cryptService;
    }

    public User AddUser(string username, string password) {
        var salt = _cryptService.GenerateSalt();
        var passwordHash = _cryptService.Hash(password, salt);

        var userModel = new UserModel {
            Id = Guid.NewGuid().ToString(),
            UserName = username,
            MasterHash = passwordHash,
            Salt = salt,
            Created = DateTimeOffset.Now,
            Updated = DateTimeOffset.Now
        };
        _dbContext.Users.Add(userModel);
        var res = _dbContext.SaveChanges();
        if (res == 0) {
            throw new UserFeedbackException("Failed creating the user");
        }

        return UserModelToEntity(userModel);
    }

    public User? CheckPasswordAttempt(string username, string password) {
        var user = _dbContext.Users.AsNoTracking().First(e => e.UserName == username);
        var hash = _cryptService.Hash(password, user.Salt);

        if(hash.Equals(user.MasterHash)) {
            return UserModelToEntity(user);
        }
        return null;
    }

    private User UserModelToEntity(UserModel e) {
        return new User(e.Id, e.Created, e.Updated, e.UserName);
    }
}