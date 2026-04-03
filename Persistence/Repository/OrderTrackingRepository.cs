using Core.Model;
using Core.ModelDto.OrderTracking;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repository
{
    public class OrderTrackingRepository : IOrderTrackingRepository
    {
        private readonly DataAccessHelper _dataAccessHelper;
        private readonly IConfiguration _config;
        public OrderTrackingRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
        {
            _dataAccessHelper = dataAccessHelper;
            _config = config;
        }

        public async Task<PaginatedListModel<OrderTrackingResponseDto>> GetOrderTrackings(int pageNumber, OrderTrackingFilterDto searchModel)
        {
            PaginatedListModel<OrderTrackingResponseDto> output = new PaginatedListModel<OrderTrackingResponseDto>();

            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("SalesOrderId", searchModel.SalesOrderId);
                p.Add("OrderFlowStageId", searchModel.OrderFlowStageId);
                p.Add("TrackingStatus", searchModel.TrackingStatus);
                p.Add("IsActive", searchModel.IsActive);
                p.Add("PageNumber", pageNumber);
                p.Add("PageSize", Convert.ToInt32(_config["SiteSettings:PageSize"]));
                p.Add("TotalRecords", DbType.Int32, direction: ParameterDirection.Output);

                var result = await _dataAccessHelper.QueryData<OrderTrackingResponseDto, dynamic>("USP_OrderTracking_GetAll", p);
                int TotalRecords = p.Get<int>("TotalRecords");
                int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

                output = new PaginatedListModel<OrderTrackingResponseDto>
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

        public async Task<List<OrderTrackingResponseDto>> GetDistinctOrderTrackings(OrderTrackingFilterDto searchModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("SalesOrderId", searchModel.SalesOrderId);
            p.Add("OrderFlowStageId", searchModel.OrderFlowStageId);
            p.Add("TrackingStatus", searchModel.TrackingStatus);
            p.Add("IsActive", searchModel.IsActive);
            var output = await _dataAccessHelper.QueryData<OrderTrackingResponseDto, dynamic>("USP_OrderTracking_GetDistinct", p);
            return output;
        }

        public async Task<OrderTrackingResponseDto> GetOrderTrackingById(int OrderTrackingId)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", OrderTrackingId);
            return (await _dataAccessHelper.QueryData<OrderTrackingResponseDto, dynamic>("USP_OrderTracking_GetById", p)).FirstOrDefault();
        }


        public async Task<int> InsertOrderTracking(OrderTrackingRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
            p.Add("SalesOrderId", insertRequestModel.SalesOrderId);
            p.Add("OrderFlowStageId", insertRequestModel.OrderFlowStageId);
            p.Add("TrackingStatus", insertRequestModel.TrackingStatus);
            p.Add("TrackingMessage", insertRequestModel.TrackingMessage);
            p.Add("LocationName", insertRequestModel.LocationName);
            p.Add("Latitude", insertRequestModel.Latitude);
            p.Add("Longitude", insertRequestModel.Longitude);
            p.Add("IsCustomerVisible", insertRequestModel.IsCustomerVisible);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsActive", insertRequestModel.IsActive);
            p.Add("UserId", insertRequestModel.UserId);

            await _dataAccessHelper.ExecuteData("USP_OrderTracking_Insert", p);
            return p.Get<int>("Id");
        }

        public async Task<int> UpdateOrderTracking(int OrderTrackingId, OrderTrackingRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", OrderTrackingId);
            p.Add("SalesOrderId", insertRequestModel.SalesOrderId);
            p.Add("OrderFlowStageId", insertRequestModel.OrderFlowStageId);
            p.Add("TrackingStatus", insertRequestModel.TrackingStatus);
            p.Add("TrackingMessage", insertRequestModel.TrackingMessage);
            p.Add("LocationName", insertRequestModel.LocationName);
            p.Add("Latitude", insertRequestModel.Latitude);
            p.Add("Longitude", insertRequestModel.Longitude);
            p.Add("IsCustomerVisible", insertRequestModel.IsCustomerVisible);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsActive", insertRequestModel.IsActive);
            p.Add("UserId", insertRequestModel.UserId);

            return await _dataAccessHelper.ExecuteData("USP_OrderTracking_Update", p);
        }

        public async Task<int> DeleteOrderTracking(int OrderTrackingId, OrderTrackingRequestDto deleteRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", OrderTrackingId);
            p.Add("SalesOrderId", deleteRequestModel.SalesOrderId);
            return await _dataAccessHelper.ExecuteData("USP_OrderTracking_Delete", p);
        }
    }
}
