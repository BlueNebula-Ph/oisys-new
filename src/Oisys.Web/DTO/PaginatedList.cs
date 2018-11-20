using System.Collections.Generic;

namespace OisysNew.DTO
{
    public class PaginatedList<T>
    {
        public List<T> Items { get; set; }

        public int Total_count { get; set; }

        public PaginatedList(List<T> items, int totalCount)
        {
            Items = items;
            Total_count = totalCount;
        }
    }
}