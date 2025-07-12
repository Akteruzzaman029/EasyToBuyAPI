using Core.Model;
using Core.ModelDto.OrderItem;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repository
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly DataAccessHelper _dataAccessHelper;
        private readonly IConfiguration _config;
        public OrderItemRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
        {
            _dataAccessHelper = dataAccessHelper;
            _config = config;
        }

        public async Task<PaginatedListModel<OrderItemResponseDto>> GetOrderItems(int pageNumber, OrderItemFilterDto searchModel)
        {
            PaginatedListModel<OrderItemResponseDto> output = new PaginatedListModel<OrderItemResponseDto>();

            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("Name", searchModel.Name);
                p.Add("OrderId", searchModel.OrderId);
                p.Add("IsActive", searchModel.IsActive);
                p.Add("PageNumber", pageNumber);
                p.Add("PageSize", Convert.ToInt32(_config["SiteSettings:PageSize"]));
                p.Add("TotalRecords", DbType.Int32, direction: ParameterDirection.Output);

                var result = await _dataAccessHelper.QueryData<OrderItemResponseDto, dynamic>("USP_OrderItem_GetAll", p);
                int TotalRecords = p.Get<int>("TotalRecords");
                int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

                output = new PaginatedListModel<OrderItemResponseDto>
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


        public async Task<List<OrderItemResponseDto>> GetDistinctOrderItems(OrderItemFilterDto searchModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Name", searchModel.Name);
            p.Add("OrderId", searchModel.OrderId);

            var output = await _dataAccessHelper.QueryData<OrderItemResponseDto, dynamic>("USP_OrderItem_GetDistinct", p);

            return output;
        }

        public async Task<OrderItemResponseDto> GetOrderItemById(int OrderItemId)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", OrderItemId);
            return (await _dataAccessHelper.QueryData<OrderItemResponseDto, dynamic>("USP_OrderItem_GetById", p)).FirstOrDefault();
        }

        public async Task<List<OrderItemResponseDto>> GetOrderItemsByName(OrderItemRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            var output = await _dataAccessHelper.QueryData<OrderItemResponseDto, dynamic>("USP_OrderItem_GetOrderItemsByName", p);
            return output;
        }


        public async Task<int> InsertOrderItem(OrderItemRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
            p.Add("OrderId", insertRequestModel.OrderId);
            p.Add("ProductId", insertRequestModel.ProductId);
            p.Add("Quantity", insertRequestModel.Quantity);
            p.Add("UnitPrice", insertRequestModel.UnitPrice);
            p.Add("Discount", insertRequestModel.Discount);
            p.Add("IsFixedAmount", insertRequestModel.IsFixedAmount);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsActive", insertRequestModel.IsActive);

            await _dataAccessHelper.ExecuteData("USP_OrderItem_Insert", p);
            return p.Get<int>("Id");
        }

        public async Task<int> UpdateOrderItem(int OrderItemId, OrderItemRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", OrderItemId);
            p.Add("OrderId", insertRequestModel.OrderId);
            p.Add("ProductId", insertRequestModel.ProductId);
            p.Add("Quantity", insertRequestModel.Quantity);
            p.Add("UnitPrice", insertRequestModel.UnitPrice);
            p.Add("Discount", insertRequestModel.Discount);
            p.Add("IsFixedAmount", insertRequestModel.IsFixedAmount);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsActive", insertRequestModel.IsActive);

            return await _dataAccessHelper.ExecuteData("USP_OrderItem_Update", p);
        }

        public async Task<int> DeleteOrderItem(int OrderItemId, OrderItemRequestDto deleteRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", OrderItemId);
            p.Add("OrderId", deleteRequestModel.OrderId);

            return await _dataAccessHelper.ExecuteData("USP_OrderItem_Delete", p);
        }
    }
}
