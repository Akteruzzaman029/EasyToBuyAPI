using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.OrderType;
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
    public class OrderTypeController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<OrderTypeController> _logger;
        private readonly IOrderTypeRepository _OrderTypeRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private ResponseDto _responseDto = new ResponseDto();

        public OrderTypeController(SecurityHelper securityHelper,
            ILogger<OrderTypeController> logger,
            IOrderTypeRepository OrderTypeRepository,
            UserManager<ApplicationUser> userManager
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._OrderTypeRepository = OrderTypeRepository;
            this._userManager = userManager;
        }

        [HttpPost("GetOrderType")]
        public async Task<IActionResult> GetOrderType(int pageNumber, [FromBody] OrderTypeFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("OrderType_InvalidPageNumber : "+ pageNumber);

            var result = await _OrderTypeRepository.GetOrderTypes(pageNumber, searchModel);
            if (result == null)
                return NotFound("OrderType_NotFoundList");

            return Ok(result);
        }

        [HttpPost("GetAllOrderTypes")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDistinctOrderType([FromBody] OrderTypeFilterDto searchModel)
        {
            var result = await _OrderTypeRepository.GetDistinctOrderTypes(searchModel);
            if (result == null)
                return NotFound("OrderType_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetOrderTypeById/{id}")]
        public async Task<IActionResult> GetOrderTypeById(int id)
        {

            if (id==0)
                return BadRequest("OrderType_InvalidId : "+ id);

            var result = await _OrderTypeRepository.GetOrderTypeById(id);
            if (result == null)
                return NotFound("OrderType_NotFoundId : "+ id);

            return Ok(result);
        }


        [HttpPost("InsertOrderType")]
        public async Task<IActionResult> InsertOrderType([FromBody] OrderTypeRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("OrderType_Null");

                int insertedOrderTypeId = await _OrderTypeRepository.InsertOrderType(requestModel);
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



        [HttpPost("UpdateOrderType/{OrderTypeId}")]
        public async Task<IActionResult> UpdateOrderType(int OrderTypeId, [FromBody] OrderTypeRequestDto updateRequestModel)
        {

            OrderTypeRequestDto requestModel = updateRequestModel;
            if (requestModel == null)
                return BadRequest("OrderType_Null");

            var requestProcessConfirm = await _OrderTypeRepository.GetOrderTypeById(OrderTypeId);
            if (requestProcessConfirm == null)
                return NotFound("OrderType_NotFoundId : "+ OrderTypeId);

            int insertedOrderType = await _OrderTypeRepository.UpdateOrderType(OrderTypeId, requestModel);

            if (insertedOrderType <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeleteOrderType/{OrderTypeId}")]
        public async Task<IActionResult> DeleteOrderType(int OrderTypeId, [FromBody] OrderTypeRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion

            OrderTypeRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("OrderType_Null");

            var requestProcessConfirm = await _OrderTypeRepository.GetOrderTypeById(OrderTypeId);
            if (requestProcessConfirm == null)
                return NotFound("OrderType_NotFoundId : "+ OrderTypeId);

            int insertedOrderType = await _OrderTypeRepository.DeleteOrderType(OrderTypeId, requestModel);

            if (insertedOrderType <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }
    }
}
