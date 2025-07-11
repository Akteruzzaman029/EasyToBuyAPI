namespace Core.ModelDto.ExpenditureHead
{
    public class ExpenditureHeadRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
