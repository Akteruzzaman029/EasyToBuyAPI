using System.ComponentModel.DataAnnotations;

namespace Core.Model
{
    public class OrderDeliveryModel : BaseEntity
    {
        public int CompanyId { get; set; }
        public int SalesOrderId { get; set; }
        public int? DeliveryProviderId { get; set; }
        public int? RiderId { get; set; }
        public string? TrackingNo { get; set; }
        public string? PickupAddress { get; set; }
        public string? DeliveryAddress { get; set; }

        public decimal? PickupLatitude { get; set; }
        public decimal? PickupLongitude { get; set; }
        public decimal? DeliveryLatitude { get; set; }
        public decimal? DeliveryLongitude { get; set; }
        public decimal? DistanceKm { get; set; }
        public decimal? WeightKg { get; set; }

        public int? DeliveryZoneId { get; set; }
        public int? ShipmentStatus { get; set; }

        public DateTime? AssignedAt { get; set; }
        public DateTime? PickupAt { get; set; }
        public DateTime? DispatchAt { get; set; }
        public DateTime? OutForDeliveryAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public DateTime? FailedAt { get; set; }
        public DateTime? ReturnInitiatedAt { get; set; }

        public string? ReceiverName { get; set; }
        public string? ReceiverPhone { get; set; }
        public string? FailureReason { get; set; }
    }
}

