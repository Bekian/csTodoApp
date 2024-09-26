using Task;
using System.Collections.Generic;

namespace TaskManager
{
    class TaskManager
    {
        public List<Task.Task> tasks = new List<Task.Task>();
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
                        Console.WriteLine("readlines found null line, check source for error");
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
                        if (value == "ID" || value == "Task" || value == "Date" || value == "Completed")
                        {
                            continue;
                        }

                        if (i >= 4)
                        {
                            Console.WriteLine($"CSV Read error: index out of range, Verify your source file is the correct structure.\nindex value: {i}");
                        }
                        // Console.WriteLine($"value: {value:g}, index: {i:g}");
                        switch (i)
                        {
                            case 0:
                                taskId = int.Parse(value);
                                break;
                            case 1:
                                taskDescription = value;
                                break;
                            case 2:
                                taskDateTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(value)).DateTime;
                                break;
                            case 3:
                                taskCompletion = bool.Parse(value);
                                break;
                            default:
                                // invalid i value
                                Console.WriteLine($"CSV Read error: index out of range, Verify your source file is the correct structure.\nindex value: {i}");
                                break;
                        }
                        i++;
                    }
                    if (i == 4)
                    {
                        // Console.WriteLine($"ID: {taskId:g}, Description: {taskDescription:g}, Timestamp: {taskDateTime:g}, Completed: {taskCompletion:g}"); // debug, keep till prod
                        tasks.Add(new Task.Task(taskId, taskDescription, taskDateTime, taskCompletion));
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while reading the CSV: {ex.Message}");
            }
            FilePath = filePath;
        }

        //TODO: implement passing vars for listing a single item or all items and change default to only listing incomlpete items.
        // lists all tasks in the tasklist
        public void ListTasks()
        {
            foreach (var task in tasks)
            {
                if (task.Completed)
                {
                    continue;
                }
                else
                {
                    Console.WriteLine($"ID: {task.ID:g}, Description: {task.Description:g}, Timestamp: {task.CreationTimeStamp:g}, Completed: {task.Completed:g}");
                }
            }
            Console.WriteLine();
        }

        // list a singular task by ID
        public void ListTasks(int ID)
        {
            var task = tasks[ID - 1]; // this needs to be validated
            Console.WriteLine($"ID: {task.ID:g}, Description: {task.Description:g}, Timestamp: {task.CreationTimeStamp:g}, Completed: {task.Completed:g}");
            Console.WriteLine();
        }

        // lists all tasks 
        public void ListTasks(bool all)
        {
            if (all)
            {
                foreach (var task in tasks)
                {
                    Console.WriteLine($"ID: {task.ID:g}, Description: {task.Description:g}, Timestamp: {task.CreationTimeStamp:g}, Completed: {task.Completed:g}");

                }
                Console.WriteLine();
            }
            else
            {
                ListTasks();
            }
        }

        // adds a task to the tasklist
        public void AddTask(string description)
        {
            // change this to use the GetLastID func to calculate the correct ID
            var newTask = new Task.Task(tasks.Count + 1, description);
            tasks.Add(newTask);
        }

        // completes a task given an ID
        public void CompleteTask(int ID)
        {
            tasks[ID - 1].Complete();
        }

        // deletes a task from the list via ID
        public void DeleteTask(int ID)
        {
            tasks.RemoveAt(ID - 1);
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

        // a function that saves the tasklist into csv format
        public void SaveCSV()
        {
            try
            {
                using (var writer = new StreamWriter(FilePath))
                {
                    writer.WriteLine("ID,Task,Date,Completed");
                    foreach (var task in tasks)
                    {
                        // convert the dateTime value to unix time in seconds (this program parses it as unix time in seconds)
                        long unixTimeSeconds = new DateTimeOffset(task.CreationTimeStamp).ToUnixTimeSeconds();
                        writer.WriteLine($"{task.ID},{task.Description},{unixTimeSeconds},{task.Completed}");
                    }
                }
                Console.WriteLine($"Tasks saved to {FilePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while saving the CSV: {ex.Message}");
            }
        }

        public TaskManager(string csvFilePath)
        {
            ReadCSV(csvFilePath);
            // list tasks
            // ListTasks();
            // add a task
            // AddTask("task sample");
            // CompleteTask(5);
            // DeleteTask(5);
        }
    }
}