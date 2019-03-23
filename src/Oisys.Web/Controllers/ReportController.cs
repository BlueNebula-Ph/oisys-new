using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OisysNew.DTO.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OisysNew.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IOisysDbContext context;
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public ReportController(
            IOisysDbContext context,
            IMapper mapper,
            ILogger<ReportController> logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet("count-sheet")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ItemCount>>> GetCountSheet()
        {
            try
            {
                var items = await context.Items
                    .AsNoTracking()
                    .OrderBy(a => a.Code)
                    .ProjectTo<ItemCount>(mapper.ConfigurationProvider)
                    .ToListAsync();

                return items;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("product-sales/{dateFrom?}/{dateTo?}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ProductSales>>> GetSalesByProduct([FromRoute]DateTime? dateFrom, [FromRoute]DateTime? dateTo)
        {
            try
            {
                var orderLineItems = context
                    .OrderLineItems
                        .Include(a => a.Order)
                        .Include(a => a.Item)
                    .AsNoTracking();

                if (dateFrom != null && dateTo != null)
                {
                    dateFrom = dateFrom == DateTime.MinValue ? DateTime.Today : dateFrom;
                    dateTo = dateTo == DateTime.MinValue ? DateTime.Today : dateTo;
                    orderLineItems = orderLineItems.Where(a => a.Order.Date >= dateFrom && a.Order.Date < dateTo.Value.AddDays(1));
                }

                var groupedItems = await orderLineItems
                    .OrderBy(a => a.Item.Name)
                    .GroupBy(a => a.Item.Name)
                    .Select(a => new ProductSales
                    {
                        Item = a.Key,
                        QuantitySold = a.Sum(x => x.Quantity),
                        TotalSales = a.Sum(x => x.Quantity * x.UnitPrice)
                    })
                    .ToListAsync();
                return groupedItems;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("orders/{dateFrom?}/{dateTo?}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<OrderReport>>> GetOrderSummary([FromRoute]DateTime? dateFrom, [FromRoute]DateTime? dateTo)
        {
            try
            {
                var orders = context.Orders
                    .AsNoTracking();

                if (dateFrom != null && dateTo != null)
                {
                    dateFrom = dateFrom == DateTime.MinValue ? DateTime.Today : dateFrom;
                    dateTo = dateTo == DateTime.MinValue ? DateTime.Today : dateTo;
                    orders = orders.Where(a => a.Date >= dateFrom && a.Date < dateTo.Value.AddDays(1));
                }

                var mappedOrders = await orders
                    .OrderByDescending(a => a.Date)
                    .ProjectTo<OrderReport>(mapper.ConfigurationProvider)
                    .ToListAsync();
                return mappedOrders;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}