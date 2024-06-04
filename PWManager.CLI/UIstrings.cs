namespace PWManager.CLI;
internal static class UIstrings {

    // Console Runner
    public const string ERROR_OCCURED = "An Error occured!";
    // ----------------------------------------

    // Command Parser
    public const string UNKNOWN_COMMAND = "Unknown command ";
    // ----------------------------------------

    // CLI Enviroment
    public const string USER = "User";
    public const string GROUP = "Group";
    // ----------------------------------------

    // Password Criteria Settings
    public const string PWCRITERIA_LOWER_CASE = "Lower case characters: a-z";
    public const string PWCRITERIA_UPPER_CASE = "Upper case characters: A-Z";
    public const string PWCRITERIA_NUMERIC = "Nnumeric characters: 0-9";
    public const string PWCRITERIA_SPECIAL = "Special characters: !#$%&*+,-.:;<=>?^_~?";
    public const string PWCRITERIA_BRACKETS = "Brackets: ()[]{}";
    public const string PWCRITERIA_SPACE = "Spaces";
    // ----------------------------------------

    // ACTIONS
    public const string SELECT_ACTION = "Select an action";

    // Settings
    public const string ACTION_SHOW_SETTINGS = "Show current settings";
    public const string ACTION_CHANGE_MAIN_GROUP = "Change Main Group";
    public const string ACTION_CHANGE_CLIPBOARD_TIMEOUT = "Change Clipboard Timeout";
    public const string ACTION_CHANGE_ACCOUNT_TIMEOUT = "Change Account Timeout";
    public const string ACTION_CHANGE_PW_CRITERIA = "Change Password Criterias";

    // Account
    public const string ACTION_COPY_PASSWORD = "Copy Password";
    public const string ACTION_COPY_LOGIN = "Copy Login-Name";
    public const string ACTION_REGNERATE_PW = "Regenerate Password";
    public const string ACTION_DELETE_ACCOUNT = "Delete";

    // Group
    public const string ACTION_NEW_GROUP = "New group";
    public const string ACTION_SWITCH_GROUP = "Switch group";
    public const string ACTION_LIST_GROUPS = "List all groups";
    public const string ACTION_DELETE_GROUP = "Delete group";

    public const string ACTION_RETURN = "Go Back";
    // ----------------------------------------

    // Prompt Helper
    public const string EMPTY_INPUT = "Your input was empty! Try again!";
    public const string PASSWORD_TOO_SHORT = "Your password was too short. Please use a password with at least 4 characters!";

    public static string ValueCannotBeLessThan(int val) => $"Value cannot be less than {val}";
    
    public const string ENTER_PASSWORD = "Enter your password";
    public const string ENTER_MASTER_PASSWORD = "Enter your master password to confirm";

    public const string REPEAT_PASSWORD = "Repeat your password";
    public const string REPEAT_PASSWORD_FAILED = "You failed to repeat your password 3 times in a row!";
    public const string REPEAT_PASSWORD_DOES_NOT_MATCH = "The repeated password does not match your password!";

    public const string INVALID_PASSWORD = "Invalid password.";
    public const string TRY_AGAIN = "Please try again.";

    public const string PAGINATION_HINT = "(Press enter to view more, q to stop)";
    public static string DeletionOf(string input) => $"Are you sure you want to delete {input}?";
    // ----------------------------------------

    // INIT Controller
    public const string DESIRED_PATH = "Where do you want to create your database file?";
    public const string PATH_DOES_NOT_EXIST = "The given path does not exist.";

    public const string DESIRED_NAME = "What's your desired user name?";
    public const string INVALID_NAME = "Invalid name! It must be longer than 1 character and must include only letters!";

    public const string CREATED_DATABASE = "Created your database! Enjoy";
    // ----------------------------------------

    // LOGIN Controller
    public const string ENTER_USERNAME = "Please enter your username";
    public const string ENTER_PATH = "Please enter the location of your databasefile";
    public static string WelcomeMessage(string input) => $"Welcome {input} :)";
    // ----------------------------------------

    // GROUP Controller
    // public const string NO_GROUPS_AVAILABLE = "There are no groups in your database. Something is really wrong!";

    public const string SWITCH_GROUP_PROMPT = "To which group do you want to switch to";
    public const string NEW_GROUP_NAME = "What's the name of the new group";
    public const string GROUP_SWITCH_CONFIRM = "Switched to new group!";

    public const string DELETE_ABORTED = "Delete aborted!";

    public const string DELETE_STANDARD_GROUP = "You are going to delete your standard group.";
    public const string PROVIDE_NEW_STANDARD_GROUP = "Please provide a new standard group.";

    public static string DeletionOfGroupConfirmed(string input) => $"Group '{input}' deleted!";
    // ----------------------------------------

    // NEW Controller
    public const string PROMPT_NAME_WEBSITE = "What's the name of the website?";
    public const string PROMPT_LOGIN_NAME = "What's your name or email for the login?";
    public const string PROMPT_GENERATE_PW = "Do you want to generate a random password?";

    public const string PASSWORD_PROVISION_FAILED = "You failed to provide a password. Please try again creating your account!";
    public const string ACCOUNT_CREATION_CONFIRM = "Created the account!";
    // ----------------------------------------

    // GET Controller
    public const string COPIED_PASSWORD = "Copied the password to your clipboard!";
    public const string COPIED_LOGIN_NAME = "Copied the login-name to your clipboard!";


    public const string REGENERATION_ABORTED = "Regeneration aborted!";
    public const string REGENERATION_CONFIRMED = "Regenerated your password!";
    public static string ConfirmPwRegenerationOf(string input) => $"Are you sure you want to update the password of {input}?";

