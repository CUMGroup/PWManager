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
    public const string REPEAT_PASSWORD_FAILED = "You failed to repeat your password 3 times in a row! Please try again!";

    public const string INVALID_PASSWORD = "Invalid password.";

    public const string PAGINATION_HINT = "(Press enter to view more, q to stop)";
    // ----------------------------------------
}
