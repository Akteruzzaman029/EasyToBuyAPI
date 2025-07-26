using Core.Model;

namespace Core.ModelDto.Cart
{
    public class CartResponseDto : CartModel
    {
        public string ProductName { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal Quantity { get; set; }
        public decimal Discount { get; set; }
        public decimal UnitPriceAfterDiscount { get; set; }
        public decimal TotalPriceAfterDiscount { get; set; }

    }
}
