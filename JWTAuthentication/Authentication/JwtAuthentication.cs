using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using System.Security.Principal;
using System.Security.Cryptography;

namespace JWTAuthentication.Authentication
{
    public class JwtAuthentication : IJwtAuthentication
    {

        #region Fields

        private readonly string _key;

        public JwtAuthentication(string key) => _key = key;

        #endregion

        #region Actions

        public AuthBody Authenticate(string userId, string email, DateTime? validTill = null)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenKey = Encoding.ASCII.GetBytes(_key);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, userId), new Claim("emailId", email) }),
                Expires = validTill ?? DateTime.Now.AddYears(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

            // Generate refresh token(random, not a JWT)
            var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

            // save refresh token in DB against this user (with expiry)
            //_refreshTokenService.Save(userId, refreshToken, DateTime.UtcNow.AddDays(7));
            AuthBody authResponse = new AuthBody
            {
                AccessToken = token,
                RefreshToken = refreshToken

            };
            return authResponse;
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_key)),
                ValidateLifetime = false // 👈 ignore expiry
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

            return principal;
        }
        #endregion
    }
}
