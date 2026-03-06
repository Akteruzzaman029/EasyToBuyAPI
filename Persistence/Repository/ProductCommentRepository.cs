using Core.Model;
using Core.ModelDto.ProductComment;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repository
{
    public class ProductCommentRepository : IProductCommentRepository
    {
        private readonly DataAccessHelper _dataAccessHelper;
        private readonly IConfiguration _config;
        public ProductCommentRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
        {
            _dataAccessHelper = dataAccessHelper;
            _config = config;
        }

        public async Task<PaginatedListModel<ProductCommentResponseDto>> GetProductComments(int pageNumber, ProductCommentFilterDto searchModel)
        {
            PaginatedListModel<ProductCommentResponseDto> output = new PaginatedListModel<ProductCommentResponseDto>();

            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("CompanyId", searchModel.CompanyId);
                p.Add("ProductId", searchModel.ProductId);
                p.Add("UserId", searchModel.UserId);
                p.Add("Comment", searchModel.Comment);
                p.Add("IsActive", searchModel.IsActive);
                p.Add("PageNumber", pageNumber);
                p.Add("PageSize", Convert.ToInt32(_config["SiteSettings:PageSize"]));
                p.Add("TotalRecords", DbType.Int32, direction: ParameterDirection.Output);

                var result = await _dataAccessHelper.QueryData<ProductCommentResponseDto, dynamic>("USP_ProductComment_GetAll", p);
                int TotalRecords = p.Get<int>("TotalRecords");
                int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

                output = new PaginatedListModel<ProductCommentResponseDto>
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


        public async Task<List<ProductCommentResponseDto>> GetDistinctProductComments(ProductCommentFilterDto searchModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("CompanyId", searchModel.CompanyId);
            p.Add("ProductId", searchModel.ProductId);
            p.Add("UserId", searchModel.UserId);
            p.Add("Comment", searchModel.Comment);

            var output = await _dataAccessHelper.QueryData<ProductCommentResponseDto, dynamic>("USP_ProductComment_GetDistinct", p);

            return output;
        }

        public async Task<ProductCommentResponseDto> GetProductCommentById(int ProductCommentId)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", ProductCommentId);
            return (await _dataAccessHelper.QueryData<ProductCommentResponseDto, dynamic>("USP_ProductComment_GetById", p)).FirstOrDefault();
        }

        public async Task<List<ProductCommentResponseDto>> GetProductCommentsByName(ProductCommentRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("CompanyId", insertRequestModel.CompanyId);
            p.Add("ProductId", insertRequestModel.ProductId);
            p.Add("UserId", insertRequestModel.UserId);
            p.Add("Comment", insertRequestModel.Comment);
            var output = await _dataAccessHelper.QueryData<ProductCommentResponseDto, dynamic>("USP_ProductComment_GetProductCommentsByName", p);
            return output;
        }


        public async Task<int> InsertProductComment(ProductCommentRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
            p.Add("CompanyId", insertRequestModel.CompanyId);
            p.Add("ProductId", insertRequestModel.ProductId);
            p.Add("UserId", insertRequestModel.UserId);
            p.Add("Comment", insertRequestModel.Comment);
            p.Add("FileId", insertRequestModel.FileId);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsActive", insertRequestModel.IsActive);

            await _dataAccessHelper.ExecuteData("USP_ProductComment_Insert", p);
            return p.Get<int>("Id");
        }

        public async Task<int> UpdateProductComment(int ProductCommentId, ProductCommentRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", ProductCommentId);
            p.Add("CompanyId", insertRequestModel.CompanyId);
            p.Add("ProductId", insertRequestModel.ProductId);
            p.Add("UserId", insertRequestModel.UserId);
            p.Add("Comment", insertRequestModel.Comment);
            p.Add("FileId", insertRequestModel.FileId);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsActive", insertRequestModel.IsActive);

            return await _dataAccessHelper.ExecuteData("USP_ProductComment_Update", p);
        }

        public async Task<int> DeleteProductComment(int ProductCommentId, ProductCommentRequestDto deleteRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", ProductCommentId);
            p.Add("CompanyId", deleteRequestModel.CompanyId);
            p.Add("ProductId", deleteRequestModel.ProductId);
            p.Add("UserId", deleteRequestModel.UserId);

            return await _dataAccessHelper.ExecuteData("USP_ProductComment_Delete", p);
        }
    }
}
