using System;
using System.Data.SqlClient;

namespace Flash
{
    internal class CreateFlashcard
    {
        static string connectionString = "Data Source=(LocalDB)\\LocalDBDemo;Integrated Security=True";

        internal static void GetCreateFlashcard()
        {      
            /*
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    connection.ChangeDatabase("DataBaseFlashCard");

                    // Check if 'Stacks' table exists
                    string checkTableQuery =
                        @"SELECT COUNT(*) 
                          FROM INFORMATION_SCHEMA.TABLES 
                          WHERE TABLE_NAME = 'Stacks'";

                    using (SqlCommand checkTableCommand = new SqlCommand(checkTableQuery, connection))
                    {
                        int tableCount = Convert.ToInt32(checkTableCommand.ExecuteScalar());
                        if (tableCount == 0)
                        {
                            // If 'Stacks' table doesn't exist, create it
                            string createStacksTableQuery =
                                @"CREATE TABLE Stacks (
                                  Stack_Primary_Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                                  Stack_Id INT NOT NULL,
                                  Stack_Name NVARCHAR(50) NOT NULL,
                                  Front NVARCHAR(50) NOT NULL,
                                  Back NVARCHAR(50) NOT NULL
                                )";
                            using (SqlCommand createTableCommand = new SqlCommand(createStacksTableQuery, connection))
                            {
                                createTableCommand.ExecuteNonQuery();
                                Console.WriteLine("Table 'Stacks' created successfully.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Table 'Stacks' already exists in database 'DataBaseFlashCard'.");
                        }
                    }

                    // Check if 'Flashcards' table exists
                    checkTableQuery =
                        @"SELECT COUNT(*) 
                          FROM INFORMATION_SCHEMA.TABLES 
                          WHERE TABLE_NAME = 'Flashcards'";

                    using (SqlCommand checkTableCommand = new SqlCommand(checkTableQuery, connection))
                    {
                        int tableCount = Convert.ToInt32(checkTableCommand.ExecuteScalar());
                        if (tableCount == 0)
                        {
                            // If 'Flashcards' table doesn't exist, create it
                            string createFlashcardsTableQuery =
                                @"CREATE TABLE Flashcards (
                                  Flashcard_Primary_Key INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                                  Stack_Id INT NOT NULL, 
                                  Stack_Name NVARCHAR(50) NOT NULL,
                                  Front NVARCHAR(50) NOT NULL,
                                  Back NVARCHAR(50) NOT NULL,
                                  CONSTRAINT FK_Stack_Id FOREIGN KEY (Stack_Id) REFERENCES Stacks(Stack_Primary_Id)
                                )";
                            using (SqlCommand createTableCommand = new SqlCommand(createFlashcardsTableQuery, connection))
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
            */
        }
    }
}
