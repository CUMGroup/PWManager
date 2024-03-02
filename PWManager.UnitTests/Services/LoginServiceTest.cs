using NSubstitute;
using NSubstitute.ReturnsExtensions;
using PWManager.Application.Context;
using PWManager.Application.Exceptions;
using PWManager.Data.Abstraction;
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
        private DataContextWrapper _wrapper = Substitute.For<DataContextWrapper>();
        private IUserEnvironment _userEnv = Substitute.For<IUserEnvironment>();
        private ICryptEnvironment _cryptEnv = Substitute.For<ICryptEnvironment>();
        private IUserRepository _userRepo = Substitute.For<IUserRepository>();

        [Fact]
        public void Login_Should_SetEnviroment() {
            _wrapper.DatabaseExists(Arg.Any<string>()).Returns(true);
            var user = new User(Guid.NewGuid().ToString(), DateTimeOffset.Now, DateTimeOffset.Now, "TestUserName");
            _userRepo.CheckPasswordAttempt(Arg.Any<string>(), Arg.Any<string>()).Returns(user);
            var group = new Group("TestGroup", user.Id);
            _settingsRepository.GetSettings().Returns(
                    new Settings(user.Id, null, null, new MainGroupSetting(group.Identifier))
                );
            _groupRepo.GetGroup(Arg.Any<string>()).Returns(group);

            _sut = new LoginService(_wrapper, _userRepo, _groupRepo, _cryptService, _settingsRepository, _userEnv, _cryptEnv);
            _sut.Login("TestUserName", "WhatAPasswort", ".");

            Assert.Equal(user, _userEnv.CurrentUser);
            Assert.NotNull(_cryptEnv.EncryptionKey);
            Assert.NotNull(_userEnv.CurrentGroup);
        }

        [Fact]
        public void Login_ShouldNot_IfDatabaseDoesntExists() {
            _wrapper.DatabaseExists(Arg.Any<string>()).Returns(false);
            var user = new User(Guid.NewGuid().ToString(), DateTimeOffset.Now, DateTimeOffset.Now, "TestUserName");
            _userRepo.CheckPasswordAttempt(Arg.Any<string>(), Arg.Any<string>()).Returns(user);


            _sut = new LoginService(_wrapper, _userRepo, _groupRepo, _cryptService, _settingsRepository, _userEnv, _cryptEnv);

            var ex = Assert.Throws<UserFeedbackException>(() => _sut.Login("TestUserName", "WhatAPassword", "."));
            Assert.Equal("Database not found.", ex.Message);
        }

        [Fact]
        public void Login_ShouldNot_IfUserNotFound() {
            _wrapper.DatabaseExists(Arg.Any<string>()).Returns(true);
            var user = new User(Guid.NewGuid().ToString(), DateTimeOffset.Now, DateTimeOffset.Now, "TestUserName");
            _userRepo.CheckPasswordAttempt(Arg.Any<string>(), Arg.Any<string>()).ReturnsNull();


            _sut = new LoginService(_wrapper, _userRepo, _groupRepo, _cryptService, _settingsRepository, _userEnv, _cryptEnv);

            var ex = Assert.Throws<UserFeedbackException>(() => _sut.Login("TestUserName", "WhatAPassword", "."));
            Assert.Equal("No such user found.", ex.Message);
        }

    }
}
