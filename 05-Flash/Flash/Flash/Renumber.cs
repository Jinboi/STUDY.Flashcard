using Flash.Helper.DTO;

namespace Flash;

internal class Renumber
{

    internal static void GetRenumberFlashcards(List<FlashcardDto> flashcards)
    {
        for (int i = 0; i < flashcards.Count; i++)
        {
            flashcards[i].Flashcard_Primary_Id = i + 1;
        }
    }


    internal static void GetRenumberStacks(List<StacksDto> stacks)
    {
        for (int i = 0; i < stacks.Count; i++)
        {
            stacks[i].Stack_Primary_Id = i + 1;
        }
    }


    internal static void GetRenumberStudys(List<StudyDto> studys)
    {
        for (int i = 0; i < studys.Count; i++)
        {
            studys[i].Study_Primary_Id = i + 1;
        }
    }
}
