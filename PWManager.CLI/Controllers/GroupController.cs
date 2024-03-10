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

    public GroupController(IGroupService groupService, IUserEnvironment userEnv, ICliEnvironment cliEnv) {
        _groupService = groupService;
        _userEnv = userEnv;
        _cliEnv = cliEnv;
    }

    public ExitCondition Handle(string[] args) {
        Prompt.Select("Please select", );

        return ExitCondition.CONTINUE;
    }
}

