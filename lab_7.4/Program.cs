using System;
using System.Collections.Generic;

class TaskScheduler<TTask, TPriority>
{
    // Делегат для виконання завдань
    public delegate void TaskExecution(TTask task);
    private SortedDictionary<TPriority, Queue<TTask>> taskQueue = new SortedDictionary<TPriority, Queue<TTask>>();
    private TaskExecution taskExecutor;

    // Конструктор який приймає делегат для виконання завдань
    public TaskScheduler(TaskExecution executor)
    {
        taskExecutor = executor ?? throw new ArgumentNullException(nameof(executor));
    }

    public void AddTask(TTask task, TPriority priority)
    {
        if (!taskQueue.ContainsKey(priority))
        {
            taskQueue[priority] = new Queue<TTask>();
        }

        taskQueue[priority].Enqueue(task);
    }

    // Метод для виконання завдання з найвищим пріоритетом
    public void ExecuteNext()
    {
        if (taskQueue.Count > 0)
        {
            var highestPriority = taskQueue.Keys.Max();
            var nextTask = taskQueue[highestPriority].Dequeue();
            if (taskQueue[highestPriority].Count == 0)
            {
                taskQueue.Remove(highestPriority);
            }
            taskExecutor(nextTask);
        }
        else
        {
            Console.WriteLine("No tasks in the queue.");
        }
    }
}

class Program
{
    static void Main()
    {
        // Створення планувальника завдань для рядків та цілих чисел
        TaskScheduler<string, int> scheduler = new TaskScheduler<string, int>(ExecuteTask);
        while (true)
        {
            Console.WriteLine("Enter task (or 'exit' to quit):");
            string task = Console.ReadLine();

            if (task.ToLower() == "exit")
            {
                break;
            }

            Console.WriteLine("Enter priority:");
            // Обробка введення для пріоритету
            if (int.TryParse(Console.ReadLine(), out int priority))
            {
                scheduler.AddTask(task, priority);
            }
            else
            {
                Console.WriteLine("Invalid priority. Please enter an integer.");
            }
        }

        // Метод для виконання завдання з найвищим пріоритетом
        scheduler.ExecuteNext();
    }

    // Метод для виконання завдань рядків
    static void ExecuteTask(string task)
    {
        Console.WriteLine($"Executing task: {task}");
    }
}
