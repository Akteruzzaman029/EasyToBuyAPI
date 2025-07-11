using Microsoft.AspNetCore.Http;

namespace Core.ModelDto.Expenditure
{
    public class ExpenditureRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public int ExpenditureHeadId { get; set; }
        public int FileId { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
