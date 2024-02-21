using Spectre.Console;
using System.Data.SqlClient;

namespace Flash;

internal class ManageStacks
{
    static string connectionString = "Data Source=(LocalDB)\\LocalDBDemo;Integrated Security=True";
    internal static void GetManageStacks()
    {
        Console.Clear();
        AnsiConsole.Write
                (
                    new FigletText("Manage Stacks")
                        .LeftJustified()
                        .Color(Color.Green)
                );

        var panel = new Panel("What Would You Like to Do?");

        AnsiConsole.Write(
                new Panel(panel)
                    .Header("")
                    .Collapse()
                    .RoundedBorder()
                    .BorderColor(Color.Green));
        //see available stacks 
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                connection.ChangeDatabase("DataBaseFlashCard");


                string showAllStacks = @"
                    SELECT name
                    FROM sys.tables";


                using (SqlCommand showAllStacksCommand = new SqlCommand(showAllStacks, connection))
                {
                    // Execute the command and obtain a data reader
                    using (SqlDataReader reader = showAllStacksCommand.ExecuteReader())
                    {
                        // Display the names of all tables
                        Console.WriteLine("Tables in the 'DataBaseFlashCard' database:");
                        while (reader.Read())
                        {
                            Console.WriteLine(reader.GetString(0));
                        }
                    }
                }

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }

        // Create a list of Items, apply separate styles to each
        var rows = new List<Text>(){
                    new Text("-Type 0 to Exit", new Style(Color.Green, Color.Black)),
                    new Text("-Type 1 to Input Current Stacks", new Style(Color.Green, Color.Black)),
                    };

        // Renders each item with own style
        AnsiConsole.Write(new Rows(rows));

        string command = Console.ReadLine(); 

        switch (command)
        {
            case "0":
                Console.WriteLine("\nGoodbye!\n");
                MainMenu.GetMainMenu();
                break;
            case "1":
                InputCurrentStacks.GetInputCurrentStacks();
                break;

            default:
                Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                break;
        }
    }
}
