using System.Text;
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
    
    private const string InSessionHelp = """
                                         ---------------------------------
                                         Usage: $ [option]
                                            option:
                                                delete-database     Delete the current database
                                                get                 Get a specific account
                                                group               Add, Change, List or Delete groups
                                                list                List all accounts in the current group
                                                new                 Create a new account in the current group
                                                quit                Quits the application
                                                settings            Opens the settings menu
                                                help                To view this message
                                                help [option]       To view a help message for a command
                                         """;
    private const string InSessionHelpDeleteDb = """
                                         ---------------------------------
                                         Usage: $ delete-database
                                            Description:
                                                Deletes the entire database file if you are the only user. 
                                                Otherwise, it removes your user data from the file.
                                                
                                                YOU WILL LOSE ALL YOUR ACCOUNTS AND GROUPS!
                                         """;
    private const string InSessionHelpGet = """
                                         ---------------------------------
                                         Usage: $ get
                                            Description:
                                                Lets you search through all accounts in this group.
                                                
                                                If you have selected an account, you can copy information, update your password, or delete the account.
                                         """;
    private const string InSessionHelpGroup = """
                                         ---------------------------------
                                         Usage: $ group
                                            Description:
                                                Displays different group based actions:
                                                    - Creating a new group
                                                    - Changing the current group
                                                    - List all of your groups
                                                    - Delete the current group
                                                Each option will present a fitting dialog as a guide.
                                         """;
    private const string InSessionHelpList = """
                                         ---------------------------------
                                         Usage: $ list
                                            Description:
                                                Retrieves a list of all the names of the accounts of the current group.
                                         """;
    private const string InSessionHelpNew = """
                                         ---------------------------------
                                         Usage: $ new
                                            Description:
                                                Add a new account to the current group.
                                                You will get asked to enter an account identifier and a login name.
                                                You can either generate a new password or enter one yourself.
                                         """;
    private const string InSessionHelpQuit = """
                                         ---------------------------------
                                         Usage: $ quit
                                            Description:
                                                Quits the application. Instead you can also use Ctrl+C (SIGINT).
                                         """;
    private const string InSessionHelpSettings = """
                                         ---------------------------------
                                         Usage: $ settings
                                            Description:
                                                Opens a dialog to adjust the application settings
                                                - Clipboard Timeout to empty it after copying a password
                                                - Inactivity Timeout to automatically log out after a no action
                                                - Password Generator Settings
                                                - Main Group, that gets checked out after login 
                                         """;
    
    private const string NoSessionHelp = """
                                         ---------------------------------
                                         Usage: pwMan [option]
                                            option:
                                                init            Creates a new database
                                                login           Login to your database
                                                help            To view this message
                                                help [option]   To view a help message for a command
                                         """;
    private const string NoSessionHelpInit = """
                                         ---------------------------------
                                         Usage: pwMan init
                                            Description:
                                                Starts a dialog to initialize a new database for the password manager. 
                                                It asks where the database file should be located, whats the main user's name, and whats the master password.
                                         """;
    private const string NoSessionHelpLogin = """
                                         ---------------------------------
                                         Usage: pwMan login [options]
                                            options:
                                                -d, --directory     Specify the directory of the database file used to login. Default is the last used.
                                                -u, --username      Specify which user gets logged in. Default is the last used.
                                            Description:
                                                Tries to login the user into the password database. If no options are specified, the required ones (like password) will be asked!
                                         """;
}