using System.Data.SqlClient;

namespace Flash;

internal class ConnectFlashCardDatabase
{
    internal static void CreateFlashCardDatabase(string connectionString)
    {
        //create Flashcard database 
        try
        {

            //create flashcards database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string checkDatabaseQuery =
                    @"SELECT COUNT(*) 
                    FROM sys.databases 
                    WHERE name = 'DataBaseFlashCard'";

                using (SqlCommand checkDatabaseQueryCommand = new SqlCommand(checkDatabaseQuery, connection))
                {
                    int databaseCount = Convert.ToInt32(checkDatabaseQueryCommand.ExecuteScalar());
                    if (databaseCount == 0)
                    {
                        string createDatabaseQuery =
                            @"CREATE DATABASE DataBaseFlashCard";

                        using (SqlCommand createDatabaseCommand = new SqlCommand(createDatabaseQuery, connection))
                        {
                            createDatabaseCommand.ExecuteNonQuery();
                            Console.WriteLine("Database 'DataBaseFlashCard' created successfully.");
                        }
                    }
                }

                connection.ChangeDatabase("DataBaseFlashCard");

                string checkTableQuery =
                    @"SELECT COUNT(*) 
                    FROM INFORMATION_SCHEMA.TABLES 
                    WHERE TABLE_NAME = 'Flahcards'";


                using (SqlCommand checkTableCommand = new SqlCommand(checkTableQuery, connection))
                {
                    int tableCount = Convert.ToInt32(checkTableCommand.ExecuteScalar());
                    if (tableCount == 0)
                    {
                        string createTableQuery =
                            @"CREATE TABLE Flahcards (
                            Id INT PRIMARY KEY,
                            Name NVARCHAR(50),
                            StackId INT, -- Foreign key reference to Stacks table
                            FOREIGN KEY (StackId) REFERENCES Stacks(Id)
                            )";
                        using (SqlCommand createTableCommand = new SqlCommand(createTableQuery, connection))
                        {
                            createTableCommand.ExecuteNonQuery();
                            Console.WriteLine("Table 'Flahcards' created successfully.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Table 'Flahcards' already exists in database 'FlashCardDataBase'.");
                    }
                }


                // Generate random values for ID and Name
                Random random = new Random();
                int id = random.Next(1, 2); // Generate random ID between 1 and 1000
                string name = "Name_" + id.ToString(); // Generate a random name based on the ID

                // Check if the ID already exists in testTable
                string checkIdQuery = $"SELECT COUNT(*) FROM Flahcards WHERE Id = {id}";
                using (SqlCommand checkIdCommand = new SqlCommand(checkIdQuery, connection))
                {
                    int idCount = Convert.ToInt32(checkIdCommand.ExecuteScalar());
                    if (idCount == 0)
                    {
                        // Insert the record if ID doesn't exist
                        string insertRecordQuery = $"INSERT INTO Flahcards (Id, Name) VALUES ({id}, '{name}')";
                        using (SqlCommand insertRecordCommand = new SqlCommand(insertRecordQuery, connection))
                        {
                            insertRecordCommand.ExecuteNonQuery();
                            Console.WriteLine($"Record inserted into 'Flahcards': ID = {id}, Name = {name}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Record with ID = {id} already exists in 'Flahcards'.");
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
