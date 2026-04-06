using Core.Model;
using Core.ModelDto.SalesOrder;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;
using static Core.BaseEnum;

namespace Persistence.Repository
{
    public class SalesOrderRepository : ISalesOrderRepository
    {
        private readonly DataAccessHelper _dataAccessHelper;
        private readonly IConfiguration _config;
        public SalesOrderRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
        {
            _dataAccessHelper = dataAccessHelper;
            _config = config;
        }

        public async Task<PaginatedListModel<SalesOrderResponseDto>> GetSalesOrders(int pageNumber, SalesOrderFilterDto searchModel)
        {
            PaginatedListModel<SalesOrderResponseDto> output = new PaginatedListModel<SalesOrderResponseDto>();

            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("OrderNo",searchModel.OrderNo);
                p.Add("CompanyId",searchModel.CompanyId);
                p.Add("UserId", searchModel.UserId);
                p.Add("OrderTypeId",searchModel.OrderTypeId);
                p.Add("FlowId",searchModel.FlowId);
                p.Add("CurrentStageId",searchModel.CurrentStageId);
                p.Add("DeliveryChargeRuleId",searchModel.DeliveryChargeRuleId);
                p.Add("PaymentStatus",searchModel.PaymentStatus);
                p.Add("DeliveryStatus",searchModel.DeliveryStatus);
                p.Add("OrderStatus", searchModel.OrderStatus);
                p.Add("IsActive", searchModel.IsActive);
                p.Add("PageNumber", pageNumber);
                p.Add("PageSize", Convert.ToInt32(_config["SiteSettings:PageSize"]));
                p.Add("TotalRecords", DbType.Int32, direction: ParameterDirection.Output);

                var result = await _dataAccessHelper.QueryData<SalesOrderResponseDto, dynamic>("USP_SalesOrder_GetAll", p);
                int TotalRecords = p.Get<int>("TotalRecords");
                int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

                output = new PaginatedListModel<SalesOrderResponseDto>
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

        public async Task<List<SalesOrderResponseDto>> GetDistinctSalesOrders(SalesOrderFilterDto searchModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("OrderNo", searchModel.OrderNo);
            p.Add("CompanyId", searchModel.CompanyId);
            p.Add("UserId", searchModel.UserId);
            p.Add("OrderTypeId", searchModel.OrderTypeId);
            p.Add("FlowId", searchModel.FlowId);
            p.Add("CurrentStageId", searchModel.CurrentStageId);
            p.Add("DeliveryChargeRuleId", searchModel.DeliveryChargeRuleId);
            p.Add("PaymentStatus", searchModel.PaymentStatus);
            p.Add("DeliveryStatus", searchModel.DeliveryStatus);
            p.Add("OrderStatus", searchModel.OrderStatus);
            p.Add("IsActive", searchModel.IsActive);
            var output = await _dataAccessHelper.QueryData<SalesOrderResponseDto, dynamic>("USP_SalesOrder_GetDistinct", p);
            return output;
        }

        public async Task<SalesOrderResponseDto> GetSalesOrderById(int SalesOrderId)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", SalesOrderId);
            return (await _dataAccessHelper.QueryData<SalesOrderResponseDto, dynamic>("USP_SalesOrder_GetById", p)).FirstOrDefault();
        }


        public async Task<int> InsertSalesOrder(SalesOrderRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
            p.Add("OrderNo", insertRequestModel.OrderNo);
            p.Add("CompanyId", insertRequestModel.CompanyId);
            p.Add("AddressId", insertRequestModel.AddressId);
            p.Add("OrderTypeId", insertRequestModel.OrderTypeId);
            p.Add("FlowId", insertRequestModel.FlowId);
            p.Add("CurrentStageId", insertRequestModel.CurrentStageId);
            p.Add("TotalAmount", insertRequestModel.TotalAmount);
            p.Add("TotalDiscount", insertRequestModel.TotalDiscount);
            p.Add("DeliveryCharge", insertRequestModel.DeliveryCharge);
            p.Add("ExtraDeliveryCharge", insertRequestModel.ExtraDeliveryCharge);
            p.Add("DiscountOnDelivery", insertRequestModel.DiscountOnDelivery);
            p.Add("FinalDeliveryCharge", insertRequestModel.FinalDeliveryCharge);
            p.Add("NetAmount", insertRequestModel.NetAmount);
            p.Add("DeliveryChargeRuleId", insertRequestModel.DeliveryChargeRuleId);
            p.Add("PaymentStatus", insertRequestModel.PaymentStatus);
            p.Add("IsActive", insertRequestModel.IsActive);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("UserId", insertRequestModel.UserId);

            await _dataAccessHelper.ExecuteData("USP_SalesOrder_Insert", p);
            return p.Get<int>("Id");
        }

        public async Task<int> UpdateSalesOrder(int SalesOrderId, SalesOrderRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", SalesOrderId);
            p.Add("OrderNo", insertRequestModel.OrderNo);
            p.Add("CompanyId", insertRequestModel.CompanyId);
            p.Add("AddressId", insertRequestModel.AddressId);
            p.Add("OrderTypeId", insertRequestModel.OrderTypeId);
            p.Add("FlowId", insertRequestModel.FlowId);
            p.Add("CurrentStageId", insertRequestModel.CurrentStageId);
            p.Add("TotalAmount", insertRequestModel.TotalAmount);
            p.Add("TotalDiscount", insertRequestModel.TotalDiscount);
            p.Add("DeliveryCharge", insertRequestModel.DeliveryCharge);
            p.Add("ExtraDeliveryCharge", insertRequestModel.ExtraDeliveryCharge);
            p.Add("DiscountOnDelivery", insertRequestModel.DiscountOnDelivery);
            p.Add("FinalDeliveryCharge", insertRequestModel.FinalDeliveryCharge);
            p.Add("NetAmount", insertRequestModel.NetAmount);
            p.Add("DeliveryChargeRuleId", insertRequestModel.DeliveryChargeRuleId);
            p.Add("PaymentStatus", insertRequestModel.PaymentStatus);
            p.Add("IsActive", insertRequestModel.IsActive);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("UserId", insertRequestModel.UserId);

            return await _dataAccessHelper.ExecuteData("USP_SalesOrder_Update", p);
        }

        public async Task<int> DeleteSalesOrder(int SalesOrderId, SalesOrderRequestDto deleteRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", SalesOrderId);
            p.Add("CompanyId", deleteRequestModel.CompanyId);
            return await _dataAccessHelper.ExecuteData("USP_SalesOrder_Delete", p);
        }
    }
}
