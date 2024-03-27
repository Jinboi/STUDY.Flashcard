using System.Data.SqlClient;

namespace Flash.Helper.SubManageStacksHelper;
internal class ManageStacksService
{
    internal static int GetCheckFlashcardsTableExists()
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



    internal static void GetCreateAFlashcard(string currentWorkingStack)
    {

        int currentWorkingStackId;
        string getCurrentStackIdQuery =
            $@"SELECT Stack_Primary_Id 
            FROM Stacks 
            WHERE Name = '{currentWorkingStack}'";

        using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
        {
            connection.Open();
            connection.ChangeDatabase("DataBaseFlashCard");

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

                    string insertFlashcardQuery =
                        @"INSERT INTO Flashcards (Front, Back, Stack_Primary_Id)
                                          VALUES (@Front, @Back, @StackPrimaryId)";

                    using (SqlCommand insertFlashcardCommand = new SqlCommand(insertFlashcardQuery, connection))
                    {
                        insertFlashcardCommand.Parameters.AddWithValue("@Front", front);
                        insertFlashcardCommand.Parameters.AddWithValue("@Back", back);
                        insertFlashcardCommand.Parameters.AddWithValue("@StackPrimaryId", currentWorkingStackId);

                        insertFlashcardCommand.ExecuteNonQuery();
                        Console.WriteLine("Flashcard created successfully.");
                    }
                }
            }
        }
    }



    internal static void GetCreateFlashcardsTable()
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


    internal static void GetDeleteSelectedFlashcard(int currentWorkingFlashcardId)
    {

        try
        {
            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
            {
                connection.Open();
                connection.ChangeDatabase("DataBaseFlashCard");

                string getCurrentFlashcardIdQuery =
                    $@"SELECT Flashcard_Primary_Id, Front, Back, Stack_Primary_Id  
                           FROM Flashcards 
                           WHERE Flashcard_Primary_Id = '{currentWorkingFlashcardId}'";

                using (SqlCommand getCurrentFlashcardIdCommand = new SqlCommand(getCurrentFlashcardIdQuery, connection))
                {
                    object result = getCurrentFlashcardIdCommand.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        List<FlashcardDto> flashcards = new List<FlashcardDto>();
                        using (SqlCommand command = new SqlCommand(getCurrentFlashcardIdQuery, connection))
                        {
                            command.Parameters.AddWithValue("@currentWorkingFlashcardId", currentWorkingFlashcardId);

                            // Execute the command and retrieve the data
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                // Check if any rows are returned
                                if (reader.HasRows)
                                {
                                    // Loop through each row and create DTOs
                                    while (reader.Read())
                                    {
                                        FlashcardDto flashcard = new FlashcardDto
                                        {
                                            Flashcard_Primary_Id = reader.GetInt32(0),
                                            Front = reader.GetString(1),
                                            Back = reader.GetString(2),
                                            Stack_Primary_Id = reader.GetInt32(3)
                                        };
                                        flashcards.Add(flashcard);
                                    }

                                    Renumber.GetRenumberFlashcards(flashcards);

                                    // Display flashcards
                                    foreach (var flashcard in flashcards)
                                    {
                                        Console.WriteLine(@$"
                                            Flashcard_Primary_Id: {flashcard.Flashcard_Primary_Id}, 
                                            Front: {flashcard.Front}, 
                                            Back: {flashcard.Back}, 
                                            Stack_Primary_Id: {flashcard.Stack_Primary_Id}");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("No flashcards TO EDIT found.");
                                }
                            }
                        }
                    }
                }

                Console.WriteLine("Deleting this flashcard");

                string deleteFlashcardQuery =
                    @"DELETE FROM Flashcards 
                        WHERE Flashcard_Primary_Id = @FlashcardId";

                using (SqlCommand deleteCommand = new SqlCommand(deleteFlashcardQuery, connection))
                {
                    deleteCommand.Parameters.AddWithValue("@FlashcardId", currentWorkingFlashcardId);

                    int rowsAffected = deleteCommand.ExecuteNonQuery();
                    Console.WriteLine($"{rowsAffected} row(s) deleted.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }

    }


    internal static void GetEditFlashcardsInCurrentStack(int currentWorkingFlashcardId)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
            {
                connection.Open();
                connection.ChangeDatabase("DataBaseFlashCard");

                Console.WriteLine("Update Front");
                string updatedFront = Console.ReadLine();
                Console.WriteLine("Update Back");
                string updatedBack = Console.ReadLine();

                // Update the flashcard with the new front and back
                string updateFlashcardQuery =
                    @"UPDATE Flashcards 
                        SET Front = @UpdatedFront, Back = @UpdatedBack 
                        WHERE Flashcard_Primary_Id = @FlashcardId";

                using (SqlCommand updateCommand = new SqlCommand(updateFlashcardQuery, connection))
                {
                    updateCommand.Parameters.AddWithValue("@UpdatedFront", updatedFront);
                    updateCommand.Parameters.AddWithValue("@UpdatedBack", updatedBack);
                    updateCommand.Parameters.AddWithValue("@FlashcardId", currentWorkingFlashcardId);

                    int rowsAffected = updateCommand.ExecuteNonQuery();
                    Console.WriteLine($"{rowsAffected} row(s) updated.");
                }
            }
        }

        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }



    internal static int GetSelectXAmount(string xAmountString)
    {
        int xAmount;

        if (int.TryParse(xAmountString, out xAmount))
        {
            // Conversion successful, xAmount now contains the integer value
            Console.WriteLine("xAmount is: " + xAmount);
        }
        else
        {
            // Conversion failed, handle the invalid input here
            Console.WriteLine("Invalid input. Please enter a valid integer.");
        }

        return xAmount;
    }


    internal static void GetShowFlashcardsInCurrentStack(int currentWorkingFlashcardId)
    {

        try
        {
            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
            {
                connection.Open();
                connection.ChangeDatabase("DataBaseFlashCard");

                string getCurrentFlashcardIdQuery =
                    $@"SELECT Flashcard_Primary_Id, Front, Back, Stack_Primary_Id  
                           FROM Flashcards 
                           WHERE Flashcard_Primary_Id = '{currentWorkingFlashcardId}'";

                using (SqlCommand getCurrentFlashcardIdCommand = new SqlCommand(getCurrentFlashcardIdQuery, connection))
                {
                    object result = getCurrentFlashcardIdCommand.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        List<FlashcardDto> flashcards = new List<FlashcardDto>();
                        using (SqlCommand command = new SqlCommand(getCurrentFlashcardIdQuery, connection))
                        {
                            command.Parameters.AddWithValue("@currentWorkingFlashcardId", currentWorkingFlashcardId);

                            // Execute the command and retrieve the data
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                // Check if any rows are returned
                                if (reader.HasRows)
                                {
                                    // Loop through each row and create DTOs
                                    while (reader.Read())
                                    {
                                        FlashcardDto flashcard = new FlashcardDto
                                        {
                                            Flashcard_Primary_Id = reader.GetInt32(0),
                                            Front = reader.GetString(1),
                                            Back = reader.GetString(2),
                                            Stack_Primary_Id = reader.GetInt32(3)
                                        };
                                        flashcards.Add(flashcard);
                                    }

                                    Renumber.GetRenumberFlashcards(flashcards);

                                    // Display flashcards
                                    foreach (var flashcard in flashcards)
                                    {
                                        Console.WriteLine(@$"
                                            Flashcard_Primary_Id: {flashcard.Flashcard_Primary_Id},
                                            Front: {flashcard.Front}, 
                                            Back: {flashcard.Back}, 
                                            Stack_Primary_Id: {flashcard.Stack_Primary_Id}");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("No flashcards TO EDIT found.");
                                }
                            }
                        }
                    }
                }
            }
        }

        catch (Exception ex)
        {

            Console.WriteLine("Error: " + ex.Message);
        }

    }



    internal static void GetViewAllFlashcardsMethod(string currentWorkingStack)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
            {
                connection.Open();
                connection.ChangeDatabase("DataBaseFlashCard");

                int currentWorkingStackId;
                string getCurrentStackIdQuery =
                    $@"SELECT Stack_Primary_Id 
                           FROM Stacks 
                           WHERE Name = '{currentWorkingStack}'";

                using (SqlCommand getCurrentStackIdCommand = new SqlCommand(getCurrentStackIdQuery, connection))
                {
                    object result = getCurrentStackIdCommand.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        currentWorkingStackId = Convert.ToInt32(result);

                        string selectQuery =
                            $@"SELECT Flashcard_Primary_Id , Front, Back, Stack_Primary_Id
                                   FROM Flashcards 
                                   WHERE Stack_Primary_Id = '{currentWorkingStackId}'";

                        List<FlashcardDto> flashcards = new List<FlashcardDto>();
                        using (SqlCommand command = new SqlCommand(selectQuery, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        FlashcardDto flashcard = new FlashcardDto
                                        {
                                            Flashcard_Primary_Id = reader.GetInt32(0),
                                            Front = reader.GetString(1),
                                            Back = reader.GetString(2),
                                            Stack_Primary_Id = reader.GetInt32(3)
                                        };
                                        flashcards.Add(flashcard);
                                    }

                                    Renumber.GetRenumberFlashcards(flashcards);

                                    foreach (var flashcard in flashcards)
                                    {
                                        Console.WriteLine($"Flashcard_Primary_Id: {flashcard.Flashcard_Primary_Id}, Front: {flashcard.Front}, Back: {flashcard.Back}, Stack_Primary_Id: {flashcard.Stack_Primary_Id}");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("No flashcards found.");
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }

    }


    internal static void GetViewXAmountFlashcardsMethod(string currentWorkingStack, int xAmount)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
            {
                connection.Open();
                connection.ChangeDatabase("DataBaseFlashCard");

                int currentWorkingStackId;
                string getCurrentStackIdQuery =
                    $@"SELECT Stack_Primary_Id 
                           FROM Stacks 
                           WHERE Name = '{currentWorkingStack}'";

                using (SqlCommand getCurrentStackIdCommand = new SqlCommand(getCurrentStackIdQuery, connection))
                {
                    object result = getCurrentStackIdCommand.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        currentWorkingStackId = Convert.ToInt32(result);

                        // Query to select a limited number of rows from the Flashcards table for the current stack
                        string selectQuery =
                             $@"SELECT TOP (@xAmount) Flashcard_Primary_Id, Front, Back, Stack_Primary_Id
                                   FROM Flashcards 
                                   WHERE Stack_Primary_Id = @currentWorkingStackId";

                        List<FlashcardDto> flashcards = new List<FlashcardDto>();
                        using (SqlCommand command = new SqlCommand(selectQuery, connection))
                        {
                            // Add parameters
                            command.Parameters.AddWithValue("@xAmount", xAmount);
                            command.Parameters.AddWithValue("@currentWorkingStackId", currentWorkingStackId);

                            // Execute the command and retrieve the data
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                // Check if any rows are returned
                                if (reader.HasRows)
                                {
                                    // Loop through each row and create DTOs
                                    while (reader.Read())
                                    {
                                        FlashcardDto flashcard = new FlashcardDto
                                        {
                                            Flashcard_Primary_Id = reader.GetInt32(0),
                                            Front = reader.GetString(1),
                                            Back = reader.GetString(2),
                                            Stack_Primary_Id = reader.GetInt32(3)
                                        };
                                        flashcards.Add(flashcard);
                                    }

                                    Renumber.GetRenumberFlashcards(flashcards);

                                    // Display flashcards
                                    foreach (var flashcard in flashcards)
                                    {
                                        Console.WriteLine($"Flashcard_Primary_Id: {flashcard.Flashcard_Primary_Id}, Front: {flashcard.Front}, Back: {flashcard.Back}, Stack_Primary_Id: {flashcard.Stack_Primary_Id}");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("No flashcards found.");
                                }
                            }
                        }
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
