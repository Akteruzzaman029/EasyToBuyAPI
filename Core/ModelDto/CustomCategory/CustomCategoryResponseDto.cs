using Core.Model;

namespace Core.ModelDto.CustomCategory
{
    public class CustomCategoryResponseDto : CustomCategoryModel
    {
        public string CategoryName { get; set; } = string.Empty;
        public string CustomCategoryConfigName { get; set; } = string.Empty;
        public string Class { get; set; } = string.Empty;
        public int  ConfigSLNo { get; set; }
    }
}
