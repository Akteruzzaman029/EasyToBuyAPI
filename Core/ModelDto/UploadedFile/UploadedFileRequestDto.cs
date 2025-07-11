namespace Core.ModelDto.UploadedFile
{
    public class UploadedFileRequestDto
    {
        public string UserId { get; set; } // Assuming this is tied to your user authentication
        public string FileName { get; set; }
        public byte[] FileData { get; set; } // Stored as binary data
        public long FileSize { get; set; }
        public string ContentType { get; set; }
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
    }
}
