using System.Diagnostics;

namespace Tutorial4_question1
{
    class Question1
    {
        static int sharedCounter = 0;
        const int Iterations = 100000;
        const int TaskCount = 5;

        static async Task Main()
        {
            Console.WriteLine($"Muc tieu: Ky vong ket qua la {Iterations * TaskCount}\n");

            // Gay ra Race Condition
            sharedCounter = 0;
            await RunTest("Race Condition", () => {
                sharedCounter++; 
            });

            // Sua loi dung lock
            sharedCounter = 0;
            object lockObj = new object();
            await RunTest("Sử dụng lock", () => {
                lock (lockObj)
                {
                    sharedCounter++;
                }
            });

            // Sua loi dung Interlocked
            sharedCounter = 0;
            await RunTest("Sử dụng Interlocked.Increment", () => {
                Interlocked.Increment(ref sharedCounter);
            });
        }

        static async Task RunTest(string testName, Action action)
        {
            Stopwatch sw = Stopwatch.StartNew();
            List<Task> tasks = new List<Task>();

            for (int i = 0; i < TaskCount; i++)
            {
                tasks.Add(Task.Run(() => {
                    for (int j = 0; j < Iterations; j++) action();
                }));
            }

            await Task.WhenAll(tasks);
            sw.Stop();

            Console.WriteLine($"{testName}:");
            Console.WriteLine($"- Ket qua: {sharedCounter}");
            Console.WriteLine($"- Thoi gian: {sw.ElapsedMilliseconds} ms\n");
        }
    }
}
