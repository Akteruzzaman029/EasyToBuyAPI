namespace Core.ModelDto.OrderDelivery
{
    public class OrderDeliveryFilterDto
    {
        public int CompanyId { get; set; }
        public int SalesOrderId { get; set; }
        public int? DeliveryProviderId { get; set; }
        public bool IsActive { get; set; }
    }
}
