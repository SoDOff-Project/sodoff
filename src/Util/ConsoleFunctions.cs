using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Configuration;


namespace sodoff.Utils;

public class ConsoleFunctions {
    //function to log when the server start and when the server is ready
    public static void log(string Option, int AssetPort) {
        if (Option == "Server Running")
        {
            Console.WriteLine("Server is running at Asset Server: http://localhost:" + AssetPort + " and Api Server: http://localhost:5000");
            Console.WriteLine("You can start the game now");
            Console.WriteLine("Press Ctrl + C to close the server");
        }
    }
    // asks if user wants to automally open the game
    public static void prompt(string GamePath)
    {
        string key;
        //asks the user if the game client should be automally opened
        Console.WriteLine("Do you want to open the game");
        Console.WriteLine("Y/n");
        //reads the user input
        key = Console.ReadLine();
        //allows uppercase
        key.ToUpper();
        //checks for y in string then automally opens the game client
        if (key.StartsWith("y"))
        {
            //checks if GamePath isnt null to prevend errors
            if (GamePath != null)
            {
                //starts the game automally
                Console.WriteLine("Opening game");
                Process.Start(GamePath);
            }
        }
        //if user wants to open the game manually
        else
        {
            Console.WriteLine("You can manually open the game now");
        }
    }
}
