using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.Cart;
using Infrastructure.IRepository;
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
    public class CartController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<CartController> _logger;
        private readonly ICartRepository _CartRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<NotificationHub> _hubContext;
        private ResponseDto _responseDto = new ResponseDto();

        public CartController(SecurityHelper securityHelper,
            ILogger<CartController> logger,
            ICartRepository CartRepository,
            UserManager<ApplicationUser> userManager,
            IHubContext<NotificationHub> hubContext
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._CartRepository = CartRepository;
            this._userManager = userManager;
            this._hubContext = hubContext;
        }

        [HttpPost("GetCart")]
        public async Task<IActionResult> GetCart(int pageNumber, [FromBody] CartFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("Cart_InvalidPageNumber : "+ pageNumber);

            var result = await _CartRepository.GetCarts(pageNumber, searchModel);
            if (result == null)
                return NotFound("Cart_NotFoundList");

            return Ok(result);
        }

        [HttpPost("GetAllCarts")]
        public async Task<IActionResult> GetDistinctCart([FromBody] CartFilterDto searchModel)
        {
            var result = await _CartRepository.GetDistinctCarts(searchModel);
            if (result == null)
                return NotFound("Cart_NotFoundList");

            return Ok(result);
        }


        [HttpGet("GetCartById/{id}")]
        public async Task<IActionResult> GetCartById(int id)
        {

            if (id==0)
                return BadRequest("Cart_InvalidId : "+ id);

            var result = await _CartRepository.GetCartById(id);
            if (result == null)
                return NotFound("Cart_NotFoundId : "+ id);

            return Ok(result);
        }


        [HttpPost("InsertCart")]
        public async Task<IActionResult> InsertCart([FromBody] CartRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("Cart_Null");

                int insertedCartId = await _CartRepository.InsertCart(requestModel);
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



        [HttpPost("UpdateCart/{CartId}")]
        public async Task<IActionResult> UpdateCart(int CartId, [FromBody] CartRequestDto updateRequestModel)
        {

            CartRequestDto requestModel = updateRequestModel;
            if (requestModel == null)
                return BadRequest("Cart_Null");

            var requestProcessConfirm = await _CartRepository.GetCartById(CartId);
            if (requestProcessConfirm == null)
                return NotFound("Cart_NotFoundId : "+ CartId);

            int insertedCart = await _CartRepository.UpdateCart(CartId, requestModel);

            if (insertedCart <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeleteCart/{CartId}")]
        public async Task<IActionResult> DeleteCart(int CartId, [FromBody] CartRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion

            CartRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("Cart_Null");

            var requestProcessConfirm = await _CartRepository.GetCartById(CartId);
            if (requestProcessConfirm == null)
                return NotFound("Cart_NotFoundId : "+ CartId);

            int insertedCart = await _CartRepository.DeleteCart(CartId, requestModel);

            if (insertedCart <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }
    }
}
