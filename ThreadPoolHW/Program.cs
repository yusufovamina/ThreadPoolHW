using System;
using System.Collections.Generic;
using ThreadPoolHW;

class Program
{
      static  List<Student> students = new List<Student>();
    static void Main()
    {
        
        Random random = new Random();
        
        for (int i = 0; i < 50; i++)
        {
            students.Add(new Student
            {

                StudentNumber = random.Next(100,999),
                Name = string.Empty,
                Surname = string.Empty
            });
        }


        Timer timer = new Timer(ShowThreadPoolInfo, null, 1000, 1000);
        Task[] tasks = new Task[30];
        TaskScheduler scheduler = null;
        scheduler = TaskScheduler.Default;

        //int numb=765;
        //Console.WriteLine("Enter student number: ");
        //numb = Convert.ToInt32(Console.ReadLine());
        for (int i = 0; i < tasks.Length; i++)
        {
            tasks[i] = new Task(() => {
                Thread.Sleep(2000);
                SearchStudent(0);
                ShowLoadingProgressCallback(0);
            });
            tasks[i].Start();

        }
        Task.WaitAll(tasks);
        Thread.Sleep(2000);
        timer.Dispose();

        Console.ReadLine();

    }

    static void ShowLoadingProgressCallback(object _)
    {
        Console.Write("Loading ");
        for (int i = 0; i < 20; i++)
        {
            Console.Write("|");
            Thread.Sleep(100); 
        }
        Console.WriteLine();
        Console.WriteLine($"Loading task ID: {Task.CurrentId}, Thread ID: {Thread.CurrentThread.ManagedThreadId}");
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
        int studentNumber = 457;
        Console.WriteLine($"Search task ID: {Task.CurrentId}, Thread ID: {Thread.CurrentThread.ManagedThreadId}");

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