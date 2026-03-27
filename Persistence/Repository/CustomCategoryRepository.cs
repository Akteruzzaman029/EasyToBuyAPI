using Core.Model;
using Core.ModelDto.CustomCategory;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repository
{
    public class CustomCategoryRepository : ICustomCategoryRepository
    {
        private readonly DataAccessHelper _dataAccessHelper;
        private readonly IConfiguration _config;
        public CustomCategoryRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
        {
            _dataAccessHelper = dataAccessHelper;
            _config = config;
        }

        public async Task<PaginatedListModel<CustomCategoryResponseDto>> GetCustomCategories(int pageNumber, CustomCategoryFilterDto searchModel)
        {
            PaginatedListModel<CustomCategoryResponseDto> output = new PaginatedListModel<CustomCategoryResponseDto>();

            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("Name", searchModel.Name);
                p.Add("TypeTag", searchModel.TypeTag);
                p.Add("CompanyId", searchModel.CompanyId);
                p.Add("CategoryId", searchModel.CategoryId);
                p.Add("IsActive", searchModel.IsActive);
                p.Add("PageNumber", pageNumber);
                p.Add("PageSize", Convert.ToInt32(_config["SiteSettings:PageSize"]));
                p.Add("TotalRecords", DbType.Int32, direction: ParameterDirection.Output);

                var result = await _dataAccessHelper.QueryData<CustomCategoryResponseDto, dynamic>("USP_CustomCategory_GetAll", p);
                int TotalRecords = p.Get<int>("TotalRecords");
                int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

                output = new PaginatedListModel<CustomCategoryResponseDto>
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

        public async Task<List<CustomCategoryResponseDto>> GetDistinctCustomCategories(CustomCategoryFilterDto searchModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Name", string.IsNullOrWhiteSpace(searchModel.Name) ? null : searchModel.Name);
            p.Add("TypeTag", string.IsNullOrWhiteSpace(searchModel.TypeTag) ? null : searchModel.TypeTag);
            p.Add("CompanyId", searchModel.CompanyId);
            p.Add("CategoryId", searchModel.CategoryId); 
            var output = await _dataAccessHelper.QueryData<CustomCategoryResponseDto, dynamic>("USP_CustomCategory_GetDistinct", p);
            return output;
        }

        public async Task<CustomCategoryResponseDto> GetCustomCategoryById(int CustomCategoryId)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", CustomCategoryId);
            return (await _dataAccessHelper.QueryData<CustomCategoryResponseDto, dynamic>("USP_CustomCategory_GetById", p)).FirstOrDefault();
        }


        public async Task<int> InsertCustomCategory(CustomCategoryRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
            p.Add("Name", insertRequestModel.Name);
            p.Add("TypeTag", insertRequestModel.TypeTag);
            p.Add("FileId", insertRequestModel.FileId);
            p.Add("CompanyId", insertRequestModel.CompanyId);
            p.Add("CategoryId", insertRequestModel.CategoryId);
            p.Add("SequenceNo", insertRequestModel.SequenceNo);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsActive", insertRequestModel.IsActive);
            p.Add("UserId", insertRequestModel.UserId);

            await _dataAccessHelper.ExecuteData("USP_CustomCategory_Insert", p);
            return p.Get<int>("Id");
        }

        public async Task<int> UpdateCustomCategory(int CustomCategoryId, CustomCategoryRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", CustomCategoryId);
            p.Add("Name", insertRequestModel.Name);
            p.Add("TypeTag", insertRequestModel.TypeTag);
            p.Add("FileId", insertRequestModel.FileId);
            p.Add("CompanyId", insertRequestModel.CompanyId);
            p.Add("CategoryId", insertRequestModel.CategoryId);
            p.Add("SequenceNo", insertRequestModel.SequenceNo);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsActive", insertRequestModel.IsActive);
            p.Add("UserId", insertRequestModel.UserId);

            return await _dataAccessHelper.ExecuteData("USP_CustomCategory_Update", p);
        }

        public async Task<int> DeleteCustomCategory(int CustomCategoryId, CustomCategoryRequestDto deleteRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", CustomCategoryId);
            p.Add("CompanyId", deleteRequestModel.CompanyId);
            p.Add("CategoryId", deleteRequestModel.CategoryId);
            return await _dataAccessHelper.ExecuteData("USP_CustomCategory_Delete", p);
        }
    }
}
