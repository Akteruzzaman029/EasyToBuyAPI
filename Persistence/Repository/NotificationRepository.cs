using Core.Model;
using Core.ModelDto.Notification;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly DataAccessHelper _dataAccessHelper;
        private readonly IConfiguration _config;
        public NotificationRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
        {
            _dataAccessHelper = dataAccessHelper;
            _config = config;
        }

        public async Task<PaginatedListModel<NotificationResponseDto>> GetNotifications(int pageNumber, NotificationFilterDto searchModel)
        {
            PaginatedListModel<NotificationResponseDto> output = new PaginatedListModel<NotificationResponseDto>();

            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("UserId", searchModel.UserId);
                p.Add("Type", searchModel.Type);
                p.Add("Messaage", searchModel.Messaage);
                p.Add("Seen", searchModel.Seen);
                p.Add("IsActive", searchModel.IsActive);
                p.Add("PageNumber", pageNumber);
                p.Add("PageSize", Convert.ToInt32(_config["SiteSettings:PageSize"]));
                p.Add("TotalRecords", DbType.Int32, direction: ParameterDirection.Output);

                var result = await _dataAccessHelper.QueryData<NotificationResponseDto, dynamic>("USP_Notification_GetAll", p);
                int TotalRecords = p.Get<int>("TotalRecords");
                int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

                output = new PaginatedListModel<NotificationResponseDto>
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

        public async Task<List<NotificationResponseDto>> GetDistinctNotifications(string userId)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("UserId", userId);

            var output = await _dataAccessHelper.QueryData<NotificationResponseDto, dynamic>("USP_Notification_GetDistinct", p);

            return output;
        }

        public async Task<NotificationResponseDto> GetNotificationById(int NotificationId)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", NotificationId);
            return (await _dataAccessHelper.QueryData<NotificationResponseDto, dynamic>("USP_Notification_GetById", p)).FirstOrDefault();
        }


        public async Task<int> InsertNotification(NotificationRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
            p.Add("UserId", insertRequestModel.UserId);
            p.Add("Type", insertRequestModel.Type);
            p.Add("Messaage", insertRequestModel.Messaage);
            p.Add("Seen", insertRequestModel.Seen);
            p.Add("IsActive", insertRequestModel.IsActive);

            await _dataAccessHelper.ExecuteData("USP_Notification_Insert", p);
            return p.Get<int>("Id");
        }

        public async Task<int> UpdateNotification(int NotificationId, NotificationRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", NotificationId);
            p.Add("UserId", insertRequestModel.UserId);
            p.Add("Type", insertRequestModel.Type);
            p.Add("Messaage", insertRequestModel.Messaage);
            p.Add("Seen", insertRequestModel.Seen);
            p.Add("IsActive", insertRequestModel.IsActive);
            p.Add("IsActive", insertRequestModel.IsActive);

            return await _dataAccessHelper.ExecuteData("USP_Notification_Update", p);
        }

        public async Task<int> DeleteNotification(int NotificationId, NotificationRequestDto deleteRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", NotificationId);
            p.Add("UserId", deleteRequestModel.UserId);

            return await _dataAccessHelper.ExecuteData("USP_Notification_Delete", p);
        }
    }
}
