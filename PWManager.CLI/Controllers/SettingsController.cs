
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

    public SettingsController(ISettingsService settingsService)
    {
        _settingsService = settingsService;
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

    private bool HandleChangeClipboardTimeout() {
        var seconds = Prompt.Input<int>("After how many seconds shoud your Clipboard be cleared?");
        while(seconds <= 0) {
            PromptHelper.PrintColoredText(ConsoleColor.Red, "Timeout cannot be less or equal than 0.");
            seconds = Prompt.Input<int>("After how many seconds shoud your Clipboard be cleared?");
        }
        var timeout = new TimeSpan(TimeSpan.TicksPerSecond*seconds);
        _settingsService.ChangeClipboardTimeoutSetting(new ClipboardTimeoutSetting(timeout));
        return true;
    }

    private bool HandleChangePwGenCriteria() {
        var pwGenCriteria = CreatePwGenerationCriteria();
        if(pwGenCriteria is null) {
            return false;
        }
        _settingsService.ChangePasswordGenerationCriteria(pwGenCriteria);
        return true;
    }

    private PasswordGeneratorCriteria? CreatePwGenerationCriteria() {
        Console.WriteLine("Please select your desired configurations:");

        var includeLowerCase = Prompt.Confirm("Include lower case characters: a-z?"); 
        var includeUpperCase = Prompt.Confirm("Include upper case characters: A-Z?"); 
        var includeNumeric = Prompt.Confirm("Include numeric characters: 0-9?");
        var includeSpecial = Prompt.Confirm("Include Special characters: !#$%&*+,-.:;<=>?^_~?");
        var includeBrackets = Prompt.Confirm("Include brackets: ()[]{}?");
        var includeSpaces = Prompt.Confirm("Include spaces?");

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
