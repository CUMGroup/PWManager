using PWManager.Application.Context;
using PWManager.Application.Services.Interfaces;
using PWManager.CLI.Abstractions;
using PWManager.CLI.Attributes;
using PWManager.CLI.Enums;
using PWManager.CLI.Interfaces;
using PWManager.Domain.Repositories;
using PWManager.Domain.ValueObjects;
using Sharprompt;

namespace PWManager.CLI.Controllers;

[SessionOnly]
public class GroupController : IController {
    private readonly IGroupService _groupService;
    private readonly IUserEnvironment _userEnv;
    private readonly ISettingsRepository _settingsRepository; // TODO

    private readonly ILoginService _loginService;

    private readonly string newGroup = "New group";
    private readonly string switchGroup = "Switch group";
    private readonly string listAllGroups = "List all groups";
    private readonly string exit = "Return";
    private readonly string deleteGroup;

    public GroupController(IGroupService groupService, IUserEnvironment userEnv, ILoginService loginService, ISettingsRepository settingsRepository) {
        _groupService = groupService;
        _userEnv = userEnv;
        _loginService = loginService;
        _settingsRepository = settingsRepository; // TODO
        deleteGroup = $"Delete group '{_userEnv.CurrentGroup!.Identifier}'";
    }

    public ExitCondition Handle(string[] args) { // TODO: eventuell refactoring
        var option = Prompt.Select("Select an action", new[] { newGroup, switchGroup, listAllGroups, deleteGroup, exit });
        
        if (option.Equals(exit)) {
            return ExitCondition.CONTINUE;
        }

        if (option.Equals(newGroup)) {
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
            HandleDeletion(currentgroup);
        }

        return ExitCondition.CONTINUE;
    }

    private void SwitchGroup() {
        var groups = _groupService.GetAllGroupNames();
        var groupidentifier = Prompt.Select("To which group do you want to switch to", groups);

        _groupService.SwitchGroup(groupidentifier);
    }

    private void CreateNewGroupAndSwitchToIt() {
        var groupIdentifier = PromptHelper.GetInput("What's the name of the new group");

        _groupService.AddGroup(_userEnv.CurrentUser!.Id, groupIdentifier);
        _groupService.SwitchGroup(groupIdentifier);
        PromptHelper.PrintColoredText(ConsoleColor.Green, "Switched to new group!");
    }

    private bool HandleDeletion(string identifier) {
        var groups = _groupService.GetAllGroupNames();
        groups.Remove(identifier);

        if (!PromptHelper.ConfirmDeletion(identifier, _loginService, _userEnv)) {
            PromptHelper.PrintColoredText(ConsoleColor.Red, "Delete aborted!");
            return false;
        }

        var settings = _settingsRepository.GetSettings(); // TODD in den SettingsService auslagern (?)
        var isMainGroup = settings.MainGroup.MainGroupIdentifier.Equals(identifier);
        if (isMainGroup) {
            PromptHelper.PrintColoredText(ConsoleColor.Yellow, "You are going to delete your standard group.");
            PromptHelper.PrintColoredText(ConsoleColor.Yellow, "Please provide a new standard group.");
        }

        if (groups.Count > 0) {
            SwitchGroup();
        } else {
            CreateNewGroupAndSwitchToIt();
        }

        if (isMainGroup) { // TODD in den SettingsService auslagern (!)
            settings.MainGroup = new MainGroupSetting(_userEnv.CurrentGroup!.Identifier);
            _settingsRepository.UpdateSettings(settings);
        }

        _groupService.DeleteGroup(identifier);
        PromptHelper.PrintColoredText(ConsoleColor.Green, $"Group '{identifier}' deleted!");
        return true;
    }
}

