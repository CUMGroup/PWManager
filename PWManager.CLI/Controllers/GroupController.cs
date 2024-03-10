using PWManager.Application.Context;
using PWManager.Application.Services;
using PWManager.Application.Services.Interfaces;
using PWManager.CLI.Abstractions;
using PWManager.CLI.Attributes;
using PWManager.CLI.Enums;
using PWManager.CLI.Interfaces;
using PWManager.Data.Services;
using Sharprompt;

namespace PWManager.CLI.Controllers;

[SessionOnly]
public class GroupController : IController {
    private readonly IGroupService _groupService;
    private readonly IUserEnvironment _userEnv;

    private readonly ILoginService _loginService;

    private readonly string newGroup = "New Group";
    private readonly string switchGroup = "Switch Group";
    private readonly string listAllGroups = "List all Groups";
    private readonly string deleteGroup;

    public GroupController(IGroupService groupService, IUserEnvironment userEnv, ILoginService loginService) {
        _groupService = groupService;
        _userEnv = userEnv;
        _loginService = loginService;
        deleteGroup = $"Delete Group '{_userEnv.CurrentGroup!.Identifier}'";
    }

    public ExitCondition Handle(string[] args) {
        var option = Prompt.Select("What do you wanna do", new[] { newGroup, switchGroup, listAllGroups, deleteGroup });
        
        if(option.Equals(newGroup)) {
            CreateNewGroupAndSwitchToIt();
            return ExitCondition.CONTINUE;
        }

        if (option.Equals(switchGroup)) {
            SwitchGroup();
            return ExitCondition.CONTINUE;
        }

        if (option.Equals(listAllGroups)) {
            var groups = _groupService.GetAllGroupNames();
            PromptHelper.PrintPaginated(groups);

            return ExitCondition.CONTINUE;
        }

        if (option.Equals(deleteGroup)) {
            var currentgroup = _userEnv.CurrentGroup!.Identifier;

            var confirm = Prompt.Confirm($"Are you sure you want to delete the group '{currentgroup}'");
            
            if(!confirm) {
                return ExitCondition.CONTINUE;
            }

            HandleDeletion(currentgroup);

            if (_groupService.GetAllGroupNames().Count > 0) {
                SwitchGroup();
            } else {
                CreateNewGroupAndSwitchToIt();
            }
        }

        return ExitCondition.CONTINUE;
    }

    private void SwitchGroup() {
        var groups = _groupService.GetAllGroupNames();
        var groupidentifier = Prompt.Select("To which group do you want to switch to", groups);

        _groupService.SwitchGroup(groupidentifier);
    }

    private void CreateNewGroupAndSwitchToIt() {
        var groupIdentifier = PromptHelper.GetInput("What's the name of the new Group");

        _groupService.AddGroup(_userEnv.CurrentUser!.Id, groupIdentifier);
        _groupService.SwitchGroup(groupIdentifier);
    }

    private bool HandleDeletion(string identifier) {
        if (!PromptHelper.ConfirmDeletion(identifier, _loginService, _userEnv)) {
            PromptHelper.PrintColoredText(ConsoleColor.Red, "Delete aborted!");
            return false;
        }
        _groupService.DeleteGroup(identifier);
        PromptHelper.PrintColoredText(ConsoleColor.Green, "Group deleted!");
        return true;
    }
}

