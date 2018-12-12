using OisysNew.Helpers;
using System.Collections;
using System.Threading.Tasks;

namespace OisysNew.Services
{
    public interface IOrderService
    {
        Task ProcessReturns(IEnumerable creditMemoLineItems, AdjustmentType adjustmentType);

        Task ProcessDeliveries();
    }
}
