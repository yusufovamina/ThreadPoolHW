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
                StudentNumber = random.Next(1,50),
                Name = string.Empty,
                Surname = string.Empty
            });
        }


        Timer timer = new Timer(ShowThreadPoolInfo, null, 1000, 1000);
        Task[] tasks = new Task[30];
        Task[] tasks2 = new Task[30];
        TaskScheduler scheduler = null;
        scheduler = TaskScheduler.Default;

        for (int i = 0; i < tasks2.Length; i++)
        {
            tasks2[i] = new Task(() => {
                Thread.Sleep(2000);
                ShowLoadingProgress(0);
                
            });
            tasks2[i].Start();

        }

        for (int i = 0; i < tasks.Length; i++)
        {
            tasks[i] = new Task(() => {
                Thread.Sleep(2000);
                SearchStudent(0);
              

            });
            tasks[i].Start();

        }
        Task.WaitAll(tasks);
        Thread.Sleep(2000);
        timer.Dispose();

        Console.ReadLine();

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
        int studentNumber = -9;
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