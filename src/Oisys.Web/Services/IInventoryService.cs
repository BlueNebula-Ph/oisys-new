using System.Collections.Generic;
using System.Threading.Tasks;

namespace OisysNew.Services
{
    public interface IInventoryService
    {
        Task AdjustItemQuantities(IEnumerable<InventoryAdjustment> adjustments);
    }
}
