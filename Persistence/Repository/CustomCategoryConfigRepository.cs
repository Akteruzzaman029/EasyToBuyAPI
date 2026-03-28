using Core.Model;
using Core.ModelDto.CustomCategoryConfig;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repository
{
    public class CustomCategoryConfigRepository : ICustomCategoryConfigRepository
    {
        private readonly DataAccessHelper _dataAccessHelper;
        private readonly IConfiguration _config;
        public CustomCategoryConfigRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
        {
            _dataAccessHelper = dataAccessHelper;
            _config = config;
        }

        public async Task<PaginatedListModel<CustomCategoryConfigResponseDto>> GetCustomCategoryConfigs(int pageNumber, CustomCategoryConfigFilterDto searchModel)
        {
            PaginatedListModel<CustomCategoryConfigResponseDto> output = new PaginatedListModel<CustomCategoryConfigResponseDto>();

            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("Name", searchModel.Name);
                p.Add("CompanyId", searchModel.CompanyId);
                p.Add("IsActive", searchModel.IsActive);
                p.Add("PageNumber", pageNumber);
                p.Add("PageSize", Convert.ToInt32(_config["SiteSettings:PageSize"]));
                p.Add("TotalRecords", DbType.Int32, direction: ParameterDirection.Output);

                var result = await _dataAccessHelper.QueryData<CustomCategoryConfigResponseDto, dynamic>("USP_CustomCategoryConfig_GetAll", p);
                int TotalRecords = p.Get<int>("TotalRecords");
                int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

                output = new PaginatedListModel<CustomCategoryConfigResponseDto>
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

        public async Task<List<CustomCategoryConfigResponseDto>> GetDistinctCustomCategoryConfigs(CustomCategoryConfigFilterDto searchModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Name", string.IsNullOrWhiteSpace(searchModel.Name) ? null : searchModel.Name);
            p.Add("CompanyId", searchModel.CompanyId);
            var output = await _dataAccessHelper.QueryData<CustomCategoryConfigResponseDto, dynamic>("USP_CustomCategoryConfig_GetDistinct", p);
            return output;
        }

        public async Task<CustomCategoryConfigResponseDto> GetCustomCategoryConfigById(int CustomCategoryConfigId)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", CustomCategoryConfigId);
            return (await _dataAccessHelper.QueryData<CustomCategoryConfigResponseDto, dynamic>("USP_CustomCategoryConfig_GetById", p)).FirstOrDefault();
        }


        public async Task<int> InsertCustomCategoryConfig(CustomCategoryConfigRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
            p.Add("Name", insertRequestModel.Name);
            p.Add("Class", insertRequestModel.Class);
            p.Add("CompanyId", insertRequestModel.CompanyId);
            p.Add("SequenceNo", insertRequestModel.SequenceNo);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsActive", insertRequestModel.IsActive);
            p.Add("UserId", insertRequestModel.UserId);

            await _dataAccessHelper.ExecuteData("USP_CustomCategoryConfig_Insert", p);
            return p.Get<int>("Id");
        }

        public async Task<int> UpdateCustomCategoryConfig(int CustomCategoryConfigId, CustomCategoryConfigRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", CustomCategoryConfigId);
            p.Add("Name", insertRequestModel.Name);
            p.Add("Class", insertRequestModel.Class);
            p.Add("CompanyId", insertRequestModel.CompanyId);
            p.Add("SequenceNo", insertRequestModel.SequenceNo);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsActive", insertRequestModel.IsActive);
            p.Add("UserId", insertRequestModel.UserId);

            return await _dataAccessHelper.ExecuteData("USP_CustomCategoryConfig_Update", p);
        }

        public async Task<int> DeleteCustomCategoryConfig(int CustomCategoryConfigId, CustomCategoryConfigRequestDto deleteRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", CustomCategoryConfigId);
            p.Add("CompanyId", deleteRequestModel.CompanyId);
            return await _dataAccessHelper.ExecuteData("USP_CustomCategoryConfig_Delete", p);
        }
    }
}
