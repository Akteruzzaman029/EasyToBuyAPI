using Core.Model;
using Core.ModelDto.Post;

namespace Infrastructure.IRepository;

public interface IPostRepository
{
    Task<PaginatedListModel<PostResponseDto>> GetPosts(int pageNumber, PostFilterDto searchModel);
    Task<List<PostResponseDto>> GetDistinctPosts(string userId);
    Task<List<PostResponseDto>> GetPostsByCategory();
    Task<List<PostResponseDto>> GetPostsByParent();
    Task<PostResponseDto> GetPostById(int PostId);
    Task<int> InsertPost(PostRequestDto insertRequestModel);
    Task<int> UpdatePost(int PostId, PostRequestDto updateRequestModel);
    Task<int> DeletePost(int PostId, PostRequestDto deleteRequestModel);
}
