using System.Reflection;
using PWManager.Domain.ValueObjects;

namespace PWManager.UnitTests.Domain.ValueObjects; 

public class PasswordCriteriaTests {

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Criteria_ShouldNot_AcceptInvalidMinLength(int minLength) {
        var exception = Assert.Throws<ArgumentException>(() => {
            var criteria = new PasswordGeneratorCriteria(true, true, true, true, true, true, minLength, 10);
        });
        
        Assert.Equal("MinLength cannot be less than or equal to 0", exception.Message);
    }
    
    [Fact]
    public void Criteria_ShouldNot_AcceptInvalidMaxLength() {
        var exception = Assert.Throws<ArgumentException>(() => {
            var criteria = new PasswordGeneratorCriteria(true, true, true, true, true, true, 10, 5);
        });
        
        Assert.Equal("MaxLength cannot be less than MinLength", exception.Message);
    }

    [Fact]
    public void Criteria_Should_AcceptEqualMinMaxLength() {
        try {
            var criteria = new PasswordGeneratorCriteria(true, true, true, true, true, true, 5, 5);
        }
        catch (Exception ex) {
            Assert.Fail("Exception was thrown when Min and MaxLength were equal " + ex.Message);
        }
    }

    [Fact]
    public void Criteria_ShouldNot_AcceptNoIncludeSelected() {
        var exception = Assert.Throws<ArgumentException>(() => {
            var criteria = new PasswordGeneratorCriteria(false,false,false,false,false,false,5,5);
        });
        
        Assert.Equal("Password Generator must have some characters enabled!", exception.Message);

    }
}