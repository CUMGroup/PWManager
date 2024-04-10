using PWManager.Application.Exceptions;
using PWManager.Domain.ValueObjects;

namespace PWManager.Application.Services; 

public class PasswordBuilder {

    private PasswordGeneratorService _generatorService;

    private bool _includeLowerCase = false;
    private bool _includeUpperCase = false;
    private bool _includeSpecial = false;
    private bool _includeSpaces = false;
    private bool _includeNumeric = false;
    private bool _includeBrackets = false;
    private int _minLength = 5;
    private int _maxLength = 8;
    
    private PasswordBuilder() : this(new PasswordGeneratorService(null)) {}
    
    private PasswordBuilder(PasswordGeneratorService generatorService) {
        _generatorService = generatorService;
    }

    public PasswordBuilder SetMinLength(int minLength) {
        if (minLength <= 0) {
            throw new PasswordGenerationException(MessageStrings.MIN_LENGTH_TO_SMALL);
        }
        _minLength = minLength;
        return this;
    }
    
    public PasswordBuilder SetMaxLength(int maxLength) {
        if (maxLength < _minLength) {
            throw new PasswordGenerationException(MessageStrings.MAX_LENGTH_TO_SMALL);
        }
        _maxLength = maxLength;
        return this;
    }
    
    public PasswordBuilder IncludeUppercase() {
        _includeUpperCase = true;
        return this;
    }
    public PasswordBuilder IncludeLowercase() {
        _includeLowerCase = true;
        return this;
    }
    public PasswordBuilder IncludeSpecialChars() {
        _includeSpecial = true;
        return this;
    }
    public PasswordBuilder IncludeSpaces() {
        _includeSpaces = true;
        return this;
    }
    public PasswordBuilder IncludeNumeric() {
        _includeNumeric = true;
        return this;
    }
    public PasswordBuilder IncludeBrackets() {
        _includeBrackets = true;
        return this;
    }

    public string BuildPassword() {
        var criteria = BuildCriteria();
        return _generatorService.GeneratePasswordWith(criteria);
    }

    internal PasswordGeneratorCriteria BuildCriteria() {
        return new PasswordGeneratorCriteria(_includeLowerCase, _includeUpperCase, _includeNumeric,
            _includeSpecial, _includeBrackets, _includeSpaces, _minLength, _maxLength);
    }

    public static PasswordBuilder Create() {
        return new PasswordBuilder();
    }
}