namespace Core.ModelDto.OrderFlowStage
{
    public class OrderFlowStageRequestDto
    {
        public int OrderFlowId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int SequenceNo { get; set; }
        public bool IsInitialStage { get; set; }
        public bool IsFinalStage { get; set; }
        public bool CustomerVisible { get; set; }
        public string? ColorCode { get; set; }
        public string? Icon { get; set; }
        public string UserId { get; set; }
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
    }
}
