using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OisysNew.DTO;
using OisysNew.DTO.Item;
using OisysNew.Extensions;
using OisysNew.Helpers;
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
            context = context;
            mapper = mapper;
            listHelpers = listHelpers;
            inventoryService = inventoryService;
            logger = logger;
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
        [HttpGet("lookup", Name = "GetItemLookup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ItemLookup>>> GetLookup()
        {
            try
            {
                // get list of active items (not deleted)
                var list = context.Items
                    .AsNoTracking();

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
        public async Task<ActionResult<ItemSummary>> GetItemById(long id)
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

                var itemSummary = mapper.Map<ItemSummary>(entity);
                return itemSummary;
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
        public async Task<ActionResult> Create([FromBody]SaveItemRequest entity)
        {
            try
            {
                var item = mapper.Map<Item>(entity);

                context.ItemTransactionHistories.Add(new ItemTransactionHistory
                {
                    ItemId = item.Id,
                    Date = DateTime.Now,
                    Quantity = entity.Quantity
                });

                await context.Items.AddAsync(item);
                await context.SaveChangesAsync();

                var mappedItem = mapper.Map<ItemSummary>(item);

                return CreatedAtRoute(nameof(GetItemById), new { id = item.Id }, mappedItem);
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
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Update(long id, [FromBody]SaveItemRequest entity)
        {
            try
            {
                var item = await context.Items.SingleOrDefaultAsync(t => t.Id == id);
                if (item == null)
                {
                    return NotFound(id);
                }

                mapper.Map(entity, item);
                context.Update(item);
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
        /// <param name="entity"><see cref="SaveItemAdjustmentRequest"/></param>
        /// <returns>None</returns>
        [HttpPost("{id}/adjust")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AdjustItem(int id, [FromBody]SaveItemAdjustmentRequest entity)
        {
            try
            {
                var item = await context.Items.SingleOrDefaultAsync(c => c.Id == id);

                if (item == null)
                {
                    return NotFound(id);
                }

                await inventoryService.AdjustItemQuantities(new List<InventoryAdjustment>
                {
                    new InventoryAdjustment
                    {
                        SaveAdjustmentDetails = true,
                        ItemId = id,
                        Quantity = entity.AdjustmentQuantity,
                        AdjustmentType = entity.AdjustmentType,
                        MachineName = entity.Machine,
                        OperatorName = entity.Operator,
                        Remarks = entity.Remarks
                    }
                });

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