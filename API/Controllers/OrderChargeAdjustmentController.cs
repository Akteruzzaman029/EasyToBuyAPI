using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.OrderChargeAdjustment;
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
    public class OrderChargeAdjustmentController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<OrderChargeAdjustmentController> _logger;
        private readonly IOrderChargeAdjustmentRepository _OrderChargeAdjustmentRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private ResponseDto _responseDto = new ResponseDto();

        public OrderChargeAdjustmentController(SecurityHelper securityHelper,
            ILogger<OrderChargeAdjustmentController> logger,
            IOrderChargeAdjustmentRepository OrderChargeAdjustmentRepository,
            UserManager<ApplicationUser> userManager
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._OrderChargeAdjustmentRepository = OrderChargeAdjustmentRepository;
            this._userManager = userManager;
        }

        [HttpPost("GetOrderChargeAdjustment")]
        public async Task<IActionResult> GetOrderChargeAdjustment(int pageNumber, [FromBody] OrderChargeAdjustmentFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("OrderChargeAdjustment_InvalidPageNumber : "+ pageNumber);

            var result = await _OrderChargeAdjustmentRepository.GetOrderChargeAdjustments(pageNumber, searchModel);
            if (result == null)
                return NotFound("OrderChargeAdjustment_NotFoundList");

            return Ok(result);
        }

        [HttpPost("GetAllOrderChargeAdjustments")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDistinctOrderChargeAdjustment([FromBody] OrderChargeAdjustmentFilterDto searchModel)
        {
            var result = await _OrderChargeAdjustmentRepository.GetDistinctOrderChargeAdjustments(searchModel);
            if (result == null)
                return NotFound("OrderChargeAdjustment_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetOrderChargeAdjustmentById/{id}")]
        public async Task<IActionResult> GetOrderChargeAdjustmentById(int id)
        {

            if (id==0)
                return BadRequest("OrderChargeAdjustment_InvalidId : "+ id);

            var result = await _OrderChargeAdjustmentRepository.GetOrderChargeAdjustmentById(id);
            if (result == null)
                return NotFound("OrderChargeAdjustment_NotFoundId : "+ id);

            return Ok(result);
        }


        [HttpPost("InsertOrderChargeAdjustment")]
        public async Task<IActionResult> InsertOrderChargeAdjustment([FromBody] OrderChargeAdjustmentRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("OrderChargeAdjustment_Null");

                int insertedOrderChargeAdjustmentId = await _OrderChargeAdjustmentRepository.InsertOrderChargeAdjustment(requestModel);
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



        [HttpPost("UpdateOrderChargeAdjustment/{OrderChargeAdjustmentId}")]
        public async Task<IActionResult> UpdateOrderChargeAdjustment(int OrderChargeAdjustmentId, [FromBody] OrderChargeAdjustmentRequestDto updateRequestModel)
        {

            OrderChargeAdjustmentRequestDto requestModel = updateRequestModel;
            if (requestModel == null)
                return BadRequest("OrderChargeAdjustment_Null");

            var requestProcessConfirm = await _OrderChargeAdjustmentRepository.GetOrderChargeAdjustmentById(OrderChargeAdjustmentId);
            if (requestProcessConfirm == null)
                return NotFound("OrderChargeAdjustment_NotFoundId : "+ OrderChargeAdjustmentId);

            int insertedOrderChargeAdjustment = await _OrderChargeAdjustmentRepository.UpdateOrderChargeAdjustment(OrderChargeAdjustmentId, requestModel);

            if (insertedOrderChargeAdjustment <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeleteOrderChargeAdjustment/{OrderChargeAdjustmentId}")]
        public async Task<IActionResult> DeleteOrderChargeAdjustment(int OrderChargeAdjustmentId, [FromBody] OrderChargeAdjustmentRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion

            OrderChargeAdjustmentRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("OrderChargeAdjustment_Null");

            var requestProcessConfirm = await _OrderChargeAdjustmentRepository.GetOrderChargeAdjustmentById(OrderChargeAdjustmentId);
            if (requestProcessConfirm == null)
                return NotFound("OrderChargeAdjustment_NotFoundId : "+ OrderChargeAdjustmentId);

            int insertedOrderChargeAdjustment = await _OrderChargeAdjustmentRepository.DeleteOrderChargeAdjustment(OrderChargeAdjustmentId, requestModel);

            if (insertedOrderChargeAdjustment <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }
    }
}
