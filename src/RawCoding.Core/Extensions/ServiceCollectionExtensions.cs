using System.IO;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RawCoding.Data;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class Services
    {
        public static PlatformServiceBuilder AddPlatformServices(this IServiceCollection services)
        {
            return new PlatformServiceBuilder(services);
        }
    }
}