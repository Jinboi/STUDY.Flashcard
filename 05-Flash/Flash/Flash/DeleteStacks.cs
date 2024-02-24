using System.Data.SqlClient;

namespace Flash
{
    internal class DeleteStacks
    {
        static string connectionString = "Data Source=(LocalDB)\\LocalDBDemo;Integrated Security=True";
        internal static void GetDeleteStacks()
        {
            Console.WriteLine("All the stacks in your database: ");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.ChangeDatabase("DataBaseFlashCard");
                try
                {
                    string showAllStacks = @"
                    SELECT Name, Stack_Primary_Id
                    FROM Stacks";


                    using (SqlCommand showAllStacksCommand = new SqlCommand(showAllStacks, connection))
                    {
                        // Execute the command and obtain a data reader
                        using (SqlDataReader reader = showAllStacksCommand.ExecuteReader())
                        {
                            // Display the names of all tables
                            Console.WriteLine("Stacks in the 'Stacks' Table:");
                            while (reader.Read())
                            {
                                Console.WriteLine(reader.GetString(0));
                                Console.WriteLine(reader.GetInt32(1));
                            }
                        }
                    }
                }

                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }

            }

            Console.WriteLine("Whaich stack to delete?");
            string currentWorkingStack = Console.ReadLine();
            int currentWorkingStackId;

            if (Int32.TryParse(currentWorkingStack, out currentWorkingStackId))
            {

                Console.WriteLine($"Selected Stack_Primary_Id: {currentWorkingStackId}");
            }
            else
            {
                Console.WriteLine("Unable to convert the string to an integer.");
            }


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.ChangeDatabase("DataBaseFlashCard");

                // Delete flashcards associated with the stack
                string deleteFlashcardsQuery =
                    @"DELETE FROM Flashcards 
                      WHERE Stack_Primary_Id = @StackId";

                using (SqlCommand deleteFlashcardsCommand = new SqlCommand(deleteFlashcardsQuery, connection))
                {
                    deleteFlashcardsCommand.Parameters.AddWithValue("@StackId", currentWorkingStackId);
                    int flashcardsDeleted = deleteFlashcardsCommand.ExecuteNonQuery();
                    Console.WriteLine($"Deleted {flashcardsDeleted} flashcard(s) associated with the stack.");
                }

                // Delete study data associated with the stack
                string deleteStudyDataQuery =
                    @"DELETE FROM Study 
                      WHERE Stack_Primary_Id = @StackId";

                using (SqlCommand deleteStudyDataCommand = new SqlCommand(deleteStudyDataQuery, connection))
                {
                    deleteStudyDataCommand.Parameters.AddWithValue("@StackId", currentWorkingStackId);
                    int studyDataDeleted = deleteStudyDataCommand.ExecuteNonQuery();
                    Console.WriteLine($"Deleted {studyDataDeleted} study data record(s) associated with the stack.");
                }

                // Delete the stack
                string deleteStackQuery =
                    @"DELETE FROM Stacks 
                      WHERE Stack_Primary_Id = @StackId";

                using (SqlCommand deleteStackCommand = new SqlCommand(deleteStackQuery, connection))
                {
                    deleteStackCommand.Parameters.AddWithValue("@StackId", currentWorkingStackId);
                    int stacksDeleted = deleteStackCommand.ExecuteNonQuery();
                    Console.WriteLine($"Deleted stack with Stack_Primary_Id: {currentWorkingStackId}");
                }
            }
        }    
    }
}
