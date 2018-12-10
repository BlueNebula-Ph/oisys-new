using OisysNew.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OisysNew.Services
{
    public interface IOrderService
    {
        Task ProcessReturns(IEnumerable<CreditMemoLineItem> creditMemoLineItems);

        Task ProcessDeliveries();
    }
}
