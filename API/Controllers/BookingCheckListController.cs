using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.BookingCheckList;
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
    public class BookingCheckListController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<BookingCheckListController> _logger;
        private readonly IBookingCheckListRepository _BookingCheckListRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IUserPackageRepository _UserPackageRepository;
        private ResponseDto _responseDto = new ResponseDto();

        public BookingCheckListController(SecurityHelper securityHelper,
            ILogger<BookingCheckListController> logger,
            IBookingCheckListRepository BookingCheckListRepository,
            UserManager<ApplicationUser> userManager,
            IHubContext<NotificationHub> hubContext,
            IUserPackageRepository UserPackageRepository
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._BookingCheckListRepository = BookingCheckListRepository;
            this._userManager = userManager;
            this._hubContext = hubContext;
            this._UserPackageRepository = UserPackageRepository;
        }

        [HttpPost("GetBookingCheckList")]
        public async Task<IActionResult> GetBookingCheckList(int pageNumber, [FromBody] BookingCheckListFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("BookingCheckList_InvalidPageNumber : "+ pageNumber);

            var result = await _BookingCheckListRepository.GetBookingCheckLists(pageNumber, searchModel);
            if (result == null)
                return NotFound("BookingCheckList_NotFoundList");

            return Ok(result);
        }

        [HttpPost("GetAllBookingCheckLists")]
        public async Task<IActionResult> GetDistinctBookingCheckList([FromBody] BookingCheckListFilterDto searchModel)
        {
            var result = await _BookingCheckListRepository.GetDistinctBookingCheckLists(searchModel);
            if (result == null)
                return NotFound("BookingCheckList_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetMonthlySlotAvailability")]
        public async Task<IActionResult> GetMonthlySlotAvailability(DateTime StartDate)
        {
            var result = await _BookingCheckListRepository.GetMonthlySlotAvailability(StartDate);
            if (result == null)
                return NotFound("BookingCheckList_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetDayWiseBookingCheckLists")]
        public async Task<IActionResult> GetDayWiseBookingCheckLists(DateTime StartDate)
        {
            var result = await _BookingCheckListRepository.GetDayWiseBookingCheckLists(StartDate);
            if (result == null)
                return NotFound("BookingCheckList_NotFoundList");

            return Ok(result);
        }


        [HttpGet("GetBookingCheckListByBookingId/{id}")]
        public async Task<IActionResult> GetBookingCheckListByBookingId(int id)
        {

            if (id == 0)
                return BadRequest("BookingCheckList_Invalid_BookingId : " + id);

            var result = await _BookingCheckListRepository.GetBookingCheckListByBookingId(id);
            if (result == null)
                return NotFound("BookingCheckList_NotFound_BookingId : " + id);

            return Ok(result);
        }
       

        [HttpGet("GetBookingCheckListById/{id}")]
        public async Task<IActionResult> GetBookingCheckListById(int id)
        {

            if (id==0)
                return BadRequest("BookingCheckList_InvalidId : "+ id);

            var result = await _BookingCheckListRepository.GetBookingCheckListById(id);
            if (result == null)
                return NotFound("BookingCheckList_NotFoundId : "+ id);

            return Ok(result);
        }


        [HttpPost("InsertBookingCheckList")]
        public async Task<IActionResult> InsertBookingCheckList([FromBody] BookingCheckListRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("BookingCheckList_Null");

                if (requestModel == null)
                    return BadRequest("BookingCheckList_Null");

                //var findObj = await _BookingCheckListRepository.GetBookingCheckListByName(requestModel);

                //if (findObj != null)
                //    return BadRequest("BookingCheckList already exists");

                int insertedBookingCheckListId = await _BookingCheckListRepository.InsertBookingCheckList(requestModel);
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

        [HttpPost("UpdateBookingCheckList/{BookingCheckListId}")]
        public async Task<IActionResult> UpdateBookingCheckList(int BookingCheckListId, [FromBody] BookingCheckListRequestDto updateRequestModel)
        {

            BookingCheckListRequestDto requestModel = updateRequestModel;
            if (requestModel == null)
                return BadRequest("BookingCheckList_Null");

            var requestProcessConfirm = await _BookingCheckListRepository.GetBookingCheckListById(BookingCheckListId);
            if (requestProcessConfirm == null)
                return NotFound("BookingCheckList_NotFoundId : "+ BookingCheckListId);

            int insertedBookingCheckList = await _BookingCheckListRepository.UpdateBookingCheckList(BookingCheckListId, requestModel);

            if (insertedBookingCheckList <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeleteBookingCheckList/{BookingCheckListId}")]
        public async Task<IActionResult> DeleteBookingCheckList(int BookingCheckListId, [FromBody] BookingCheckListRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion

            BookingCheckListRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("BookingCheckList_Null");

            var requestProcessConfirm = await _BookingCheckListRepository.GetBookingCheckListById(BookingCheckListId);
            if (requestProcessConfirm == null)
                return NotFound("BookingCheckList_NotFoundId : "+ BookingCheckListId);

            int insertedBookingCheckList = await _BookingCheckListRepository.DeleteBookingCheckList(BookingCheckListId, requestModel);

            if (insertedBookingCheckList <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }
    }
}
