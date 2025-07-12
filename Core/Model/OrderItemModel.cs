namespace Core.Model
{
    public class OrderItemModel : BaseEntity
    {
        public int? OrderId { get; set; }            // Foreign key to Order table
        public int? ProductId { get; set; }          // Foreign key to Product table
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }        // Discount value
        public bool IsFixedAmount { get; set; }      // True: fixed amount, False: percentage

    }
}
