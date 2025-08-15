using Core.Model;
using Core.ModelDto.Company;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repository
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly DataAccessHelper _dataAccessHelper;
        private readonly IConfiguration _config;
        public CompanyRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
        {
            _dataAccessHelper = dataAccessHelper;
            _config = config;
        }

        public async Task<PaginatedListModel<CompanyResponseDto>> GetCompanys(int pageNumber, CompanyFilterDto searchModel)
        {
            PaginatedListModel<CompanyResponseDto> output = new PaginatedListModel<CompanyResponseDto>();

            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("Code", searchModel.Code);
                p.Add("Name", searchModel.Name);
                p.Add("ShortName", searchModel.ShortName);
                p.Add("Address", searchModel.Address);
                p.Add("IsActive", searchModel.IsActive);
                p.Add("PageNumber", pageNumber);
                p.Add("PageSize", Convert.ToInt32(_config["SiteSettings:PageSize"]));
                p.Add("TotalRecords", DbType.Int32, direction: ParameterDirection.Output);

                var result = await _dataAccessHelper.QueryData<CompanyResponseDto, dynamic>("USP_Company_GetAll", p);
                int TotalRecords = p.Get<int>("TotalRecords");
                int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

                output = new PaginatedListModel<CompanyResponseDto>
                {
                    PageIndex = pageNumber,
                    TotalRecords = TotalRecords,
                    TotalPages = totalPages,
                    HasPreviousPage = pageNumber > 1,
                    HasNextPage = pageNumber < totalPages,
                    Items = result.ToList()
                };

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return output;
        }


        public async Task<List<CompanyResponseDto>> GetDistinctCompanys(CompanyFilterDto searchModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Code", searchModel.Code);
            p.Add("Name", searchModel.Name);
            p.Add("ShortName", searchModel.ShortName);
            p.Add("Address", searchModel.Address);

            var output = await _dataAccessHelper.QueryData<CompanyResponseDto, dynamic>("USP_Company_GetDistinct", p);

            return output;
        }

        public async Task<CompanyResponseDto> GetCompanyByCode(string code)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Code", code);
            return (await _dataAccessHelper.QueryData<CompanyResponseDto, dynamic>("USP_Company_GetByCode", p)).FirstOrDefault();
        }


        public async Task<CompanyResponseDto> GetCompanyById(int CompanyId)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", CompanyId);
            return (await _dataAccessHelper.QueryData<CompanyResponseDto, dynamic>("USP_Company_GetById", p)).FirstOrDefault();
        }

        public async Task<List<CompanyResponseDto>> GetCompanysByName(CompanyRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Code", insertRequestModel.Code);
            p.Add("Name", insertRequestModel.Name);
            var output = await _dataAccessHelper.QueryData<CompanyResponseDto, dynamic>("USP_Company_GetCompanysByName", p);
            return output;
        }


        public async Task<int> InsertCompany(CompanyRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
            p.Add("Code", insertRequestModel.Code);
            p.Add("Name", insertRequestModel.Name);
            p.Add("ShortName", insertRequestModel.ShortName);
            p.Add("Address", insertRequestModel.Address);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsActive", insertRequestModel.IsActive);

            await _dataAccessHelper.ExecuteData("USP_Company_Insert", p);
            return p.Get<int>("Id");
        }

        public async Task<int> UpdateCompany(int CompanyId, CompanyRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", CompanyId);
            p.Add("Code", insertRequestModel.Code);
            p.Add("Name", insertRequestModel.Name);
            p.Add("ShortName", insertRequestModel.ShortName);
            p.Add("Address", insertRequestModel.Address);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsActive", insertRequestModel.IsActive);


            return await _dataAccessHelper.ExecuteData("USP_Company_Update", p);
        }

        public async Task<int> DeleteCompany(int CompanyId, CompanyRequestDto deleteRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", CompanyId);
            p.Add("Code", deleteRequestModel.Code);

            return await _dataAccessHelper.ExecuteData("USP_Company_Delete", p);
        }
    }
}
