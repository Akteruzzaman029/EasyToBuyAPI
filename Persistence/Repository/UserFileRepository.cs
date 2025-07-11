using Core.Model;
using Core.ModelDto.UserFile;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repository
{
    public class UserFileRepository : IUserFileRepository
    {
        private readonly DataAccessHelper _dataAccessHelper;
        private readonly IConfiguration _config;
        public UserFileRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
        {
            _dataAccessHelper = dataAccessHelper;
            _config = config;
        }

        public async Task<PaginatedListModel<UserFileResponseDto>> GetUserFiles(int pageNumber, UserFileFilterDto searchModel)
        {
            PaginatedListModel<UserFileResponseDto> output = new PaginatedListModel<UserFileResponseDto>();

            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("Name", searchModel.Name);
                p.Add("UserId", searchModel.UserId);
                p.Add("IsActive", searchModel.IsActive);
                p.Add("PageNumber", pageNumber);
                p.Add("PageSize", Convert.ToInt32(_config["SiteSettings:PageSize"]));
                p.Add("TotalRecords", DbType.Int32, direction: ParameterDirection.Output);

                var result = await _dataAccessHelper.QueryData<UserFileResponseDto, dynamic>("USP_UserFile_GetAll", p);
                int TotalRecords = p.Get<int>("TotalRecords");
                int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

                output = new PaginatedListModel<UserFileResponseDto>
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

        public async Task<List<UserFileResponseDto>> GetDistinctUserFiles(string UserId)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("UserId", UserId);
            var output = await _dataAccessHelper.QueryData<UserFileResponseDto, dynamic>("USP_UserFile_GetDistinct", p);
            return output;
        }

        public async Task<UserFileResponseDto> GetUserFileById(int userfileId)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", userfileId);
            return (await _dataAccessHelper.QueryData<UserFileResponseDto, dynamic>("USP_UserFile_GetById", p)).FirstOrDefault();
        }


        public async Task<int> InsertUserFile(UserFileRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
            p.Add("Name", insertRequestModel.Name);
            p.Add("UserId", insertRequestModel.UserId);
            p.Add("FileId", insertRequestModel.FileId);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsActive", insertRequestModel.IsActive);

            await _dataAccessHelper.ExecuteData("USP_UserFile_Insert", p);
            return p.Get<int>("Id");
        }

        public async Task<int> UpdateUserFile(int userfileId, UserFileRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", userfileId);
            p.Add("Name", insertRequestModel.Name);
            p.Add("UserId", insertRequestModel.UserId);
            p.Add("FileId", insertRequestModel.FileId);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsActive", insertRequestModel.IsActive);

            return await _dataAccessHelper.ExecuteData("USP_UserFile_Update", p);
        }

        public async Task<int> DeleteUserFile(int userfileId, UserFileRequestDto deleteRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", userfileId);
            p.Add("UserId", deleteRequestModel.UserId);
            return await _dataAccessHelper.ExecuteData("USP_UserFile_Delete", p);
        }
    }
}
