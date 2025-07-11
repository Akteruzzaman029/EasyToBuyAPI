using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.Appointment;
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
    public class AppointmentController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<AppointmentController> _logger;
        private readonly IAppointmentRepository _AppointmentRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private ResponseDto _responseDto = new ResponseDto();

        public AppointmentController(SecurityHelper securityHelper,
            ILogger<AppointmentController> logger,
            IAppointmentRepository AppointmentRepository,
            UserManager<ApplicationUser> userManager
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._AppointmentRepository = AppointmentRepository;
            this._userManager = userManager;
        }

        [HttpPost("GetAppointment")]
        public async Task<IActionResult> GetAppointment(int pageNumber, [FromBody] AppointmentFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("Appointment_InvalidPageNumber : "+ pageNumber);

            var result = await _AppointmentRepository.GetAppointments(pageNumber, searchModel);
            if (result == null)
                return NotFound("Appointment_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetAllAppointmentes/{companyId}")]
        public async Task<IActionResult> GetDistinctAppointment(int companyId)
        {
            var result = await _AppointmentRepository.GetDistinctAppointments(companyId);
            if (result == null)
                return NotFound("Appointment_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetAppointmentById/{id}")]
        public async Task<IActionResult> GetAppointmentById(int id)
        {

            if (id==0)
                return BadRequest("Appointment_InvalidId : "+ id);

            var result = await _AppointmentRepository.GetAppointmentById(id);
            if (result == null)
                return NotFound("Appointment_NotFoundId : "+ id);

            return Ok(result);
        }


        [HttpPost("InsertAppointment")]
        public async Task<IActionResult> InsertAppointment([FromBody] AppointmentRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("Appointment_Null");

                int insertedAppointmentId = await _AppointmentRepository.InsertAppointment(requestModel);
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



        [HttpPost("UpdateAppointment/{AppointmentId}")]
        public async Task<IActionResult> UpdateAppointment(int AppointmentId, [FromBody] AppointmentRequestDto updateRequestModel)
        {

            AppointmentRequestDto requestModel = updateRequestModel;
            if (requestModel == null)
                return BadRequest("Appointment_Null");

            var requestProcessConfirm = await _AppointmentRepository.GetAppointmentById(AppointmentId);
            if (requestProcessConfirm == null)
                return NotFound("Appointment_NotFoundId : "+ AppointmentId);

            int insertedAppointment = await _AppointmentRepository.UpdateAppointment(AppointmentId, requestModel);

            if (insertedAppointment <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeleteAppointment/{AppointmentId}")]
        public async Task<IActionResult> DeleteAppointment(int AppointmentId, [FromBody] AppointmentRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion

            AppointmentRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("Appointment_Null");

            var requestProcessConfirm = await _AppointmentRepository.GetAppointmentById(AppointmentId);
            if (requestProcessConfirm == null)
                return NotFound("Appointment_NotFoundId : "+ AppointmentId);

            int insertedAppointment = await _AppointmentRepository.DeleteAppointment(AppointmentId, requestModel);

            if (insertedAppointment <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }
    }
}
