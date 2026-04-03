using Core.Model;
using Core.ModelDto.OrderFlowStageTransition;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repository
{
    public class OrderFlowStageTransitionRepository : IOrderFlowStageTransitionRepository
    {
        private readonly DataAccessHelper _dataAccessHelper;
        private readonly IConfiguration _config;
        public OrderFlowStageTransitionRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
        {
            _dataAccessHelper = dataAccessHelper;
            _config = config;
        }

        public async Task<PaginatedListModel<OrderFlowStageTransitionResponseDto>> GetOrderFlowStageTransitions(int pageNumber, OrderFlowStageTransitionFilterDto searchModel)
        {
            PaginatedListModel<OrderFlowStageTransitionResponseDto> output = new PaginatedListModel<OrderFlowStageTransitionResponseDto>();

            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("FlowId", searchModel.FlowId);
                p.Add("FromStageId", searchModel.FromStageId);
                p.Add("ToStageId", searchModel.ToStageId);
                p.Add("IsAllowed", searchModel.IsAllowed);
                p.Add("PageNumber", pageNumber);
                p.Add("PageSize", Convert.ToInt32(_config["SiteSettings:PageSize"]));
                p.Add("TotalRecords", DbType.Int32, direction: ParameterDirection.Output);

                var result = await _dataAccessHelper.QueryData<OrderFlowStageTransitionResponseDto, dynamic>("USP_OrderFlowStageTransition_GetAll", p);
                int TotalRecords = p.Get<int>("TotalRecords");
                int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

                output = new PaginatedListModel<OrderFlowStageTransitionResponseDto>
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

        public async Task<List<OrderFlowStageTransitionResponseDto>> GetDistinctOrderFlowStageTransitions(OrderFlowStageTransitionFilterDto searchModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("FlowId", searchModel.FlowId);
            p.Add("FromStageId", searchModel.FromStageId);
            p.Add("ToStageId", searchModel.ToStageId);
            p.Add("IsAllowed", searchModel.IsAllowed); ;
            var output = await _dataAccessHelper.QueryData<OrderFlowStageTransitionResponseDto, dynamic>("USP_OrderFlowStageTransition_GetDistinct", p);
            return output;
        }

        public async Task<OrderFlowStageTransitionResponseDto> GetOrderFlowStageTransitionById(int OrderFlowStageTransitionId)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", OrderFlowStageTransitionId);
            return (await _dataAccessHelper.QueryData<OrderFlowStageTransitionResponseDto, dynamic>("USP_OrderFlowStageTransition_GetById", p)).FirstOrDefault();
        }


        public async Task<int> InsertOrderFlowStageTransition(OrderFlowStageTransitionRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
            p.Add("FlowId", insertRequestModel.FlowId);
            p.Add("FromStageId", insertRequestModel.FromStageId);
            p.Add("ToStageId", insertRequestModel.ToStageId);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsAllowed", insertRequestModel.IsAllowed);
            p.Add("UserId", insertRequestModel.UserId);

            await _dataAccessHelper.ExecuteData("USP_OrderFlowStageTransition_Insert", p);
            return p.Get<int>("Id");
        }

        public async Task<int> UpdateOrderFlowStageTransition(int OrderFlowStageTransitionId, OrderFlowStageTransitionRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", OrderFlowStageTransitionId);
            p.Add("FlowId", insertRequestModel.FlowId);
            p.Add("FromStageId", insertRequestModel.FromStageId);
            p.Add("ToStageId", insertRequestModel.ToStageId);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsAllowed", insertRequestModel.IsAllowed);
            p.Add("UserId", insertRequestModel.UserId);

            return await _dataAccessHelper.ExecuteData("USP_OrderFlowStageTransition_Update", p);
        }

        public async Task<int> DeleteOrderFlowStageTransition(int OrderFlowStageTransitionId, OrderFlowStageTransitionRequestDto deleteRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", OrderFlowStageTransitionId);
            p.Add("FlowId", deleteRequestModel.FlowId);
            return await _dataAccessHelper.ExecuteData("USP_OrderFlowStageTransition_Delete", p);
        }
    }
}
