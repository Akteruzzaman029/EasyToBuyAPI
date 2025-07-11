using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.Attendance;
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
    public class AttendanceController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<AttendanceController> _logger;
        private readonly IAttendanceRepository _AttendanceRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<NotificationHub> _hubContext;
        private ResponseDto _responseDto = new ResponseDto();

        public AttendanceController(SecurityHelper securityHelper,
            ILogger<AttendanceController> logger,
            IAttendanceRepository AttendanceRepository,
            UserManager<ApplicationUser> userManager,
            IHubContext<NotificationHub> hubContext
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._AttendanceRepository = AttendanceRepository;
            this._userManager = userManager;
            this._hubContext = hubContext;
        }

        [HttpPost("GetAttendance")]
        public async Task<IActionResult> GetAttendance(int pageNumber, [FromBody] AttendanceFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("Attendance_InvalidPageNumber : "+ pageNumber);

            var result = await _AttendanceRepository.GetAttendances(pageNumber, searchModel);
            if (result == null)
                return NotFound("Attendance_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetAllAttendances/{postId}")]
        public async Task<IActionResult> GetDistinctAttendance(int postId)
        {
            var result = await _AttendanceRepository.GetDistinctAttendances(postId);
            if (result == null)
                return NotFound("Attendance_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetAttendanceById/{id}")]
        public async Task<IActionResult> GetAttendanceById(int id)
        {

            if (id==0)
                return BadRequest("Attendance_InvalidId : "+ id);

            var result = await _AttendanceRepository.GetAttendanceById(id);
            if (result == null)
                return NotFound("Attendance_NotFoundId : "+ id);

            return Ok(result);
        }


        [HttpPost("InsertAttendance")]
        public async Task<IActionResult> InsertAttendance([FromBody] AttendanceRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("Attendance_Null");

                int insertedAttendanceId = await _AttendanceRepository.InsertAttendance(requestModel);
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



        [HttpPost("UpdateAttendance/{AttendanceId}")]
        public async Task<IActionResult> UpdateAttendance(int AttendanceId, [FromBody] AttendanceRequestDto updateRequestModel)
        {

            AttendanceRequestDto requestModel = updateRequestModel;
            if (requestModel == null)
                return BadRequest("Attendance_Null");

            var requestProcessConfirm = await _AttendanceRepository.GetAttendanceById(AttendanceId);
            if (requestProcessConfirm == null)
                return NotFound("Attendance_NotFoundId : "+ AttendanceId);

            int insertedAttendance = await _AttendanceRepository.UpdateAttendance(AttendanceId, requestModel);

            if (insertedAttendance <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeleteAttendance/{AttendanceId}")]
        public async Task<IActionResult> DeleteAttendance(int AttendanceId, [FromBody] AttendanceRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion

            AttendanceRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("Attendance_Null");

            var requestProcessConfirm = await _AttendanceRepository.GetAttendanceById(AttendanceId);
            if (requestProcessConfirm == null)
                return NotFound("Attendance_NotFoundId : "+ AttendanceId);

            int insertedAttendance = await _AttendanceRepository.DeleteAttendance(AttendanceId, requestModel);

            if (insertedAttendance <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }
    }
}
