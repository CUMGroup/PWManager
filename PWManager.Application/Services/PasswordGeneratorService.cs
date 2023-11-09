﻿using System.Text;
using PWManager.Application.Interfaces;
using PWManager.Domain.Exceptions;
using PWManager.Domain.Services.Interfaces;
using PWManager.Domain.ValueObjects;

namespace PWManager.Application.Services; 

public class PasswordGeneratorService : IPasswordGeneratorService {

    private readonly IUserSettings _userSettings;

    private const string Lowercase = "abcdefghijklmnopqrstuvwxyz";
    private const string Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string SpecialChars = "!#$%&*+,-.:;<=>?^_~";
    private const string Numeric = "0123456789";
    private const char Space = ' ';
    private const string Brackets = "()[]{}";

    private readonly Random _rng;
    
    public PasswordGeneratorService(IUserSettings userSettings) : this(userSettings, new Random()) {
    }

    public PasswordGeneratorService(IUserSettings userSettings, Random rng) {
        _userSettings = userSettings;
        _rng = rng;
    }

    public string GeneratePasswordWith(PasswordGeneratorCriteria criteria) {
        var possibleChars = BuildPossibleChars(criteria);
        if (possibleChars.Length <= 0) {
            throw new PasswordGenerationException("Possible Password character set is empty!");
        }

        var password = new StringBuilder();
        var passwordLength = _rng.Next(criteria.MinLength, criteria.MaxLength + 1);
        for (int i = 0; i < passwordLength; ++i) {
            var randomCharIndex = _rng.Next(possibleChars.Length);
            password.Append(possibleChars[randomCharIndex]);
        }

        return password.ToString();
    }

    public string GeneratePassword() {
        return GeneratePasswordWith(_userSettings.GetPasswordGeneratorCriteria());
    }

    private static char[] BuildPossibleChars(PasswordGeneratorCriteria criteria) {
        var allPossibleChars = new StringBuilder();
        if (criteria.IncludeUpperCase) allPossibleChars.Append(Uppercase);
        if (criteria.IncludeBrackets) allPossibleChars.Append(Brackets);
        if (criteria.IncludeNumeric) allPossibleChars.Append(Numeric);
        if (criteria.IncludeSpaces) allPossibleChars.Append(Space);
        if (criteria.IncludeSpecial) allPossibleChars.Append(SpecialChars);
        if (criteria.IncludeLowerCase) allPossibleChars.Append(Lowercase);
        return allPossibleChars.ToString().ToCharArray();
    }
}