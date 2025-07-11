namespace Core.ModelDto.Post
{
    public class PostRequestDto
    {
        public int CategoryId { get; set; }
        public int ParentId { get; set; }
        public string ShortName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int? FileId { get; set; } 
        public string Remarks { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
