using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.ProductComment;
using Infrastructure.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Persistence.Repository;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCommentController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<ProductCommentController> _logger;
        private readonly IProductCommentRepository _ProductCommentRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<NotificationHub> _hubContext;
        private ResponseDto _responseDto = new ResponseDto();

        public ProductCommentController(SecurityHelper securityHelper,
            ILogger<ProductCommentController> logger,
            IProductCommentRepository ProductCommentRepository,
            UserManager<ApplicationUser> userManager,
            IHubContext<NotificationHub> hubContext
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._ProductCommentRepository = ProductCommentRepository;
            this._userManager = userManager;
            this._hubContext = hubContext;
        }

        [HttpPost("GetProductComment")]
        public async Task<IActionResult> GetProductComment(int pageNumber, [FromBody] ProductCommentFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("ProductComment_InvalidPageNumber : "+ pageNumber);

            var result = await _ProductCommentRepository.GetProductComments(pageNumber, searchModel);
            if (result == null)
                return NotFound("ProductComment_NotFoundList");

            return Ok(result);
        }

        [HttpPost("GetAllProductComments")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDistinctProductComment([FromBody] ProductCommentFilterDto searchModel)
        {
            var result = await _ProductCommentRepository.GetDistinctProductComments(searchModel);
            if (result == null)
                return NotFound("ProductComment_NotFoundList");

            return Ok(result);
        }


        [HttpGet("GetProductCommentById/{id}")]
        public async Task<IActionResult> GetProductCommentById(int id)
        {

            if (id==0)
                return BadRequest("ProductComment_InvalidId : "+ id);

            var result = await _ProductCommentRepository.GetProductCommentById(id);
            if (result == null)
                return NotFound("ProductComment_NotFoundId : "+ id);

            return Ok(result);
        }


        [HttpPost("InsertProductComment")]
        public async Task<IActionResult> InsertProductComment([FromBody] ProductCommentRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("ProductComment_Null");

                int insertedProductCommentId = await _ProductCommentRepository.InsertProductComment(requestModel);
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



        [HttpPost("UpdateProductComment/{ProductCommentId}")]
        public async Task<IActionResult> UpdateProductComment(int ProductCommentId, [FromBody] ProductCommentRequestDto updateRequestModel)
        {

            ProductCommentRequestDto requestModel = updateRequestModel;
            if (requestModel == null)
                return BadRequest("ProductComment_Null");

            var requestProcessConfirm = await _ProductCommentRepository.GetProductCommentById(ProductCommentId);
            if (requestProcessConfirm == null)
                return NotFound("ProductComment_NotFoundId : "+ ProductCommentId);

            int insertedProductComment = await _ProductCommentRepository.UpdateProductComment(ProductCommentId, requestModel);

            if (insertedProductComment <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeleteProductComment/{ProductCommentId}")]
        public async Task<IActionResult> DeleteProductComment(int ProductCommentId, [FromBody] ProductCommentRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion

            ProductCommentRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("ProductComment_Null");

            var requestProcessConfirm = await _ProductCommentRepository.GetProductCommentById(ProductCommentId);
            if (requestProcessConfirm == null)
                return NotFound("ProductComment_NotFoundId : "+ ProductCommentId);

            int insertedProductComment = await _ProductCommentRepository.DeleteProductComment(ProductCommentId, requestModel);

            if (insertedProductComment <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }
    }
}
