using PWManager.Domain.Services.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PWManager.Data.Models; 

internal class AccountModel : ISecureProperties {
    
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
    
    public string GroupId { get; set; }

    public List<(Func<string>, Action<string>)> SecurableProperties() {
        return new List<(Func<string>, Action<string>)> { 
            ( () => IdentifierCrypt, (val) => IdentifierCrypt = val),
            ( () => LoginNameCrypt, (val) => LoginNameCrypt = val),
            ( () => PasswordCrypt, (val) => PasswordCrypt = val)
        };
    }
}