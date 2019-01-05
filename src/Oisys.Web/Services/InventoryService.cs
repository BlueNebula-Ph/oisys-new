using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OisysNew.DTO.CreditMemo;
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

        public async Task ProcessAdjustments(IEnumerable itemsToUpdate, AdjustmentType adjustmentType, string remarks)
        {
            foreach (var item in itemsToUpdate)
            {
                switch (item)
                {
                    case OrderLineItem orderLineItem:
                        await ProcessItemAdjustmentAsync(orderLineItem.ItemId, orderLineItem.Quantity, adjustmentType);
                        await UpdateOrderLineItemHistory(orderLineItem, adjustmentType, remarks);
                        break;
                    case SaveOrderLineItemRequest orderDetailLineItemRequest:
                        await ProcessItemAdjustmentAsync(orderDetailLineItemRequest.ItemId, orderDetailLineItemRequest.Quantity, adjustmentType);
                        var lineItem = mapper.Map<OrderLineItem>(orderDetailLineItemRequest);
                        await UpdateOrderLineItemHistory(lineItem, adjustmentType, remarks);
                        break;
                    case Adjustment adjustment:
                        await ProcessItemAdjustmentAsync(adjustment.ItemId, adjustment.Quantity, adjustmentType);
                        UpdateAdjustmentItemHistory(adjustment);
                        break;
                    case CreditMemoLineItem creditMemoLineItem:
                        await ProcessItemAdjustmentAsync(creditMemoLineItem.ItemId, creditMemoLineItem.Quantity, adjustmentType);
                        await UpdateCreditMemoItemHistory(creditMemoLineItem, adjustmentType, remarks);
                        break;
                    case SaveCreditMemoLineItemRequest creditMemoLineItemRequest:
                        await ProcessItemAdjustmentAsync(creditMemoLineItemRequest.ItemId, creditMemoLineItemRequest.Quantity, adjustmentType);
                        var cmLineItem = mapper.Map<CreditMemoLineItem>(creditMemoLineItemRequest);
                        await UpdateCreditMemoItemHistory(cmLineItem, adjustmentType, remarks);
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

        private async Task UpdateOrderLineItemHistory(OrderLineItem orderLineItem, AdjustmentType adjustmentType, string remarks)
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
                    remarks);
            }
            else
            {
                UpdateItemHistory(itemHistory, 
                    orderLineItem.Quantity, 
                    adjustmentType, 
                    remarks);
            }
        }

        private void UpdateAdjustmentItemHistory(Adjustment adjustment)
        {
            var itemHistory = new ItemHistory
            {
                ItemId = adjustment.ItemId,
                Date = DateTime.Now,
                Quantity = adjustment.AdjustmentType == AdjustmentType.Add.ToString() ? adjustment.Quantity : adjustment.Quantity * -1,
                Remarks = adjustment.Remarks,
                Adjustment = adjustment
            };

            context.Entry(itemHistory).State = EntityState.Modified;
        }

        private async Task UpdateCreditMemoItemHistory(CreditMemoLineItem creditMemoLineItem, AdjustmentType adjustmentType, string remarks)
        {
            var itemHistory = await context
                   .ItemHistories
                   .FirstOrDefaultAsync(a => a.CreditMemoLineItemId == creditMemoLineItem.Id);

            if (itemHistory == null)
            {
                var newItemHistory = CreateItemHistory(
                        creditMemoLineItem.ItemId,
                        adjustmentType,
                        creditMemoLineItem.Quantity,
                        remarks);

                creditMemoLineItem.TransactionHistory = newItemHistory;
                context.Entry(newItemHistory).State = EntityState.Added;
            }
            else
            {
                if (adjustmentType == AdjustmentType.Deduct)
                {
                    DeleteItemHistory(itemHistory);
                }
                else if(adjustmentType == AdjustmentType.Add)
                {
                    UpdateItemHistory(itemHistory,
                        creditMemoLineItem.Quantity,
                        adjustmentType,
                        remarks);
                }
            }
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

        private void UpdateItemHistory(ItemHistory itemHistory, int quantity, AdjustmentType adjustmentType, string remarks)
        {
            itemHistory.Date = DateTime.Now;
            itemHistory.Quantity = adjustmentType == AdjustmentType.Add ?
                quantity : quantity * -1;
            itemHistory.Remarks = remarks;

            context.Entry(itemHistory).State = EntityState.Modified;
        }

        private void DeleteItemHistory(ItemHistory itemHistory)
        {
            context.Entry(itemHistory).State = EntityState.Deleted;
        }
    }
}
