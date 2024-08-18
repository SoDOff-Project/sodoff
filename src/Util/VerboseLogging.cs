using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace sodoff.Utils;

public class VerboseLogging {
    public static void log() {
        Console.WriteLine("[INFO] Server is loading Assets");
        Console.WriteLine("[INFO] Loading api");
    }
    public static void ServerStartedMessage()
    {
        Console.WriteLine("Server is running");
        Console.WriteLine("You can run the game now");
    }
}