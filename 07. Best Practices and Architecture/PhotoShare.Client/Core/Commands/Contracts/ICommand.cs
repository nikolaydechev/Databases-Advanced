namespace PhotoShare.Client.Core.Commands.Contracts
{
    public interface ICommand
    {
        string Execute(Session session, params string[] arguments);
    }
}
