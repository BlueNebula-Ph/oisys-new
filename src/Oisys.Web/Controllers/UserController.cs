using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OisysNew.DTO;
using OisysNew.DTO.User;
using OisysNew.Helpers;
using OisysNew.Helpers.Interfaces;
using OisysNew.Models;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace OisysNew.Controllers
{
    /// <summary>
    /// <see cref="UserController"/> handles basic add, edit, delete and fetching of users.
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IOisysDbContext context;
        private readonly IMapper mapper;
        private readonly IListHelpers listHelpers;
        private readonly IPasswordHasher<ApplicationUser> passwordHasher;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="context">The DbContext</param>
        /// <param name="mapper">The mapper</param>
        /// <param name="builder">The summary list builder</param>
        /// <param name="passwordHasher">The tool for hashing passwords</param>
        public UserController(
            IOisysDbContext context,
            IMapper mapper,
            IListHelpers listHelpers,
            IPasswordHasher<ApplicationUser> passwordHasher,
            ILogger<UserController> logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.listHelpers = listHelpers;
            this.passwordHasher = passwordHasher;
            this.logger = logger;
        }

        /// <summary>
        /// Returns list of active <see cref="ApplicationUser"/>
        /// </summary>
        /// <param name="filter"><see cref="UserFilterRequest"/></param>
        /// <returns>List of Users</returns>
        [HttpPost("search", Name = "GetAllUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaginatedList<UserSummary>>> GetAll([FromBody]UserFilterRequest filter)
        {
            try
            {
                // get list of active users (not deleted)
                var list = context.Users
                    .AsNoTracking();

                // filter
                if (filter != null)
                {
                    list = list.Where(a => a.Username.Contains(filter.SearchTerm) ||
                        a.Firstname.Contains(filter.SearchTerm) ||
                        a.Lastname.Contains(filter.SearchTerm));
                }

                // sort
                var ordering = $"{Constants.ColumnNames.Username} {Constants.DefaultSortDirection}";
                if (!string.IsNullOrEmpty(filter?.SortBy))
                {
                    ordering = $"{filter.SortBy} {filter.SortDirection}";
                }

                list = list.OrderBy(ordering);

                var result = await listHelpers.CreatePaginatedListAsync<ApplicationUser, UserSummary>(list, filter.PageNumber, filter.PageSize);
                return result;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Gets a specific <see cref="ApplicationUser"/>.
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>User summary object</returns>
        [HttpGet("{id}", Name = "GetUserById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserSummary>> GetUserById(long id)
        {
            try
            {
                var entity = await context.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(c => c.Id == id);

                if (entity == null)
                {
                    return NotFound(id);
                }

                var user = mapper.Map<UserSummary>(entity);
                return user;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Creates a <see cref="ApplicationUser"/>.
        /// </summary>
        /// <param name="entity">User to be created</param>
        /// <returns>User object</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SaveUserRequest>> Create([FromBody]SaveUserRequest entity)
        {
            try
            {
                var user = mapper.Map<ApplicationUser>(entity);
                user.PasswordHash = passwordHasher.HashPassword(user, entity.Password);

                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();

                return CreatedAtRoute(nameof(GetUserById), new { id = user.Id }, entity);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Updates a specific <see cref="ApplicationUser"/>.
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="request">entity</param>
        /// <returns>None</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Update(long id, [FromBody]SaveUserRequest request)
        {
            try
            {
                var user = await context.Users
                    .AsNoTracking()
                    .SingleOrDefaultAsync(t => t.Id == id);

                if (user == null)
                {
                    return NotFound();
                }

                var oldPassword = user.PasswordHash;
                user = mapper.Map<ApplicationUser>(request);
                user.PasswordHash = request.UpdatePassword ?
                    passwordHasher.HashPassword(user, request.Password) :
                    oldPassword;

                context.Update(user);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (DbUpdateConcurrencyException concurrencyEx)
            {
                logger.LogError(concurrencyEx.Message);
                return StatusCode(StatusCodes.Status409Conflict, Constants.ErrorMessages.ConcurrencyErrorMessage);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Deletes a specific <see cref="ApplicationUser"/>.
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>None</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                var user = await context.Users
                    .SingleOrDefaultAsync(c => c.Id == id);

                if (user == null)
                {
                    return NotFound(id);
                }

                user.IsDeleted = true;
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}