using Core.Model;

namespace Core.ModelDto.OrderFlowStageTransition
{
    public class OrderFlowStageTransitionResponseDto : OrderFlowStageTransitionModel
    {
        public string FlowName { get; set; }
        public string FromStageName { get; set; }
        public string ToStageName { get; set; }
    }
}
