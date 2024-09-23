using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Task;
using TaskManager;

class Project
{
    static void Main(string[] args)
    {
        var tasks = new TaskManager.TaskManager("data.csv");
        Console.WriteLine("enter an input:");
        // var input = Console.ReadLine();
        // Console.WriteLine($"user input: {input:g}");
        // switch (input)
        // {
        //     case "list":
        //         Console.WriteLine("you used the list command!");
        //         break;
        //     case "add":
        //         Console.WriteLine("you used the add command!");
        //         break;
        //     default:
        //         break;
        // }
    }
}
