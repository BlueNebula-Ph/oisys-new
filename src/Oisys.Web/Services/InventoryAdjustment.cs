using OisysNew.Helpers;
namespace OisysNew.Services
{
    public class InventoryAdjustment
    {
        public int ItemId { get; set; }

        public int Quantity { get; set; }

        public AdjustmentType AdjustmentType { get; set; }

        public bool SaveAdjustmentDetails { get; set; }

        public string Remarks { get; set; }

        public string MachineName { get; set; }

        public string OperatorName { get; set; }
    }
}
