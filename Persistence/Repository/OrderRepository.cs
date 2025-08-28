using Core.Model;
using Core.ModelDto.Order;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DataAccessHelper _dataAccessHelper;
        private readonly IConfiguration _config;
        public OrderRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
        {
            _dataAccessHelper = dataAccessHelper;
            _config = config;
        }

        public async Task<PaginatedListModel<OrderResponseDto>> GetOrders(int pageNumber, OrderFilterDto searchModel)
        {
            PaginatedListModel<OrderResponseDto> output = new PaginatedListModel<OrderResponseDto>();

            try
            {
                var startDate = searchModel.StartDate.ToString("yyyy-MM-dd");
                var endDate = searchModel.EndDate.ToString("yyyy-MM-dd");
                DynamicParameters p = new DynamicParameters();
                p.Add("StartDate", startDate);
                p.Add("EndDate", endDate);
                p.Add("OrderType", searchModel.OrderType);
                p.Add("CompanyId", searchModel.CompanyId);
                p.Add("UserId", searchModel.UserId);
                p.Add("OrderNo", searchModel.OrderNo);
                p.Add("OrderStatus", searchModel.OrderStatus);
                p.Add("IsActive", searchModel.IsActive);
                p.Add("PageNumber", pageNumber);
                p.Add("PageSize", Convert.ToInt32(_config["SiteSettings:PageSize"]));
                p.Add("TotalRecords", DbType.Int32, direction: ParameterDirection.Output);

                var result = await _dataAccessHelper.QueryData<OrderResponseDto, dynamic>("USP_Order_GetAll", p);
                int TotalRecords = p.Get<int>("TotalRecords");
                int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

                output = new PaginatedListModel<OrderResponseDto>
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

        public async Task<List<OrderResponseDto>> GetDistinctOrders(OrderFilterDto searchModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("CompanyId", searchModel.CompanyId);
            p.Add("OrderNo", searchModel.OrderNo);
            p.Add("OrderStatus", searchModel.OrderStatus);

            var output = await _dataAccessHelper.QueryData<OrderResponseDto, dynamic>("USP_Order_GetDistinct", p);

            return output;
        }

        public async Task<OrderResponseDto> GetOrderById(int OrderId)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", OrderId);
            return (await _dataAccessHelper.QueryData<OrderResponseDto, dynamic>("USP_Order_GetById", p)).FirstOrDefault();
        }

        public async Task<List<OrderResponseDto>> GetOrdersByName(OrderRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("CompanyId", insertRequestModel.CompanyId);
            p.Add("OrderNo", insertRequestModel.OrderNo);
            p.Add("OrderStatus", insertRequestModel.OrderStatus);
            var output = await _dataAccessHelper.QueryData<OrderResponseDto, dynamic>("USP_Order_GetOrdersByName", p);
            return output;
        }


        public async Task<int> InsertOrder(OrderRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
            p.Add("AddressId", insertRequestModel.AddressId);
            p.Add("OrderType", insertRequestModel.OrderType);
            p.Add("CompanyId", insertRequestModel.CompanyId);
            p.Add("UserId", insertRequestModel.UserId);
            p.Add("OrderStatus", insertRequestModel.OrderStatus);
            p.Add("TotalAmount", insertRequestModel.TotalAmount);
            p.Add("TotalDiscount", insertRequestModel.TotalDiscount);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsActive", insertRequestModel.IsActive);

            await _dataAccessHelper.ExecuteData("USP_Order_Insert", p);
            return p.Get<int>("Id");
        }

        public async Task<int> UpdateOrder(int OrderId, OrderRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", OrderId);
            p.Add("AddressId", insertRequestModel.AddressId);
            p.Add("OrderType", insertRequestModel.OrderType);
            p.Add("CompanyId", insertRequestModel.CompanyId);
            p.Add("UserId", insertRequestModel.UserId);
            p.Add("OrderNo", insertRequestModel.OrderNo);
            p.Add("OrderStatus", insertRequestModel.OrderStatus);
            p.Add("TotalAmount", insertRequestModel.TotalAmount);
            p.Add("TotalDiscount", insertRequestModel.TotalDiscount);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsActive", insertRequestModel.IsActive);

            return await _dataAccessHelper.ExecuteData("USP_Order_Update", p);
        }

        public async Task<int> DeleteOrder(int OrderId, OrderRequestDto deleteRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", OrderId);
            p.Add("CompanyId", deleteRequestModel.CompanyId);

            return await _dataAccessHelper.ExecuteData("USP_Order_Delete", p);
        }
    }
}
