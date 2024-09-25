namespace Task
{
    public class Task
    {
        // the id of the task within the list
        public int ID;
        // the literal description of the task
        public string Description;
        // the timestamp for when the task was created
        public DateTime CreationTimeStamp;
        // bool that is true if the task is completed; completion status
        public bool Completed;

        // constuctor for creating an existing task
        // usually used for ingesting multiple tasks via TaskManager.ReadCSV
        public Task(int ID, string Description, DateTime TimeStamp, bool completion)
        {
            this.ID = ID;
            this.Description = Description;
            CreationTimeStamp = TimeStamp;
            Completed = completion;
        }

        // constructor for creating a new task
        // TODO: revisit for potential dynamic ID addition
        public Task(int ID, string Description)
        {
            this.ID = ID;
            this.Description = Description;
            CreationTimeStamp = DateTime.Now;
            Completed = false;
        }

        // mark a task as complete
        public void Complete()
        {
            Completed = true;
        }

    }
}