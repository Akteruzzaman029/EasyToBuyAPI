namespace Core.Model
{
    public class CartModel : BaseEntity
    {
        public int CompanyId { get; set; }
        public string? UserId { get; set; }         // Nullable for guest
        public string? GuestId { get; set; }        // Nullable for logged-in user
        public int CartType { get; set; }           // e.g., 1 = Normal Cart, 2 = Wishlist, etc.
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }

    }
}
