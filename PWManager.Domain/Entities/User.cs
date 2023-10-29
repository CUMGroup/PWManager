using PWManager.Domain.Common;

namespace PWManager.Domain.Entities {
    public class User : Entity {
        public string MasterPassword { get; init; }
        public User(string masterHash) : base(){
            MasterPassword = masterHash;
        }

        public User(string id, DateTimeOffset created, DateTimeOffset updated, string masterHash) : base(id, created, updated){
            MasterPassword = masterHash;
        }
    }
}
