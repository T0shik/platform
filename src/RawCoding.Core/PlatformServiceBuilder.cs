using System;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RawCoding.Data
{
    public class PlatformServiceBuilder
    {
        private readonly IServiceCollection _services;
        private readonly ServiceProvider _provider;
        private readonly IConfiguration _config;
        private readonly PlatformSettings _settings;

        public PlatformServiceBuilder(IServiceCollection services)
        {
            _services = services;
            _provider = services.BuildServiceProvider();
            _config = _provider.GetService<IConfiguration>() ?? throw new NullReferenceException(nameof(IConfiguration));
            var settingsSection = _config.GetSection(nameof(PlatformSettings));
            services.Configure<PlatformSettings>(settingsSection);
            _settings = settingsSection.Get<PlatformSettings>();
        }

        public PlatformServiceBuilder AddPlatformDbContext()
        {
            _services.AddDbContext<PlatformDbContext>(options => { options.UseInMemoryDatabase(PlatformConstants.Name); })
                .AddDataProtection()
                .PersistKeysToDbContext<PlatformDbContext>()
                .SetApplicationName(PlatformConstants.Name);

            return this;
        }

        public PlatformServiceBuilder AddPlatformCookieAuthentication()
        {
            _services.AddAuthentication(PlatformConstants.Schemas.Default)
                .AddCookie(PlatformConstants.Schemas.Default, options => { options.Cookie.Name = PlatformConstants.Schemas.Default; });

            return this;
        }

        public PlatformServiceBuilder AddVisitorCookie()
        {
            _services.AddAuthentication(PlatformConstants.Schemas.Visitor)
                .AddCookie(PlatformConstants.Schemas.Visitor,
                    options =>
                    {
                        options.Cookie.HttpOnly = false;
                        options.Cookie.IsEssential = true;
                        options.Cookie.SameSite = SameSiteMode.None;
                        options.Cookie.SecurePolicy = CookieSecurePolicy.None;
                        options.Cookie.Domain = _settings.CookieDomain;
                        options.Cookie.Name = PlatformConstants.Schemas.Visitor;
                        options.ExpireTimeSpan = TimeSpan.FromDays(365);
                        options.LoginPath = PlatformConstants.VisitorSignInPath;
                    });

            return this;
        }
    }
}