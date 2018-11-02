using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OisysNew.DTO;
using OisysNew.DTO.Item;
using OisysNew.Helpers;
using OisysNew.Models;
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
        private readonly OisysDbContext context;
        private readonly IMapper mapper;
        private readonly IListHelpers listHelpers;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemController"/> class.
        /// </summary>
        /// <param name="context">DbContext</param>
        /// <param name="mapper">Automapper</param>
        /// <param name="listHelpers">List Helpers</param>
        public ItemController(OisysDbContext context, IMapper mapper, IListHelpers listHelpers)
        {
            this.context = context;
            this.mapper = mapper;
            this.listHelpers = listHelpers;
        }

        /// <summary>
        /// Returns list of active <see cref="Item"/>
        /// </summary>
        /// <param name="filter"><see cref="ItemFilterRequest"/></param>
        /// <returns>List of Items</returns>
        [HttpPost("search", Name = "GetAllItems")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<PaginatedList<ItemSummary>>> GetAll([FromBody]ItemFilterRequest filter)
        {
            // get list of active items (not deleted)
            var list = this.context.Items
                .AsNoTracking()
                .Where(c => !c.IsDeleted);

            // filter
            if (!string.IsNullOrEmpty(filter?.SearchTerm))
            {
                list = list.Where(c => c.Code.Contains(filter.SearchTerm) || c.Name.Contains(filter.SearchTerm));
            }

            if (!(filter?.CategoryId).IsNullOrZero())
            {
                list = list.Where(c => c.CategoryId == filter.CategoryId);
            }

            // sort
            var ordering = $"Code {Constants.DefaultSortDirection}";
            if (!string.IsNullOrEmpty(filter?.SortBy))
            {
                ordering = $"{filter.SortBy} {filter.SortDirection}";
            }

            list = list.OrderBy(ordering);

            var result = await this.listHelpers.CreatePaginatedListAsync<Item, ItemSummary>(list, filter.PageNumber, filter.PageSize);
            return result;
        }

        /// <summary>
        /// Returns list of active <see cref="Item"/>
        /// </summary>
        /// <returns>List of Items</returns>
        [HttpGet("lookup", Name = "GetItemLookup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ItemLookup>>> GetLookup()
        {
            // get list of active items (not deleted)
            var list = this.context.Items
                .AsNoTracking()
                .Where(c => !c.IsDeleted);

            // sort
            var ordering = $"Name {Constants.DefaultSortDirection}";

            list = list.OrderBy(ordering);

            var entities = await list.ProjectTo<ItemLookup>(mapper.ConfigurationProvider).ToListAsync();
            return entities;
        }

        /// <summary>
        /// Gets a specific <see cref="Item"/>.
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>Item</returns>
        [HttpGet("{id}", Name = "GetItem")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ItemSummary>> GetById(long id)
        {
            var entity = await this.context.Items
                .Include(c => c.Category)
                .Include(c => c.Adjustments)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (entity == null)
            {
                return this.NotFound(id);
            }

            // Sort the adjusments by date desc
            // Hacky! Find better solution if possible
            entity.Adjustments = entity.Adjustments
                .OrderByDescending(t => t.AdjustmentDate)
                .Select(adjustment => adjustment)
                .ToList();

            var mappedEntity = this.mapper.Map<ItemSummary>(entity);
            return mappedEntity;
        }

        /// <summary>
        /// Creates a <see cref="Item"/>.
        /// </summary>
        /// <param name="entity">entity to be created</param>
        /// <returns>Item</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody]SaveItemRequest entity)
        {
            var item = this.mapper.Map<Item>(entity);

            // TODO: Modify quantity
            //this.adjustmentService.ModifyQuantity(QuantityType.Both, item, entity.Quantity, AdjustmentType.Add, Constants.AdjustmentRemarks.InitialQuantity);

            await this.context.Items.AddAsync(item);
            await this.context.SaveChangesAsync();

            var mappedItem = this.mapper.Map<ItemSummary>(item);

            return this.CreatedAtRoute("GetItem", new { id = item.Id }, mappedItem);
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
        public async Task<IActionResult> Update(long id, [FromBody]SaveItemRequest entity)
        {
            var item = await this.context.Items.SingleOrDefaultAsync(t => t.Id == id);
            if (item == null)
            {
                return this.NotFound(id);
            }

            try
            {
                this.mapper.Map(entity, item);
                this.context.Update(item);
                await this.context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException concurrencyEx)
            {
                return this.BadRequest(concurrencyEx);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex);
            }

            return new NoContentResult();
        }

        /// <summary>
        /// Deletes a specific <see cref="Item"/>.
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>None</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(long id)
        {
            var entity = await this.context.Items.SingleOrDefaultAsync(t => t.Id == id);
            if (entity == null)
            {
                return this.NotFound(id);
            }

            entity.IsDeleted = true;
            this.context.Update(entity);
            await this.context.SaveChangesAsync();

            return new NoContentResult();
        }

        /// <summary>
        /// Adjusts the actual and current quantity a specific <see cref="Item"/>.
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="entity"><see cref="SaveItemAdjustmentRequest"/></param>
        /// <returns>None</returns>
        //[HttpPost("{id}/adjust")]
        //public async Task<IActionResult> AdjustItem(long id, [FromBody]SaveItemAdjustmentRequest entity)
        //{
        //    if (entity == null || entity.Id == 0 || id == 0 || !this.ModelState.IsValid)
        //    {
        //        return this.BadRequest();
        //    }

        //    var item = await this.context.Items
        //        .SingleOrDefaultAsync(c => c.Id == id);

        //    if (item == null)
        //    {
        //        return this.NotFound(id);
        //    }

        //    try
        //    {
        //        this.adjustmentService.ModifyQuantity(QuantityType.Both, item, entity.AdjustmentQuantity, entity.AdjustmentType, entity.Remarks, entity.Machine, entity.Operator);

        //        await this.context.SaveChangesAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        return this.BadRequest(ex);
        //    }

        //    return this.Ok();
        //}
    }
}
