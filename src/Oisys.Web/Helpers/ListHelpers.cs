using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Oisys.Web.DTO;
using OisysNew.DTO;
using OisysNew.Models;

namespace OisysNew.Helpers
{
    public class ListHelpers : IListHelpers
    {
        private readonly IMapper mapper;

        public ListHelpers(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public async Task<PaginatedList<T1>> CreatePaginatedListAsync<T, T1>(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source
                .Page(pageNumber, pageSize)
                .ProjectTo<T1>(this.mapper.ConfigurationProvider)
                .ToListAsync();

            return new PaginatedList<T1>(items, count);
        }
    }

    public interface IListHelpers
    {
        Task<PaginatedList<T1>> CreatePaginatedListAsync<T, T1>(IQueryable<T> source, int pageNumber, int pageIndex);
    }
}