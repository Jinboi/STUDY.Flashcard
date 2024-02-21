using Spectre.Console;
using System.Data.SqlClient;
using System;
using System.Globalization;

namespace Flash;

class Program
{
    static string connectionString = "Data Source=(LocalDB)\\LocalDBDemo;Integrated Security=True";
    static void Main(string[] args)
    {
        ConnectFlashCardDatabase.CreateFlashCardDatabase(connectionString);
        ConnectStacksDatabase.CreateStacksDatabase(connectionString);
        Console.ReadLine();
        MainMenu.GetMainMenu();
    }
}



