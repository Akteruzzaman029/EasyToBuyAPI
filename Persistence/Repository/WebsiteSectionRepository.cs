using Core.Model;
using Core.ModelDto.Product;
using Core.ModelDto.WebsiteSection;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repository
{
    public class WebsiteSectionRepository : IWebsiteSectionRepository
    {
        private readonly DataAccessHelper _dataAccessHelper;
        private readonly IConfiguration _config;
        public WebsiteSectionRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
        {
            _dataAccessHelper = dataAccessHelper;
            _config = config;
        }

        public async Task<PaginatedListModel<WebsiteSectionResponseDto>> GetWebsiteSections(int pageNumber, WebsiteSectionFilterDto searchModel)
        {
            PaginatedListModel<WebsiteSectionResponseDto> output = new();
            try
            {
                DynamicParameters p = new();
                p.Add("Name", searchModel.Name);
                p.Add("HeaderName", searchModel.HeaderName);
                p.Add("IsActive", searchModel.IsActive);
                p.Add("PageNumber", pageNumber);
                p.Add("PageSize", Convert.ToInt32(_config["SiteSettings:PageSize"]));
                p.Add("TotalRecords", DbType.Int32, direction: ParameterDirection.Output);

                var result = await _dataAccessHelper.QueryData<WebsiteSectionResponseDto, dynamic>("USP_WebsiteSection_GetAll", p);
                int totalRecords = p.Get<int>("TotalRecords");
                int totalPages = (int)Math.Ceiling(totalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

                output = new PaginatedListModel<WebsiteSectionResponseDto>
                {
                    PageIndex = pageNumber,
                    TotalRecords = totalRecords,
                    TotalPages = totalPages,
                    HasPreviousPage = pageNumber > 1,
                    HasNextPage = pageNumber < totalPages,
                    Items = result.ToList()
                };
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            return output;
        }

        public async Task<WebsiteSectionResponseDto> GetWebsiteSectionById(int id)
        {
            DynamicParameters p = new();
            p.Add("Id", id);
            return (await _dataAccessHelper.QueryData<WebsiteSectionResponseDto, dynamic>("USP_WebsiteSection_GetById", p)).FirstOrDefault();
        }

        public async Task<int> InsertWebsiteSection(WebsiteSectionRequestDto model)
        {
            DynamicParameters p = new();
            p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
            p.Add("Name", model.Name);
            p.Add("HeaderName", model.HeaderName);
            p.Add("SequenceNo", model.SequenceNo);
            p.Add("FileId", model.FileId);
            p.Add("Remarks", model.Remarks);
            p.Add("IsActive", model.IsActive);
            p.Add("CreatedBy", model.UserId);

            await _dataAccessHelper.ExecuteData("USP_WebsiteSection_Insert", p);
            return p.Get<int>("Id");
        }

        public async Task<int> UpdateWebsiteSection(int id, WebsiteSectionRequestDto model)
        {
            DynamicParameters p = new();
            p.Add("Id", id);
            p.Add("Name", model.Name);
            p.Add("HeaderName", model.HeaderName);
            p.Add("SequenceNo", model.SequenceNo);
            p.Add("FileId", model.FileId);
            p.Add("Remarks", model.Remarks);
            p.Add("IsActive", model.IsActive);
            p.Add("LastModifiedBy", model.UserId);

            return await _dataAccessHelper.ExecuteData("USP_WebsiteSection_Update", p);
        }

        public async Task<int> DeleteWebsiteSection(int id, WebsiteSectionRequestDto model)
        {
            DynamicParameters p = new();
            p.Add("Id", id);
            return await _dataAccessHelper.ExecuteData("USP_WebsiteSection_Delete", p);
        }
    }
}
