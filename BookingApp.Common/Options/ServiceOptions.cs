using System.Collections.Generic;

namespace BookingApp.Common.Options;

public class ServiceOptions
{
    public Dictionary<string, string> ServiceUrls { get; set; } = new();
}
