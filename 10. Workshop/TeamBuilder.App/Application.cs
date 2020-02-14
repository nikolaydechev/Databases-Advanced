namespace TeamBuilder.App
{
    using System;
    using TeamBuilder.App.Core;
    using TeamBuilder.Data;

    public class Application
    {
        public static void Main()
        {
            //Console.WriteLine(ResetDatabase());

            var engine = new Engine(new CommandDispatcher());
            engine.Run();
        }

        private static string ResetDatabase()
        {
            using (var context = new TeamBuilderContext())
            {
                context.Database.EnsureDeleted();

                context.Database.EnsureCreated();
            }

            return $"Database was successfully reset.";
        }
    }
}
