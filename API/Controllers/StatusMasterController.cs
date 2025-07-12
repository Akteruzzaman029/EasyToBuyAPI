using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.StatusMaster;
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
    public class StatusMasterController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<StatusMasterController> _logger;
        private readonly IStatusMasterRepository _StatusMasterRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<NotificationHub> _hubContext;
        private ResponseDto _responseDto = new ResponseDto();

        public StatusMasterController(SecurityHelper securityHelper,
            ILogger<StatusMasterController> logger,
            IStatusMasterRepository StatusMasterRepository,
            UserManager<ApplicationUser> userManager,
            IHubContext<NotificationHub> hubContext
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._StatusMasterRepository = StatusMasterRepository;
            this._userManager = userManager;
            this._hubContext = hubContext;
        }

        [HttpPost("GetStatusMaster")]
        public async Task<IActionResult> GetStatusMaster(int pageNumber, [FromBody] StatusMasterFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("StatusMaster_InvalidPageNumber : "+ pageNumber);

            var result = await _StatusMasterRepository.GetStatusMasters(pageNumber, searchModel);
            if (result == null)
                return NotFound("StatusMaster_NotFoundList");

            return Ok(result);
        }

        [HttpPost("GetAllStatusMasters")]
        public async Task<IActionResult> GetDistinctStatusMaster([FromBody] StatusMasterFilterDto searchModel)
        {
            var result = await _StatusMasterRepository.GetDistinctStatusMasters(searchModel);
            if (result == null)
                return NotFound("StatusMaster_NotFoundList");

            return Ok(result);
        }


        [HttpGet("GetStatusMasterById/{id}")]
        public async Task<IActionResult> GetStatusMasterById(int id)
        {

            if (id==0)
                return BadRequest("StatusMaster_InvalidId : "+ id);

            var result = await _StatusMasterRepository.GetStatusMasterById(id);
            if (result == null)
                return NotFound("StatusMaster_NotFoundId : "+ id);

            return Ok(result);
        }


        [HttpPost("InsertStatusMaster")]
        public async Task<IActionResult> InsertStatusMaster([FromBody] StatusMasterRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("StatusMaster_Null");

                int insertedStatusMasterId = await _StatusMasterRepository.InsertStatusMaster(requestModel);
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



        [HttpPost("UpdateStatusMaster/{StatusMasterId}")]
        public async Task<IActionResult> UpdateStatusMaster(int StatusMasterId, [FromBody] StatusMasterRequestDto updateRequestModel)
        {

            StatusMasterRequestDto requestModel = updateRequestModel;
            if (requestModel == null)
                return BadRequest("StatusMaster_Null");

            var requestProcessConfirm = await _StatusMasterRepository.GetStatusMasterById(StatusMasterId);
            if (requestProcessConfirm == null)
                return NotFound("StatusMaster_NotFoundId : "+ StatusMasterId);

            int insertedStatusMaster = await _StatusMasterRepository.UpdateStatusMaster(StatusMasterId, requestModel);

            if (insertedStatusMaster <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeleteStatusMaster/{StatusMasterId}")]
        public async Task<IActionResult> DeleteStatusMaster(int StatusMasterId, [FromBody] StatusMasterRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion

            StatusMasterRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("StatusMaster_Null");

            var requestProcessConfirm = await _StatusMasterRepository.GetStatusMasterById(StatusMasterId);
            if (requestProcessConfirm == null)
                return NotFound("StatusMaster_NotFoundId : "+ StatusMasterId);

            int insertedStatusMaster = await _StatusMasterRepository.DeleteStatusMaster(StatusMasterId, requestModel);

            if (insertedStatusMaster <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }
    }
}
