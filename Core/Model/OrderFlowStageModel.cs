using System.ComponentModel.DataAnnotations;

namespace Core.Model
{
    public class OrderFlowStageModel: BaseEntity
    {
        public int OrderFlowId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public int SequenceNo { get; set; }
        public bool IsInitialStage { get; set; }
        public bool IsFinalStage { get; set; }
        public bool CustomerVisible { get; set; }
        public string? ColorCode { get; set; }
        public string? Icon { get; set; }
    }
}

