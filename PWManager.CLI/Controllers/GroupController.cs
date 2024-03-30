using PWManager.Application.Context;
using PWManager.Application.Exceptions;
using PWManager.Application.Services.Interfaces;
using PWManager.CLI.Abstractions;
using PWManager.CLI.Attributes;
using PWManager.CLI.Enums;
using PWManager.CLI.Interfaces;
using PWManager.Domain.ValueObjects;
using Sharprompt;

namespace PWManager.CLI.Controllers;

[SessionOnly]
public class GroupController : IController {

    private readonly IUserEnvironment _userEnv;

    private readonly IGroupService _groupService;
    private readonly ISettingsService _settingsService;
    private readonly ILoginService _loginService;

    public GroupController(IUserEnvironment userEnv, IGroupService groupService, ILoginService loginService, ISettingsService settingsService) {
        _userEnv = userEnv;
        _groupService = groupService;
        _loginService = loginService;
        _settingsService = settingsService;
    }

    public ExitCondition Handle(string[] args) {

        GroupAction action;
        var executed = false;
        do {
            action = GetGroupAction();
            executed = ExecuteAction(action);
        } while ((action != GroupAction.DELETE_GROUP || !executed) && action != GroupAction.RETURN);

        return ExitCondition.CONTINUE;
    }

    private GroupAction GetGroupAction() {
        throw new NotImplementedException();
    }

    private bool ExecuteAction(GroupAction action) {
        return action switch {
            GroupAction.NEW_GROUP => HandleCreateNewGroupAndSwitchToIt(),
            GroupAction.SWITCH_GROUP => HandleSwitchGroup(_groupService.GetAllGroupNames()),
            GroupAction.LIST_GROUPS => HandleListAllGroups(),
            GroupAction.DELETE_GROUP => HandleDeletion(),
            GroupAction.RETURN => true,
            _ => false
        };
    }

    private bool HandleListAllGroups() {
        PromptHelper.PrintPaginated(_groupService.GetAllGroupNames());
        return true;
    }

    private bool HandleSwitchGroup(List<string> groups) {
        if (!groups.Any()) {
            throw new UserFeedbackException("There are no groups in your database. Something is really wrong!"); // TODO: Exception Message auslagern
        }
        var groupidentifier = Prompt.Select(UIstrings.SWITCH_GROUP_PROMPT, groups);
        _groupService.SwitchGroup(groupidentifier);

        return true;
    }

    private bool HandleCreateNewGroupAndSwitchToIt() {
        var groupIdentifier = PromptHelper.GetInput(UIstrings.NEW_GROUP_NAME);

        _groupService.AddGroup(_userEnv.CurrentUser!.Id, groupIdentifier);
        _groupService.SwitchGroup(groupIdentifier);

        PromptHelper.PrintColoredText(ConsoleColor.Green, UIstrings.GROUP_SWITCH_CONFIRM);
        return true;
    }

    private bool HandleDeletion() {
        var identifier = _userEnv.CurrentGroup!.Identifier;

        var username = _userEnv.CurrentUser!.UserName;
        if (!PromptHelper.ConfirmDeletion(identifier, (pT) => _loginService.CheckPassword(username, pT))) {
            PromptHelper.PrintColoredText(ConsoleColor.Red, UIstrings.DELETE_ABORTED);
            return false;
        }

        var settings = _settingsService.GetSettings(); 
        var isMainGroup = settings.MainGroup.MainGroupIdentifier.Equals(identifier);
        if (isMainGroup) {
            PromptHelper.PrintColoredText(ConsoleColor.Yellow, UIstrings.DELETE_STANDARD_GROUP);
            PromptHelper.PrintColoredText(ConsoleColor.Yellow, UIstrings.PROVIDE_NEW_STANDARD_GROUP);
        }

        var groups = _groupService.GetAllGroupNames();
        groups.Remove(identifier);

        if (groups.Count > 0) {
            HandleSwitchGroup(groups);
        } else {
            HandleCreateNewGroupAndSwitchToIt();
        }

        if (isMainGroup) { 
            var newMainGroupSetting = new MainGroupSetting(_userEnv.CurrentGroup!.Identifier);
            _settingsService.ChangeMainGroupSetting(newMainGroupSetting);
        }

        _groupService.DeleteGroup(identifier);
        PromptHelper.PrintColoredText(ConsoleColor.Green,UIstrings.DeletionOfGroupConfirmed(identifier));
        return true;
    }
}

