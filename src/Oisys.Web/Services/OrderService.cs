using AutoMapper;
using Microsoft.Extensions.Logging;
using OisysNew.DTO.CreditMemo;
using OisysNew.DTO.Delivery;
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

        public async Task ProcessDeliveries(IEnumerable deliveryLineItems, AdjustmentType adjustmentType)
        {
            foreach (var lineItem in deliveryLineItems)
            {
                switch (lineItem)
                {
                    case DeliveryLineItem deliveryLineItem:
                        await UpdateQuantityDelivered(deliveryLineItem, adjustmentType);
                        break;
                    case SaveDeliveryLineItemRequest request:
                        var delLineItem = mapper.Map<DeliveryLineItem>(request);
                        await UpdateQuantityDelivered(delLineItem, adjustmentType);
                        break;
                    case null:
                        break;
                }
            }
        }

        public async Task ProcessReturns(IEnumerable creditMemoLineItems, AdjustmentType adjustmentType)
        {
            foreach (var lineItem in creditMemoLineItems)
            {
                switch (lineItem)
                {
                    case CreditMemoLineItem creditMemoLineItem:
                        await UpdateQuantityReturned(creditMemoLineItem, adjustmentType);
                        break;
                    case SaveCreditMemoLineItemRequest request:
                        var cmLineItem = mapper.Map<CreditMemoLineItem>(request);
                        await UpdateQuantityReturned(cmLineItem, adjustmentType);
                        break;
                    case null:
                        break;
                }
            }
        }

        private async Task UpdateQuantityReturned(CreditMemoLineItem lineItem, AdjustmentType adjustmentType)
        {
            var orderLineItem = await FetchLineItem(lineItem.OrderLineItemId);

            orderLineItem.QuantityReturned = adjustmentType == AdjustmentType.Add ?
                orderLineItem.QuantityReturned + lineItem.Quantity :
                orderLineItem.QuantityReturned - lineItem.Quantity;

            // Quantity returned cannot be more than original order quantity
            if (orderLineItem.QuantityReturned > orderLineItem.Quantity)
            {
                throw new QuantityReturnedException($"Quantity returned cannot be more than original quantity.");
            }
        }

        private async Task UpdateQuantityDelivered(DeliveryLineItem lineItem, AdjustmentType adjustmentType)
        {
            var orderLineItem = await FetchLineItem(lineItem.OrderLineItemId);

            orderLineItem.QuantityDelivered = adjustmentType == AdjustmentType.Add ?
                orderLineItem.QuantityDelivered + lineItem.Quantity :
                orderLineItem.QuantityDelivered - lineItem.Quantity;

            // Quantity delivered cannot be more than original order quantity
            if (orderLineItem.QuantityDelivered > orderLineItem.Quantity)
            {
                throw new QuantityDeliveredException($"Quantity delivered cannot be more than original quantity.");
            }
        }

        private async Task<OrderLineItem> FetchLineItem(long orderLineItemId)
        {
            var orderLineItem = await context.OrderLineItems.FindAsync(orderLineItemId);
            if (orderLineItem == null)
            {
                throw new ArgumentException($"Order line item with id {orderLineItemId} not found.");
            }
            return orderLineItem;
        }
    }
}