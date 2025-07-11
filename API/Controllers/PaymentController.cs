using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.Payment;
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
    public class PaymentController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<PaymentController> _logger;
        private readonly IPaymentRepository _PaymentRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private ResponseDto _responseDto = new ResponseDto();

        public PaymentController(SecurityHelper securityHelper,
            ILogger<PaymentController> logger,
            IPaymentRepository PaymentRepository,
            UserManager<ApplicationUser> userManager
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._PaymentRepository = PaymentRepository;
            this._userManager = userManager;
        }

        [HttpPost("GetPayment")]
        public async Task<IActionResult> GetPayment(int pageNumber, [FromBody] PaymentFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("Payment_InvalidPageNumber : "+ pageNumber);

            var result = await _PaymentRepository.GetPayments(pageNumber, searchModel);
            if (result == null)
                return NotFound("Payment_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetAllPaymentes/{companyId}")]
        public async Task<IActionResult> GetDistinctPayment(int companyId)
        {
            var result = await _PaymentRepository.GetDistinctPayments(companyId);
            if (result == null)
                return NotFound("Payment_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetPaymentByStudentId/{id}")]
        public async Task<IActionResult> GetPaymentByStudentId(int id)
        {

            if (id == 0)
                return BadRequest("Payment_InvalidId : " + id);

            var result = await _PaymentRepository.GetPaymentByStudentId(id);
            if (result == null)
                return NotFound("Payment_NotFound_StudentId : " + id);

            return Ok(result);
        }

        [HttpGet("GetPaymentReceiptById/{id}")]
        public async Task<IActionResult> GetPaymentReceiptById(int id)
        {

            if (id == 0)
                return BadRequest("Payment_InvalidId : " + id);

            var result = await _PaymentRepository.GetPaymentReceiptById(id);
            if (result == null)
                return NotFound("Payment_NotFound_StudentId : " + id);

            return Ok(result);
        }

        [HttpGet("GetPaymentById/{id}")]
        public async Task<IActionResult> GetPaymentById(int id)
        {

            if (id==0)
                return BadRequest("Payment_InvalidId : "+ id);

            var result = await _PaymentRepository.GetPaymentById(id);
            if (result == null)
                return NotFound("Payment_NotFoundId : "+ id);

            return Ok(result);
        }


        [HttpPost("InsertPayment")]
        public async Task<IActionResult> InsertPayment([FromBody] PaymentRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("Payment_Null");

                int insertedPaymentId = await _PaymentRepository.InsertPayment(requestModel);
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



        [HttpPost("UpdatePayment/{PaymentId}")]
        public async Task<IActionResult> UpdatePayment(int PaymentId, [FromBody] PaymentRequestDto updateRequestModel)
        {

            PaymentRequestDto requestModel = updateRequestModel;
            if (requestModel == null)
                return BadRequest("Payment_Null");

            var requestProcessConfirm = await _PaymentRepository.GetPaymentById(PaymentId);
            if (requestProcessConfirm == null)
                return NotFound("Payment_NotFoundId : "+ PaymentId);

            int insertedPayment = await _PaymentRepository.UpdatePayment(PaymentId, requestModel);

            if (insertedPayment <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeletePayment/{PaymentId}")]
        public async Task<IActionResult> DeletePayment(int PaymentId, [FromBody] PaymentRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion

            PaymentRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("Payment_Null");

            var requestProcessConfirm = await _PaymentRepository.GetPaymentById(PaymentId);
            if (requestProcessConfirm == null)
                return NotFound("Payment_NotFoundId : "+ PaymentId);

            int insertedPayment = await _PaymentRepository.DeletePayment(PaymentId, requestModel);

            if (insertedPayment <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }
    }
}
