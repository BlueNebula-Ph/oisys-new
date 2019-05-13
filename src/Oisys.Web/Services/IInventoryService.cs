using OisysNew.Helpers;
using System.Collections;
using System.Threading.Tasks;

namespace OisysNew.Services
{
    public interface IInventoryService
    {
        Task ProcessAdjustments(IEnumerable itemsToUpdate, AdjustmentType adjustmentType, string remarks, QuantityType quantityType);
    }
}
