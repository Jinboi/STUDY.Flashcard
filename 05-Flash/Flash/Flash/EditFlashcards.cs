
namespace Flash
{
    internal class EditFlashcards
    {
        public static void GetEditFlashcards(string currentWorkingStack)
        {
            Console.WriteLine($"Current Stack: {currentWorkingStack}\n");

            ViewAllFlashcards.GetViewAllFlashcards(currentWorkingStack);

            Console.WriteLine("What to edit?");
            Console.ReadLine();
        }
    }
}
