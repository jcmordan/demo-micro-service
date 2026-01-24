using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace BookingApp.Common.Logging;

public class TraceIdMiddleware
{
    private readonly RequestDelegate _next;
    private const string TraceIdHeader = "X-Trace-Id";

    public TraceIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(TraceIdHeader, out var traceId))
        {
            traceId = Guid.NewGuid().ToString();
        }

        context.Items["TraceId"] = traceId;
        context.Response.Headers[TraceIdHeader] = traceId;

        using (LogContext.PushProperty("TraceId", traceId))
        {
            await _next(context);
        }
    }
}
