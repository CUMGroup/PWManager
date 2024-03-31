using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using PWManager.Application.Context;
using PWManager.Application.Exceptions;
using PWManager.CLI.Abstractions;
using PWManager.CLI.Attributes;
using PWManager.CLI.Enums;
using PWManager.CLI.Interfaces;
using PWManager.CLI.Parser;
using Sharprompt;

namespace PWManager.CLI {
    internal class ConsoleRunner : IRunner {

        private readonly Type?[] _controller = new Type[Enum.GetNames<AvailableCommands>().Length];
        private readonly IServiceProvider _provider;
        private readonly ICliEnvironment _environment;
        private readonly IDebugEnvironment _debugInfo;
        private readonly ICancelEnvironment _cancelEnvironment;
        private readonly CommandParser _commandParser;

        public ConsoleRunner(IServiceProvider provider, ICliEnvironment environment, IDebugEnvironment debugInfo, ICancelEnvironment cancelEnvironment) {
            _provider = provider;
            _environment = environment;
            _debugInfo = debugInfo;
            _cancelEnvironment = cancelEnvironment;
            _commandParser = new CommandParser();
        }

        public void Run(string[] args) {
            try {
                var exitCondition = ExecuteCommand(args);
                while (exitCondition is ExitCondition.CONTINUE) {
                    _environment.WritePrompt();
                    var input = ConsoleInteraction.ReadLine();
                    if (input is null) {
                        continue;
                    }

                    try {
                        exitCondition = ExecuteCommand(input);
                    }catch (UserFeedbackException ex) {
                        Console.WriteLine(ex.Message);
                        if (_environment.RunningSession) {
                            exitCondition = ExitCondition.CONTINUE;
                            continue;
                        }

                        return;
                    }catch (PromptCanceledException) {/*Prompt cancelling with ctrl+C*/}
                }
            }
            catch(UserFeedbackException ex) {Console.WriteLine(ex.Message);}
            catch (PromptCanceledException) { /* SIGINT handling */ }
            catch (Exception ex) {
                Console.WriteLine(UIstrings.ERROR_OCCURED);
                if (_debugInfo.IsDevelopmentMode) {
                    Console.WriteLine(ex);
                }
            }
        }

        private ExitCondition ExecuteCommand(string input) {
            return ExecuteCommand(_commandParser.ParseCommandWithArguments(input).ToArray());
        }
        private ExitCondition ExecuteCommand(string[] args) {
            _cancelEnvironment.CancelableState = false;
            if (args.Length <= 0 && _environment.RunningSession) {
                _cancelEnvironment.CancelableState = true;
                return ExitCondition.CONTINUE;
            }
            var cmd = args.Length <= 0 ? AvailableCommands.HELP : _commandParser.ParseCommand(args[0]);
            var controllerType = _controller[(int) cmd];
            if (controllerType is null || IsSessionLocked(controllerType)) {
                controllerType = _controller[(int)AvailableCommands.HELP]!;
            }

            var controller = (IController) _provider.GetRequiredService(controllerType);
            var commandArgs = args.Length <= 1 ? Array.Empty<string>() : args[1..];
            
            var exit = controller.Handle(commandArgs);
            _cancelEnvironment.CancelableState = true;
            return exit;
        }

        public void MapCommand<TCommand>(AvailableCommands command) {
            if (!typeof(IController).IsAssignableFrom(typeof(TCommand))) {
                throw new ArgumentException(MessageStrings.CONTROLER_TYPE_ERROR);
            }
            _controller[(int) command] = typeof(TCommand);
        }

        private bool IsSessionLocked(MemberInfo controllerType) {
            var sessionOnly = Attribute.GetCustomAttribute(controllerType, typeof(SessionOnlyAttribute)) is not null 
                   && !_environment.RunningSession;
            
            var noSessionOnly = Attribute.GetCustomAttribute(controllerType, typeof(NoSessionAttribute)) is not null 
                    && _environment.RunningSession;

            return sessionOnly || noSessionOnly;
        }
    }
}
