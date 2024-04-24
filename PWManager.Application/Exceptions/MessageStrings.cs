namespace PWManager.Application.Exceptions;
public static class MessageStrings {
    // PW GEN SERVICE
    public const string EMPTY_CHARACTER_SET = "Possible Password character set is empty!";
    // ----------------------------------------

    // CRYPT SERVICE
    public const string ENCRYPT_KEY_NULL = "Encryption Key is null! Are you in a session?";
    // ----------------------------------------

    // ACCOUNT SERVICE
    public const string ACCOUNT_NOT_FOUND = "Could not find the account!";
    public const string FAILED_ADDING_ACCOUNT = "Failed adding the account!";
    public static string AccountAlreadyExist(string input) => $"Account with identifier '{input}' does already exist in your group!";
    public const string NO_ACTIVE_GROUP = "No active group found. Are you in a session?";
    public const string NO_ACTIVE_USER = "No active user found. Are you in a session?";

    // ----------------------------------------

    // GROUP SERVICE
    public static string GroupAlreadyExist(string input) => $"A group with the name '{input}' already exists!";
    public static string FailedDeletingGroup(string input) => $"Could not delete group '{input}'";
    // ----------------------------------------

    // GROUP Controller
    public const string NO_GROUPS_FOUND = "There are no groups in your database. Something is really wrong!";
    public const string NO_SETTINGS_IN_ENVIRONMENT = "There are not settings in your environment. Something is really wrong!";
    // ----------------------------------------

    // SETTINGS Controller
    public const string NO_PWGEN_CRITERIA_FOUND = "There are no password generation criteria set. Something is really wrong!";
    // ----------------------------------------

    // Console Runner
    public const string CONTROLER_TYPE_ERROR = "Controller type does not implement IController interface!";
    // ----------------------------------------

    // Config File Handler
    public const string READ_FILE_ERROR = "The config file could not be read.";
    public const string WRITE_FILE_ERROR = "The config file could not be written.";
    public const string DELETE_FILE_ERROR = "The config file could not be deleted.";
    public const string PATH_ERROR = "An unknown error occured! Could not determine execution path!";
    public const string DIRECTORY_ERROR = "An unknown error occured! Execution path is not a directory!";
    // ----------------------------------------
    
    // PasswordBuilder
    public const string MIN_LENGTH_TO_SMALL = "MinLength cannot be smaller than 0";
    public const string MAX_LENGTH_TO_SMALL = "MaxLength cannot be smaller than MinLength";
    
    // DataContext
    public const string CANNOT_DELETE_DATABASE = "Cannot delete the database!";
}
