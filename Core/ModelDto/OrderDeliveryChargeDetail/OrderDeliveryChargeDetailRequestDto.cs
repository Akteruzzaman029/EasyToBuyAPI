namespace Core.ModelDto.OrderDeliveryChargeDetail
{
    public class OrderDeliveryChargeDetailRequestDto
    {
        public int CompanyId { get; set; }
        public int SalesOrderId { get; set; }
        public int? DeliveryChargeRuleId { get; set; }
        public decimal BaseCharge { get; set; }
        public decimal DistanceCharge { get; set; }
        public decimal WeightCharge { get; set; }
        public decimal AreaCharge { get; set; }
        public decimal ExpressCharge { get; set; }
        public decimal RemoteAreaCharge { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FinalCharge { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
    }
}
