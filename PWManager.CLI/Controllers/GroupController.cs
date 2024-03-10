using PWManager.Application.Context;
using PWManager.Application.Services.Interfaces;
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

    public GroupController(IGroupService groupService, IUserEnvironment userEnv, ICliEnvironment cliEnv) {
        _groupService = groupService;
        _userEnv = userEnv;
        _cliEnv = cliEnv;
    }

    public ExitCondition Handle(string[] args) {
        var option = Prompt.Select("Select your city", new[] { newGroup, switchGroup, listAllGroups });
        
        if(option.Equals(newGroup)) {
            var groupIdentifier = Prompt.Input<string>("What's the name of the new Group");
            _groupService.AddGroup(_userEnv.CurrentUser?.Id, groupIdentifier);
        }


        return ExitCondition.CONTINUE;
    }
}

