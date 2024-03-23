
using PWManager.Application.Services.Interfaces;
using PWManager.CLI.Abstractions;
using PWManager.CLI.Enums;
using PWManager.CLI.Interfaces;
using PWManager.Domain.Repositories;
using PWManager.Domain.ValueObjects;
using Sharprompt;

namespace PWManager.CLI.Controllers;
internal class SettingsController : IController {

    private ISettingsService _settingsService;
    private IGroupService _groupService;

    public SettingsController(ISettingsService settingsService, IGroupService groupService)
    {
        _settingsService = settingsService;
        _groupService = groupService;
    }

    public ExitCondition Handle(string[] args) {

        SettingsActions action;
        var executed = false;
        do {
            action = GetSettingsAction();
            executed = ExecuteAction(action);
        } while (!executed && action != SettingsActions.RETURN);

        return ExitCondition.CONTINUE;
    }

    private SettingsActions GetSettingsAction() {
        return Prompt.Select<SettingsActions>("Select an Action");
    }

    private bool ExecuteAction(SettingsActions action) {
        return action switch {
            SettingsActions.MAIN_GROUP => HandleChangeMainGroup(),
            SettingsActions.CLIPBOARD_TIMEOUT => HandleChangeClipboardTimeout(),
            SettingsActions.PASSWORD_CRITERIA => HandleChangePwGenCriteria(),
            SettingsActions.RETURN => true,
            _ => false
        };
    }

    private bool HandleChangeMainGroup() {
        var selectedgroup = Prompt.Select<string>("Which group will be your new main group?", _groupService.GetAllGroupNames());
        var newMainGroup = new MainGroupSetting(selectedgroup);
        _settingsService.ChangeMainGroupSetting(newMainGroup);
        PromptHelper.PrintColoredText(ConsoleColor.Green, $"Main group set to '{selectedgroup}'");
        return true;
    }

    private bool HandleChangeClipboardTimeout() {
        var seconds = Prompt.Input<int>("After how many seconds shoud your Clipboard be cleared?");
        while(seconds <= 0) {
            PromptHelper.PrintColoredText(ConsoleColor.Red, "Timeout cannot be less or equal than 0");
            seconds = Prompt.Input<int>("After how many seconds shoud your Clipboard be cleared?");
        }
        var timeout = new TimeSpan(TimeSpan.TicksPerSecond * seconds);
        _settingsService.ChangeClipboardTimeoutSetting(new ClipboardTimeoutSetting(timeout));
        PromptHelper.PrintColoredText(ConsoleColor.Green, $"Timeout is now set to {seconds} seconds");
        return true;
    }

    private bool HandleChangePwGenCriteria() {
        var pwGenCriteria = CreatePwGenerationCriteria();
        if(pwGenCriteria is null) {
            return false;
        }
        _settingsService.ChangePasswordGenerationCriteria(pwGenCriteria);
        PromptHelper.PrintColoredText(ConsoleColor.Green, "New password generation criteria set!");
        return true;
    }

    private PasswordGeneratorCriteria? CreatePwGenerationCriteria() {
        Console.WriteLine("Please select your desired configurations:");
        var selects = Prompt.MultiSelect("Include", defaultValues: new PasswordCriteriaOptions[] { PasswordCriteriaOptions.LOWER_CASE, PasswordCriteriaOptions.UPPER_CASE, PasswordCriteriaOptions.NUMERIC });

        var includeLowerCase = selects.Contains(PasswordCriteriaOptions.LOWER_CASE); 
        var includeUpperCase = selects.Contains(PasswordCriteriaOptions.UPPER_CASE); ; 
        var includeNumeric = selects.Contains(PasswordCriteriaOptions.NUMERIC); ;
        var includeSpecial = selects.Contains(PasswordCriteriaOptions.SPECIAL); ;
        var includeBrackets = selects.Contains(PasswordCriteriaOptions.BRACKETS); ;
        var includeSpaces = selects.Contains(PasswordCriteriaOptions.SPACE); ;

        var minLength = Prompt.Input<int>("What's the minimum length your passwoard should have?");
        var maxLength = Prompt.Input<int>("What's the maximum length your passwoard should have?");

        try {
            var pwGenCriteria = new PasswordGeneratorCriteria(includeLowerCase, includeUpperCase, includeNumeric, includeSpecial, includeBrackets, 
                includeSpaces, minLength, maxLength);
            return pwGenCriteria;
        } catch (Exception ex) {
            PromptHelper.PrintColoredText(ConsoleColor.Red, ex.Message);
            return null;
        }
    }
}
