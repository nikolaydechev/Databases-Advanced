namespace P03_FootballBetting.App
{
    using P03_FootballBetting.Data;

    public class Program
    {
        public static void Main()
        {
            using (var dbContext = new FootballBettingContext())
            {
                dbContext.Database.EnsureCreated();
            }
        }
    }
}
