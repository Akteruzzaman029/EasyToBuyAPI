using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.OrderDeliveryChargeDetail;
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
    public class OrderDeliveryChargeDetailController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<OrderDeliveryChargeDetailController> _logger;
        private readonly IOrderDeliveryChargeDetailRepository _OrderDeliveryChargeDetailRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private ResponseDto _responseDto = new ResponseDto();

        public OrderDeliveryChargeDetailController(SecurityHelper securityHelper,
            ILogger<OrderDeliveryChargeDetailController> logger,
            IOrderDeliveryChargeDetailRepository OrderDeliveryChargeDetailRepository,
            UserManager<ApplicationUser> userManager
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._OrderDeliveryChargeDetailRepository = OrderDeliveryChargeDetailRepository;
            this._userManager = userManager;
        }

        [HttpPost("GetOrderDeliveryChargeDetail")]
        public async Task<IActionResult> GetOrderDeliveryChargeDetail(int pageNumber, [FromBody] OrderDeliveryChargeDetailFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("OrderDeliveryChargeDetail_InvalidPageNumber : "+ pageNumber);

            var result = await _OrderDeliveryChargeDetailRepository.GetOrderDeliveryChargeDetails(pageNumber, searchModel);
            if (result == null)
                return NotFound("OrderDeliveryChargeDetail_NotFoundList");

            return Ok(result);
        }

        [HttpPost("GetAllOrderDeliveryChargeDetails")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDistinctOrderDeliveryChargeDetail([FromBody] OrderDeliveryChargeDetailFilterDto searchModel)
        {
            var result = await _OrderDeliveryChargeDetailRepository.GetDistinctOrderDeliveryChargeDetails(searchModel);
            if (result == null)
                return NotFound("OrderDeliveryChargeDetail_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetOrderDeliveryChargeDetailById/{id}")]
        public async Task<IActionResult> GetOrderDeliveryChargeDetailById(int id)
        {

            if (id==0)
                return BadRequest("OrderDeliveryChargeDetail_InvalidId : "+ id);

            var result = await _OrderDeliveryChargeDetailRepository.GetOrderDeliveryChargeDetailById(id);
            if (result == null)
                return NotFound("OrderDeliveryChargeDetail_NotFoundId : "+ id);

            return Ok(result);
        }


        [HttpPost("InsertOrderDeliveryChargeDetail")]
        public async Task<IActionResult> InsertOrderDeliveryChargeDetail([FromBody] OrderDeliveryChargeDetailRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("OrderDeliveryChargeDetail_Null");

                int insertedOrderDeliveryChargeDetailId = await _OrderDeliveryChargeDetailRepository.InsertOrderDeliveryChargeDetail(requestModel);
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



        [HttpPost("UpdateOrderDeliveryChargeDetail/{OrderDeliveryChargeDetailId}")]
        public async Task<IActionResult> UpdateOrderDeliveryChargeDetail(int OrderDeliveryChargeDetailId, [FromBody] OrderDeliveryChargeDetailRequestDto updateRequestModel)
        {

            OrderDeliveryChargeDetailRequestDto requestModel = updateRequestModel;
            if (requestModel == null)
                return BadRequest("OrderDeliveryChargeDetail_Null");

            var requestProcessConfirm = await _OrderDeliveryChargeDetailRepository.GetOrderDeliveryChargeDetailById(OrderDeliveryChargeDetailId);
            if (requestProcessConfirm == null)
                return NotFound("OrderDeliveryChargeDetail_NotFoundId : "+ OrderDeliveryChargeDetailId);

            int insertedOrderDeliveryChargeDetail = await _OrderDeliveryChargeDetailRepository.UpdateOrderDeliveryChargeDetail(OrderDeliveryChargeDetailId, requestModel);

            if (insertedOrderDeliveryChargeDetail <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeleteOrderDeliveryChargeDetail/{OrderDeliveryChargeDetailId}")]
        public async Task<IActionResult> DeleteOrderDeliveryChargeDetail(int OrderDeliveryChargeDetailId, [FromBody] OrderDeliveryChargeDetailRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion

            OrderDeliveryChargeDetailRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("OrderDeliveryChargeDetail_Null");

            var requestProcessConfirm = await _OrderDeliveryChargeDetailRepository.GetOrderDeliveryChargeDetailById(OrderDeliveryChargeDetailId);
            if (requestProcessConfirm == null)
                return NotFound("OrderDeliveryChargeDetail_NotFoundId : "+ OrderDeliveryChargeDetailId);

            int insertedOrderDeliveryChargeDetail = await _OrderDeliveryChargeDetailRepository.DeleteOrderDeliveryChargeDetail(OrderDeliveryChargeDetailId, requestModel);

            if (insertedOrderDeliveryChargeDetail <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }
    }
}
