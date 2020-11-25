using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace RawCoding.Data.Extensions
{
    public static class HttpContextExtensions
    {
        public static Task AuthenticateVisitor(this HttpContext context)
        {
            var userId = Guid.NewGuid().ToString();
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
            }, PlatformConstants.Schemas.Visitor);

            var claimsPrinciple = new ClaimsPrincipal(identity);

            return context.SignInAsync(
                PlatformConstants.Schemas.Visitor,
                claimsPrinciple,
                new AuthenticationProperties {IsPersistent = true}
            );
        }
    }
}