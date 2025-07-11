namespace Core.ModelDto.Student
{
    public class StudentRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public int LearningStage { get; set; }
        public int FileId { get; set; }
        public string PostalCode { get; set; } = string.Empty;
        public int BookingId { get; set; } = 0;
        public int VehicleType { get; set; } = 0;
        public int Nooflesson { get; set; } = 0;
        public int LessonRate { get; set; } = 0;
        public decimal Amount { get; set; } = 0;
        public decimal Discount { get; set; } = 0;
        public decimal NetAmount { get; set; } = 0;
        public decimal PaymentAmount { get; set; } = 0;
        public string Remarks { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
