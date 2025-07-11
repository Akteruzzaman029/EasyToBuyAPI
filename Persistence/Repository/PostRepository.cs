using Core.Model;
using Core.ModelDto.Post;
using Dapper;
using Infrastructure.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly DataAccessHelper _dataAccessHelper;
        private readonly IConfiguration _config;
        public PostRepository(DataAccessHelper dataAccessHelper, IConfiguration config)
        {
            _dataAccessHelper = dataAccessHelper;
            _config = config;
        }

        public async Task<PaginatedListModel<PostResponseDto>> GetPosts(int pageNumber, PostFilterDto searchModel)
        {
            PaginatedListModel<PostResponseDto> output = new PaginatedListModel<PostResponseDto>();

            try
            {
                var startDate = searchModel.StartDate.ToString("yyyy-MM-dd");
                var endDate = searchModel.EndDate.ToString("yyyy-MM-dd");
                DynamicParameters p = new DynamicParameters();
                p.Add("CategoryId", searchModel.CategoryId);
                p.Add("ParentId", searchModel.ParentId);
                p.Add("Name", searchModel.Name);
                p.Add("IsActive", searchModel.IsActive);
                p.Add("PageNumber", pageNumber);
                p.Add("PageSize", Convert.ToInt32(_config["SiteSettings:PageSize"]));
                p.Add("TotalRecords", DbType.Int32, direction: ParameterDirection.Output);

                var result = await _dataAccessHelper.QueryData<PostResponseDto, dynamic>("USP_Post_GetAll", p);
                int TotalRecords = p.Get<int>("TotalRecords");
                int totalPages = (int)Math.Ceiling(TotalRecords / Convert.ToDouble(_config["SiteSettings:PageSize"]));

                output = new PaginatedListModel<PostResponseDto>
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

        public async Task<List<PostResponseDto>> GetPostsByCategory()
        {
            DynamicParameters p = new DynamicParameters();
            var output = await _dataAccessHelper.QueryData<PostResponseDto, dynamic>("USP_Post_GetPostsByCategory", p);
            return output;
        }

        public async Task<List<PostResponseDto>> GetPostsByParent()
        {
            DynamicParameters p = new DynamicParameters();
            var output = await _dataAccessHelper.QueryData<PostResponseDto, dynamic>("USP_Post_GetPostsByParent", p);
            return output;
        }

        public async Task<List<PostResponseDto>> GetDistinctPosts(string userId)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("UserId", userId);

            var output = await _dataAccessHelper.QueryData<PostResponseDto, dynamic>("USP_Post_GetDistinct", p);

            return output;
        }

        public async Task<PostResponseDto> GetPostById(int PostId)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", PostId);
            return (await _dataAccessHelper.QueryData<PostResponseDto, dynamic>("USP_Post_GetById", p)).FirstOrDefault();
        }


        public async Task<int> InsertPost(PostRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", DbType.Int32, direction: ParameterDirection.Output);
            p.Add("CategoryId", insertRequestModel.CategoryId);
            p.Add("ParentId", insertRequestModel.ParentId);
            p.Add("ShortName", insertRequestModel.ShortName);
            p.Add("Name", insertRequestModel.Name);
            p.Add("Content", insertRequestModel.Content);
            p.Add("FileId", insertRequestModel.FileId);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsActive", insertRequestModel.IsActive);

            await _dataAccessHelper.ExecuteData("USP_Post_Insert", p);
            return p.Get<int>("Id");
        }

        public async Task<int> UpdatePost(int PostId, PostRequestDto insertRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", PostId);
            p.Add("CategoryId", insertRequestModel.CategoryId);
            p.Add("ParentId", insertRequestModel.ParentId);
            p.Add("ShortName", insertRequestModel.ShortName);
            p.Add("Name", insertRequestModel.Name);
            p.Add("Content", insertRequestModel.Content);
            p.Add("FileId", insertRequestModel.FileId);
            p.Add("Remarks", insertRequestModel.Remarks);
            p.Add("IsActive", insertRequestModel.IsActive);

            return await _dataAccessHelper.ExecuteData("USP_Post_Update", p);
        }

        public async Task<int> DeletePost(int PostId, PostRequestDto deleteRequestModel)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Id", PostId);
            p.Add("CategoryId", deleteRequestModel.CategoryId);
            p.Add("ParentId", deleteRequestModel.ParentId);

            return await _dataAccessHelper.ExecuteData("USP_Post_Delete", p);
        }
    }
}
