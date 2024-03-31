using System.ComponentModel.DataAnnotations;

namespace PWManager.CLI.Enums;
public enum GroupAction {
    [Display(Name = UIstrings.ACTION_NEW_GROUP)]
    NEW_GROUP,
    [Display(Name = UIstrings.ACTION_SWITCH_GROUP)]
    SWITCH_GROUP,
    [Display(Name = UIstrings.ACTION_LIST_GROUPS)]
    LIST_GROUPS,
    [Display(Name = UIstrings.ACTION_DELETE_GROUP)]
    DELETE_GROUP,
    [Display(Name = UIstrings.ACTION_RETURN)]
    RETURN
}
