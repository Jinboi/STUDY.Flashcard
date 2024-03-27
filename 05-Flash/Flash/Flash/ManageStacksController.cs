using Flash.Helper.MainHelper;
using Flash.Helper.SubManageStacksHelper;
using System.Data.SqlClient;
using Spectre.Console;

namespace Flash;
internal class ManageStacksController
{
    internal static int GetCheckStacksTable()
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
    internal static void GetCreateFlashcard(string currentWorkingStack)
    {
        int flashcardsTableCount = ManageStacksService.GetCheckFlashcardsTableExists();

        if (flashcardsTableCount == 0)
        {
            ManageStacksService.GetCreateFlashcardsTable();
        }
        else
        {
            Console.WriteLine("Flashcards Table alreaday exists");
        }

        ManageStacksService.GetCreateAFlashcard(currentWorkingStack);

        MainHelper.MainMenuReturnComments();
    }
    internal static void GetCreateStacksTable()
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
    internal static void GetDeleteFlashcards(string currentWorkingStack)
    {
        Console.WriteLine($"Current Stack: {currentWorkingStack}\n");

        GetViewAllFlashcards(currentWorkingStack);

        Console.WriteLine("What to delete?");

        int currentWorkingFlashcardId = MainHelper.GetSelectFlashcardToWorkWith();

        ManageStacksService.GetDeleteSelectedFlashcard(currentWorkingFlashcardId);

        MainHelper.MainMenuReturnComments();
    }
    public static void GetEditFlashcards(string currentWorkingStack)
    {
        Console.WriteLine($"Current Stack: {currentWorkingStack}\n");

        GetViewAllFlashcards(currentWorkingStack);

        Console.WriteLine("What to edit?");

        int currentWorkingFlashcardId = MainHelper.GetSelectFlashcardToWorkWith();

        ManageStacksService.GetShowFlashcardsInCurrentStack(currentWorkingFlashcardId);

        ManageStacksService.GetEditFlashcardsInCurrentStack(currentWorkingFlashcardId);

        MainHelper.MainMenuReturnComments();
    }
    internal static void GetViewAllFlashcards(string currentWorkingStack)
    {
        Console.WriteLine($"{currentWorkingStack}");

        ManageStacksService.GetViewAllFlashcardsMethod(currentWorkingStack);

        MainHelper.MainMenuReturnComments();
    }
    internal static void GetViewXAmountFlashcards(string currentWorkingStack)
    {
        Console.WriteLine($"{currentWorkingStack}");

        Console.WriteLine($"What is your X amount?");

        string xAmountString = Console.ReadLine();

        int xAmount = ManageStacksService.GetSelectXAmount(xAmountString);

        ManageStacksService.GetViewXAmountFlashcardsMethod(currentWorkingStack, xAmount);

        MainHelper.MainMenuReturnComments();
    }
    internal static void GetWorkingStackMenu(string currentWorkingStack)
    {
        MainHelper.GetShowBanner("Current Stack Menu", Color.Gold1);

        try
        {
            MainHelper.GetCurrentStackMenuOptions(currentWorkingStack);

            string command = Console.ReadLine();

            ExecuteCurrentStackMenuOptions.GetExecuteCurrentStackMenuOptions(command, currentWorkingStack);
        }

        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }

        MainHelper.MainMenuReturnComments();
    }
}
