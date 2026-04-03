using System.ComponentModel.DataAnnotations;

namespace Core.Model
{
    public class OrderChargeAdjustmentModel
    {
        public int Id { get; set; }
        public int SalesOrderId { get; set; }
        public string ChargeHead { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public bool IsAddition { get; set; }
        public string? Reason { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

