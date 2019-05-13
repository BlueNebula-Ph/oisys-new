using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OisysNew.DTO;
using OisysNew.DTO.Item;
using OisysNew.Extensions;
using OisysNew.Filters;
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
    /// <see cref="ItemController"/> class handles Item basic add, edit, delete and get.
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : Controller
    {
        private readonly IOisysDbContext context;
        private readonly IMapper mapper;
        private readonly IListHelpers listHelpers;
        private readonly IInventoryService inventoryService;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemController"/> class.
        /// </summary>
        /// <param name="context">DbContext</param>
        /// <param name="mapper">Automapper</param>
        /// <param name="listHelpers">List Helpers</param>
        /// <param name="inventoryService">Inventory service</param>
        /// <param name="logger">Logger</param>
        public ItemController(
            IOisysDbContext context,
            IMapper mapper,
            IListHelpers listHelpers,
            IInventoryService inventoryService,
            ILogger<ItemController> logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.listHelpers = listHelpers;
            this.inventoryService = inventoryService;
            this.logger = logger;
        }

        /// <summary>
        /// Returns list of active <see cref="Item"/>
        /// </summary>
        /// <param name="filter"><see cref="ItemFilterRequest"/></param>
        /// <returns>List of Items</returns>
        [HttpPost("search", Name = "GetAllItems")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaginatedList<ItemSummary>>> GetAll([FromBody]ItemFilterRequest filter)
        {
            try
            {
                // get list of active items (not deleted)
                var list = context.Items.AsNoTracking();

                // filter
                if (!string.IsNullOrEmpty(filter?.SearchTerm))
                {
                    list = list.Where(c => c.Code.Contains(filter.SearchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                        c.Name.Contains(filter.SearchTerm, StringComparison.CurrentCultureIgnoreCase));
                }

                if (!(filter?.CategoryId).IsNullOrZero())
                {
                    list = list.Where(c => c.CategoryId == filter.CategoryId);
                }

                if (filter?.IsQuantityLow ?? false)
                {
                    list = list.Where(c => c.Quantity < Constants.DefaultLowQuantity);
                }

                // sort
                var ordering = $"{Constants.ColumnNames.Code} {Constants.DefaultSortDirection}";
                if (!string.IsNullOrEmpty(filter?.SortBy))
                {
                    ordering = $"{filter.SortBy} {filter.SortDirection}";
                }

                list = list.OrderBy(ordering);

                var result = await listHelpers.CreatePaginatedListAsync<Item, ItemSummary>(list, filter.PageNumber, filter.PageSize);
                return result;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Returns list of active <see cref="Item"/>
        /// </summary>
        /// <returns>List of Items</returns>
        [HttpGet("lookup/{code?}", Name = "GetItemLookup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ItemLookup>>> GetLookup(string code = "")
        {
            try
            {
                // get list of active items (not deleted)
                var list = context.Items
                    .AsNoTracking();

                if (!string.IsNullOrEmpty(code))
                {
                    list = list.Where(a => a.Code.StartsWith(code, StringComparison.CurrentCultureIgnoreCase));
                }

                // sort
                var ordering = $"{Constants.ColumnNames.Name} {Constants.DefaultSortDirection}";

                list = list.OrderBy(ordering);

                var items = await list.ProjectTo<ItemLookup>(mapper.ConfigurationProvider).ToListAsync();
                return items;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Gets a specific <see cref="Item"/>.
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>Item</returns>
        [HttpGet("{id}", Name = "GetItemById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ItemDetail>> GetItemById(long id)
        {
            try
            {
                var entity = await context.Items
                    .Include(a => a.Category)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(c => c.Id == id);

                if (entity == null)
                {
                    return NotFound();
                }

                var itemDetail = mapper.Map<ItemDetail>(entity);
                return itemDetail;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Gets <see cref="ItemHistory"/> of an item.
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="page">page index requested (0 based)</param>
        /// <param name="size">page size requested</param>
        /// <returns>List of item histories</returns>
        [HttpGet("{id}/history", Name = "GetItemTransactionHistory")]
        [ProducesResponseType(typeof(PaginatedList<ItemHistorySummary>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaginatedList<ItemHistorySummary>>> GetItemTransactionHistory([FromRoute]long id, [FromQuery]int page, [FromQuery]int size)
        {
            try
            {
                var transactionHistories = context.ItemHistories
                    .AsNoTracking()
                    .Where(a => a.ItemId == id)
                    .OrderByDescending(a => a.Date);

                return await listHelpers.CreatePaginatedListAsync<ItemHistory, ItemHistorySummary>(
                    transactionHistories, page, size);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Gets item adjustments
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="page">page index requested (0 based)</param>
        /// <param name="size">page size requested</param>
        /// <returns>List of item histories</returns>
        [HttpGet("adjustments/{category}", Name = "GetAdjustments")]
        [ProducesResponseType(typeof(PaginatedList<ItemAdjustmentSummary>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaginatedList<ItemAdjustmentSummary>>> GetAdjustments([FromRoute]int category, [FromQuery]int page, [FromQuery]int size, [FromQuery]int itemId = 0)
        {
            try
            {
                var adjustments = context.Adjustments
                    .AsNoTracking()
                    .Where(a => (int)a.Category == category);

                if (itemId != 0)
                {
                    adjustments = adjustments.Where(a => a.ItemId == itemId);
                }

                adjustments = adjustments.OrderByDescending(a => a.AdjustmentDate);

                return await listHelpers.CreatePaginatedListAsync<Adjustment, ItemAdjustmentSummary>(adjustments, page, size);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Gets <see cref="ItemOrderSummary"/> of an item.
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="page">page index requested (0 based)</param>
        /// <param name="size">page size requested</param>
        /// <returns>List of item order history</returns>
        [HttpGet("{id}/orders", Name = "GetItemOrderHistory")]
        [ProducesResponseType(typeof(PaginatedList<ItemOrderSummary>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaginatedList<ItemOrderSummary>>> GetItemOrderHistory([FromRoute]long id, [FromQuery]int page, [FromQuery]int size)
        {
            try
            {
                var orderHistory = context.OrderLineItems
                    .Include(a => a.Order)
                        .ThenInclude(a => a.Customer)
                    .AsNoTracking()
                    .Where(a => a.ItemId == id)
                    .OrderByDescending(a => a.Order.Date);

                return await listHelpers.CreatePaginatedListAsync<OrderLineItem, ItemOrderSummary>(
                    orderHistory, page, size);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Creates a <see cref="Item"/>.
        /// </summary>
        /// <param name="entity">entity to be created</param>
        /// <returns>Item</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ServiceFilter(typeof(ValidateDuplicateItemCodeAttribute))]
        public async Task<ActionResult> Create([FromBody]SaveItemRequest entity)
        {
            try
            {
                var item = mapper.Map<Item>(entity);

                context.ItemHistories.Add(new ItemHistory
                {
                    Item = item,
                    Date = DateTime.Now,
                    Quantity = entity.Quantity,
                    Remarks = Constants.AdjustmentRemarks.InitialQuantity
                });

                await context.Items.AddAsync(item);
                await context.SaveChangesAsync();

                return CreatedAtRoute(nameof(GetItemById), new { id = item.Id }, entity);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Updates a specific <see cref="Item"/>.
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
        [ServiceFilter(typeof(ValidateDuplicateItemCodeAttribute))]
        public async Task<ActionResult> Update(long id, [FromBody]SaveItemRequest entity)
        {
            try
            {
                var item = await context.Items
                    .AsNoTracking()
                    .SingleOrDefaultAsync(t => t.Id == id);

                if (item == null)
                {
                    return NotFound(id);
                }

                item = mapper.Map<Item>(entity);
                context.Update(item);
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
        /// Deletes a specific <see cref="Item"/>.
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
                var entity = await context.Items.SingleOrDefaultAsync(t => t.Id == id);
                if (entity == null)
                {
                    return NotFound(id);
                }

                entity.IsDeleted = true;
                context.Update(entity);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Adjusts the actual and current quantity a specific <see cref="Item"/>.
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="saveAdjustmentRequest"><see cref="SaveItemAdjustmentRequest"/></param>
        /// <returns>None</returns>
        [HttpPost("{itemId}/adjust")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AdjustItem(long itemId, [FromBody]SaveItemAdjustmentRequest saveAdjustmentRequest)
        {
            try
            {
                var item = await context.Items.SingleOrDefaultAsync(c => c.Id == itemId);

                if (item == null)
                {
                    return NotFound();
                }

                var adjustment = mapper.Map<Adjustment>(saveAdjustmentRequest);

                // Perform adjustment
                var adjustments = new List<Adjustment> { adjustment };
                await inventoryService.ProcessAdjustments(adjustments, saveAdjustmentRequest.AdjustmentType, saveAdjustmentRequest.Remarks, QuantityType.Both);

                await context.AddAsync(adjustment);
                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}