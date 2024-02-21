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
                }

                connection.ChangeDatabase("DataBaseStacks");

                string checkTableQuery =
                    @"SELECT COUNT(*) 
                    FROM INFORMATION_SCHEMA.TABLES 
                    WHERE TABLE_NAME = 'Stacks'";

                using (SqlCommand checkTableCommand = new SqlCommand(checkTableQuery, connection))
                {
                    int tableCount = Convert.ToInt32(checkTableCommand.ExecuteScalar());
                    if (tableCount == 0)
                    {
                        string createTableQuery =
                            @"CREATE TABLE Stacks (
                            Id INT PRIMARY KEY,
                            Front NVARCHAR(50),
                            Back NVARCHAR(50),                            
                            )";
                        using (SqlCommand createTableCommand = new SqlCommand(createTableQuery, connection))
                        {
                            createTableCommand.ExecuteNonQuery();
                            Console.WriteLine("Table 'Stacks' created successfully.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Table 'Stacks' already exists in database 'DataBaseStacks'.");
                    }
                }


                // Generate random values for ID and Name
                Random random = new Random();
                int id = random.Next(1, 2); // Generate random ID between 1 and 1000
                string name = "Name_" + id.ToString(); // Generate a random name based on the ID

                // Check if the ID already exists in testTable
                string checkIdQuery = $"SELECT COUNT(*) FROM Stacks WHERE Id = {id}";
                using (SqlCommand checkIdCommand = new SqlCommand(checkIdQuery, connection))
                {
                    int idCount = Convert.ToInt32(checkIdCommand.ExecuteScalar());
                    if (idCount == 0)
                    {
                        // Insert the record if ID doesn't exist
                        string insertRecordQuery = $"INSERT INTO Stacks (Id, Name) VALUES ({id}, '{name}')";
                        using (SqlCommand insertRecordCommand = new SqlCommand(insertRecordQuery, connection))
                        {
                            insertRecordCommand.ExecuteNonQuery();
                            Console.WriteLine($"Record inserted into 'Stacks': ID = {id}, Name = {name}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Record with ID = {id} already exists in 'Stacks'.");
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
