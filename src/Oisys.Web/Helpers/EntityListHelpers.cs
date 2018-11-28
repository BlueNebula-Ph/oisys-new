using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OisysNew.DTO;
using OisysNew.Helpers.Interfaces;
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
                    logger.LogInformation($"Marking {item.GetType()} with id # {item.Id} for deletion.");
                    context.Entry(item).State = EntityState.Deleted;
                }
            }
        }
    }
}
