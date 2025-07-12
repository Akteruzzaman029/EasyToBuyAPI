using Core.Model;
using Core.ModelDto.OrderPayment;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repository
{
    public class OrderPaymentRepository : IOrderPaymentRepository
    {
        private readonly DataAccessHelper _dataAccessHelper;
        private readonly IConfiguration _config;
        public OrderPaymentRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
        {
            _dataAccessHelper = dataAccessHelper;
            _config = config;
        }

        public async Task<PaginatedListModel<OrderPaymentResponseDto>> GetOrderPayments(int pageNumber, OrderPaymentFilterDto searchModel)
        {
            PaginatedListModel<OrderPaymentResponseDto> output = new PaginatedListModel<OrderPaymentResponseDto>();

            try
            {
                var startDate = searchModel.StartDate.ToString("yyyy-MM-dd");
                var endDate = searchModel.EndDate.ToString("yyyy-MM-dd");
                DynamicParameters p = new DynamicParameters();
                p.Add("StartDate", startDate);
                p.Add("EndDate", endDate);
                p.Add("CompanyId", searchModel.CompanyId);
                p.Add("OrderNo", searchModel.OrderNo);
                p.Add("Reference", searchModel.Reference);
                p.Add("PaymentStatus", searchModel.PaymentStatus);
                p.Add("IsActive", searchModel.IsActive);
                p.Add("PageNumber", pageNumber);
                p.Add("PageSize", Convert.ToInt32(_config["SiteSettings:PageSize"]));
                p.Add("TotalRecords", DbType.Int32, direction: ParameterDirection.Output);

                var result = await _dataAccessHelper.QueryData<OrderPaymentResponseDto, dynamic>("USP_OrderPayment_GetAll", p);
                int TotalRecords = p.Get<int>("TotalRecords");
                int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

                output = new PaginatedListModel<OrderPaymentResponseDto>
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


        public async Task<List<OrderPaymentResponseDto>> GetDistinctOrderPayments(OrderPaymentFilterDto searchModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("CompanyId", searchModel.CompanyId);
            p.Add("OrderNo", searchModel.OrderNo);
            p.Add("Reference", searchModel.Reference);

            var output = await _dataAccessHelper.QueryData<OrderPaymentResponseDto, dynamic>("USP_OrderPayment_GetDistinct", p);

            return output;
        }

        public async Task<OrderPaymentResponseDto> GetOrderPaymentById(int OrderPaymentId)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", OrderPaymentId);
            return (await _dataAccessHelper.QueryData<OrderPaymentResponseDto, dynamic>("USP_OrderPayment_GetById", p)).FirstOrDefault();
        }

        public async Task<List<OrderPaymentResponseDto>> GetOrderPaymentsByName(OrderPaymentRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            var output = await _dataAccessHelper.QueryData<OrderPaymentResponseDto, dynamic>("USP_OrderPayment_GetOrderPaymentsByName", p);
            return output;
        }

        public async Task<int> InsertOrderPayment(OrderPaymentRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
            p.Add("CompanyId", insertRequestModel.CompanyId);
            p.Add("OrderId", insertRequestModel.OrderId);
            p.Add("PaymentMethod", insertRequestModel.PaymentMethod);
            p.Add("Reference", insertRequestModel.Reference);
            p.Add("PaymentStatus", insertRequestModel.PaymentStatus);
            p.Add("PaidAmount", insertRequestModel.PaidAmount);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsActive", insertRequestModel.IsActive);

            await _dataAccessHelper.ExecuteData("USP_OrderPayment_Insert", p);
            return p.Get<int>("Id");
        }

        public async Task<int> UpdateOrderPayment(int OrderPaymentId, OrderPaymentRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", OrderPaymentId);
            p.Add("CompanyId", insertRequestModel.CompanyId);
            p.Add("OrderId", insertRequestModel.OrderId);
            p.Add("PaymentMethod", insertRequestModel.PaymentMethod);
            p.Add("Reference", insertRequestModel.Reference);
            p.Add("PaymentStatus", insertRequestModel.PaymentStatus);
            p.Add("PaidAmount", insertRequestModel.PaidAmount);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsActive", insertRequestModel.IsActive);


            return await _dataAccessHelper.ExecuteData("USP_OrderPayment_Update", p);
        }

        public async Task<int> DeleteOrderPayment(int OrderPaymentId, OrderPaymentRequestDto deleteRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", OrderPaymentId);
            p.Add("CompanyId", deleteRequestModel.CompanyId);

            return await _dataAccessHelper.ExecuteData("USP_OrderPayment_Delete", p);
        }
    }
}
