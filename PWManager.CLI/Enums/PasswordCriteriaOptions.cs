using System.ComponentModel.DataAnnotations;

namespace PWManager.CLI.Enums;
public enum PasswordCriteriaOptions {
    [Display(Name = UIstrings.LOWER_CASE)]
    LOWER_CASE,
    [Display(Name = UIstrings.UPPER_CASE)]
    UPPER_CASE,
    [Display(Name = UIstrings.NUMERIC)]
    NUMERIC,
    [Display(Name = UIstrings.SPECIAL)]
    SPECIAL,
    [Display(Name = UIstrings.BRACKETS)]
    BRACKETS,
    [Display(Name = UIstrings.SPACE)]
    SPACE
}
