using Core.Model;
using Core.ModelDto.ProductRate;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repository
{
    public class ProductRateRepository : IProductRateRepository
    {
        private readonly DataAccessHelper _dataAccessHelper;
        private readonly IConfiguration _config;
        public ProductRateRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
        {
            _dataAccessHelper = dataAccessHelper;
            _config = config;
        }

        public async Task<PaginatedListModel<ProductRateResponseDto>> GetProductRates(int pageNumber, ProductRateFilterDto searchModel)
        {
            PaginatedListModel<ProductRateResponseDto> output = new PaginatedListModel<ProductRateResponseDto>();

            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("CompanyId", searchModel.CompanyId);
                p.Add("ProductId", searchModel.ProductId);
                p.Add("UserId", searchModel.UserId);
                p.Add("TypeId", searchModel.TypeId);
                p.Add("Rate", searchModel.Rate);
                p.Add("IsActive", searchModel.IsActive);
                p.Add("PageNumber", pageNumber);
                p.Add("PageSize", Convert.ToInt32(_config["SiteSettings:PageSize"]));
                p.Add("TotalRecords", DbType.Int32, direction: ParameterDirection.Output);

                var result = await _dataAccessHelper.QueryData<ProductRateResponseDto, dynamic>("USP_ProductRate_GetAll", p);
                int TotalRecords = p.Get<int>("TotalRecords");
                int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

                output = new PaginatedListModel<ProductRateResponseDto>
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


        public async Task<List<ProductRateResponseDto>> GetDistinctProductRates(ProductRateFilterDto searchModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("CompanyId", searchModel.CompanyId);
            p.Add("ProductId", searchModel.ProductId);
            p.Add("UserId", searchModel.UserId);
            p.Add("Rate", searchModel.Rate);
            p.Add("TypeId", searchModel.TypeId);

            var output = await _dataAccessHelper.QueryData<ProductRateResponseDto, dynamic>("USP_ProductRate_GetDistinct", p);

            return output;
        }

        public async Task<ProductRateResponseDto> GetProductRateById(int ProductRateId)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", ProductRateId);
            return (await _dataAccessHelper.QueryData<ProductRateResponseDto, dynamic>("USP_ProductRate_GetById", p)).FirstOrDefault();
        }

        public async Task<List<ProductRateResponseDto>> GetProductRatesByName(ProductRateRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("CompanyId", insertRequestModel.CompanyId);
            p.Add("ProductId", insertRequestModel.ProductId);
            p.Add("UserId", insertRequestModel.UserId);
            p.Add("Rate", insertRequestModel.Rate);
            p.Add("TypeId", insertRequestModel.TypeId);
            var output = await _dataAccessHelper.QueryData<ProductRateResponseDto, dynamic>("USP_ProductRate_GetProductRatesByName", p);
            return output;
        }


        public async Task<int> InsertProductRate(ProductRateRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
            p.Add("CompanyId", insertRequestModel.CompanyId);
            p.Add("ProductId", insertRequestModel.ProductId);
            p.Add("UserId", insertRequestModel.UserId);
            p.Add("TypeId", insertRequestModel.TypeId);
            p.Add("Rate", insertRequestModel.Rate);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsActive", insertRequestModel.IsActive);

            await _dataAccessHelper.ExecuteData("USP_ProductRate_Insert", p);
            return p.Get<int>("Id");
        }

        public async Task<int> UpdateProductRate(int ProductRateId, ProductRateRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", ProductRateId);
            p.Add("CompanyId", insertRequestModel.CompanyId);
            p.Add("ProductId", insertRequestModel.ProductId);
            p.Add("UserId", insertRequestModel.UserId);
            p.Add("TypeId", insertRequestModel.TypeId);
            p.Add("Rate", insertRequestModel.Rate);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsActive", insertRequestModel.IsActive);

            return await _dataAccessHelper.ExecuteData("USP_ProductRate_Update", p);
        }

        public async Task<int> DeleteProductRate(int ProductRateId, ProductRateRequestDto deleteRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", ProductRateId);
            p.Add("CompanyId", deleteRequestModel.CompanyId);
            p.Add("ProductId", deleteRequestModel.ProductId);
            p.Add("UserId", deleteRequestModel.UserId);

            return await _dataAccessHelper.ExecuteData("USP_ProductRate_Delete", p);
        }
    }
}
