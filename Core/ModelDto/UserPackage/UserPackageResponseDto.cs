using Core.Model;
using Microsoft.VisualBasic.FileIO;
using static Core.BaseEnum;

namespace Core.ModelDto.UserPackage
{
    public class UserPackageResponseDto : UserPackageModel
    {
        public string UserName { get; set; }=string.Empty;
        public string PackageName { get; set; }=string.Empty;
        public string BookingDay { get; set; }=string.Empty;

        public string PaymentStatusName
        {
            get
            {
                return Enum.IsDefined(typeof(PackageStatusEnum), PaymentStatus)
                    ? ((PackageStatusEnum)PaymentStatus).ToString()
                    : "none";
            }
        }
        public int TotalLessons { get; set; }
        public int DurationInDays { get; set; }
        public Decimal Price  { get; set; }
        public Decimal PaymentAmount { get; set; }
        public Decimal RemaingAmount { get; set; }
    }
}
