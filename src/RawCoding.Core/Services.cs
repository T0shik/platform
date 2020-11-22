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
        public static IServiceCollection AddPlatformBaseServices(
            this IServiceCollection services,
            IConfiguration config
        )
        {
            services.Configure<PlatformSettings>(config.GetSection(nameof(PlatformSettings)));

            return services;
        }

        public static IServiceCollection AddPlatformDbContext(
            this IServiceCollection services,
            IConfiguration config
        )
        {
            services.AddDbContext<PlatformDbContext>(options =>
                {
                    options.UseInMemoryDatabase(PlatformConstants.Name);
                })
                .AddDataProtection()
                .PersistKeysToDbContext<PlatformDbContext>()
                .SetApplicationName(PlatformConstants.Name);

            return services;
        }

        public static IServiceCollection AddPlatformCookieConfig(
            this IServiceCollection services,
            IConfiguration config
        )
        {
            services.AddAuthentication(PlatformConstants.DefaultAuthenticationSchema)
                .AddCookie(PlatformConstants.DefaultAuthenticationSchema, options => { options.Cookie.Name = PlatformConstants.IdentityCookieName; });

            return services;
        }
    }
}