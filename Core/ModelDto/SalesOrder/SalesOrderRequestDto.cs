namespace Core.ModelDto.SalesOrder
{
    public class SalesOrderRequestDto
    {
        public string OrderNo { get; set; }
        public int CompanyId { get; set; }
        public int? AddressId { get; set; }
        public string? UserId { get; set; }
        public int OrderTypeId { get; set; }
        public int FlowId { get; set; }
        public int? CurrentStageId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal DeliveryCharge { get; set; }
        public decimal ExtraDeliveryCharge { get; set; }
        public decimal DiscountOnDelivery { get; set; }
        public decimal FinalDeliveryCharge { get; set; }
        public decimal NetAmount { get; set; }
        public int? DeliveryChargeRuleId { get; set; }
        public int? PaymentStatus { get; set; }
        public string Remarks { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
