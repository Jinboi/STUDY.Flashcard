using Flash.Helper;
using Spectre.Console;

namespace Flash.Launching;
internal class DeleteStacks
{    
    internal static void GetDeleteStacks()
    {
        Console.Clear();

        ShowBanner.GetShowBanner("Manage Stacks", Color.RosyBrown);

        Console.WriteLine("This is all the stacks in your database: ");

        AllExistingStacks.ShowAllExistingStacks();

        int stackIdToDelete = StackIdToDelete.GetStackIdToDelete();
        Console.WriteLine($"The stack ID to delete is: {stackIdToDelete}");

        DeleteAStack.ExecuteDeleteAStack(stackIdToDelete);

        ReturnComment.MainMenuReturnComments();
    }

}
