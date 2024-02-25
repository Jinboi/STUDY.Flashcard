using System.Data.SqlClient;

namespace Flash.Helper;
internal class AllExistingStacks
{
    internal static void ShowAllExistingStacks()
    {
        using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
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
                            Console.WriteLine($"Stack Name: {reader.GetString(0)}, Stack Id: {reader.GetInt32(1)}");
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
