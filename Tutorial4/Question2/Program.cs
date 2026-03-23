using System.Text;

namespace Tutorial4_question2
{
    class Question2
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("--- Bắt đầu Task Coordination ---\n");

            // Cách 1: Sử dụng Task.WhenAll 
            await RunWithWhenAll();

            Console.WriteLine("\n----------------------------------\n");

            // Cách 2: Sử dụng CountdownEvent
            RunWithCountdownEvent();

            Console.WriteLine("\nChương trình kết thúc an toàn.");
        }

        static async Task RunWithWhenAll()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("[Task.WhenAll] Đang khởi chạy 3 tác vụ...");
            Random rnd = new Random();

            Task[] tasks = new Task[3];
            for (int i = 0; i < 3; i++)
            {
                int id = i + 1;
                tasks[i] = Task.Run(async () =>
                {
                    int sleepTime = rnd.Next(1000, 3000);
                    await Task.Delay(sleepTime);
                    Console.WriteLine($"   > Tác vụ {id} hoàn thành sau {sleepTime}ms");
                });
            }

            await Task.WhenAll(tasks);
            Console.WriteLine("[RESULT] Thông báo này chỉ hiện khi 3 Task dùng WhenAll xong.");
        }

        static void RunWithCountdownEvent()
        {
            Console.WriteLine("[CountdownEvent] Đang khởi chạy 3 tác vụ...");
            using (CountdownEvent countdown = new CountdownEvent(3))
            {
                for (int i = 1; i <= 3; i++)
                {
                    int id = i;
                    ThreadPool.QueueUserWorkItem(_ =>
                    {
                        Thread.Sleep(new Random().Next(1000, 3000));
                        Console.WriteLine($"   > Luồng {id} (ThreadPool) đã xong.");
                        countdown.Signal(); // Giảm biến đếm đi 1
                    });
                }

                countdown.Wait(); // Khóa luồng hiện tại cho đến khi biến đếm về 0
                Console.WriteLine("[RESULT] Thông báo này hiện sau khi CountdownEvent nhận đủ 3 tín hiệu.");
            }
        }
    }
}
