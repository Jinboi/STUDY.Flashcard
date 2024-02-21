using System.Data.SqlClient;

namespace Flash;

internal class CreateFlashcard
{
    static string connectionString = "Data Source=(LocalDB)\\LocalDBDemo;Integrated Security=True";
    internal static void GetCreateFlashcard()
    {
               
        Console.WriteLine("You're here to add a flashcard to the current stack");

        Console.WriteLine("Insert Front");
        
        string front = Console.ReadLine();

        Console.WriteLine("Insert Back");

        string back = Console.ReadLine();                 
        
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            connection.ChangeDatabase("DataBaseFlashCard");

            string checkIdCountQuery = $"SELECT COUNT(*) FROM Flashcards";
            using (SqlCommand checkIdCommand = new SqlCommand(checkIdCountQuery, connection))
            {
                int lastRowId = Convert.ToInt32(checkIdCommand.ExecuteScalar());
                int id = lastRowId + 1;
                int stackId = 1; // 1 for now , need to fix
                
                string insertRecordQuery = $"INSERT INTO Flashcards (Id, Front, Back, StackId) VALUES ({id}, {front}, {back}, {stackId})";
                using (SqlCommand insertRecordCommand = new SqlCommand(insertRecordQuery, connection))
                {
                    insertRecordCommand.ExecuteNonQuery();
                    Console.WriteLine($"Record inserted into 'Flashcards': ID = {id}, Front = {id}, Back = {id}, StackId = {stackId}");
                }                
                
            }
        }
    }
}
