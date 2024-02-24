using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PWManager.Data.Models; 

internal class UserModel {
    
    [Key]
    public string Id { get; set; }
    
    public DateTimeOffset Created { get; set; } = DateTimeOffset.Now;
    
    public DateTimeOffset Updated { get; set; } = DateTimeOffset.Now;
    
    [Required]
    public string UserName { get; set; }
    
    [Required]
    public string MasterHash { get; set; }
    
    [Required]
    public string Salt { get; set; }
}