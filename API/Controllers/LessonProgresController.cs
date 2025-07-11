using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.Booking;
using Core.ModelDto.LessonProgres;
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
    public class LessonProgresController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<LessonProgresController> _logger;
        private readonly ILessonProgresRepository _LessonProgresRepository;
        private readonly IBookingRepository _BookingRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<NotificationHub> _hubContext;
        private ResponseDto _responseDto = new ResponseDto();

        public LessonProgresController(SecurityHelper securityHelper,
            ILogger<LessonProgresController> logger,
            ILessonProgresRepository LessonProgresRepository,
            UserManager<ApplicationUser> userManager,
            IHubContext<NotificationHub> hubContext,
            IBookingRepository BookingRepository
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._LessonProgresRepository = LessonProgresRepository;
            this._userManager = userManager;
            this._hubContext = hubContext;
            this._BookingRepository = BookingRepository;
        }

        [HttpPost("GetLessonProgres")]
        public async Task<IActionResult> GetLessonProgres(int pageNumber, [FromBody] LessonProgresFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("LessonProgres_InvalidPageNumber : "+ pageNumber);

            var result = await _LessonProgresRepository.GetLessonProgress(pageNumber, searchModel);
            if (result == null)
                return NotFound("LessonProgres_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetAllLessonProgress/{postId}")]
        public async Task<IActionResult> GetDistinctLessonProgres(int postId)
        {
            var result = await _LessonProgresRepository.GetDistinctLessonProgress(postId);
            if (result == null)
                return NotFound("LessonProgres_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetLessonProgresById/{id}")]
        public async Task<IActionResult> GetLessonProgresById(int id)
        {

            if (id==0)
                return BadRequest("LessonProgres_InvalidId : "+ id);

            var result = await _LessonProgresRepository.GetLessonProgresById(id);
            if (result == null)
                return NotFound("LessonProgres_NotFoundId : "+ id);

            return Ok(result);
        }


        [HttpPost("InsertLessonProgres")]
        public async Task<IActionResult> InsertLessonProgres([FromBody] LessonProgresRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("LessonProgres_Null");

                int insertedLessonProgresId = await _LessonProgresRepository.InsertLessonProgres(requestModel);
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


        [HttpPost("LessonProgres")]
        public async Task<IActionResult> LessonProgres([FromBody] LessonProgresDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("LessonProgres_Null");

                LessonProgresRequestDto requestModelDto=new LessonProgresRequestDto();
                BookingResponseDto oBookingResponseDto = new BookingResponseDto();

                oBookingResponseDto = await _BookingRepository.GetBookingById(requestModel.BookingId);

                int insertedLessonProgresId = await _LessonProgresRepository.InsertLessonProgres(requestModelDto);
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

        [HttpPost("UpdateLessonProgres/{LessonProgresId}")]
        public async Task<IActionResult> UpdateLessonProgres(int LessonProgresId, [FromBody] LessonProgresRequestDto updateRequestModel)
        {

            LessonProgresRequestDto requestModel = updateRequestModel;
            if (requestModel == null)
                return BadRequest("LessonProgres_Null");

            var requestProcessConfirm = await _LessonProgresRepository.GetLessonProgresById(LessonProgresId);
            if (requestProcessConfirm == null)
                return NotFound("LessonProgres_NotFoundId : "+ LessonProgresId);

            int insertedLessonProgres = await _LessonProgresRepository.UpdateLessonProgres(LessonProgresId, requestModel);

            if (insertedLessonProgres <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeleteLessonProgres/{LessonProgresId}")]
        public async Task<IActionResult> DeleteLessonProgres(int LessonProgresId, [FromBody] LessonProgresRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion

            LessonProgresRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("LessonProgres_Null");

            var requestProcessConfirm = await _LessonProgresRepository.GetLessonProgresById(LessonProgresId);
            if (requestProcessConfirm == null)
                return NotFound("LessonProgres_NotFoundId : "+ LessonProgresId);

            int insertedLessonProgres = await _LessonProgresRepository.DeleteLessonProgres(LessonProgresId, requestModel);

            if (insertedLessonProgres <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }
    }
}
