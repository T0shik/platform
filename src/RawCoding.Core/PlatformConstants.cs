using System.Security.Claims;

namespace RawCoding.Data
{
    public struct PlatformConstants
    {
        public const string Name = "RawCoding.Platform";

        public const string VisitorSignInPath = "/visitors/sign-in";

        public struct Policies
        {
            public const string Admin = nameof(Admin);
            public const string Visitor = nameof(Visitor);
        }

        public struct Schemas
        {
            public const string Default = "RawCoding.Identity";
            public const string Visitor = "Visitor";
        }

        public struct Claims
        {
            public const string Role = nameof(Role);
        }

        public struct Roles
        {
            public const string Admin = nameof(Admin);
        }

        public static readonly Claim AdminClaim = new Claim(Claims.Role, Roles.Admin);

        public struct Shop
        {
            public static readonly Claim ManagerClaim = new Claim(Claims.Role, Roles.ShopManager);

            public struct Policies
            {
                public const string ShopManager = nameof(ShopManager);
            }

            public struct Roles
            {
                public const string ShopManager = nameof(ShopManager);
            }
        }
    }
}