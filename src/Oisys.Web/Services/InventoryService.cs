using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OisysNew.DTO.Order;
using OisysNew.Extensions;
using OisysNew.Helpers;
using OisysNew.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
            context = context;
            mapper = mapper;
            logger = logger;
        }

        public void ProcessOrderDetails(IEnumerable<OrderLineItem> orderDetails)
        {
            foreach (var detail in orderDetails)
            {
                var adjustmentQuantity = detail.Quantity * -1;

                var item = context.Items.Find(detail.ItemId);

                if (item != null)
                {
                    var transactions = item.TransactionHistory;
                    var currentQty = transactions.Sum(a => a.Quantity);

                    //item.Quantity = currentQty + adjustmentQuantity - 
                    //    (transactions.FirstOrDefault(a => a.ItemId == item.Id && a.order)?.Quantity ?? 0);
                }

                if (detail.TransactionHistory == null)
                {
                    detail.TransactionHistory = new ItemTransactionHistoryOrder
                    {
                        Date = DateTime.Now,
                        ItemId = detail.ItemId,
                        Quantity = adjustmentQuantity
                    };
                }
                else
                {
                    detail.TransactionHistory.Date = DateTime.Now;
                    detail.TransactionHistory.ItemId = detail.ItemId;
                    detail.TransactionHistory.Quantity = adjustmentQuantity;
                }
            }
        }

        public async Task AdjustItemQuantities(IEnumerable<InventoryAdjustment> adjustments)
        {
            foreach (var adjustment in adjustments)
            {
                var item = await context.Items.FindAsync(adjustment.ItemId);

                if (item != null)
                {


                    //item.Quantity = adjustment.AdjustmentType == AdjustmentType.Add ?
                    //    item.Quantity + adjustment.Quantity :
                    //    item.Quantity - adjustment.Quantity;

                    context.Update(item);

                    if (adjustment.SaveAdjustmentDetails)
                    {
                        AddAdjusment(adjustment);
                    }
                }
            }
        }

        public void TestAdjustments()
        {
            var x = context.ItemTransactionHistories.Local.AsQueryable();

            foreach (var y in x)
            {
                var item = context.Items.Find(y.ItemId);
                var oldTransactions = context.ItemTransactionHistories
                    .Where(a => a.ItemId == y.ItemId)
                    .Sum(a => a.Quantity);

                var newTransactions = context.ItemTransactionHistories.Local
                    .Where(a => a.ItemId == y.ItemId)
                    .Sum(a => a.Quantity);

                item.Quantity = oldTransactions + newTransactions;

                context.Update(item);
            }
        }

        private void AddAdjusment(InventoryAdjustment adjustment)
        {
            context.Add(new Adjustment
            {
                ItemId = adjustment.ItemId,
                AdjustmentDate = DateTime.Now,
                AdjustmentType = adjustment.AdjustmentType.GetDisplayName(),
                Quantity = adjustment.Quantity,
                Remarks = adjustment.Remarks,
                Operator = adjustment.OperatorName,
                Machine = adjustment.MachineName,
            });
        }
    }
}
