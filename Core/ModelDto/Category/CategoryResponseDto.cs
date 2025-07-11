using Core.Model;

namespace Core.ModelDto.Category
{
    public class CategoryResponseDto : CategoryModel
    {
        public string SubCategoryName { get; set; } = string.Empty;
    }
}
