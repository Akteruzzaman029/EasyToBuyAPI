using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.OrderPayment;
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
    public class OrderPaymentController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<OrderPaymentController> _logger;
        private readonly IOrderPaymentRepository _OrderPaymentRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<NotificationHub> _hubContext;
        private ResponseDto _responseDto = new ResponseDto();

        public OrderPaymentController(SecurityHelper securityHelper,
            ILogger<OrderPaymentController> logger,
            IOrderPaymentRepository OrderPaymentRepository,
            UserManager<ApplicationUser> userManager,
            IHubContext<NotificationHub> hubContext
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._OrderPaymentRepository = OrderPaymentRepository;
            this._userManager = userManager;
            this._hubContext = hubContext;
        }

        [HttpPost("GetOrderPayment")]
        public async Task<IActionResult> GetOrderPayment(int pageNumber, [FromBody] OrderPaymentFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("OrderPayment_InvalidPageNumber : "+ pageNumber);

            var result = await _OrderPaymentRepository.GetOrderPayments(pageNumber, searchModel);
            if (result == null)
                return NotFound("OrderPayment_NotFoundList");

            return Ok(result);
        }

        [HttpPost("GetAllOrderPayments")]
        public async Task<IActionResult> GetDistinctOrderPayment([FromBody] OrderPaymentFilterDto searchModel)
        {
            var result = await _OrderPaymentRepository.GetDistinctOrderPayments(searchModel);
            if (result == null)
                return NotFound("OrderPayment_NotFoundList");

            return Ok(result);
        }


        [HttpGet("GetOrderPaymentById/{id}")]
        public async Task<IActionResult> GetOrderPaymentById(int id)
        {

            if (id==0)
                return BadRequest("OrderPayment_InvalidId : "+ id);

            var result = await _OrderPaymentRepository.GetOrderPaymentById(id);
            if (result == null)
                return NotFound("OrderPayment_NotFoundId : "+ id);

            return Ok(result);
        }


        [HttpPost("InsertOrderPayment")]
        public async Task<IActionResult> InsertOrderPayment([FromBody] OrderPaymentRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("OrderPayment_Null");

                int insertedOrderPaymentId = await _OrderPaymentRepository.InsertOrderPayment(requestModel);
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



        [HttpPost("UpdateOrderPayment/{OrderPaymentId}")]
        public async Task<IActionResult> UpdateOrderPayment(int OrderPaymentId, [FromBody] OrderPaymentRequestDto updateRequestModel)
        {

            OrderPaymentRequestDto requestModel = updateRequestModel;
            if (requestModel == null)
                return BadRequest("OrderPayment_Null");

            var requestProcessConfirm = await _OrderPaymentRepository.GetOrderPaymentById(OrderPaymentId);
            if (requestProcessConfirm == null)
                return NotFound("OrderPayment_NotFoundId : "+ OrderPaymentId);

            int insertedOrderPayment = await _OrderPaymentRepository.UpdateOrderPayment(OrderPaymentId, requestModel);

            if (insertedOrderPayment <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeleteOrderPayment/{OrderPaymentId}")]
        public async Task<IActionResult> DeleteOrderPayment(int OrderPaymentId, [FromBody] OrderPaymentRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion

            OrderPaymentRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("OrderPayment_Null");

            var requestProcessConfirm = await _OrderPaymentRepository.GetOrderPaymentById(OrderPaymentId);
            if (requestProcessConfirm == null)
                return NotFound("OrderPayment_NotFoundId : "+ OrderPaymentId);

            int insertedOrderPayment = await _OrderPaymentRepository.DeleteOrderPayment(OrderPaymentId, requestModel);

            if (insertedOrderPayment <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }
    }
}
