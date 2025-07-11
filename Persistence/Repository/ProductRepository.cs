using Core.Model;
using Core.ModelDto.Product;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataAccessHelper _dataAccessHelper;
        private readonly IConfiguration _config;
        public ProductRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
        {
            _dataAccessHelper = dataAccessHelper;
            _config = config;
        }

        public async Task<PaginatedListModel<ProductResponseDto>> GetProducts(int pageNumber, ProductFilterDto searchModel)
        {
            PaginatedListModel<ProductResponseDto> output = new PaginatedListModel<ProductResponseDto>();

            try
            {
                //var startDate = searchModel.StartDate.ToString("yyyy-MM-dd");
                //var endDate = searchModel.EndDate.ToString("yyyy-MM-dd");
                DynamicParameters p = new DynamicParameters();
                p.Add("CompanyId", searchModel.CompanyId);
                p.Add("CategoryId", searchModel.CategoryId);
                p.Add("SubCategoryId", searchModel.SubCategoryId);
                p.Add("MeasurementUnitId", searchModel.MeasurementUnitId);
                p.Add("PackTypeId", searchModel.PackTypeId);
                p.Add("ModelNo", searchModel.ModelNo);
                p.Add("Name", searchModel.Name);
                p.Add("IsActive", searchModel.IsActive);
                p.Add("PageNumber", pageNumber);
                p.Add("PageSize", Convert.ToInt32(_config["SiteSettings:PageSize"]));
                p.Add("TotalRecords", DbType.Int32, direction: ParameterDirection.Output);

                var result = await _dataAccessHelper.QueryData<ProductResponseDto, dynamic>("USP_Product_GetAll", p);
                int TotalRecords = p.Get<int>("TotalRecords");
                int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

                output = new PaginatedListModel<ProductResponseDto>
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


        public async Task<List<ProductResponseDto>> GetDistinctProducts(ProductFilterDto searchModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("CompanyId", searchModel.CompanyId);
            p.Add("CategoryId", searchModel.CategoryId);
            p.Add("SubCategoryId", searchModel.SubCategoryId);
            p.Add("MeasurementUnitId", searchModel.MeasurementUnitId);
            p.Add("PackTypeId", searchModel.PackTypeId);
            p.Add("ModelNo", searchModel.ModelNo);
            p.Add("Name", searchModel.Name);

            var output = await _dataAccessHelper.QueryData<ProductResponseDto, dynamic>("USP_Product_GetDistinct", p);

            return output;
        }

        public async Task<ProductResponseDto> GetProductById(int ProductId)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", ProductId);
            return (await _dataAccessHelper.QueryData<ProductResponseDto, dynamic>("USP_Product_GetById", p)).FirstOrDefault();
        }

        public async Task<List<ProductResponseDto>> GetProductsByName(ProductRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            var output = await _dataAccessHelper.QueryData<ProductResponseDto, dynamic>("USP_Product_GetProductsByName", p);
            return output;
        }


        public async Task<int> InsertProduct(ProductRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
            p.Add("CompanyId", insertRequestModel.CompanyId);
            p.Add("CategoryId", insertRequestModel.CategoryId);
            p.Add("SubCategoryId", insertRequestModel.SubCategoryId);
            p.Add("MeasurementUnitId", insertRequestModel.MeasurementUnitId);
            p.Add("PackTypeId", insertRequestModel.PackTypeId);
            p.Add("ModelNo", insertRequestModel.ModelNo);
            p.Add("PurchasePrice", insertRequestModel.PurchasePrice);
            p.Add("VAT", insertRequestModel.VAT);
            p.Add("Name", insertRequestModel.Name);
            p.Add("ShortName", insertRequestModel.ShortName);
            p.Add("Description", insertRequestModel.Description);
            p.Add("IsConsider", insertRequestModel.IsConsider);
            p.Add("IsBarCode", insertRequestModel.IsBarCode);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsActive", insertRequestModel.IsActive);

            await _dataAccessHelper.ExecuteData("USP_Product_Insert", p);
            return p.Get<int>("Id");
        }

        public async Task<int> UpdateProduct(int ProductId, ProductRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", ProductId);
            p.Add("CompanyId", insertRequestModel.CompanyId);
            p.Add("CategoryId", insertRequestModel.CategoryId);
            p.Add("SubCategoryId", insertRequestModel.SubCategoryId);
            p.Add("MeasurementUnitId", insertRequestModel.MeasurementUnitId);
            p.Add("PackTypeId", insertRequestModel.PackTypeId);
            p.Add("ModelNo", insertRequestModel.ModelNo);
            p.Add("PurchasePrice", insertRequestModel.PurchasePrice);
            p.Add("VAT", insertRequestModel.VAT);
            p.Add("Name", insertRequestModel.Name);
            p.Add("ShortName", insertRequestModel.ShortName);
            p.Add("Description", insertRequestModel.Description);
            p.Add("IsConsider", insertRequestModel.IsConsider);
            p.Add("IsBarCode", insertRequestModel.IsBarCode);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsActive", insertRequestModel.IsActive);

            return await _dataAccessHelper.ExecuteData("USP_Product_Update", p);
        }

        public async Task<int> DeleteProduct(int ProductId, ProductRequestDto deleteRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", ProductId);
            p.Add("CompanyId", deleteRequestModel.CompanyId);
            p.Add("CategoryId", deleteRequestModel.CategoryId);
            p.Add("SubCategoryId", deleteRequestModel.SubCategoryId);

            return await _dataAccessHelper.ExecuteData("USP_Product_Delete", p);
        }
    }
}
