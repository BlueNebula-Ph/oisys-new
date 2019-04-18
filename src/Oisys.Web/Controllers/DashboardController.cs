using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OisysNew.DTO.Dashboard;
using OisysNew.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OisysNew.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IOisysDbContext context;
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public DashboardController(
            IOisysDbContext context,
            IMapper mapper,
            ILogger<DashboardController> logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet("orders/{days}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> GetOrdersCount([FromRoute]int days)
        {
            try
            {
                var daysBefore = GetPreviousDate(days);
                var ordersCount = await context.Orders
                    .AsNoTracking()
                    .Where(a => a.Date >= daysBefore && a.Date < DateTime.Now)
                    .CountAsync();

                return ordersCount;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        [HttpGet("sales/{days}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<decimal>> GetSales([FromRoute]int days)
        {
            try
            {
                var daysBefore = GetPreviousDate(days);
                var totalSales = await context.Orders
                    .AsNoTracking()
                    .Where(a => a.Date >= daysBefore && a.Date < DateTime.Now)
                    .SumAsync(a => a.TotalAmount);

                return totalSales;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        [HttpGet("orders-due")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> GetOrdersDue()
        {
            try
            {
                var date = DateTime.Now.AddDays(7);
                var totalOrdersDue = await context.Orders
                    .AsNoTracking()
                    .Where(a => a.DueDate >= DateTime.Now.Date && a.DueDate < date)
                    .CountAsync();

                return totalOrdersDue;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        [HttpGet("low-quantity")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> GetLowQuantityItemCount()
        {
            try
            {
                var lowQuantityItemCount = await context.Items
                    .AsNoTracking()
                    .Where(a => a.Quantity < Constants.DefaultLowQuantity)
                    .CountAsync();

                return lowQuantityItemCount;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("order-list")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<DashboardGroupedOrder>>> GetOrderList()
        {
            try
            {
                var orders = await context.OrderLineItems
                    .Include(a => a.Order).ThenInclude(a => a.Customer).ThenInclude(a => a.Province)
                    .Include(a => a.Order).ThenInclude(a => a.Customer).ThenInclude(a => a.City)
                    .Include(a => a.Item).ThenInclude(a => a.Category)
                    .AsNoTracking()
                    .Where(a => a.Quantity != a.QuantityDelivered)
                    .GroupBy(a =>
                        new
                        {
                            CityName = a.Order.Customer.City.Name,
                            ProvinceName = a.Order.Customer.Province.Name
                        })
                    .Select(a =>
                        new DashboardGroupedOrder
                        {
                            City = a.Key.CityName,
                            Province = a.Key.ProvinceName,
                            Orders = a.Select(x => new DashboardOrder
                            {
                                Category = x.Item.Category.Name,
                                Customer = x.Order.Customer.Name,
                                Date = x.Order.Date.ToShortDateString(),
                                DueDate = x.Order.DueDate.HasValue ?
                                    x.Order.DueDate.Value.ToShortDateString() : string.Empty,
                                ItemCode = x.Item.Code,
                                ItemName = x.Item.Name,
                                OrderCode = x.Order.Code,
                                Quantity = x.Quantity,
                                QuantityDelivered = x.QuantityDelivered,
                                Unit = x.Item.Unit
                            })
                        })
                    .ToListAsync();

                return orders;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private DateTime GetPreviousDate(int daysBefore)
        {
            return DateTime.Now.AddDays(daysBefore * -1);
        }
    }
}