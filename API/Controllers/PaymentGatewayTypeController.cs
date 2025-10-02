using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.PaymentGatewayType;
using Infrastructure.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Persistence.Repository;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentGatewayTypeController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<PaymentGatewayTypeController> _logger;
        private readonly IPaymentGatewayTypeRepository _PaymentGatewayTypeRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private ResponseDto _responseDto = new ResponseDto();

        public PaymentGatewayTypeController(SecurityHelper securityHelper,
            ILogger<PaymentGatewayTypeController> logger,
            IPaymentGatewayTypeRepository PaymentGatewayTypeRepository,
            UserManager<ApplicationUser> userManager
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._PaymentGatewayTypeRepository = PaymentGatewayTypeRepository;
            this._userManager = userManager;
        }

        [HttpPost("GetPaymentGatewayType")]
        public async Task<IActionResult> GetPaymentGatewayType(int pageNumber, [FromBody] PaymentGatewayTypeFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("PaymentGatewayType_InvalidPageNumber : "+ pageNumber);

            var result = await _PaymentGatewayTypeRepository.GetCategories(pageNumber, searchModel);
            if (result == null)
                return NotFound("PaymentGatewayType_NotFoundList");

            return Ok(result);
        }

        [HttpPost("GetAllCategories")]
        public async Task<IActionResult> GetDistinctPaymentGatewayType([FromBody] PaymentGatewayTypeFilterDto searchModel)
        {
            var result = await _PaymentGatewayTypeRepository.GetDistinctCategories(searchModel);
            if (result == null)
                return NotFound("PaymentGatewayType_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetPaymentGatewayTypeById/{id}")]
        public async Task<IActionResult> GetPaymentGatewayTypeById(int id)
        {

            if (id==0)
                return BadRequest("PaymentGatewayType_InvalidId : "+ id);

            var result = await _PaymentGatewayTypeRepository.GetPaymentGatewayTypeById(id);
            if (result == null)
                return NotFound("PaymentGatewayType_NotFoundId : "+ id);

            return Ok(result);
        }


        [HttpPost("InsertPaymentGatewayType")]
        public async Task<IActionResult> InsertPaymentGatewayType([FromBody] PaymentGatewayTypeRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("PaymentGatewayType_Null");

                int insertedPaymentGatewayTypeId = await _PaymentGatewayTypeRepository.InsertPaymentGatewayType(requestModel);
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



        [HttpPost("UpdatePaymentGatewayType/{PaymentGatewayTypeId}")]
        public async Task<IActionResult> UpdatePaymentGatewayType(int PaymentGatewayTypeId, [FromBody] PaymentGatewayTypeRequestDto updateRequestModel)
        {

            PaymentGatewayTypeRequestDto requestModel = updateRequestModel;
            if (requestModel == null)
                return BadRequest("PaymentGatewayType_Null");

            var requestProcessConfirm = await _PaymentGatewayTypeRepository.GetPaymentGatewayTypeById(PaymentGatewayTypeId);
            if (requestProcessConfirm == null)
                return NotFound("PaymentGatewayType_NotFoundId : "+ PaymentGatewayTypeId);

            int insertedPaymentGatewayType = await _PaymentGatewayTypeRepository.UpdatePaymentGatewayType(PaymentGatewayTypeId, requestModel);

            if (insertedPaymentGatewayType <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeletePaymentGatewayType/{PaymentGatewayTypeId}")]
        public async Task<IActionResult> DeletePaymentGatewayType(int PaymentGatewayTypeId, [FromBody] PaymentGatewayTypeRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion

            PaymentGatewayTypeRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("PaymentGatewayType_Null");

            var requestProcessConfirm = await _PaymentGatewayTypeRepository.GetPaymentGatewayTypeById(PaymentGatewayTypeId);
            if (requestProcessConfirm == null)
                return NotFound("PaymentGatewayType_NotFoundId : "+ PaymentGatewayTypeId);

            int insertedPaymentGatewayType = await _PaymentGatewayTypeRepository.DeletePaymentGatewayType(PaymentGatewayTypeId, requestModel);

            if (insertedPaymentGatewayType <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }
    }
}
