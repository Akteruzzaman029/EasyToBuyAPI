using Core.Model;
using static Core.BaseEnum;

namespace Core.ModelDto.Vehicle
{
    public class VehicleResponseDto : VehicleModel
    {

        public string UserName { get; set; } = string.Empty;
        public string FuelTypeName
        {
            get
            {
                return Enum.IsDefined(typeof(FuelTypeEnum), FuelType)
                    ? ((FuelTypeEnum)FuelType).ToString()
                    : "none";
            }
        }
        public string TransmissionTypeName
        {
            get
            {
                return Enum.IsDefined(typeof(TransmissionTypeEnum), TransmissionType)
                    ? ((TransmissionTypeEnum)TransmissionType).ToString()
                    : "none";
            }
        }
        public string StatusName
        {
            get
            {
                return Enum.IsDefined(typeof(VehicleStatusEnum), Status)
                    ? ((VehicleStatusEnum)Status).ToString()
                    : "none";
            }
        }
    }
}
