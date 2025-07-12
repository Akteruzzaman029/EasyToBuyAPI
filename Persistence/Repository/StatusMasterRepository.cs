using Core.Model;
using Core.ModelDto.StatusMaster;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repository
{
    public class StatusMasterRepository : IStatusMasterRepository
    {
        private readonly DataAccessHelper _dataAccessHelper;
        private readonly IConfiguration _config;
        public StatusMasterRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
        {
            _dataAccessHelper = dataAccessHelper;
            _config = config;
        }

        public async Task<PaginatedListModel<StatusMasterResponseDto>> GetStatusMasters(int pageNumber, StatusMasterFilterDto searchModel)
        {
            PaginatedListModel<StatusMasterResponseDto> output = new PaginatedListModel<StatusMasterResponseDto>();

            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("Name", searchModel.Name);
                p.Add("IsActive", searchModel.IsActive);
                p.Add("PageNumber", pageNumber);
                p.Add("PageSize", Convert.ToInt32(_config["SiteSettings:PageSize"]));
                p.Add("TotalRecords", DbType.Int32, direction: ParameterDirection.Output);

                var result = await _dataAccessHelper.QueryData<StatusMasterResponseDto, dynamic>("USP_StatusMaster_GetAll", p);
                int TotalRecords = p.Get<int>("TotalRecords");
                int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

                output = new PaginatedListModel<StatusMasterResponseDto>
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


        public async Task<List<StatusMasterResponseDto>> GetDistinctStatusMasters(StatusMasterFilterDto searchModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Name", searchModel.Name);

            var output = await _dataAccessHelper.QueryData<StatusMasterResponseDto, dynamic>("USP_StatusMaster_GetDistinct", p);

            return output;
        }

        public async Task<StatusMasterResponseDto> GetStatusMasterById(int StatusMasterId)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", StatusMasterId);
            return (await _dataAccessHelper.QueryData<StatusMasterResponseDto, dynamic>("USP_StatusMaster_GetById", p)).FirstOrDefault();
        }

        public async Task<List<StatusMasterResponseDto>> GetStatusMastersByName(StatusMasterRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            var output = await _dataAccessHelper.QueryData<StatusMasterResponseDto, dynamic>("USP_StatusMaster_GetStatusMastersByName", p);
            return output;
        }


        public async Task<int> InsertStatusMaster(StatusMasterRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
            p.Add("StatusGroup", insertRequestModel.StatusGroup);
            p.Add("StatusCode", insertRequestModel.StatusCode);
            p.Add("StatusName", insertRequestModel.StatusName);
            p.Add("Description", insertRequestModel.Description);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsActive", insertRequestModel.IsActive);

            await _dataAccessHelper.ExecuteData("USP_StatusMaster_Insert", p);
            return p.Get<int>("Id");
        }

        public async Task<int> UpdateStatusMaster(int StatusMasterId, StatusMasterRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", StatusMasterId);
            p.Add("StatusGroup", insertRequestModel.StatusGroup);
            p.Add("StatusCode", insertRequestModel.StatusCode);
            p.Add("StatusName", insertRequestModel.StatusName);
            p.Add("Description", insertRequestModel.Description);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsActive", insertRequestModel.IsActive);

            return await _dataAccessHelper.ExecuteData("USP_StatusMaster_Update", p);
        }

        public async Task<int> DeleteStatusMaster(int StatusMasterId, StatusMasterRequestDto deleteRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", StatusMasterId);
            return await _dataAccessHelper.ExecuteData("USP_StatusMaster_Delete", p);
        }
    }
}
