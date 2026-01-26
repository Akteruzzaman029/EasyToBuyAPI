namespace Core.ModelDto.UserPackage
{
    public class UserPackageRequestDto
    {
        public string UserId { get; set; } = string.Empty;
        public int PackageId { get; set; }
        public int PaymentStatus { get; set; }
        public DateTime PackageStartDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int NoOfLesson { get; set; } = 0;
        public int LessonRate { get; set; } = 0;
        public int CompanyId { get; set; } = 0;
        public decimal Amount { get; set; } = 0;
        public decimal Discount { get; set; } = 0;
        public decimal NetAmount { get; set; } = 0;
        public string Remarks { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
