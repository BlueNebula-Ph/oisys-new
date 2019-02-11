using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OisysNew.DTO;
using OisysNew.DTO.Delivery;
using OisysNew.Exceptions;
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
    /// <see cref="DeliveryController"/> class handles Delivery basic add, edit, delete and get.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryController : Controller
    {
        private readonly IOisysDbContext context;
        private readonly IMapper mapper;
        private readonly IListHelpers listHelpers;
        private readonly IEntityListHelpers entityListHelpers;
        private readonly IOrderService orderService;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeliveryController"/> class.
        /// </summary>
        /// <param name="context">DbContext</param>
        /// <param name="mapper">Automapper</param>
        /// <param name="listHelpers">List helper</param>
        /// <param name="entityListHelpers">Entity list helper</param>
        /// <param name="orderService">Order service</param>
        /// <param name="logger">The logger</param>
        public DeliveryController(
            IOisysDbContext context,
            IMapper mapper,
            IListHelpers listHelpers,
            IEntityListHelpers entityListHelpers,
            IOrderService orderService,
            ILogger<DeliveryController> logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.listHelpers = listHelpers;
            this.entityListHelpers = entityListHelpers;
            this.orderService = orderService;
            this.logger = logger;
        }

        /// <summary>
        /// Returns list of active <see cref="Delivery"/>
        /// </summary>
        /// <param name="filter"><see cref="DeliveryFilterRequest"/></param>
        /// <returns>List of Deliverys</returns>
        [HttpPost("search", Name = "GetAllDelivery")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaginatedList<DeliverySummary>>> GetAll([FromBody]DeliveryFilterRequest filter)
        {
            try
            {
                // get list of active deliveries (not deleted)
                var list = context.Deliveries.AsNoTracking();

                // filter
                if (!string.IsNullOrEmpty(filter?.SearchTerm))
                {
                    list = list.Where(c => c.DeliveryNumber.ToString().Contains(filter.SearchTerm));
                }

                if (!(filter?.CustomerId).IsNullOrZero())
                {
                    list = list.Where(c => c.LineItems.Any(a => a.OrderLineItem.Order.CustomerId == filter.CustomerId));
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

                // sort
                var ordering = $"{Constants.ColumnNames.DeliveryNumber} {Constants.DefaultSortDirection}";
                if (!string.IsNullOrEmpty(filter?.SortBy))
                {
                    ordering = $"{filter.SortBy} {filter.SortDirection}";
                }

                list = list.OrderBy(ordering);

                var entities = await listHelpers.CreatePaginatedListAsync<Delivery, DeliverySummary>(list, filter.PageNumber, filter.PageSize);
                return entities;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Gets a specific <see cref="Delivery"/>.
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>Delivery</returns>
        [HttpGet("{id}", Name = "GetDeliveryById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DeliverySummary>> GetDeliveryById(long id)
        {
            try
            {
                var entity = await context.Deliveries
                    .AsNoTracking()
                    .Include(c => c.Province)
                    .Include(c => c.City)
                    .Include(c => c.LineItems)
                        .ThenInclude(lineItem => (lineItem as DeliveryLineItem).OrderLineItem)
                        .ThenInclude(orderLineItem => orderLineItem.Order)
                        .ThenInclude(order => order.Customer)
                    .Include(c => c.LineItems)
                        .ThenInclude(lineItem => (lineItem as DeliveryLineItem).OrderLineItem)
                        .ThenInclude(orderLineItem => orderLineItem.Item)
                        .ThenInclude(item => item.Category)
                    .SingleOrDefaultAsync(c => c.Id == id);

                if (entity == null)
                {
                    return NotFound();
                }

                var deliverySummary = mapper.Map<DeliverySummary>(entity);
                return deliverySummary;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Creates a <see cref="Delivery"/>.
        /// </summary>
        /// <param name="entity">entity to be created</param>
        /// <returns>Delivery</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Create([FromBody]SaveDeliveryRequest entity)
        {
            try
            {
                var delivery = mapper.Map<Delivery>(entity);
                await context.Deliveries.AddAsync(delivery);

                // Update the order line item for quantity returned
                await orderService.ProcessDeliveries(delivery.LineItems, AdjustmentType.Add);

                await context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status201Created);
            }
            catch (QuantityDeliveredException dEx)
            {
                logger.LogError(dEx.Message);

                ModelState.AddModelError(Constants.ErrorMessage, dEx.Message);
                return BadRequest(ModelState);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Updates a specific <see cref="Delivery"/>.
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
        public async Task<ActionResult> Update(long id, [FromBody]SaveDeliveryRequest entity)
        {
            var deliveryExists = await context.Deliveries
                .AnyAsync(c => c.Id == id);

            if (!deliveryExists)
            {
                return NotFound();
            }

            try
            {
                // Adjust actual quantities accordingly
                //foreach (var updatedDeliveryDetail in entity.Details)
                //{
                //    var deliveryDetailItem = await this.context.Items.FindAsync(updatedDeliveryDetail.ItemId);
                //    if (deliveryDetailItem != null)
                //    {
                //        // Fetch the old detail as no tracking to not interfere with the context-tracked order detail
                //        var oldDetail = await this.context.OrderDetails
                //            .AsNoTracking()
                //            .SingleOrDefaultAsync(c => c.Id == updatedDeliveryDetail.Id);

                //        // If the order detail exists, return the old quantities back
                //        if (oldDetail != null)
                //        {
                //            this.adjustmentService.ModifyQuantity(QuantityType.ActualQuantity, deliveryDetailItem, oldDetail.Quantity, AdjustmentType.Add, Constants.AdjustmentRemarks.DeliveryUpdated);

                //            // If DeliveryId is set to 0, do not re-add the actual quantity
                //            if (updatedDeliveryDetail.DeliveryId == 0)
                //            {
                //                continue;
                //            }
                //        }

                //        // Deduct the correct amount from the item's current quantity
                //        this.adjustmentService.ModifyQuantity(QuantityType.ActualQuantity, deliveryDetailItem, updatedDeliveryDetail.Quantity, AdjustmentType.Deduct, Constants.AdjustmentRemarks.DeliveryUpdated);
                //    }
                //}

                // Map the entity to an delivery object
                var delivery = this.mapper.Map<Delivery>(entity);

                this.context.Update(delivery);

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
        /// Deletes a specific <see cref="Delivery"/>.
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
                var delivery = await context.Deliveries
                .Include(c => c.LineItems)
                    .ThenInclude(lineItem => (lineItem as DeliveryLineItem).OrderLineItem)
                    .ThenInclude(orderLineItem => orderLineItem.Item)
                .SingleOrDefaultAsync(c => c.Id == id);

                if (delivery == null)
                {
                    return NotFound();
                }

                // Update the order line item for quantity delivered
                await orderService.ProcessDeliveries(delivery.LineItems, AdjustmentType.Deduct);

                context.Remove(delivery);
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