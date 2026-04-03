namespace Core.ModelDto.OrderFlowStage
{
    public class OrderFlowStageFilterDto
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public int SequenceNo { get; set; }
        public bool IsActive { get; set; }
    }
}
