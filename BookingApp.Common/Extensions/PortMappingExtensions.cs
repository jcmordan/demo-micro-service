using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace BookingApp.Common.Extensions;

public static class PortMappingExtensions
{
    /**
     * Configures the service to run on the URL specified in the ServiceUrls section of the configuration.
     */
    public static WebApplicationBuilder AddServiceUrl(this WebApplicationBuilder builder, string serviceName)
    {
        var url = builder.Configuration[$"ServiceUrls:{serviceName}"];
        
        if (!string.IsNullOrEmpty(url))
        {
            builder.WebHost.UseUrls(url);
        }

        return builder;
    }
}
