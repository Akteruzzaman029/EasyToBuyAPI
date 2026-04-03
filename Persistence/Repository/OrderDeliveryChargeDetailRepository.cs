using Core.Model;
using Core.ModelDto.OrderDeliveryChargeDetail;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repository
{
    public class OrderDeliveryChargeDetailRepository : IOrderDeliveryChargeDetailRepository
    {
        private readonly DataAccessHelper _dataAccessHelper;
        private readonly IConfiguration _config;
        public OrderDeliveryChargeDetailRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
        {
            _dataAccessHelper = dataAccessHelper;
            _config = config;
        }

        public async Task<PaginatedListModel<OrderDeliveryChargeDetailResponseDto>> GetOrderDeliveryChargeDetails(int pageNumber, OrderDeliveryChargeDetailFilterDto searchModel)
        {
            PaginatedListModel<OrderDeliveryChargeDetailResponseDto> output = new PaginatedListModel<OrderDeliveryChargeDetailResponseDto>();

            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("CompanyId", searchModel.CompanyId);
                p.Add("SalesOrderId", searchModel.SalesOrderId);
                p.Add("DeliveryChargeRuleId", searchModel.DeliveryChargeRuleId);
                p.Add("PageNumber", pageNumber);
                p.Add("PageSize", Convert.ToInt32(_config["SiteSettings:PageSize"]));
                p.Add("TotalRecords", DbType.Int32, direction: ParameterDirection.Output);

                var result = await _dataAccessHelper.QueryData<OrderDeliveryChargeDetailResponseDto, dynamic>("USP_OrderDeliveryChargeDetail_GetAll", p);
                int TotalRecords = p.Get<int>("TotalRecords");
                int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

                output = new PaginatedListModel<OrderDeliveryChargeDetailResponseDto>
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

        public async Task<List<OrderDeliveryChargeDetailResponseDto>> GetDistinctOrderDeliveryChargeDetails(OrderDeliveryChargeDetailFilterDto searchModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("CompanyId", searchModel.CompanyId);
            p.Add("SalesOrderId", searchModel.SalesOrderId);
            p.Add("DeliveryChargeRuleId", searchModel.DeliveryChargeRuleId);
            var output = await _dataAccessHelper.QueryData<OrderDeliveryChargeDetailResponseDto, dynamic>("USP_OrderDeliveryChargeDetail_GetDistinct", p);
            return output;
        }

        public async Task<OrderDeliveryChargeDetailResponseDto> GetOrderDeliveryChargeDetailById(int OrderDeliveryChargeDetailId)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", OrderDeliveryChargeDetailId);
            return (await _dataAccessHelper.QueryData<OrderDeliveryChargeDetailResponseDto, dynamic>("USP_OrderDeliveryChargeDetail_GetById", p)).FirstOrDefault();
        }


        public async Task<int> InsertOrderDeliveryChargeDetail(OrderDeliveryChargeDetailRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
            p.Add("CompanyId", insertRequestModel.CompanyId);
            p.Add("SalesOrderId", insertRequestModel.SalesOrderId);
            p.Add("DeliveryChargeRuleId", insertRequestModel.DeliveryChargeRuleId);
            p.Add("BaseCharge", insertRequestModel.BaseCharge);
            p.Add("DistanceCharge", insertRequestModel.DistanceCharge);
            p.Add("WeightCharge", insertRequestModel.WeightCharge);
            p.Add("AreaCharge", insertRequestModel.AreaCharge);
            p.Add("ExpressCharge", insertRequestModel.ExpressCharge);
            p.Add("RemoteAreaCharge", insertRequestModel.RemoteAreaCharge);
            p.Add("DiscountAmount", insertRequestModel.DiscountAmount);
            p.Add("FinalCharge", insertRequestModel.FinalCharge);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("UserId", insertRequestModel.UserId);

            await _dataAccessHelper.ExecuteData("USP_OrderDeliveryChargeDetail_Insert", p);
            return p.Get<int>("Id");
        }

        public async Task<int> UpdateOrderDeliveryChargeDetail(int OrderDeliveryChargeDetailId, OrderDeliveryChargeDetailRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", OrderDeliveryChargeDetailId);
            p.Add("CompanyId", insertRequestModel.CompanyId);
            p.Add("SalesOrderId", insertRequestModel.SalesOrderId);
            p.Add("DeliveryChargeRuleId", insertRequestModel.DeliveryChargeRuleId);
            p.Add("BaseCharge", insertRequestModel.BaseCharge);
            p.Add("DistanceCharge", insertRequestModel.DistanceCharge);
            p.Add("WeightCharge", insertRequestModel.WeightCharge);
            p.Add("AreaCharge", insertRequestModel.AreaCharge);
            p.Add("ExpressCharge", insertRequestModel.ExpressCharge);
            p.Add("RemoteAreaCharge", insertRequestModel.RemoteAreaCharge);
            p.Add("DiscountAmount", insertRequestModel.DiscountAmount);
            p.Add("FinalCharge", insertRequestModel.FinalCharge);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("UserId", insertRequestModel.UserId);

            return await _dataAccessHelper.ExecuteData("USP_OrderDeliveryChargeDetail_Update", p);
        }

        public async Task<int> DeleteOrderDeliveryChargeDetail(int OrderDeliveryChargeDetailId, OrderDeliveryChargeDetailRequestDto deleteRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", OrderDeliveryChargeDetailId);
            p.Add("SalesOrderId", deleteRequestModel.SalesOrderId);
            return await _dataAccessHelper.ExecuteData("USP_OrderDeliveryChargeDetail_Delete", p);
        }
    }
}
