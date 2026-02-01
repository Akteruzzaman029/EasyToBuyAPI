namespace Core.ModelDto.WebsiteSection
{
    public class WebsiteSectionRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string HeaderName { get; set; } = string.Empty;
        public int SequenceNo { get; set; }
        public int? FileId { get; set; }
        public string Remarks { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string UserId { get; set; } = string.Empty; 
    }

    public class WebsiteSectionResponseDto : WebsiteSectionRequestDto
    {
        public int Id { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; } = string.Empty;
        public DateTime? LastModifiedDate { get; set; }
    }

    public class WebsiteSectionFilterDto
    {
        public string Name { get; set; } = string.Empty;
        public string HeaderName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}