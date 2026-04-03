namespace Core.ModelDto.SalesOrderItem
{
    public class SalesOrderItemRequestDto
    {
        public int SalesOrderId { get; set; }
        public int? ProductId { get; set; }
        public string? Name { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public bool IsFixedAmount { get; set; }
        public decimal VatAmount { get; set; }
        public decimal LineTotal { get; set; }
        public decimal NetAmount { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
