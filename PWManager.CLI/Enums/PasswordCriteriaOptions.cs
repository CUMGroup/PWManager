using System.ComponentModel.DataAnnotations;

namespace PWManager.CLI.Enums;
public enum PasswordCriteriaOptions {
    [Display(Name = "Lower case characters: a-z")]
    LOWER_CASE,
    [Display(Name = "Upper case characters: A-Z")]
    UPPER_CASE,
    [Display(Name = "Nnumeric characters: 0-9")]
    NUMERIC,
    [Display(Name = "Special characters: !#$%&*+,-.:;<=>?^_~?")]
    SPECIAL,
    [Display(Name = "Brackets: ()[]{}")]
    BRACKETS,
    [Display(Name = "Spaces")]
    SPACE
}
