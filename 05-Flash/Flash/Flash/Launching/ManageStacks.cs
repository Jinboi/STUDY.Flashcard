using Spectre.Console;
using Flash.Launching.SubManageStacks;
using Flash.Helper;
using System.Data.SqlClient;

namespace Flash.Launching;
internal class ManageStacks
{
    internal static void GetManageStacks()
    {
        Console.Clear();

        ShowBanner.GetShowBanner("Manage Stacks", Color.Green);

        int stacksTableCount = CheckStacksTable();
        
        if (stacksTableCount == 0)
        {
            CreateStacksTable();
        }
        else
        {
            Console.WriteLine("StacksTable already exists");
        }           

        AllExistingStacks.ShowAllExistingStacks();

        Console.WriteLine("Input Name of the Stack you want to work with Or Input 0 to Return to MainMenu");
        Console.WriteLine("If you add a Stack Name that doesn't exist, you'll be creating a new Stack under that Name.");

        string currentWorkingStack = Console.ReadLine(); 

        if (currentWorkingStack == "0")
        {
            ReturnComment.MainMenuReturnComments();
        }

        else
        {
            CheckExistingStacks.GetCheckExistingStacks(currentWorkingStack);

            WorkingStackMenu.GetWorkingStackMenu(currentWorkingStack);
        }

        ReturnComment.MainMenuReturnComments();
    }

    internal static int CheckStacksTable()
    {
        using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
        {
            connection.Open();
            connection.ChangeDatabase("DataBaseFlashCard");

            // Check if 'Flashcards' table exists
            string checkStacksTableQuery =
                @"SELECT COUNT(*) 
                        FROM INFORMATION_SCHEMA.TABLES 
                        WHERE TABLE_NAME = 'Stacks'";

            using (SqlCommand checkStacksTableCommand = new SqlCommand(checkStacksTableQuery, connection))
            {
                int stacksTableCount = Convert.ToInt32(checkStacksTableCommand.ExecuteScalar());
                return stacksTableCount;
            }
        }
    }

    internal static void CreateStacksTable()
    {
        using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
        {
            connection.Open();
            connection.ChangeDatabase("DataBaseFlashCard");

            string createStacksTableQuery = @"
                CREATE TABLE Stacks (
                Stack_Primary_Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                Name NVARCHAR(50) NOT NULL
                )";

            using (SqlCommand createStacksTableCommand = new SqlCommand(createStacksTableQuery, connection))
            {
                createStacksTableCommand.ExecuteNonQuery();
                Console.WriteLine("Table 'Stacks' created successfully.");
            }
        }
    }
}
