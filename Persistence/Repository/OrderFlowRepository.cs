using Core.Model;
using Core.ModelDto.OrderFlow;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repository
{
    public class OrderFlowRepository : IOrderFlowRepository
    {
        private readonly DataAccessHelper _dataAccessHelper;
        private readonly IConfiguration _config;
        public OrderFlowRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
        {
            _dataAccessHelper = dataAccessHelper;
            _config = config;
        }

        public async Task<PaginatedListModel<OrderFlowResponseDto>> GetOrderFlows(int pageNumber, OrderFlowFilterDto searchModel)
        {
            PaginatedListModel<OrderFlowResponseDto> output = new PaginatedListModel<OrderFlowResponseDto>();

            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("Name", searchModel.Name);
                p.Add("OderTypeId", searchModel.OderTypeId);
                p.Add("CompanyId", searchModel.CompanyId);
                p.Add("IsActive", searchModel.IsActive);
                p.Add("PageNumber", pageNumber);
                p.Add("PageSize", Convert.ToInt32(_config["SiteSettings:PageSize"]));
                p.Add("TotalRecords", DbType.Int32, direction: ParameterDirection.Output);

                var result = await _dataAccessHelper.QueryData<OrderFlowResponseDto, dynamic>("USP_OrderFlow_GetAll", p);
                int TotalRecords = p.Get<int>("TotalRecords");
                int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

                output = new PaginatedListModel<OrderFlowResponseDto>
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

        public async Task<List<OrderFlowResponseDto>> GetDistinctOrderFlows(OrderFlowFilterDto searchModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Name", searchModel.Name);
            p.Add("OderTypeId", searchModel.OderTypeId);
            p.Add("CompanyId", searchModel.CompanyId);
            p.Add("IsActive", searchModel.IsActive);
            var output = await _dataAccessHelper.QueryData<OrderFlowResponseDto, dynamic>("USP_OrderFlow_GetDistinct", p);
            return output;
        }

        public async Task<OrderFlowResponseDto> GetOrderFlowById(int OrderFlowId)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", OrderFlowId);
            return (await _dataAccessHelper.QueryData<OrderFlowResponseDto, dynamic>("USP_OrderFlow_GetById", p)).FirstOrDefault();
        }


        public async Task<int> InsertOrderFlow(OrderFlowRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
            p.Add("CompanyId", insertRequestModel.CompanyId);
            p.Add("OderTypeId", insertRequestModel.OderTypeId);
            p.Add("Name", insertRequestModel.Name);
            p.Add("IsDefault", insertRequestModel.IsDefault);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsActive", insertRequestModel.IsActive);
            p.Add("UserId", insertRequestModel.UserId);

            await _dataAccessHelper.ExecuteData("USP_OrderFlow_Insert", p);
            return p.Get<int>("Id");
        }

        public async Task<int> UpdateOrderFlow(int OrderFlowId, OrderFlowRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", OrderFlowId);
            p.Add("CompanyId", insertRequestModel.CompanyId);
            p.Add("OderTypeId", insertRequestModel.OderTypeId);
            p.Add("Name", insertRequestModel.Name);
            p.Add("IsDefault", insertRequestModel.IsDefault);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsActive", insertRequestModel.IsActive);
            p.Add("UserId", insertRequestModel.UserId);

            return await _dataAccessHelper.ExecuteData("USP_OrderFlow_Update", p);
        }

        public async Task<int> DeleteOrderFlow(int OrderFlowId, OrderFlowRequestDto deleteRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", OrderFlowId);
            p.Add("OderTypeId", deleteRequestModel.OderTypeId);
            return await _dataAccessHelper.ExecuteData("USP_OrderFlow_Delete", p);
        }
    }
}
