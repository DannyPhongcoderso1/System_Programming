using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.WindowsServices;
using worker;

var builder = Host.CreateApplicationBuilder(args);

// QUAN TRỌNG: Phải có dòng này để chạy được như Windows Service
builder.Services.AddWindowsService(options =>
{
    options.ServiceName = "TradingService";
});

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();