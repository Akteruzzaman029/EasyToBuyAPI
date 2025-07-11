using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.SlotAssignment;
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
    public class SlotAssignmentController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<SlotAssignmentController> _logger;
        private readonly ISlotAssignmentRepository _SlotAssignmentRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<NotificationHub> _hubContext;
        private ResponseDto _responseDto = new ResponseDto();

        public SlotAssignmentController(SecurityHelper securityHelper,
            ILogger<SlotAssignmentController> logger,
            ISlotAssignmentRepository SlotAssignmentRepository,
            UserManager<ApplicationUser> userManager,
            IHubContext<NotificationHub> hubContext
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._SlotAssignmentRepository = SlotAssignmentRepository;
            this._userManager = userManager;
            this._hubContext = hubContext;
        }

        [HttpPost("GetSlotAssignment")]
        public async Task<IActionResult> GetSlotAssignment(int pageNumber, [FromBody] SlotAssignmentFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("SlotAssignment_InvalidPageNumber : "+ pageNumber);

            var result = await _SlotAssignmentRepository.GetSlotAssignments(pageNumber, searchModel);
            if (result == null)
                return NotFound("SlotAssignment_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetAllSlotAssignments/{postId}")]
        public async Task<IActionResult> GetDistinctSlotAssignment(int postId)
        {
            var result = await _SlotAssignmentRepository.GetDistinctSlotAssignments(postId);
            if (result == null)
                return NotFound("SlotAssignment_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetSlotAssignmentById/{id}")]
        public async Task<IActionResult> GetSlotAssignmentById(int id)
        {

            if (id==0)
                return BadRequest("SlotAssignment_InvalidId : "+ id);

            var result = await _SlotAssignmentRepository.GetSlotAssignmentById(id);
            if (result == null)
                return NotFound("SlotAssignment_NotFoundId : "+ id);

            return Ok(result);
        }


        [HttpPost("InsertSlotAssignment")]
        public async Task<IActionResult> InsertSlotAssignment([FromBody] SlotAssignmentRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("SlotAssignment_Null");

                var obj= await _SlotAssignmentRepository.GetSlotAssignmentByName(requestModel);
                if (obj != null)
                    return BadRequest("SlotAssignment_Dublicate_Null");

                int insertedSlotAssignmentId = await _SlotAssignmentRepository.InsertSlotAssignment(requestModel);
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



        [HttpPost("UpdateSlotAssignment/{SlotAssignmentId}")]
        public async Task<IActionResult> UpdateSlotAssignment(int SlotAssignmentId, [FromBody] SlotAssignmentRequestDto updateRequestModel)
        {

            SlotAssignmentRequestDto requestModel = updateRequestModel;
            if (requestModel == null)
                return BadRequest("SlotAssignment_Null");

            var requestProcessConfirm = await _SlotAssignmentRepository.GetSlotAssignmentById(SlotAssignmentId);
            if (requestProcessConfirm == null)
                return NotFound("SlotAssignment_NotFoundId : "+ SlotAssignmentId);

            int insertedSlotAssignment = await _SlotAssignmentRepository.UpdateSlotAssignment(SlotAssignmentId, requestModel);

            if (insertedSlotAssignment <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeleteSlotAssignment/{SlotAssignmentId}")]
        public async Task<IActionResult> DeleteSlotAssignment(int SlotAssignmentId, [FromBody] SlotAssignmentRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion

            SlotAssignmentRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("SlotAssignment_Null");

            var requestProcessConfirm = await _SlotAssignmentRepository.GetSlotAssignmentById(SlotAssignmentId);
            if (requestProcessConfirm == null)
                return NotFound("SlotAssignment_NotFoundId : "+ SlotAssignmentId);

            int insertedSlotAssignment = await _SlotAssignmentRepository.DeleteSlotAssignment(SlotAssignmentId, requestModel);

            if (insertedSlotAssignment <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }
    }
}
