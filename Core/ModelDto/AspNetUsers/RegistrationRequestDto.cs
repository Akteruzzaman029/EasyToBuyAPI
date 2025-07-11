using System.ComponentModel.DataAnnotations;

namespace Core.ModelDto.AspNetUsers
{
    public class RegistrationRequestDto
    {
        [Required, Display(Name = "Name")]
        public string FullName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;

        [Required, EmailAddress, Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public int LearningStage { get; set; } = 0;
        public int FileId { get; set; } = 0;
        public int VehicleType { get; set; } = 0;
        public int Type { get; set; } = 0;
        public int PackageId { get; set; } = 0;
        public int BookingId { get; set; } = 0;
        public int Nooflesson { get; set; } = 0;
        public int LessonRate { get; set; } = 0;
        public decimal Amount { get; set; }= 0;
        public decimal PaymentAmount { get; set; }= 0;
        public decimal Discount { get; set; }= 0;
        public decimal NetAmount { get; set; } = 0;

        [Required, Phone, Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }

        [Required, StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = string.Empty;

    }
}
