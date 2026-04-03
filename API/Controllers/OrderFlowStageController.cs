using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.OrderFlowStage;
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
    public class OrderFlowStageController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<OrderFlowStageController> _logger;
        private readonly IOrderFlowStageRepository _OrderFlowStageRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private ResponseDto _responseDto = new ResponseDto();

        public OrderFlowStageController(SecurityHelper securityHelper,
            ILogger<OrderFlowStageController> logger,
            IOrderFlowStageRepository OrderFlowStageRepository,
            UserManager<ApplicationUser> userManager
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._OrderFlowStageRepository = OrderFlowStageRepository;
            this._userManager = userManager;
        }

        [HttpPost("GetOrderFlowStage")]
        public async Task<IActionResult> GetOrderFlowStage(int pageNumber, [FromBody] OrderFlowStageFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("OrderFlowStage_InvalidPageNumber : "+ pageNumber);

            var result = await _OrderFlowStageRepository.GetOrderFlowStages(pageNumber, searchModel);
            if (result == null)
                return NotFound("OrderFlowStage_NotFoundList");

            return Ok(result);
        }

        [HttpPost("GetAllOrderFlowStages")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDistinctOrderFlowStage([FromBody] OrderFlowStageFilterDto searchModel)
        {
            var result = await _OrderFlowStageRepository.GetDistinctOrderFlowStages(searchModel);
            if (result == null)
                return NotFound("OrderFlowStage_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetOrderFlowStageById/{id}")]
        public async Task<IActionResult> GetOrderFlowStageById(int id)
        {

            if (id==0)
                return BadRequest("OrderFlowStage_InvalidId : "+ id);

            var result = await _OrderFlowStageRepository.GetOrderFlowStageById(id);
            if (result == null)
                return NotFound("OrderFlowStage_NotFoundId : "+ id);

            return Ok(result);
        }


        [HttpPost("InsertOrderFlowStage")]
        public async Task<IActionResult> InsertOrderFlowStage([FromBody] OrderFlowStageRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("OrderFlowStage_Null");

                int insertedOrderFlowStageId = await _OrderFlowStageRepository.InsertOrderFlowStage(requestModel);
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



        [HttpPost("UpdateOrderFlowStage/{OrderFlowStageId}")]
        public async Task<IActionResult> UpdateOrderFlowStage(int OrderFlowStageId, [FromBody] OrderFlowStageRequestDto updateRequestModel)
        {

            OrderFlowStageRequestDto requestModel = updateRequestModel;
            if (requestModel == null)
                return BadRequest("OrderFlowStage_Null");

            var requestProcessConfirm = await _OrderFlowStageRepository.GetOrderFlowStageById(OrderFlowStageId);
            if (requestProcessConfirm == null)
                return NotFound("OrderFlowStage_NotFoundId : "+ OrderFlowStageId);

            int insertedOrderFlowStage = await _OrderFlowStageRepository.UpdateOrderFlowStage(OrderFlowStageId, requestModel);

            if (insertedOrderFlowStage <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeleteOrderFlowStage/{OrderFlowStageId}")]
        public async Task<IActionResult> DeleteOrderFlowStage(int OrderFlowStageId, [FromBody] OrderFlowStageRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion

            OrderFlowStageRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("OrderFlowStage_Null");

            var requestProcessConfirm = await _OrderFlowStageRepository.GetOrderFlowStageById(OrderFlowStageId);
            if (requestProcessConfirm == null)
                return NotFound("OrderFlowStage_NotFoundId : "+ OrderFlowStageId);

            int insertedOrderFlowStage = await _OrderFlowStageRepository.DeleteOrderFlowStage(OrderFlowStageId, requestModel);

            if (insertedOrderFlowStage <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }
    }
}
