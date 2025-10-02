using Core.Model;
using Core.ModelDto.PaymentGatewayType;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repository
{
    public class PaymentGatewayTypeRepository : IPaymentGatewayTypeRepository
    {
        private readonly DataAccessHelper _dataAccessHelper;
        private readonly IConfiguration _config;
        public PaymentGatewayTypeRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
        {
            _dataAccessHelper = dataAccessHelper;
            _config = config;
        }

        public async Task<PaginatedListModel<PaymentGatewayTypeResponseDto>> GetCategories(int pageNumber, PaymentGatewayTypeFilterDto searchModel)
        {
            PaginatedListModel<PaymentGatewayTypeResponseDto> output = new PaginatedListModel<PaymentGatewayTypeResponseDto>();

            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("Name", searchModel.Name);
                p.Add("CompanyId", searchModel.CompanyId);
                p.Add("IsActive", searchModel.IsActive);
                p.Add("PageNumber", pageNumber);
                p.Add("PageSize", Convert.ToInt32(_config["SiteSettings:PageSize"]));
                p.Add("TotalRecords", DbType.Int32, direction: ParameterDirection.Output);

                var result = await _dataAccessHelper.QueryData<PaymentGatewayTypeResponseDto, dynamic>("USP_PaymentGatewayType_GetAll", p);
                int TotalRecords = p.Get<int>("TotalRecords");
                int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

                output = new PaginatedListModel<PaymentGatewayTypeResponseDto>
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

        public async Task<List<PaymentGatewayTypeResponseDto>> GetDistinctCategories(PaymentGatewayTypeFilterDto searchModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Name", searchModel.Name);
            p.Add("CompanyId", searchModel.CompanyId);
            var output = await _dataAccessHelper.QueryData<PaymentGatewayTypeResponseDto, dynamic>("USP_PaymentGatewayType_GetDistinct", p);
            return output;
        }

        public async Task<PaymentGatewayTypeResponseDto> GetPaymentGatewayTypeById(int PaymentGatewayTypeId)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", PaymentGatewayTypeId);
            return (await _dataAccessHelper.QueryData<PaymentGatewayTypeResponseDto, dynamic>("USP_PaymentGatewayType_GetById", p)).FirstOrDefault();
        }

        public async Task<PaymentGatewayTypeResponseDto> GetPaymentGatewayTypeByName(PaymentGatewayTypeRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Name", insertRequestModel.Name);
            return (await _dataAccessHelper.QueryData<PaymentGatewayTypeResponseDto, dynamic>("USP_PaymentGatewayType_GetById", p)).FirstOrDefault();
        }


        public async Task<int> InsertPaymentGatewayType(PaymentGatewayTypeRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
            p.Add("Name", insertRequestModel.Name);
            p.Add("CompanyId", insertRequestModel.CompanyId);
            p.Add("SequenceNo", insertRequestModel.SequenceNo);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsActive", insertRequestModel.IsActive);
            p.Add("UserId", insertRequestModel.UserId);

            await _dataAccessHelper.ExecuteData("USP_PaymentGatewayType_Insert", p);
            return p.Get<int>("Id");
        }

        public async Task<int> UpdatePaymentGatewayType(int PaymentGatewayTypeId, PaymentGatewayTypeRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", PaymentGatewayTypeId);
            p.Add("Name", insertRequestModel.Name);
            p.Add("CompanyId", insertRequestModel.CompanyId);
            p.Add("SequenceNo", insertRequestModel.SequenceNo);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsActive", insertRequestModel.IsActive);
            p.Add("UserId", insertRequestModel.UserId);

            return await _dataAccessHelper.ExecuteData("USP_PaymentGatewayType_Update", p);
        }

        public async Task<int> DeletePaymentGatewayType(int PaymentGatewayTypeId, PaymentGatewayTypeRequestDto deleteRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", PaymentGatewayTypeId);
            p.Add("CompanyId", deleteRequestModel.CompanyId);
            return await _dataAccessHelper.ExecuteData("USP_PaymentGatewayType_Delete", p);
        }
    }
}
