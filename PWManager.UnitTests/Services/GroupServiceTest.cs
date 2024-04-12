using NSubstitute;
using NSubstitute.ReceivedExtensions;
using PWManager.Application.Context;
using PWManager.Application.Exceptions;
using PWManager.Application.Services;
using PWManager.Domain.Entities;
using PWManager.Domain.Repositories;

namespace PWManager.UnitTests.Services; 

public class GroupServiceTest {

    private GroupService _sut;

    [Fact]
    public void GroupService_Should_AddGroup() {
        var groupRepo = MockGroupRepo();
        
        _sut = new GroupService(null, groupRepo);
        
        _sut.AddGroup("userIdlel", "NewIdentifier");

        groupRepo.Received().AddGroup(Arg.Is<Group>(e => e.Identifier == "NewIdentifier" && e.UserId == "userIdlel"));
    }

    [Fact]
    public void GroupService_ShouldNot_AddExistingGroup() {
        var groupRepo = MockGroupRepo();
        groupRepo.GetGroup(Arg.Any<string>()).Returns(new Group("", ""));

        _sut = new GroupService(null, groupRepo);

        var ex = Assert.Throws<UserFeedbackException>(() => _sut.AddGroup("yes", "yes"));
        
        Assert.Equal(MessageStrings.GroupAlreadyExist("yes"), ex.Message);
    }

    [Fact]
    public void GroupService_Should_DeleteGroup() {
        var repo = MockGroupRepo();
        
        _sut = new GroupService(null, repo);
        
        _sut.DeleteGroup("identifier");

        repo.Received().RemoveGroup(Arg.Is<string>(e => e == "identifier"));
    }

    [Fact]
    public void GroupService_ShouldNot_DeleteGroupFailed() {
        var repo = MockGroupRepo();
        repo.RemoveGroup(Arg.Any<string>()).Returns(false);

        _sut = new GroupService(null, repo);
        
        var ex = Assert.Throws<UserFeedbackException>(() => _sut.DeleteGroup("identifier"));
        
        Assert.Equal(MessageStrings.FailedDeletingGroup("identifier"), ex.Message);
    }

    [Fact]
    public void GroupService_Should_GetAllNames() {
        var repo = MockGroupRepo();

        _sut = new GroupService(null, repo);

        var names = _sut.GetAllGroupNames();
        
        Assert.Equal(2, names.Count);
        Assert.Equal("Name1", names[0]);
        Assert.Equal("Name2", names[1]);
    }

    [Fact]
    public void GroupService_Should_SwitchGroup() {
        var env = Substitute.For<IUserEnvironment>();
        var repo = MockGroupRepo();
        var group = new Group("a", "asd");
        repo.GetGroup(Arg.Any<string>()).Returns(group);

        _sut = new GroupService(env, repo);

        _sut.SwitchGroup("a");
        
        env.Received().CurrentGroup = group;
    }

    private IGroupRepository MockGroupRepo() {
        var groupRepo = Substitute.For<IGroupRepository>();
        groupRepo.RemoveGroup(Arg.Any<string>()).Returns(true);
        groupRepo.GetAllGroupNames().Returns(new List<string> { "Name1", "Name2" });
        return groupRepo;
    }
}