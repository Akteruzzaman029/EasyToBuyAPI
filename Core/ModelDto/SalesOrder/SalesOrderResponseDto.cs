using Core.Model;

namespace Core.ModelDto.SalesOrder
{
    public class SalesOrderResponseDto : SalesOrderModel
    {
        public int? CustomerName { get; set; }
        public int OrderTypeName { get; set; }
        public int FlowName { get; set; }
        public int? CurrentStageName { get; set; }
        public int? DeliveryChargeRuleName { get; set; }
        public int? PaymentStatusName { get; set; }
        public int? DeliveryStatusName { get; set; }
        public int? OrderStatusName { get; set; }
    }
}
