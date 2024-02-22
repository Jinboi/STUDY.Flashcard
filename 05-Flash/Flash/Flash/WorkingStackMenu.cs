
 using Flash;
using Spectre.Console;

namespace Flash
{
    internal class WorkingStackMenu
    {
        internal static void GetWorkingStackMenu(string currentWorkingStack)
        {
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
                        ManageStacks.GetManageStacks();
                        break;
                    case "2":
                        Console.WriteLine("View All Flashcards in Stack");
                        ViewAllFlashcards.GetViewAllFlashcards(currentWorkingStack);
                        break;
                    case "3":
                        Console.WriteLine("View X amount of cards in stack");
                        ViewXAmountFlashcards.GetViewXAmountFlashcards(currentWorkingStack);
                        break;
                    case "4":
                        Console.WriteLine("Create a Flashcard in current stack");
                        CreateFlashcard.GetCreateFlashcard(currentWorkingStack);
                        break;
                    case "5":
                        Console.WriteLine("Edit a Flashcard");
                        EditFlashcards.GetEditFlashcards(currentWorkingStack);
                        break;
                    case "6":
                        Console.WriteLine("Delete a Flashcard");
                        DeleteFlashcards.GetDeleteFlashcards(currentWorkingStack);
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
