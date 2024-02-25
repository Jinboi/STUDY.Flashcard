using Spectre.Console;
using Flash.Launching.SubManageStacks;
using Flash.Helper;

namespace Flash.Launching;
internal class ManageStacks
{
    internal static void GetManageStacks()
    {
        Console.Clear();

        ShowBanner.GetShowBanner("Manage Stacks", Color.Green);

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
}
