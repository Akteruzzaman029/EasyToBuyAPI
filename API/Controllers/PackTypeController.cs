using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.PackType;
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
    public class PackTypeController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<PackTypeController> _logger;
        private readonly IPackTypeRepository _PackTypeRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<NotificationHub> _hubContext;
        private ResponseDto _responseDto = new ResponseDto();

        public PackTypeController(SecurityHelper securityHelper,
            ILogger<PackTypeController> logger,
            IPackTypeRepository PackTypeRepository,
            UserManager<ApplicationUser> userManager,
            IHubContext<NotificationHub> hubContext
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._PackTypeRepository = PackTypeRepository;
            this._userManager = userManager;
            this._hubContext = hubContext;
        }

        [HttpPost("GetPackType")]
        public async Task<IActionResult> GetPackType(int pageNumber, [FromBody] PackTypeFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("PackType_InvalidPageNumber : "+ pageNumber);

            var result = await _PackTypeRepository.GetPackTypes(pageNumber, searchModel);
            if (result == null)
                return NotFound("PackType_NotFoundList");

            return Ok(result);
        }

        [HttpPost("GetAllPackTypes")]
        public async Task<IActionResult> GetDistinctPackType([FromBody] PackTypeFilterDto searchModel)
        {
            var result = await _PackTypeRepository.GetDistinctPackTypes(searchModel);
            if (result == null)
                return NotFound("PackType_NotFoundList");

            return Ok(result);
        }


        [HttpGet("GetPackTypeById/{id}")]
        public async Task<IActionResult> GetPackTypeById(int id)
        {

            if (id==0)
                return BadRequest("PackType_InvalidId : "+ id);

            var result = await _PackTypeRepository.GetPackTypeById(id);
            if (result == null)
                return NotFound("PackType_NotFoundId : "+ id);

            return Ok(result);
        }


        [HttpPost("InsertPackType")]
        public async Task<IActionResult> InsertPackType([FromBody] PackTypeRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("PackType_Null");

                int insertedPackTypeId = await _PackTypeRepository.InsertPackType(requestModel);
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



        [HttpPost("UpdatePackType/{PackTypeId}")]
        public async Task<IActionResult> UpdatePackType(int PackTypeId, [FromBody] PackTypeRequestDto updateRequestModel)
        {

            PackTypeRequestDto requestModel = updateRequestModel;
            if (requestModel == null)
                return BadRequest("PackType_Null");

            var requestProcessConfirm = await _PackTypeRepository.GetPackTypeById(PackTypeId);
            if (requestProcessConfirm == null)
                return NotFound("PackType_NotFoundId : "+ PackTypeId);

            int insertedPackType = await _PackTypeRepository.UpdatePackType(PackTypeId, requestModel);

            if (insertedPackType <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeletePackType/{PackTypeId}")]
        public async Task<IActionResult> DeletePackType(int PackTypeId, [FromBody] PackTypeRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion

            PackTypeRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("PackType_Null");

            var requestProcessConfirm = await _PackTypeRepository.GetPackTypeById(PackTypeId);
            if (requestProcessConfirm == null)
                return NotFound("PackType_NotFoundId : "+ PackTypeId);

            int insertedPackType = await _PackTypeRepository.DeletePackType(PackTypeId, requestModel);

            if (insertedPackType <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }
    }
}
