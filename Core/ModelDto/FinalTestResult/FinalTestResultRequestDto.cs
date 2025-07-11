namespace Core.ModelDto.FinalTestResult
{
    public class FinalTestResultRequestDto
    {
        public int? StudentId { get; set; }
        public string Instruction { get; set; } = string.Empty;
        public DateTime TestDate { get; set; }
        public int? TestType { get; set; }
        public int? Score { get; set; }
        public bool? Passed { get; set; }
        public string? EvaluatedBy { get; set; }
        public DateTime EvaluatedDate { get; set; }
        public string Remarks { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
