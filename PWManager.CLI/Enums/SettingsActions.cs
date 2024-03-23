using System.ComponentModel.DataAnnotations;

namespace PWManager.CLI.Enums;
public enum SettingsActions {
    [Display(Name = "Show current settings")]
    CURRENT_SETTINGS, 
    [Display(Name = "Change Main Group")]
    MAIN_GROUP,
    [Display(Name = "Change Clipboard Timeout")]
    CLIPBOARD_TIMEOUT,
    [Display(Name = "Change Password Criterias")]
    PASSWORD_CRITERIA,
    [Display(Name = "Go Back")]
    RETURN
}
