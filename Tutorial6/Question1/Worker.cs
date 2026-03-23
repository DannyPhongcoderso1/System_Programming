using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        // Task 4: In log khi bắt đầu (Start)
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Trading Service started at: {time}", DateTimeOffset.Now);
            await base.StartAsync(cancellationToken);
        }

        // Task 3: Vòng lặp chạy mỗi 30 giây
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                // Đợi 30 giây theo yêu cầu đề bài
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }

        // Task 4: In log khi kết thúc (Stop)
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Trading Service stopped at: {time}", DateTimeOffset.Now);
            await base.StopAsync(cancellationToken);
        }
    }
}
