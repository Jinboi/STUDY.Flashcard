using Spectre.Console;
using System.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;

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

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            connection.ChangeDatabase("DataBaseFlashCard");

            // Check if 'Stacks' table exists
            string checkTableQuery =
                @"SELECT COUNT(*) 
                          FROM INFORMATION_SCHEMA.TABLES 
                          WHERE TABLE_NAME = 'Stacks'";

            using (SqlCommand checkTableCommand = new SqlCommand(checkTableQuery, connection))
            {
                int tableCount = Convert.ToInt32(checkTableCommand.ExecuteScalar());
                if (tableCount == 0)
                {
                    // If 'Stacks' table doesn't exist, create it
                    string createStacksTableQuery =
                        @"CREATE TABLE Stacks (
                                  Stack_Primary_Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                                  Name NVARCHAR(50) UNIQUE NOT NULL                                 
                                )";
                    using (SqlCommand createTableCommand = new SqlCommand(createStacksTableQuery, connection))
                    {
                        createTableCommand.ExecuteNonQuery();
                        Console.WriteLine("Table 'Stacks' created successfully.");
                    }
                }
                else
                {
                    Console.WriteLine("Table 'Stacks' already exists in database 'DataBaseFlashCard'.");
                }
            }

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


            Console.WriteLine("Input a current stack name Or Input 0 to exit input");

            string currentWorkingStack = Console.ReadLine();





            if (currentWorkingStack == "0")
            {
                Console.WriteLine("\nGoodbye!\n");
                MainMenu.GetMainMenu();
            }

            else
            {
                //insert into stacks table

                
                string checkDuplicatedStackQuery =
                    @$"SELECT COUNT(*) 
                    FROM dbo.Stacks 
                    WHERE Name = '{currentWorkingStack}'";

                using (SqlCommand checkDuplicatedStackCommand = new SqlCommand(checkDuplicatedStackQuery, connection))
                { 
                    int SameNameStacksCount = Convert.ToInt32(checkDuplicatedStackCommand.ExecuteScalar());
                    if (SameNameStacksCount == 0)
                    {
                        // Insert data into 'Stacks' table
                        string insertStackQuery = $"INSERT INTO Stacks (Name) VALUES ('{currentWorkingStack}')";

                        using (SqlCommand insertStackCommand = new SqlCommand(insertStackQuery, connection))
                        {

                            insertStackCommand.ExecuteNonQuery();
                            Console.WriteLine($"Added {currentWorkingStack} to Stacks");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Did not added {currentWorkingStack} to Stacks as it already exists");
                    }

                }

                WorkingStackMenu.GetWorkingStackMenu(currentWorkingStack);
            }



        }
    }
}
