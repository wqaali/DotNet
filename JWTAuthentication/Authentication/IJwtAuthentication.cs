using System;
using System.Security.Claims;

namespace JWTAuthentication.Authentication
{
    public interface IJwtAuthentication
    {
        public AuthBody Authenticate(string userId, string email, DateTime? validTill = null);
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
