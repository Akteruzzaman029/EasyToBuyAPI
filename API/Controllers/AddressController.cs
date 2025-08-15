using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.Address;
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
    public class AddressController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<AddressController> _logger;
        private readonly IAddressRepository _AddressRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<NotificationHub> _hubContext;
        private ResponseDto _responseDto = new ResponseDto();

        public AddressController(SecurityHelper securityHelper,
            ILogger<AddressController> logger,
            IAddressRepository AddressRepository,
            UserManager<ApplicationUser> userManager,
            IHubContext<NotificationHub> hubContext
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._AddressRepository = AddressRepository;
            this._userManager = userManager;
            this._hubContext = hubContext;
        }

        [HttpPost("GetAddress")]
        public async Task<IActionResult> GetAddress(int pageNumber, [FromBody] AddressFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("Address_InvalidPageNumber : "+ pageNumber);

            var result = await _AddressRepository.GetAddresss(pageNumber, searchModel);
            if (result == null)
                return NotFound("Address_NotFoundList");

            return Ok(result);
        }

        [HttpPost("GetAllAddresss")]
        public async Task<IActionResult> GetDistinctAddress([FromBody] AddressFilterDto searchModel)
        {
            var result = await _AddressRepository.GetDistinctAddresss(searchModel);
            if (result == null)
                return NotFound("Address_NotFoundList");

            return Ok(result);
        }


        [HttpGet("GetAddressById/{id}")]
        public async Task<IActionResult> GetAddressById(int id)
        {

            if (id==0)
                return BadRequest("Address_InvalidId : "+ id);

            var result = await _AddressRepository.GetAddressById(id);
            if (result == null)
                return NotFound("Address_NotFoundId : "+ id);

            return Ok(result);
        }


        [HttpPost("InsertAddress")]
        public async Task<IActionResult> InsertAddress([FromBody] AddressRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("Address_Null");

                int insertedAddressId = await _AddressRepository.InsertAddress(requestModel);
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



        [HttpPost("UpdateAddress/{AddressId}")]
        public async Task<IActionResult> UpdateAddress(int AddressId, [FromBody] AddressRequestDto updateRequestModel)
        {

            AddressRequestDto requestModel = updateRequestModel;
            if (requestModel == null)
                return BadRequest("Address_Null");

            var requestProcessConfirm = await _AddressRepository.GetAddressById(AddressId);
            if (requestProcessConfirm == null)
                return NotFound("Address_NotFoundId : "+ AddressId);

            int insertedAddress = await _AddressRepository.UpdateAddress(AddressId, requestModel);

            if (insertedAddress <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeleteAddress/{AddressId}")]
        public async Task<IActionResult> DeleteAddress(int AddressId, [FromBody] AddressRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion

            AddressRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("Address_Null");

            var requestProcessConfirm = await _AddressRepository.GetAddressById(AddressId);
            if (requestProcessConfirm == null)
                return NotFound("Address_NotFoundId : "+ AddressId);

            int insertedAddress = await _AddressRepository.DeleteAddress(AddressId, requestModel);

            if (insertedAddress <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }
    }
}
