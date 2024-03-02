using PWManager.Application.Context;
using PWManager.CLI.Enums;
using PWManager.CLI.Interfaces;

namespace PWManager.CLI.Controllers; 

public class HelpController : IController {

    private readonly ICliEnvironment _env;
    public HelpController(ICliEnvironment env) {
        _env = env;
    }

    public ExitCondition Handle(string[] args) {
        if (_env.RunningSession) {
            Console.WriteLine("help for running session");
            return ExitCondition.CONTINUE;
        }
        Console.WriteLine("Help for new session");
        return ExitCondition.EXIT;
    }
}