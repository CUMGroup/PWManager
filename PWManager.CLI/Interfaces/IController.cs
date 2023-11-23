using PWManager.CLI.Enums;

namespace PWManager.CLI.Interfaces; 

public interface IController {

    ExitCondition Handle(string[] args);
    
}