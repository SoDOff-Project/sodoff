using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace sodoff.Utils;

public class PrintInfo {
    public static void print() {
        Console.WriteLine("Press Ctrl + C to close the server");
    }
}