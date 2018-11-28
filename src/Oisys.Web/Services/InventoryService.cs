using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OisysNew.DTO.Order;
using OisysNew.Helpers;
using OisysNew.Models;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace OisysNew.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IOisysDbContext context;
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public InventoryService(
            IOisysDbContext context,
            IMapper mapper,
            ILogger<InventoryService> logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task ProcessAdjustments(IEnumerable quantitiesAdded = null, IEnumerable quantitiesDeducted = null)
        {
            if (quantitiesAdded != null)
            {
                await UpdateItems(quantitiesAdded, AdjustmentType.Add);
            }

            if (quantitiesDeducted != null)
            {
                await UpdateItems(quantitiesDeducted, AdjustmentType.Deduct);
            }
        }

        private async Task UpdateItems(IEnumerable itemsToUpdate, AdjustmentType adjustmentType)
        {
            foreach (var item in itemsToUpdate)
            {
                switch (item)
                {
                    case OrderLineItem orderLineItem:
                        await ProcessItemAdjustmentAsync(orderLineItem.ItemId, orderLineItem.Quantity, adjustmentType);
                        await UpdateOrderLineItemHistory(orderLineItem, adjustmentType);
                        break;
                    case SaveOrderDetailRequest orderDetailRequest:
                        await ProcessItemAdjustmentAsync(orderDetailRequest.ItemId, orderDetailRequest.Quantity, adjustmentType);
                        var lineItem = mapper.Map<OrderLineItem>(orderDetailRequest);
                        await UpdateOrderLineItemHistory(lineItem, adjustmentType);
                        break;
                    case Adjustment adjustment:
                        await ProcessItemAdjustmentAsync(adjustment.ItemId, adjustment.Quantity, adjustmentType);
                        UpdateAdjustmentItemHistory(adjustment);
                        break;
                    case null:
                        break;
                }
            }
        }

        private async Task ProcessItemAdjustmentAsync(long itemId, int quantity, AdjustmentType adjustmentType)
        {
            var item = await context.Items.FindAsync(itemId);
            if (item == null)
            {
                throw new ArgumentException("Item does not exist.");
            }

            item.Quantity = adjustmentType == AdjustmentType.Add ?
                item.Quantity + quantity : 
                item.Quantity - quantity;

            context.Entry(item).State = EntityState.Modified;
        }

        private async Task UpdateOrderLineItemHistory(OrderLineItem orderLineItem, AdjustmentType adjustmentType)
        {
            var itemHistory = await context
                    .ItemHistories
                    .FirstOrDefaultAsync(a => a.OrderLineItemId == orderLineItem.Id);

            if (itemHistory == null)
            {
                orderLineItem.TransactionHistory = CreateItemHistory(
                    orderLineItem.ItemId,
                    adjustmentType,
                    orderLineItem.Quantity,
                    Constants.AdjustmentRemarks.OrderUpdated);
            }
            else
            {
                UpdateItemHistory(itemHistory, 
                    orderLineItem.Quantity, 
                    adjustmentType, 
                    Constants.AdjustmentRemarks.OrderUpdated);
            }
        }

        private void UpdateAdjustmentItemHistory(Adjustment adjustment)
        {
            var itemHistory = new ItemHistory
            {
                ItemId = adjustment.ItemId,
                Date = DateTime.Now,
                Quantity = adjustment.AdjustmentType == AdjustmentType.Add.ToString() ? adjustment.Quantity : adjustment.Quantity * -1,
                Remarks = Constants.AdjustmentRemarks.OrderUpdated,
                Adjustment = adjustment
            };

            context.Entry(itemHistory).State = EntityState.Modified;
        }

        private ItemHistory CreateItemHistory(long itemId, AdjustmentType adjustmentType, int quantity, string remarks)
        {
            return new ItemHistory
            {
                ItemId = itemId,
                Quantity = adjustmentType == AdjustmentType.Add ? quantity : quantity * -1,
                Date = DateTime.Now,
                Remarks = remarks
            };
        }

        private ItemHistory UpdateItemHistory(ItemHistory itemHistory, int quantity, AdjustmentType adjustmentType, string remarks)
        {
            itemHistory.Date = DateTime.Now;
            itemHistory.Quantity = adjustmentType == AdjustmentType.Add ?
                quantity : quantity * -1;
            itemHistory.Remarks = remarks;

            return itemHistory;
        }
    }
}
