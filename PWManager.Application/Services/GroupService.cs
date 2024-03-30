using PWManager.Application.Context;
using PWManager.Application.Exceptions;
using PWManager.Application.Services.Interfaces;
using PWManager.Domain.Repositories;
using PWManager.Domain.Entities;

namespace PWManager.Application.Services;
public class GroupService : IGroupService {

    private IUserEnvironment _environment;
    private IGroupRepository _groupRepo;

    public GroupService(IUserEnvironment environment, IGroupRepository groupRepository)
    {
        _environment = environment;
        _groupRepo = groupRepository;
    }

    public void AddGroup(string userID, string identifier) {
        var group = _groupRepo.GetGroup(identifier);

        if (group is not null) {
            throw new UserFeedbackException(MessageStrings.GroupAlreadyExist(identifier));
        }

        group = new Group(identifier, userID);
        _groupRepo.AddGroup(group);
    }

    public void DeleteGroup(string identifier) {
        var succ = _groupRepo.RemoveGroup(identifier);
        if(!succ) {
            throw new UserFeedbackException(MessageStrings.FailedDeletingGroup(identifier));
        }
    }

    public List<string> GetAllGroupNames() {
        return _groupRepo.GetAllGroupNames();
    }

    public void SwitchGroup(string identifier) {
        var group = _groupRepo.GetGroup(identifier);
        _environment.CurrentGroup = group;
    }
}
