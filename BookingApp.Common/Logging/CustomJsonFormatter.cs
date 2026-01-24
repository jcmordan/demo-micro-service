using System.Text.Json;
using Serilog.Events;
using Serilog.Formatting;

namespace BookingApp.Common.Logging;

public class CustomJsonFormatter : ITextFormatter
{
    public void Format(LogEvent logEvent, TextWriter output)
    {
        var logObject = new Dictionary<string, object?>
        {
            ["timestamp"] = logEvent.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff"),
            ["level"] = logEvent.Level.ToString(),
            ["message"] = logEvent.RenderMessage()
        };

        foreach (var property in logEvent.Properties)
        {
            // Map Serilog property names to user requested names
            var key = property.Key switch
            {
                "ServiceName" => "service",
                "TraceId" => "traceId",
                _ => property.Key
            };
            
            logObject[key] = property.Value is ScalarValue sv ? sv.Value : property.Value.ToString();
        }

        if (logEvent.Exception != null)
        {
            logObject["exception"] = logEvent.Exception.ToString();
        }

        output.WriteLine(JsonSerializer.Serialize(logObject));
    }
}
