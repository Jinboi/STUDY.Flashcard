using System.Data.SqlClient;

namespace Flash
{
    internal class ViewXAmountFlashcards
    {
        static string connectionString = "Data Source=(LocalDB)\\LocalDBDemo;Integrated Security=True";

        internal static void GetViewXAmountFlashcards(string currentWorkingStack)
        {

            Console.WriteLine($"{currentWorkingStack}");

            Console.WriteLine($"What is your X amount?");

            string xAmountString = Console.ReadLine();

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

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
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
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

        }
    }
}
