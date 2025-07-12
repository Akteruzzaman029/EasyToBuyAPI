namespace Core.Model
{
    public class StatusMasterModel : BaseEntity
    {
        public int CompanyId { get; set; }
        public string? StatusGroup { get; set; }           // e.g., 'Order', 'Payment', 'User'
        public int StatusCode { get; set; }                // e.g., 0, 1, 2 (for each group)
        public string? StatusName { get; set; }            // e.g., 'Pending', 'Paid'
        public string? Description { get; set; }
    }
}
