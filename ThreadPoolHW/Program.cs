using System;
using System.Collections.Generic;
using ThreadPoolHW;

class Program
{
    static TaskScheduler scheduler = null;
    
      static  List<Student> students = new List<Student>();
    static void Main()
    {
        
        Random random = new Random();
        
        for (int i = 0; i < 50; i++)
        {
            students.Add(new Student
            {
                StudentNumber = random.Next(1,50),
                Name = string.Empty,
                Surname = string.Empty
            });
        }


        Timer timer = new Timer(ShowThreadPoolInfo, null, 1000, 1000);
        scheduler = TaskScheduler.Default;

        Task[] loadingTasks = new Task[30];
        Task[] searchTasks = new Task[30];

        Task.Factory.StartNew(() => RunTasks(), CancellationToken.None, TaskCreationOptions.None, scheduler);


        
        Thread.Sleep(2000);
        timer.Dispose();

        Console.ReadLine();

    }

    static void RunTasks()
    {
        for (int i = 0; i < 30; i++)
        {
            Task.Factory.StartNew(() => ShowLoadingProgress(0), CancellationToken.None, TaskCreationOptions.None, scheduler).Wait();
            Task.Factory.StartNew(() => SearchStudent(0), CancellationToken.None, TaskCreationOptions.None, scheduler).Wait();
        }
    }
    static void ShowLoadingProgress(object _)
    {
        Console.Write("Loading ");
        string a = "";
        for (int i = 0; i < 20; i++)
        {
            a+="|";
            Thread.Sleep(100); 
        }
        Console.WriteLine(a);
        Console.WriteLine($"Loading task ID: {Task.CurrentId}, Thread ID: {Thread.CurrentThread.ManagedThreadId}, iz pula potokov {Thread.CurrentThread.IsThreadPoolThread}");
    }

    public static void ShowThreadPoolInfo(object _)
    {
        ThreadPool.GetAvailableThreads(out int threads, out int completionsPorts);
        ThreadPool.GetMaxThreads(out int maxThreads, out int maxCompletionsPorts);

        Console.WriteLine($"         Worker Threads - [{threads} : {maxThreads}]");
        Console.WriteLine($"         Completion Threads - [{completionsPorts} : {maxCompletionsPorts}]");
    }


static void SearchStudent(object _)
    {
        int studentNumber = 458;
        Console.WriteLine($"Search task ID: {Task.CurrentId}, Thread ID: {Thread.CurrentThread.ManagedThreadId}, iz pula potokov {Thread.CurrentThread.IsThreadPoolThread}");

        foreach (var student in students)
        {
            Thread.Sleep(1000); 
            if (student.StudentNumber == studentNumber)
            {
                Console.WriteLine($"Student found in thread: {Thread.CurrentThread.ManagedThreadId}");
                return;
            }
        }

        Console.WriteLine("Student not found.");
    }

}