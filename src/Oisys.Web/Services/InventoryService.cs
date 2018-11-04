using Microsoft.Extensions.Logging;
using OisysNew.Extensions;
using OisysNew.Helpers;
using OisysNew.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OisysNew.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IOisysDbContext context;
        private readonly ILogger logger;

        public InventoryService(
            IOisysDbContext context,
            ILogger<InventoryService> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task AdjustItemQuantities(IEnumerable<InventoryAdjustment> adjustments)
        {
            foreach (var adjustment in adjustments)
            {
                var item = await this.context.Items.FindAsync(adjustment.ItemId);

                if (item != null)
                {
                    item.Quantity = adjustment.AdjustmentType == AdjustmentType.Add ?
                        item.Quantity + adjustment.Quantity :
                        item.Quantity - adjustment.Quantity;

                    this.context.Update(item);

                    if (adjustment.SaveAdjustmentDetails)
                    {
                        this.AddAdjusment(adjustment);
                    }
                }
            }
        }

        private void AddAdjusment(InventoryAdjustment adjustment)
        {
            this.context.Add(new Adjustment
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
