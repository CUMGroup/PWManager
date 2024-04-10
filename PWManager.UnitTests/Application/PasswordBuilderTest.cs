using NSubstitute;
using PWManager.Application.Services;
using PWManager.Domain.Services.Interfaces;
using PWManager.Domain.ValueObjects;

namespace PWManager.UnitTests.Application; 

public class PasswordBuilderTest {

    [Fact]
    public void PasswordBuilder_Should_SetMinLength() {
        var criteria = PasswordBuilder.Create()
            .IncludeUppercase()
            .SetMinLength(8)
            .BuildCriteria();
        
        Assert.Equal(8, criteria.MinLength);
    }
    
    [Fact]
    public void PasswordBuilder_Should_SetMaxLength() {
        var criteria = PasswordBuilder.Create()
            .IncludeUppercase()
            .SetMaxLength(80)
            .BuildCriteria();
        
        Assert.Equal(80, criteria.MaxLength);
    }
    
    [Fact]
    public void PasswordBuilder_Should_IncludeLowercase() {
        var criteria = PasswordBuilder.Create()
            .IncludeLowercase()
            .BuildCriteria();
        
        Assert.True(criteria.IncludeLowerCase);
    }
    
    [Fact]
    public void PasswordBuilder_Should_IncludeUppercase() {
        var criteria = PasswordBuilder.Create()
            .IncludeUppercase()
            .BuildCriteria();
        
        Assert.True(criteria.IncludeUpperCase);
    }
    
    [Fact]
    public void PasswordBuilder_Should_IncludeSpecialChars() {
        var criteria = PasswordBuilder.Create()
            .IncludeSpecialChars()
            .BuildCriteria();
        
        Assert.True(criteria.IncludeSpecial);
    }
    
    [Fact]
    public void PasswordBuilder_Should_IncludeSpacesAndUppercase() {
        var criteria = PasswordBuilder.Create()
            .IncludeUppercase()
            .IncludeSpaces()
            .BuildCriteria();
        
        Assert.True(criteria.IncludeSpaces);
    }
    
    [Fact]
    public void PasswordBuilder_Should_IncludeBrackets() {
        var criteria = PasswordBuilder.Create()
            .IncludeBrackets()
            .BuildCriteria();
        
        Assert.True(criteria.IncludeBrackets);
    }
    
    [Fact]
    public void PasswordBuilder_Should_IncludeNumeric() {
        var criteria = PasswordBuilder.Create()
            .IncludeNumeric()
            .BuildCriteria();
        
        Assert.True(criteria.IncludeNumeric);
    }
    
    [Fact]
    public void PasswordBuilder_Should_IncludeAll() {
        var criteria = PasswordBuilder.Create()
            .IncludeUppercase()
            .IncludeLowercase()
            .IncludeSpaces()
            .IncludeSpecialChars()
            .IncludeBrackets()
            .IncludeNumeric()
            .SetMinLength(10)
            .SetMaxLength(100)
            .BuildCriteria();
        
        Assert.True(criteria.IncludeUpperCase);
        Assert.True(criteria.IncludeLowerCase);
        Assert.True(criteria.IncludeSpaces);
        Assert.True(criteria.IncludeSpecial);
        Assert.True(criteria.IncludeBrackets);
        Assert.True(criteria.IncludeNumeric);
        Assert.Equal(10, criteria.MinLength);
        Assert.Equal(100, criteria.MaxLength);
    }

    [Fact]
    public void PasswordBuilder_Should_BuildPassword() {
        var generator = Substitute.For<IPasswordGeneratorService>();
        generator.GeneratePasswordWith(Arg.Any<PasswordGeneratorCriteria>()).Returns("GeneratedPassword");
        var password = PasswordBuilder.Create(generator)
            .IncludeUppercase()
            .BuildPassword();
        
        Assert.Equal("GeneratedPassword", password);
    }
}