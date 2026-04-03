using Core.Model;
using Core.ModelDto.OrderType;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repository
{
    public class OrderTypeRepository : IOrderTypeRepository
    {
        private readonly DataAccessHelper _dataAccessHelper;
        private readonly IConfiguration _config;
        public OrderTypeRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
        {
            _dataAccessHelper = dataAccessHelper;
            _config = config;
        }

        public async Task<PaginatedListModel<OrderTypeResponseDto>> GetOrderTypes(int pageNumber, OrderTypeFilterDto searchModel)
        {
            PaginatedListModel<OrderTypeResponseDto> output = new PaginatedListModel<OrderTypeResponseDto>();

            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("Name", searchModel.Name);
                p.Add("Code", searchModel.Code);
                p.Add("CompanyId", searchModel.CompanyId);
                p.Add("IsActive", searchModel.IsActive);
                p.Add("PageNumber", pageNumber);
                p.Add("PageSize", Convert.ToInt32(_config["SiteSettings:PageSize"]));
                p.Add("TotalRecords", DbType.Int32, direction: ParameterDirection.Output);

                var result = await _dataAccessHelper.QueryData<OrderTypeResponseDto, dynamic>("USP_OrderType_GetAll", p);
                int TotalRecords = p.Get<int>("TotalRecords");
                int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

                output = new PaginatedListModel<OrderTypeResponseDto>
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

        public async Task<List<OrderTypeResponseDto>> GetDistinctOrderTypes(OrderTypeFilterDto searchModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Name", searchModel.Name);
            p.Add("Code", searchModel.Code);
            p.Add("CompanyId", searchModel.CompanyId);
            var output = await _dataAccessHelper.QueryData<OrderTypeResponseDto, dynamic>("USP_OrderType_GetDistinct", p);
            return output;
        }

        public async Task<OrderTypeResponseDto> GetOrderTypeById(int OrderTypeId)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", OrderTypeId);
            return (await _dataAccessHelper.QueryData<OrderTypeResponseDto, dynamic>("USP_OrderType_GetById", p)).FirstOrDefault();
        }


        public async Task<int> InsertOrderType(OrderTypeRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
            p.Add("Name", insertRequestModel.Name);
            p.Add("Code", insertRequestModel.Code);
            p.Add("CompanyId", insertRequestModel.CompanyId);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsActive", insertRequestModel.IsActive);
            p.Add("UserId", insertRequestModel.UserId);

            await _dataAccessHelper.ExecuteData("USP_OrderType_Insert", p);
            return p.Get<int>("Id");
        }

        public async Task<int> UpdateOrderType(int OrderTypeId, OrderTypeRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", OrderTypeId);
            p.Add("Name", insertRequestModel.Name);
            p.Add("Code", insertRequestModel.Code);
            p.Add("CompanyId", insertRequestModel.CompanyId);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsActive", insertRequestModel.IsActive);
            p.Add("UserId", insertRequestModel.UserId);

            return await _dataAccessHelper.ExecuteData("USP_OrderType_Update", p);
        }

        public async Task<int> DeleteOrderType(int OrderTypeId, OrderTypeRequestDto deleteRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", OrderTypeId);
            p.Add("CompanyId", deleteRequestModel.CompanyId);
            return await _dataAccessHelper.ExecuteData("USP_OrderType_Delete", p);
        }
    }
}
