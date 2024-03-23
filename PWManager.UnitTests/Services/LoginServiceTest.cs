using NSubstitute;
using NSubstitute.ExceptionExtensions;
using PWManager.Application.Abstractions.Interfaces;
using PWManager.Application.Context;
using PWManager.Application.Exceptions;
using PWManager.Data.Services;
using PWManager.Domain.Entities;
using PWManager.Domain.Repositories;
using PWManager.Domain.Services.Interfaces;
using PWManager.Domain.ValueObjects;

namespace PWManager.UnitTests.Services {
    public class LoginServiceTest {

        private LoginService _sut;
        private IGroupRepository _groupRepo = Substitute.For<IGroupRepository>();
        private ICryptService _cryptService = Substitute.For<ICryptService>();
        private ISettingsRepository _settingsRepository = Substitute.For<ISettingsRepository>();
        private IDataContextInitializer _dataContextInitializer = Substitute.For<IDataContextInitializer>();
        private ICliEnvironment _cliEnv = Substitute.For<ICliEnvironment>();
        private IUserEnvironment _userEnv = Substitute.For<IUserEnvironment>();
        private ICryptEnvironment _cryptEnv = Substitute.For<ICryptEnvironment>();
        private IUserRepository _userRepo = Substitute.For<IUserRepository>();

        [Fact]
        public void Login_Should_SetEnvironment() {
            SetupDatabaseExists(true);
            var user = SetupPasswordCheckReturnsUser();
            var group = SetupGroupRepoReturnsGroup(user.Id);
            SetupSettingsRepo(user.Id, group.Identifier);
            
            _sut = new LoginService(_userRepo, _groupRepo, _cryptService, _settingsRepository, _cliEnv, _userEnv, _cryptEnv, _dataContextInitializer);
            
            _sut.Login("TestUserName", "WhatAPasswort", ".");

            Assert.Equal(user, _userEnv.CurrentUser);
            Assert.NotNull(_cryptEnv.EncryptionKey);
            Assert.NotNull(_userEnv.CurrentGroup);
            Assert.True(_cliEnv.RunningSession);
        }

        [Fact]
        public void Login_ShouldNot_IfDatabaseDoesntExists() {
            SetupDatabaseExists(false);
            SetupPasswordCheckReturnsUser();
            
            _sut = new LoginService(_userRepo, _groupRepo, _cryptService, _settingsRepository, _cliEnv, _userEnv, _cryptEnv, _dataContextInitializer);

            var ex = Assert.Throws<UserFeedbackException>(() => _sut.Login("TestUserName", "WhatAPassword", "."));
            
            Assert.Equal("Database not found.", ex.Message);
        }

        [Fact]
        public void Login_ShouldNot_IfUserNotFound() {
            SetupDatabaseExists(true);
            SetupPasswordCheckThrowsException();
            
            _sut = new LoginService(_userRepo, _groupRepo, _cryptService, _settingsRepository, _cliEnv, _userEnv, _cryptEnv, _dataContextInitializer);

            Assert.Throws<UserFeedbackException>(() => _sut.Login("TestUserName", "WhatAPassword", "."));
        }


        private void SetupDatabaseExists(bool exists) {
            _dataContextInitializer.DatabaseExists(Arg.Any<string>()).Returns(exists);
        }

        private User SetupPasswordCheckReturnsUser() {
            var user = new User(Guid.NewGuid().ToString(), DateTimeOffset.Now, DateTimeOffset.Now, "TestUserName");
            _userRepo.CheckPasswordAttempt(Arg.Any<string>(), Arg.Any<string>()).Returns(user);
            return user;
        }

        private void SetupPasswordCheckThrowsException() {
            _userRepo.CheckPasswordAttempt(Arg.Any<string>(), Arg.Any<string>()).Throws<UserFeedbackException>();
        }

        private void SetupSettingsRepo(string userId, string mainGroup) {
            _settingsRepository.GetSettings().Returns(
                new Settings(userId, null, null, new MainGroupSetting(mainGroup))
            );
        }

        private Group SetupGroupRepoReturnsGroup(string userId) {
            var group = new Group("TestGroup", userId);
            _groupRepo.GetGroup(Arg.Any<string>()).Returns(group);
            return group;
        }
    }
}
