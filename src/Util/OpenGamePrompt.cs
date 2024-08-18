using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace sodoff.Utils;

public class OpenGamePrompt {
    public static void prompt() {
        string key;
        Console.WriteLine("Do you want to open the game");
        Console.WriteLine("yes or no");
        key = Console.ReadLine();
        if (key == "yes")
        {
            Console.WriteLine("Opening game");
            Process.Start(Directory.GetParent(Environment.CurrentDirectory) + "/Client/" + "DOMain.exe");
        }
        if (key == "no")
        {
            Console.WriteLine("You can manually open the game now");
        }
    }
}