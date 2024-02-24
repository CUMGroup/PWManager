using PWManager.Domain.Services.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PWManager.Data.Models; 

internal class GroupModel : ISecureProperties{
    [Key]
    public string Id { get; set; }
    
    public DateTimeOffset Created { get; set; } = DateTimeOffset.Now;
    
    public DateTimeOffset Updated { get; set; } = DateTimeOffset.Now;
    
    [Required]
    public string IdentifierCrypt { get; set; }
    
    public List<AccountModel> Accounts { get; set; }

    public string UserId { get; set; }
    public UserModel User { get; set; }

    public List<(Func<string>, Action<string>)> SecurableProperties() {
        return new List<(Func<string>, Action<string>)> {
            ( () => IdentifierCrypt, (val) => IdentifierCrypt = val)
        };
    }
}