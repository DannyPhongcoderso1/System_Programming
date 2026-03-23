using System.Diagnostics;

namespace Tutorial3_question5
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("--- Demo Async/Await & Pitfalls ---");

            // 1. Sử dụng await đúng cách
            Console.WriteLine("\n[1] Starting Async operation...");
            Stopwatch sw = Stopwatch.StartNew();
            await PerformLongRunningWorkAsync();
            sw.Stop();
            Console.WriteLine($"[1] Completed in: {sw.ElapsedMilliseconds} ms");

            // 2. Minh họa sai lầm với async void (Chỉ gọi để xem behavior)
            Console.WriteLine("\n[2] Triggering async void (Risk: Unhandled Exception)...");
            AsyncVoidExceptionDanger();
            await Task.Delay(500); // Đợi một chút để xem log

            // 3. Minh họa vấn đề với Task.Result (Cảnh báo: Có thể gây Deadlock)
            Console.WriteLine("\n[3] Warning: Task.Result / Task.Wait() blocks the thread.");
            var result = Task.Run(() => "Data from Task").Result;
            Console.WriteLine($"[3] Task.Result: {result}");
        }

        static async Task PerformLongRunningWorkAsync()
        {
            // Mô phỏng tác vụ tốn thời gian (I/O, Network...) mà không khóa CPU
            await Task.Delay(1000);
        }

        // SAI LẦM: Tránh dùng async void trừ khi là Event Handler
        static async void AsyncVoidExceptionDanger()
        {
            try
            {
                await Task.Delay(100);
                // Nếu ném lỗi ở đây, Main sẽ không bắt được vì không có Task để theo dõi
                // throw new Exception("Boom!"); 
                Console.WriteLine("Async void finished.");
            }
            catch (Exception ex) { Console.WriteLine($"Caught in void: {ex.Message}"); }
        }
    }
}
