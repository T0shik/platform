using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RawCoding.Data;
using RawCoding.Data.Extensions;

namespace RawCoding.Identity
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        private readonly PlatformSettings _platformSettings;

        public Startup(
            IWebHostEnvironment env,
            IConfiguration config
        )
        {
            _env = env;
            _config = config;
            _platformSettings = config.GetPlatformConfiguration();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddPlatformBaseServices(_config)
                .AddPlatformDbContext(_config);

            services.AddIdentity<PlatformUser, IdentityRole>(c =>
                {
                    if (_env.IsDevelopment())
                    {
                        c.Password.RequireDigit = false;
                        c.Password.RequireLowercase = false;
                        c.Password.RequireUppercase = false;
                        c.Password.RequireNonAlphanumeric = false;
                        c.Password.RequiredLength = 4;
                    }
                    else
                    {
                        // todo implement
                    }
                })
                .AddEntityFrameworkStores<PlatformDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(o =>
            {
                o.Cookie.Name = PlatformConstants.IdentityCookieName;
                o.Cookie.Domain = _platformSettings.CookieDomain;
            });

            services.AddRazorPages();
            services.Configure<RouteOptions>(o => { o.LowercaseUrls = true; });
        }

        public void Configure(IApplicationBuilder app)
        {
            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapRazorPages(); });
        }
    }
}