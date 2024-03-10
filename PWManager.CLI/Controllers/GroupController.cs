using PWManager.Application.Context;
using PWManager.Application.Services.Interfaces;
using PWManager.CLI.Abstractions;
using PWManager.CLI.Attributes;
using PWManager.CLI.Enums;
using PWManager.CLI.Interfaces;
using Sharprompt;

namespace PWManager.CLI.Controllers;

[SessionOnly]
public class GroupController : IController {
    private readonly IGroupService _groupService;
    private readonly IUserEnvironment _userEnv; 
    private readonly ICliEnvironment _cliEnv;

    private readonly string newGroup = "New Group";
    private readonly string switchGroup = "Switch Group";
    private readonly string listAllGroups = "List all Groups";
    private readonly string deleteGroup;

    public GroupController(IGroupService groupService, IUserEnvironment userEnv, ICliEnvironment cliEnv) {
        _groupService = groupService;
        _userEnv = userEnv;
        _cliEnv = cliEnv;
        deleteGroup = $"Delete Group '{_userEnv.CurrentGroup!.Identifier}'";
    }

    public ExitCondition Handle(string[] args) {
        var option = Prompt.Select("What do you wanna do", new[] { newGroup, switchGroup, listAllGroups, deleteGroup });
        
        if(option.Equals(newGroup)) {
            var groupIdentifier = PromptHelper.GetInput("What's the name of the new Group");
            _groupService.AddGroup(_userEnv.CurrentUser!.Id, groupIdentifier);
            _groupService.SwitchGroup(groupIdentifier);
            return ExitCondition.CONTINUE;
        }
        if (option.Equals(switchGroup)) {
            var groups = _groupService.GetAllGroupNames();
            var groupidentifier = Prompt.Select("To which group do you want to switch to", groups);
            _groupService.SwitchGroup(groupidentifier);
        }
        if (option.Equals(listAllGroups)) {
            var groups = _groupService.GetAllGroupNames();
        }
        if (option.Equals(deleteGroup)) {

        }


        return ExitCondition.CONTINUE;
    }
}

