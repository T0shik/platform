using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RawCoding.Shop.Database;
using Stripe;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using RawCoding.Data;
using RawCoding.Data.Extensions;
using RawCoding.S3;
using RawCoding.Shop.UI.Middleware.Shop;
using RawCoding.Shop.UI.Workers.Email;

namespace RawCoding.Shop.UI
{
    public class Startup
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;
        private const string NuxtAppCors = nameof(NuxtAppCors);

        public Startup(
            IConfiguration configuration,
            IWebHostEnvironment env)
        {
            _config = configuration;
            _env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
            });

            services.Configure<StripeSettings>(_config.GetSection(nameof(StripeSettings)));
            services.Configure<ShopSettings>(_config.GetSection(nameof(ShopSettings)));
            StripeConfiguration.ApiKey = _config.GetSection("Stripe")["SecretKey"];

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                if (_env.IsProduction())
                {
                    options.UseNpgsql(_config.GetConnectionString("DefaultConnection"));
                }
                else
                {
                    options.UseInMemoryDatabase("DevShop");
                }
            });

            services.AddPlatformServices()
                .AddPlatformCookieAuthentication()
                .AddVisitorCookie();

            services.AddAuthorization(options =>
            {
                options.AddVisitorPolicy()
                    .AddAdminPolicy();

                options.AddPolicy(PlatformConstants.Shop.Policies.ShopManager, policy => policy
                    .AddAuthenticationSchemes(PlatformConstants.Schemas.Default)
                    .RequireClaim(PlatformConstants.Claims.Role,
                        PlatformConstants.Shop.Roles.ShopManager,
                        PlatformConstants.Roles.Admin
                    )
                    .RequireAuthenticatedUser());
            });

            services.AddControllers();

            services
                .AddApplicationServices()
                .AddEmailService(_config)
                .AddRawCodingS3Client(() => _config.GetSection(nameof(S3StorageSettings)).Get<S3StorageSettings>())
                .AddScoped<PaymentIntentService>();

            services.AddCors(options => options
                .AddPolicy(NuxtAppCors, builder => builder
                    .WithOrigins("http://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()));
        }

        public void Configure(IApplicationBuilder app)
        {
            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseCors(NuxtAppCors)
                .UseCookiePolicy()
                .UseRouting()
                .UseMiddleware<ShopMiddleware>()
                .UseStatusCodePages(context =>
                {
                    var pathBase = context.HttpContext.Request.PathBase;
                    var endpoint = StatusCodeEndpoint(context.HttpContext.Response.StatusCode);
                    context.HttpContext.Response.Redirect(pathBase + endpoint);
                    return Task.CompletedTask;
                })
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapDefaultControllerRoute()
                        .RequireAuthorization(PlatformConstants.Policies.Visitor);
                });
        }

        private static string StatusCodeEndpoint(int code) =>
            code switch
            {
                StatusCodes.Status404NotFound => "/not-found",
                _ => "/",
            };
    }
}