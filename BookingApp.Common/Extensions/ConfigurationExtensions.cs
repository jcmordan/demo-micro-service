using dotenv.net;
using Microsoft.Extensions.Configuration;

namespace BookingApp.Common.Extensions;

public static class ConfigurationExtensions
{
    /**
     * Adds support for .env files to the configuration builder.
     * The settings in the .env file will override existing configuration.
     */
    public static IConfigurationBuilder AddDotEnv(this IConfigurationBuilder builder)
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        string? envPath = null;

        while (currentDirectory != null)
        {
            var potentialPath = Path.Combine(currentDirectory, ".env");
            if (File.Exists(potentialPath))
            {
                envPath = potentialPath;
                break;
            }
            currentDirectory = Directory.GetParent(currentDirectory)?.FullName;
        }
        
        if (envPath != null)
        {
            DotEnv.Load(new DotEnvOptions(envFilePaths: [envPath]));
            builder.AddEnvironmentVariables();
        }

        return builder;
    }
}
