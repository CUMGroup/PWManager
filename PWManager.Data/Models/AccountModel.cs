using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PWManager.Data.Models; 

internal class AccountModel {
    
    [Key]
    public string Id { get; set; }
    
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTimeOffset Created { get; set; }
    
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTimeOffset Updated { get; set; }
    
    [Required]
    public string IdentifierCrypt { get; set; }
    
    [Required]
    public string LoginNameCrypt { get; set; }
    [Required]
    public string PasswordCrypt { get; set; }
}