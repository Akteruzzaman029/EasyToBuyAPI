namespace Core.ModelDto.OrderFlowStageTransition
{
    public class OrderFlowStageTransitionRequestDto
    {
        public int FlowId { get; set; }
        public int FromStageId { get; set; }
        public int ToStageId { get; set; }
        public bool IsAllowed { get; set; }
        public string? Remarks { get; set; }
        public string? UserId { get; set; }
    }
}
