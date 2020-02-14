namespace HospitalStartUp
{
    using P01_HospitalDatabase.Data;
    using P01_HospitalDatabase.Initializer;

    public class Program
    {
        public static void Main(string[] args)
        {
            using (var dbContext = new HospitalContext())
            {
                //dbContext.Database.EnsureDeleted();
                //dbContext.Database.EnsureCreated();

                DatabaseInitializer.InitialSeed(dbContext);
            }
        }
    }
}
