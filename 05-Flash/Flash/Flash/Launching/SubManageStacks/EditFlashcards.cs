using System.Data.SqlClient;
using Flash.Helper.MainHelper;
using Flash.Helper.Renumber;
using Flash.Helper.SubManageStacksHelper.EditFlashcardHelper;

namespace Flash.Launching.SubManageStacks;
internal class EditFlashcards
{
    public static void GetEditFlashcards(string currentWorkingStack)
    {
        Console.WriteLine($"Current Stack: {currentWorkingStack}\n");

        ViewAllFlashcards.GetViewAllFlashcards(currentWorkingStack);

        Console.WriteLine("What to edit?");

        int currentWorkingFlashcardId = SelectFlashcardToWorkWith.GetSelectFlashcardToWorkWith();

        ShowFlashcardsInCurrentStack.GetShowFlashcardsInCurrentStack(currentWorkingFlashcardId);

        EditFlashcardsInCurrentStack.GetEditFlashcardsInCurrentStack(currentWorkingFlashcardId);

        ReturnComment.MainMenuReturnComments();
    }

}
