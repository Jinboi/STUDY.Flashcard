
using System.Data.SqlClient;

namespace Flash;
internal static class StudyHelper
{

    internal static void GetCreateStudyTable(int studyStack)
    {
        using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
        {
            connection.Open();
            connection.ChangeDatabase("DataBaseFlashCard");

            string checkTableQuery =
            @"SELECT COUNT(*) 
                        FROM INFORMATION_SCHEMA.TABLES 
                        WHERE TABLE_NAME = 'Study'";

            using (SqlCommand checkTableCommand = new SqlCommand(checkTableQuery, connection))
            {
                int tableCount = Convert.ToInt32(checkTableCommand.ExecuteScalar());
                if (tableCount == 0)
                {
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


    internal static void GetRecordToStudy(int studyStack, int correctAnswer, int totalQuestions)
    {

        Console.WriteLine($"Your score of this study session is: {correctAnswer} out of {totalQuestions}");

        float floatScore = (float)correctAnswer / totalQuestions;

        string score = floatScore.ToString();

        DateTime date = DateTime.Now;

        Console.WriteLine($"score is {score}");
        Console.WriteLine($"date is {date}");

        Console.ReadLine();

        using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
        {
            connection.Open();
            connection.ChangeDatabase("DataBaseFlashCard");

            string insertStudyQuery =
            @"INSERT INTO Study (Date, Score, Stack_Primary_Id)
                            VALUES (@Date, @Score, @StackPrimaryId)";

            using (SqlCommand insertStudyCommand = new SqlCommand(insertStudyQuery, connection))
            {
                insertStudyCommand.Parameters.AddWithValue("@Date", date);
                insertStudyCommand.Parameters.AddWithValue("@Score", score);
                insertStudyCommand.Parameters.AddWithValue("@StackPrimaryId", studyStack);

                insertStudyCommand.ExecuteNonQuery();
                Console.WriteLine("Recorded StudySession Data successfully.");
            }
        }
    }


    internal static int GetSetUpStudyStack()
    {
        Console.WriteLine("Insert the Stack_Priamry_Id of the stack you want to study");

        string studyStackString = Console.ReadLine();
        int studyStack;

        if (int.TryParse(studyStackString, out studyStack))
        {
            Console.WriteLine($"Selected Stack_Primary_Id: {studyStack}");
        }
        else
        {
            Console.WriteLine("something's wrong");
        }
        return studyStack;
    }


    internal static (int, int) GetShowFlashcardToStudy(int studyStack)
    {
        int correctAnswer = 0;
        int wrongAnswer = 0;
        int totalQuestions = 0;

        using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
        {
            connection.Open();
            connection.ChangeDatabase("DataBaseFlashCard");

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

            string selectQuery =
                    $@"SELECT Flashcard_Primary_Id, Front, Back, Stack_Primary_Id
                                        FROM Flashcards 
                                        WHERE Stack_Primary_Id = @studyStackId";

            List<FlashcardDto> flashcards = new List<FlashcardDto>();

            using (SqlCommand command = new SqlCommand(selectQuery, connection))
            {
                command.Parameters.AddWithValue("@studyStackId", studyStack);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
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

                        Renumber.GetRenumberFlashcards(flashcards);

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

}
