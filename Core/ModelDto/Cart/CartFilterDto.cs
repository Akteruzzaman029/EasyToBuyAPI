namespace Core.ModelDto.Cart
{
    public class CartFilterDto
    {
        public int CompanyId { get; set; }
        public string? UserId { get; set; }         // Nullable for guest
        public string? GuestId { get; set; }        // Nullable for logged-in user
        public int CartType { get; set; }           // e.g., 1 = Normal Cart, 2 = Wishlist, etc.
        public bool IsActive { get; set; }
    }
}
