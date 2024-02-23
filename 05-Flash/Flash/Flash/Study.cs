
using Spectre.Console;
using System.Data.SqlClient;

namespace Flash
{
    internal class Study
    {
        static string connectionString = "Data Source=(LocalDB)\\LocalDBDemo;Integrated Security=True";
        internal static void GetStudy()
        {
            Console.Clear();
            AnsiConsole.Write
                    (
                        new FigletText("Study")
                            .LeftJustified()
                            .Color(Color.Navy)
                    );

            var panel = new Panel("What Would You Like to Do?");

            AnsiConsole.Write(
                    new Panel(panel)
                        .Header("")
                        .Collapse()
                        .RoundedBorder()
                        .BorderColor(Color.Navy));


            using (SqlConnection connection = new SqlConnection(connectionString))
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

                Console.WriteLine("Insert the Stack_Priamry_Id of the stack you want to study");

                string studyStackString = Console.ReadLine();
                int studyStack;

                if (Int32.TryParse(studyStackString, out studyStack))
                {

                    Console.WriteLine($"Selected Stack_Primary_Id: {studyStack}");
                }
                else
                {
                    Console.WriteLine("Unable to convert the string to an integer.");
                }

                Console.ReadLine();




                // Check if 'Stacks' table exists
                string checkTableQuery =
                    @"SELECT COUNT(*) 
                        FROM INFORMATION_SCHEMA.TABLES 
                        WHERE TABLE_NAME = 'Study'";

                using (SqlCommand checkTableCommand = new SqlCommand(checkTableQuery, connection))
                {
                    int tableCount = Convert.ToInt32(checkTableCommand.ExecuteScalar());
                    if (tableCount == 0)
                    {
                        // If 'Stacks' table doesn't exist, create it
                        string createStacksTableQuery =
                            @"CREATE TABLE Study (
                                Study_Primary_Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                                Date DATE NOT NULL,
                                Score FLOAT NOT NULL,
                                Stack_Primary_Id INT FOREIGN KEY REFERENCES Stacks(Stack_Primary_Id)
                            )";
                        using (SqlCommand createTableCommand = new SqlCommand(createStacksTableQuery, connection))
                        {
                            createTableCommand.ExecuteNonQuery();
                            Console.WriteLine("Table 'Study' created successfully.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Table 'Study' already exists in database 'DataBaseFlashCard'.");
                    }
                }
                Console.ReadLine();

                Console.WriteLine("Showing first flashcard and so on");


                int totalNumberOfFlashCardsInThatStack;

                string totalNumberOfFlashCardsInThatStackString =
                            @$"SELECT COUNT(*) 
                            FROM Flashcards
                            WHERE Stack_Primary_ID = '{studyStack}'";

                using (SqlCommand totalNumberOfFlashCardsInThatStackStringCommand = new SqlCommand(totalNumberOfFlashCardsInThatStackString, connection))
                {
                    totalNumberOfFlashCardsInThatStack = (int)totalNumberOfFlashCardsInThatStackStringCommand.ExecuteScalar();
                    Console.WriteLine($"This is the total number = {totalNumberOfFlashCardsInThatStack}");
                }

                for (int studyFlashcardCounter = 1; studyFlashcardCounter <= totalNumberOfFlashCardsInThatStack; studyFlashcardCounter++)
                {

                    // Query to select a limited number of rows from the Flashcards table for the current stack
                    string selectQuery =
                         $@"SELECT Flashcard_Primary_Id, Front, Back, Stack_Primary_Id
                                       FROM Flashcards 
                                       WHERE Stack_Primary_Id = @studyFlashcardCounter";

                    List<FlashcardDto> flashcards = new List<FlashcardDto>();

                    using (SqlCommand command = new SqlCommand(selectQuery, connection))
                    {
                        // Add parameters
                        command.Parameters.AddWithValue("@studyFlashcardCounter", studyFlashcardCounter);

                        // Execute the command and retrieve the data
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Check if any rows are returned
                            if (reader.HasRows)
                            {
                                // Loop through each row and create DTOs
                                while (reader.Read())
                                {
                                    FlashcardDto flashcard = new FlashcardDto
                                    {
                                        Flashcard_Primary_Id = reader.GetInt32(0),
                                        Front = reader.GetString(1),
                                        Back = reader.GetString(2),
                                        Stack_Primary_Id = reader.GetInt32(3)
                                    };
                                    flashcards.Add(flashcard);
                                }

                                RenumberFlashcards.GetRenumberFlashcards(flashcards);


                                int correctAnswer = 0;
                                int wrongAnswer = 0;
                                int totalQuestions = 0;
                                // Display flashcards
                                foreach (var flashcard in flashcards)
                                {
                                    Console.WriteLine($"Front: {flashcard.Front}");
                                    Console.WriteLine("What's the back");

                                    string answer = Console.ReadLine();

                                    if (answer == flashcard.Back)
                                    {
                                        Console.WriteLine("Correct!");
                                        correctAnswer++;
                                        totalQuestions++;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Incorrect!");
                                        wrongAnswer++;
                                        totalQuestions++;
                                    }
                                }
                                Console.WriteLine($"Your score of this study session is: {correctAnswer} out of {totalQuestions}");
                            }

                            else
                            {
                                Console.WriteLine("No flashcards found.");
                            }
                        }
                    }

                }
                    
                //ADD TO scores to STUDY Stack

            }            
        }        
    }
}
