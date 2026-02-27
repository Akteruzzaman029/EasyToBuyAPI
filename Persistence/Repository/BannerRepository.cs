using Core.Model;
using Core.ModelDto.Banner;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repository
{
    public class BannerRepository : IBannerRepository
    {
        private readonly DataAccessHelper _dataAccessHelper;
        private readonly IConfiguration _config;
        public BannerRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
        {
            _dataAccessHelper = dataAccessHelper;
            _config = config;
        }

        public async Task<PaginatedListModel<BannerResponseDto>> GetCategories(int pageNumber, BannerFilterDto searchModel)
        {
            PaginatedListModel<BannerResponseDto> output = new PaginatedListModel<BannerResponseDto>();

            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("Name", searchModel.Name);
                p.Add("TypeTag", searchModel.TypeTag);
                p.Add("CompanyId", searchModel.CompanyId);
                p.Add("IsActive", searchModel.IsActive);
                p.Add("PageNumber", pageNumber);
                p.Add("PageSize", Convert.ToInt32(_config["SiteSettings:PageSize"]));
                p.Add("TotalRecords", DbType.Int32, direction: ParameterDirection.Output);

                var result = await _dataAccessHelper.QueryData<BannerResponseDto, dynamic>("USP_Banner_GetAll", p);
                int TotalRecords = p.Get<int>("TotalRecords");
                int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

                output = new PaginatedListModel<BannerResponseDto>
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

        public async Task<List<BannerResponseDto>> GetDistinctCategories(BannerFilterDto searchModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Name", searchModel.Name);
            p.Add("TypeTag", searchModel.TypeTag);
            p.Add("CompanyId", searchModel.CompanyId);
            var output = await _dataAccessHelper.QueryData<BannerResponseDto, dynamic>("USP_Banner_GetDistinct", p);
            return output;
        }

        public async Task<BannerResponseDto> GetBannerById(int BannerId)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", BannerId);
            return (await _dataAccessHelper.QueryData<BannerResponseDto, dynamic>("USP_Banner_GetById", p)).FirstOrDefault();
        }


        public async Task<int> InsertBanner(BannerRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
            p.Add("Name", insertRequestModel.Name);
            p.Add("Title", insertRequestModel.Title);
            p.Add("TypeTag", insertRequestModel.TypeTag);
            p.Add("Description", insertRequestModel.Description);
            p.Add("FileId", insertRequestModel.FileId);
            p.Add("CompanyId", insertRequestModel.CompanyId);
            p.Add("SequenceNo", insertRequestModel.SequenceNo);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsActive", insertRequestModel.IsActive);
            p.Add("UserId", insertRequestModel.UserId);

            await _dataAccessHelper.ExecuteData("USP_Banner_Insert", p);
            return p.Get<int>("Id");
        }

        public async Task<int> UpdateBanner(int BannerId, BannerRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", BannerId);
            p.Add("Name", insertRequestModel.Name);
            p.Add("Title", insertRequestModel.Title);
            p.Add("TypeTag", insertRequestModel.TypeTag);
            p.Add("Description", insertRequestModel.Description);
            p.Add("FileId", insertRequestModel.FileId);
            p.Add("CompanyId", insertRequestModel.CompanyId);
            p.Add("SequenceNo", insertRequestModel.SequenceNo);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsActive", insertRequestModel.IsActive);
            p.Add("UserId", insertRequestModel.UserId);

            return await _dataAccessHelper.ExecuteData("USP_Banner_Update", p);
        }

        public async Task<int> DeleteBanner(int BannerId, BannerRequestDto deleteRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", BannerId);
            p.Add("CompanyId", deleteRequestModel.CompanyId);
            return await _dataAccessHelper.ExecuteData("USP_Banner_Delete", p);
        }
    }
}
