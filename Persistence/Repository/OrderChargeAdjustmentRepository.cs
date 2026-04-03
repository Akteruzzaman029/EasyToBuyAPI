using Core.Model;
using Core.ModelDto.OrderChargeAdjustment;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repository
{
    public class OrderChargeAdjustmentRepository : IOrderChargeAdjustmentRepository
    {
        private readonly DataAccessHelper _dataAccessHelper;
        private readonly IConfiguration _config;
        public OrderChargeAdjustmentRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
        {
            _dataAccessHelper = dataAccessHelper;
            _config = config;
        }

        public async Task<PaginatedListModel<OrderChargeAdjustmentResponseDto>> GetOrderChargeAdjustments(int pageNumber, OrderChargeAdjustmentFilterDto searchModel)
        {
            PaginatedListModel<OrderChargeAdjustmentResponseDto> output = new PaginatedListModel<OrderChargeAdjustmentResponseDto>();

            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("ChargeHead", searchModel.ChargeHead);
                p.Add("SalesOrderId", searchModel.SalesOrderId);
                p.Add("PageNumber", pageNumber);
                p.Add("PageSize", Convert.ToInt32(_config["SiteSettings:PageSize"]));
                p.Add("TotalRecords", DbType.Int32, direction: ParameterDirection.Output);

                var result = await _dataAccessHelper.QueryData<OrderChargeAdjustmentResponseDto, dynamic>("USP_OrderChargeAdjustment_GetAll", p);
                int TotalRecords = p.Get<int>("TotalRecords");
                int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

                output = new PaginatedListModel<OrderChargeAdjustmentResponseDto>
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

        public async Task<List<OrderChargeAdjustmentResponseDto>> GetDistinctOrderChargeAdjustments(OrderChargeAdjustmentFilterDto searchModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("ChargeHead", searchModel.ChargeHead);
            p.Add("SalesOrderId", searchModel.SalesOrderId);
            var output = await _dataAccessHelper.QueryData<OrderChargeAdjustmentResponseDto, dynamic>("USP_OrderChargeAdjustment_GetDistinct", p);
            return output;
        }

        public async Task<OrderChargeAdjustmentResponseDto> GetOrderChargeAdjustmentById(int OrderChargeAdjustmentId)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", OrderChargeAdjustmentId);
            return (await _dataAccessHelper.QueryData<OrderChargeAdjustmentResponseDto, dynamic>("USP_OrderChargeAdjustment_GetById", p)).FirstOrDefault();
        }


        public async Task<int> InsertOrderChargeAdjustment(OrderChargeAdjustmentRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
            p.Add("SalesOrderId", insertRequestModel.SalesOrderId);
            p.Add("ChargeHead", insertRequestModel.ChargeHead);
            p.Add("Amount", insertRequestModel.Amount);
            p.Add("IsAddition", insertRequestModel.IsAddition);
            p.Add("Reason", insertRequestModel.Reason);
            p.Add("UserId", insertRequestModel.UserId);

            await _dataAccessHelper.ExecuteData("USP_OrderChargeAdjustment_Insert", p);
            return p.Get<int>("Id");
        }

        public async Task<int> UpdateOrderChargeAdjustment(int OrderChargeAdjustmentId, OrderChargeAdjustmentRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", OrderChargeAdjustmentId);
            p.Add("SalesOrderId", insertRequestModel.SalesOrderId);
            p.Add("ChargeHead", insertRequestModel.ChargeHead);
            p.Add("Amount", insertRequestModel.Amount);
            p.Add("IsAddition", insertRequestModel.IsAddition);
            p.Add("Reason", insertRequestModel.Reason);
            p.Add("UserId", insertRequestModel.UserId);

            return await _dataAccessHelper.ExecuteData("USP_OrderChargeAdjustment_Update", p);
        }

        public async Task<int> DeleteOrderChargeAdjustment(int OrderChargeAdjustmentId, OrderChargeAdjustmentRequestDto deleteRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", OrderChargeAdjustmentId);
            p.Add("SalesOrderId", deleteRequestModel.SalesOrderId);
            return await _dataAccessHelper.ExecuteData("USP_OrderChargeAdjustment_Delete", p);
        }
    }
}
