namespace Core.ModelDto.OrderChargeAdjustment
{
    public class OrderChargeAdjustmentRequestDto
    {
        public int SalesOrderId { get; set; }
        public string ChargeHead { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public bool IsAddition { get; set; }
        public string Reason { get; set; }
        public string UserId { get; set; } = string.Empty;
       
    }
}
