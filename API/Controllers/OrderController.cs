using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.Order;
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
    public class OrderController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderRepository _OrderRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<NotificationHub> _hubContext;

        private readonly IOrderItemRepository _OrderItemRepository;
        private ResponseDto _responseDto = new ResponseDto();

        public OrderController(SecurityHelper securityHelper,
            ILogger<OrderController> logger,
            IOrderRepository OrderRepository,
            IOrderItemRepository OrderItemRepository,
        UserManager<ApplicationUser> userManager,
            IHubContext<NotificationHub> hubContext
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._OrderRepository = OrderRepository;
            this._userManager = userManager;
            this._hubContext = hubContext;
            this._OrderItemRepository = OrderItemRepository;    
        }

        [HttpPost("GetOrder")]
        public async Task<IActionResult> GetOrder(int pageNumber, [FromBody] OrderFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("Order_InvalidPageNumber : "+ pageNumber);

            var result = await _OrderRepository.GetOrders(pageNumber, searchModel);
            if (result == null)
                return NotFound("Order_NotFoundList");

            return Ok(result);
        }

        [HttpPost("GetAllOrders")]
        public async Task<IActionResult> GetDistinctOrder([FromBody] OrderFilterDto searchModel)
        {
            var result = await _OrderRepository.GetDistinctOrders(searchModel);
            if (result == null)
                return NotFound("Order_NotFoundList");

            return Ok(result);
        }


        [HttpGet("GetOrderById/{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {

            if (id==0)
                return BadRequest("Order_InvalidId : "+ id);

            var result = await _OrderRepository.GetOrderById(id);
            if (result == null)
                return NotFound("Order_NotFoundId : "+ id);

            return Ok(result);
        }


        [HttpPost("InsertOrder")]
        public async Task<IActionResult> InsertOrder([FromBody] OrderRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("Order_Null");

                int insertedOrderId = await _OrderRepository.InsertOrder(requestModel);

                if (requestModel.OrderItems.Count>0)
                {
                    foreach (var item in requestModel.OrderItems)
                    {
                        item.OrderId = insertedOrderId;
                        await _OrderItemRepository.InsertOrderItem(item);
                    }
                }

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



        [HttpPost("UpdateOrder/{OrderId}")]
        public async Task<IActionResult> UpdateOrder(int OrderId, [FromBody] OrderRequestDto updateRequestModel)
        {

            OrderRequestDto requestModel = updateRequestModel;
            if (requestModel == null)
                return BadRequest("Order_Null");

            var requestProcessConfirm = await _OrderRepository.GetOrderById(OrderId);
            if (requestProcessConfirm == null)
                return NotFound("Order_NotFoundId : "+ OrderId);

            int insertedOrder = await _OrderRepository.UpdateOrder(OrderId, requestModel);

            if (insertedOrder <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeleteOrder/{OrderId}")]
        public async Task<IActionResult> DeleteOrder(int OrderId, [FromBody] OrderRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion

            OrderRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("Order_Null");

            var requestProcessConfirm = await _OrderRepository.GetOrderById(OrderId);
            if (requestProcessConfirm == null)
                return NotFound("Order_NotFoundId : "+ OrderId);

            int insertedOrder = await _OrderRepository.DeleteOrder(OrderId, requestModel);

            if (insertedOrder <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }
    }
}
