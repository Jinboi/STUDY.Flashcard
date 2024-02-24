
using Spectre.Console;
using System.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Flash
{
    internal class Study
    {
        static string connectionString = "Data Source=(LocalDB)\\LocalDBDemo;Integrated Security=True";
        internal static void GetStudy()
        {
            Console.Clear();
            AnsiConsole.Writexw
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

            }
            

            int studyStack = SetUpStudyStack();
            CreateStudyTable(studyStack);
            (int correctAnswer, int totalQuestions) = ShowFlashcardToStudy(studyStack);
            RecordToStudy(studyStack, correctAnswer, totalQuestions);
        }

        internal static int SetUpStudyStack()
        {
            Console.WriteLine("Insert the Stack_Priamry_Id of the stack you want to study");

            string studyStackString = Console.ReadLine();
            int studyStack;

            if (Int32.TryParse(studyStackString, out studyStack))
            {
                Console.WriteLine($"Selected Stack_Primary_Id: {studyStack}");                
            }
            else
            {
                Console.WriteLine("something's wrong");
            }                
            return studyStack;            
        }

        internal static void CreateStudyTable(int studyStack)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.ChangeDatabase("DataBaseFlashCard");

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
                                Score NVARCHAR(50) NOT NULL,
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
            }

        }

        internal static (int, int) ShowFlashcardToStudy(int studyStack)
        {
            int correctAnswer = 0;
            int wrongAnswer = 0;
            int totalQuestions = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.ChangeDatabase("DataBaseFlashCard");                

                
                //get the total number of flashcards


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

                

                // Query to select a limited number of rows from the Flashcards table for the current stack
                string selectQuery =
                        $@"SELECT Flashcard_Primary_Id, Front, Back, Stack_Primary_Id
                                        FROM Flashcards 
                                        WHERE Stack_Primary_Id = @studyStackId";

                List<FlashcardDto> flashcards = new List<FlashcardDto>();

                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    // Add parameters
                    command.Parameters.AddWithValue("@studyStackId", studyStack);

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
                            Console.WriteLine($"you got {correctAnswer} correct out of {totalQuestions}");

                        }
                    }
                    
                }
            }
            return (correctAnswer, totalQuestions);
        }



        internal static void RecordToStudy(int studyStack, int correctAnswer, int totalQuestions)
        {

            Console.WriteLine($"Your score of this study session is: {correctAnswer} out of {totalQuestions}");

            //ADD TO scores to STUDY Stack

            // Calculate score as an float
            float floatScore = (float)correctAnswer / totalQuestions;

            string score = floatScore.ToString();


            DateTime date = DateTime.Now;

            Console.WriteLine($"score is {score}");
            Console.WriteLine($"date is {date}");


            Console.ReadLine();



            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                connection.ChangeDatabase("DataBaseFlashCard");



                // Insert flashcard into 'Flashcards' table
                string insertStudyQuery =
                @"INSERT INTO Study (Date, Score, Stack_Primary_Id)
                            VALUES (@Date, @Score, @StackPrimaryId)";

                using (SqlCommand insertStudyCommand = new SqlCommand(insertStudyQuery, connection))
                {
                    // Add parameters
                    insertStudyCommand.Parameters.AddWithValue("@Date", date);
                    insertStudyCommand.Parameters.AddWithValue("@Score", score);
                    insertStudyCommand.Parameters.AddWithValue("@StackPrimaryId", studyStack);

                    // Execute the command
                    insertStudyCommand.ExecuteNonQuery();
                    Console.WriteLine("Recorded StudySession Data successfully.");
                }


            }



        }
    }
}


                           


            
        




                     

