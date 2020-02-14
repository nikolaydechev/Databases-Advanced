namespace MinionNames
{
    using System;
    using System.Data.SqlClient;

    public class AddMinion
    {
        private const string CheckIfTownExistsQuery = "SELECT Name FROM Towns WHERE Name = @name";
        private const string MinionId = "SELECT Id FROM Minions WHERE Name = @name";
        private const string VillainId = "SELECT Id FROM Villains WHERE Name = @name";

        private const string AddInMinionsVillains =
            "INSERT INTO MinionsVillains(MinionId, VillainId) VALUES (@minionId, @villainId)";
        private const string AddNewMinion = "INSERT INTO Minions(Name, Age, TownId) VALUES (@name, @age, NULL)";
        private const string AddTownQuery = "INSERT INTO Towns(Name, CountryId) VALUES (@name, NULL)";
        private const string CheckIfVillainExists = "SELECT Name FROM Villains WHERE Name = @name";
        private const string AddVillainQuery = "INSERT INTO Villains(Name, EvilnessFactorId) VALUES (@name, 4)";

        public void Run(SqlConnection connection)
        {
            var minionInfo = Console.ReadLine().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var villainInfo = Console.ReadLine().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var town = minionInfo[3];
            this.AddTownIfDoesntExists(connection, town);

            var villain = villainInfo[1];
            this.AddNewVillain(connection, villain);

            var cmd = new SqlCommand(AddNewMinion, connection);
            cmd.Parameters.AddWithValue("@name", minionInfo[1]);
            cmd.Parameters.AddWithValue("@age", int.Parse(minionInfo[2]));
            cmd.ExecuteNonQuery();

            var cmd1 = new SqlCommand(MinionId, connection);
            cmd1.Parameters.AddWithValue("@name", minionInfo[1]);
            int minionId = (int)cmd1.ExecuteScalar();

            var cmd2 = new SqlCommand(VillainId, connection);
            cmd2.Parameters.AddWithValue("@name", villain);
            int villainId = (int)cmd2.ExecuteScalar();

            var cmd3 = new SqlCommand(AddInMinionsVillains, connection);
            cmd3.Parameters.AddWithValue("@minionId", minionId);
            cmd3.Parameters.AddWithValue("@villainId", villainId);
            cmd3.ExecuteNonQuery();
            Console.WriteLine($"Successfully added {minionInfo[1]} to be minion of {villain}.");
        }

        private void AddNewVillain(SqlConnection connection, string villain)
        {
            var cmd = new SqlCommand(CheckIfVillainExists, connection);
            cmd.Parameters.AddWithValue("@name", villain);

            if ((string)cmd.ExecuteScalar() == null)
            {
                this.ExecuteCommand(connection,AddVillainQuery, villain);
                Console.WriteLine($"Villain {villain} was added to the database.");
            }
        }

        private void AddTownIfDoesntExists(SqlConnection connection, string town)
        {
            var cmd1 = new SqlCommand(CheckIfTownExistsQuery, connection);
            cmd1.Parameters.AddWithValue("@name", town);

            if ((string)cmd1.ExecuteScalar() == null)
            {
                this.ExecuteCommand(connection,AddTownQuery, town);
                Console.WriteLine($"Town {town} was added to the database.");
            }
        }

        private void ExecuteCommand(SqlConnection connection,string query, string parameter)
        {
            var cmd1 = new SqlCommand(query, connection);
            cmd1.Parameters.AddWithValue("@name", parameter);
            cmd1.ExecuteNonQuery();
        }
    }
}
