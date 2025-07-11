namespace Core.ModelDto.PackType
{
    public class PackTypeRequestDto
    {
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Remarks { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
