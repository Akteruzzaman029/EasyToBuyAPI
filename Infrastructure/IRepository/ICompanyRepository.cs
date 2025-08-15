using Core.Model;
using Core.ModelDto.Company;

namespace Infrastructure.IRepository;

public interface ICompanyRepository
{
    Task<PaginatedListModel<CompanyResponseDto>> GetCompanys(int pageNumber, CompanyFilterDto searchModel);
    Task<List<CompanyResponseDto>> GetDistinctCompanys(CompanyFilterDto searchModel);
    Task<CompanyResponseDto> GetCompanyById(int CompanyId);
    Task<CompanyResponseDto> GetCompanyByCode(string companyCode);
    Task<List<CompanyResponseDto>> GetCompanysByName(CompanyRequestDto insertRequestModel);
    Task<int> InsertCompany(CompanyRequestDto insertRequestModel);
    Task<int> UpdateCompany(int CompanyId, CompanyRequestDto updateRequestModel);
    Task<int> DeleteCompany(int CompanyId, CompanyRequestDto deleteRequestModel);
}
