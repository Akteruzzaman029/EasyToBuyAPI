using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.Slot;
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
    public class SlotController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<SlotController> _logger;
        private readonly ISlotRepository _SlotRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private ResponseDto _responseDto = new ResponseDto();

        public SlotController(SecurityHelper securityHelper,
            ILogger<SlotController> logger,
            ISlotRepository SlotRepository,
            UserManager<ApplicationUser> userManager
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._SlotRepository = SlotRepository;
            this._userManager = userManager;
        }

        [HttpPost("GetSlot")]
        public async Task<IActionResult> GetSlot(int pageNumber, [FromBody] SlotFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("Slot_InvalidPageNumber : "+ pageNumber);

            var result = await _SlotRepository.GetSlots(pageNumber, searchModel);
            if (result == null)
                return NotFound("Slot_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetAllSlotes")]
        public async Task<IActionResult> GetDistinctSlot(DateTime StartDate)
        {
            var result = await _SlotRepository.GetDistinctSlots(StartDate);
            if (result == null)
                return NotFound("Slot_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetSlotById/{id}")]
        public async Task<IActionResult> GetSlotById(int id)
        {

            if (id==0)
                return BadRequest("Slot_InvalidId : "+ id);

            var result = await _SlotRepository.GetSlotById(id);
            if (result == null)
                return NotFound("Slot_NotFoundId : "+ id);

            return Ok(result);
        }


        [HttpGet("GetMonthlySlot")]
        public async Task<IActionResult> GetMonthlySlot(DateTime StartDate)
        {
            var result = await _SlotRepository.GetMonthlySlot(StartDate);
            if (result == null)
                return NotFound("Slot_NotFoundList");

            return Ok(result);
        }


        [HttpPost("InsertSlot")]
        public async Task<IActionResult> InsertSlot([FromBody] SlotRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("Slot_Null");

                int insertedSlotId = await _SlotRepository.InsertSlot(requestModel);
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



        [HttpPost("UpdateSlot/{SlotId}")]
        public async Task<IActionResult> UpdateSlot(int SlotId, [FromBody] SlotRequestDto updateRequestModel)
        {

            SlotRequestDto requestModel = updateRequestModel;
            if (requestModel == null)
                return BadRequest("Slot_Null");

            var requestProcessConfirm = await _SlotRepository.GetSlotById(SlotId);
            if (requestProcessConfirm == null)
                return NotFound("Slot_NotFoundId : "+ SlotId);

            int insertedSlot = await _SlotRepository.UpdateSlot(SlotId, requestModel);

            if (insertedSlot <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeleteSlot/{SlotId}")]
        public async Task<IActionResult> DeleteSlot(int SlotId, [FromBody] SlotRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion

            SlotRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("Slot_Null");

            var requestProcessConfirm = await _SlotRepository.GetSlotById(SlotId);
            if (requestProcessConfirm == null)
                return NotFound("Slot_NotFoundId : "+ SlotId);

            int insertedSlot = await _SlotRepository.DeleteSlot(SlotId, requestModel);

            if (insertedSlot <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }
    }
}
