
using PWManager.CLI.Interfaces;

namespace PWManager.CLI {
    internal class ConsoleRunner : IRunner {

        public ConsoleRunner() { }

        public void Run(string[] args) {
            Console.WriteLine("Hello World");
        }
    }
}
