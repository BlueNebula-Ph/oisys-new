using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using OisysNew.DTO;
using OisysNew.Helpers.Interfaces;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

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
            // 0 based paging system. So always add 1.
            var newPageNumber = pageNumber + 1;

            var count = await source.CountAsync();
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            var items = await source
                .Page(newPageNumber, pageSize)
                .ProjectTo<T1>(mapper.ConfigurationProvider)
                .ToListAsync();

            return new PaginatedList<T1>(items, pageSize, count, totalPages, pageNumber);
        }
    }
}