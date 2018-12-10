using System.Collections;
using System.Threading.Tasks;

namespace OisysNew.Services
{
    public interface IInventoryService
    {
        Task ProcessAdjustments(IEnumerable quantitiesAdded = null, IEnumerable quantitiesDeducted = null, string remarks = "");
    }
}
