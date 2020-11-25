using Microsoft.AspNetCore.Authorization;

namespace RawCoding.Data.Extensions
{
    public static class AuthorizationOptionsExtensions
    {
        public static AuthorizationOptions AddVisitorPolicy(this AuthorizationOptions options)
        {
            options.AddPolicy(PlatformConstants.Policies.Visitor, policy => policy
                .AddAuthenticationSchemes(PlatformConstants.Schemas.Visitor)
                .RequireAuthenticatedUser());

            return options;
        }

        public static AuthorizationOptions AddAdminPolicy(this AuthorizationOptions options)
        {
            options.AddPolicy(PlatformConstants.Policies.Admin, policy => policy
                .AddAuthenticationSchemes(PlatformConstants.Schemas.Default)
                .RequireClaim(PlatformConstants.Claims.Role, PlatformConstants.Roles.Admin)
                .RequireAuthenticatedUser());

            return options;
        }
    }
}