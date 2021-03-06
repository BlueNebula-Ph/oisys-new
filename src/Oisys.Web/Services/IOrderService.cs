﻿using OisysNew.Helpers;
using System.Collections;
using System.Threading.Tasks;

namespace OisysNew.Services
{
    public interface IOrderService
    {
        Task ProcessReturns(IEnumerable creditMemoLineItems, AdjustmentType adjustmentType);

        Task ProcessDeliveries(IEnumerable deliveryLineItems, AdjustmentType adjustmentType);

        Task ProcessInvoice(IEnumerable invoiceLineItems, bool isInvoiced);
    }
}
