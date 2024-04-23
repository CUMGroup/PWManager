using System.Reflection;
using PWManager.Application.Exceptions;

namespace PWManager.CLI.Abstractions {
    public static class ConfigFileHandler {

        public static bool DefaultFileExists() {
            return File.Exists(Path.Combine(GetPath(), "last.txt"));
        }
        public static string ReadDefaultFile() {
            var defaultFilePath = GetPath();

            try {
                return File.ReadAllText(Path.Combine(defaultFilePath, "last.txt"));
            } catch (IOException) {
                throw new UserFeedbackException(MessageStrings.READ_FILE_ERROR); 
            }
        }
        public static void WriteDefaultFile(string username, string path) {
            var defaultFilePath = GetPath();
            path = Path.GetFullPath(path);

            try {
                File.WriteAllText(Path.Combine(defaultFilePath, "last.txt"), username + '\n' + path);
            } catch (IOException) {
                throw new UserFeedbackException(MessageStrings.WRITE_FILE_ERROR); 
            }
        }

        public static void DeleteDefaultFile() {
            var defaultFilePath = Path.Combine(GetPath(), "last.txt");
            try {
                File.Delete(defaultFilePath);
            }
            catch (IOException) {
                throw new UserFeedbackException(MessageStrings.DELETE_FILE_ERROR);
            }
        }

        private static string GetPath() {
            var assembly = Assembly.GetEntryAssembly();
            if (assembly is null) {
                throw new ApplicationException(MessageStrings.PATH_ERROR); 
            }
            var path = Path.GetDirectoryName(assembly.Location);
            if (path is null) {
                throw new ApplicationException(MessageStrings.DIRECTORY_ERROR); 
            }

            return path;
        }
    }
}
