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
                            // no correct args were used, throw error message
                            Console.WriteLine($"args \"{args[1]}\" not valid, try again. Use the help command for correct usage. e.g. `help list`");
                            break;
                    }
                }
                else
                {
                    // list tasks when no args are provided
                    tasks.ListTasks();
                }
                Console.WriteLine("you used the list command!"); // leave till prod
                break;
            case "add":
                Console.WriteLine(args.Length);
                if (args.Length < 2)
                {
                    Console.WriteLine("No task description provided, try again. Use the help command or correct usage. e.g. `help add`");
                    break;
                }
                // provided a "validated" task description so we join the string args and add the task
                var newTaskDescription = string.Join(" ", args[1..]);
                tasks.AddTask(newTaskDescription);
                break;
            case "complete":
                // validate the ID
                int ID;
                if (args.Length == 2)
                {
                    // try to parse the ID, if it succeeds use that to complete the task
                    var _ = int.TryParse(args[1], out ID);
                    // TODO: otherwise, throw an error message here
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
                    var _ = int.TryParse(args[1], out ID2);
                    // TODO: otherwise, throw an error message here
                    tasks.DeleteTask(ID2 - 1);
                }
                break;
            case "help":
                // TODO: refactor this to utilize help args
                TaskManager.TaskManager.Help(); // call Help method from TaskManager
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
