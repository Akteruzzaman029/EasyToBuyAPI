using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.Vehicle;
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
    public class VehicleController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<VehicleController> _logger;
        private readonly IVehicleRepository _VehicleRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private ResponseDto _responseDto = new ResponseDto();

        public VehicleController(SecurityHelper securityHelper,
            ILogger<VehicleController> logger,
            IVehicleRepository VehicleRepository,
            UserManager<ApplicationUser> userManager
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._VehicleRepository = VehicleRepository;
            this._userManager = userManager;
        }

        [HttpPost("GetVehicle")]
        public async Task<IActionResult> GetVehicle(int pageNumber, [FromBody] VehicleFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("Vehicle_InvalidPageNumber : "+ pageNumber);

            var result = await _VehicleRepository.GetVehicles(pageNumber, searchModel);
            if (result == null)
                return NotFound("Vehicle_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetAllVehicles")]
        public async Task<IActionResult> GetDistinctVehicle()
        {
            var result = await _VehicleRepository.GetDistinctVehicles();
            if (result == null)
                return NotFound("Vehicle_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetVehicleById/{id}")]
        public async Task<IActionResult> GetVehicleById(int id)
        {

            if (id==0)
                return BadRequest("Vehicle_InvalidId : "+ id);

            var result = await _VehicleRepository.GetVehicleById(id);
            if (result == null)
                return NotFound("Vehicle_NotFoundId : "+ id);

            return Ok(result);
        }


        [HttpPost("InsertVehicle")]
        public async Task<IActionResult> InsertVehicle([FromBody] VehicleRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("Vehicle_Null");

                int insertedVehicleId = await _VehicleRepository.InsertVehicle(requestModel);
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



        [HttpPost("UpdateVehicle/{VehicleId}")]
        public async Task<IActionResult> UpdateVehicle(int VehicleId, [FromBody] VehicleRequestDto updateRequestModel)
        {

            VehicleRequestDto requestModel = updateRequestModel;
            if (requestModel == null)
                return BadRequest("Vehicle_Null");

            var requestProcessConfirm = await _VehicleRepository.GetVehicleById(VehicleId);
            if (requestProcessConfirm == null)
                return NotFound("Vehicle_NotFoundId : "+ VehicleId);

            int insertedVehicle = await _VehicleRepository.UpdateVehicle(VehicleId, requestModel);

            if (insertedVehicle <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeleteVehicle/{VehicleId}")]
        public async Task<IActionResult> DeleteVehicle(int VehicleId, [FromBody] VehicleRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion

            VehicleRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("Vehicle_Null");

            var requestProcessConfirm = await _VehicleRepository.GetVehicleById(VehicleId);
            if (requestProcessConfirm == null)
                return NotFound("Vehicle_NotFoundId : "+ VehicleId);

            int insertedVehicle = await _VehicleRepository.DeleteVehicle(VehicleId, requestModel);

            if (insertedVehicle <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }
    }
}
