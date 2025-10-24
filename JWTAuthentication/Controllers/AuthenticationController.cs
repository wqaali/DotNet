using JWTAuthentication.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace JWTAuthentication.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    
    public class AuthenticationController : ControllerBase
    {
        readonly IJwtAuthentication _jwtAuthentication;
        public AuthenticationController(IJwtAuthentication jwtAuthentication) { _jwtAuthentication = jwtAuthentication; }

        [HttpGet("TestApi")]
        [AllowAnonymous]
        [EnableRateLimiting("general")]
        public ActionResult Test()
        {
            return Ok("Hello");
            
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [EnableRateLimiting("login")]
        public ActionResult UserAuthentication([FromBody] UserModel user)
        {
            //Validate user from Db and get Id of user
            // Currently using hardcoded id as example
            var userId = "125487";
            string email = "abc@gmail.com";
            AuthBody? token = _jwtAuthentication.Authenticate(userId, email, null);

            return Ok(value: token);
        }
        [HttpPost("RefreshToken")]
        [AllowAnonymous]
        public ActionResult RefreshToken([FromBody] AuthBody request)
        {
            var principal = _jwtAuthentication.GetPrincipalFromExpiredToken(request.AccessToken);
            var userId = principal.Identity.Name;

            // Validate refresh token from DB
            //var savedRefreshToken = _refreshTokenService.Get(userId);
            if (String.IsNullOrEmpty(request.RefreshToken))
                return Unauthorized();

            // Generate new tokens
            var newTokens = _jwtAuthentication.Authenticate(userId, principal.FindFirst("emailId")?.Value);
            return Ok(newTokens);
        }
    }
}
