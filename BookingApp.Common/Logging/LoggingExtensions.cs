using Microsoft.AspNetCore.Builder;
using Serilog;
using Serilog.Events;

namespace BookingApp.Common.Logging;

public static class LoggingExtensions
{
    public static void AddCustomLogging(this WebApplicationBuilder builder, string serviceName)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("System", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("ServiceName", serviceName)
            .WriteTo.Console(new CustomJsonFormatter())
            .CreateLogger();

        builder.Host.UseSerilog();
    }
}
