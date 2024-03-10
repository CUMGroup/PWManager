using PWManager.Domain.Services.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PWManager.Data.Models; 

internal class GroupModel : ISecureProperties{
    [Key]
    public string Id { get; set; } = null!;
    
    public DateTimeOffset Created { get; set; } = DateTimeOffset.Now;
    
    public DateTimeOffset Updated { get; set; } = DateTimeOffset.Now;
    
    [Required]
    public string IdentifierCrypt { get; set; } = null!;
    
    public List<AccountModel> Accounts { get; set; } = null!;

    public string UserId { get; set; } = null!;
    public UserModel User { get; set; } = null!;

    public List<(Func<string>, Action<string>)> SecurableProperties() {
        return new List<(Func<string>, Action<string>)> {
            ( () => IdentifierCrypt, (val) => IdentifierCrypt = val)
        };
    }
}