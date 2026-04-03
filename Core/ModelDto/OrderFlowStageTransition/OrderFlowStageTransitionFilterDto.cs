namespace Core.ModelDto.OrderFlowStageTransition
{
    public class OrderFlowStageTransitionFilterDto
    {
        public int FlowId { get; set; }
        public int FromStageId { get; set; }
        public int ToStageId { get; set; }
        public Boolean IsAllowed { get; set; }
    }
}
