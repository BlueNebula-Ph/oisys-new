using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OisysNew.DTO;
using OisysNew.DTO.CreditMemo;
using OisysNew.Exceptions;
using OisysNew.Extensions;
using OisysNew.Helpers;
using OisysNew.Helpers.Interfaces;
using OisysNew.Models;
using OisysNew.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace OisysNew.Controllers
{
    /// <summary>
    /// <see cref="CreditMemoController"/> handles adding, updating and deleting of credit memos.
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CreditMemoController : Controller
    {
        private readonly IOisysDbContext context;
        private readonly IMapper mapper;
        private readonly IListHelpers listHelpers;
        private readonly IEntityListHelpers entityListHelpers;
        private readonly IInventoryService inventoryService;
        private readonly IOrderService orderService;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreditMemoController"/> class.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        /// <param name="listHelpers"></param>
        /// <param name="entityListHelpers"></param>
        /// <param name="inventoryService"></param>
        /// <param name="orderService"></param>
        /// <param name="logger"></param>
        public CreditMemoController(
            IOisysDbContext context,
            IMapper mapper,
            IListHelpers listHelpers,
            IEntityListHelpers entityListHelpers,
            IInventoryService inventoryService,
            IOrderService orderService,
            ILogger<CreditMemoController> logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.listHelpers = listHelpers;
            this.entityListHelpers = entityListHelpers;
            this.inventoryService = inventoryService;
            this.orderService = orderService;
            this.logger = logger;
        }

        /// <summary>
        /// Returns list of active <see cref="CreditMemo"/>
        /// </summary>
        /// <param name="filter"><see cref="CreditMemoFilterRequest"/></param>
        /// <returns>List of CreditMemos</returns>
        [HttpPost("search", Name = "GetAllCreditMemos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaginatedList<CreditMemoSummary>>> GetAll([FromBody]CreditMemoFilterRequest filter)
        {
            try
            {
                // get list of active creditMemos (not deleted)
                var list = context.CreditMemos.AsNoTracking();

                // filter
                if (!string.IsNullOrEmpty(filter?.SearchTerm))
                {
                    list = list.Where(c => c.Code.ToString().Contains(filter.SearchTerm));
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
                    list = list.Where(c => c.LineItems.Any(d => d.OrderLineItem.ItemId == filter.ItemId));
                }

                var isInvoiced = filter?.IsInvoiced ?? false;
                list = list.Where(c => c.IsInvoiced == isInvoiced);

                // sort
                var ordering = $"{Constants.ColumnNames.Code} {Constants.DefaultSortDirection}";
                if (!string.IsNullOrEmpty(filter?.SortBy))
                {
                    ordering = $"{filter.SortBy} {filter.SortDirection}";
                }

                list = list.OrderBy(ordering);

                var entities = await listHelpers.CreatePaginatedListAsync<CreditMemo, CreditMemoSummary>(list, filter.PageNumber, filter.PageSize);
                return entities;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Gets a specific <see cref="CreditMemo"/>.
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>CreditMemo</returns>
        [HttpGet("{id}", Name = "GetCreditMemoById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CreditMemoDetail>> GetCreditMemoById(long id)
        {
            try
            {
                var entity = await context.CreditMemos
                    .Include(c => c.Customer)
                        .ThenInclude(c => c.Province)
                    .Include(c => c.Customer)
                        .ThenInclude(c => c.City)
                    .Include(c => c.LineItems)
                        .ThenInclude(lineItem => (lineItem as CreditMemoLineItem).Item)
                    .Include(c => c.LineItems)
                        .ThenInclude(lineItem => (lineItem as CreditMemoLineItem).OrderLineItem)
                        .ThenInclude(orderLineItem => orderLineItem.Order)
                        .ThenInclude(order => order.Customer)
                        .ThenInclude(customer => customer.Province)
                    .Include(c => c.LineItems)
                        .ThenInclude(lineItem => (lineItem as CreditMemoLineItem).OrderLineItem)
                        .ThenInclude(orderLineItem => orderLineItem.Order)
                        .ThenInclude(order => order.Customer)
                        .ThenInclude(customer => customer.City)
                    .Include(c => c.LineItems)
                        .ThenInclude(lineItem => (lineItem as CreditMemoLineItem).OrderLineItem)
                        .ThenInclude(orderLineItem => orderLineItem.Item)
                        .ThenInclude(item => item.Category)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(c => c.Id == id);

                if (entity == null)
                {
                    return NotFound();
                }

                var creditMemoDetail = mapper.Map<CreditMemoDetail>(entity);
                return creditMemoDetail;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Returns list of active <see cref="CreditMemo"/>
        /// </summary>
        /// <param name="customerId">Customer Id</param>
        /// <returns>List of Credit Memo per Customer</returns>
        [HttpGet("{customerId}/lookup/{isInvoiced?}", Name = "GetCreditMemoLookup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CreditMemoLookup>>> GetLookup(int customerId, bool isInvoiced = false)
        {
            // get list of active items (not deleted)
            var list = context.CreditMemos
                .AsNoTracking()
                .Where(c => c.CustomerId == customerId && c.IsInvoiced == isInvoiced)
                .OrderBy(c => c.Code);

            var creditMemos = await list.ProjectTo<CreditMemoLookup>(mapper.ConfigurationProvider).ToListAsync();
            return creditMemos;
        }

        /// <summary>
        /// Creates a <see cref="CreditMemo"/>.
        /// </summary>
        /// <param name="entity">entity to be created</param>
        /// <returns>CreditMemo</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Create([FromBody]SaveCreditMemoRequest entity)
        {
            try
            {
                var creditMemo = mapper.Map<CreditMemo>(entity);
                await context.CreditMemos.AddAsync(creditMemo);

                // Adjust the inventory quantities for items returned
                var itemsToBeReturned = creditMemo.LineItems.Where(a => a.ReturnedToInventory);
                await inventoryService.ProcessAdjustments(itemsToBeReturned, AdjustmentType.Add, Constants.AdjustmentRemarks.CreditMemoCreated);

                // Update the order line item for quantity returned
                await orderService.ProcessReturns(creditMemo.LineItems, AdjustmentType.Add);

                // TODO: Add a customer transaction record for crediting

                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status201Created);
            }
            catch (QuantityReturnedException qEx)
            {
                logger.LogError(qEx.Message);
                ModelState.AddModelError(Constants.ErrorMessage, qEx.Message);

                return BadRequest(ModelState);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Updates a specific <see cref="CreditMemo"/>.
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="updatedCreditMemo">the updated credit memo</param>
        /// <returns>None</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Update(long id, [FromBody]SaveCreditMemoRequest updatedCreditMemo)
        {
            try
            {
                var creditMemo = context.CreditMemos
                        .Include(c => c.Customer)
                        .Include(c => c.LineItems).ThenInclude(detail => (detail as CreditMemoLineItem).TransactionHistory)
                        .Include(c => c.LineItems).ThenInclude(detail => (detail as CreditMemoLineItem).OrderLineItem).ThenInclude(orderLineItem => orderLineItem.Item)
                        .AsNoTracking()
                        .SingleOrDefault(c => c.Id == id);

                if (creditMemo == null)
                {
                    return NotFound();
                }

                // Adjust inventory quantities
                var itemsToBeCleared = creditMemo.LineItems.Where(a => a.ReturnedToInventory);
                await inventoryService.ProcessAdjustments(itemsToBeCleared, AdjustmentType.Deduct, Constants.AdjustmentRemarks.CreditMemoUpdated);

                var itemsToBeReturned = updatedCreditMemo.LineItems.Where(a => a.ShouldAddBackToInventory);
                await inventoryService.ProcessAdjustments(itemsToBeReturned, AdjustmentType.Add, Constants.AdjustmentRemarks.CreditMemoUpdated);

                // Update order line items for quantity returned
                await orderService.ProcessReturns(creditMemo.LineItems, AdjustmentType.Deduct);
                await orderService.ProcessReturns(updatedCreditMemo.LineItems, AdjustmentType.Add);

                // Update the credit memo values
                creditMemo = mapper.Map<CreditMemo>(updatedCreditMemo);

                context.Update(creditMemo);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (QuantityReturnedException qEx)
            {
                logger.LogError(qEx.Message);
                ModelState.AddModelError(Constants.ErrorMessage, qEx.Message);

                return BadRequest(ModelState);
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
        /// Deletes a specific <see cref="CreditMemo"/>.
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
                var creditMemo = await context.CreditMemos
                                .Include(c => c.LineItems)
                                    .ThenInclude(detail => (detail as CreditMemoLineItem).OrderLineItem)
                                        .ThenInclude(orderLineItem => orderLineItem.Item)
                                .Include(c => c.LineItems)
                                    .ThenInclude(detail => (detail as CreditMemoLineItem).TransactionHistory)
                                .AsNoTracking()
                                .SingleOrDefaultAsync(c => c.Id == id);

                if (creditMemo == null)
                {
                    return NotFound(id);
                }

                // Adjust inventory quantities
                var itemsToBeCleared = creditMemo.LineItems.Where(a => a.ReturnedToInventory);
                await inventoryService.ProcessAdjustments(itemsToBeCleared, AdjustmentType.Deduct, Constants.AdjustmentRemarks.CreditMemoDeleted);

                // Update order line items for quantity returned
                await orderService.ProcessReturns(creditMemo.LineItems, AdjustmentType.Deduct);

                // TODO: Update customer transaction record for crediting


                context.Remove(creditMemo);
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