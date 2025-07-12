using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.OrderItem;
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
    public class OrderItemController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<OrderItemController> _logger;
        private readonly IOrderItemRepository _OrderItemRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<NotificationHub> _hubContext;
        private ResponseDto _responseDto = new ResponseDto();

        public OrderItemController(SecurityHelper securityHelper,
            ILogger<OrderItemController> logger,
            IOrderItemRepository OrderItemRepository,
            UserManager<ApplicationUser> userManager,
            IHubContext<NotificationHub> hubContext
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._OrderItemRepository = OrderItemRepository;
            this._userManager = userManager;
            this._hubContext = hubContext;
        }

        [HttpPost("GetOrderItem")]
        public async Task<IActionResult> GetOrderItem(int pageNumber, [FromBody] OrderItemFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("OrderItem_InvalidPageNumber : "+ pageNumber);

            var result = await _OrderItemRepository.GetOrderItems(pageNumber, searchModel);
            if (result == null)
                return NotFound("OrderItem_NotFoundList");

            return Ok(result);
        }

        [HttpPost("GetAllOrderItems")]
        public async Task<IActionResult> GetDistinctOrderItem([FromBody] OrderItemFilterDto searchModel)
        {
            var result = await _OrderItemRepository.GetDistinctOrderItems(searchModel);
            if (result == null)
                return NotFound("OrderItem_NotFoundList");

            return Ok(result);
        }


        [HttpGet("GetOrderItemById/{id}")]
        public async Task<IActionResult> GetOrderItemById(int id)
        {

            if (id==0)
                return BadRequest("OrderItem_InvalidId : "+ id);

            var result = await _OrderItemRepository.GetOrderItemById(id);
            if (result == null)
                return NotFound("OrderItem_NotFoundId : "+ id);

            return Ok(result);
        }


        [HttpPost("InsertOrderItem")]
        public async Task<IActionResult> InsertOrderItem([FromBody] OrderItemRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("OrderItem_Null");

                int insertedOrderItemId = await _OrderItemRepository.InsertOrderItem(requestModel);
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



        [HttpPost("UpdateOrderItem/{OrderItemId}")]
        public async Task<IActionResult> UpdateOrderItem(int OrderItemId, [FromBody] OrderItemRequestDto updateRequestModel)
        {

            OrderItemRequestDto requestModel = updateRequestModel;
            if (requestModel == null)
                return BadRequest("OrderItem_Null");

            var requestProcessConfirm = await _OrderItemRepository.GetOrderItemById(OrderItemId);
            if (requestProcessConfirm == null)
                return NotFound("OrderItem_NotFoundId : "+ OrderItemId);

            int insertedOrderItem = await _OrderItemRepository.UpdateOrderItem(OrderItemId, requestModel);

            if (insertedOrderItem <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeleteOrderItem/{OrderItemId}")]
        public async Task<IActionResult> DeleteOrderItem(int OrderItemId, [FromBody] OrderItemRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion

            OrderItemRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("OrderItem_Null");

            var requestProcessConfirm = await _OrderItemRepository.GetOrderItemById(OrderItemId);
            if (requestProcessConfirm == null)
                return NotFound("OrderItem_NotFoundId : "+ OrderItemId);

            int insertedOrderItem = await _OrderItemRepository.DeleteOrderItem(OrderItemId, requestModel);

            if (insertedOrderItem <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }
    }
}
