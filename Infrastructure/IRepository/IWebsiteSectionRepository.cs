using Core.Model;
using Core.ModelDto.Product;
using Core.ModelDto.WebsiteSection;

namespace Infrastructure.IRepository;

public interface IWebsiteSectionRepository
{
    Task<PaginatedListModel<WebsiteSectionResponseDto>> GetWebsiteSections(int pageNumber, WebsiteSectionFilterDto searchModel);
    Task<WebsiteSectionResponseDto> GetWebsiteSectionById(int id);
    Task<int> InsertWebsiteSection(WebsiteSectionRequestDto requestModel);
    Task<int> UpdateWebsiteSection(int id, WebsiteSectionRequestDto requestModel);
    Task<int> DeleteWebsiteSection(int id, WebsiteSectionRequestDto requestModel);
}
