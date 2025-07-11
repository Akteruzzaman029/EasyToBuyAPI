using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.MeasurementUnit;
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
    public class MeasurementUnitController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<MeasurementUnitController> _logger;
        private readonly IMeasurementUnitRepository _MeasurementUnitRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<NotificationHub> _hubContext;
        private ResponseDto _responseDto = new ResponseDto();

        public MeasurementUnitController(SecurityHelper securityHelper,
            ILogger<MeasurementUnitController> logger,
            IMeasurementUnitRepository MeasurementUnitRepository,
            UserManager<ApplicationUser> userManager,
            IHubContext<NotificationHub> hubContext
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._MeasurementUnitRepository = MeasurementUnitRepository;
            this._userManager = userManager;
            this._hubContext = hubContext;
        }

        [HttpPost("GetMeasurementUnit")]
        public async Task<IActionResult> GetMeasurementUnit(int pageNumber, [FromBody] MeasurementUnitFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("MeasurementUnit_InvalidPageNumber : "+ pageNumber);

            var result = await _MeasurementUnitRepository.GetMeasurementUnits(pageNumber, searchModel);
            if (result == null)
                return NotFound("MeasurementUnit_NotFoundList");

            return Ok(result);
        }

        [HttpPost("GetAllMeasurementUnits")]
        public async Task<IActionResult> GetDistinctMeasurementUnit([FromBody] MeasurementUnitFilterDto searchModel)
        {
            var result = await _MeasurementUnitRepository.GetDistinctMeasurementUnits(searchModel);
            if (result == null)
                return NotFound("MeasurementUnit_NotFoundList");

            return Ok(result);
        }


        [HttpGet("GetMeasurementUnitById/{id}")]
        public async Task<IActionResult> GetMeasurementUnitById(int id)
        {

            if (id==0)
                return BadRequest("MeasurementUnit_InvalidId : "+ id);

            var result = await _MeasurementUnitRepository.GetMeasurementUnitById(id);
            if (result == null)
                return NotFound("MeasurementUnit_NotFoundId : "+ id);

            return Ok(result);
        }


        [HttpPost("InsertMeasurementUnit")]
        public async Task<IActionResult> InsertMeasurementUnit([FromBody] MeasurementUnitRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("MeasurementUnit_Null");

                int insertedMeasurementUnitId = await _MeasurementUnitRepository.InsertMeasurementUnit(requestModel);
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



        [HttpPost("UpdateMeasurementUnit/{MeasurementUnitId}")]
        public async Task<IActionResult> UpdateMeasurementUnit(int MeasurementUnitId, [FromBody] MeasurementUnitRequestDto updateRequestModel)
        {

            MeasurementUnitRequestDto requestModel = updateRequestModel;
            if (requestModel == null)
                return BadRequest("MeasurementUnit_Null");

            var requestProcessConfirm = await _MeasurementUnitRepository.GetMeasurementUnitById(MeasurementUnitId);
            if (requestProcessConfirm == null)
                return NotFound("MeasurementUnit_NotFoundId : "+ MeasurementUnitId);

            int insertedMeasurementUnit = await _MeasurementUnitRepository.UpdateMeasurementUnit(MeasurementUnitId, requestModel);

            if (insertedMeasurementUnit <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeleteMeasurementUnit/{MeasurementUnitId}")]
        public async Task<IActionResult> DeleteMeasurementUnit(int MeasurementUnitId, [FromBody] MeasurementUnitRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion

            MeasurementUnitRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("MeasurementUnit_Null");

            var requestProcessConfirm = await _MeasurementUnitRepository.GetMeasurementUnitById(MeasurementUnitId);
            if (requestProcessConfirm == null)
                return NotFound("MeasurementUnit_NotFoundId : "+ MeasurementUnitId);

            int insertedMeasurementUnit = await _MeasurementUnitRepository.DeleteMeasurementUnit(MeasurementUnitId, requestModel);

            if (insertedMeasurementUnit <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }
    }
}
