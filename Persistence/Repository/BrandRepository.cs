using Core.Model;
using Core.ModelDto.Brand;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repository
{
    public class BrandRepository : IBrandRepository
    {
        private readonly DataAccessHelper _dataAccessHelper;
        private readonly IConfiguration _config;
        public BrandRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
        {
            _dataAccessHelper = dataAccessHelper;
            _config = config;
        }

        public async Task<PaginatedListModel<BrandResponseDto>> GetBrands(int pageNumber, BrandFilterDto searchModel)
        {
            PaginatedListModel<BrandResponseDto> output = new PaginatedListModel<BrandResponseDto>();

            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("Name", searchModel.Name);
                p.Add("CompanyId", searchModel.CompanyId);
                p.Add("IsActive", searchModel.IsActive);
                p.Add("PageNumber", pageNumber);
                p.Add("PageSize", Convert.ToInt32(_config["SiteSettings:PageSize"]));
                p.Add("TotalRecords", DbType.Int32, direction: ParameterDirection.Output);

                var result = await _dataAccessHelper.QueryData<BrandResponseDto, dynamic>("USP_Brand_GetAll", p);
                int TotalRecords = p.Get<int>("TotalRecords");
                int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

                output = new PaginatedListModel<BrandResponseDto>
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

        public async Task<List<BrandResponseDto>> GetDistinctBrands(BrandFilterDto searchModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Name", searchModel.Name);
            p.Add("CompanyId", searchModel.CompanyId);
            var output = await _dataAccessHelper.QueryData<BrandResponseDto, dynamic>("USP_Brand_GetDistinct", p);
            return output;
        }

        public async Task<BrandResponseDto> GetBrandById(int brandId)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", brandId);
            return (await _dataAccessHelper.QueryData<BrandResponseDto, dynamic>("USP_Brand_GetById", p)).FirstOrDefault();
        }


        public async Task<int> InsertBrand(BrandRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
            p.Add("Name", insertRequestModel.Name);
            p.Add("Icon", insertRequestModel.Icon);
            p.Add("FileId", insertRequestModel.FileId);
            p.Add("CompanyId", insertRequestModel.CompanyId);
            p.Add("SequenceNo", insertRequestModel.SequenceNo);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsActive", insertRequestModel.IsActive);
            p.Add("UserId", insertRequestModel.UserId);

            await _dataAccessHelper.ExecuteData("USP_Brand_Insert", p);
            return p.Get<int>("Id");
        }

        public async Task<int> UpdateBrand(int brandId, BrandRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", brandId);
            p.Add("Name", insertRequestModel.Name);
            p.Add("Icon", insertRequestModel.Icon);
            p.Add("FileId", insertRequestModel.FileId);
            p.Add("CompanyId", insertRequestModel.CompanyId);
            p.Add("SequenceNo", insertRequestModel.SequenceNo);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsActive", insertRequestModel.IsActive);
            p.Add("UserId", insertRequestModel.UserId);

            return await _dataAccessHelper.ExecuteData("USP_Brand_Update", p);
        }

        public async Task<int> DeleteBrand(int brandId, BrandRequestDto deleteRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", brandId);
            p.Add("CompanyId", deleteRequestModel.CompanyId);
            return await _dataAccessHelper.ExecuteData("USP_Brand_Delete", p);
        }
    }
}
