using System;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.Generic;

namespace Tutorial3_question4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const int workItemCount = 100;

            // 1. Sử dụng Thread (Tạo thủ công)
            Console.WriteLine("\n--- Starting with Manual Threads ---");
            MeasurePerformance(() => {
                List<Thread> threads = new List<Thread>();
                for (int i = 0; i < workItemCount; i++)
                {
                    Thread t = new Thread(() => DoWork("Thread"));
                    threads.Add(t);
                    t.Start();
                }
                foreach (var t in threads) t.Join(); // Đợi tất cả hoàn thành
            });

            // 2. Sử dụng ThreadPool
            Console.WriteLine("\n--- Starting with ThreadPool ---");
            MeasurePerformance(() => {
                using (CountdownEvent cte = new CountdownEvent(workItemCount))
                {
                    for (int i = 0; i < workItemCount; i++)
                    {
                        ThreadPool.QueueUserWorkItem(_ => {
                            DoWork("ThreadPool");
                            cte.Signal();
                        });
                    }
                    cte.Wait();
                }
            });

            // 3. Sử dụng Task (TPL - Task Parallel Library)
            Console.WriteLine("\n--- Starting with Tasks ---");
            MeasurePerformance(() => {
                Task[] tasks = new Task[workItemCount];
                for (int i = 0; i < workItemCount; i++)
                {
                    tasks[i] = Task.Run(() => DoWork("Task"));
                }
                Task.WaitAll(tasks);
            });
        }

        static void DoWork(string type)
        {
            // In ra ID của Thread đang xử lý
            // Console.WriteLine($"[{type}] Managed Thread ID: {Environment.CurrentManagedThreadId}");
            Thread.Sleep(10); // Giả lập công việc tốn chút thời gian
        }

        static void MeasurePerformance(Action action)
        {
            Stopwatch sw = Stopwatch.StartNew();
            action();
            sw.Stop();
            Console.WriteLine($"Total Time: {sw.ElapsedMilliseconds} ms");
        }
    }
}
