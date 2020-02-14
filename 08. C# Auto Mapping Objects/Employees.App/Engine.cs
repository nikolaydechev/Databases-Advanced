namespace Employees.App
{
    using System;
    using System.Linq;

    internal class Engine
    {
        private readonly IServiceProvider serviceProvider;
        private readonly CommandParser commandParser;

        public Engine(IServiceProvider serviceProvider, CommandParser commandParser)
        {
            this.serviceProvider = serviceProvider;
            this.commandParser = commandParser;
        }

        public void Run()
        {
            while (true)
            {
                string[] commandInput = Console.ReadLine().Split(' ');

                string commandName = commandInput[0];

                string[] commandArgs = commandInput.Skip(1).ToArray();

                try
                {
                    var command = this.commandParser.ParseCommand(this.serviceProvider, commandName);

                    var result = command.Execute(commandArgs);

                    Console.WriteLine(result);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
