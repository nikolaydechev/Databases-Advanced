namespace Employees.App.Commands
{
    using System;
    using Employees.App.Commands.Contracts;

    public class ExitCommand : ICommand
    {
        public string Execute(string[] data)
        {
            Console.WriteLine($"Goodbye!");

            Environment.Exit(0);

            return string.Empty;
        }
    }
}
