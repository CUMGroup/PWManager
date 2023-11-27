using PWManager.CLI.Enums;
using PWManager.CLI.Parser;

namespace PWManager.UnitTests.Cli; 

public class CommandParserTest {

    private CommandParser _sut;

    public CommandParserTest() {
        _sut = new CommandParser();
    }
    
    [Fact]
    public void ArgParser_Should_ParseSingleArgument() {
        var res = _sut.ParseCommandWithArguments("singleArgument").ToArray();
        Assert.Single(res);
        Assert.Equal("singleArgument", res.First());
    }
    
    [Fact]
    public void ArgParser_Should_ParseWithSpaces() {
        var res = _sut.ParseCommandWithArguments("firstArg  secondArg ").ToArray();
        Assert.Equal(2, res.Length);
        Assert.Equal("firstArg", res[0]);
        Assert.Equal("secondArg", res[1]);
    }
    
    [Fact]
    public void ArgParser_Should_ParseWithQuotes() {
        var res = _sut.ParseCommandWithArguments("firstArg  \"secondArg\" ").ToArray();
        Assert.Equal(2, res.Length);
        Assert.Equal("firstArg", res[0]);
        Assert.Equal("secondArg", res[1]);
    }
    
    [Fact]
    public void ArgParser_Should_ParseWithSingleQuotes() {
        var res = _sut.ParseCommandWithArguments("firstArg  'secondArg' ").ToArray();
        Assert.Equal(2, res.Length);
        Assert.Equal("firstArg", res[0]);
        Assert.Equal("secondArg", res[1]);
    }
    
    [Fact]
    public void ArgParser_Should_ParseWithSpacesInQuotes() {
        var res = _sut.ParseCommandWithArguments("firstArg \"second arg with spaces\" thirdArg").ToArray();
        Assert.Equal(3, res.Length);
        Assert.Equal("firstArg", res[0]);
        Assert.Equal("second arg with spaces", res[1]);
        Assert.Equal("thirdArg", res[2]);
    }

    [Fact]
    public void CommandParser_Should_ParseExistingEnums() {
        var res = _sut.ParseCommand("IniT");
        Assert.Equal(AvailableCommands.INIT, res);
    }
    
    [Fact]
    public void CommandParser_Should_DefaultToHelpForNonExistingCommands() {
        var res = _sut.ParseCommand("wambo");
        Assert.Equal(AvailableCommands.HELP, res);
    }
}