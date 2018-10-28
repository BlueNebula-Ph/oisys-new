using System.Collections.Generic;

namespace Oisys.Web.DTO
{
    public class PaginatedList<T>
    {
        public List<T> Items { get; set; }

        public int Total_count { get; set; }

        public PaginatedList(List<T> items, int totalCount)
        {
            this.Items = items;
            this.Total_count = totalCount;
        }
    }
}