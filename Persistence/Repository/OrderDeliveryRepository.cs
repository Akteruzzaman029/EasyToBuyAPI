using Core.Model;
using Core.ModelDto.OrderDelivery;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repository
{
    public class OrderDeliveryRepository : IOrderDeliveryRepository
    {
        private readonly DataAccessHelper _dataAccessHelper;
        private readonly IConfiguration _config;
        public OrderDeliveryRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
        {
            _dataAccessHelper = dataAccessHelper;
            _config = config;
        }

        public async Task<PaginatedListModel<OrderDeliveryResponseDto>> GetOrderDeliverys(int pageNumber, OrderDeliveryFilterDto searchModel)
        {
            PaginatedListModel<OrderDeliveryResponseDto> output = new PaginatedListModel<OrderDeliveryResponseDto>();

            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("CompanyId", searchModel.CompanyId);
                p.Add("SalesOrderId", searchModel.SalesOrderId);
                p.Add("DeliveryProviderId", searchModel.DeliveryProviderId);
                p.Add("IsActive", searchModel.IsActive);
                p.Add("PageNumber", pageNumber);
                p.Add("PageSize", Convert.ToInt32(_config["SiteSettings:PageSize"]));
                p.Add("TotalRecords", DbType.Int32, direction: ParameterDirection.Output);

                var result = await _dataAccessHelper.QueryData<OrderDeliveryResponseDto, dynamic>("USP_OrderDelivery_GetAll", p);
                int TotalRecords = p.Get<int>("TotalRecords");
                int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

                output = new PaginatedListModel<OrderDeliveryResponseDto>
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

        public async Task<List<OrderDeliveryResponseDto>> GetDistinctOrderDeliverys(OrderDeliveryFilterDto searchModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("CompanyId", searchModel.CompanyId);
            p.Add("SalesOrderId", searchModel.SalesOrderId);
            p.Add("DeliveryProviderId", searchModel.DeliveryProviderId);
            p.Add("IsActive", searchModel.IsActive);
            var output = await _dataAccessHelper.QueryData<OrderDeliveryResponseDto, dynamic>("USP_OrderDelivery_GetDistinct", p);
            return output;
        }

        public async Task<OrderDeliveryResponseDto> GetOrderDeliveryById(int OrderDeliveryId)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", OrderDeliveryId);
            return (await _dataAccessHelper.QueryData<OrderDeliveryResponseDto, dynamic>("USP_OrderDelivery_GetById", p)).FirstOrDefault();
        }


        public async Task<int> InsertOrderDelivery(OrderDeliveryRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
            p.Add("CompanyId",insertRequestModel.CompanyId);
            p.Add("SalesOrderId",insertRequestModel.SalesOrderId);
            p.Add("DeliveryProviderId",insertRequestModel.DeliveryProviderId);
            p.Add("RiderId",insertRequestModel.RiderId);
            p.Add("TrackingNo",insertRequestModel.TrackingNo);
            p.Add("PickupAddress",insertRequestModel.PickupAddress);
            p.Add("DeliveryAddress",insertRequestModel.DeliveryAddress);
            p.Add("PickupLatitude",insertRequestModel.PickupLatitude);
            p.Add("PickupLongitude",insertRequestModel.PickupLongitude);
            p.Add("DeliveryLatitude",insertRequestModel.DeliveryLatitude);
            p.Add("DeliveryLongitude",insertRequestModel.DeliveryLongitude);
            p.Add("DistanceKm",insertRequestModel.DistanceKm);
            p.Add("WeightKg",insertRequestModel.WeightKg);
            p.Add("DeliveryZoneId",insertRequestModel.DeliveryZoneId);
            p.Add("ShipmentStatus",insertRequestModel.ShipmentStatus);
            p.Add("AssignedAt",insertRequestModel.AssignedAt);
            p.Add("PickupAt",insertRequestModel.PickupAt);
            p.Add("DispatchAt",insertRequestModel.DispatchAt);
            p.Add("OutForDeliveryAt",insertRequestModel.OutForDeliveryAt);
            p.Add("DeliveredAt",insertRequestModel.DeliveredAt);
            p.Add("FailedAt",insertRequestModel.FailedAt);
            p.Add("ReturnInitiatedAt",insertRequestModel.ReturnInitiatedAt);
            p.Add("ReceiverName",insertRequestModel.ReceiverName);
            p.Add("ReceiverPhone",insertRequestModel.ReceiverPhone);
            p.Add("FailureReason", insertRequestModel.FailureReason);
            p.Add("IsActive", insertRequestModel.IsActive);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("UserId", insertRequestModel.UserId);

            await _dataAccessHelper.ExecuteData("USP_OrderDelivery_Insert", p);
            return p.Get<int>("Id");
        }

        public async Task<int> UpdateOrderDelivery(int OrderDeliveryId, OrderDeliveryRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", OrderDeliveryId);
            p.Add("CompanyId", insertRequestModel.CompanyId);
            p.Add("SalesOrderId", insertRequestModel.SalesOrderId);
            p.Add("DeliveryProviderId", insertRequestModel.DeliveryProviderId);
            p.Add("RiderId", insertRequestModel.RiderId);
            p.Add("TrackingNo", insertRequestModel.TrackingNo);
            p.Add("PickupAddress", insertRequestModel.PickupAddress);
            p.Add("DeliveryAddress", insertRequestModel.DeliveryAddress);
            p.Add("PickupLatitude", insertRequestModel.PickupLatitude);
            p.Add("PickupLongitude", insertRequestModel.PickupLongitude);
            p.Add("DeliveryLatitude", insertRequestModel.DeliveryLatitude);
            p.Add("DeliveryLongitude", insertRequestModel.DeliveryLongitude);
            p.Add("DistanceKm", insertRequestModel.DistanceKm);
            p.Add("WeightKg", insertRequestModel.WeightKg);
            p.Add("DeliveryZoneId", insertRequestModel.DeliveryZoneId);
            p.Add("ShipmentStatus", insertRequestModel.ShipmentStatus);
            p.Add("AssignedAt", insertRequestModel.AssignedAt);
            p.Add("PickupAt", insertRequestModel.PickupAt);
            p.Add("DispatchAt", insertRequestModel.DispatchAt);
            p.Add("OutForDeliveryAt", insertRequestModel.OutForDeliveryAt);
            p.Add("DeliveredAt", insertRequestModel.DeliveredAt);
            p.Add("FailedAt", insertRequestModel.FailedAt);
            p.Add("ReturnInitiatedAt", insertRequestModel.ReturnInitiatedAt);
            p.Add("ReceiverName", insertRequestModel.ReceiverName);
            p.Add("ReceiverPhone", insertRequestModel.ReceiverPhone);
            p.Add("FailureReason", insertRequestModel.FailureReason);
            p.Add("IsActive", insertRequestModel.IsActive);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("UserId", insertRequestModel.UserId);
            return await _dataAccessHelper.ExecuteData("USP_OrderDelivery_Update", p);
        }

        public async Task<int> DeleteOrderDelivery(int OrderDeliveryId, OrderDeliveryRequestDto deleteRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", OrderDeliveryId);
            p.Add("SalesOrderId", deleteRequestModel.SalesOrderId);
            return await _dataAccessHelper.ExecuteData("USP_OrderDelivery_Delete", p);
        }
    }
}
