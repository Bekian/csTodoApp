using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Task;
using CSTodoApp1;

class Project
{

    public CSTodoApp1.TaskManager tasks = new("data.csv");

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
                            // no correct args were used, throw error message
                            Console.WriteLine($"args \"{args[1]}\" not valid, try again. Use the help command for correct usage. e.g. `help list`");
                            break;
                    }
                }
                // list tasks when no args are provided
                tasks.ListTasks();
                break;
            case "add":
                if (args.Length < 2)
                {
                    Console.WriteLine("No task description provided, try again. Use the help command or correct usage. e.g. `help add`");
                    break;
                }
                // join the string args and add the task
                tasks.AddTask(string.Join(" ", args[1..]));
                break;
            case "complete":
                // validate the ID
                int ID;
                if (args.Length == 2)
                {
                    // try to parse the ID, if it succeeds use that to complete the task
                    var validID = int.TryParse(args[1], out ID);
                    // otherwise, throw an error message here
                    if (!validID)
                    {
                        Console.WriteLine($"Invalid ID provided: {args[1]}, try again.");
                        break;
                    }
                    tasks.CompleteTask(ID - 1);
                }
                break;
            case "delete":
                // validate the ID
                // TODO: see if this declaration can be moved
                int ID2;
                if (args.Length == 2)
                {
                    // try to parse the ID, if it succeeds use that to delete the task
                    var validID = int.TryParse(args[1], out ID2);
                    // otherwise, throw an error message here
                    if (!validID)
                    {
                        Console.WriteLine($"Invalid ID provided: {args[1]}, try again.");
                        break;
                    }
                    tasks.DeleteTask(ID2 - 1);
                }
                break;
            case "help":
                // if there are more than 2 args we know the help command was used with an arg, and we can send that to the function
                if (args.Length >= 2) { CSTodoApp1.TaskManager.Help(args[1..]); }
                // call Help method from TaskManager 
                else { CSTodoApp1.TaskManager.Help(); }
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
            // process cli args here
            project.ProcessArgs(args);
        }
        else
        {
            // handle repl
            while (true)
            {
                Console.WriteLine("enter an input or press enter to exit:");
                var userInput = Console.ReadLine();
                if (userInput == null || userInput == "")
                {
                    Console.WriteLine("No input provided, exiting...");
                    break;
                }
                // process repl args
                project.ProcessArgs(userInput.Split(" "));
            }
        }

        project.tasks.SaveCSV();
    }
}
