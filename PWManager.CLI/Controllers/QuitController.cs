using PWManager.CLI.Attributes;
using PWManager.CLI.Enums;
using PWManager.CLI.Interfaces;

namespace PWManager.CLI.Controllers {
    [SessionOnly]
    public class QuitController : IController {
        public ExitCondition Handle(string[] args) {
            return ExitCondition.EXIT;
        }
    }
}
