using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.OrderDelivery;
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
    public class OrderDeliveryController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<OrderDeliveryController> _logger;
        private readonly IOrderDeliveryRepository _OrderDeliveryRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private ResponseDto _responseDto = new ResponseDto();

        public OrderDeliveryController(SecurityHelper securityHelper,
            ILogger<OrderDeliveryController> logger,
            IOrderDeliveryRepository OrderDeliveryRepository,
            UserManager<ApplicationUser> userManager
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._OrderDeliveryRepository = OrderDeliveryRepository;
            this._userManager = userManager;
        }

        [HttpPost("GetOrderDelivery")]
        public async Task<IActionResult> GetOrderDelivery(int pageNumber, [FromBody] OrderDeliveryFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("OrderDelivery_InvalidPageNumber : "+ pageNumber);

            var result = await _OrderDeliveryRepository.GetOrderDeliverys(pageNumber, searchModel);
            if (result == null)
                return NotFound("OrderDelivery_NotFoundList");

            return Ok(result);
        }

        [HttpPost("GetAllOrderDeliverys")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDistinctOrderDelivery([FromBody] OrderDeliveryFilterDto searchModel)
        {
            var result = await _OrderDeliveryRepository.GetDistinctOrderDeliverys(searchModel);
            if (result == null)
                return NotFound("OrderDelivery_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetOrderDeliveryById/{id}")]
        public async Task<IActionResult> GetOrderDeliveryById(int id)
        {

            if (id==0)
                return BadRequest("OrderDelivery_InvalidId : "+ id);

            var result = await _OrderDeliveryRepository.GetOrderDeliveryById(id);
            if (result == null)
                return NotFound("OrderDelivery_NotFoundId : "+ id);

            return Ok(result);
        }


        [HttpPost("InsertOrderDelivery")]
        public async Task<IActionResult> InsertOrderDelivery([FromBody] OrderDeliveryRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("OrderDelivery_Null");

                int insertedOrderDeliveryId = await _OrderDeliveryRepository.InsertOrderDelivery(requestModel);
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



        [HttpPost("UpdateOrderDelivery/{OrderDeliveryId}")]
        public async Task<IActionResult> UpdateOrderDelivery(int OrderDeliveryId, [FromBody] OrderDeliveryRequestDto updateRequestModel)
        {

            OrderDeliveryRequestDto requestModel = updateRequestModel;
            if (requestModel == null)
                return BadRequest("OrderDelivery_Null");

            var requestProcessConfirm = await _OrderDeliveryRepository.GetOrderDeliveryById(OrderDeliveryId);
            if (requestProcessConfirm == null)
                return NotFound("OrderDelivery_NotFoundId : "+ OrderDeliveryId);

            int insertedOrderDelivery = await _OrderDeliveryRepository.UpdateOrderDelivery(OrderDeliveryId, requestModel);

            if (insertedOrderDelivery <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeleteOrderDelivery/{OrderDeliveryId}")]
        public async Task<IActionResult> DeleteOrderDelivery(int OrderDeliveryId, [FromBody] OrderDeliveryRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion

            OrderDeliveryRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("OrderDelivery_Null");

            var requestProcessConfirm = await _OrderDeliveryRepository.GetOrderDeliveryById(OrderDeliveryId);
            if (requestProcessConfirm == null)
                return NotFound("OrderDelivery_NotFoundId : "+ OrderDeliveryId);

            int insertedOrderDelivery = await _OrderDeliveryRepository.DeleteOrderDelivery(OrderDeliveryId, requestModel);

            if (insertedOrderDelivery <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }
    }
}
