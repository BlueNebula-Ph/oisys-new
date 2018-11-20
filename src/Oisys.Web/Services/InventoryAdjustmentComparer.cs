using System.Collections.Generic;

namespace OisysNew.Services
{
    public class InventoryAdjustmentComparer : IEqualityComparer<InventoryAdjustment>
    {
        public bool Equals(InventoryAdjustment x, InventoryAdjustment y)
        {
            return (object.ReferenceEquals(x, y)) || 
                (x != null && y != null && x.ItemId.Equals(y.ItemId) && x.Quantity.Equals(y.Quantity));
        }

        public int GetHashCode(InventoryAdjustment obj)
        {
            //Get hash code for the Name field if it is not null. 
            int hashItemId = obj.ItemId.GetHashCode();

            //Get hash code for the Code field. 
            int hashQuantity = obj.Quantity.GetHashCode();

            //Calculate the hash code for the product. 
            return hashItemId ^ hashQuantity;
        }
    }
}
