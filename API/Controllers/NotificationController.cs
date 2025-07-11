using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.Notification;
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
    public class NotificationController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<NotificationController> _logger;
        private readonly INotificationRepository _NotificationRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<NotificationHub> _hubContext;
        private ResponseDto _responseDto = new ResponseDto();

        public NotificationController(SecurityHelper securityHelper,
            ILogger<NotificationController> logger,
            INotificationRepository NotificationRepository,
            UserManager<ApplicationUser> userManager,
            IHubContext<NotificationHub> hubContext
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._NotificationRepository = NotificationRepository;
            this._userManager = userManager;

            this._hubContext = hubContext;
        }

        [HttpPost("GetNotification")]
        public async Task<IActionResult> GetNotification(int pageNumber, [FromBody] NotificationFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("Notification_InvalidPageNumber : "+ pageNumber);

            var result = await _NotificationRepository.GetNotifications(pageNumber, searchModel);
            if (result == null)
                return NotFound("Notification_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetAllNotifications/{userId}")]
        public async Task<IActionResult> GetDistinctNotification(string userId)
        {
            var result = await _NotificationRepository.GetDistinctNotifications(userId);
            if (result == null)
                return NotFound("Notification_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetNotificationById/{id}")]
        public async Task<IActionResult> GetNotificationById(int id)
        {

            if (id==0)
                return BadRequest("Notification_InvalidId : "+ id);

            var result = await _NotificationRepository.GetNotificationById(id);
            if (result == null)
                return NotFound("Notification_NotFoundId : "+ id);

            return Ok(result);
        }


        [HttpPost("InsertNotification")]
        public async Task<IActionResult> InsertNotification([FromBody] NotificationRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("Notification_Null");

                int insertedNotificationId = await _NotificationRepository.InsertNotification(requestModel);
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



        [HttpPost("UpdateNotification/{NotificationId}")]
        public async Task<IActionResult> UpdateNotification(int NotificationId, [FromBody] NotificationRequestDto updateRequestModel)
        {

            NotificationRequestDto requestModel = updateRequestModel;
            if (requestModel == null)
                return BadRequest("Notification_Null");

            var requestProcessConfirm = await _NotificationRepository.GetNotificationById(NotificationId);
            if (requestProcessConfirm == null)
                return NotFound("Notification_NotFoundId : "+ NotificationId);

            int insertedNotification = await _NotificationRepository.UpdateNotification(NotificationId, requestModel);

            if (insertedNotification <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeleteNotification/{NotificationId}")]
        public async Task<IActionResult> DeleteNotification(int NotificationId, [FromBody] NotificationRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion

            NotificationRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("Notification_Null");

            var requestProcessConfirm = await _NotificationRepository.GetNotificationById(NotificationId);
            if (requestProcessConfirm == null)
                return NotFound("Notification_NotFoundId : "+ NotificationId);

            int insertedNotification = await _NotificationRepository.DeleteNotification(NotificationId, requestModel);

            if (insertedNotification <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }
    }
}
