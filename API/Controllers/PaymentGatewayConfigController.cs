using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.PaymentGatewayConfig;
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
    public class PaymentGatewayConfigController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<PaymentGatewayConfigController> _logger;
        private readonly IPaymentGatewayConfigRepository _PaymentGatewayConfigRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private ResponseDto _responseDto = new ResponseDto();

        public PaymentGatewayConfigController(SecurityHelper securityHelper,
            ILogger<PaymentGatewayConfigController> logger,
            IPaymentGatewayConfigRepository PaymentGatewayConfigRepository,
            UserManager<ApplicationUser> userManager
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._PaymentGatewayConfigRepository = PaymentGatewayConfigRepository;
            this._userManager = userManager;
        }

        [HttpPost("GetPaymentGatewayConfig")]
        public async Task<IActionResult> GetPaymentGatewayConfig(int pageNumber, [FromBody] PaymentGatewayConfigFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("PaymentGatewayConfig_InvalidPageNumber : "+ pageNumber);

            var result = await _PaymentGatewayConfigRepository.GetCategories(pageNumber, searchModel);
            if (result == null)
                return NotFound("PaymentGatewayConfig_NotFoundList");

            return Ok(result);
        }

        [HttpPost("GetAllCategories")]
        public async Task<IActionResult> GetDistinctPaymentGatewayConfig([FromBody] PaymentGatewayConfigFilterDto searchModel)
        {
            var result = await _PaymentGatewayConfigRepository.GetDistinctCategories(searchModel);
            if (result == null)
                return NotFound("PaymentGatewayConfig_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetPaymentGatewayConfigById/{id}")]
        public async Task<IActionResult> GetPaymentGatewayConfigById(int id)
        {

            if (id==0)
                return BadRequest("PaymentGatewayConfig_InvalidId : "+ id);

            var result = await _PaymentGatewayConfigRepository.GetPaymentGatewayConfigById(id);
            if (result == null)
                return NotFound("PaymentGatewayConfig_NotFoundId : "+ id);

            return Ok(result);
        }


        [HttpPost("InsertPaymentGatewayConfig")]
        public async Task<IActionResult> InsertPaymentGatewayConfig([FromBody] PaymentGatewayConfigRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("PaymentGatewayConfig_Null");

                int insertedPaymentGatewayConfigId = await _PaymentGatewayConfigRepository.InsertPaymentGatewayConfig(requestModel);
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



        [HttpPost("UpdatePaymentGatewayConfig/{PaymentGatewayConfigId}")]
        public async Task<IActionResult> UpdatePaymentGatewayConfig(int PaymentGatewayConfigId, [FromBody] PaymentGatewayConfigRequestDto updateRequestModel)
        {

            PaymentGatewayConfigRequestDto requestModel = updateRequestModel;
            if (requestModel == null)
                return BadRequest("PaymentGatewayConfig_Null");

            var requestProcessConfirm = await _PaymentGatewayConfigRepository.GetPaymentGatewayConfigById(PaymentGatewayConfigId);
            if (requestProcessConfirm == null)
                return NotFound("PaymentGatewayConfig_NotFoundId : "+ PaymentGatewayConfigId);

            int insertedPaymentGatewayConfig = await _PaymentGatewayConfigRepository.UpdatePaymentGatewayConfig(PaymentGatewayConfigId, requestModel);

            if (insertedPaymentGatewayConfig <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeletePaymentGatewayConfig/{PaymentGatewayConfigId}")]
        public async Task<IActionResult> DeletePaymentGatewayConfig(int PaymentGatewayConfigId, [FromBody] PaymentGatewayConfigRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion

            PaymentGatewayConfigRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("PaymentGatewayConfig_Null");

            var requestProcessConfirm = await _PaymentGatewayConfigRepository.GetPaymentGatewayConfigById(PaymentGatewayConfigId);
            if (requestProcessConfirm == null)
                return NotFound("PaymentGatewayConfig_NotFoundId : "+ PaymentGatewayConfigId);

            int insertedPaymentGatewayConfig = await _PaymentGatewayConfigRepository.DeletePaymentGatewayConfig(PaymentGatewayConfigId, requestModel);

            if (insertedPaymentGatewayConfig <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }
    }
}
