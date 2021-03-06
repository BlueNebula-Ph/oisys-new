﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OisysNew.DTO;
using OisysNew.DTO.Province;
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
    /// <see cref="ProvinceController"/> class handles basic add, edit, delete and fetching of provinces.
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProvinceController : ControllerBase
    {
        private readonly IOisysDbContext context;
        private readonly IMapper mapper;
        private readonly IListHelpers listHelpers;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProvinceController"/> class.
        /// </summary>
        /// <param name="context">DbContext</param>
        /// <param name="mapper">Automapper</param>
        /// <param name="listHelpers">List helper</param>
        /// <param name="logger">The logger</param>
        public ProvinceController(
            IOisysDbContext context, 
            IMapper mapper, 
            IListHelpers listHelpers,
            ILogger<ProvinceController> logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.listHelpers = listHelpers;
            this.logger = logger;
        }

        /// <summary>
        /// Returns list of active <see cref="Province"/>
        /// </summary>
        /// <param name="filter"><see cref="ProvinceFilterRequest"/></param>
        /// <returns>List of Province</returns>
        [HttpPost("search", Name = "GetAllProvince")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaginatedList<ProvinceSummary>>> GetAll([FromBody]ProvinceFilterRequest filter)
        {
            try
            {
                // get list of active sales quote (not deleted)
                var list = context.Provinces
                    .Include(c => c.Cities)
                    .AsNoTracking();

                // filter
                if (!string.IsNullOrEmpty(filter?.SearchTerm))
                {
                    list = list.Where(c => c.Name.Contains(filter.SearchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                        c.Cities.Any(a => a.Name.Contains(filter.SearchTerm, StringComparison.CurrentCultureIgnoreCase)));
                }

                // sort
                var ordering = $"{Constants.ColumnNames.Name} {Constants.DefaultSortDirection}";
                if (!string.IsNullOrEmpty(filter?.SortBy))
                {
                    ordering = $"{filter.SortBy} {filter.SortDirection}";
                }

                list = list.OrderBy(ordering);

                var result = await listHelpers.CreatePaginatedListAsync<Province, ProvinceSummary>(list, filter.PageNumber, filter.PageSize);
                return result;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Returns list of active <see cref="Province"/>
        /// </summary>
        /// <returns>List of Provinces</returns>
        [HttpGet("lookup/{name?}", Name = "GetProvinceLookup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ProvinceLookup>>> GetLookup(string name = "")
        {
            try
            {
                // get list of active items (not deleted)
                var list = context.Provinces.AsNoTracking();

                if (!string.IsNullOrEmpty(name))
                {
                    list = list.Where(a => a.Name.StartsWith(name, StringComparison.CurrentCultureIgnoreCase));
                }

                // sort
                var ordering = $"Name {Constants.DefaultSortDirection}";

                list = list.OrderBy(ordering);

                var provinces = await list.ProjectTo<ProvinceLookup>(mapper.ConfigurationProvider).ToListAsync();
                return provinces;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Gets a specific <see cref="Province"/>.
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>Province</returns>
        [HttpGet("{id}", Name = "GetProvinceById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProvinceSummary>> GetProvinceById(long id)
        {
            try
            {
                var entity = await context.Provinces
                    .Include(c => c.Cities)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(c => c.Id == id);

                if (entity == null)
                {
                    return NotFound(id);
                }

                var province = mapper.Map<ProvinceSummary>(entity);
                return province;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Creates a <see cref="Province"/>.
        /// </summary>
        /// <param name="entity">entity to be created</param>
        /// <returns>Province</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SaveProvinceRequest>> Create([FromBody]SaveProvinceRequest entity)
        {
            try
            {
                var province = mapper.Map<Province>(entity);
                await context.Provinces.AddAsync(province);
                await context.SaveChangesAsync();

                return CreatedAtRoute(nameof(GetProvinceById), new { id = province.Id }, entity);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Updates a specific <see cref="Province"/>.
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="entity">entity</param>
        /// <returns>None</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Update(long id, [FromBody]SaveProvinceRequest entity)
        {
            try
            {
                var province = await context.Provinces
                    .AsNoTracking()
                    .SingleOrDefaultAsync(t => t.Id == id);

                if (province == null)
                {
                    return NotFound(id);
                }

                province = mapper.Map<Province>(entity);
                context.Update(province);
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
        /// Deletes a specific <see cref="Province"/>.
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
                var province = await context.Provinces
                    .SingleOrDefaultAsync(c => c.Id == id);

                if (province == null)
                {
                    return NotFound(id);
                }

                province.IsDeleted = true;
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