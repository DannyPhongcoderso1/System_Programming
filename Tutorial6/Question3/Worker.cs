using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using System.Collections.Concurrent; // Thư viện cho ConcurrentDictionary
using System.IO;

namespace worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private string _inputFolder;
        private string _processedFolder;
        private int _intervalSeconds;

        // Task 4 (Q3): Ngăn chặn việc xử lý trùng lặp (Double Processing)
        // Key là đường dẫn file, Value là giá trị tạm (0)
        private static readonly ConcurrentDictionary<string, byte> _processingFiles = new();

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Trading Service (Q3) Starting...");

            // Đọc Registry (Question 2)
            LoadRegistryConfiguration();

            // Đảm bảo thư mục tồn tại
            if (!Directory.Exists(_inputFolder)) Directory.CreateDirectory(_inputFolder);
            if (!Directory.Exists(_processedFolder)) Directory.CreateDirectory(_processedFolder);

            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Task 1 (Q3): Sử dụng FileSystemWatcher để theo dõi file JSON
            using FileSystemWatcher watcher = new FileSystemWatcher(_inputFolder, "*.json");

            // Khi có file mới được tạo ra
            watcher.Created += (s, e) =>
            {
                _logger.LogInformation($"Phát hiện file mới: {e.Name}");
                // Xử lý bất đồng bộ để có thể nhận nhiều file cùng lúc
                Task.Run(() => ProcessTradeFileAsync(e.FullPath), stoppingToken);
            };

            watcher.EnableRaisingEvents = true;

            // Vòng lặp duy trì Service (Question 1)
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Hệ thống đang theo dõi thư mục... (Vòng lặp Q1)");
                await Task.Delay(TimeSpan.FromSeconds(_intervalSeconds), stoppingToken);
            }
        }

        // Logic xử lý file (Question 3 - Task 1, 2, 3, 4)
        private async Task ProcessTradeFileAsync(string filePath)
        {
            // Task 4: Kiểm tra thread safety - Nếu file đang được xử lý bởi luồng khác thì bỏ qua
            if (!_processingFiles.TryAdd(filePath, 0)) return;

            try
            {
                // Task 1: Đọc nội dung file
                _logger.LogInformation($"Đang đọc file: {Path.GetFileName(filePath)}");
                string content = await File.ReadAllTextAsync(filePath);

                // Task 2: Giả lập xử lý dữ liệu (ví dụ: mất 2 giây)
                await Task.Delay(2000);

                // Task 3: Di chuyển file sang thư mục Processed
                string fileName = Path.GetFileName(filePath);
                string destinationPath = Path.Combine(_processedFolder, fileName);

                // Di chuyển file (ghi đè nếu file đã tồn tại ở đích)
                File.Move(filePath, destinationPath, overwrite: true);

                _logger.LogInformation($"Hoàn thành! Đã chuyển {fileName} sang thư mục Processed.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi khi xử lý file {filePath}: {ex.Message}");
            }
            finally
            {
                // Xử lý xong thì xóa khỏi danh sách đang xử lý
                _processingFiles.TryRemove(filePath, out _);
            }
        }

        private void LoadRegistryConfiguration()
        {
            try
            {
                using RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\TradingService");
                if (key != null)
                {
                    _inputFolder = key.GetValue("InputFolder")?.ToString() ?? @"D:\CS\System_programming\Trading\Input";
                    _processedFolder = key.GetValue("ProcessedFolder")?.ToString() ?? @"D:\CS\System_programming\Trading\Processed";
                    _intervalSeconds = Convert.ToInt32(key.GetValue("IntervalSeconds") ?? 30);
                }
                else
                {
                    _inputFolder = @"D:\CS\System_programming\Trading\Input";
                    _processedFolder = @"D:\CS\System_programming\Trading\Processed";
                    _intervalSeconds = 30;
                }
            }
            catch { /* Dùng mặc định nếu lỗi Registry */ }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Trading Service (Q3) Stopped.");
            await base.StopAsync(cancellationToken);
        }
    }
}