using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PWManager.Data.Models; 

internal class SettingsModel {
    
    [Key]
    public string Id { get; set; }
    
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTimeOffset Created { get; set; }
    
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTimeOffset Updated { get; set; }
    
    public string UserId { get; set; }
    public UserModel User { get; set; }

    [Required] 
    public bool IncludeLowerCase { get; set; } = true;
    [Required]
    public bool IncludeUpperCase { get; set; } = true;
    [Required]
    public bool IncludeNumeric { get; set; } = true;
    [Required]
    public bool IncludeSpecial { get; set; } = true;
    [Required]
    public bool IncludeSpaces { get; set; } = false;
    [Required]
    public bool IncludeBrackets { get; set; } = false;
    [Required] 
    public int MinLength { get; set; } = 8; 
    [Required]
    public int MaxLength { get; set; } = 10;
    
    public TimeSpan TimeOutDuration { get; set; }
    
    [Required]
    public string MainGroupIdentifier { get; set; } = "main";
}