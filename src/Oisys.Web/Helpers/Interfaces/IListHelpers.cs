using OisysNew.DTO;
using System.Linq;
using System.Threading.Tasks;

namespace OisysNew.Helpers.Interfaces
{
    public interface IListHelpers
    {
        Task<PaginatedList<T1>> CreatePaginatedListAsync<T, T1>(IQueryable<T> source, int pageNumber, int pageSize);
    }
}