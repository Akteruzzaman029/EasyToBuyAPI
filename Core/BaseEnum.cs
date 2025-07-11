using System.ComponentModel;

namespace Core;


public static class BaseEnum
{
    public enum LearningStage
    {
        NotStarted = 0,
        Introduction = 1,
        Intermediate = 2,
        Advanced = 3,
        Mastered = 4
    }

    public enum VehicleType
    {
        Car = 1,
        Motorcycle = 2,
        Truck = 3,
        Bus = 4,
        Van = 5,
    }
    public enum VehicleStatusEnum
    {
        Pending = 0,
        Active = 1,
        Inactive = 2,
        Maintenance = 3,
        Retired = 4
    }

    public enum InstructorLevelEnum
    {
        Junior = 1,
        Mid = 2,
        Senior = 3,
        Lead = 4,
        Learner = 5
    }

    public enum UserRoleEnum
    {
        SystemAdmin = 1,
        Admin = 2,
        Instructor = 3,
        Student = 4,
        GeneralUser = 5
    }

    public enum TransmissionTypeEnum
    {
        Manual=1,
        Automatic=2
    }

    public enum FuelTypeEnum
    {
        Petrol = 1,
        Diesel = 2,
        Electric = 3
    }

    public enum PackageStatusEnum
    {
        Pending = 1,
        [Description("In Progress")]
        InProgress = 2,
        Completed = 3,
        Rejected = 4
    }
    public enum AppointmentStatusEnum
    {
        Initialize = 1,
        Scheduled = 2,
        Completed = 3,
        Cancelled = 4,
        Rescheduled = 5,
        Rejected = 6,
    }

    public enum DayOfWeekEnum
    {
        Sunday = 0,
        Monday = 1,
        Tuesday = 2,
        Wednesday = 3,
        Thursday = 4,
        Friday = 5,
        Saturday = 6
    }

    // Add more enums as needed
}

