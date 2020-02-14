namespace _03._MinionNames
{
    using System;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using MinionNames;

    public class Program
    {
        public static void Main()
        {
            // 03. MinionNames
            //string queryAddress = @"C:\Users\Nikolai\source\repos\DB Apps Introduction\03. MinionNames\query.txt";

            var builder = new SqlConnectionStringBuilder()
            {
                ["Server"] = "DESKTOP-KKTNDOL\\SQLEXPRESS",
                ["Integrated Security"] = true,
                ["Database"] = "MinionsDB"
            };

            var connection = new SqlConnection(builder.ToString());

            connection.Open();

            var addMinion = new AddMinion();
            using (connection)
            {
                try
                {
                    addMinion.Run(connection);
                }
                catch (Exception e)
                {

                    Console.WriteLine(e.Message);
                }

            }

            //int villainId = int.Parse(Console.ReadLine());

            //connection.Open();

            //using (connection)
            //{
            //    try
            //    {
            //        string villainNameQuery = $"SELECT Name FROM Villains WHERE Id = {villainId}";
            //        var villainNameCommand = new SqlCommand(villainNameQuery, connection);
            //        string villainName = (string)villainNameCommand.ExecuteScalar();

            //        Console.WriteLine(villainName == null
            //            ? $"No villain with ID {villainId} exists in the database."
            //            : $"Villain: {villainName}");

            //        string minionsCountQuery = File.ReadLines(queryAddress).Skip(9).Take(1).First();
            //        var minionCountCommand = new SqlCommand(minionsCountQuery, connection);
            //        minionCountCommand.Parameters.AddWithValue("@villainId", villainId);
            //        int minionsCount = (int)minionCountCommand.ExecuteScalar();

            //        if (minionsCount == 0)
            //        {
            //            Console.WriteLine("(no minions)");
            //            return;
            //        }

            //        int count = 1;
            //        string minionsQuery = string.Join(Environment.NewLine, File.ReadLines(queryAddress).Take(8).ToArray());
            //        var minionsCommand = new SqlCommand(minionsQuery, connection);
            //        minionsCommand.Parameters.AddWithValue("villainId", villainId);
            //        var reader = minionsCommand.ExecuteReader();

            //        while (reader.Read())
            //        {
            //            Console.WriteLine($"{count++}. {ToTitleCase(reader["MinionName"].ToString())} {reader["MinionAge"]}");
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine(e.Message);
            //    }
            //}
        }

        public static string ToTitleCase(string str)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
        }
    }
}
