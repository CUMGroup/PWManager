using System.ComponentModel.DataAnnotations;

namespace PWManager.CLI.Enums;
public enum PasswordCriteriaOptions {
    [Display(Name = UIstrings.PWCRITERIA_LOWER_CASE)]
    LOWER_CASE,
    [Display(Name = UIstrings.PWCRITERIA_UPPER_CASE)]
    UPPER_CASE,
    [Display(Name = UIstrings.PWCRITERIA_NUMERIC)]
    NUMERIC,
    [Display(Name = UIstrings.PWCRITERIA_SPECIAL)]
    SPECIAL,
    [Display(Name = UIstrings.PWCRITERIA_BRACKETS)]
    BRACKETS,
    [Display(Name = UIstrings.PWCRITERIA_SPACE)]
    SPACE
}
