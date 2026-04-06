namespace Core.Model
{
    public class OrderModel : BaseEntity
    {
        public int CompanyId { get; set; }
        public int OrderType { get; set; }
        public int OrderTypeId { get; set; }
        public int FlowId { get; set; }
        public int CurrentStageId { get; set; }
        public int? AddressId { get; set; }
        public string? UserId { get; set; }           // Nullable for guest orders
        public string? OrderNo { get; set; }          // Unique order number or code
        public decimal TotalAmount { get; set; }      // Total value of the order
        public decimal TotalDiscount { get; set; }      // Total value of the order
        public int? OrderStatus { get; set; }         // You can map this to an enum
        public int? PaymentStatus { get; set; }         // You can map this to an enum

    }
}
