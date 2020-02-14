namespace PhotoShare.Client.Core
{
    using System;
    using System.Linq;
    using System.Reflection;
    using PhotoShare.Client.Core.Commands.Contracts;

    public class CommandDispatcher
    {
        private readonly Session session;

        public CommandDispatcher(Session session)
        {
            this.session = session;
        }

        public string DispatchCommand(string[] commandParameters)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var commandTypes = assembly.GetTypes()
                .Where(t => t.GetInterfaces().Contains(typeof(ICommand)))
                .ToArray();

            var commandType = commandTypes
                .SingleOrDefault(t => t.Name == $"{commandParameters[0]}Command");

            if (commandType == null)
            {
                throw new InvalidOperationException($"Command {commandParameters[0]} not valid!");
            }

            //var constructor = commandType.GetConstructors().First();

            var command = (ICommand)Activator.CreateInstance(commandType);

            return command.Execute(this.session, commandParameters);
        }
    }
}
