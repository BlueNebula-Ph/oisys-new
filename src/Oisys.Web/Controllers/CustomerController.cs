using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oisys.Web.DTO;
using OisysNew.DTO.Customer;
using OisysNew.Helpers;
using OisysNew.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace OisysNew.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly OisysDbContext context;
        private readonly IMapper mapper;
        private readonly IListHelpers listHelpers;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerController"/> class.
        /// </summary>
        /// <param name="context">DbContext</param>
        /// <param name="mapper">Automapper</param>
        /// <param name="listHelpers">List helper</param>
        public CustomerController(
            OisysDbContext context, 
            IMapper mapper, 
            IListHelpers listHelpers)
        {
            this.context = context;
            this.mapper = mapper;
            this.listHelpers = listHelpers;
        }

        /// <summary>
        /// Returns list of active <see cref="Customer"/>
        /// </summary>
        /// <param name="filter"><see cref="CustomerFilterRequest"/></param>
        /// <returns>List of Customer</returns>
        [HttpPost("search", Name = "GetAllCustomers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PaginatedList<CustomerSummary>>> GetAll([FromBody]CustomerFilterRequest filter)
        {
            // get list of active customers (not deleted)
            var list = this.context.Customers
                .AsNoTracking()
                .Where(c => !c.IsDeleted);

            // filter
            if (!string.IsNullOrEmpty(filter?.SearchTerm))
            {
                list = list.Where(c => c.Keywords.Contains(filter.SearchTerm, StringComparison.CurrentCultureIgnoreCase));
            }

            if (!(filter?.ProvinceId).IsNullOrZero())
            {
                list = list.Where(c => c.ProvinceId == filter.ProvinceId);
            }

            if (!(filter?.CityId).IsNullOrZero())
            {
                list = list.Where(c => c.CityId == filter.CityId);
            }

            // sort
            var ordering = $"Name {Constants.DefaultSortDirection}";
            if (!string.IsNullOrEmpty(filter?.SortBy))
            {
                ordering = $"{filter.SortBy} {filter.SortDirection}";
            }

            list = list.OrderBy(ordering);

            var result = await this.listHelpers.CreatePaginatedListAsync<Customer, CustomerSummary>(list, filter.PageNumber, filter.PageSize);
            return result;
        }

        /// <summary>
        /// Returns list of active <see cref="Customer"/>
        /// </summary>
        /// <returns>List of Customers</returns>
        [HttpGet("lookup", Name = "GetCustomerLookup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CustomerLookup>>> GetLookup()
        {
            // get list of active items (not deleted)
            var list = this.context.Customers
                .AsNoTracking()
                .Where(c => !c.IsDeleted);

            // sort
            var ordering = $"Name {Constants.DefaultSortDirection}";

            list = list.OrderBy(ordering);

            var entities = await list.ProjectTo<CustomerLookup>().ToListAsync();
            return entities;
        }

        /// <summary>
        /// Returns list of active <see cref="Customer"/> with corresponding orders.
        /// </summary>
        /// <param name="isDelivered">Indicates whether orders returned are delivered or not.</param>
        /// <returns>List of Customers with orders</returns>
        //[HttpGet("lookupWithOrders/{isDelivered?}", Name = "GetCustomerLookupWithOrders")]
        //public IActionResult GetLookupWithOrders(bool isDelivered = false)
        //{
        //    // get list of active items (not deleted)
        //    var list = this.context.Customers
        //        .AsNoTracking()
        //        .Where(c => !c.IsDeleted);

        //    // sort
        //    var ordering = $"Name {Constants.DefaultSortDirection}";

        //    list = list.OrderBy(ordering);

        //    var customers = list.ProjectTo<CustomerWithOrdersLookup>().ToList();

        //    // get the corresponding open orders of each customer
        //    foreach (var customer in customers)
        //    {
        //        var orderDetails = this.context.OrderDetails
        //        .Include(c => c.Item)
        //        .ThenInclude(c => c.Category)
        //        .AsNoTracking()
        //        .Where(c => !c.Order.IsDeleted && c.Order.CustomerId == customer.Id);

        //        if (!isDelivered)
        //        {
        //            orderDetails = orderDetails.Where(a => a.QuantityDelivered != a.Quantity);
        //        }

        //        orderDetails = orderDetails.OrderBy(c => c.Item.Code);

        //        customer.OrderDetails = orderDetails.ProjectTo<OrderDetailLookup>().ToList();
        //    }

        //    return this.Ok(customers);
        //}

        /// <summary>
        /// Gets a specific <see cref="Customer"/>.
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>Customer</returns>
        [HttpGet("{id}", Name = "GetCustomer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CustomerSummary>> GetById(long id)
        {
            var entity = await this.context.Customers
                .AsNoTracking()
                .Include(c => c.City)
                .Include(c => c.Province)
                .Include(c => c.Transactions)
                .Include(c => c.PriceList)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (entity == null)
            {
                return this.NotFound(id);
            }

            // Sort the transactions by date desc
            // Hacky! Find better solution if possible
            entity.Transactions = entity.Transactions
                .OrderByDescending(t => t.Date)
                .Select(transaction => transaction)
                .ToList();

            var mappedEntity = this.mapper.Map<CustomerSummary>(entity);
            return mappedEntity;
        }

        /// <summary>
        /// Creates a <see cref="Customer"/>.
        /// </summary>
        /// <param name="entity">entity to be created</param>
        /// <returns>Customer</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SaveCustomerRequest>> Create([FromBody] SaveCustomerRequest entity)
        {
            var customer = this.mapper.Map<Customer>(entity);
            await this.context.Customers.AddAsync(customer);
            await this.context.SaveChangesAsync();

            return this.CreatedAtRoute("GetCustomer", new { id = customer.Id }, entity);
        }

        /// <summary>
        /// Updates a specific <see cref="Customer"/>.
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="entity">entity</param>
        /// <returns>None</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update(long id, [FromBody] SaveCustomerRequest entity)
        {
            var customer = await this.context.Customers.SingleOrDefaultAsync(t => t.Id == id);
            if (customer == null)
            {
                return this.NotFound(id);
            }

            try
            {
                this.mapper.Map(entity, customer);
                this.context.Update(customer);
                await this.context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex);
            }

            return new NoContentResult();
        }

        /// <summary>
        /// Deletes a specific <see cref="Customer"/>.
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>None</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(long id)
        {
            var customer = await this.context.Customers.SingleOrDefaultAsync(t => t.Id == id);
            if (customer == null)
            {
                return this.NotFound(id);
            }

            customer.IsDeleted = true;
            this.context.Update(customer);
            await this.context.SaveChangesAsync();

            return new NoContentResult();
        }

        /// <summary>
        /// Adds a transaction record to a <see cref="Customer"/> record
        /// </summary>
        /// <param name="customerId">Customer Id</param>
        /// <param name="entity">Entity to add</param>
        /// <returns><see cref="Customer"/></returns>
        //[HttpPost("{customerId}/transaction")]
        //public async Task<IActionResult> AddCustomerTransaction(long customerId, [FromBody] SaveCustomerTrxRequest entity)
        //{
        //    if (entity == null || customerId == 0 || !this.ModelState.IsValid)
        //    {
        //        return this.BadRequest(this.ModelState);
        //    }

        //    var transaction = this.mapper.Map<CustomerTransaction>(entity);

        //    await this.context.CustomerTransactions.AddAsync(transaction);
        //    await this.context.SaveChangesAsync();

        //    return this.Ok(entity);
        //}

        /// <summary>
        /// Gets all transactions of a customer
        /// </summary>
        /// <param name="filter">Filter values</param>
        /// <returns>Returns list of <see cref="CustomerTransactionSummary"/></returns>
        //[HttpPost("getTransactions")]
        //public async Task<IActionResult> GetCustomerTransactions([FromBody] CustomerTransactionFilterRequest filter)
        //{
        //    if (!this.ModelState.IsValid)
        //    {
        //        return this.BadRequest(this.ModelState);
        //    }

        //    var transactions = this.context.CustomerTransactions
        //        .AsNoTracking();

        //    if (!(filter?.CustomerId).IsNullOrZero())
        //    {
        //        transactions = transactions.Where(c => c.CustomerId == filter.CustomerId);
        //    }

        //    if (filter?.DateFrom != null || filter?.DateTo != null)
        //    {
        //        filter.DateFrom = filter.DateFrom == null || filter.DateFrom == DateTime.MinValue ? DateTime.Today : filter.DateFrom;
        //        filter.DateTo = filter.DateTo == null || filter.DateTo == DateTime.MinValue ? DateTime.Today : filter.DateTo;

        //        transactions = transactions.Where(c => c.Date >= filter.DateFrom && c.Date < filter.DateTo.Value.AddDays(1));
        //    }

        //    // sort
        //    var ordering = $"Date {Constants.DefaultSortDirection}";
        //    if (!string.IsNullOrEmpty(filter?.SortBy))
        //    {
        //        ordering = $"{filter.SortBy} {filter.SortDirection}";
        //    }

        //    transactions = transactions.OrderBy(ordering);

        //    var mappedTransactions = await this.transactionListBuilder.BuildAsync(transactions, filter);

        //    return this.Ok(mappedTransactions);
        //}
    }
}