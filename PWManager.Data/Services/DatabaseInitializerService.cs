using System.Text.RegularExpressions;
using PWManager.Application.Abstractions.Interfaces;
using PWManager.Application.Context;
using PWManager.Application.Exceptions;
using PWManager.Application.Services.Interfaces;
using PWManager.Data.Abstraction;
using PWManager.Domain.Repositories;
using PWManager.Domain.Services.Interfaces;
using Group = PWManager.Domain.Entities.Group;

namespace PWManager.Data.Services; 

internal class DatabaseInitializerService : IDatabaseInitializerService {

    private readonly IUserRepository _userRepo;
    private readonly IGroupRepository _groupRepository;
    private readonly IUserEnvironment _userEnvironment;
    private readonly ICryptEnvironment _cryptEnvironment;
    private readonly ICryptService _cryptService;
    private readonly IDataContextInitializer _dataContextInitializer;

    public DatabaseInitializerService(IUserRepository userRepo, IGroupRepository groupRepository, IUserEnvironment environment, ICryptService cryptService, ICryptEnvironment cryptEnvironment, IDataContextInitializer dataContextInitializer) {
        _userRepo = userRepo;
        _groupRepository = groupRepository;
        _userEnvironment = environment;
        _cryptService = cryptService;
        _cryptEnvironment = cryptEnvironment;
        _dataContextInitializer = dataContextInitializer;
    }
    
    public void InitDatabase(string path, string username, string password) {
        CheckIfDataBaseExistsOnPath(path);

        if (username.Length < 1 || !Regex.IsMatch(username, @"^[a-zA-Z]+$")) {
            throw new UserFeedbackException("Invalid Username! Only letters are allowed!");
        }
        
        _dataContextInitializer.InitDataContext(path);
        
        var user = _userRepo.AddUser(username, password);
        _userEnvironment.CurrentUser = user;
        _cryptEnvironment.EncryptionKey = _cryptService.DeriveKeyFrom(password, username);
        
        _groupRepository.AddGroup(new Group("main", user.Id));
    }

    public void CheckIfDataBaseExistsOnPath(string path) {
        if (_dataContextInitializer.DatabaseExists(path)) {
            throw new UserFeedbackException(
                "Database initialization failed! The database already exists at the specified path!");
        }
    }
}