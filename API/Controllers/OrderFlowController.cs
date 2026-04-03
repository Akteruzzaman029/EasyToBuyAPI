using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.OrderFlow;
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
    public class OrderFlowController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<OrderFlowController> _logger;
        private readonly IOrderFlowRepository _OrderFlowRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private ResponseDto _responseDto = new ResponseDto();

        public OrderFlowController(SecurityHelper securityHelper,
            ILogger<OrderFlowController> logger,
            IOrderFlowRepository OrderFlowRepository,
            UserManager<ApplicationUser> userManager
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._OrderFlowRepository = OrderFlowRepository;
            this._userManager = userManager;
        }

        [HttpPost("GetOrderFlow")]
        public async Task<IActionResult> GetOrderFlow(int pageNumber, [FromBody] OrderFlowFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("OrderFlow_InvalidPageNumber : "+ pageNumber);

            var result = await _OrderFlowRepository.GetOrderFlows(pageNumber, searchModel);
            if (result == null)
                return NotFound("OrderFlow_NotFoundList");

            return Ok(result);
        }

        [HttpPost("GetAllOrderFlows")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDistinctOrderFlow([FromBody] OrderFlowFilterDto searchModel)
        {
            var result = await _OrderFlowRepository.GetDistinctOrderFlows(searchModel);
            if (result == null)
                return NotFound("OrderFlow_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetOrderFlowById/{id}")]
        public async Task<IActionResult> GetOrderFlowById(int id)
        {

            if (id==0)
                return BadRequest("OrderFlow_InvalidId : "+ id);

            var result = await _OrderFlowRepository.GetOrderFlowById(id);
            if (result == null)
                return NotFound("OrderFlow_NotFoundId : "+ id);

            return Ok(result);
        }


        [HttpPost("InsertOrderFlow")]
        public async Task<IActionResult> InsertOrderFlow([FromBody] OrderFlowRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("OrderFlow_Null");

                int insertedOrderFlowId = await _OrderFlowRepository.InsertOrderFlow(requestModel);
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



        [HttpPost("UpdateOrderFlow/{OrderFlowId}")]
        public async Task<IActionResult> UpdateOrderFlow(int OrderFlowId, [FromBody] OrderFlowRequestDto updateRequestModel)
        {

            OrderFlowRequestDto requestModel = updateRequestModel;
            if (requestModel == null)
                return BadRequest("OrderFlow_Null");

            var requestProcessConfirm = await _OrderFlowRepository.GetOrderFlowById(OrderFlowId);
            if (requestProcessConfirm == null)
                return NotFound("OrderFlow_NotFoundId : "+ OrderFlowId);

            int insertedOrderFlow = await _OrderFlowRepository.UpdateOrderFlow(OrderFlowId, requestModel);

            if (insertedOrderFlow <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeleteOrderFlow/{OrderFlowId}")]
        public async Task<IActionResult> DeleteOrderFlow(int OrderFlowId, [FromBody] OrderFlowRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion

            OrderFlowRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("OrderFlow_Null");

            var requestProcessConfirm = await _OrderFlowRepository.GetOrderFlowById(OrderFlowId);
            if (requestProcessConfirm == null)
                return NotFound("OrderFlow_NotFoundId : "+ OrderFlowId);

            int insertedOrderFlow = await _OrderFlowRepository.DeleteOrderFlow(OrderFlowId, requestModel);

            if (insertedOrderFlow <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }
    }
}
