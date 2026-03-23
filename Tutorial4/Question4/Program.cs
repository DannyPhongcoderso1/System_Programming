using System.IO.Compression;
using System.Text;

namespace Tutorial4_question4
{
    class Question4
    {
        private static string watchPath = "./WatchFolder";
        private static string outputPath = "./OutputFolder";

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            // Chuẩn bị thư mục
            Directory.CreateDirectory(watchPath);
            Directory.CreateDirectory(outputPath);

            using var watcher = new FileSystemWatcher(watchPath);

            // Chỉ theo dõi file văn bản
            watcher.Filter = "*.txt";
            watcher.Created += OnCreated;
            watcher.EnableRaisingEvents = true;

            Console.WriteLine($"--- Đang theo dõi thư mục: {Path.GetFullPath(watchPath)} ---");
            Console.WriteLine("Nhấn [Enter] để thoát.");
            Console.ReadLine();
        }

        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"[*] Phát hiện file mới: {e.Name}");

            // Xử lý bất đồng bộ để không chặn luồng chính của Watcher
            Task.Run(() => ProcessFileWithRetry(e.FullPath));
        }

        private static void ProcessFileWithRetry(string filePath)
        {
            int maxRetries = 5;
            int delay = 500;

            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    // Đọc và nén file
                    byte[] fileBytes = File.ReadAllBytes(filePath);
                    string compressedPath = Path.Combine(outputPath, Path.GetFileName(filePath) + ".gz");

                    using (FileStream fs = new FileStream(compressedPath, FileMode.Create))
                    using (GZipStream gz = new GZipStream(fs, CompressionMode.Compress))
                    {
                        gz.Write(fileBytes, 0, fileBytes.Length);
                    }

                    Console.WriteLine($"[OK] Đã nén thành công: {Path.GetFileName(compressedPath)}");
                    return;
                }
                catch (IOException)
                {
                    // Lỗi phổ biến: File đang bị tiến trình tạo nó giữ khóa (chưa ghi xong)
                    Console.WriteLine($"[!] File đang bận, thử lại lần {i + 1}...");
                    System.Threading.Thread.Sleep(delay);
                }
            }
        }
    }
}
