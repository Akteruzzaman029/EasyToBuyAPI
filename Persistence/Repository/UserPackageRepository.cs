using Core.Model;
using Core.ModelDto.UserPackage;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repository;

public class UserPackageRepository : IUserPackageRepository
{
    private readonly DataAccessHelper _dataAccessHelper;
    private readonly IConfiguration _config;
    public UserPackageRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
    {
        _dataAccessHelper = dataAccessHelper;
        _config = config;
    }

    public async Task<PaginatedListModel<UserPackageResponseDto>> GetUserPackages(int pageNumber, UserPackageFilterDto searchModel)
    {
        PaginatedListModel<UserPackageResponseDto> output = new PaginatedListModel<UserPackageResponseDto>();

        try
        {
            var startDate = searchModel.StartDate.ToString("yyyy-MM-dd");
            var endDate = searchModel.EndDate.ToString("yyyy-MM-dd");
            DynamicParameters p = new DynamicParameters();
            p.Add("StartDate", startDate);
            p.Add("EndDate", endDate);
            p.Add("Name", searchModel.Name);
            p.Add("PackageId", searchModel.PackageId);
            p.Add("PaymentStatus", searchModel.PaymentStatus);
            p.Add("IsActive", searchModel.IsActive);
            p.Add("PageNumber", pageNumber);
            p.Add("PageSize", Convert.ToInt32(_config["SiteSettings:PageSize"]));
            p.Add("TotalRecords", DbType.Int32, direction: ParameterDirection.Output);

            var result = await _dataAccessHelper.QueryData<UserPackageResponseDto, dynamic>("USP_UserPackage_GetAll", p);
            int TotalRecords = p.Get<int>("TotalRecords");
            int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

            output = new PaginatedListModel<UserPackageResponseDto>
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
     public async Task<List<dynamic>> GetUserPackagesDueList(UserPackageFilterDto searchModel)
    {
        List<dynamic> output = new List<dynamic>();

        try
        {
            var startDate = searchModel.StartDate.ToString("yyyy-MM-dd");
            var endDate = searchModel.EndDate.ToString("yyyy-MM-dd");
            DynamicParameters p = new DynamicParameters();
            p.Add("StartDate", startDate);
            p.Add("EndDate", endDate);
            p.Add("Name", searchModel.Name);
            p.Add("IdNo", searchModel.IdNo);
            output = await _dataAccessHelper.QueryData<dynamic, dynamic>("USP_UserPackage_DueList", p);

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        return output;
    }


    public async Task<List<dynamic>> GetProfitAndLoss(UserPackageFilterDto searchModel)
    {
        var startDate = searchModel.StartDate.ToString("yyyy-MM-dd");
        var endDate = searchModel.EndDate.ToString("yyyy-MM-dd");
        DynamicParameters p = new DynamicParameters();
        p.Add("StartDate", startDate);
        p.Add("EndDate", endDate);
        var output = await _dataAccessHelper.QueryData<dynamic, dynamic>("USP_ProfitAndLoss_MonthlyReport", p);
        return output;
    }

    public async Task<List<dynamic>> GetProfitAndLossDetail(UserPackageFilterDto searchModel)
    {
        var startDate = searchModel.StartDate.ToString("yyyy-MM-dd");
        var endDate = searchModel.EndDate.ToString("yyyy-MM-dd");
        DynamicParameters p = new DynamicParameters();
        p.Add("StartDate", startDate);
        p.Add("EndDate", endDate);
        var output = await _dataAccessHelper.QueryData<dynamic, dynamic>("USP_ProfitAndLoss_DetailedReport", p);
        return output;
    }
    public async Task<List<UserPackageResponseDto>> GetDistinctUserPackages(int PackageId)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("PackageId", PackageId);

        var output = await _dataAccessHelper.QueryData<UserPackageResponseDto, dynamic>("USP_UserPackage_GetDistinct", p);

        return output;
    }

    public async Task<UserPackageResponseDto> GetUserPackageById(int UserPackageId)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", UserPackageId);
        return (await _dataAccessHelper.QueryData<UserPackageResponseDto, dynamic>("USP_UserPackage_GetById", p)).FirstOrDefault();
    } 
    public async Task<UserPackageResponseDto> GetUserPackageByStudentId(int studentID)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("StudentID", studentID);
        return (await _dataAccessHelper.QueryData<UserPackageResponseDto, dynamic>("USP_UserPackage_GetByStudentId", p)).FirstOrDefault();
    }


    public async Task<int> InsertUserPackage(UserPackageRequestDto insertRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
        p.Add("UserId", insertRequestModel.UserId);
        p.Add("PackageId", insertRequestModel.PackageId);
        p.Add("PaymentStatus", insertRequestModel.PaymentStatus);
        p.Add("PackageStartDate", insertRequestModel.PackageStartDate);
        p.Add("ExpiryDate", insertRequestModel.ExpiryDate);
        p.Add("NoOfLesson", insertRequestModel.NoOfLesson);
        p.Add("LessonRate", insertRequestModel.LessonRate);
        p.Add("Amount", insertRequestModel.Amount);
        p.Add("Discount", insertRequestModel.Discount);
        p.Add("NetAmount", insertRequestModel.NetAmount);
        p.Add("Remarks", insertRequestModel.Remarks);
        p.Add("IsActive", insertRequestModel.IsActive);
        await _dataAccessHelper.ExecuteData("USP_UserPackage_Insert", p);
        return p.Get<int>("Id");
    }

    public async Task<int> UpdateUserPackage(int UserPackageId, UserPackageRequestDto insertRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", UserPackageId);
        p.Add("UserId", insertRequestModel.UserId);
        p.Add("PackageId", insertRequestModel.PackageId);
        p.Add("PaymentStatus", insertRequestModel.PaymentStatus);
        p.Add("PackageStartDate", insertRequestModel.PackageStartDate);
        p.Add("ExpiryDate", insertRequestModel.ExpiryDate);
        p.Add("NoOfLesson", insertRequestModel.NoOfLesson);
        p.Add("LessonRate", insertRequestModel.LessonRate);
        p.Add("Amount", insertRequestModel.Amount);
        p.Add("Discount", insertRequestModel.Discount);
        p.Add("NetAmount", insertRequestModel.NetAmount);
        p.Add("Remarks", insertRequestModel.Remarks);
        p.Add("IsActive", insertRequestModel.IsActive);

        return await _dataAccessHelper.ExecuteData("USP_UserPackage_Update", p);
    }

    public async Task<int> DeleteUserPackage(int UserPackageId, UserPackageRequestDto deleteRequestModel)
    {
        DynamicParameters p = new DynamicParameters();
        p.Add("Id", UserPackageId);

        return await _dataAccessHelper.ExecuteData("USP_UserPackage_Delete", p);
    }
}
