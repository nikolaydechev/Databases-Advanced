namespace TeamBuilder.App.Core.Commands
{
    using System;
    using TeamBuilder.App.Core.Commands.Contracts;
    using TeamBuilder.App.Utilities;

    public class ExitCommand : ICommand
    {
        public string Execute(string[] inputArgs)
        {
            Check.CheckLength(0, inputArgs);

            Console.WriteLine("Bye!");

            Environment.Exit(0);

            return string.Empty;
        }
    }
}
