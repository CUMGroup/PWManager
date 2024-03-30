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
            DisplayInSessionHelp(args.Length == 0 ? null : args[0]);
            return ExitCondition.CONTINUE;
        }
        DisplayOutOfSessionHelp(args.Length == 0 ? null : args[0]);
        return ExitCondition.EXIT;
    }
    
    private void DisplayOutOfSessionHelp(string? specificHelp) {
        switch (specificHelp?.ToLower()) {
            case "init":
                Console.WriteLine(NoSessionHelpInit);
                break;
            case "login":
                Console.WriteLine(NoSessionHelpLogin);
                break;
            default:
                Console.WriteLine(NoSessionHelp);
                break;
        }
    }

    private void DisplayInSessionHelp(string? specificHelp) {
        switch (specificHelp?.ToLower()) {
            case "group":
                Console.WriteLine(InSessionHelpGroup);
                break;
            case "new":
                Console.WriteLine(InSessionHelpNew);
                break;
            case "list":
                Console.WriteLine(InSessionHelpList);
                break;
            case "get":
                Console.WriteLine(InSessionHelpGet);
                break;
            case "settings":
                Console.WriteLine(InSessionHelpSettings);
                break;
            case "delete-database":
                Console.WriteLine(InSessionHelpDeleteDb);
                break;
            case "quit":
                Console.WriteLine(InSessionHelpQuit);
                break;
            default:
                Console.WriteLine(InSessionHelp);
                break;
        }
    }
    
    private const string InSessionHelp = UIstrings.IN_SESSIONHELP;
    private const string InSessionHelpDeleteDb = UIstrings.IN_SESSIONHELP_DELETEDB;
    private const string InSessionHelpGet = UIstrings.IN_SESSIONHELP_GET;
    private const string InSessionHelpGroup = UIstrings.IN_SESSIONHELP_GROUP;
    private const string InSessionHelpList = UIstrings.IN_SESSIONHELP_LIST;
    private const string InSessionHelpNew = UIstrings.IN_SESSIONHELP_NEW;
    private const string InSessionHelpQuit = UIstrings.IN_SESSIONHELP_QUIT;
    private const string InSessionHelpSettings = UIstrings.IN_SESSIONHELP_SETTINGS;
    
    private const string NoSessionHelp = UIstrings.NO_SESSIONHELP;
    private const string NoSessionHelpInit = UIstrings.NO_SESSIONHELP_INIT;
    private const string NoSessionHelpLogin = UIstrings.NO_SESSIONHELP_LOGIN;
}