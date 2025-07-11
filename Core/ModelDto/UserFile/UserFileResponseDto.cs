using Core.Model;

namespace Core.ModelDto.UserFile
{
    public class UserFileResponseDto : UserFileModel
    {
        public string FileName { get; set; } = string.Empty;
    }
}
