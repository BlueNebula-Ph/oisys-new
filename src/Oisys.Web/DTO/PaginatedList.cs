using System.Collections.Generic;

namespace OisysNew.DTO
{
    public class PaginatedList<T>
    {
        public List<T> Items { get; set; }

        public PageInfo PageInfo { get; set; }

        public PaginatedList(List<T> items, 
            int size,
            int totalElements,
            int totalPages,
            int pageNumber)
        {
            Items = items;
            PageInfo = new PageInfo
            {
                PageNumber = pageNumber,
                Size = size,
                TotalElements = totalElements,
                TotalPages = totalPages
            };
        }
    }
}