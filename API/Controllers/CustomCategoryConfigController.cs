using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.CustomCategoryConfig;
using Infrastructure.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Persistence.Repository;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomCategoryConfigController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<CustomCategoryConfigController> _logger;
        private readonly ICustomCategoryConfigRepository _CustomCategoryConfigRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private ResponseDto _responseDto = new ResponseDto();

        public CustomCategoryConfigController(SecurityHelper securityHelper,
            ILogger<CustomCategoryConfigController> logger,
            ICustomCategoryConfigRepository CustomCategoryConfigRepository,
            UserManager<ApplicationUser> userManager
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._CustomCategoryConfigRepository = CustomCategoryConfigRepository;
            this._userManager = userManager;
        }

        [HttpPost("GetCustomCategoryConfig")]
        public async Task<IActionResult> GetCustomCategoryConfig(int pageNumber, [FromBody] CustomCategoryConfigFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("CustomCategoryConfig_InvalidPageNumber : "+ pageNumber);

            var result = await _CustomCategoryConfigRepository.GetCustomCategoryConfigs(pageNumber, searchModel);
            if (result == null)
                return NotFound("CustomCategoryConfig_NotFoundList");

            return Ok(result);
        }

        [HttpPost("GetAllCustomCategoryConfigs")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDistinctCustomCategoryConfig([FromBody] CustomCategoryConfigFilterDto searchModel)
        {
            var result = await _CustomCategoryConfigRepository.GetDistinctCustomCategoryConfigs(searchModel);
            if (result == null)
                return NotFound("CustomCategoryConfig_NotFoundList");

            return Ok(result);
        }


        [HttpGet("GetCustomCategoryConfigById/{id}")]
        public async Task<IActionResult> GetCustomCategoryConfigById(int id)
        {

            if (id==0)
                return BadRequest("CustomCategoryConfig_InvalidId : "+ id);

            var result = await _CustomCategoryConfigRepository.GetCustomCategoryConfigById(id);
            if (result == null)
                return NotFound("CustomCategoryConfig_NotFoundId : "+ id);

            return Ok(result);
        }


        [HttpPost("InsertCustomCategoryConfig")]
        public async Task<IActionResult> InsertCustomCategoryConfig([FromBody] CustomCategoryConfigRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("CustomCategoryConfig_Null");

                int insertedCustomCategoryConfigId = await _CustomCategoryConfigRepository.InsertCustomCategoryConfig(requestModel);
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



        [HttpPost("UpdateCustomCategoryConfig/{CustomCategoryConfigId}")]
        public async Task<IActionResult> UpdateCustomCategoryConfig(int CustomCategoryConfigId, [FromBody] CustomCategoryConfigRequestDto updateRequestModel)
        {

            CustomCategoryConfigRequestDto requestModel = updateRequestModel;
            if (requestModel == null)
                return BadRequest("CustomCategoryConfig_Null");

            var requestProcessConfirm = await _CustomCategoryConfigRepository.GetCustomCategoryConfigById(CustomCategoryConfigId);
            if (requestProcessConfirm == null)
                return NotFound("CustomCategoryConfig_NotFoundId : "+ CustomCategoryConfigId);

            int insertedCustomCategoryConfig = await _CustomCategoryConfigRepository.UpdateCustomCategoryConfig(CustomCategoryConfigId, requestModel);

            if (insertedCustomCategoryConfig <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeleteCustomCategoryConfig/{CustomCategoryConfigId}")]
        public async Task<IActionResult> DeleteCustomCategoryConfig(int CustomCategoryConfigId, [FromBody] CustomCategoryConfigRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion

            CustomCategoryConfigRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("CustomCategoryConfig_Null");

            var requestProcessConfirm = await _CustomCategoryConfigRepository.GetCustomCategoryConfigById(CustomCategoryConfigId);
            if (requestProcessConfirm == null)
                return NotFound("CustomCategoryConfig_NotFoundId : "+ CustomCategoryConfigId);

            int insertedCustomCategoryConfig = await _CustomCategoryConfigRepository.DeleteCustomCategoryConfig(CustomCategoryConfigId, requestModel);

            if (insertedCustomCategoryConfig <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }
    }
}
