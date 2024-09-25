using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Task;
using TaskManager;

class Project
{

    // continue working on building out arg parsing
    static void ProcessArgs(string args)
    {
        switch (args)
        {
            case "list":
                Console.WriteLine("you used the list command!");
                break;
            case "add":
                Console.WriteLine("you used the add command!");
                break;
            default:
                break;
        }
    }

    // continue working on building out parsing, condense if possible
    static void ProcessArgs(string[] args)
    {
        switch (args[0])
        {
            case "list":
                Console.WriteLine("you used the list command!");
                break;
            case "add":
                Console.WriteLine("you used the add command!");
                break;
            default:
                break;
        }
    }

    static void Main(string[] args)
    {
        var tasks = new TaskManager.TaskManager("data.csv");

        if (args.Length >= 1)
        {
            Console.WriteLine("cli args picked");
            // process args here
            ProcessArgs(args);
        }
        else
        {
            Console.WriteLine("repl picked");
            var exitCondition = false;
            while (exitCondition == false)
            {
                Console.WriteLine("enter an input:");
                var userInput = Console.ReadLine();
                // process args
                if (userInput == null || userInput == "")
                {
                    // this should be unreachable
                    Console.WriteLine("No user input provided");
                    break;
                }
                ProcessArgs(userInput);
            }
        }

        // var input = Console.ReadLine();
        // Console.WriteLine($"user input: {input:g}");
        tasks.SaveCSV();
    }
}
