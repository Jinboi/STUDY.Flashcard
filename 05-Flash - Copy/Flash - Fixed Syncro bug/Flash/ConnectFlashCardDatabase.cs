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
                    WHERE TABLE_NAME = 'Flashcards'";


                using (SqlCommand checkTableCommand = new SqlCommand(checkTableQuery, connection))
                {
                    int tableCount = Convert.ToInt32(checkTableCommand.ExecuteScalar());
                    if (tableCount == 0)
                    {
                        string createTableQuery =
                            @"CREATE TABLE Flashcards (
                            Id INT PRIMARY KEY,
                            StackId INT, 
                            Front NVARCHAR(50),
                            Back NVARCHAR(50),                           
                            )";
                        using (SqlCommand createTableCommand = new SqlCommand(createTableQuery, connection))
                        {
                            createTableCommand.ExecuteNonQuery();
                            Console.WriteLine("Table 'Flashcards' created successfully.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Table 'Flashcards' already exists in database 'FlashCardDataBase'.");
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
