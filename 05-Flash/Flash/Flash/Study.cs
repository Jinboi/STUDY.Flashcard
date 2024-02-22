
using Spectre.Console;
using System.Data.SqlClient;

namespace Flash
{
    internal class Study
    {
        static string connectionString = "Data Source=(LocalDB)\\LocalDBDemo;Integrated Security=True";
        internal static void GetStudy()
        {
            Console.Clear();
            AnsiConsole.Write
                    (
                        new FigletText("Study")
                            .LeftJustified()
                            .Color(Color.Navy)
                    );

            var panel = new Panel("What Would You Like to Do?");

            AnsiConsole.Write(
                    new Panel(panel)
                        .Header("")
                        .Collapse()
                        .RoundedBorder()
                        .BorderColor(Color.Navy));


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.ChangeDatabase("DataBaseFlashCard");


                //see available stacks 
                try
                {
                    string showAllStacks = @"
                    SELECT Name
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
                            }
                        }
                    }
                }

                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }

                Console.WriteLine("Insert the Stack_Priamry_Id of the stack you want to study");

                string studyStackString = Console.ReadLine();
                int studyStack;

                if (Int32.TryParse(studyStackString, out studyStack))
                {

                    Console.WriteLine($"Selected Stack_Primary_Id: {studyStack}");
                }
                else
                {
                    Console.WriteLine("Unable to convert the string to an integer.");
                }

      




            }
        }
    }
}
