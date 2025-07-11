namespace Core.ModelDto.UploadedFile
{
    public class UploadedFileFilterDto
    {
        public string UserId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
