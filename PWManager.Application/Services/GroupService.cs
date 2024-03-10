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

        try {
            var group = _groupRepo.GetGroup(identifier);
            if (group is not null) {
                throw new UserFeedbackException($"A group with the name '{identifier}' already exists!");
            }
        } catch (UserFeedbackException) {
           var  group = new Group(identifier, userID);
            _groupRepo.AddGroup(group);
        }
    }

    public void DeleteGroup(string identifier) {
        throw new NotImplementedException();
    }

    public List<string> GetAllGroupNames() {
        throw new NotImplementedException();
    }
}
