namespace Core.ModelDto.OrderChargeAdjustment
{
    public class OrderChargeAdjustmentFilterDto
    {
        public int SalesOrderId { get; set; }
        public string ChargeHead { get; set; } = string.Empty;
    }
}
