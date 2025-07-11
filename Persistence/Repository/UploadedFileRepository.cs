using Core.Model;
using Core.ModelDto.UploadedFile;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repository
{
    public class UploadedFileRepository : IUploadedFileRepository
    {
        private readonly DataAccessHelper _dataAccessHelper;
        private readonly IConfiguration _config;
        public UploadedFileRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
        {
            _dataAccessHelper = dataAccessHelper;
            _config = config;
        }

        public async Task<PaginatedListModel<UploadedFileResponseDto>> GetUploadedFiles(int pageNumber, UploadedFileFilterDto searchModel)
        {
            PaginatedListModel<UploadedFileResponseDto> output = new PaginatedListModel<UploadedFileResponseDto>();

            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("UserId", searchModel.UserId);
                p.Add("Name", searchModel.Name);
                p.Add("IsActive", searchModel.IsActive);
                p.Add("PageNumber", pageNumber);
                p.Add("PageSize", Convert.ToInt32(_config["SiteSettings:PageSize"]));
                p.Add("TotalRecords", DbType.Int32, direction: ParameterDirection.Output);

                var result = await _dataAccessHelper.QueryData<UploadedFileResponseDto, dynamic>("USP_UploadedFile_GetAll", p);
                int TotalRecords = p.Get<int>("TotalRecords");
                int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

                output = new PaginatedListModel<UploadedFileResponseDto>
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

        public async Task<List<UploadedFileResponseDto>> GetDistinctUploadedFiles(string userId)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("UserId", userId);

            var output = await _dataAccessHelper.QueryData<UploadedFileResponseDto, dynamic>("USP_UploadedFile_GetDistinct", p);

            return output;
        }

        public async Task<UploadedFileResponseDto> GetUploadedFileById(int UploadedFileId)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", UploadedFileId);
            return (await _dataAccessHelper.QueryData<UploadedFileResponseDto, dynamic>("USP_UploadedFile_GetById", p)).FirstOrDefault();
        }


        public async Task<int> InsertUploadedFile(UploadedFileRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
            p.Add("UserId", insertRequestModel.UserId);
            p.Add("FileName", insertRequestModel.FileName);
            p.Add("FileSize", insertRequestModel.FileSize);
            p.Add("ContentType", insertRequestModel.ContentType);
            p.Add("FileData", insertRequestModel.FileData);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsActive", insertRequestModel.IsActive);

            await _dataAccessHelper.ExecuteData("USP_UploadedFile_Insert", p);
            return p.Get<int>("Id");
        }

        public async Task<int> UpdateUploadedFile(int UploadedFileId, UploadedFileRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", UploadedFileId);
            p.Add("UserId", insertRequestModel.UserId);
            p.Add("FileName", insertRequestModel.FileName);
            p.Add("FileSize", insertRequestModel.FileSize);
            p.Add("FileData", insertRequestModel.FileData);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsActive", insertRequestModel.IsActive);

            return await _dataAccessHelper.ExecuteData("USP_UploadedFile_Update", p);
        }

        public async Task<int> DeleteUploadedFile(int UploadedFileId, UploadedFileRequestDto deleteRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", UploadedFileId);
            p.Add("UserId", deleteRequestModel.UserId);

            return await _dataAccessHelper.ExecuteData("USP_UploadedFile_Delete", p);
        }

        public async Task<List<CategoryModel>> Export(UploadedFileFilterDto searchModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("UserId", searchModel.UserId);
            p.Add("Name", searchModel.Name);
            p.Add("IsActive", searchModel.IsActive);
            return await _dataAccessHelper.QueryData<CategoryModel, dynamic>("USP_UploadedFile_Export", p);
        }
    }
}
