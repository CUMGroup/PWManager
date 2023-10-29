using PWManager.Domain.ValueObjects;

namespace PWManager.Domain.Services.Interfaces {
    public interface IPasswordGeneratorService {
        public string GeneratePasswordWith(PasswordGeneratorCriteria criteria);
        public string GeneratePassword();
    }
}
