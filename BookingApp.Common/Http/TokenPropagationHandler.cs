using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace BookingApp.Common.Http;

/// <summary>
/// A delegating handler that propagates the Authorization header from the incoming HTTP request to outgoing HTTP requests.
/// </summary>
public class TokenPropagationHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="TokenPropagationHandler"/> class.
    /// </summary>
    /// <param name="httpContextAccessor">The HTTP context accessor.</param>
    public TokenPropagationHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <inheritdoc />
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var context = _httpContextAccessor.HttpContext;
        if (context != null)
        {
            // Propagate Authorization header
            if (context.Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                if (!request.Headers.Contains("Authorization"))
                {
                    request.Headers.Add("Authorization", authHeader.ToString());
                }
            }

            // Propagate TraceId header
            if (context.Request.Headers.TryGetValue("X-Trace-Id", out var traceId))
            {
                request.Headers.Add("X-Trace-Id", traceId.ToString());
            }
            else if (context.Items.TryGetValue("TraceId", out var itemTraceId))
            {
                request.Headers.Add("X-Trace-Id", itemTraceId?.ToString());
            }
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
