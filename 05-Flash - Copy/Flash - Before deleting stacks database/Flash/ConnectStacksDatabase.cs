using System.Data.SqlClient;

namespace Flash;

internal class ConnectStacksDatabase
{
    internal static void CreateStacksDatabase(string connectionString)
    {

        //create stacks database
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string checkDatabaseQuery =
                    @"SELECT COUNT(*) 
                    FROM sys.databases 
                    WHERE name = 'DataBaseStacks'";

                using (SqlCommand checkDatabaseQueryCommand = new SqlCommand(checkDatabaseQuery, connection))
                {
                    int databaseCount = Convert.ToInt32(checkDatabaseQueryCommand.ExecuteScalar());
                    if (databaseCount == 0)
                    {
                        string createDatabaseQuery =
                            @"CREATE DATABASE DataBaseStacks";

                        using (SqlCommand createDatabaseCommand = new SqlCommand(createDatabaseQuery, connection))
                        {
                            createDatabaseCommand.ExecuteNonQuery();
                            Console.WriteLine("Database 'DataBaseStacks' created successfully.");
                        }
                    }

                    else
                    {
                        Console.WriteLine("Database 'DataBaseStacks' created already.");
                    }
                }


            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }    
}
