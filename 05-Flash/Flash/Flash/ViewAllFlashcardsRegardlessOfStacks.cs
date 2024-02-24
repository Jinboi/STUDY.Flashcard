using System.Data.SqlClient;

namespace Flash
{
    internal class ViewAllFlashcardsRegardlessOfStacks
    {
        static string connectionString = "Data Source=(LocalDB)\\LocalDBDemo;Integrated Security=True";
        internal static void GetViewAllFlashcardsRegardlessOfStacks()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    connection.ChangeDatabase("DataBaseFlashCard");
                    
                    // Query to select all rows and columns from the Flashcards table
                    string selectQuery =
                        $@"SELECT Flashcard_Primary_Id , Front, Back, Stack_Primary_Id
                            FROM Flashcards";

                    List<FlashcardDto> flashcards = new List<FlashcardDto>();
                    using (SqlCommand command = new SqlCommand(selectQuery, connection))
                    {
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
                                Console.WriteLine("No flashcards found.");
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
}
