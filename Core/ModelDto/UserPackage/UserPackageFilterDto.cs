namespace Core.ModelDto.UserPackage
{
    public class UserPackageFilterDto
    {
        public string Name { get; set; } = string.Empty;
        public string IdNo { get; set; } = string.Empty;
        public int PackageId { get; set; } 
        public int PaymentStatus { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}
