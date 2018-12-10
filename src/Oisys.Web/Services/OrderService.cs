using Microsoft.Extensions.Logging;
using OisysNew.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OisysNew.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOisysDbContext context;
        private readonly ILogger logger;

        public OrderService(
            IOisysDbContext context,
            ILogger<OrderService> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public Task ProcessDeliveries()
        {
            throw new NotImplementedException();
        }

        public async Task ProcessReturns(IEnumerable<CreditMemoLineItem> creditMemoLineItems)
        {
            foreach (var lineItem in creditMemoLineItems)
            {
                var orderLineItem = await context.OrderLineItems.FindAsync(lineItem.OrderLineItemId);
                if (orderLineItem == null)
                {
                    logger.LogWarning($"Order line item with id {lineItem.OrderLineItemId} not found.");
                    continue;
                }

                orderLineItem.QuantityReturned = lineItem.Quantity;
            }
        }
    }
}