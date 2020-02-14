namespace PhotoShare.Client.Core.Commands
{
    using System;
    using PhotoShare.Client.Core.Commands.Contracts;

    public class ExitCommand : ICommand
    {
        public string Execute(Session session, params string[] data)
        {
            Environment.Exit(0);

            return "Good bye!";
        }
    }
}
