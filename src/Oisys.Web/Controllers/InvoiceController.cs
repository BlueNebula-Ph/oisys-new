using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OisysNew.DTO;
using OisysNew.DTO.Invoice;
using OisysNew.Extensions;
using OisysNew.Helpers;
using OisysNew.Helpers.Interfaces;
using OisysNew.Models;
using OisysNew.Services;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace OisysNew.Controllers
{
    /// <summary>
    /// <see cref="InvoiceController"/> class handles adding, editing, deleting and fetching of invoices.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : Controller
    {
        private readonly IOisysDbContext context;
        private readonly IMapper mapper;
        private readonly IListHelpers listHelpers;
        private readonly IEntityListHelpers entityListHelpers;
        private readonly IOrderService orderService;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="InvoiceController"/> class.
        /// </summary>
        /// <param name="context">The DbContext</param>
        /// <param name="mapper">Automapper</param>
        /// <param name="listHelpers">List helper</param>
        /// <param name="entityListHelpers">Entity list helper</param>
        /// <param name="logger">The logger</param>
        public InvoiceController(IOisysDbContext context, 
            IMapper mapper, 
            IListHelpers listHelpers,
            IEntityListHelpers entityListHelpers,
            IOrderService orderService,
            ILogger<InvoiceController> logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.listHelpers = listHelpers;
            this.entityListHelpers = entityListHelpers;
            this.orderService = orderService;
            this.logger = logger;
        }

        /// <summary>
        /// Returns list of active <see cref="Invoice"/>
        /// </summary>
        /// <param name="filter"><see cref="InvoiceFilterRequest"/></param>
        /// <returns>List of Invoices</returns>
        [HttpPost("search", Name = "GetAllInvoice")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaginatedList<InvoiceSummary>>> GetAll([FromBody]InvoiceFilterRequest filter)
        {
            try
            {
                // get list of active customers (not deleted)
                var list = context.Invoices
                    .Include(c => c.Customer)
                    .AsNoTracking();

                // filter
                if (!string.IsNullOrEmpty(filter?.SearchTerm))
                {
                    list = list.Where(c => c.InvoiceNumber.ToString().Contains(filter.SearchTerm));
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

                // sort
                var ordering = $"{Constants.ColumnNames.InvoiceNumber} {Constants.DefaultSortDirection}";
                if (!string.IsNullOrEmpty(filter?.SortBy))
                {
                    ordering = $"{filter.SortBy} {filter.SortDirection}";
                }

                list = list.OrderBy(ordering);

                var entities = await listHelpers.CreatePaginatedListAsync<Invoice, InvoiceSummary>(list, filter.PageNumber, filter.PageSize);
                return entities;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Gets a specific <see cref="Invoice"/>.
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>Invoice</returns>
        [HttpGet("{id}", Name = "GetInvoiceById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<InvoiceDetail>> GetInvoiceById(long id)
        {
            try
            {
                var entity = await context.Invoices
                .AsNoTracking()
                .SingleOrDefaultAsync(c => c.Id == id);

                if (entity == null)
                {
                    return NotFound();
                }

                var invoiceDetail = mapper.Map<InvoiceDetail>(entity);
                return invoiceDetail;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Creates a <see cref="Invoice"/>.
        /// </summary>
        /// <param name="entity">entity to be created</param>
        /// <returns>Invoice</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Create([FromBody]SaveInvoiceRequest entity)
        {
            try
            {
                var invoice = mapper.Map<Invoice>(entity);
                await orderService.ProcessInvoice(invoice.LineItems, true);

                await context.Invoices.AddAsync(invoice);
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
        /// Updates a specific <see cref="Invoice"/>.
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
        public async Task<ActionResult> Update(long id, [FromBody]SaveInvoiceRequest entity)
        {
            var invoiceExists = await this.context.Invoices
                .AnyAsync(c => c.Id == id);

            if (!invoiceExists)
            {
                return NotFound(id);
            }

            try
            {
                // Map the entity to an invoice object
                var invoice = this.mapper.Map<Invoice>(entity);

                this.context.Update(invoice);

                await this.context.SaveChangesAsync();
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
        /// Deletes a specific <see cref="Invoice"/>.
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
                var invoice = await context.Invoices
                    .Include(c => c.LineItems)
                    .SingleOrDefaultAsync(c => c.Id == id);

                if (invoice == null)
                {
                    return NotFound();
                }

                await orderService.ProcessInvoice(invoice.LineItems, false);
                context.Remove(invoice);

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