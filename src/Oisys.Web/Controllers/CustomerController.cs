using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OisysNew.DTO;
using OisysNew.DTO.Customer;
using OisysNew.Extensions;
using OisysNew.Helpers;
using OisysNew.Helpers.Interfaces;
using OisysNew.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace OisysNew.Controllers
{
    /// <summary>
    /// <see cref="CustomerController"/> handles adding, updating and deleting of customers.
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IOisysDbContext context;
        private readonly IMapper mapper;
        private readonly IListHelpers listHelpers;
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerController"/> class.
        /// </summary>
        /// <param name="context">DbContext</param>
        /// <param name="mapper">Automapper</param>
        /// <param name="listHelpers">List helper</param>
        /// <param name="logger">Logger</param>
        public CustomerController(
            IOisysDbContext context,
            IMapper mapper,
            IListHelpers listHelpers,
            ILogger<CustomerController> logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.listHelpers = listHelpers;
            this.logger = logger;
        }

        /// <summary>
        /// Returns list of active <see cref="Customer"/>
        /// </summary>
        /// <param name="filter"><see cref="CustomerFilterRequest"/></param>
        /// <returns>List of Customer</returns>
        [HttpPost("search", Name = "GetAllCustomers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaginatedList<CustomerSummary>>> GetAll([FromBody]CustomerFilterRequest filter)
        {
            try
            {
                // get list of active customers (not deleted)
                var list = context.Customers.AsNoTracking();

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
                var ordering = $"{Constants.ColumnNames.Name} {Constants.DefaultSortDirection}";
                if (!string.IsNullOrEmpty(filter?.SortBy))
                {
                    ordering = $"{filter.SortBy} {filter.SortDirection}";
                }

                list = list.OrderBy(ordering);

                var result = await listHelpers.CreatePaginatedListAsync<Customer, CustomerSummary>(list, filter.PageNumber, filter.PageSize);
                return result;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Returns list of active <see cref="Customer"/>
        /// </summary>
        /// <returns>List of Customers</returns>
        [HttpGet("lookup/{provinceId?}/{cityId?}/{name?}", Name = "GetCustomerLookup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<CustomerLookup>>> GetLookup(int provinceId = 0, int cityId = 0, string name = "")
        {
            try
            {
                // get list of active items (not deleted)
                var list = context.Customers.AsNoTracking();

                if (!string.IsNullOrEmpty(name))
                {
                    list = list.Where(a => a.Name.StartsWith(name, StringComparison.CurrentCultureIgnoreCase));
                }

                if (provinceId != 0)
                {
                    list = list.Where(a => a.ProvinceId == provinceId);
                }

                if (cityId != 0)
                {
                    list = list.Where(a => a.CityId == cityId);
                }

                // sort
                var ordering = $"{Constants.ColumnNames.Name} {Constants.DefaultSortDirection}";

                list = list.OrderBy(ordering);

                var customers = await list.ProjectTo<CustomerLookup>(mapper.ConfigurationProvider).ToListAsync();
                return customers;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Gets a specific <see cref="Customer"/>.
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>Customer</returns>
        [HttpGet("{id}", Name = "GetCustomerById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CustomerDetail>> GetCustomerById(long id)
        {
            try
            {
                var entity = await context.Customers
                    .Include(c => c.City)
                    .Include(c => c.Province).ThenInclude(c => c.Cities)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (entity == null)
                {
                    return NotFound();
                }

                var customerDetail = mapper.Map<CustomerDetail>(entity);
                return customerDetail;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Creates a <see cref="Customer"/>.
        /// </summary>
        /// <param name="entity">entity to be created</param>
        /// <returns>Customer</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SaveCustomerRequest>> Create([FromBody] SaveCustomerRequest entity)
        {
            try
            {
                var customer = mapper.Map<Customer>(entity);
                await context.Customers.AddAsync(customer);
                await context.SaveChangesAsync();

                return CreatedAtRoute(nameof(GetCustomerById), new { id = customer.Id }, entity);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Updates a specific <see cref="Customer"/>.
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
        public async Task<ActionResult> Update(long id, [FromBody] SaveCustomerRequest entity)
        {
            try
            {
                var customer = await context.Customers
                    .AsNoTracking()
                    .SingleOrDefaultAsync(a => a.Id == id);

                if (customer == null)
                {
                    return NotFound(id);
                }

                customer = mapper.Map<Customer>(entity);
                context.Update(customer);
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
        /// Deletes a specific <see cref="Customer"/>.
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
                var customer = await context.Customers.SingleOrDefaultAsync(t => t.Id == id);
                if (customer == null)
                {
                    return NotFound(id);
                }

                customer.IsDeleted = true;
                context.Update(customer);
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
        /// Gets all transactions of a customer
        /// </summary>
        /// <param name="filter">Filter values</param>
        /// <returns>Returns list of <see cref="CustomerTransactionSummary"/></returns>
        [HttpGet("{id}/transactions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaginatedList<CustomerTransactionSummary>>> GetCustomerTransactions([FromRoute]long id, [FromQuery]int page, [FromQuery]int size)
        {
            try
            {
                var customerExists = await context.Customers.AnyAsync(a => a.Id == id);
                if (!customerExists)
                {
                    return NotFound();
                }

                var orders = context.Orders
                    .AsNoTracking()
                    .Where(c => c.CustomerId == id)
                    .ProjectTo<CustomerTransactionSummary>(mapper.ConfigurationProvider);

                var creditMemos = context.CreditMemos
                    .AsNoTracking()
                    .Where(a => a.CustomerId == id)
                    .ProjectTo<CustomerTransactionSummary>(mapper.ConfigurationProvider);

                var combined = orders
                    .Concat(creditMemos)
                    .OrderByDescending(a => a.Date);

                var newPageNumber = page + 1;

                var count = await combined.CountAsync();
                var totalPages = (int)Math.Ceiling(count / (double)size);

                var items = await combined
                    .Page(newPageNumber, size)
                    .ToListAsync();

                return new PaginatedList<CustomerTransactionSummary>(items, size, count, totalPages, page);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}