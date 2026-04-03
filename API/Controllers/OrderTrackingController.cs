using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.OrderTracking;
using Infrastructure.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Persistence.Repository;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderTrackingController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<OrderTrackingController> _logger;
        private readonly IOrderTrackingRepository _OrderTrackingRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private ResponseDto _responseDto = new ResponseDto();

        public OrderTrackingController(SecurityHelper securityHelper,
            ILogger<OrderTrackingController> logger,
            IOrderTrackingRepository OrderTrackingRepository,
            UserManager<ApplicationUser> userManager
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._OrderTrackingRepository = OrderTrackingRepository;
            this._userManager = userManager;
        }

        [HttpPost("GetOrderTracking")]
        public async Task<IActionResult> GetOrderTracking(int pageNumber, [FromBody] OrderTrackingFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("OrderTracking_InvalidPageNumber : "+ pageNumber);

            var result = await _OrderTrackingRepository.GetOrderTrackings(pageNumber, searchModel);
            if (result == null)
                return NotFound("OrderTracking_NotFoundList");

            return Ok(result);
        }

        [HttpPost("GetAllOrderTrackings")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDistinctOrderTracking([FromBody] OrderTrackingFilterDto searchModel)
        {
            var result = await _OrderTrackingRepository.GetDistinctOrderTrackings(searchModel);
            if (result == null)
                return NotFound("OrderTracking_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetOrderTrackingById/{id}")]
        public async Task<IActionResult> GetOrderTrackingById(int id)
        {

            if (id==0)
                return BadRequest("OrderTracking_InvalidId : "+ id);

            var result = await _OrderTrackingRepository.GetOrderTrackingById(id);
            if (result == null)
                return NotFound("OrderTracking_NotFoundId : "+ id);

            return Ok(result);
        }


        [HttpPost("InsertOrderTracking")]
        public async Task<IActionResult> InsertOrderTracking([FromBody] OrderTrackingRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("OrderTracking_Null");

                int insertedOrderTrackingId = await _OrderTrackingRepository.InsertOrderTracking(requestModel);
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



        [HttpPost("UpdateOrderTracking/{OrderTrackingId}")]
        public async Task<IActionResult> UpdateOrderTracking(int OrderTrackingId, [FromBody] OrderTrackingRequestDto updateRequestModel)
        {

            OrderTrackingRequestDto requestModel = updateRequestModel;
            if (requestModel == null)
                return BadRequest("OrderTracking_Null");

            var requestProcessConfirm = await _OrderTrackingRepository.GetOrderTrackingById(OrderTrackingId);
            if (requestProcessConfirm == null)
                return NotFound("OrderTracking_NotFoundId : "+ OrderTrackingId);

            int insertedOrderTracking = await _OrderTrackingRepository.UpdateOrderTracking(OrderTrackingId, requestModel);

            if (insertedOrderTracking <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeleteOrderTracking/{OrderTrackingId}")]
        public async Task<IActionResult> DeleteOrderTracking(int OrderTrackingId, [FromBody] OrderTrackingRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion

            OrderTrackingRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("OrderTracking_Null");

            var requestProcessConfirm = await _OrderTrackingRepository.GetOrderTrackingById(OrderTrackingId);
            if (requestProcessConfirm == null)
                return NotFound("OrderTracking_NotFoundId : "+ OrderTrackingId);

            int insertedOrderTracking = await _OrderTrackingRepository.DeleteOrderTracking(OrderTrackingId, requestModel);

            if (insertedOrderTracking <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }
    }
}
