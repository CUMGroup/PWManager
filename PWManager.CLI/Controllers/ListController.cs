using PWManager.CLI.Attributes;
using PWManager.CLI.Enums;
using PWManager.CLI.Interfaces;

namespace PWManager.CLI.Controllers; 

[SessionOnly]
public class ListController : IController {
    
    
    public ExitCondition Handle(string[] args) {
        
        
        
        return ExitCondition.CONTINUE;
    }
}