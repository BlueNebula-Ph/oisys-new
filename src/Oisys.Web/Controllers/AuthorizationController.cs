using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OisysNew.Configuration;
using OisysNew.DTO.Login;
using OisysNew.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OisysNew.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthorizationController : Controller
    {
        private readonly IOisysDbContext context;
        private readonly IPasswordHasher<ApplicationUser> hasher;
        private readonly IOptions<AuthOptions> authOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationController"/> class.
        /// </summary>
        /// <param name="context">The database context</param>
        /// <param name="hasher">The password hasher</param>
        /// <param name="authOptions">The authentication configuration</param>
        public AuthorizationController(
            OisysDbContext context,
            IPasswordHasher<ApplicationUser> hasher,
            IOptions<AuthOptions> authOptions)
        {
            this.context = context;
            this.hasher = hasher;
            this.authOptions = authOptions;
        }

        /// <summary>
        /// Generates a Json Web Token
        /// </summary>
        /// <param name="model">The login request model</param>
        /// <returns>An object containing the JWT</returns>
        [HttpPost("token")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Token([FromForm]LoginRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid username or password.");
            }

            // Validate User
            var user = await context.Users.FirstOrDefaultAsync(a => a.Username == model.Username);
            if (user == null)
            {
                return BadRequest("Unable to login. User not found.");
            }

            if (hasher.VerifyHashedPassword(user, user.PasswordHash, model.Password) != PasswordVerificationResult.Success)
            {
                return BadRequest("Incorrect username or password.");
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, model.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sid, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.GivenName, $"{user.Firstname} {user.Lastname}"),
                new Claim("accessRights", user.AccessRights),
            };

            var signingCred = new SigningCredentials(new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(authOptions.Value.Key)), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(90),
                notBefore: DateTime.UtcNow,
                signingCredentials: signingCred);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expirationDate = token.ValidTo,
            });
        }
    }
}