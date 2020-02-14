namespace P03_SalesDatabase
{
    using P03_SalesDatabase.Data;

    public class Program
    {
        public static void Main()
        {
            using (var dbContext = new SalesContext())
            {
                dbContext.Database.EnsureCreated();
            }
        }
    }
}
