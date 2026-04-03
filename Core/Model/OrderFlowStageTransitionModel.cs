using System.ComponentModel.DataAnnotations;

namespace Core.Model
{
    public class OrderFlowStageTransitionModel
    {
        public int Id { get; set; }
        public int FlowId { get; set; }
        public int FromStageId { get; set; }
        public int ToStageId { get; set; }
        public bool IsAllowed { get; set; }
        public string? Remarks { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

