using Core.Model;

namespace Core.ModelDto.Post
{
    public class PostResponseDto : PostModel
    {

        public string CategoryName { get; set; }=string.Empty;
        public string SubCategoryName { get; set; } = string.Empty;
        public List<PostResponseDto> Posts { get; set; }= new List<PostResponseDto>();

    }
}
