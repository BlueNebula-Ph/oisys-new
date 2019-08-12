using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OisysNew.DTO.CreditMemo;
using OisysNew.DTO.Delivery;
using OisysNew.DTO.Invoice;
using OisysNew.Exceptions;
using OisysNew.Extensions;
using OisysNew.Helpers;
using OisysNew.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task ProcessInvoice(IEnumerable invoiceLineItems, bool isInvoiced)
        {
            foreach (var lineItem in invoiceLineItems)
            {
                switch (lineItem)
                {
                    case InvoiceLineItem invoiceLineItem:
                        await UpdateOrderOrCreditMemo(invoiceLineItem, isInvoiced);
                        break;
                    case SaveInvoiceLineItemRequest invoice:
                        var invLineItem = mapper.Map<InvoiceLineItem>(invoice);
                        await UpdateOrderOrCreditMemo(invLineItem, isInvoiced);
                        break;
                    case null:
                        break;
                }
            }
        }

        private async Task UpdateQuantityReturned(CreditMemoLineItem lineItem, AdjustmentType adjustmentType)
        {
            var orderLineItem = await FetchLineItem(lineItem.OrderLineItemId);
            var tempQuantityReturned = orderLineItem.QuantityReturned;

            orderLineItem.QuantityReturned = adjustmentType == AdjustmentType.Add ?
                orderLineItem.QuantityReturned + lineItem.Quantity :
                orderLineItem.QuantityReturned - lineItem.Quantity;

            // Quantity returned cannot be more than original order quantity
            if (orderLineItem.QuantityReturned > orderLineItem.Quantity)
            {
                var errorBuilder = new StringBuilder("Quantity returned cannot be more than the quantity ordered.<br />");
                if (tempQuantityReturned != 0)
                    errorBuilder.Append($"{orderLineItem.Item.Name} of order {orderLineItem.Order.Code} already has {tempQuantityReturned}/{orderLineItem.Quantity} returned.");
                else
                    errorBuilder.Append($"{orderLineItem.Item.Name} of order {orderLineItem.Order.Code} only has {orderLineItem.Quantity} quantity ordered.");

                throw new QuantityReturnedException(errorBuilder.ToString());
            }
        }

        private async Task UpdateQuantityDelivered(DeliveryLineItem lineItem, AdjustmentType adjustmentType)
        {
            var orderLineItem = await FetchLineItem(lineItem.OrderLineItemId);
            var tempQuantityDelivered = orderLineItem.QuantityDelivered;

            orderLineItem.QuantityDelivered = adjustmentType == AdjustmentType.Add ?
                orderLineItem.QuantityDelivered + lineItem.Quantity :
                orderLineItem.QuantityDelivered - lineItem.Quantity;

            // Quantity delivered cannot be more than original order quantity
            if (orderLineItem.QuantityDelivered > orderLineItem.Quantity)
            {
                var errorBuilder = new StringBuilder("Quantity delivered cannot be more than the quantity ordered.<br />");
                if (tempQuantityDelivered != 0)
                    errorBuilder.Append($"{orderLineItem.Item.Name} of order {orderLineItem.Order.Code} already has {tempQuantityDelivered}/{orderLineItem.Quantity} delivered.");
                else
                    errorBuilder.Append($"{orderLineItem.Item.Name} of order {orderLineItem.Order.Code} only has {orderLineItem.Quantity} quantity ordered.");

                throw new QuantityDeliveredException(errorBuilder.ToString());
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

        private async Task UpdateOrderOrCreditMemo(InvoiceLineItem lineItem, bool isInvoiced)
        {
            if (!lineItem.OrderId.IsNullOrZero())
            {
                var order = await FetchOrder(lineItem.OrderId.Value);
                foreach (var item in order.LineItems)
                {
                    item.QuantityInvoiced = isInvoiced ?
                        item.QuantityDelivered : item.QuantityDelivered - item.QuantityInvoiced;
                }
                order.IsInvoiced = order.LineItems.All(a => a.Quantity == a.QuantityInvoiced);
            }
            else if (!lineItem.CreditMemoId.IsNullOrZero())
            {
                var creditMemo = await FetchCreditMemo(lineItem.CreditMemoId.Value);
                creditMemo.IsInvoiced = isInvoiced;
            }
        }

        private async Task<Order> FetchOrder(long orderId)
        {
            var order = await context.Orders.FindAsync(orderId);
            if (order == null)
            {
                throw new ArgumentException($"Order with id {orderId} not found.");
            }
            return order;
        }

        private async Task<CreditMemo> FetchCreditMemo(long creditMemoId)
        {
            var creditMemo = await context.CreditMemos.FindAsync(creditMemoId);
            if (creditMemo == null)
            {
                throw new ArgumentException($"Credit memo with id {creditMemoId} not found.");
            }
            return creditMemo;
        }
    }
}