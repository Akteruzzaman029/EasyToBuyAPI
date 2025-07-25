using Core.Model;
using Core.ModelDto.Category;

namespace Infrastructure.IRepository;

public interface ICategoryRepository
{
    Task<PaginatedListModel<CategoryResponseDto>> GetCategories(int pageNumber, CategoryFilterDto searchModel);
    Task<List<CategoryResponseDto>> GetDistinctCategories(CategoryFilterDto searchModel);
    Task<CategoryResponseDto> GetCategoryById(int categoryId);
    Task<int> InsertCategory(CategoryRequestDto insertRequestModel);
    Task<int> UpdateCategory(int categoryId, CategoryRequestDto updateRequestModel);
    Task<int> DeleteCategory(int categoryId, CategoryRequestDto deleteRequestModel);
}
