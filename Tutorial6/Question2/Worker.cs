using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Win32; 

namespace worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        // Các biến để lưu cấu hình đọc từ Registry
        private string _inputFolder;
        private string _processedFolder;
        private int _intervalSeconds;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        // Question 2 - Task 1, 2, 3: Đọc cấu hình khi khởi động
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Trading Service (Q2) starting...");

            try
            {
                // Task 1: Mở Registry Key
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\TradingService"))
                {
                    // Task 2: Kiểm tra nếu Key không tồn tại
                    if (key == null)
                    {
                        _logger.LogWarning("Registry Key 'HKLM\\Software\\TradingService' không tìm thấy. Sử dụng cấu hình mặc định.");
                        SetDefaultConfig();
                    }
                    else
                    {
                        // Đọc giá trị từ Registry
                        _inputFolder = key.GetValue("InputFolder")?.ToString();
                        _processedFolder = key.GetValue("ProcessedFolder")?.ToString();

                        // Đọc Interval, nếu không có thì mặc định 30
                        var intervalObj = key.GetValue("IntervalSeconds");
                        _intervalSeconds = intervalObj != null ? Convert.ToInt32(intervalObj) : 30;

                        // Task 3: Kiểm tra tính hợp lệ của cấu hình
                        if (string.IsNullOrEmpty(_inputFolder) || string.IsNullOrEmpty(_processedFolder))
                        {
                            _logger.LogError("Cấu hình thư mục trong Registry bị trống!");
                            SetDefaultConfig();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Task 3: Log lỗi nếu có vấn đề khi đọc cấu hình
                _logger.LogError($"Lỗi khi đọc Registry: {ex.Message}");
                SetDefaultConfig();
            }

            _logger.LogInformation($"Cấu hình: Thư mục vào={_inputFolder}, Thư mục ra={_processedFolder}, Chu kỳ={_intervalSeconds}s");
            await base.StartAsync(cancellationToken);
        }

        private void SetDefaultConfig()
        {
            _inputFolder = @"C:\Trading\Input";
            _processedFolder = @"C:\Trading\Processed";
            _intervalSeconds = 30;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker Q2 running. Checking folder: {folder} at {time}", _inputFolder, DateTimeOffset.Now);

                // Sử dụng biến _intervalSeconds thay vì con số 30 cố định
                await Task.Delay(TimeSpan.FromSeconds(_intervalSeconds), stoppingToken);
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Trading Service (Q2) stopped.");
            await base.StopAsync(cancellationToken);
        }
    }
}