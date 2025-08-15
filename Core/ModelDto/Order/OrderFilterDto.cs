namespace Core.ModelDto.Order
{
    public class OrderFilterDto
    {
        public int CompanyId { get; set; }
        public int OrderType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? UserId { get; set; }          // Unique order number or code
        public string? OrderNo { get; set; }          // Unique order number or code
        public int? OrderStatus { get; set; }         // You can map this to an enum
        public bool IsActive { get; set; }
    }
}
