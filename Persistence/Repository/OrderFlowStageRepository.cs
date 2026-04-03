using Core.Model;
using Core.ModelDto.OrderFlowStage;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repository
{
    public class OrderFlowStageRepository : IOrderFlowStageRepository
    {
        private readonly DataAccessHelper _dataAccessHelper;
        private readonly IConfiguration _config;
        public OrderFlowStageRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
        {
            _dataAccessHelper = dataAccessHelper;
            _config = config;
        }

        public async Task<PaginatedListModel<OrderFlowStageResponseDto>> GetOrderFlowStages(int pageNumber, OrderFlowStageFilterDto searchModel)
        {
            PaginatedListModel<OrderFlowStageResponseDto> output = new PaginatedListModel<OrderFlowStageResponseDto>();

            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("Name", searchModel.Name);
                p.Add("Code", searchModel.Code);
                p.Add("SequenceNo", searchModel.SequenceNo);
                p.Add("IsActive", searchModel.IsActive);
                p.Add("PageNumber", pageNumber);
                p.Add("PageSize", Convert.ToInt32(_config["SiteSettings:PageSize"]));
                p.Add("TotalRecords", DbType.Int32, direction: ParameterDirection.Output);

                var result = await _dataAccessHelper.QueryData<OrderFlowStageResponseDto, dynamic>("USP_OrderFlowStage_GetAll", p);
                int TotalRecords = p.Get<int>("TotalRecords");
                int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

                output = new PaginatedListModel<OrderFlowStageResponseDto>
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

        public async Task<List<OrderFlowStageResponseDto>> GetDistinctOrderFlowStages(OrderFlowStageFilterDto searchModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Name", searchModel.Name);
            p.Add("Code", searchModel.Code);
            p.Add("SequenceNo", searchModel.SequenceNo);
            p.Add("IsActive", searchModel.IsActive);
            var output = await _dataAccessHelper.QueryData<OrderFlowStageResponseDto, dynamic>("USP_OrderFlowStage_GetDistinct", p);
            return output;
        }

        public async Task<OrderFlowStageResponseDto> GetOrderFlowStageById(int OrderFlowStageId)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", OrderFlowStageId);
            return (await _dataAccessHelper.QueryData<OrderFlowStageResponseDto, dynamic>("USP_OrderFlowStage_GetById", p)).FirstOrDefault();
        }


        public async Task<int> InsertOrderFlowStage(OrderFlowStageRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
            p.Add("OrderFlowId", insertRequestModel.OrderFlowId);
            p.Add("Name", insertRequestModel.Name);
            p.Add("Code", insertRequestModel.Code);
            p.Add("SequenceNo", insertRequestModel.SequenceNo);
            p.Add("IsInitialStage", insertRequestModel.IsInitialStage);
            p.Add("IsFinalStage", insertRequestModel.IsFinalStage);
            p.Add("CustomerVisible", insertRequestModel.CustomerVisible);
            p.Add("ColorCode", insertRequestModel.ColorCode);
            p.Add("Icon", insertRequestModel.Icon);
            p.Add("UserId", insertRequestModel.UserId);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsActive", insertRequestModel.IsActive);

            await _dataAccessHelper.ExecuteData("USP_OrderFlowStage_Insert", p);
            return p.Get<int>("Id");
        }

        public async Task<int> UpdateOrderFlowStage(int OrderFlowStageId, OrderFlowStageRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", OrderFlowStageId);
            p.Add("OrderFlowId", insertRequestModel.OrderFlowId);
            p.Add("Name", insertRequestModel.Name);
            p.Add("Code", insertRequestModel.Code);
            p.Add("SequenceNo", insertRequestModel.SequenceNo);
            p.Add("IsInitialStage", insertRequestModel.IsInitialStage);
            p.Add("IsFinalStage", insertRequestModel.IsFinalStage);
            p.Add("CustomerVisible", insertRequestModel.CustomerVisible);
            p.Add("ColorCode", insertRequestModel.ColorCode);
            p.Add("Icon", insertRequestModel.Icon);
            p.Add("UserId", insertRequestModel.UserId);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsActive", insertRequestModel.IsActive);

            return await _dataAccessHelper.ExecuteData("USP_OrderFlowStage_Update", p);
        }

        public async Task<int> DeleteOrderFlowStage(int OrderFlowStageId, OrderFlowStageRequestDto deleteRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", OrderFlowStageId);
            p.Add("OrderFlowId", deleteRequestModel.OrderFlowId);
            return await _dataAccessHelper.ExecuteData("USP_OrderFlowStage_Delete", p);
        }
    }
}
