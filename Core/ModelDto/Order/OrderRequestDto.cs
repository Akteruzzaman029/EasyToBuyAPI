using Core.ModelDto.OrderItem;

namespace Core.ModelDto.Order
{
    public class OrderRequestDto
    {
        public int CompanyId { get; set; }
        public int OrderType { get; set; }
        public int? AddressId { get; set; }
        public string? UserId { get; set; }           // Nullable for guest orders
        public string? OrderNo { get; set; }          // Unique order number or code
        public decimal TotalAmount { get; set; }      // Total value of the order
        public decimal TotalDiscount { get; set; }      // Total value of the order
        public int? OrderStatus { get; set; }         // You can map this to an enum
        public string Remarks { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public List<OrderItemRequestDto> OrderItems { get; set; } = new List<OrderItemRequestDto>();
    }
}
