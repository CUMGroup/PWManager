using System.Reflection;
using PWManager.Application.Exceptions;

namespace PWManager.CLI.Abstractions {
    public static class ConfigFileHandler {
        public static string ReadDefaultFile() {
            var defaultFilePath = GetPath();

            try {
                return File.ReadAllText(Path.Combine(defaultFilePath, "last.txt"));
            } catch (IOException) {
                throw new UserFeedbackException("The config file could not be read");
            }
        }
        public static void WriteDefaultFile(string username, string path) {
            var defaultFilePath = GetPath();
            path = Path.GetFullPath(path);

            try {
                File.WriteAllText(Path.Combine(defaultFilePath, "last.txt"), username + '\n' + path);
            } catch (IOException) {
                throw new UserFeedbackException("The config file could not be written");
            }
        }

        private static string GetPath() {
            var assembly = Assembly.GetEntryAssembly();
            if (assembly is null) {
                throw new ApplicationException("An unknown error occured! Could not determine execution path!");
            }
            var path = Path.GetDirectoryName(assembly.Location);
            if (path is null) {
                throw new ApplicationException("An unknown error occured! Execution path is not a directory!");
            }

            return path;
        }
    }
}
