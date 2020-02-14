namespace Employees.App
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Employees.App.Commands.Contracts;

    public class CommandParser
    {
        public ICommand ParseCommand(IServiceProvider serviceProvider, string commandName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var commandTypes = assembly.GetTypes()
                .Where(t => t.GetInterfaces().Contains(typeof(ICommand)))
                .ToArray();

            var commandType = commandTypes
                .SingleOrDefault(t => t.Name == $"{commandName}Command");

            if (commandType == null)
            {
                throw new InvalidOperationException("Invalid command!");
            }

            var command = this.InjectServices(serviceProvider, commandType);

            return command;
        }

        private ICommand InjectServices(IServiceProvider serviceProvider, Type type)
        {
            var constructor = type.GetConstructors().First();

            var constructorParameters = constructor
                .GetParameters()
                .Select(pi => pi.ParameterType)
                .ToArray();

            var constructorArgs = constructorParameters
                .Select(s => serviceProvider.GetService(s))
                .ToArray();

            var command = (ICommand)constructor.Invoke(constructorArgs);

            return command;
        }
    }
}
