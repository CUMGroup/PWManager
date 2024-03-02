﻿using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using PWManager.Application.Context;
using PWManager.Application.Exceptions;
using PWManager.CLI.Abstractions;
using PWManager.CLI.Attributes;
using PWManager.CLI.Enums;
using PWManager.CLI.Interfaces;
using PWManager.CLI.Parser;

namespace PWManager.CLI {
    internal class ConsoleRunner : IRunner {

        private readonly Type?[] _controller = new Type[Enum.GetNames<AvailableCommands>().Length];
        private readonly IServiceProvider _provider;
        private readonly ICliEnvironment _environment;
        private readonly IDebugEnvironment _debugInfo;
        private readonly CommandParser _commandParser;

        public ConsoleRunner(IServiceProvider provider, ICliEnvironment environment, IDebugEnvironment debugInfo) {
            _provider = provider;
            _environment = environment;
            _debugInfo = debugInfo;
            _commandParser = new CommandParser();
        }

        public void Run(string[] args) {
            Console.WriteLine("Hello World");
            try {
                var exitCondition = ExecuteCommand(args);
                while (exitCondition is ExitCondition.CONTINUE) {
                    Console.Write(_environment.Prompt);
                    var input = Console.ReadLine();
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
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine("An Error occured!");
                if (_debugInfo.IsDevelopmentMode) {
                    Console.WriteLine(ex);
                }
            }
        }

        private ExitCondition ExecuteCommand(string input) {
            return ExecuteCommand(_commandParser.ParseCommandWithArguments(input).ToArray());
        }
        private ExitCondition ExecuteCommand(string[] args) {
            var cmd = _commandParser.ParseCommand(args[0]);
            var controllerType = _controller[(int) cmd];
            if (controllerType is null || IsSessionLocked(controllerType)) {
                controllerType = _controller[(int)AvailableCommands.HELP]!;
            }

            var controller = (IController) _provider.GetRequiredService(controllerType);
            return controller.Handle(args[1..]);
        }

        public void MapCommand<TCommand>(AvailableCommands command) {
            if (!typeof(IController).IsAssignableFrom(typeof(TCommand))) {
                throw new ArgumentException("Controller type does not implement IController interface");
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
