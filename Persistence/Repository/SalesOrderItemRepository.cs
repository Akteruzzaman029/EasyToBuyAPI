using Core.Model;
using Core.ModelDto.SalesOrderItem;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repository
{
    public class SalesOrderItemRepository : ISalesOrderItemRepository
    {
        private readonly DataAccessHelper _dataAccessHelper;
        private readonly IConfiguration _config;
        public SalesOrderItemRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
        {
            _dataAccessHelper = dataAccessHelper;
            _config = config;
        }

        public async Task<PaginatedListModel<SalesOrderItemResponseDto>> GetSalesOrderItems(int pageNumber, SalesOrderItemFilterDto searchModel)
        {
            PaginatedListModel<SalesOrderItemResponseDto> output = new PaginatedListModel<SalesOrderItemResponseDto>();

            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("SalesOrderId", searchModel.SalesOrderId);
                p.Add("ProductId", searchModel.ProductId);
                p.Add("IsActive", searchModel.IsActive);
                p.Add("PageNumber", pageNumber);
                p.Add("PageSize", Convert.ToInt32(_config["SiteSettings:PageSize"]));
                p.Add("TotalRecords", DbType.Int32, direction: ParameterDirection.Output);

                var result = await _dataAccessHelper.QueryData<SalesOrderItemResponseDto, dynamic>("USP_SalesOrderItem_GetAll", p);
                int TotalRecords = p.Get<int>("TotalRecords");
                int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

                output = new PaginatedListModel<SalesOrderItemResponseDto>
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

        public async Task<List<SalesOrderItemResponseDto>> GetDistinctSalesOrderItems(SalesOrderItemFilterDto searchModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("SalesOrderId", searchModel.SalesOrderId);
            p.Add("ProductId", searchModel.ProductId);
            p.Add("IsActive", searchModel.IsActive);
            var output = await _dataAccessHelper.QueryData<SalesOrderItemResponseDto, dynamic>("USP_SalesOrderItem_GetDistinct", p);
            return output;
        }

        public async Task<SalesOrderItemResponseDto> GetSalesOrderItemById(int SalesOrderItemId)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", SalesOrderItemId);
            return (await _dataAccessHelper.QueryData<SalesOrderItemResponseDto, dynamic>("USP_SalesOrderItem_GetById", p)).FirstOrDefault();
        }


        public async Task<int> InsertSalesOrderItem(SalesOrderItemRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
            p.Add("SalesOrderId", insertRequestModel.SalesOrderId);
            p.Add("ProductId", insertRequestModel.ProductId);
            p.Add("Quantity", insertRequestModel.Quantity);
            p.Add("UnitPrice", insertRequestModel.UnitPrice);
            p.Add("Discount", insertRequestModel.Discount);
            p.Add("IsFixedAmount", insertRequestModel.IsFixedAmount);
            p.Add("VatAmount", insertRequestModel.VatAmount);
            p.Add("LineTotal", insertRequestModel.LineTotal);
            p.Add("NetAmount", insertRequestModel.NetAmount);
            p.Add("IsActive", insertRequestModel.IsActive);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("UserId", insertRequestModel.UserId);

            await _dataAccessHelper.ExecuteData("USP_SalesOrderItem_Insert", p);
            return p.Get<int>("Id");
        }

        public async Task<int> UpdateSalesOrderItem(int SalesOrderItemId, SalesOrderItemRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", SalesOrderItemId);
            p.Add("SalesOrderId", insertRequestModel.SalesOrderId);
            p.Add("ProductId", insertRequestModel.ProductId);
            p.Add("Quantity", insertRequestModel.Quantity);
            p.Add("UnitPrice", insertRequestModel.UnitPrice);
            p.Add("Discount", insertRequestModel.Discount);
            p.Add("IsFixedAmount", insertRequestModel.IsFixedAmount);
            p.Add("VatAmount", insertRequestModel.VatAmount);
            p.Add("LineTotal", insertRequestModel.LineTotal);
            p.Add("NetAmount", insertRequestModel.NetAmount);
            p.Add("IsActive", insertRequestModel.IsActive);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("UserId", insertRequestModel.UserId);

            return await _dataAccessHelper.ExecuteData("USP_SalesOrderItem_Update", p);
        }

        public async Task<int> DeleteSalesOrderItem(int SalesOrderItemId, SalesOrderItemRequestDto deleteRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", SalesOrderItemId);
            p.Add("SalesOrderId", deleteRequestModel.SalesOrderId);
            return await _dataAccessHelper.ExecuteData("USP_SalesOrderItem_Delete", p);
        }
    }
}
