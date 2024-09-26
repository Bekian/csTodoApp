using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Task;
using TaskManager;

class Project
{

    public TaskManager.TaskManager tasks = new("data.csv");

    // continue working on building out parsing, condense if possible
    void ProcessArgs(string[] args)
    {
        switch (args[0])
        {
            case "list":
                if (args.Length > 1)
                {
                    switch (args[1])
                    {
                        case "--all":
                            // call taskmanager.list(-all) or whatever
                            tasks.ListTasks(true);
                            break;
                        case "-a":
                            // call taskmanager.list(-all) or whatever
                            tasks.ListTasks(true);
                            break;
                        case "--item":
                            // call taskmanager.list.id(id)
                            tasks.ListTasks(int.Parse(args[2]));
                            break;
                        case "-i":
                            // call taskmanager.ListTasks(id)
                            tasks.ListTasks(int.Parse(args[2]));
                            break;
                        default:
                            // no additional args were used, so we use the basic list function
                            tasks.ListTasks();
                            break;
                    }
                }
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
        Project project = new();
        if (args.Length >= 1)
        {
            Console.WriteLine("cli args picked");
            // process args here
            project.ProcessArgs(args);
        }
        else
        {
            Console.WriteLine("repl picked");
            var exitCondition = false;
            while (exitCondition == false)
            {
                Console.WriteLine("enter an input or press enter to exit:");
                var userInput = Console.ReadLine();
                // process args
                if (userInput == null || userInput == "")
                {
                    Console.WriteLine("No input provided, exiting.");
                    break;
                }
                project.ProcessArgs(userInput.Split(" "));
            }
        }

        // var input = Console.ReadLine();
        // Console.WriteLine($"user input: {input:g}");
        project.tasks.SaveCSV();
    }
}
