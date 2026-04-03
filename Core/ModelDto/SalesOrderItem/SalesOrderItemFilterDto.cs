namespace Core.ModelDto.SalesOrderItem
{
    public class SalesOrderItemFilterDto
    {
        public int SalesOrderId { get; set; }
        public int? ProductId { get; set; }
        public string? Name { get; set; }
        public bool IsActive { get; set; }
    }
}
