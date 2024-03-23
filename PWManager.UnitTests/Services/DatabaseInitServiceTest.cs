using System.Runtime.InteropServices.JavaScript;
using NSubstitute;
using PWManager.Application.Abstractions.Interfaces;
using PWManager.Application.Context;
using PWManager.Application.Exceptions;
using PWManager.Data.Abstraction;
using PWManager.Data.Services;
using PWManager.Domain.Entities;
using PWManager.Domain.Repositories;
using PWManager.Domain.Services.Interfaces;

namespace PWManager.UnitTests.Services; 

public class DatabaseInitServiceTest {

    private DatabaseInitializerService _sut;
    private IGroupRepository _groupRepo = Substitute.For<IGroupRepository>();
    private ICryptService _cryptService = Substitute.For<ICryptService>();
    private IUserEnvironment _userEnv = Substitute.For<IUserEnvironment>();
    private ICryptEnvironment _cryptEnv = Substitute.For<ICryptEnvironment>();
    private IDataContextInitializer _dataContextInitializer = Substitute.For<IDataContextInitializer>();
    private IUserRepository _userRepo = Substitute.For<IUserRepository>();
    
    [Fact]
    public void Init_Should_SetEnvironment() {
        SetupDatabaseExists(false);
        var user = SetupUserRepoReturnsUser();
        
        _sut = new DatabaseInitializerService(_userRepo,_groupRepo,_userEnv, _cryptService, _cryptEnv, _dataContextInitializer);
        
        _sut.InitDatabase(".", "TestUserName", "WhatAPassword");
        
        Assert.Equal(user,_userEnv.CurrentUser);
        Assert.NotNull(_cryptEnv.EncryptionKey);
    }

    [Fact]
    public void Init_ShouldNot_AcceptInvalidName() {
        SetupDatabaseExists(false);
        
        _sut = new DatabaseInitializerService(_userRepo,_groupRepo,_userEnv, _cryptService, _cryptEnv, _dataContextInitializer);

        var ex = Assert.Throws<UserFeedbackException>(() => _sut.InitDatabase(".","asd$", "WhatAPassword"));
        
        Assert.Equal("Invalid Username! Only letters are allowed!", ex.Message);
    }
    
    [Fact]
    public void Init_ShouldNot_InitExistingDb() {
        SetupDatabaseExists(true);
        
        _sut = new DatabaseInitializerService(_userRepo,_groupRepo,_userEnv, _cryptService, _cryptEnv, _dataContextInitializer);

        var ex = Assert.Throws<UserFeedbackException>(() => _sut.InitDatabase(".","TestUserName", "WhatAPassword"));
        
        Assert.Equal("Database initialization failed! The database already exists at the specified path!", ex.Message);
    }

    private void SetupDatabaseExists(bool exists) {
        _dataContextInitializer.DatabaseExists(Arg.Any<string>()).Returns(exists);
    }

    private User SetupUserRepoReturnsUser() {
        var user = new User(Guid.NewGuid().ToString(), DateTimeOffset.Now, DateTimeOffset.Now, "TestUserName");
        _userRepo.AddUser(Arg.Any<string>(), Arg.Any<string>()).Returns(user);
        return user;
    }
}