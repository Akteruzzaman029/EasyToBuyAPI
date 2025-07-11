using Core.Model;
using Core.ModelDto.Payment;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repository;

public class PaymentRepository : IPaymentRepository
{
    private readonly DataAccessHelper _dataAccessHelper;
    private readonly IConfiguration _config;
    public PaymentRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
    {
        _dataAccessHelper = dataAccessHelper;
        _config = config;
    }

    public async Task<PaginatedListModel<PaymentResponseDto>> GetPayments(int pageNumber, PaymentFilterDto searchModel)
    {
        PaginatedListModel<PaymentResponseDto> output = new PaginatedListModel<PaymentResponseDto>();

        try
        {
            var startDate = searchModel.StartDate.ToString("yyyy-MM-dd");
            var endDate = searchModel.EndDate.ToString("yyyy-MM-dd");
            DynamicParameters p = new DynamicParameters();
            p.Add("StartDate", startDate);
            p.Add("EndDate", endDate);
            p.Add("Name", searchModel.Name);
            p.Add("PackageName", searchModel.PackageName);
            p.Add("Status", searchModel.Status);
            p.Add("IsActive", searchModel.IsActive);
            p.Add("PageNumber", pageNumber);
            p.Add("PageSize", Convert.ToInt32(_config["SiteSettings:PageSize"]));
            p.Add("TotalRecords", DbType.Int32, direction: ParameterDirection.Output);

            var result = await _dataAccessHelper.QueryData<PaymentResponseDto, dynamic>("USP_Payment_GetAll", p);
            int TotalRecords = p.Get<int>("TotalRecords");
            int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

            output = new PaginatedListModel<PaymentResponseDto>
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

    public async Task<List<PaymentResponseDto>> GetDistinctPayments(int postId)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("PostId", postId);

        var output = await _dataAccessHelper.QueryData<PaymentResponseDto, dynamic>("USP_Payment_GetDistinct", p);

        return output;
    }
    
    public async Task<List<PaymentResponseDto>> GetPaymentByStudentId(int StudentID)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("StudentID", StudentID);
        var output = await _dataAccessHelper.QueryData<PaymentResponseDto, dynamic>("USP_Payment_GetByStudentId", p);
        return output;
    }

    public async Task<PaymentResponseDto> GetPaymentById(int PaymentId)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", PaymentId);
        return (await _dataAccessHelper.QueryData<PaymentResponseDto, dynamic>("USP_Payment_GetById", p)).FirstOrDefault();
    }
    
    public async Task<PaymentResponseDto> GetPaymentReceiptById(int PaymentId)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", PaymentId);
        return (await _dataAccessHelper.QueryData<PaymentResponseDto, dynamic>("USP_Payment_Receipt_GetById", p)).FirstOrDefault();
    }



    public async Task<int> InsertPayment(PaymentRequestDto insertRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
        p.Add("UserId", insertRequestModel.UserId);
        p.Add("PackageId", insertRequestModel.PackageId);
        p.Add("Amount", insertRequestModel.Amount);
        p.Add("Status", insertRequestModel.Status);
        p.Add("TransactionDate", insertRequestModel.TransactionDate);
        p.Add("PaymentMethod", insertRequestModel.PaymentMethod);
        p.Add("Remarks", insertRequestModel.Remarks);
        p.Add("IsActive", insertRequestModel.IsActive);
        await _dataAccessHelper.ExecuteData("USP_Payment_Insert", p);
        return p.Get<int>("Id");
    }

    public async Task<int> UpdatePayment(int PaymentId, PaymentRequestDto insertRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", PaymentId);
        p.Add("UserId", insertRequestModel.UserId);
        p.Add("PackageId", insertRequestModel.PackageId);
        p.Add("Amount", insertRequestModel.Amount);
        p.Add("Status", insertRequestModel.Status);
        p.Add("TransactionDate", insertRequestModel.TransactionDate);
        p.Add("PaymentMethod", insertRequestModel.PaymentMethod);
        p.Add("Remarks", insertRequestModel.Remarks);
        p.Add("IsActive", insertRequestModel.IsActive); ;

        return await _dataAccessHelper.ExecuteData("USP_Payment_Update", p);
    }

    public async Task<int> DeletePayment(int PaymentId, PaymentRequestDto deleteRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", PaymentId);

        return await _dataAccessHelper.ExecuteData("USP_Payment_Delete", p);
    }
}
