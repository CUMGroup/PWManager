using System.ComponentModel.DataAnnotations;

namespace PWManager.CLI.Enums;
public enum SettingsActions {
    [Display(Name = UIstrings.ACTION_SHOW_SETTINGS)]
    CURRENT_SETTINGS, 
    [Display(Name = UIstrings.ACTION_CHANGE_MAIN_GROUP)]
    MAIN_GROUP,
    [Display(Name = UIstrings.ACTION_CHANGE_TIMEOUT)]
    CLIPBOARD_TIMEOUT,
    [Display(Name = UIstrings.ACTION_CHANGE_PW_CRITERIA)]
    PASSWORD_CRITERIA,
    [Display(Name = UIstrings.ACTION_RETURN)]
    RETURN
}
