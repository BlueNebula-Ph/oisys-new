using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OisysNew.DTO;
using OisysNew.DTO.Province;
using OisysNew.Helpers;
using OisysNew.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace OisysNew.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ProvinceController : ControllerBase
    {
        private readonly OisysDbContext context;
        private readonly IMapper mapper;
        private readonly IListHelpers listHelpers;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProvinceController"/> class.
        /// </summary>
        /// <param name="context">DbContext</param>
        /// <param name="mapper">Automapper</param>
        /// <param name="listHelpers">List helper</param>
        public ProvinceController(OisysDbContext context, IMapper mapper, IListHelpers listHelpers)
        {
            this.context = context;
            this.mapper = mapper;
            this.listHelpers = listHelpers;
        }

        /// <summary>
        /// Returns list of active <see cref="Province"/>
        /// </summary>
        /// <param name="filter"><see cref="ProvinceFilterRequest"/></param>
        /// <returns>List of Province</returns>
        [HttpPost("search", Name = "GetAllProvince")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PaginatedList<ProvinceSummary>>> GetAll([FromBody]ProvinceFilterRequest filter)
        {
            // get list of active sales quote (not deleted)
            var list = this.context.Provinces
                .AsNoTracking()
                .Include(c => c.Cities)
                .Where(c => !c.IsDeleted);

            // filter
            if (!string.IsNullOrEmpty(filter?.SearchTerm))
            {
                list = list.Where(c => c.Name.Contains(filter.SearchTerm, StringComparison.CurrentCultureIgnoreCase) || 
                    c.Cities.Any(a => a.Name.Contains(filter.SearchTerm, StringComparison.CurrentCultureIgnoreCase)));
            }

            // sort
            var ordering = $"Name {Constants.DefaultSortDirection}";
            if (!string.IsNullOrEmpty(filter?.SortBy))
            {
                ordering = $"{filter.SortBy} {filter.SortDirection}";
            }

            list = list.OrderBy(ordering);

            var result = await this.listHelpers.CreatePaginatedListAsync<Province, ProvinceSummary>(list, filter.PageNumber, filter.PageSize);
            return result;
        }

        /// <summary>
        /// Returns list of active <see cref="Province"/>
        /// </summary>
        /// <returns>List of Provinces</returns>
        [HttpGet("lookup", Name = "GetProvinceLookup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProvinceLookup>>> GetLookup()
        {
            // get list of active items (not deleted)
            var list = this.context.Provinces
                .AsNoTracking()
                .Where(c => !c.IsDeleted);

            // sort
            var ordering = $"Name {Constants.DefaultSortDirection}";

            list = list.OrderBy(ordering);

            var provinces = await list.ProjectTo<ProvinceLookup>(this.mapper.ConfigurationProvider).ToListAsync();
            return provinces;
        }

        /// <summary>
        /// Gets a specific <see cref="Province"/>.
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>Province</returns>
        [HttpGet("{id}", Name = "GetProvince")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProvinceSummary>> GetById(long id)
        {
            var entity = await this.context.Provinces
                .AsNoTracking()
                .Include(c => c.Cities)
                .SingleOrDefaultAsync(c => c.Id == id);

            if (entity == null)
            {
                return this.NotFound(id);
            }

            var province = this.mapper.Map<ProvinceSummary>(entity);
            return province;
        }

        /// <summary>
        /// Creates a <see cref="Province"/>.
        /// </summary>
        /// <param name="entity">entity to be created</param>
        /// <returns>Province</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SaveProvinceRequest>> Create([FromBody]SaveProvinceRequest entity)
        {
            // TODO: Move to a filter
            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            var province = this.mapper.Map<Province>(entity);
            await this.context.Provinces.AddAsync(province);
            await this.context.SaveChangesAsync();

            return this.CreatedAtRoute("GetProvince", new { id = province.Id }, entity);
        }

        /// <summary>
        /// Updates a specific <see cref="Province"/>.
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="entity">entity</param>
        /// <returns>None</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update(long id, [FromBody]SaveProvinceRequest entity)
        {
            var province = await this.context.Provinces
                .AsNoTracking()
                .SingleOrDefaultAsync(t => t.Id == id);

            if (province == null)
            {
                return this.NotFound(id);
            }

            try
            {
                province = this.mapper.Map<Province>(entity);
                this.context.Update(province);
                await this.context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException concurrencyEx)
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
        /// Deletes a specific <see cref="Province"/>.
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>None</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(long id)
        {
            var city = await this.context.Provinces
                .SingleOrDefaultAsync(c => c.Id == id);

            if (city == null)
            {
                return this.NotFound(id);
            }

            city.IsDeleted = true;
            await this.context.SaveChangesAsync();
            return new NoContentResult();
        }
    }
}