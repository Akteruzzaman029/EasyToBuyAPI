using Core.Model;
using Core.ModelDto.Category;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataAccessHelper _dataAccessHelper;
        private readonly IConfiguration _config;
        public CategoryRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
        {
            _dataAccessHelper = dataAccessHelper;
            _config = config;
        }

        public async Task<PaginatedListModel<CategoryResponseDto>> GetCategories(int pageNumber, CategoryFilterDto searchModel)
        {
            PaginatedListModel<CategoryResponseDto> output = new PaginatedListModel<CategoryResponseDto>();

            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("Name", searchModel.Name);
                p.Add("CompanyId", searchModel.CompanyId);
                p.Add("ParentId", searchModel.ParentId);
                p.Add("IsActive", searchModel.IsActive);
                p.Add("PageNumber", pageNumber);
                p.Add("PageSize", Convert.ToInt32(_config["SiteSettings:PageSize"]));
                p.Add("TotalRecords", DbType.Int32, direction: ParameterDirection.Output);

                var result = await _dataAccessHelper.QueryData<CategoryResponseDto, dynamic>("USP_Category_GetAll", p);
                int TotalRecords = p.Get<int>("TotalRecords");
                int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

                output = new PaginatedListModel<CategoryResponseDto>
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

        public async Task<List<CategoryResponseDto>> GetDistinctCategories(CategoryFilterDto searchModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Name", searchModel.Name);
            p.Add("CompanyId", searchModel.CompanyId);
            p.Add("ParentId", searchModel.ParentId);
            var output = await _dataAccessHelper.QueryData<CategoryResponseDto, dynamic>("USP_Category_GetDistinct", p);
            return output;
        }

        public async Task<CategoryResponseDto> GetCategoryById(int categoryId)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", categoryId);
            return (await _dataAccessHelper.QueryData<CategoryResponseDto, dynamic>("USP_Category_GetById", p)).FirstOrDefault();
        }


        public async Task<int> InsertCategory(CategoryRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
            p.Add("Name", insertRequestModel.Name);
            p.Add("CompanyId", insertRequestModel.CompanyId);
            p.Add("ParentId", insertRequestModel.ParentId);
            p.Add("SequenceNo", insertRequestModel.SequenceNo);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsActive", insertRequestModel.IsActive);
            p.Add("UserId", insertRequestModel.UserId);

            await _dataAccessHelper.ExecuteData("USP_Category_Insert", p);
            return p.Get<int>("Id");
        }

        public async Task<int> UpdateCategory(int categoryId, CategoryRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", categoryId);
            p.Add("Name", insertRequestModel.Name);
            p.Add("CompanyId", insertRequestModel.CompanyId);
            p.Add("ParentId", insertRequestModel.ParentId);
            p.Add("SequenceNo", insertRequestModel.SequenceNo);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsActive", insertRequestModel.IsActive);
            p.Add("UserId", insertRequestModel.UserId);

            return await _dataAccessHelper.ExecuteData("USP_Category_Update", p);
        }

        public async Task<int> DeleteCategory(int categoryId, CategoryRequestDto deleteRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", categoryId);
            p.Add("CompanyId", deleteRequestModel.CompanyId);
            p.Add("ParentId", deleteRequestModel.ParentId);
            return await _dataAccessHelper.ExecuteData("USP_Category_Delete", p);
        }
    }
}
