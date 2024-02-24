using System.Text.RegularExpressions;
using PWManager.Application.Context;
using PWManager.Application.Exceptions;
using PWManager.Application.Services.Interfaces;
using PWManager.Domain.Repositories;
using PWManager.Domain.Services.Interfaces;
using Group = PWManager.Domain.Entities.Group;

namespace PWManager.Data.Services; 

internal class DatabaseInitializerService : IDatabaseInitializerService {

    private readonly IUserRepository _userRepo;
    private readonly IGroupRepository _groupRepository;
    private readonly IApplicationEnvironment _environment;
    private readonly ICryptService _cryptService;
    
    public DatabaseInitializerService(IUserRepository userRepo, IGroupRepository groupRepository, IApplicationEnvironment environment, ICryptService cryptService) {
        _userRepo = userRepo;
        _groupRepository = groupRepository;
        _environment = environment;
        _cryptService = cryptService;
    }
    
    public void InitDatabase(string path, string username, string password) {
        if (DataContext.DatabaseExists(path)) {
            throw new UserFeedbackException(
                "Database initialization failed! The database already exists at the specified path!");
        }

        if (username.Length < 1 || !Regex.IsMatch(username, @"^[a-zA-Z]+$")) {
            throw new UserFeedbackException("Invalid Username! Only letters are allowed!");
        }
        
        DataContext.InitDataContext(path);
        
        var user = _userRepo.AddUser(username, password);
        _environment.CurrentUser = user;
        _environment.EncryptionKey = _cryptService.DeriveKeyFrom(password, username);
        
        _groupRepository.AddGroup(new Group("main", user.Id));
    }
}