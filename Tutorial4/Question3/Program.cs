using System.Text;

namespace Tutorial4_question3
{
    class Question3
    {
        private static string logPath = "system_log.txt";
        private static object fileLock = new object();

        static async Task Main(string[] args)
        {
            if (File.Exists(logPath)) File.Delete(logPath);

            Console.WriteLine("--- Thử nghiệm ghi file đa luồng ---");

            // 1. Chạy KHÔNG ĐỒNG BỘ (Dễ gây lỗi IO hoặc ghi đè dữ liệu)
            // Task.Run(() => UnsafeWriteLogs()); 

            // 2. Chạy CÓ ĐỒNG BỘ (Dùng lock)
            await SafeWriteLogs();

            Console.WriteLine("\nHoàn thành! Kiểm tra file system_log.txt.");
        }

        static async Task SafeWriteLogs()
        {
            List<Task> tasks = new List<Task>();
            for (int i = 1; i <= 10; i++)
            {
                int taskId = i;
                tasks.Add(Task.Run(() =>
                {
                    for (int j = 0; j < 5; j++)
                    {
                        string message = $"[Task {taskId}] Ghi log lần {j} lúc {DateTime.Now:HH:mm:ss.fff}\n";

                        // Cơ chế khóa để bảo vệ File I/O
                        lock (fileLock)
                        {
                            File.AppendAllText(logPath, message, Encoding.UTF8);
                        }
                    }
                }));
            }
            await Task.WhenAll(tasks);
        }
    }
}
