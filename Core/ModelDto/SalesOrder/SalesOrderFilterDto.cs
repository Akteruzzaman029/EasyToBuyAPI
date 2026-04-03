namespace Core.ModelDto.SalesOrder
{
    public class SalesOrderFilterDto
    {
        public string OrderNo { get; set; } 
        public int CompanyId { get; set; }
        public int? CustomerId { get; set;  }
        public int OrderTypeId { get; set; }
        public int FlowId { get; set; }
        public int? CurrentStageId { get; set; }
        public int? DeliveryChargeRuleId { get; set; }
        public int? PaymentStatus { get; set; }
        public int? DeliveryStatus { get; set; }
        public int? OrderStatus { get; set; }
        public bool IsActive { get; set; }
    }
}
