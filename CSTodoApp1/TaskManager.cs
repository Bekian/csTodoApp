using Task;
using System.Collections.Generic;

namespace TaskManager
{
    class TaskManager
    {
        public List<Task.Task> tasks = new List<Task.Task>();

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
                    short taskId = 0;
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
                                taskId = short.Parse(value);
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
                        Console.WriteLine($"ID: {taskId:g}, Description: {taskDescription:g}, Timestamp: {taskDateTime:g}, Completed: {taskCompletion:g}"); // debug, keep till prod
                        tasks.Add(new Task.Task(taskId, taskDescription, taskDateTime, taskCompletion));
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while reading the CSV: {ex.Message}");
            }

        }

        public TaskManager(string csvFilePath)
        {
            ReadCSV(csvFilePath);
        }
    }
}