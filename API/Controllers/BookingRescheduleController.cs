using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.BookingReschedule;
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
    public class BookingRescheduleController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<BookingRescheduleController> _logger;
        private readonly IBookingRescheduleRepository _BookingRescheduleRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<NotificationHub> _hubContext;
        private ResponseDto _responseDto = new ResponseDto();

        public BookingRescheduleController(SecurityHelper securityHelper,
            ILogger<BookingRescheduleController> logger,
            IBookingRescheduleRepository BookingRescheduleRepository,
            UserManager<ApplicationUser> userManager,
            IHubContext<NotificationHub> hubContext
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._BookingRescheduleRepository = BookingRescheduleRepository;
            this._userManager = userManager;
            this._hubContext = hubContext;
        }

        [HttpPost("GetBookingReschedule")]
        public async Task<IActionResult> GetBookingReschedule(int pageNumber, [FromBody] BookingRescheduleFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("BookingReschedule_InvalidPageNumber : "+ pageNumber);

            var result = await _BookingRescheduleRepository.GetBookingReschedules(pageNumber, searchModel);
            if (result == null)
                return NotFound("BookingReschedule_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetAllBookingReschedules/{postId}")]
        public async Task<IActionResult> GetDistinctBookingReschedule(int postId)
        {
            var result = await _BookingRescheduleRepository.GetDistinctBookingReschedules(postId);
            if (result == null)
                return NotFound("BookingReschedule_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetBookingRescheduleById/{id}")]
        public async Task<IActionResult> GetBookingRescheduleById(int id)
        {

            if (id==0)
                return BadRequest("BookingReschedule_InvalidId : "+ id);

            var result = await _BookingRescheduleRepository.GetBookingRescheduleById(id);
            if (result == null)
                return NotFound("BookingReschedule_NotFoundId : "+ id);

            return Ok(result);
        }


        [HttpPost("InsertBookingReschedule")]
        public async Task<IActionResult> InsertBookingReschedule([FromBody] BookingRescheduleRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("BookingReschedule_Null");

                int insertedBookingRescheduleId = await _BookingRescheduleRepository.InsertBookingReschedule(requestModel);
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



        [HttpPost("UpdateBookingReschedule/{BookingRescheduleId}")]
        public async Task<IActionResult> UpdateBookingReschedule(int BookingRescheduleId, [FromBody] BookingRescheduleRequestDto updateRequestModel)
        {

            BookingRescheduleRequestDto requestModel = updateRequestModel;
            if (requestModel == null)
                return BadRequest("BookingReschedule_Null");

            var requestProcessConfirm = await _BookingRescheduleRepository.GetBookingRescheduleById(BookingRescheduleId);
            if (requestProcessConfirm == null)
                return NotFound("BookingReschedule_NotFoundId : "+ BookingRescheduleId);

            int insertedBookingReschedule = await _BookingRescheduleRepository.UpdateBookingReschedule(BookingRescheduleId, requestModel);

            if (insertedBookingReschedule <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeleteBookingReschedule/{BookingRescheduleId}")]
        public async Task<IActionResult> DeleteBookingReschedule(int BookingRescheduleId, [FromBody] BookingRescheduleRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion

            BookingRescheduleRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("BookingReschedule_Null");

            var requestProcessConfirm = await _BookingRescheduleRepository.GetBookingRescheduleById(BookingRescheduleId);
            if (requestProcessConfirm == null)
                return NotFound("BookingReschedule_NotFoundId : "+ BookingRescheduleId);

            int insertedBookingReschedule = await _BookingRescheduleRepository.DeleteBookingReschedule(BookingRescheduleId, requestModel);

            if (insertedBookingReschedule <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }
    }
}
