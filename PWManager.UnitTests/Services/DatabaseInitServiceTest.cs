using System.Runtime.InteropServices.JavaScript;
using NSubstitute;
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
    private DataContextWrapper _wrapper = Substitute.For<DataContextWrapper>();
    private IUserRepository _userRepo = Substitute.For<IUserRepository>();
    
    [Fact]
    public void Init_Should_SetEnvironment() {
        _wrapper.DatabaseExists(Arg.Any<string>()).Returns(false);
        var user = new User(Guid.NewGuid().ToString(), DateTimeOffset.Now, DateTimeOffset.Now, "TestUserName");
        _userRepo.AddUser(Arg.Any<string>(), Arg.Any<string>()).Returns(user);
        
        
        _sut = new DatabaseInitializerService(_wrapper,_userRepo,_groupRepo,_userEnv, _cryptService, _cryptEnv);
        
        _sut.InitDatabase(".", "TestUserName", "WhatAPassword");
        
        Assert.Equal(user,_userEnv.CurrentUser);
        Assert.NotNull(_cryptEnv.EncryptionKey);
    }

    [Fact]
    public void Init_ShouldNot_AcceptInvalidName() {
        _wrapper.DatabaseExists(Arg.Any<string>()).Returns(false);
        _sut = new DatabaseInitializerService(_wrapper,_userRepo,_groupRepo,_userEnv, _cryptService, _cryptEnv);

        var ex = Assert.Throws<UserFeedbackException>(() => _sut.InitDatabase(".","asd$", "WhatAPassword"));
        
        Assert.Equal("Invalid Username! Only letters are allowed!", ex.Message);
    }
    
    [Fact]
    public void Init_ShouldNot_InitExistingDb() {
        _wrapper.DatabaseExists(Arg.Any<string>()).Returns(true);
        _sut = new DatabaseInitializerService(_wrapper,_userRepo,_groupRepo,_userEnv, _cryptService, _cryptEnv);

        var ex = Assert.Throws<UserFeedbackException>(() => _sut.InitDatabase(".","TestUserName", "WhatAPassword"));
        
        Assert.Equal("Database initialization failed! The database already exists at the specified path!", ex.Message);
    }
}