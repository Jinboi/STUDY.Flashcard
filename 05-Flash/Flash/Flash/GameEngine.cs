using Flash.Helper.MainHelper;
using Flash;
using Spectre.Console;
using System.Data.SqlClient;

namespace Flash;
internal class GameEngine
{
    internal static void GetMainMenu()
    {
        Console.Clear();
        bool closeApp = false;
        while (closeApp == false)
        {
            MainHelper.GetShowBanner("MAIN MENU", Color.White);

            MainHelper.GetShowMainMenuOptions();

            string command = Console.ReadLine();

            ExecuteMainMenuOptions.GetExecuteMainMenuOptions(command, closeApp);
        }
    }
    internal static void GetDeleteStacks()
    {
        Console.Clear();

        MainHelper.GetShowBanner("Manage Stacks", Color.RosyBrown);

        int stacksTableCount = MainHelper.GetCheckStacksTableExists();

        if (stacksTableCount == 0)
        {
            Console.WriteLine("Cannot delete a stack. Stacks Table does not exist.");
            MainHelper.MainMenuReturnComments();
        }
        else
        {
            Console.WriteLine("Stacks Table alreaday exists");
        }

        Console.WriteLine("This is all the stacks in your Stacks Table: ");

        MainHelper.ShowAllExistingStacks();

        int stackIdToDelete = MainHelper.GetStackIdToDelete();

        Console.WriteLine($"The stack ID to delete is: {stackIdToDelete}");

        MainHelper.ExecuteDeleteAStack(stackIdToDelete);

        MainHelper.MainMenuReturnComments();
    }
    internal static void GetManageStacks()
    {
        Console.Clear();

        MainHelper.GetShowBanner("Manage Stacks", Color.Green);

        int stacksTableCount = ManageStacksController.GetCheckStacksTable();

        if (stacksTableCount == 0)
        {
            ManageStacksController.GetCreateStacksTable();
        }
        else
        {
            Console.WriteLine("StacksTable already exists\n");
        }

        MainHelper.ShowAllExistingStacks();

        Console.WriteLine("\nInput Name of the Stack you want to work with Or Input 0 to Return to MainMenu");
        Console.WriteLine("\nIf you add a Stack Name that doesn't exist, you'll be creating a new Stack under that Name.");

        string currentWorkingStack = Console.ReadLine();

        if (currentWorkingStack == "0")
        {
            MainHelper.MainMenuReturnComments();
        }

        else
        {
            MainHelper.GetCheckExistingStacks(currentWorkingStack);

            ManageStacksController.GetWorkingStackMenu(currentWorkingStack);
        }

        MainHelper.MainMenuReturnComments();
    }
    internal static void GetStudy()
    {
        Console.Clear();

        MainHelper.GetShowBanner("Study", Color.Yellow);

        using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
        {
            connection.Open();
            connection.ChangeDatabase("DataBaseFlashCard");


            //see available stacks 
            try
            {
                string showAllStacks =
                    @"SELECT Stack_Primary_Id, Name
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
                            AnsiConsole.WriteLine($"ID: {reader.GetInt32(0)}, Name: {reader.GetString(1)}");
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        int studyStack = StudyHelper.GetSetUpStudyStack();

        StudyHelper.GetCreateStudyTable(studyStack);

        (int correctAnswer, int totalQuestions) = StudyHelper.GetShowFlashcardToStudy(studyStack);

        StudyHelper.GetRecordToStudy(studyStack, correctAnswer, totalQuestions);

        MainHelper.MainMenuReturnComments();
    }
    internal static void GetStudyHistory()
    {
        Console.Clear();

        MainHelper.GetShowBanner("View Study Session Data", Color.Orange3);

        MainHelper.GetShowStudyHistory();

        MainHelper.MainMenuReturnComments();
    }
    internal static void GetManageFlashcards()
    {
        Console.Clear();

        MainHelper.GetShowBanner("All Flashcards in All Stacks", Color.Plum1);

        MainHelper.GetShowAllCardsInAllStacks();

        MainHelper.MainMenuReturnComments();
    }
}
