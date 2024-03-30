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
    public const string ACTION_CHANGE_TIMEOUT = "Change Clipboard Timeout";
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
    public const string ACTION_DELETE_GROUP = "";

    public const string ACTION_RETURN = "Go Back";
    // ----------------------------------------

    // Prompt Helper
    public const string EMPTY_INPUT = "Your input was empty! Try again!";
    public const string PASSWORD_TOO_SHORT = "Your password was too short. Please use a password with at least 8 characters!";

    public const string ENTER_PASSWORD = "Enter your password";
    public const string ENTER_MASTER_PASSWORD = "Enter your password";

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
    public const string INVALID_NAME = "Invalid name! It mus be longer than 1 character and must include only letters!";

    public const string CREATED_DATABASE = "Created your database! Enjoy";
    // ----------------------------------------

    // LOGIN Controller
    public const string ENTER_USERNAME = "Please enter your username";
    public const string ENTER_PATH = "Please enter the location of your databasefile";
    public static string WelcomeMessage(string input) => $"Welcome {input} :)";
    // ----------------------------------------

    // GROUP Controller
    public const string NO_GROUPS_AVAILABLE = "There are no groups in your database. Something is really wrong!";

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
    // ----------------------------------------

    // SETTINGS Controller
    // ----------------------------------------

    // HELP Controller
    // ----------------------------------------
}
