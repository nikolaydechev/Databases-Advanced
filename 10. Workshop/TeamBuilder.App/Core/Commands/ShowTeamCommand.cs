namespace TeamBuilder.App.Core.Commands
{
    using System;
    using System.Linq;
    using System.Text;
    using TeamBuilder.App.Core.Commands.Contracts;
    using TeamBuilder.App.Utilities;
    using TeamBuilder.Data;

    public class ShowTeamCommand : ICommand
    {
        public string Execute(string[] inputArgs)
        {
            //•	ShowTeam<teamName>

            string teamName = inputArgs[0];
            var sb = new StringBuilder();

            using (var context = new TeamBuilderContext())
            {
                var team = context.Teams.FirstOrDefault(t => t.Name == teamName);

                if (team == null)
                {
                    throw new ArgumentException(Constants.ErrorMessages.TeamNotFound, teamName);
                }

                sb.AppendLine($"{teamName} {team.Acronym}");
                sb.AppendLine($"Members:");

                var members = context.Teams
                    .Where(t=>t.Id == team.Id)
                    .SelectMany(t => t.Members)
                    .Select(t=>"--" + t.User.Username)
                    .ToArray();

                sb.AppendLine(string.Join(Environment.NewLine, members));
            }

            return sb.ToString().Trim();
        }
    }
}
