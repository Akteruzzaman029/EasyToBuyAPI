using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.VehicleAvailability;
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
    public class VehicleAvailabilityController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<VehicleAvailabilityController> _logger;
        private readonly IVehicleAvailabilityRepository _VehicleAvailabilityRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<NotificationHub> _hubContext;
        private ResponseDto _responseDto = new ResponseDto();

        public VehicleAvailabilityController(SecurityHelper securityHelper,
            ILogger<VehicleAvailabilityController> logger,
            IVehicleAvailabilityRepository VehicleAvailabilityRepository,
            UserManager<ApplicationUser> userManager,
            IHubContext<NotificationHub> hubContext
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._VehicleAvailabilityRepository = VehicleAvailabilityRepository;
            this._userManager = userManager;
            this._hubContext = hubContext;
        }

        [HttpPost("GetVehicleAvailability")]
        public async Task<IActionResult> GetVehicleAvailability(int pageNumber, [FromBody] VehicleAvailabilityFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("VehicleAvailability_InvalidPageNumber : "+ pageNumber);

            var result = await _VehicleAvailabilityRepository.GetVehicleAvailabilitys(pageNumber, searchModel);
            if (result == null)
                return NotFound("VehicleAvailability_NotFoundList");

            return Ok(result);
        }

        [HttpPost("GetAllVehicleAvailabilitys")]
        public async Task<IActionResult> GetDistinctVehicleAvailability([FromBody] VehicleAvailabilityFilterDto searchModel)
        {
            var result = await _VehicleAvailabilityRepository.GetDistinctVehicleAvailabilitys(searchModel);
            if (result == null)
                return NotFound("VehicleAvailability_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetVehicleAvailabilityById/{id}")]
        public async Task<IActionResult> GetVehicleAvailabilityById(int id)
        {

            if (id==0)
                return BadRequest("VehicleAvailability_InvalidId : "+ id);

            var result = await _VehicleAvailabilityRepository.GetVehicleAvailabilityById(id);
            if (result == null)
                return NotFound("VehicleAvailability_NotFoundId : "+ id);

            return Ok(result);
        }


        [HttpPost("InsertVehicleAvailability")]
        public async Task<IActionResult> InsertVehicleAvailability([FromBody] VehicleAvailabilityRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("VehicleAvailability_Null");

                var obj= await _VehicleAvailabilityRepository.GetVehicleAvailabilityByName(requestModel);
                if (obj != null)
                    return BadRequest("VehicleAvailability_Dublicate_Null");

                int insertedVehicleAvailabilityId = await _VehicleAvailabilityRepository.InsertVehicleAvailability(requestModel);
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



        [HttpPost("UpdateVehicleAvailability/{VehicleAvailabilityId}")]
        public async Task<IActionResult> UpdateVehicleAvailability(int VehicleAvailabilityId, [FromBody] VehicleAvailabilityRequestDto updateRequestModel)
        {

            VehicleAvailabilityRequestDto requestModel = updateRequestModel;
            if (requestModel == null)
                return BadRequest("VehicleAvailability_Null");

            var requestProcessConfirm = await _VehicleAvailabilityRepository.GetVehicleAvailabilityById(VehicleAvailabilityId);
            if (requestProcessConfirm == null)
                return NotFound("VehicleAvailability_NotFoundId : "+ VehicleAvailabilityId);

            int insertedVehicleAvailability = await _VehicleAvailabilityRepository.UpdateVehicleAvailability(VehicleAvailabilityId, requestModel);

            if (insertedVehicleAvailability <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeleteVehicleAvailability/{VehicleAvailabilityId}")]
        public async Task<IActionResult> DeleteVehicleAvailability(int VehicleAvailabilityId, [FromBody] VehicleAvailabilityRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion

            VehicleAvailabilityRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("VehicleAvailability_Null");

            var requestProcessConfirm = await _VehicleAvailabilityRepository.GetVehicleAvailabilityById(VehicleAvailabilityId);
            if (requestProcessConfirm == null)
                return NotFound("VehicleAvailability_NotFoundId : "+ VehicleAvailabilityId);

            int insertedVehicleAvailability = await _VehicleAvailabilityRepository.DeleteVehicleAvailability(VehicleAvailabilityId, requestModel);

            if (insertedVehicleAvailability <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }
    }
}
