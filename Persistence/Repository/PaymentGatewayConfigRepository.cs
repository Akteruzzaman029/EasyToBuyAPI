using Core.Model;
using Core.ModelDto.PaymentGatewayConfig;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repository
{
    public class PaymentGatewayConfigRepository : IPaymentGatewayConfigRepository
    {
        private readonly DataAccessHelper _dataAccessHelper;
        private readonly IConfiguration _config;
        public PaymentGatewayConfigRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
        {
            _dataAccessHelper = dataAccessHelper;
            _config = config;
        }

        public async Task<PaginatedListModel<PaymentGatewayConfigResponseDto>> GetCategories(int pageNumber, PaymentGatewayConfigFilterDto searchModel)
        {
            PaginatedListModel<PaymentGatewayConfigResponseDto> output = new PaginatedListModel<PaymentGatewayConfigResponseDto>();

            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("Name", searchModel.Name);
                p.Add("CompanyId", searchModel.CompanyId);
                p.Add("PaymentGatewayTypeId", searchModel.PaymentGatewayTypeId);
                p.Add("IsActive", searchModel.IsActive);
                p.Add("PageNumber", pageNumber);
                p.Add("PageSize", Convert.ToInt32(_config["SiteSettings:PageSize"]));
                p.Add("TotalRecords", DbType.Int32, direction: ParameterDirection.Output);

                var result = await _dataAccessHelper.QueryData<PaymentGatewayConfigResponseDto, dynamic>("USP_PaymentGatewayConfig_GetAll", p);
                int TotalRecords = p.Get<int>("TotalRecords");
                int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

                output = new PaginatedListModel<PaymentGatewayConfigResponseDto>
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

        public async Task<List<PaymentGatewayConfigResponseDto>> GetDistinctCategories(PaymentGatewayConfigFilterDto searchModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("CompanyId", searchModel.CompanyId);
            p.Add("PaymentGatewayTypeId", searchModel.PaymentGatewayTypeId);
            var output = await _dataAccessHelper.QueryData<PaymentGatewayConfigResponseDto, dynamic>("USP_PaymentGatewayConfig_GetDistinct", p);
            return output;
        }

        public async Task<PaymentGatewayConfigResponseDto> GetPaymentGatewayConfigById(int PaymentGatewayConfigId)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", PaymentGatewayConfigId);
            return (await _dataAccessHelper.QueryData<PaymentGatewayConfigResponseDto, dynamic>("USP_PaymentGatewayConfig_GetById", p)).FirstOrDefault();
        }

        public async Task<PaymentGatewayConfigResponseDto> GetPaymentGatewayConfigByName(PaymentGatewayConfigRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("CompanyId", insertRequestModel.CompanyId);
            p.Add("PaymentGatewayTypeId", insertRequestModel.PaymentGatewayTypeId);
            p.Add("Name", insertRequestModel.Name);
            p.Add("UserName", insertRequestModel.UserName);
            p.Add("Password", insertRequestModel.Password);
            return (await _dataAccessHelper.QueryData<PaymentGatewayConfigResponseDto, dynamic>("USP_PaymentGatewayConfig_GetById", p)).FirstOrDefault();
        }


        public async Task<int> InsertPaymentGatewayConfig(PaymentGatewayConfigRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
            p.Add("Name ", insertRequestModel.Name);
            p.Add("CompanyId ", insertRequestModel.CompanyId);
            p.Add("PaymentGatewayTypeId", insertRequestModel.PaymentGatewayTypeId);
            p.Add("MerchantId ", insertRequestModel.MerchantId);
            p.Add("StoreId ", insertRequestModel.StoreId);
            p.Add("UserName ", insertRequestModel.UserName);
            p.Add("Password ", insertRequestModel.Password);
            p.Add("HashKey ", insertRequestModel.HashKey);
            p.Add("SuccessUrl ", insertRequestModel.SuccessUrl);
            p.Add("FailUrl ", insertRequestModel.FailUrl);
            p.Add("CancelUrl ", insertRequestModel.CancelUrl);
            p.Add("BaseUrl ", insertRequestModel.BaseUrl);
            p.Add("SiteUrl ", insertRequestModel.SiteUrl);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsActive", insertRequestModel.IsActive);

            await _dataAccessHelper.ExecuteData("USP_PaymentGatewayConfig_Insert", p);
            return p.Get<int>("Id");
        }

        public async Task<int> UpdatePaymentGatewayConfig(int PaymentGatewayConfigId, PaymentGatewayConfigRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", PaymentGatewayConfigId);
            p.Add("Name ", insertRequestModel.Name );
            p.Add("CompanyId ", insertRequestModel.CompanyId );
            p.Add("PaymentGatewayTypeId", insertRequestModel.PaymentGatewayTypeId);
            p.Add("MerchantId ", insertRequestModel.MerchantId );
            p.Add("StoreId ", insertRequestModel.StoreId );
            p.Add("UserName ", insertRequestModel.UserName );
            p.Add("Password ", insertRequestModel.Password );
            p.Add("HashKey ", insertRequestModel.HashKey );
            p.Add("SuccessUrl ", insertRequestModel.SuccessUrl );
            p.Add("FailUrl ", insertRequestModel.FailUrl );
            p.Add("CancelUrl ", insertRequestModel.CancelUrl );
            p.Add("BaseUrl ", insertRequestModel.BaseUrl );
            p.Add("SiteUrl ", insertRequestModel.SiteUrl);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsActive", insertRequestModel.IsActive);



            return await _dataAccessHelper.ExecuteData("USP_PaymentGatewayConfig_Update", p);
        }

        public async Task<int> DeletePaymentGatewayConfig(int PaymentGatewayConfigId, PaymentGatewayConfigRequestDto deleteRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", PaymentGatewayConfigId);
            p.Add("CompanyId", deleteRequestModel.CompanyId);
            p.Add("PaymentGatewayTypeId", deleteRequestModel.PaymentGatewayTypeId);
            return await _dataAccessHelper.ExecuteData("USP_PaymentGatewayConfig_Delete", p);
        }
    }
}
