using NSubstitute;
using PWManager.Application.Context;
using PWManager.Application.Services;
using PWManager.Domain.Entities;
using PWManager.Domain.Repositories;
using PWManager.Domain.ValueObjects;

namespace PWManager.UnitTests.Services; 

public class SettingsServiceTest {

    private SettingsService _sut;

    [Fact]
    public void SettingsService_Should_ChangeClipboardTimeout() {
        var env = MockEnvWithSettings();
        var repo = Substitute.For<ISettingsRepository>();
        _sut = new SettingsService(repo,env);

        var time = TimeSpan.FromSeconds(50);
        _sut.ChangeClipboardTimeoutSetting(time);
        
        Assert.Equal(time, env.UserSettings.Timeout.ClipboardTimeOutDuration);
        repo.Received().UpdateSettings(Arg.Any<Settings>());
    }
    
    [Fact]
    public void SettingsService_Should_ChangeAccountTimeout() {
        var env = MockEnvWithSettings();
        var repo = Substitute.For<ISettingsRepository>();
        _sut = new SettingsService(repo,env);

        var time = TimeSpan.FromSeconds(50);
        _sut.ChangeAccountTimeoutSetting(time);
        
        Assert.Equal(time, env.UserSettings.Timeout.AccountTimeOutDuration);
        repo.Received().UpdateSettings(Arg.Any<Settings>());
    }
    
    [Fact]
    public void SettingsService_Should_ChangeMainGroup() {
        var env = MockEnvWithSettings();
        var repo = Substitute.For<ISettingsRepository>();
        _sut = new SettingsService(repo,env);

        var main = "newMainGroup";
        _sut.ChangeMainGroupSetting(new MainGroupSetting(main));
        
        Assert.Equal(main, env.UserSettings.MainGroup.MainGroupIdentifier);
        repo.Received().UpdateSettings(Arg.Any<Settings>());
    }
    
    [Fact]
    public void SettingsService_Should_ChangePasswordGenerator() {
        var env = MockEnvWithSettings();
        var repo = Substitute.For<ISettingsRepository>();
        _sut = new SettingsService(repo,env);

        var pw = new PasswordGeneratorCriteria(false, false, false, true, true, true, 2, 5);
        _sut.ChangePasswordGenerationCriteria(pw);
        
        Assert.Equal(pw, env.UserSettings.PwGenCriteria);
        repo.Received().UpdateSettings(Arg.Any<Settings>());
    }

    [Fact]
    public void SettingsService_Should_GetSettingsFromEnv() {
        var env = MockEnvWithSettings();
        _sut = new SettingsService(null, env);

        var settings = _sut.GetSettings();
        
        Assert.Equal(settings, env.UserSettings);
    }
    
    [Fact]
    public void SettingsService_Should_GetSettingsFromRepo() {
        var env = MockEnvWithSettings();
        var repo = Substitute.For<ISettingsRepository>();
        repo.GetSettings().Returns(env.UserSettings);

        env = Substitute.For<IUserEnvironment>();
        
        _sut = new SettingsService(repo, env);

        var settings = _sut.GetSettings();
        
        Assert.Equal(settings, env.UserSettings);
        Assert.Equal(settings, repo.GetSettings());
    }

    private IUserEnvironment MockEnvWithSettings() {
        var env = Substitute.For<IUserEnvironment>();
        env.UserSettings.Returns(new Settings(
            "userId",
            new PasswordGeneratorCriteria(true, true, true, true, true, true, 5, 10),
            new TimeoutSettings(TimeSpan.FromSeconds(10), TimeSpan.FromMinutes(2)),
            new MainGroupSetting("main")
        ));
        return env;
    }
}