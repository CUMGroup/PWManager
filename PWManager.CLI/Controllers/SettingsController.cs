
using PWManager.Application.Exceptions;
using PWManager.Application.Services.Interfaces;
using PWManager.CLI.Abstractions;
using PWManager.CLI.Enums;
using PWManager.CLI.Interfaces;
using PWManager.Domain.ValueObjects;

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
        return ConsoleInteraction.Select<SettingsActions>(UIstrings.SELECT_ACTION);
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
            throw new UserFeedbackException(MessageStrings.NO_GROUPS_FOUND);
        }

        var selectedGroup = ConsoleInteraction.Select<string>(UIstrings.MAIN_GROUP_CHANGE, groups);
        var newMainGroup = new MainGroupSetting(selectedGroup);
        _settingsService.ChangeMainGroupSetting(newMainGroup);

        PromptHelper.PrintColoredText(ConsoleColor.Green, UIstrings.ConfirmOfMainGroupChangedTo(selectedGroup));
        return true;
    }

    private bool HandleChangeClipboardTimeout() {
        var seconds = PromptHelper.GetInputGreaterThan(UIstrings.CLIPBOARD_TIMEOUT_PROMPT, 0);

        var timeout = new TimeSpan(TimeSpan.TicksPerSecond * seconds);
        _settingsService.ChangeClipboardTimeoutSetting(timeout);

        PromptHelper.PrintColoredText(ConsoleColor.Green, UIstrings.ConfirmOfClipboardTimeoutSetTo(seconds));
        return true;
    }
    private bool HandleChangeAccountTimeout() {
        var minutes = PromptHelper.GetInputGreaterThan(UIstrings.ACCOUNT_TIMEOUT_PROMPT, 0);
        var timeout = new TimeSpan(TimeSpan.TicksPerSecond * minutes * 60);
        _settingsService.ChangeAccountTimeoutSetting(timeout);

        PromptHelper.PrintColoredText(ConsoleColor.Green, UIstrings.ConfirmOfAccountTimeoutSetTo(minutes));
        return true;
    }
    
    private bool HandleChangePwGenCriteria() {
        var pwGenCriteria = CreatePwGenerationCriteria();
        if(pwGenCriteria is null) {
            return false;
        }

        _settingsService.ChangePasswordGenerationCriteria(pwGenCriteria);
        PromptHelper.PrintColoredText(ConsoleColor.Green, UIstrings.PWGEN_CRITERIA_CONFIRM);

        return true;
    }

    private PasswordGeneratorCriteria? CreatePwGenerationCriteria() {
        Console.WriteLine(UIstrings.PWGEN_CRITERIA_PROMPT);

        List<PasswordCriteriaOptions> defaults = GetDefaults();

        var selects = ConsoleInteraction.MultiSelect(UIstrings.INCLUDE, defaultValues: defaults).ToArray();

        var includeLowerCase = selects.Contains(PasswordCriteriaOptions.LOWER_CASE);
        var includeUpperCase = selects.Contains(PasswordCriteriaOptions.UPPER_CASE); ;
        var includeNumeric = selects.Contains(PasswordCriteriaOptions.NUMERIC); ;
        var includeSpecial = selects.Contains(PasswordCriteriaOptions.SPECIAL); ;
        var includeBrackets = selects.Contains(PasswordCriteriaOptions.BRACKETS); ;
        var includeSpaces = selects.Contains(PasswordCriteriaOptions.SPACE); ;

        var minLength = ConsoleInteraction.Input<int>(UIstrings.PWGEN_MINIMUM_PROMPT);
        var maxLength = ConsoleInteraction.Input<int>(UIstrings.PWGEN_MAXIMUM_PROMPT);

        try {
            var pwGenCriteria = new PasswordGeneratorCriteria(includeLowerCase, includeUpperCase, includeNumeric, includeSpecial, includeBrackets,
                includeSpaces, minLength, maxLength);
            return pwGenCriteria;
        } catch (ArgumentException ex) {
            PromptHelper.PrintColoredText(ConsoleColor.Red, ex.Message);
            return null;
        }
    }

    private List<PasswordCriteriaOptions> GetDefaults() {
        var currentPWCriteria = _settingsService.GetSettings().PwGenCriteria;
        if (currentPWCriteria is null) {
            throw new UserFeedbackException(MessageStrings.NO_PWGEN_CRITERIA_FOUND);
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

        Console.WriteLine(UIstrings.SEPARATOR);

        Console.Write(UIstrings.MAIN_GROUP);
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(settings.MainGroup.MainGroupIdentifier);
        Console.ForegroundColor = defaultcolor;

        Console.Write(UIstrings.CLIPPBOARD_TIMEOUT);
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(settings.Timeout.ClipboardTimeOutDuration.TotalSeconds + " s");
        Console.ForegroundColor = defaultcolor; 
        
        Console.Write("Account Timeout: ");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(settings.Timeout.AccountTimeOutDuration.TotalMinutes + " m");
        Console.ForegroundColor = defaultcolor;

        Console.WriteLine(UIstrings.SEPARATOR);

        Console.WriteLine(UIstrings.PASSWORD_GEN_CRITERIA);
        Console.Write(UIstrings.MIN_LENGTH);
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(settings.PwGenCriteria.MinLength);
        Console.ForegroundColor = defaultcolor;
        Console.Write(UIstrings.MAX_LENGTH);
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(settings.PwGenCriteria.MaxLength);
        Console.ForegroundColor = defaultcolor;

        ShowInclude(settings.PwGenCriteria.IncludeLowerCase);
        Console.WriteLine(UIstrings.PWCRITERIA_LOWER_CASE);

        ShowInclude(settings.PwGenCriteria.IncludeUpperCase);
        Console.WriteLine(UIstrings.PWCRITERIA_UPPER_CASE);

        ShowInclude(settings.PwGenCriteria.IncludeNumeric);
        Console.WriteLine(UIstrings.PWCRITERIA_NUMERIC);

        ShowInclude(settings.PwGenCriteria.IncludeSpecial);
        Console.WriteLine(UIstrings.PWCRITERIA_SPECIAL);

        ShowInclude(settings.PwGenCriteria.IncludeBrackets);
        Console.WriteLine(UIstrings.PWCRITERIA_BRACKETS);

        ShowInclude(settings.PwGenCriteria.IncludeSpaces);
        Console.WriteLine(UIstrings.PWCRITERIA_SPACE);
        Console.WriteLine(UIstrings.SEPARATOR);

        Console.ForegroundColor = defaultcolor;

        return true;
    }

    private static void ShowInclude(bool include) {
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
