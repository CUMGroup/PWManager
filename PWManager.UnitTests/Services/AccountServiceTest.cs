using NSubstitute;
using PWManager.Application.Context;
using PWManager.Application.Services;
using PWManager.Application.Services.Interfaces;
using PWManager.Domain.Entities;
using PWManager.Domain.Repositories;
using PWManager.Domain.Services.Interfaces;

namespace PWManager.UnitTests.Services; 

public class AccountServiceTest {

    private AccountService _sut;

    [Fact]
    public void AccountService_Should_GetAllNames() {
        var envWithGroup = MockUserEnvironmentWithGroup();

        _sut = new AccountService(envWithGroup, null, null, null);

        var groups = _sut.GetCurrentAccountNames();
        
        Assert.Equal(2, groups.Count);
        Assert.Equal("AccountId", groups[0]);
        Assert.Equal("AccountId2", groups[1]);
    }

    [Fact]
    public void AccountService_Should_AddNewAccount() {
        var env = MockUserEnvironmentWithGroup();
        var groupRepo = MockGroupRepo();
        
        _sut = new AccountService(env, groupRepo, null, null);

        _sut.AddNewAccount("NewIdentifier", "NewName", "NewPassword");
        
        Assert.Equal(3, env.CurrentGroup.Accounts.Count);
        Assert.Equal("NewIdentifier", env.CurrentGroup.Accounts[2].Identifier);
        Assert.Equal("NewName", env.CurrentGroup.Accounts[2].LoginName);
        Assert.Equal("NewPassword", env.CurrentGroup.Accounts[2].Password);
        
        groupRepo.Received().AddAccountToGroup(Arg.Any<Account>(), Arg.Any<Group>());

    }

    [Fact]
    public void AccountService_Should_GetAccountByIdentifier() {
        var env = MockUserEnvironmentWithGroup();

        _sut = new AccountService(env, null, null, null);
        var account = _sut.GetAccountByIdentifier("AccountId");
        
        Assert.Equal(env.CurrentGroup.Accounts[0], account);
    }

    [Fact]
    public void AccountService_Should_CopyPassword() {
        var env = MockUserEnvironmentWithGroup();
        var clipboard = Substitute.For<IClipboard>();

        _sut = new AccountService(env, null, clipboard, null);
        
        _sut.CopyPasswordToClipboard("AccountId");
        
        clipboard.Received().WriteClipboard(Arg.Is<string>(e => e == "Password1"));
    }
    
    [Fact]
    public void AccountService_Should_CopyLoginName() {
        var env = MockUserEnvironmentWithGroup();
        var clipboard = Substitute.For<IClipboard>();

        _sut = new AccountService(env, null, clipboard, null);
        
        _sut.CopyLoginnameToClipboard("AccountId");
        
        clipboard.Received().WriteClipboard(Arg.Is<string>(e => e == "Name1"));
    }

    [Fact]
    public void AccountService_Should_RegeneratePassword() {
        var env = MockUserEnvironmentWithGroup();
        var newPassword = "NewPasswordGenerated";
        var passGen = MockPassGenReturns(newPassword);
        var groupRepo = MockGroupRepo();

        _sut = new AccountService(env, groupRepo, null, passGen);
        
        _sut.RegeneratePassword("AccountId");
        
        Assert.Equal(newPassword, env.CurrentGroup.Accounts[0].Password);
        groupRepo.Received().UpdateAccountInGroup(Arg.Any<Account>(), Arg.Any<Group>());
    }

    [Fact]
    public void AccountService_Should_DeleteAccount() {
        var env = MockUserEnvironmentWithGroup();
        var groupRepo = MockGroupRepo();

        _sut = new AccountService(env, groupRepo, null, null);
        
        _sut.DeleteAccount("AccountId");
        
        Assert.Null(env.CurrentGroup.FindByIdentifier("AccountId"));
        groupRepo.Received().DeleteAccountInGroup(Arg.Any<Account>(), Arg.Any<Group>());
    }

    private IGroupRepository MockGroupRepo() {
        var groupRepo = Substitute.For<IGroupRepository>();
        groupRepo.AddAccountToGroup(Arg.Any<Account>(), Arg.Any<Group>()).Returns(true);
        groupRepo.UpdateAccountInGroup(Arg.Any<Account>(), Arg.Any<Group>()).Returns(true);
        groupRepo.DeleteAccountInGroup(Arg.Any<Account>(), Arg.Any<Group>()).Returns(true);
        return groupRepo;
    }
    private IUserEnvironment MockUserEnvironmentWithGroup() {
        var env = Substitute.For<IUserEnvironment>();
        env.CurrentGroup.Returns(new Group("GroupId", new List<Account>() {new Account("AccountId", "Name1", "Password1"), new Account("AccountId2", "Name2", "Password2")} ));
        return env;
    }

    private IPasswordGeneratorService MockPassGenReturns(string pw) {
        var pwGen = Substitute.For<IPasswordGeneratorService>();
        pwGen.GeneratePassword().Returns(pw);
        return pwGen;
    }
}