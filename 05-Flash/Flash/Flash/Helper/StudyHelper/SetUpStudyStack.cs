namespace Flash.Helper.StudyHelper;
internal class SetUpStudyStack
{
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
}
