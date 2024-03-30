﻿
using PWManager.Application.Exceptions;
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
        do {
            action = GetSettingsAction();
            ExecuteAction(action);
        } while (action != SettingsActions.RETURN);

        return ExitCondition.CONTINUE;
    }

    private SettingsActions GetSettingsAction() {
        return Prompt.Select<SettingsActions>("Select an Action");
    }

    private bool ExecuteAction(SettingsActions action) {
        return action switch {
            SettingsActions.CURRENT_SETTINGS => ShowCurrentSettings(),
            SettingsActions.MAIN_GROUP => HandleChangeMainGroup(),
            SettingsActions.CLIPBOARD_TIMEOUT => HandleChangeClipboardTimeout(),
            SettingsActions.ACCOUNT_TIMEOUT => HandleChangeAccountTimeout(),
            SettingsActions.PASSWORD_CRITERIA => HandleChangePwGenCriteria(),
            SettingsActions.RETURN => true,
            _ => false
        };
    }

    private bool HandleChangeMainGroup() {
        var groups = _groupService.GetAllGroupNames();
        if (!groups.Any()) {
            throw new UserFeedbackException("There are no groups in your database. Something is really wrong!");
        }

        var selectedgroup = ConsoleInteraction.Select<string>("Which group will be your new main group?", groups);
        var newMainGroup = new MainGroupSetting(selectedgroup);
        _settingsService.ChangeMainGroupSetting(newMainGroup);

        PromptHelper.PrintColoredText(ConsoleColor.Green, $"Main group set to '{selectedgroup}'");
        return true;
    }

    private bool HandleChangeClipboardTimeout() {
        var seconds = PromptHelper.GetInputGreaterThan("After how many seconds should your Clipboard be cleared?", 0);
        var timeout = new TimeSpan(TimeSpan.TicksPerSecond * seconds);
        _settingsService.ChangeClipboardTimeoutSetting(timeout);

        PromptHelper.PrintColoredText(ConsoleColor.Green, $"Clipboard Timeout is now set to {seconds} seconds");
        return true;
    }
    private bool HandleChangeAccountTimeout() {
        var minutes = PromptHelper.GetInputGreaterThan("After how many minutes of inactivity should the application quit?", 0);
        var timeout = new TimeSpan(TimeSpan.TicksPerSecond * minutes * 60);
        _settingsService.ChangeAccountTimeoutSetting(timeout);

        PromptHelper.PrintColoredText(ConsoleColor.Green, $"Account Timeout is now set to {minutes} minutes");
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

        List<PasswordCriteriaOptions> defaults = getDefaults();

        var selects = ConsoleInteraction.MultiSelect<PasswordCriteriaOptions>("Include", defaultValues: defaults);

        var includeLowerCase = selects.Contains(PasswordCriteriaOptions.LOWER_CASE);
        var includeUpperCase = selects.Contains(PasswordCriteriaOptions.UPPER_CASE); ;
        var includeNumeric = selects.Contains(PasswordCriteriaOptions.NUMERIC); ;
        var includeSpecial = selects.Contains(PasswordCriteriaOptions.SPECIAL); ;
        var includeBrackets = selects.Contains(PasswordCriteriaOptions.BRACKETS); ;
        var includeSpaces = selects.Contains(PasswordCriteriaOptions.SPACE); ;

        var minLength = ConsoleInteraction.Input<int>("What's the minimum length your password should have?");
        var maxLength = ConsoleInteraction.Input<int>("What's the maximum length your password should have?");

        try {
            var pwGenCriteria = new PasswordGeneratorCriteria(includeLowerCase, includeUpperCase, includeNumeric, includeSpecial, includeBrackets,
                includeSpaces, minLength, maxLength);
            return pwGenCriteria;
        } catch (ArgumentException ex) {
            PromptHelper.PrintColoredText(ConsoleColor.Red, ex.Message);
            return null;
        }
    }

    private List<PasswordCriteriaOptions> getDefaults() {
        var currentPWCriteria = _settingsService.GetSettings().PwGenCriteria;
        if (currentPWCriteria is null) {
            throw new UserFeedbackException("There are no password generation criteria set. Something is really wrong!");
        }

        var defaults = new List<PasswordCriteriaOptions>();
        if (currentPWCriteria.IncludeLowerCase) defaults.Add(PasswordCriteriaOptions.LOWER_CASE);
        if (currentPWCriteria.IncludeUpperCase) defaults.Add(PasswordCriteriaOptions.UPPER_CASE);
        if (currentPWCriteria.IncludeNumeric) defaults.Add(PasswordCriteriaOptions.NUMERIC);
        if (currentPWCriteria.IncludeSpecial) defaults.Add(PasswordCriteriaOptions.SPECIAL);
        if (currentPWCriteria.IncludeBrackets) defaults.Add(PasswordCriteriaOptions.BRACKETS);
        if (currentPWCriteria.IncludeSpaces) defaults.Add(PasswordCriteriaOptions.SPACE);
        return defaults;
    }

    public bool ShowCurrentSettings() {
        var settings = _settingsService.GetSettings();

        var defaultcolor = Console.ForegroundColor;

        Console.WriteLine("-----------------------------");
        Console.Write("Main Group: ");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(settings.MainGroup.MainGroupIdentifier);
        Console.ForegroundColor = defaultcolor;

        Console.Write("Clipboard Timeout: ");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(settings.Timeout.ClipboardTimeOutDuration.TotalSeconds + " s");
        Console.ForegroundColor = defaultcolor; 
        
        Console.Write("Account Timeout: ");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(settings.Timeout.AccountTimeOutDuration.TotalMinutes + " m");
        Console.ForegroundColor = defaultcolor;

        Console.WriteLine("-----------------------------");

        Console.WriteLine("Password Generation Criteria:");
        Console.Write("Min Length: ");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(settings.PwGenCriteria.MinLength);
        Console.ForegroundColor = defaultcolor;
        Console.Write("Max Length: ");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(settings.PwGenCriteria.MaxLength);
        Console.ForegroundColor = defaultcolor;

        ShowInclude(settings.PwGenCriteria.IncludeLowerCase);
        Console.WriteLine(UIstrings.LOWER_CASE);

        ShowInclude(settings.PwGenCriteria.IncludeUpperCase);
        Console.WriteLine(UIstrings.UPPER_CASE);

        ShowInclude(settings.PwGenCriteria.IncludeNumeric);
        Console.WriteLine(UIstrings.NUMERIC);

        ShowInclude(settings.PwGenCriteria.IncludeSpecial);
        Console.WriteLine(UIstrings.SPECIAL);

        ShowInclude(settings.PwGenCriteria.IncludeBrackets);
        Console.WriteLine(UIstrings.BRACKETS);

        ShowInclude(settings.PwGenCriteria.IncludeSpaces);
        Console.WriteLine(UIstrings.SPACE);
        Console.WriteLine("-----------------------------");

        Console.ForegroundColor = defaultcolor;

        return true;
    }

    private void ShowInclude(bool include) {
        var defaultcolor = Console.ForegroundColor;

        if (include) {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("+ ");
        } else {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("- ");
        }

        Console.ForegroundColor = defaultcolor;
    }
}
