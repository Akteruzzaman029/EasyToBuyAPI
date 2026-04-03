namespace Core.ModelDto.OrderTracking
{
    public class OrderTrackingFilterDto
    {
        public int SalesOrderId { get; set; }
        public int OrderFlowStageId { get; set; }
        public int? TrackingStatus { get; set; }
        public bool IsCustomerVisible { get; set; }
        public bool IsActive { get; set; }
    }
}
