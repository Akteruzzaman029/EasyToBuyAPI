using Core.Model;
using Core.ModelDto.CustomCategoryConfig;

namespace Infrastructure.IRepository;

public interface ICustomCategoryConfigRepository
{
    Task<PaginatedListModel<CustomCategoryConfigResponseDto>> GetCustomCategoryConfigs(int pageNumber, CustomCategoryConfigFilterDto searchModel);
    Task<List<CustomCategoryConfigResponseDto>> GetDistinctCustomCategoryConfigs(CustomCategoryConfigFilterDto searchModel);
    Task<CustomCategoryConfigResponseDto> GetCustomCategoryConfigById(int CustomCategoryConfigId);
    Task<int> InsertCustomCategoryConfig(CustomCategoryConfigRequestDto insertRequestModel);
    Task<int> UpdateCustomCategoryConfig(int CustomCategoryConfigId, CustomCategoryConfigRequestDto updateRequestModel);
    Task<int> DeleteCustomCategoryConfig(int CustomCategoryConfigId, CustomCategoryConfigRequestDto deleteRequestModel);
}
