namespace Core.ModelDto.Category
{
    public class CategoryRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public int SequenceNo { get; set; } 
        public int ParentId { get; set; } 
        public string Remarks { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
