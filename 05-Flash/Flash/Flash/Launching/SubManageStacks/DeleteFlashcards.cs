using System.Data.SqlClient;
using Flash.Helper.Renumber;
using Flash.Helper;

namespace Flash.Launching.SubManageStacks;
internal class DeleteFlashcards
{
    internal static void GetDeleteFlashcards(string currentWorkingStack)
    {
        Console.WriteLine($"Current Stack: {currentWorkingStack}\n");

        ViewAllFlashcards.GetViewAllFlashcards(currentWorkingStack);

        Console.WriteLine("What to delete?");
        string currentWorkingFlashcardString = Console.ReadLine();
        int currentWorkingFlashcardId;

        if (int.TryParse(currentWorkingFlashcardString, out currentWorkingFlashcardId))
        {

            Console.WriteLine($"Selected Flashcard_Primary_Id: {currentWorkingFlashcardId}");
        }
        else
        {
            Console.WriteLine("Unable to convert the string to an integer.");
        }

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

                                    RenumberFlashcards.GetRenumberFlashcards(flashcards);

                                    // Display flashcards
                                    foreach (var flashcard in flashcards)
                                    {
                                        Console.WriteLine($"Flashcard_Primary_Id: {flashcard.Flashcard_Primary_Id}, Front: {flashcard.Front}, Back: {flashcard.Back}, Stack_Primary_Id: {flashcard.Stack_Primary_Id}");
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

                // Update the flashcard with the new front and back
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

        ReturnComment.MainMenuReturnComments();
    }

    
}

