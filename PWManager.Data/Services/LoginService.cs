using PWManager.Application.Abstractions.Interfaces;
using PWManager.Application.Context;
using PWManager.Application.Exceptions;
using PWManager.Application.Services.Interfaces;
using PWManager.Data.Abstraction;
using PWManager.Domain.Exceptions;
using PWManager.Domain.Repositories;
using PWManager.Domain.Services.Interfaces;

namespace PWManager.Data.Services {
    public class LoginService : ILoginService {

        private readonly IUserRepository _userRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly ISettingsRepository _settingsRepository;
        private readonly ICryptService _cryptService;
        private readonly ICliEnvironment _cliEnv;
        private readonly IUserEnvironment _userEnv;
        private readonly ICryptEnvironment _cryptEnv;

        private readonly IDataContextInitializer _dataContextInitializer;

        public LoginService(IUserRepository userRepository, IGroupRepository groupRepository, ICryptService cryptService, ISettingsRepository settingsRepository, ICliEnvironment cliEnv, IUserEnvironment userEnv, ICryptEnvironment cryptEnv, IDataContextInitializer dataContextInitializer) { 
            _userRepository = userRepository;
            _groupRepository = groupRepository;
            _settingsRepository = settingsRepository;
            _cryptEnv = cryptEnv;
            _cryptService = cryptService;
            _userEnv = userEnv;
            _cliEnv = cliEnv;
            _dataContextInitializer = dataContextInitializer;
        }
        public bool Login(string username, string password, string dbPath) {
            if(!_dataContextInitializer.DatabaseExists(dbPath)) {
                throw new UserFeedbackException("Database not found.");
            }

            _dataContextInitializer.InitDataContext(dbPath);

            var user = _userRepository.CheckPasswordAttempt(username, password);
            if(user is null) {
                return false;
            }

            _userEnv.CurrentUser = user;
            _cryptEnv.EncryptionKey = _cryptService.DeriveKeyFrom(password, username);

            var mainGroup = _settingsRepository.GetSettings().MainGroup;

            _cliEnv.RunningSession = true;
            _userEnv.CurrentGroup = _groupRepository.GetGroup(mainGroup.MainGroupIdentifier);

            return true;
        }

        public bool CheckPassword(string username, string password) {
            return _userRepository.CheckPasswordAttempt(username, password) is not null;
        }
    }
}
