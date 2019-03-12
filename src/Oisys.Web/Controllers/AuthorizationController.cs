using AutoMapper;
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
using System.Text;
using System.Threading.Tasks;

namespace OisysNew.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthorizationController : Controller
    {
        private readonly IOisysDbContext context;
        private readonly IMapper mapper;
        private readonly IPasswordHasher<ApplicationUser> hasher;
        private readonly IOptions<AuthOptions> authOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationController"/> class.
        /// </summary>
        /// <param name="context">The database context</param>
        /// <param name="mapper">The mapper</param>
        /// <param name="hasher">The password hasher</param>
        /// <param name="authOptions">The authentication configuration</param>
        public AuthorizationController(
            IOisysDbContext context,
            IMapper mapper,
            IPasswordHasher<ApplicationUser> hasher,
            IOptions<AuthOptions> authOptions)
        {
            this.context = context;
            this.mapper = mapper;
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
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDto>> Token([FromForm]LoginRequest model)
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

            var signingCred = new SigningCredentials(new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(authOptions.Value.Key)), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                expires: DateTime.UtcNow.AddDays(90),
                notBefore: DateTime.UtcNow,
                signingCredentials: signingCred);

            var userDto = mapper.Map<UserDto>(user);
            userDto.Token = new JwtSecurityTokenHandler().WriteToken(token);

            return userDto;
        }
    }
}