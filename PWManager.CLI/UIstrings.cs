using System.Text.RegularExpressions;

namespace PWManager.CLI;
internal static class UIstrings {

    // Password Criteria Settings
    public const string PWCRITERIA_LOWER_CASE = "Lower case characters: a-z";
    public const string PWCRITERIA_UPPER_CASE = "Upper case characters: A-Z";
    public const string PWCRITERIA_NUMERIC = "Nnumeric characters: 0-9";
    public const string PWCRITERIA_SPECIAL = "Special characters: !#$%&*+,-.:;<=>?^_~?";
    public const string PWCRITERIA_BRACKETS = "Brackets: ()[]{}";
    public const string PWCRITERIA_SPACE = "Spaces";
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
}
