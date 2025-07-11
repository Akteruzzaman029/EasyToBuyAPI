using Core.Model;
using Core.ModelDto.PackType;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repository
{
    public class PackTypeRepository : IPackTypeRepository
    {
        private readonly DataAccessHelper _dataAccessHelper;
        private readonly IConfiguration _config;
        public PackTypeRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
        {
            _dataAccessHelper = dataAccessHelper;
            _config = config;
        }

        public async Task<PaginatedListModel<PackTypeResponseDto>> GetPackTypes(int pageNumber, PackTypeFilterDto searchModel)
        {
            PaginatedListModel<PackTypeResponseDto> output = new PaginatedListModel<PackTypeResponseDto>();

            try
            {
                //var startDate = searchModel.StartDate.ToString("yyyy-MM-dd");
                //var endDate = searchModel.EndDate.ToString("yyyy-MM-dd");
                DynamicParameters p = new DynamicParameters();
                p.Add("CompanyId", searchModel.CompanyId);
                p.Add("Name", searchModel.Name);
                p.Add("IsActive", searchModel.IsActive);
                p.Add("PageNumber", pageNumber);
                p.Add("PageSize", Convert.ToInt32(_config["SiteSettings:PageSize"]));
                p.Add("TotalRecords", DbType.Int32, direction: ParameterDirection.Output);

                var result = await _dataAccessHelper.QueryData<PackTypeResponseDto, dynamic>("USP_PackType_GetAll", p);
                int TotalRecords = p.Get<int>("TotalRecords");
                int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

                output = new PaginatedListModel<PackTypeResponseDto>
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


        public async Task<List<PackTypeResponseDto>> GetDistinctPackTypes(PackTypeFilterDto searchModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("CompanyId", searchModel.CompanyId);
            p.Add("Name", searchModel.Name);

            var output = await _dataAccessHelper.QueryData<PackTypeResponseDto, dynamic>("USP_PackType_GetDistinct", p);

            return output;
        }

        public async Task<PackTypeResponseDto> GetPackTypeById(int PackTypeId)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", PackTypeId);
            return (await _dataAccessHelper.QueryData<PackTypeResponseDto, dynamic>("USP_PackType_GetById", p)).FirstOrDefault();
        }

        public async Task<List<PackTypeResponseDto>> GetPackTypesByName(PackTypeRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            var output = await _dataAccessHelper.QueryData<PackTypeResponseDto, dynamic>("USP_PackType_GetPackTypesByName", p);
            return output;
        }


        public async Task<int> InsertPackType(PackTypeRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
            p.Add("CompanyId", insertRequestModel.CompanyId);
            p.Add("Name", insertRequestModel.Name);
            p.Add("ShortName", insertRequestModel.ShortName);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsActive", insertRequestModel.IsActive);

            await _dataAccessHelper.ExecuteData("USP_PackType_Insert", p);
            return p.Get<int>("Id");
        }

        public async Task<int> UpdatePackType(int PackTypeId, PackTypeRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", PackTypeId);
            p.Add("CompanyId", insertRequestModel.CompanyId);
            p.Add("Name", insertRequestModel.Name);
            p.Add("ShortName", insertRequestModel.ShortName);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsActive", insertRequestModel.IsActive);


            return await _dataAccessHelper.ExecuteData("USP_PackType_Update", p);
        }

        public async Task<int> DeletePackType(int PackTypeId, PackTypeRequestDto deleteRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", PackTypeId);
            p.Add("CompanyId", deleteRequestModel.CompanyId);

            return await _dataAccessHelper.ExecuteData("USP_PackType_Delete", p);
        }
    }
}
