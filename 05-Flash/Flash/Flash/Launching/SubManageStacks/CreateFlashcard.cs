using Flash.Helper;
using System.Data.SqlClient;


namespace Flash.Launching.SubManageStacks;
internal class CreateFlashcard
{
    internal static void GetCreateFlashcard(string currentWorkingStack)
    {
        int flashcardsTableCount = CheckFlashcardsTableExists();

        if (flashcardsTableCount == 0 ) 
        {
            CreateFlashcardsTable();
        }
        else
        {
            Console.WriteLine("Flashcards Table alreaday exists");
        }

        CreateAFlashcard(currentWorkingStack);

        ReturnComment.MainMenuReturnComments();
    }

    internal static void CreateAFlashcard(string currentWorkingStack)
    {
        int currentWorkingStackId;
        string getCurrentStackIdQuery =
            $@"SELECT Stack_Primary_Id 
            FROM Stacks 
            WHERE Name = '{currentWorkingStack}'";

        using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
        {
            using (SqlCommand getCurrentStackIdCommand = new SqlCommand(getCurrentStackIdQuery, connection))
            {
                object result = getCurrentStackIdCommand.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    currentWorkingStackId = Convert.ToInt32(result);

                    Console.WriteLine("Write front");
                    string front = Console.ReadLine();
                    Console.WriteLine("Write back");
                    string back = Console.ReadLine();

                    // Insert flashcard into 'Flashcards' table
                    string insertFlashcardQuery =
                        @"INSERT INTO Flashcards (Front, Back, Stack_Primary_Id)
                                          VALUES (@Front, @Back, @StackPrimaryId)";

                    using (SqlCommand insertFlashcardCommand = new SqlCommand(insertFlashcardQuery, connection))
                    {
                        // Add parameters
                        insertFlashcardCommand.Parameters.AddWithValue("@Front", front);
                        insertFlashcardCommand.Parameters.AddWithValue("@Back", back);
                        insertFlashcardCommand.Parameters.AddWithValue("@StackPrimaryId", currentWorkingStackId);

                        // Execute the command
                        insertFlashcardCommand.ExecuteNonQuery();
                        Console.WriteLine("Flashcard created successfully.");
                    }
                }
            }
        }
    }

    internal static void CreateFlashcardsTable()    
    {
        using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
        {
            connection.Open();
            connection.ChangeDatabase("DataBaseFlashCard");

            string createFlashcardsTableQuery = @"
                CREATE TABLE Flashcards (
                Flashcard_Primary_Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                Front NVARCHAR(50) NOT NULL,
                Back NVARCHAR(50) NOT NULL,
                Stack_Primary_Id INT FOREIGN KEY REFERENCES Stacks(Stack_Primary_Id)
                )";

            using (SqlCommand createFlashcardsTableCommand = new SqlCommand(createFlashcardsTableQuery, connection))
            {
                createFlashcardsTableCommand.ExecuteNonQuery();
                Console.WriteLine("Table 'Flashcards' created successfully.");
            }
        }
    }

    internal static int CheckFlashcardsTableExists()
    {
        using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
        {
            connection.Open();
            connection.ChangeDatabase("DataBaseFlashCard");

            // Check if 'Flashcards' table exists
            string checkFlashcardsTableQuery =
                @"SELECT COUNT(*) 
                        FROM INFORMATION_SCHEMA.TABLES 
                        WHERE TABLE_NAME = 'Flashcards'";

            using (SqlCommand checkFlashcardsTableCommand = new SqlCommand(checkFlashcardsTableQuery, connection))
            {
                int flashcardsTableCount = Convert.ToInt32(checkFlashcardsTableCommand.ExecuteScalar());
                return flashcardsTableCount;
            }
        }
    }
}
