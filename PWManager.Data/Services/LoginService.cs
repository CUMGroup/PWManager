using PWManager.Application.Context;
using PWManager.Application.Services.Interfaces;
using PWManager.Domain.Exceptions;
using PWManager.Domain.Repositories;
using PWManager.Domain.Services.Interfaces;

namespace PWManager.Data.Services {
    public class LoginService : ILoginService {

        private readonly IUserRepository _userRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly ISettingsRepository _settingsRepository;
        private readonly ICryptService _cryptService;
        private readonly IApplicationEnvironment _env;

        public LoginService(IUserRepository userRepository, IGroupRepository groupRepository, ICryptService cryptService, ISettingsRepository settingsRepository, IApplicationEnvironment env) { 
            _userRepository = userRepository;
            _groupRepository = groupRepository;
            _settingsRepository = settingsRepository;
            _cryptService = cryptService;
            _env = env;
        }
        public void Login(string username, string password, string dbPath) {
            // TODO: Check if DB exists
            DataContext.InitDataContext(dbPath);

            var user = _userRepository.CheckPasswordAttempt(username, password);
            if(user is null) {
                throw new LoginException("No such user found."); // TODO: Change with UserFeedbackException
            }

            _env.CurrentUser = user;
            _env.EncryptionKey = _cryptService.DeriveKeyFrom(password, username);

            var mainGroup = _settingsRepository.GetSettings().MainGroup;
            _env.CurrentGroup = _groupRepository.GetGroup(mainGroup.MainGroupIdentifier);
        }
    }
}
