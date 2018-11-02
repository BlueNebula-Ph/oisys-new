using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OisysNew.DTO;
using OisysNew.DTO.Category;
using OisysNew.Helpers;
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
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly OisysDbContext context;
        private readonly IMapper mapper;
        private readonly IListHelpers listHelpers;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryController"/> class.
        /// </summary>
        /// <param name="context">DbContext</param>
        /// <param name="mapper">Automapper</param>
        /// <param name="listHelpers">List helper</param>
        public CategoryController(
            OisysDbContext context,
            IMapper mapper,
            IListHelpers listHelpers)
        {
            this.context = context;
            this.mapper = mapper;
            this.listHelpers = listHelpers;
        }

        /// <summary>
        /// Returns list of active <see cref="Category"/>
        /// </summary>
        /// <param name="filter"><see cref="CategoryFilterRequest"/></param>
        /// <returns>List of Category</returns>
        [HttpPost("search", Name = "GetAllCategories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PaginatedList<CategorySummary>>> GetAll([FromBody] CategoryFilterRequest filter)
        {
            // get list of active categories (not deleted)
            var list = this.context.Categories
                .AsNoTracking()
                .Where(c => !c.IsDeleted);

            // filter
            if (!string.IsNullOrEmpty(filter?.SearchTerm))
            {
                list = list.Where(c => c.Name.Contains(filter.SearchTerm, StringComparison.CurrentCultureIgnoreCase));
            }

            // sort
            var ordering = $"Name {Constants.DefaultSortDirection}";
            if (!string.IsNullOrEmpty(filter?.SortBy))
            {
                ordering = $"{filter.SortBy} {filter.SortDirection}";
            }

            list = list.OrderBy(ordering);

            var result = await this.listHelpers.CreatePaginatedListAsync<Category, CategorySummary>(list, filter.PageNumber, filter.PageSize);
            return result;
        }

        /// <summary>
        /// Returns list of active <see cref="Category"/>
        /// </summary>
        /// <returns>List of Categories</returns>
        [HttpGet("lookup", Name = "GetCategoryLookup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CategoryLookup>>> GetLookup()
        {
            // get list of active items (not deleted)
            var list = this.context.Categories
                .AsNoTracking()
                .Where(c => !c.IsDeleted);

            // sort
            var ordering = $"Name {Constants.DefaultSortDirection}";

            list = list.OrderBy(ordering);

            var categories = await list.ProjectTo<CategoryLookup>().ToListAsync();
            return categories;
        }

        /// <summary>
        /// Gets a specific <see cref="Category"/>.
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>Category</returns>
        [HttpGet("{id}", Name = "GetCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategorySummary>> GetById(long id)
        {
            var entity = await this.context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (entity == null)
            {
                return this.NotFound(id);
            }

            var category = this.mapper.Map<CategorySummary>(entity);
            return category;
        }

        /// <summary>
        /// Creates a <see cref="Category"/>.
        /// </summary>
        /// <param name="entity">entity to be created</param>
        /// <returns>Category</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SaveCategoryRequest>> Create([FromBody] SaveCategoryRequest entity)
        {
            // TODO: Move to a filter
            if(!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            var category = this.mapper.Map<Category>(entity);
            await this.context.Categories.AddAsync(category);
            await this.context.SaveChangesAsync();

            return this.CreatedAtRoute("GetCategory", new { id = category.Id }, entity);
        }

        /// <summary>
        /// Updates a specific <see cref="Category"/>.
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="entity">entity</param>
        /// <returns>None</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update(long id, [FromBody] SaveCategoryRequest entity)
        {
            var category = await this.context.Categories.SingleOrDefaultAsync(t => t.Id == id);
            if (category == null)
            {
                return this.NotFound(id);
            }

            try
            {
                this.mapper.Map(entity, category);
                this.context.Update(category);
                await this.context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException concurrencyEx)
            {
                return this.BadRequest(concurrencyEx);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex);
            }

            return new NoContentResult();
        }

        /// <summary>
        /// Deletes a specific <see cref="Category"/>.
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>None</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(long id)
        {
            var category = await this.context.Categories.SingleOrDefaultAsync(t => t.Id == id);
            if (category == null)
            {
                return this.NotFound(id);
            }

            category.IsDeleted = true;
            this.context.Update(category);
            await this.context.SaveChangesAsync();

            return new NoContentResult();
        }
    }
}