using System;
using System.Data.SqlClient;

namespace Flash
{
    internal class CreateFlashcard
    {
        static string connectionString = "Data Source=(LocalDB)\\LocalDBDemo;Integrated Security=True";

        internal static void GetCreateFlashcard()
        {
            Console.WriteLine("You're here to add a flashcard to the current stack");

            Console.WriteLine("What's the name of the current stack again?");
            string stackName = Console.ReadLine();

            Console.WriteLine("Assign a number for the StackId that you want to put this Flashcard Under");
            string stackIdString = Console.ReadLine();
            int stackId = int.Parse(stackIdString);

            Console.WriteLine("Insert Front");
            string front = Console.ReadLine();

            Console.WriteLine("Insert Back");
            string back = Console.ReadLine();

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

                    // Insert data into 'Stacks' table
                    string insertStackQuery = $"INSERT INTO Stacks (Stack_Id, Stack_Name, Front, Back) VALUES ('{stackId}', '{stackName}', '{front}', '{back}')";
                    using (SqlCommand insertStackCommand = new SqlCommand(insertStackQuery, connection))
                    {
                        insertStackCommand.ExecuteNonQuery();
                        Console.WriteLine("Added to Stacks");
                    }

                    // Insert data into 'Flashcards' table
                    string insertFlashcardQuery = $"INSERT INTO Flashcards (Stack_Id, Stack_Name, Front, Back) VALUES ('{stackId}', '{stackName}', '{front}', '{back}')";
                    using (SqlCommand insertFlashcardCommand = new SqlCommand(insertFlashcardQuery, connection))
                    {
                        insertFlashcardCommand.ExecuteNonQuery();
                        Console.WriteLine("Added to Flashcard");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
