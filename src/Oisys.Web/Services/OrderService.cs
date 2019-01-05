using AutoMapper;
using Microsoft.Extensions.Logging;
using OisysNew.DTO.CreditMemo;
using OisysNew.Exceptions;
using OisysNew.Helpers;
using OisysNew.Models;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace OisysNew.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOisysDbContext context;
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public OrderService(
            IOisysDbContext context,
            IMapper mapper,
            ILogger<OrderService> logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        public Task ProcessDeliveries()
        {
            throw new NotImplementedException();
        }

        public async Task ProcessReturns(IEnumerable creditMemoLineItems, AdjustmentType adjustmentType)
        {
            foreach (var lineItem in creditMemoLineItems)
            {
                switch (lineItem)
                {
                    case CreditMemoLineItem creditMemoLineItem:
                        await UpdateOrderLineItem(creditMemoLineItem, adjustmentType);
                        break;
                    case SaveCreditMemoLineItemRequest request:
                        var cmLineItem = mapper.Map<CreditMemoLineItem>(request);
                        await UpdateOrderLineItem(cmLineItem, adjustmentType);
                        break;
                    case null:
                        break;
                }
            }
        }

        private async Task UpdateOrderLineItem(CreditMemoLineItem lineItem, AdjustmentType adjustmentType)
        {
            var orderLineItem = await context.OrderLineItems.FindAsync(lineItem.OrderLineItemId);
            if (orderLineItem == null)
            {
                logger.LogWarning($"Order line item with id {lineItem.OrderLineItemId} not found.");
                return;
            }

            orderLineItem.QuantityReturned = adjustmentType == AdjustmentType.Add ?
                orderLineItem.QuantityReturned + lineItem.Quantity :
                orderLineItem.QuantityReturned - lineItem.Quantity;

            // Quantity returned cannot be more than original order quantity
            if (orderLineItem.QuantityReturned > orderLineItem.Quantity)
            {
                throw new QuantityReturnedException($"Quantity returned cannot be more than original quantity.");
            }
        }
    }
}