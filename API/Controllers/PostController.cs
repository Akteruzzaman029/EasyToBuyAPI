using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.Post;
using Infrastructure.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Persistence.Repository;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<PostController> _logger;
        private readonly IPostRepository _PostRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<NotificationHub> _hubContext;
        private ResponseDto _responseDto = new ResponseDto();

        public PostController(SecurityHelper securityHelper,
            ILogger<PostController> logger,
            IPostRepository PostRepository,
            UserManager<ApplicationUser> userManager,
            IHubContext<NotificationHub> hubContext
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._PostRepository = PostRepository;
            this._userManager = userManager;
            this._hubContext = hubContext;
        }

        [HttpPost("GetPost")]
        public async Task<IActionResult> GetPost(int pageNumber, [FromBody] PostFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("Post_InvalidPageNumber : "+ pageNumber);

            var result = await _PostRepository.GetPosts(pageNumber, searchModel);
            if (result == null)
                return NotFound("Post_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetAllPosts/{userId}")]
        public async Task<IActionResult> GetDistinctPost(string userId)
        {
            var result = await _PostRepository.GetDistinctPosts(userId);
            if (result == null)
                return NotFound("Post_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetPostsByCategory"), AllowAnonymous ]
        public async Task<IActionResult> GetPostsByCategory()
        {
            var result = await _PostRepository.GetPostsByCategory();
            if (result == null)
                return NotFound("Post_NotFoundList");

            var groupedPosts = result
                            .GroupBy(p => p.CategoryId)
                            .Select(g => new PostResponseDto
                            {
                                CategoryId = g.Key,
                                CategoryName = g.First().CategoryName,
                                SubCategoryName = g.First().SubCategoryName,
                                ParentId = g.First().ParentId,
                                Posts = g.ToList()
                            }).ToList();

            return Ok(groupedPosts);
        }

        [HttpGet("GetPostsByParent"), AllowAnonymous]
        public async Task<IActionResult> GetPostsByParent()
        {
            var result = await _PostRepository.GetPostsByParent();

            if (result == null || !result.Any())
                return NotFound("Post_NotFoundList");

            var groupedPosts = result
                .GroupBy(p => p.ParentId)
                .Select(g => new PostResponseDto
                {
                    ParentId = g.Key,
                    CategoryId = g.First().CategoryId,
                    CategoryName = g.First().CategoryName,
                    SubCategoryName = g.First().SubCategoryName,
                    Posts = g.ToList()
                }).ToList();

            return Ok(groupedPosts);
        }


        [HttpGet("GetPostById/{id}")]
        public async Task<IActionResult> GetPostById(int id)
        {

            if (id==0)
                return BadRequest("Post_InvalidId : "+ id);

            var result = await _PostRepository.GetPostById(id);
            if (result == null)
                return NotFound("Post_NotFoundId : "+ id);

            return Ok(result);
        }


        [HttpPost("InsertPost")]
        public async Task<IActionResult> InsertPost([FromBody] PostRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("Post_Null");

                int insertedPostId = await _PostRepository.InsertPost(requestModel);
                _responseDto.StatusCode = (int)StatusCodes.Status200OK;
                _responseDto.Message = "Data Save Successfully";

            }
            catch (Exception ex)
            {
                _responseDto = new ResponseDto();
                _responseDto.Message = ex.InnerException.Message == null ? ex.Message : ex.InnerException.Message;
            }

            return Ok(_responseDto);
        }



        [HttpPost("UpdatePost/{PostId}")]
        public async Task<IActionResult> UpdatePost(int PostId, [FromBody] PostRequestDto updateRequestModel)
        {

            PostRequestDto requestModel = updateRequestModel;
            if (requestModel == null)
                return BadRequest("Post_Null");

            var requestProcessConfirm = await _PostRepository.GetPostById(PostId);
            if (requestProcessConfirm == null)
                return NotFound("Post_NotFoundId : "+ PostId);

            int insertedPost = await _PostRepository.UpdatePost(PostId, requestModel);

            if (insertedPost <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeletePost/{PostId}")]
        public async Task<IActionResult> DeletePost(int PostId, [FromBody] PostRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion

            PostRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("Post_Null");

            var requestProcessConfirm = await _PostRepository.GetPostById(PostId);
            if (requestProcessConfirm == null)
                return NotFound("Post_NotFoundId : "+ PostId);

            int insertedPost = await _PostRepository.DeletePost(PostId, requestModel);

            if (insertedPost <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }
    }
}
