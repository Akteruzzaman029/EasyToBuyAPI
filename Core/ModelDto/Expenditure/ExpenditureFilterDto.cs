namespace Core.ModelDto.Expenditure
{
    public class ExpenditureFilterDto
    {
        public string Name { get; set; } = string.Empty;
        public int ExpenditureHeadId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}