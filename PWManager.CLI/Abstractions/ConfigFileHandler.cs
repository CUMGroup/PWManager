using PWManager.Application.Exceptions;

namespace PWManager.CLI.Abstractions {
    public class ConfigFileHandler {
        public static string ReadDefaultFile() {
            var defaultFilePath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

            try {
                return File.ReadAllText(Path.Combine(defaultFilePath, "last.txt"));
            } catch (IOException e) {
                throw new UserFeedbackException("The file could not be read");
            }
        }
        public static void WriteDefaultFile(string username, string path) {
            var defaultFilePath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

            try {
                File.WriteAllText(Path.Combine(defaultFilePath, "last.txt"), username + '\n' + path);
            } catch (IOException e) {
                throw new UserFeedbackException("The file could not be written");
            }
        }
    }
}
