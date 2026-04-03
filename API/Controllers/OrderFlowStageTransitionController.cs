using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.OrderFlowStageTransition;
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
    public class OrderFlowStageTransitionController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<OrderFlowStageTransitionController> _logger;
        private readonly IOrderFlowStageTransitionRepository _OrderFlowStageTransitionRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private ResponseDto _responseDto = new ResponseDto();

        public OrderFlowStageTransitionController(SecurityHelper securityHelper,
            ILogger<OrderFlowStageTransitionController> logger,
            IOrderFlowStageTransitionRepository OrderFlowStageTransitionRepository,
            UserManager<ApplicationUser> userManager
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._OrderFlowStageTransitionRepository = OrderFlowStageTransitionRepository;
            this._userManager = userManager;
        }

        [HttpPost("GetOrderFlowStageTransition")]
        public async Task<IActionResult> GetOrderFlowStageTransition(int pageNumber, [FromBody] OrderFlowStageTransitionFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("OrderFlowStageTransition_InvalidPageNumber : "+ pageNumber);

            var result = await _OrderFlowStageTransitionRepository.GetOrderFlowStageTransitions(pageNumber, searchModel);
            if (result == null)
                return NotFound("OrderFlowStageTransition_NotFoundList");

            return Ok(result);
        }

        [HttpPost("GetAllOrderFlowStageTransitions")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDistinctOrderFlowStageTransition([FromBody] OrderFlowStageTransitionFilterDto searchModel)
        {
            var result = await _OrderFlowStageTransitionRepository.GetDistinctOrderFlowStageTransitions(searchModel);
            if (result == null)
                return NotFound("OrderFlowStageTransition_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetOrderFlowStageTransitionById/{id}")]
        public async Task<IActionResult> GetOrderFlowStageTransitionById(int id)
        {

            if (id==0)
                return BadRequest("OrderFlowStageTransition_InvalidId : "+ id);

            var result = await _OrderFlowStageTransitionRepository.GetOrderFlowStageTransitionById(id);
            if (result == null)
                return NotFound("OrderFlowStageTransition_NotFoundId : "+ id);

            return Ok(result);
        }


        [HttpPost("InsertOrderFlowStageTransition")]
        public async Task<IActionResult> InsertOrderFlowStageTransition([FromBody] OrderFlowStageTransitionRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("OrderFlowStageTransition_Null");

                int insertedOrderFlowStageTransitionId = await _OrderFlowStageTransitionRepository.InsertOrderFlowStageTransition(requestModel);
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



        [HttpPost("UpdateOrderFlowStageTransition/{OrderFlowStageTransitionId}")]
        public async Task<IActionResult> UpdateOrderFlowStageTransition(int OrderFlowStageTransitionId, [FromBody] OrderFlowStageTransitionRequestDto updateRequestModel)
        {

            OrderFlowStageTransitionRequestDto requestModel = updateRequestModel;
            if (requestModel == null)
                return BadRequest("OrderFlowStageTransition_Null");

            var requestProcessConfirm = await _OrderFlowStageTransitionRepository.GetOrderFlowStageTransitionById(OrderFlowStageTransitionId);
            if (requestProcessConfirm == null)
                return NotFound("OrderFlowStageTransition_NotFoundId : "+ OrderFlowStageTransitionId);

            int insertedOrderFlowStageTransition = await _OrderFlowStageTransitionRepository.UpdateOrderFlowStageTransition(OrderFlowStageTransitionId, requestModel);

            if (insertedOrderFlowStageTransition <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeleteOrderFlowStageTransition/{OrderFlowStageTransitionId}")]
        public async Task<IActionResult> DeleteOrderFlowStageTransition(int OrderFlowStageTransitionId, [FromBody] OrderFlowStageTransitionRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion

            OrderFlowStageTransitionRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("OrderFlowStageTransition_Null");

            var requestProcessConfirm = await _OrderFlowStageTransitionRepository.GetOrderFlowStageTransitionById(OrderFlowStageTransitionId);
            if (requestProcessConfirm == null)
                return NotFound("OrderFlowStageTransition_NotFoundId : "+ OrderFlowStageTransitionId);

            int insertedOrderFlowStageTransition = await _OrderFlowStageTransitionRepository.DeleteOrderFlowStageTransition(OrderFlowStageTransitionId, requestModel);

            if (insertedOrderFlowStageTransition <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }
    }
}
