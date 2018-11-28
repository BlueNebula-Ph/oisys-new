using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OisysNew.DTO;
using OisysNew.DTO.Order;
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
    /// <see cref="OrderController"/> class handles Order basic add, edit, delete and get.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IOisysDbContext context;
        private readonly IMapper mapper;
        private readonly IListHelpers listHelpers;
        private readonly IEntityListHelpers entityListHelpers;
        private readonly IInventoryService inventoryService;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderController"/> class.
        /// </summary>
        /// <param name="context">DbContext</param>
        /// <param name="mapper">Automapper</param>
        /// <param name="listHelpers">Summary list helpers</param>
        /// <param name="entityListHelpers">Helper for entities</param>
        /// <param name="inventoryService">Inventory service</param>
        /// <param name="logger">Logger</param>
        public OrderController(
            IOisysDbContext context,
            IMapper mapper,
            IListHelpers listHelpers,
            IEntityListHelpers entityListHelpers,
            IInventoryService inventoryService,
            ILogger<OrderController> logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.listHelpers = listHelpers;
            this.entityListHelpers = entityListHelpers;
            this.inventoryService = inventoryService;
            this.logger = logger;
        }

        /// <summary>
        /// Returns list of active <see cref="Order"/>
        /// </summary>
        /// <param name="filter"><see cref="OrderFilterRequest"/></param>
        /// <returns>List of Orders</returns>
        [HttpPost("search", Name = "GetAllOrders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaginatedList<OrderSummary>>> GetAll([FromBody]OrderFilterRequest filter)
        {
            try
            {
                // get list of active orders (not deleted)
                var list = context.Orders
                    .Include(c => c.Customer)
                    .AsNoTracking();

                // filter
                if (!string.IsNullOrEmpty(filter?.SearchTerm))
                {
                    list = list.Where(c => c.Code.ToString().Contains(filter.SearchTerm));
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
                var ordering = $"{Constants.ColumnNames.Code} {Constants.DefaultSortDirection}";
                if (!string.IsNullOrEmpty(filter?.SortBy))
                {
                    ordering = $"{filter.SortBy} {filter.SortDirection}";
                }

                list = list.OrderBy(ordering);

                var entities = await listHelpers.CreatePaginatedListAsync<Order, OrderSummary>(list, filter.PageNumber, filter.PageSize);
                return entities;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Returns list of active <see cref="Order"/>
        /// </summary>
        /// <param name="customerId">Customer Id</param>
        /// <returns>List of Orders per Customer</returns>
        [HttpGet("{customerId}/lookup", Name = "GetOrderLookup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<OrderLookup>>> GetLookup(int customerId)
        {
            // get list of active items (not deleted)
            var list = context.Orders
                .AsNoTracking()
                .Where(c => c.CustomerId == customerId)
                .OrderBy(c => c.Code);

            var entities = await list.ProjectTo<OrderLookup>(mapper.ConfigurationProvider).ToListAsync();
            return entities;
        }

        /// <summary>
        /// Returns a list of order details
        /// </summary>
        /// <param name="customerId">Customer Id</param>
        /// <param name="isDelivered">True is delivered, false if not</param>
        /// <returns>List of order details per customer</returns>
        [HttpGet("detail/{customerId}/lookup/{isDelivered?}", Name = "GetOrderDetailLookup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<OrderDetailLookup>>> GetOrderDetailLookup(int customerId, bool isDelivered = false)
        {
            try
            {
                // get list of active items (not deleted)
                var list = context.OrderDetails
                    .Include(c => c.Item)
                    .ThenInclude(c => c.Category)
                    .AsNoTracking()
                    .Where(c => c.Order.CustomerId == customerId);

                if (!isDelivered)
                {
                    list = list.Where(c => c.QuantityDelivered != c.Quantity);
                }

                list = list.OrderBy(c => c.Item.Code);

                var entities = await list.ProjectTo<OrderDetailLookup>(mapper.ConfigurationProvider).ToListAsync();
                return entities;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Fetches orders for invoicing
        /// </summary>
        /// <param name="customerId">The customer id selected</param>
        /// <returns>A list of orders for invoicing</returns>
        [HttpGet("invoicing/{customerId}", Name = "GetOrdersForInvoicing")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<OrderLookup>>> GetOrdersForInvoicing(int customerId)
        {
            try
            {
                // get list of active items (not deleted)
                var list = context.Orders
                    .Include(c => c.LineItems)
                    .AsNoTracking()
                    .Where(c => c.CustomerId == customerId && !c.IsInvoiced)
                    .OrderBy(c => c.Code);

                var orders = await list.ProjectTo<OrderLookup>(mapper.ConfigurationProvider).ToListAsync();
                return orders;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Gets a specific <see cref="Order"/>.
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>Order</returns>
        [HttpGet("{id}", Name = "GetOrderById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<OrderSummary>> GetOrderById(int id)
        {
            try
            {
                var entity = await context.Orders
                    .Include(c => c.Customer).ThenInclude(c => c.Province)
                    .Include(c => c.Customer).ThenInclude(c => c.City)
                    .Include(c => c.LineItems).ThenInclude(d => d.Item).ThenInclude(e => e.Category)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(c => c.Id == id);

                if (entity == null)
                {
                    return NotFound(id);
                }

                var orderSummary = mapper.Map<OrderSummary>(entity);
                return orderSummary;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Creates a <see cref="Order"/>.
        /// </summary>
        /// <param name="entity">entity to be created</param>
        /// <returns>Order</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Create([FromBody]SaveOrderRequest entity)
        {
            try
            {
                var order = mapper.Map<Order>(entity);

                // Deduct quantities from inventory
                await inventoryService.ProcessAdjustments(quantitiesDeducted: order.LineItems);

                await context.Orders.AddAsync(order);
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
        /// Updates a specific <see cref="Order"/>.
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
        public async Task<ActionResult> Update(long id, [FromBody]SaveOrderRequest entity)
        {
            try
            {
                var order = await context.Orders
                    .Include(a => a.Customer)
                    .Include(a => a.LineItems).ThenInclude(detail => (detail as OrderLineItem).TransactionHistory)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(t => t.Id == id);

                if (order == null)
                {
                    return NotFound();
                }

                // Process inventory adjustments
                await inventoryService.ProcessAdjustments(order.LineItems, entity.LineItems);

                // Process deleted line items
                entityListHelpers.CheckItemsForDeletion(order.LineItems, entity.LineItems);

                // Update the order data
                order = mapper.Map<Order>(entity);

                context.Update(order);
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
        /// Deletes a specific <see cref="Order"/>.
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
                var order = await context.Orders
                    .FindAsync(id);

                if (order == null)
                {
                    return NotFound();
                }

                // Process line items
                await inventoryService.ProcessAdjustments(quantitiesAdded: order.LineItems);

                context.Remove(order);
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