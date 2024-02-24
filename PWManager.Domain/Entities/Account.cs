using PWManager.Domain.Common;
using PWManager.Domain.Exceptions;
using PWManager.Domain.Services.Interfaces;

namespace PWManager.Domain.Entities {
    public class Account : Entity, ISecureProperties {
        public string LoginName { get; set; }
        public string Password { get; set; }
    
        public Account(string loginName, string password) : base() { 
            LoginName = loginName;
            Password = password;
        }

        public Account(string id, DateTimeOffset created, DateTimeOffset updated, string loginName, string password) : base(id, created, updated) {
            LoginName = loginName;
            Password = password;
        }

        public void SecureProperties(Dictionary<string, string> securedData) {
            if(!(securedData.Count == 2)) {
                throw new SecurePropertiesException("List count does not match Properties count");
            }

        }

        public Dictionary<string, string> GetProperties() {
            return new Dictionary<string, string> { { "LoginName", LoginName }, { "Password", Password } };
        }
    }
}
