using System.ComponentModel.DataAnnotations;

namespace Core.Model
{
    public class SalesOrderModel : BaseEntity
    {
        public string OrderNo { get; set; } = string.Empty;
        public int CompanyId { get; set; }
        public int? BranchId { get; set; }
        public int? CustomerId { get; set; }
        public int? AddressId { get; set; }
        public string? UserId { get; set; }
        public int OrderTypeId { get; set; }
        public int FlowId { get; set; }
        public int? CurrentStageId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public decimal ItemTotalAmount { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal DeliveryCharge { get; set; }
        public decimal ExtraDeliveryCharge { get; set; }
        public decimal DiscountOnDelivery { get; set; }
        public decimal FinalDeliveryCharge { get; set; }
        public decimal NetAmount { get; set; }

        public int? DeliveryChargeRuleId { get; set; }
        public int? PaymentStatus { get; set; }
        public int? DeliveryStatus { get; set; }
        public int? OrderStatus { get; set; }

        public DateTime? ConfirmedAt { get; set; }
        public DateTime? CancelledAt { get; set; }
        public DateTime? CompletedAt { get; set; }

    }
}

