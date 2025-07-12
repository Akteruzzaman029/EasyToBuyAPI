namespace Core.ModelDto.OrderItem
{
    public class OrderItemFilterDto
    {
        public int OrderId { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
