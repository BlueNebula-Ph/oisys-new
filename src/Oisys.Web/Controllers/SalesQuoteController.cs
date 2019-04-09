using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OisysNew.DTO;
using OisysNew.DTO.SalesQuote;
using OisysNew.Extensions;
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
    /// <see cref="SalesQuoteController"/> class handles basic add, edit, delete and fetching of sales quotations.
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SalesQuoteController : Controller
    {
        private readonly IOisysDbContext context;
        private readonly IMapper mapper;
        private readonly IListHelpers listHelpers;
        private readonly IEntityListHelpers entityListHelpers;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SalesQuoteController"/> class.
        /// </summary>
        /// <param name="context">DbContext</param>
        /// <param name="mapper">Automapper</param>
        /// <param name="listHelpers">List helpers</param>
        public SalesQuoteController(
            IOisysDbContext context,
            IMapper mapper,
            IListHelpers listHelpers,
            IEntityListHelpers entityListHelpers,
            ILogger<SalesQuoteController> logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.listHelpers = listHelpers;
            this.entityListHelpers = entityListHelpers;
            this.logger = logger;
        }

        /// <summary>
        /// Returns list of active <see cref="SalesQuote"/>
        /// </summary>
        /// <param name="filter"><see cref="SalesQuoteFilterRequest"/></param>
        /// <returns>List of SalesQuote</returns>
        [HttpPost("search", Name = "GetAllSalesQuote")]
        [ProducesResponseType(typeof(PaginatedList<SalesQuoteSummary>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaginatedList<SalesQuoteSummary>>> GetAll([FromBody]SalesQuoteFilterRequest filter)
        {
            try
            {
                // get list of active sales quote (not deleted)
                var list = context.SalesQuotes.AsNoTracking();

                // filter
                if (!string.IsNullOrEmpty(filter?.SearchTerm))
                {
                    list = list.Where(c => c.QuoteNumber.ToString().Contains(filter.SearchTerm));
                }

                if (!(filter?.ProvinceId).IsNullOrZero())
                {
                    list = list.Where(c => c.Customer.ProvinceId == filter.ProvinceId);
                }

                if (!(filter?.CustomerId).IsNullOrZero())
                {
                    list = list.Where(c => c.CustomerId == filter.CustomerId);
                }

                if (filter?.DateFrom != null || filter?.DateTo != null)
                {
                    filter.DateFrom = filter.DateFrom == null || filter.DateFrom == DateTime.MinValue ? DateTime.Today : filter.DateFrom;
                    filter.DateTo = filter.DateTo == null || filter.DateTo == DateTime.MinValue ? DateTime.Today : filter.DateTo;
                    list = list.Where(c => c.Date >= filter.DateFrom && c.Date < filter.DateTo.Value.AddDays(1));
                }

                if (!(filter?.ItemId).IsNullOrZero())
                {
                    list = list.Where(c => c.LineItems.Any(d => d.ItemId == filter.ItemId));
                }

                // sort
                var ordering = $"{Constants.ColumnNames.QuoteNumber} {Constants.DefaultSortDirection}";
                if (!string.IsNullOrEmpty(filter?.SortBy))
                {
                    ordering = $"{filter.SortBy} {filter.SortDirection}";
                }

                list = list.OrderBy(ordering);

                var salesQuotations = await listHelpers.CreatePaginatedListAsync<SalesQuote, SalesQuoteSummary>(list, filter.PageNumber, filter.PageSize);
                return salesQuotations;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Gets a specific <see cref="SalesQuote"/>.
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>SalesQuote</returns>
        [HttpGet("{id}", Name = "GetSalesQuote")]
        [ProducesResponseType(typeof(SalesQuoteDetail), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SalesQuoteDetail>> GetById(long id)
        {
            try
            {
                var salesQuotation = await context.SalesQuotes
                    .Include(c => c.Customer).ThenInclude(y => y.Province)
                    .Include(c => c.Customer).ThenInclude(y => y.City)
                    .Include(c => c.LineItems).ThenInclude(d => d.Item).ThenInclude(f => f.Category)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(c => c.Id == id);

                if (salesQuotation == null)
                {
                    return NotFound();
                }

                var salesQuotationDetail = mapper.Map<SalesQuoteDetail>(salesQuotation);
                return salesQuotationDetail;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Creates a <see cref="SalesQuote"/>.
        /// </summary>
        /// <param name="entity">entity to be created</param>
        /// <returns>SalesQuote</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Create([FromBody]SaveSalesQuoteRequest entity)
        {
            try
            {
                var salesQuote = mapper.Map<SalesQuote>(entity);

                await context.SalesQuotes.AddAsync(salesQuote);
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
        /// Updates a specific <see cref="SalesQuote"/>.
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
        public async Task<ActionResult> Update(long id, [FromBody]SaveSalesQuoteRequest entity)
        {
            try
            {
                var salesQuote = await context.SalesQuotes
                    .Include(a => a.LineItems)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(t => t.Id == id);

                if (salesQuote == null)
                {
                    return NotFound(id);
                }

                // Process deleted line items
                entityListHelpers.CheckItemsForDeletion(salesQuote.LineItems, entity.LineItems);

                // Update the sales quotation to new values
                salesQuote = mapper.Map<SalesQuote>(entity);

                context.Update(salesQuote);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (DbUpdateConcurrencyException concurrencyEx)
            {
                logger.LogError(concurrencyEx.Message);
                return StatusCode(StatusCodes.Status409Conflict, Constants.ErrorMessages.ConcurrencyErrorMessage);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Deletes a specific <see cref="SalesQuote"/>.
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>None</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                var salesQuote = await context
                    .SalesQuotes
                    .FindAsync(id);

                if (salesQuote == null)
                {
                    return NotFound();
                }

                context.Remove(salesQuote);
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