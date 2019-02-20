using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OisysNew.DTO;
using OisysNew.DTO.CashVoucher;
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
    /// <see cref="CashVoucherController"/> class handles CashVoucher basic add, edit, delete and get.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CashVoucherController : Controller
    {
        private readonly IOisysDbContext context;
        private readonly IMapper mapper;
        private readonly IListHelpers listHelpers;
        private readonly IEntityListHelpers entityListHelpers;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CashVoucherController"/> class.
        /// </summary>
        /// <param name="context">DbContext</param>
        /// <param name="mapper">Automapper</param>
        /// <param name="listHelpers">List helpers</param>
        /// <param name="entityListHelpers">Entity list helpers</param>
        /// <param name="logger">The logger</param>
        public CashVoucherController(
            IOisysDbContext context,
            IMapper mapper,
            IListHelpers listHelpers,
            IEntityListHelpers entityListHelpers,
            ILogger<CashVoucherController> logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.listHelpers = listHelpers;
            this.entityListHelpers = entityListHelpers;
            this.logger = logger;
        }

        /// <summary>
        /// Returns list of active <see cref="CashVoucher"/>
        /// </summary>
        /// <param name="filter"><see cref="CashVoucherFilterRequest"/></param>
        /// <returns>List of CashVoucher</returns>
        [HttpPost("search", Name = "GetAllCashVouchers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaginatedList<CashVoucherSummary>>> GetAll([FromBody]CashVoucherFilterRequest filter)
        {
            try
            {
                // get list of active cashVouchers (not deleted)
                var list = context.CashVouchers.AsNoTracking();

                // filter
                if (!string.IsNullOrEmpty(filter?.SearchTerm))
                {
                    list = list.Where(c => c.VoucherNumber.ToString().Contains(filter.SearchTerm) || c.PayTo.Contains(filter.SearchTerm));
                }

                if (filter?.DateFrom != null || filter?.DateTo != null)
                {
                    filter.DateFrom = filter.DateFrom == null || filter.DateFrom == DateTime.MinValue ? DateTime.Today : filter.DateFrom;
                    filter.DateTo = filter.DateTo == null || filter.DateTo == DateTime.MinValue ? DateTime.Today : filter.DateTo;
                    list = list.Where(c => c.Date >= filter.DateFrom && c.Date < filter.DateTo.Value.AddDays(1));
                }

                // sort
                var ordering = $"{Constants.ColumnNames.VoucherNumber} {Constants.DefaultSortDirection}";
                if (!string.IsNullOrEmpty(filter?.SortBy))
                {
                    ordering = $"{filter.SortBy} {filter.SortDirection}";
                }

                list = list.OrderBy(ordering);

                var entities = await listHelpers.CreatePaginatedListAsync<CashVoucher, CashVoucherSummary>(list, filter.PageNumber, filter.PageSize);
                return entities;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Gets a specific <see cref="CashVoucher"/>.
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>CashVoucher</returns>
        [HttpGet("{id}", Name = "GetCashVoucherById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CashVoucherSummary>> GetCashVoucherById(long id)
        {
            try
            {
                var entity = await context.CashVouchers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (entity == null)
                {
                    return NotFound();
                }

                var cashVoucherSummary = mapper.Map<CashVoucherSummary>(entity);
                return cashVoucherSummary;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Creates a <see cref="CashVoucher"/>.
        /// </summary>
        /// <param name="entity">entity to be created</param>
        /// <returns>CashVoucher</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Create([FromBody]SaveCashVoucherRequest entity)
        {
            try
            {
                var cashVoucher = mapper.Map<CashVoucher>(entity);
                await context.CashVouchers.AddAsync(cashVoucher);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Updates a specific <see cref="CashVoucher"/>.
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
        public async Task<ActionResult> Update(long id, [FromBody]SaveCashVoucherRequest entity)
        {
            var cashVoucher = await context.CashVouchers.SingleOrDefaultAsync(t => t.Id == id);
            if (cashVoucher == null)
            {
                return NotFound();
            }

            try
            {
                mapper.Map(entity, cashVoucher);
                context.Update(cashVoucher);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (DbUpdateConcurrencyException concurrencyEx)
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
        /// Deletes a specific <see cref="CashVoucher"/>.
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
                var cashVoucher = await context.CashVouchers.SingleOrDefaultAsync(t => t.Id == id);
                if (cashVoucher == null)
                {
                    return NotFound();
                }

                context.Remove(cashVoucher);
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