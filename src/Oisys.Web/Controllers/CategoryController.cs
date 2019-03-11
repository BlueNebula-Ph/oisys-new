using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OisysNew.DTO;
using OisysNew.DTO.Category;
using OisysNew.Helpers;
using OisysNew.Helpers.Interfaces;
using OisysNew.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace OisysNew.Controllers
{
    /// <summary>
    /// <see cref="CategoryController"/> handles creating, reading, updating and deleting categories
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly IOisysDbContext context;
        private readonly IMapper mapper;
        private readonly IListHelpers listHelpers;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryController"/> class.
        /// </summary>
        /// <param name="context">DbContext</param>
        /// <param name="mapper">Automapper</param>
        /// <param name="listHelpers">List helper</param>
        public CategoryController(
            IOisysDbContext context,
            IMapper mapper,
            IListHelpers listHelpers,
            ILogger<CategoryController> logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.listHelpers = listHelpers;
            this.logger = logger;
        }

        /// <summary>
        /// Returns list of active <see cref="Category"/>
        /// </summary>
        /// <param name="filter"><see cref="CategoryFilterRequest"/></param>
        /// <returns>List of Category</returns>
        [HttpPost("search", Name = "GetAllCategories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaginatedList<CategorySummary>>> GetAll([FromBody] CategoryFilterRequest filter)
        {
            try
            {
                var list = context.Categories.AsNoTracking();

                // filter
                if (!string.IsNullOrEmpty(filter?.SearchTerm))
                {
                    list = list.Where(c => c.Name.Contains(filter.SearchTerm, StringComparison.CurrentCultureIgnoreCase));
                }

                // sort
                var ordering = $"{Constants.ColumnNames.Name} {Constants.DefaultSortDirection}";
                if (!string.IsNullOrEmpty(filter?.SortBy))
                {
                    ordering = $"{filter.SortBy} {filter.SortDirection}";
                }

                list = list.OrderBy(ordering);

                var result = await listHelpers.CreatePaginatedListAsync<Category, CategorySummary>(list, filter.PageNumber, filter.PageSize);
                return result;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Returns list of active <see cref="Category"/>
        /// </summary>
        /// <returns>List of Categories</returns>
        [HttpGet("lookup/{name?}", Name = "GetCategoryLookup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<CategoryLookup>>> GetLookup(string name = "")
        {
            try
            {
                // get list of active items (not deleted)
                var list = context.Categories.AsNoTracking();

                if (!string.IsNullOrEmpty(name))
                {
                    list = list.Where(a => a.Name.StartsWith(name, StringComparison.CurrentCultureIgnoreCase));
                }

                // sort
                var ordering = $"{Constants.ColumnNames.Name} {Constants.DefaultSortDirection}";

                list = list.OrderBy(ordering);

                var categories = await list.ProjectTo<CategoryLookup>(mapper.ConfigurationProvider).ToListAsync();
                return categories;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Gets a specific <see cref="Category"/>.
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>Category</returns>
        [HttpGet("{id}", Name = "GetCategoryById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CategorySummary>> GetCategoryById(long id)
        {
            try
            {
                var entity = await context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

                if (entity == null)
                {
                    return NotFound(id);
                }

                var category = mapper.Map<CategorySummary>(entity);
                return category;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Creates a <see cref="Category"/>.
        /// </summary>
        /// <param name="entity">entity to be created</param>
        /// <returns>The new category</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SaveCategoryRequest>> Create([FromBody] SaveCategoryRequest entity)
        {
            try
            {
                var category = mapper.Map<Category>(entity);
                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();

                return CreatedAtRoute(nameof(GetCategoryById), new { id = category.Id }, entity);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Updates a specific <see cref="Category"/>.
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="entity">entity</param>
        /// <returns>None</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Update(long id, [FromBody] SaveCategoryRequest entity)
        {
            try
            {
                var category = await context.Categories.SingleOrDefaultAsync(t => t.Id == id);
                if (category == null)
                {
                    return NotFound(id);
                }

                mapper.Map(entity, category);
                context.Update(category);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch(DbUpdateConcurrencyException concurrencyEx)
            {
                logger.LogError(concurrencyEx.Message);
                return StatusCode(StatusCodes.Status409Conflict);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Deletes a specific <see cref="Category"/>.
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
                var category = await context.Categories.SingleOrDefaultAsync(t => t.Id == id);
                if (category == null)
                {
                    return NotFound(id);
                }

                category.IsDeleted = true;
                context.Update(category);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}