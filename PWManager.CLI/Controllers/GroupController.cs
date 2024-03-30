using PWManager.Application.Context;
using PWManager.Application.Exceptions;
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

    private readonly string newGroup = UIstrings.ACTION_NEW_GROUP;
    private readonly string switchGroup = UIstrings.ACTION_SWITCH_GROUP;
    private readonly string listAllGroups = UIstrings.ACTION_LIST_GROUPS;
    private readonly string exit = UIstrings.ACTION_RETURN;
    private readonly string deleteGroup;

    public GroupController(IGroupService groupService, IUserEnvironment userEnv, ILoginService loginService, ISettingsRepository settingsRepository) {
        _groupService = groupService;
        _userEnv = userEnv;
        _loginService = loginService;
        _settingsRepository = settingsRepository; // TODO
        deleteGroup = $"Delete group '{_userEnv.CurrentGroup!.Identifier}'";
    }

    public ExitCondition Handle(string[] args) { // TODO: eventuell refactoring
        var option = Prompt.Select(UIstrings.SELECT_ACTION, new[] { newGroup, switchGroup, listAllGroups, deleteGroup, exit });
        
        if (option.Equals(exit)) {
            return ExitCondition.CONTINUE;
        }

        if (option.Equals(newGroup)) {
            CreateNewGroupAndSwitchToIt();
            return ExitCondition.CONTINUE;
        }

        if (option.Equals(switchGroup)) {
            SwitchGroup(_groupService.GetAllGroupNames());
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

    private void SwitchGroup(List<string> groups) {
        if (!groups.Any()) {
            throw new UserFeedbackException("There are no groups in your database. Something is really wrong!"); // TODO: Exception Message auslagern
        }
        var groupidentifier = Prompt.Select(UIstrings.SWITCH_GROUP_PROMPT, groups);
        _groupService.SwitchGroup(groupidentifier);
    }

    private void CreateNewGroupAndSwitchToIt() {
        var groupIdentifier = PromptHelper.GetInput(UIstrings.NEW_GROUP_NAME);

        _groupService.AddGroup(_userEnv.CurrentUser!.Id, groupIdentifier);
        _groupService.SwitchGroup(groupIdentifier);

        PromptHelper.PrintColoredText(ConsoleColor.Green, UIstrings.GROUP_SWITCH_CONFIRM);
    }

    private bool HandleDeletion(string identifier) {
        var groups = _groupService.GetAllGroupNames();
        groups.Remove(identifier);

        var username = _userEnv.CurrentUser!.UserName;
        if (!PromptHelper.ConfirmDeletion(identifier, (pT) => _loginService.CheckPassword(username, pT))) {
            PromptHelper.PrintColoredText(ConsoleColor.Red, UIstrings.DELETE_ABORTED);
            return false;
        }

        var settings = _settingsRepository.GetSettings(); // TODD in den SettingsService auslagern (?)
        var isMainGroup = settings.MainGroup.MainGroupIdentifier.Equals(identifier);
        if (isMainGroup) {
            PromptHelper.PrintColoredText(ConsoleColor.Yellow, UIstrings.DELETE_STANDARD_GROUP);
            PromptHelper.PrintColoredText(ConsoleColor.Yellow, UIstrings.PROVIDE_NEW_STANDARD_GROUP);
        }

        if (groups.Count > 0) {
            SwitchGroup(groups);
        } else {
            CreateNewGroupAndSwitchToIt();
        }

        if (isMainGroup) { // TODD in den SettingsService auslagern (!)
            settings.MainGroup = new MainGroupSetting(_userEnv.CurrentGroup!.Identifier);
            _settingsRepository.UpdateSettings(settings);
        }

        _groupService.DeleteGroup(identifier);
        PromptHelper.PrintColoredText(ConsoleColor.Green,UIstrings.DeletionOfGroupConfirmed(identifier));
        return true;
    }
}

