using Core.Model;
using Core.ModelDto.CustomCategory;

namespace Infrastructure.IRepository;

public interface ICustomCategoryRepository
{
    Task<PaginatedListModel<CustomCategoryResponseDto>> GetCustomCategories(int pageNumber, CustomCategoryFilterDto searchModel);
    Task<List<CustomCategoryResponseDto>> GetDistinctCustomCategories(CustomCategoryFilterDto searchModel);
    Task<CustomCategoryResponseDto> GetCustomCategoryById(int CustomCategoryId);
    Task<int> InsertCustomCategory(CustomCategoryRequestDto insertRequestModel);
    Task<int> UpdateCustomCategory(int CustomCategoryId, CustomCategoryRequestDto updateRequestModel);
    Task<int> DeleteCustomCategory(int CustomCategoryId, CustomCategoryRequestDto deleteRequestModel);
}
