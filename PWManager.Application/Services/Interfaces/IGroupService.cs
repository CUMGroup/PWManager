using PWManager.Domain.Entities;

namespace PWManager.Application.Services.Interfaces {
    public interface IGroupService {
        public List<string> GetAllGroupNames();
        public void AddGroup(string userID, string identifier);
        public void DeleteGroup(string identifier);
    }
}
