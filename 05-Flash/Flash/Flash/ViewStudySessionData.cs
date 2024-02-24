using System.Data.SqlClient;

namespace Flash
{
    internal class ViewStudySessionData
    {
        static string connectionString = "Data Source=(LocalDB)\\LocalDBDemo;Integrated Security=True";
        internal static void GetViewStudySessionData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    connection.ChangeDatabase("DataBaseFlashCard");

                    // Query to select all rows and columns from the Flashcards table
                    string selectQuery =
                        $@"SELECT Study_Primary_Id , Date, Score, Stack_Primary_Id
                            FROM Study";

                    List<StudyDto> studys = new List<StudyDto>();
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
                                    StudyDto study = new StudyDto
                                    {
                                        Study_Primary_Id = reader.GetInt32(0),
                                        Date = reader.GetDateTime(1),
                                        Score = reader.GetString(2),
                                        Stack_Primary_Id = reader.GetInt32(3)
                                    };
                                    studys.Add(study);
                                }

                                RenumberStudy.GetRenumberStudys(studys);

                                // Display flashcards
                                foreach (var study in studys)
                                {
                                    Console.WriteLine($"Study_Primary_Id: {study.Study_Primary_Id}, Date: {study.Date}, Score: {study.Score}, Stack_Primary_Id: {study.Stack_Primary_Id}");
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
