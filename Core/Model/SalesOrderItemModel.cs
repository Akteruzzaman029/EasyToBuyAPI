using System.ComponentModel.DataAnnotations;

namespace Core.Model
{
    public class SalesOrderItemModel : BaseEntity
    {
        public int OrderId { get; set; }
        public int? ProductId { get; set; }
        public string? ProductNameSnapshot { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public bool IsFixedAmount { get; set; }
        public decimal VatAmount { get; set; }
        public decimal LineTotal { get; set; }
        public decimal NetAmount { get; set; }
    }
}

