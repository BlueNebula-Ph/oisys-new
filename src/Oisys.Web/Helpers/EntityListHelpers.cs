using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OisysNew.DTO;
using OisysNew.Models;
using System.Collections.Generic;
using System.Linq;

namespace OisysNew.Helpers
{
    public class EntityListHelpers : IEntityListHelpers
    {
        private readonly IOisysDbContext context;
        private readonly ILogger logger;

        public EntityListHelpers(IOisysDbContext context, ILogger<EntityListHelpers> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        /// <inheritdoc />
        public void CheckItemsForDeletion<T, T1>(IEnumerable<T> originalList, IEnumerable<T1> updatedList)
            where T: ModelBase
            where T1: DTOBase
        {
            foreach (var item in originalList)
            {
                if (!updatedList.Any(a => a.Id == item.Id))
                {
                    context.Entry(item).State = EntityState.Deleted;
                }
            }
        }
    }

    public interface IEntityListHelpers
    {
        /// <summary>
        /// Iterates through an entity child list 
        /// and marks the items not found on the updated list for deletion.
        /// </summary>
        /// <typeparam name="T">The Entity Type</typeparam>
        /// <typeparam name="T1">The DTO Type</typeparam>
        /// <param name="originalList">The list to be iterated.</param>
        /// <param name="updatedList">The list to be checked. Items not on this list will be marked as deleted.</param>
        void CheckItemsForDeletion<T, T1>(IEnumerable<T> originalList, IEnumerable<T1> updatedList)
            where T: ModelBase
            where T1: DTOBase;
    }
}
