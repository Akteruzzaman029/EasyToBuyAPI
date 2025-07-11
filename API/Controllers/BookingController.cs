using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.Booking;
using Core.ModelDto.UserPackage;
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
    public class BookingController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<BookingController> _logger;
        private readonly IBookingRepository _BookingRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IUserPackageRepository _UserPackageRepository;
        private ResponseDto _responseDto = new ResponseDto();

        public BookingController(SecurityHelper securityHelper,
            ILogger<BookingController> logger,
            IBookingRepository BookingRepository,
            UserManager<ApplicationUser> userManager,
            IHubContext<NotificationHub> hubContext,
            IUserPackageRepository UserPackageRepository
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._BookingRepository = BookingRepository;
            this._userManager = userManager;
            this._hubContext = hubContext;
            this._UserPackageRepository = UserPackageRepository;
        }

        [HttpPost("GetBooking")]
        public async Task<IActionResult> GetBooking(int pageNumber, [FromBody] BookingFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("Booking_InvalidPageNumber : "+ pageNumber);

            var result = await _BookingRepository.GetBookings(pageNumber, searchModel);
            if (result == null)
                return NotFound("Booking_NotFoundList");

            return Ok(result);
        }

        [HttpPost("GetAllBookings")]
        public async Task<IActionResult> GetDistinctBooking([FromBody] BookingFilterDto searchModel)
        {
            var result = await _BookingRepository.GetDistinctBookings(searchModel);
            if (result == null)
                return NotFound("Booking_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetMonthlySlotAvailability")]
        public async Task<IActionResult> GetMonthlySlotAvailability(DateTime StartDate)
        {
            var result = await _BookingRepository.GetMonthlySlotAvailability(StartDate);
            if (result == null)
                return NotFound("Booking_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetDayWiseBookings")]
        public async Task<IActionResult> GetDayWiseBookings(DateTime StartDate)
        {
            var result = await _BookingRepository.GetDayWiseBookings(StartDate);
            if (result == null)
                return NotFound("Booking_NotFoundList");

            return Ok(result);
        }


        [HttpGet("GetBookingByStudentId/{id}")]
        public async Task<IActionResult> GetBookingByStudentId(int id)
        {

            if (id == 0)
                return BadRequest("Booking_Invalid_StudentID : " + id);

            var result = await _BookingRepository.GetBookingByStudentId(id);
            if (result == null)
                return NotFound("Booking_NotFound_StudentId : " + id);

            return Ok(result);
        }
       

        [HttpGet("GetBookingById/{id}")]
        public async Task<IActionResult> GetBookingById(int id)
        {

            if (id==0)
                return BadRequest("Booking_InvalidId : "+ id);

            var result = await _BookingRepository.GetBookingById(id);
            if (result == null)
                return NotFound("Booking_NotFoundId : "+ id);

            return Ok(result);
        }


        [HttpPost("InsertBooking")]
        public async Task<IActionResult> InsertBooking([FromBody] BookingRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("Booking_Null");

                if (requestModel == null)
                    return BadRequest("Booking_Null");

                var findObj = await _BookingRepository.GetBookingByName(requestModel);

                if (findObj != null)
                    return BadRequest("Booking already exists");

                int insertedBookingId = await _BookingRepository.InsertBooking(requestModel);
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


        [HttpPost("StudentSlotAssign")]
        public async Task<IActionResult> StudentSlotAssign([FromBody] BookingAssignRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("Booking_Null");

                UserPackageRequestDto insertRequestModel = new UserPackageRequestDto();
                insertRequestModel.UserId=requestModel.UserId;
                insertRequestModel.PackageId=requestModel.PackageId;
                insertRequestModel.PaymentStatus=1; // Assuming 1 means paid
                //insertRequestModel.PackageStartDate=requestModel.PackageStartDate;
                //insertRequestModel.ExpiryDate=requestModel.ExpiryDate;
                //insertRequestModel.NoOfLesson=requestModel.NoOfLesson;
                //insertRequestModel.LessonRate=requestModel.LessonRate;
                //insertRequestModel.Amount=requestModel.Amount;
                //insertRequestModel.Discount=requestModel.Discount;
                //insertRequestModel.NetAmount=requestModel.NetAmount;
                //insertRequestModel.Remarks="";
                //insertRequestModel.IsActive=true; // Assuming 1 means active

                await _UserPackageRepository.InsertUserPackage(insertRequestModel);

                foreach (var item in requestModel.Slots)
                {
                    BookingRequestDto bookingRequestDto = new BookingRequestDto
                    {
                        StudentId=requestModel.StudentId,
                        slotId=requestModel.slotId,
                        ClassDate=item.Date,
                        SlotName=item.SlotName,
                        StartTime=item.StartTime,
                        EndTime=item.EndTime,
                        Status=1,
                        Remarks=requestModel.Remarks,
                        IsActive=requestModel.IsActive,
                    };
                    int insertedBookingId = await _BookingRepository.InsertBooking(bookingRequestDto);
                }

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



        [HttpPost("UpdateBooking/{BookingId}")]
        public async Task<IActionResult> UpdateBooking(int BookingId, [FromBody] BookingRequestDto updateRequestModel)
        {

            BookingRequestDto requestModel = updateRequestModel;
            if (requestModel == null)
                return BadRequest("Booking_Null");

            var requestProcessConfirm = await _BookingRepository.GetBookingById(BookingId);
            if (requestProcessConfirm == null)
                return NotFound("Booking_NotFoundId : "+ BookingId);

            int insertedBooking = await _BookingRepository.UpdateBooking(BookingId, requestModel);

            if (insertedBooking <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeleteBooking/{BookingId}")]
        public async Task<IActionResult> DeleteBooking(int BookingId, [FromBody] BookingRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion

            BookingRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("Booking_Null");

            var requestProcessConfirm = await _BookingRepository.GetBookingById(BookingId);
            if (requestProcessConfirm == null)
                return NotFound("Booking_NotFoundId : "+ BookingId);

            int insertedBooking = await _BookingRepository.DeleteBooking(BookingId, requestModel);

            if (insertedBooking <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }
    }
}
