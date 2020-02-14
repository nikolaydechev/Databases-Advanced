namespace TeamBuilder.App.Core
{
    using System;
    using System.Linq;
    using System.Reflection;
    using TeamBuilder.App.Core.Commands.Contracts;

    public class CommandDispatcher
    {
        public string Dispatch(string input)
        {
            string result = string.Empty;

            string[] inputArgs = input.Split(new[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);

            string commandName = inputArgs.Length > 0 ? inputArgs[0] : string.Empty;
            inputArgs = inputArgs.Skip(1).ToArray();

            var assembly = Assembly.GetExecutingAssembly();

            var commandTypes = assembly.GetTypes()
                .Where(t => t.GetInterfaces().Contains(typeof(ICommand)))
                .ToArray();

            var commandType = commandTypes
                .SingleOrDefault(t => t.Name == $"{commandName}Command");

            if (commandType == null)
            {
                throw new NotSupportedException($"Command {commandName} not supported!");
            }

            var command = (ICommand)Activator.CreateInstance(commandType);

            result = command.Execute(inputArgs);

            return result;
        }
    }
}
