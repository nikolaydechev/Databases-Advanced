namespace _02._VillainNames
{
    using System;
    using System.Data.SqlClient;
    using System.IO;

    public class VillainNames
    {
        public static void Main()
        {
            var builder = new SqlConnectionStringBuilder()
            {
                ["Server"] = "DESKTOP-KKTNDOL\\SQLEXPRESS",
                ["Integrated Security"] = true,
                ["Database"] = "MinionsDB"
            };

            var connection = new SqlConnection(builder.ToString());
            connection.Open();

            using (connection)
            {
                try
                {
                    string query = File.ReadAllText(@"../../query.txt");

                    var command = new SqlCommand(query, connection);

                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        var name = reader["Name"];
                        var minionsCount = reader["MinionsCount"];

                        Console.WriteLine(name + " - " + minionsCount);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
