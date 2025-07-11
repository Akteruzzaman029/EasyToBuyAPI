using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.FinalTestResult;
using Infrastructure.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Persistence.Repository;
using System.Security.Claims;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinalTestResultController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<FinalTestResultController> _logger;
        private readonly IFinalTestResultRepository _FinalTestResultRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<NotificationHub> _hubContext;
        private ResponseDto _responseDto = new ResponseDto();

        public FinalTestResultController(SecurityHelper securityHelper,
            ILogger<FinalTestResultController> logger,
            IFinalTestResultRepository FinalTestResultRepository,
            UserManager<ApplicationUser> userManager,
            IHubContext<NotificationHub> hubContext
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._FinalTestResultRepository = FinalTestResultRepository;
            this._userManager = userManager;
            this._hubContext = hubContext;
        }

        [HttpPost("GetFinalTestResult")]
        public async Task<IActionResult> GetFinalTestResult(int pageNumber, [FromBody] FinalTestResultFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("FinalTestResult_InvalidPageNumber : "+ pageNumber);

            var result = await _FinalTestResultRepository.GetFinalTestResults(pageNumber, searchModel);
            if (result == null)
                return NotFound("FinalTestResult_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetAllFinalTestResults/{postId}")]
        public async Task<IActionResult> GetDistinctFinalTestResult(int postId)
        {
            var result = await _FinalTestResultRepository.GetDistinctFinalTestResults(postId);
            if (result == null)
                return NotFound("FinalTestResult_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetFinalTestResultById/{id}")]
        public async Task<IActionResult> GetFinalTestResultById(int id)
        {

            if (id==0)
                return BadRequest("FinalTestResult_InvalidId : "+ id);

            var result = await _FinalTestResultRepository.GetFinalTestResultById(id);
            if (result == null)
                return NotFound("FinalTestResult_NotFoundId : "+ id);

            return Ok(result);
        }


        [HttpPost("InsertFinalTestResult")]
        public async Task<IActionResult> InsertFinalTestResult([FromBody] FinalTestResultRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("FinalTestResult_Null");

                int insertedFinalTestResultId = await _FinalTestResultRepository.InsertFinalTestResult(requestModel);
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



        [HttpPost("UpdateFinalTestResult/{FinalTestResultId}")]
        public async Task<IActionResult> UpdateFinalTestResult(int FinalTestResultId, [FromBody] FinalTestResultRequestDto updateRequestModel)
        {

            FinalTestResultRequestDto requestModel = updateRequestModel;
            if (requestModel == null)
                return BadRequest("FinalTestResult_Null");

            var requestProcessConfirm = await _FinalTestResultRepository.GetFinalTestResultById(FinalTestResultId);
            if (requestProcessConfirm == null)
                return NotFound("FinalTestResult_NotFoundId : "+ FinalTestResultId);

            int insertedFinalTestResult = await _FinalTestResultRepository.UpdateFinalTestResult(FinalTestResultId, requestModel);

            if (insertedFinalTestResult <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeleteFinalTestResult/{FinalTestResultId}")]
        public async Task<IActionResult> DeleteFinalTestResult(int FinalTestResultId, [FromBody] FinalTestResultRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion

            FinalTestResultRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("FinalTestResult_Null");

            var requestProcessConfirm = await _FinalTestResultRepository.GetFinalTestResultById(FinalTestResultId);
            if (requestProcessConfirm == null)
                return NotFound("FinalTestResult_NotFoundId : "+ FinalTestResultId);

            int insertedFinalTestResult = await _FinalTestResultRepository.DeleteFinalTestResult(FinalTestResultId, requestModel);

            if (insertedFinalTestResult <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }
    }
}
