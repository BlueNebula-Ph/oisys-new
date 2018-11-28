using OisysNew.DTO;
using OisysNew.Models;
using System.Collections.Generic;

namespace OisysNew.Helpers.Interfaces
{
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
            where T : ModelBase
            where T1 : DTOBase;
    }
}
