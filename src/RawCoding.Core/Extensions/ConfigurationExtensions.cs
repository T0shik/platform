using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;

namespace RawCoding.Data.Extensions
{
    public static class ConfigurationExtensions
    {
        public static PlatformSettings GetPlatformConfiguration(this IConfiguration config) =>
            config.GetSection(nameof(PlatformSettings)).Get<PlatformSettings>();
    }
}