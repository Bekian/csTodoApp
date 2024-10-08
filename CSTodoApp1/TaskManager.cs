using Task;
using System.Collections.Generic;

namespace CSTodoApp1
{
    class TaskManager
    {
        public List<Task.Task> tasks = [];
        private string FilePath = "";

        private void ReadCSV(string filePath)
        {
            try
            {
                using var reader = new StreamReader(filePath);
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line == null)
                    {
                        // when the line is null then let the user know
                        Console.WriteLine("CSV Read error: readlines found null line, check source for error");
                        return;
                    }
                    var values = line.Split(",");

                    var i = 0;
                    // Declare variables outside the loop
                    int taskId = 0;
                    string taskDescription = string.Empty;
                    DateTime taskDateTime = DateTime.MinValue;
                    bool taskCompletion = false;

                    foreach (var value in values)
                    {
                        if (value == "ID" || value == "Task" || value == "Date" || value == "Completed") { continue; }

                        switch (i)
                        {
                            case 0:
                                // `i` might not always equal the ID so we need to parse the value, 
                                // like in the event where a task in the middle of the list is deleted
                                taskId = int.Parse(value);
                                break;
                            case 1:
                                taskDescription = value;
                                break;
                            case 2:
                                // convert the unix time seconds value to a datetime value (which has convenient reading features)
                                taskDateTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(value)).DateTime;
                                break;
                            case 3:
                                taskCompletion = bool.Parse(value);
                                break;
                            default:
                                // invalid i value
                                // this shouldnt happen, but this is a convenient place for us to check for out of bounds without needing another conditional statement
                                Console.WriteLine($"CSV Read error: index out of range, Verify your source file is the correct structure.\nindex value: {i}");
                                // maybe throw exception here?
                                break;
                        }
                        i++;
                    }
                    if (i == 4)
                    {
                        // Console.WriteLine($"ID: {taskId:g}, Description: {taskDescription:g}, Timestamp: {taskDateTime:g}, Completed: {taskCompletion:g}"); // debug, keep till prod
                        tasks.Add(new Task.Task(taskId, taskDescription, taskDateTime, taskCompletion));
                    } // dont need anything here cause we've already logged an error message
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while reading the CSV: {ex.Message}");
            }
            FilePath = filePath;
        }

        // lists all tasks in the tasklist
        // this method only lists incomplete tasks
        public void ListTasks()
        {
            Console.WriteLine();
            foreach (var task in tasks)
            {
                if (!task.Completed)
                {
                    Console.WriteLine($"ID: {task.ID:g}, Description: {task.Description:g}, Timestamp: {task.CreationTimeStamp:g}, Completed: {task.Completed:g}");
                }
            }
            Console.WriteLine();
        }

        // list a singular task by ID
        public void ListTasks(int ID)
        {
            // look for a specific task with the provided ID
            var task = tasks.FirstOrDefault(t => t.ID == ID);
            // if found then log task
            if (task != null)
            {
                Console.WriteLine();
                Console.WriteLine($"ID: {task.ID:g}, Description: {task.Description:g}, Timestamp: {task.CreationTimeStamp:g}, Completed: {task.Completed:g}");
                Console.WriteLine();
            }
            else
            {
                // else log error
                Console.WriteLine($"Task with ID {ID} not found.");
            }
        }

        // lists all tasks 
        public void ListTasks(bool all)
        {
            // in the case that the bool provided is false, we call the default ListTasks method and return
            if (!all) { ListTasks(); return; }

            Console.WriteLine();
            // print each task in the list
            foreach (var task in tasks)
            {
                Console.WriteLine($"ID: {task.ID:g}, Description: {task.Description:g}, Timestamp: {task.CreationTimeStamp:g}, Completed: {task.Completed:g}");
            }
            Console.WriteLine();
        }

        // adds a task to the tasklist
        public void AddTask(string description)
        {
            // get the last task ID
            var lastTaskID = tasks.Last().ID + 1;
            // create the new task
            var newTask = new Task.Task(lastTaskID, description);
            // add it to the tasklist
            tasks.Add(newTask);
        }

        // completes a task given an ID
        public void CompleteTask(int ID)
        {
            // find the first task that matches the given ID in the list or return null
            var task = tasks.FirstOrDefault(t => t.ID == ID);
            if (task != null)
            {
                if (!task.Completed)
                {
                    Console.WriteLine($"Task number {ID} {task.Description} completed.");
                    task.Complete(); // complete the found task
                    return;
                }
                Console.WriteLine($"Task number {ID} {task.Description} already completed.");
            }
            else
            {
                Console.WriteLine($"Task with ID {ID} not found.");
            }
        }

        // deletes a task from the list via ID
        public void DeleteTask(int ID)
        {
            // use the same method as above to remove an ID if found
            var task = tasks.FirstOrDefault(t => t.ID == ID);
            if (task != null)
            {
                Console.WriteLine($"Task: {task.Description} deleted.");
                tasks.RemoveAt(ID - 1); ; // delete the found task
            }
            else
            {
                Console.WriteLine($"Task with ID {ID} not found.");
            }
        }

        // logs the command usage for all the commands
        public static void Help()
        {
            Console.WriteLine("Commands:");
            Console.WriteLine("add - add a new task to the list");
            Console.WriteLine("list - list all tasks");
            Console.WriteLine("complete - mark a task as complete");
            Console.WriteLine("delete - delete a task");
            Console.WriteLine("help - list all commands");
        }

        // logs specific command help messages based on provided input
        public static void Help(string flag)
        {
            switch (flag)
            {
                case "add":
                    Console.WriteLine("To add a new task, type 'add' followed by the task description.");
                    Console.WriteLine("Example: 'add Buy milk' to add a task to buy milk.");
                    break;
                case "list":
                    Console.WriteLine("To list tasks, type `list`");
                    Console.WriteLine("Example: `list` will list all incomplete tasks by default.");
                    Console.WriteLine("Optionally, use the `--item` or shorthand `-i` flag followed by the ID to list a single task.");
                    Console.WriteLine("e.g. `list -i 2` will list the second task.");
                    Console.WriteLine("Or, use the `--all` or shorthand `-a` flag to list all tasks.");
                    break;
                case "complete":
                    Console.WriteLine("To mark a task as complete, type 'complete' followed by the task ID.");
                    Console.WriteLine("Example: 'complete 1' to mark task with ID 1 as complete.");
                    break;
                case "delete":
                    Console.WriteLine("To delete a task, type 'delete' followed by the task ID.");
                    Console.WriteLine("Example: 'delete 1' to delete task with ID 1.");
                    break;
                case "help":
                    Console.WriteLine("To use the help command, type 'help' followed by the command you need help with.");
                    Console.WriteLine("Example: 'help add' for information on how to add a task.");
                    Console.WriteLine("If no specific command is provided, a list of all commands will be displayed.");
                    break;
                default:
                    Console.WriteLine($"Invalid help flag provided, flag provided: {flag}\n Please provide the command with no additional flags or arguments.");
                    Help("help");
                    break;
            }
        }

        // alias to help when string args are provided 
        // it joins the args to a single string then calls the function for the user
        public static void Help(string[] args)
        {
            Help(string.Join(" ", args[0..]));
        }

        // a function that saves the tasklist into csv format
        public void SaveCSV()
        {
            try
            {
                using var writer = new StreamWriter(FilePath);
                writer.WriteLine("ID,Task,Date,Completed");
                foreach (var task in tasks)
                {
                    // convert the dateTime value to unix time in seconds (this program parses it as unix time in seconds)
                    // also subtract 5 hours due to conversion issue
                    long unixTimeSeconds = new DateTimeOffset(task.CreationTimeStamp.AddHours(-5)).ToUnixTimeSeconds();
                    writer.WriteLine($"{task.ID},{task.Description},{unixTimeSeconds},{task.Completed}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while saving the CSV: {ex.Message}");
            }
        }

        // read provided csv file when the task manager is created
        public TaskManager(string csvFilePath)
        {
            ReadCSV(csvFilePath);
        }
    }
}