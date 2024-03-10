
using System.ComponentModel.DataAnnotations;

namespace PWManager.CLI.Enums; 

public enum AccountAction {
    [Display(Name = "Copy Password")]
    COPY_PASSWORD,
    [Display(Name = "Copy Login-Name")]
    COPY_LOGINNAME,
    [Display(Name = "Regenerate Password")]
    REGENERATE_PASSWORD,
    [Display(Name = "Delete")]
    DELETE,
    [Display(Name = "Go Back")]
    RETURN
}