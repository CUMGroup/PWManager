
using System.ComponentModel.DataAnnotations;

namespace PWManager.CLI.Enums; 

public enum AccountAction {
    [Display(Name = UIstrings.ACTION_COPY_PASSWORD)]
    COPY_PASSWORD,
    [Display(Name = UIstrings.ACTION_COPY_LOGIN)]
    COPY_LOGINNAME,
    [Display(Name = UIstrings.ACTION_REGNERATE_PW)]
    REGENERATE_PASSWORD,
    [Display(Name = UIstrings.ACTION_DELETE_ACCOUNT)]
    DELETE,
    [Display(Name = UIstrings.ACTION_RETURN)]
    RETURN
}