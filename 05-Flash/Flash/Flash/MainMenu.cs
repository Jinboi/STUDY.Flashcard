using Spectre.Console;

namespace Flash;

static internal class MainMenu
{
    internal static void GetMainMenu()
    {
        Console.Clear();
        bool closeApp = false;
        while (closeApp == false)
        {
            AnsiConsole.Write
                (
                    new FigletText("MAIN MENU")
                        .LeftJustified()
                        .Color(Color.White)
                );

            var panel = new Panel("What Would You Like to Do?");

            AnsiConsole.Write(
                    new Panel(panel)
                        .Header("")
                        .Collapse()
                        .RoundedBorder()
                        .BorderColor(Color.Yellow));

            

                // Create a list of Items, apply separate styles to each
                var rows = new List<Text>(){
            new Text("-Type 0 to Exit", new Style(Color.Red, Color.Black)),
            new Text("-Type 1 to Manage Stacks", new Style(Color.Green, Color.Black)),
            new Text("-Type 2 to Manage Flashcards", new Style(Color.Blue, Color.Black)),
            new Text("-Type 3 to Study", new Style(Color.Purple, Color.Black)),
            new Text("-Type 4 to View Study Session Data", new Style(Color.Orange3, Color.Black)),
            new Text("-Type 5 to Delete a Stack",  new Style(Color.Aqua, Color.Black))
            };

            // Renders each item with own style
            AnsiConsole.Write(new Rows(rows));

            string command = Console.ReadLine();

            switch (command)
            {
                case "0":
                    Console.WriteLine("\nGoodbye!\n");
                    closeApp = true;
                    Environment.Exit(0);
                    break;
                case "1":
                    Console.WriteLine("Manage Stacks");
                    ManageStacks.GetManageStacks();
                    break;
                case "2":
                    Console.WriteLine("Manage Flashcards");
                    ViewAllFlashcardsRegardlessOfStacks.GetViewAllFlashcardsRegardlessOfStacks();
                    break;
                case "3":
                    Console.WriteLine("Study");
                    Study.GetStudy();
                    break;
                case "4":
                    Console.WriteLine("View study session data");
                    ViewStudySessionData.GetViewStudySessionData();
                    break;                
                case "5":
                    Console.WriteLine("Delete a Stack");
                    DeleteStacks.GetDeleteStacks();
                    break;

                default:
                    Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                    break;
            }
        }
    }

   
}
