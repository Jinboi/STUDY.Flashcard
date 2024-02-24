
namespace Flash
{
    internal class RenumberStudy
    {
        internal static void GetRenumberStudys(List<StudyDto> studys)
        {
            for (int i = 0; i < studys.Count; i++)
            {
                studys[i].Study_Primary_Id = i + 1;
            }
        }
    }
}
