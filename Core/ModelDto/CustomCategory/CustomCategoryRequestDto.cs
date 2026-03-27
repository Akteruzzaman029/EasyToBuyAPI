namespace Core.ModelDto.CustomCategory
{
    public class CustomCategoryRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string TypeTag { get; set; } = string.Empty;
        public int SequenceNo { get; set; } 
        public int FileId { get; set; } 
        public int CategoryId { get; set; } 
        public int CompanyId { get; set; } 
        public string UserId { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
