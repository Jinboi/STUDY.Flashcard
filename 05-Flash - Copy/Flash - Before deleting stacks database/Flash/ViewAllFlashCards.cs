using System.Data.SqlClient;

namespace Flash;

internal class ViewAllFlashCards
{
    static string connectionString = "Data Source=(LocalDB)\\LocalDBDemo;Integrated Security=True";
    internal static void GetViewAllFlashCards()
    {      
        try
        {            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();                

                connection.ChangeDatabase("DataBaseFlashCard");
                
                string showAllQuery = $"SELECT * FROM Flashcards";

                /*
                using (SqlCommand command = new SqlCommand(showAllQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {                            
                            int id = reader.GetInt32(0);
                            string Name = reader.GetString(1);                            

                            Console.WriteLine($"ID: {id}, Name: {Name}");
                        }
                    }
                }
                */
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }    
}
