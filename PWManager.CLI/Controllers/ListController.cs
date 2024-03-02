using PWManager.Application.Services.Interfaces;
using PWManager.CLI.Attributes;
using PWManager.CLI.Enums;
using PWManager.CLI.Interfaces;

namespace PWManager.CLI.Controllers; 

[SessionOnly]
public class ListController : IController {

    private readonly IAccountService _accountService;
    public ListController(IAccountService accountService) {
        _accountService = accountService;
    }

    public ExitCondition Handle(string[] args) {

        var accounts = _accountService.GetCurrentAccountNames();

        PrintPaginated(accounts);
        Console.WriteLine();
        
        return ExitCondition.CONTINUE;
    }

    private void PrintPaginated(List<string> lines) {
        var index = 0;
        while (index < lines.Count) {
            if (index >= Console.BufferHeight - 1) {
                Console.Write("(Press enter to view more, q to stop)");
                var input = Console.ReadKey(intercept:true);
                ClearConsoleLine();
                if (input.Key == ConsoleKey.Q) {
                    return;
                }

                if (input.Key is not (ConsoleKey.Enter or ConsoleKey.DownArrow)) {
                    continue;
                }
            }

            ClearConsoleLine();
            Console.WriteLine(lines[index]);
            index++;
        }
    }

    private void ClearConsoleLine() {
        var currentLine = Console.CursorTop;
        Console.SetCursorPosition(0, Console.CursorTop);
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, currentLine);
    }
}