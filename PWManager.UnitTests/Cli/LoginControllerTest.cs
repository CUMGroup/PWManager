using NSubstitute;
using PWManager.CLI.Controllers;

namespace PWManager.UnitTests.Cli {
    public class LoginControllerTest {

        private LoginController _sut;

        public LoginControllerTest()
        {
            _sut = Substitute.ForPartsOf<LoginController>(new object[] {null});
            _sut.AskForInput(Arg.Any<string>()).Returns("Test");
        }

        [Fact]
        public void Arguments_ShouldBe_Empty_WithoutArgs() { 
            string[] args = { };

            (var username, var path) = _sut.ParseArgs(args);

            Assert.Equal(0, username.Length);
            Assert.Equal(0, path.Length);
        }

        [Fact]
        public void Arguments_Should_Return_Path() {
            string[] args = { "-d", "TestPath" }; 

            (var username, var path) = _sut.ParseArgs(args);

            Assert.Equal(0, username.Length);
            Assert.Equal("TestPath", path);
        }

        [Fact]
        public void Arguments_Should_Return_Username() {
            string[] args = { "-u", "TestUserName" };

            (var username, var path) = _sut.ParseArgs(args);

            Assert.Equal("TestUserName", username);
            Assert.Equal(0, path.Length);
        }


        [Fact]
        public void Arguments_Should_Return_Path_And_PromptName() {
            string[] args = { "-d", "TestPath", "-u" };

            (var username, var path) = _sut.ParseArgs(args);

            Assert.Equal("Test", username);
            Assert.Equal("TestPath", path);
        }

        [Fact]
        public void Arguments_Should_Return_Username_And_PromptPath() {
            string[] args = { "-d", "-u", "TestUserName" };

            (var username, var path) = _sut.ParseArgs(args);

            Assert.Equal("TestUserName", username);
            Assert.Equal("Test", path);
        }

        [Fact]
        public void Arguments_Should_Return_From_Prompts() {
            string[] args = { "-d", "-u" };

            (var username, var path) = _sut.ParseArgs(args);

            Assert.Equal("Test", username);
            Assert.Equal("Test", path);
        }
    }
}
