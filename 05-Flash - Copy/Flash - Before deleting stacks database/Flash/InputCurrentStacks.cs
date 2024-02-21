using Spectre.Console;
using System.Data.SqlClient;

namespace Flash;

internal class InputCurrentStacks
{
    static string connectionString = "Data Source=(LocalDB)\\LocalDBDemo;Integrated Security=True";
    internal static void GetInputCurrentStacks()
    {
        Console.Clear();

        AnsiConsole.Write
                (
                    new FigletText("Input Current Stacks")
                        .LeftJustified()
                        .Color(Color.Orange3)
                );

        var panel = new Panel("What Would You Like to Do?");

        AnsiConsole.Write(
                new Panel(panel)
                    .Header("")
                    .Collapse()
                    .RoundedBorder()
                    .BorderColor(Color.Green));

        string showAllStacks = @"
                    SELECT name
                    FROM sys.tables";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            connection.ChangeDatabase("DataBaseFlashCard");

            using (SqlCommand showAllStacksCommand = new SqlCommand(showAllStacks, connection))
            {
                // Execute the command and obtain a data reader
                using (SqlDataReader reader = showAllStacksCommand.ExecuteReader())
                {
                    // Display the names of all tables
                    Console.WriteLine("Tables in the 'stacks' database:");
                    while (reader.Read())
                    {
                        Console.WriteLine(reader.GetString(0));
                    }
                }
            }
        }

        Console.WriteLine("Input a stack name to configure OR 0 to exit input ");
        Console.WriteLine("If no stacks, Type the name of stacks you want to create");

        string command = Console.ReadLine();
        string currentWorkingStack = "";

        if (command == "0")
        {
            Console.WriteLine("\nGoodbye!\n");
            MainMenu.GetMainMenu();
        }

        else
        {
            currentWorkingStack = command;

            //see available stacks 
            try
            {
                    Console.WriteLine($"Current Stack: {currentWorkingStack}\n");
                    // Create a list of Items, apply separate styles to each
                    var rows = new List<Text>(){
                    new Text("-Type 0 to Return to Main Menu", new Style(Color.Red, Color.Black)),
                    new Text("-Type 1 to Change Current Stack", new Style(Color.Green, Color.Black)),
                    new Text("-Type 2 to View All Flashcards in Stack", new Style(Color.Blue, Color.Black)),
                    new Text("-Type 3 to View X amount of cards in stack", new Style(Color.Purple, Color.Black)),
                    new Text("-Type 4 to Create a Flashcard in current stack", new Style(Color.Orange3, Color.Black)),
                    new Text("-Type 5 to Edit a Flashcard", new Style(Color.Red, Color.Black)),
                    new Text("-Type 6 to Delete a Flashcard", new Style(Color.Green, Color.Black)),
                    };

                    // Renders each item with own style
                    AnsiConsole.Write(new Rows(rows));

                    string commandTwo = Console.ReadLine();

                    switch (commandTwo)
                    {
                        case "0":
                            Console.WriteLine("\nGoodbye!\n");
                            MainMenu.GetMainMenu();
                            break;
                        case "1":
                            Console.WriteLine("Change Current Stack");
                            InputCurrentStacks.GetInputCurrentStacks();
                            break;
                        case "2":
                            Console.WriteLine("View All Flashcards in Stack");
                            ViewAllFlashCards.GetViewAllFlashCards();
                            break;
                        case "3":
                            Console.WriteLine("View X amount of cards in stack");
                            break;
                        case "4":
                            Console.WriteLine("Create a Flashcard in current stack");
                            CreateFlashcard.GetCreateFlashcard();
                            break;
                        case "5":
                            Console.WriteLine("Edit a Flashcard");
                            break;
                        case "6":
                            Console.WriteLine("Delete a Flashcard");
                            break;

                        default:
                            Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                            break;
                    }
                    Console.ReadLine();
                }
            
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }



    }
}


/*
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    connection.ChangeDatabase("DataBaseFlashCard");


                    string checkTableQuery =
                        @$"SELECT COUNT(*) 
                    FROM INFORMATION_SCHEMA.TABLES 
                    WHERE TABLE_NAME = '{currentWorkingStack}'";

                    using (SqlCommand checkTableCommand = new SqlCommand(checkTableQuery, connection))
                    {
                        int tableCount = Convert.ToInt32(checkTableCommand.ExecuteScalar());
                        if (tableCount == 0)
                        {
                            string createTableQuery =
                                @$"CREATE TABLE {currentWorkingStack} (
                            Id INT PRIMARY KEY, 
                            Name NVARCHAR(50)
                            )";
                            using (SqlCommand createTableCommand = new SqlCommand(createTableQuery, connection))
                            {
                                createTableCommand.ExecuteNonQuery();
                                Console.WriteLine($"Table '{currentWorkingStack}' created successfully.");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Table '{currentWorkingStack}' already exists in database 'DataBaseFlashCard'.");
                        }
                    }


                    // Generate random values for ID and Name
                    Random random = new Random();
                    int id = random.Next(1, 2); // Generate random ID between 1 and 1000
                    string name = "Name_" + id.ToString(); // Generate a random name based on the ID

                    // Check if the ID already exists in testTable
                    string checkIdQuery = $"SELECT COUNT(*) FROM {currentWorkingStack} WHERE Id = {id}";
                    using (SqlCommand checkIdCommand = new SqlCommand(checkIdQuery, connection))
                    {
                        int idCount = Convert.ToInt32(checkIdCommand.ExecuteScalar());
                        if (idCount == 0)
                        {
                            // Insert the record if ID doesn't exist
                            string insertRecordQuery = $"INSERT INTO {currentWorkingStack} (Id, Name) VALUES ({id}, '{name}')";
                            using (SqlCommand insertRecordCommand = new SqlCommand(insertRecordQuery, connection))
                            {
                                insertRecordCommand.ExecuteNonQuery();
                                Console.WriteLine($"Record inserted into '{currentWorkingStack}': ID = {id}, Name = {name}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Record with ID = {id} already exists in '{currentWorkingStack}'.");
                        }

                    }

                    Console.Clear();



                    using (SqlCommand showAllStacksCommand = new SqlCommand(showAllStacks, connection))
                    {
                        // Execute the command and obtain a data reader
                        using (SqlDataReader reader = showAllStacksCommand.ExecuteReader())
                        {
                            // Display the names of all tables
                            Console.WriteLine("Tables in the 'stacks' database:");
                            while (reader.Read())
                            {
                                Console.WriteLine(reader.GetString(0));
                            }
                        }
                    }

*/