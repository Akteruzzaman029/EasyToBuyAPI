using Core.Model;
using static Core.BaseEnum;

namespace Core.ModelDto.Student
{
    public class StudentResponseDto : StudentModel
    {
        public string VehicleTypeName =>
               Enum.IsDefined(typeof(VehicleType), VehicleType)
                   ? ((VehicleType)VehicleType).ToString()
                   : "Unknown";

        public string LearningStageName =>
            Enum.IsDefined(typeof(LearningStage), LearningStage)
                ? ((LearningStage)LearningStage).ToString()
                : "Unknown";
        public string PackageName { get; set; } = string.Empty;
        public int PackageId { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal NetAmount { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        public int Nooflesson { get; set; }
        public int BookingLesson { get; set; }
        public int RemainingLesson { get; set; }
        public int CompleteLesson { get; set; }
        public int TotalLessons { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string StudentIdNo { get; set; } = string.Empty;

    }
}

