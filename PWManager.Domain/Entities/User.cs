using PWManager.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWManager.Domain.Entities {
    public class User : Entity {
        public string MasterPassword { get; init; }
        public User(string masterHash)
        {
            MasterPassword = masterHash;
        }
    }
}
