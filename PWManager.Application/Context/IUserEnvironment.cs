using PWManager.Domain.Entities;

namespace PWManager.Application.Context; 

public interface IUserEnvironment {
    public User? CurrentUser { get; set; }
    
    public Group? CurrentGroup { get; set; }
}