    public const string ACCOUNT_DELETION_CONFIRMED = "Account deleted!";

    public const string SEARCH_ACCOUNT = "Search an account";
    public const string NO_ACCOUNTS_AVAILABLE = "There are no accounts in this group!";
    // ----------------------------------------

    // DELETE DATABASE Controller
    public const string YOUR_DATABASE = "your database?";
    public const string DATABASE_DELETED = "Your database was deleted successfully!";
    
    // ----------------------------------------
    
    // SETTINGS Controller
    public const string MAIN_GROUP_CHANGE = "Which group will be your new main group?";
    public static string ConfirmOfMainGroupChangedTo(string input) => $"Main group set to '{input}'";

    public const string CLIPBOARD_TIMEOUT_PROMPT = "After how many seconds should your Clipboard be cleared?";
    public const string ACCOUNT_TIMEOUT_PROMPT = "After how many minutes of inactivity should the application quit?";
    
    public static string ConfirmOfClipboardTimeoutSetTo(int seconds) => $"Clipboard timeout is now set to {seconds} seconds";
    public static string ConfirmOfAccountTimeoutSetTo(int minutes) => $"Account timeout is now set to {minutes} minutes";

    public const string INCLUDE = "Include";

    public const string PWGEN_CRITERIA_PROMPT = "Please select your desired configurations:";
    public const string PWGEN_CRITERIA_CONFIRM = "New password generation criteria set!";
    public const string PWGEN_MINIMUM_PROMPT = "What's the minimum length your password should have?";
    public const string PWGEN_MAXIMUM_PROMPT = "What's the maximum length your password should have?";

    public const string MAIN_GROUP = "Main Group: ";
    public const string CLIPPBOARD_TIMEOUT = "Clipboard Timeout: ";
    public const string ACCOUNT_TIMEOUT = "Account Timeout: ";
    public const string PASSWORD_GEN_CRITERIA = "Password Generation Criteria:";
    public const string MIN_LENGTH = "Min Length: ";
    public const string MAX_LENGTH = "Max Length: ";
    // ----------------------------------------

    // HELP Controller
    public const string SEPARATOR = "---------------------------------";

    public const string IN_SESSIONHELP = $"""
                                         {SEPARATOR}
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
    public const string IN_SESSIONHELP_DELETEDB = $"""
                                         {SEPARATOR}
                                         Usage: $ delete-database
                                            Description:
                                                Deletes the entire database file if you are the only user. 
                                                Otherwise, it removes your user data from the file.
                                                
                                                YOU WILL LOSE ALL YOUR ACCOUNTS AND GROUPS!
                                         """;
    public const string IN_SESSIONHELP_GET = $"""
                                         {SEPARATOR}
                                         Usage: $ get
                                            Description:
                                                Lets you search through all accounts in this group.
                                                
                                                If you have selected an account, you can copy information, update your password, or delete the account.
                                         """;
    public const string IN_SESSIONHELP_GROUP = $"""
                                         {SEPARATOR}
                                         Usage: $ group
                                            Description:
                                                Displays different group based actions:
                                                    - Creating a new group
                                                    - Changing the current group
                                                    - List all of your groups
                                                    - Delete the current group
                                                Each option will present a fitting dialog as a guide.
                                         """;
    public const string IN_SESSIONHELP_LIST = $"""
                                         {SEPARATOR}
                                         Usage: $ list
                                            Description:
                                                Retrieves a list of all the names of the accounts of the current group.
                                         """;
    public const string IN_SESSIONHELP_NEW = $"""
                                         {SEPARATOR}
                                         Usage: $ new
                                            Description:
                                                Add a new account to the current group.
                                                You will get asked to enter an account identifier and a login name.
                                                You can either generate a new password or enter one yourself.
                                         """;
    public const string IN_SESSIONHELP_QUIT = $"""
                                         {SEPARATOR}
                                         Usage: $ quit
                                            Description:
                                                Quits the application. Instead you can also use Ctrl+C (SIGINT).
                                         """;
    public const string IN_SESSIONHELP_SETTINGS = $"""
                                         {SEPARATOR}
                                         Usage: $ settings
                                            Description:
                                                Opens a dialog to adjust the application settings
                                                - Clipboard Timeout to empty it after copying a password
                                                - Inactivity Timeout to automatically log out after a no action
                                                - Password Generator Settings
                                                - Main Group, that gets checked out after login 
                                         """;

    public const string NO_SESSIONHELP = $"""
                                         {SEPARATOR}
                                         Usage: pwMan [option]
                                            option:
                                                init            Creates a new database
                                                login           Login to your database
                                                help            To view this message
                                                help [option]   To view a help message for a command
                                         """;
    public const string NO_SESSIONHELP_INIT = $"""
                                         {SEPARATOR}
                                         Usage: pwMan init
                                            Description:
                                                Starts a dialog to initialize a new database for the password manager. 
                                                It asks where the database file should be located, whats the main user's name, and whats the master password.
                                         """;
    public const string NO_SESSIONHELP_LOGIN = $"""
                                         {SEPARATOR}
                                         Usage: pwMan login [options]
                                            options:
                                                -d, --directory     Specify the directory of the database file used to login. Default is the last used.
                                                -u, --username      Specify which user gets logged in. Default is the last used.
                                            Description:
                                                Tries to login the user into the password database. If no options are specified, the required ones (like password) will be asked!
                                         """;

    // ----------------------------------------
    
    // Timeout Service

    public static string KickedDueToInactivityFor(int minutes) =>
        $"You were logged out due to inactivity for {minutes} minutes";

    // -----------------------------------------
}
