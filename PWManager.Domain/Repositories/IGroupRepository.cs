using PWManager.Domain.Entities;

namespace PWManager.Domain.Repositories {
    internal interface IGroupRepository {
        public Group GetGroup(string groupName);
        public List<string> GetAllGroupNames();
        public bool AddGroup(Group group);
        public bool UpdateGroup(Group group);
        public bool RemoveGroup(string groupName);
    }
}
