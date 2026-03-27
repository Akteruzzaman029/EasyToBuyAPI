using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.CustomCategory;
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
    public class CustomCategoryController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<CustomCategoryController> _logger;
        private readonly ICustomCategoryRepository _CustomCategoryRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private ResponseDto _responseDto = new ResponseDto();

        public CustomCategoryController(SecurityHelper securityHelper,
            ILogger<CustomCategoryController> logger,
            ICustomCategoryRepository CustomCategoryRepository,
            UserManager<ApplicationUser> userManager
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._CustomCategoryRepository = CustomCategoryRepository;
            this._userManager = userManager;
        }

        [HttpPost("GetCustomCategory")]
        public async Task<IActionResult> GetCustomCategory(int pageNumber, [FromBody] CustomCategoryFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("CustomCategory_InvalidPageNumber : "+ pageNumber);

            var result = await _CustomCategoryRepository.GetCustomCategories(pageNumber, searchModel);
            if (result == null)
                return NotFound("CustomCategory_NotFoundList");

            return Ok(result);
        }

        [HttpPost("GetAllCustomCategories")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDistinctCustomCategory([FromBody] CustomCategoryFilterDto searchModel)
        {
            var result = await _CustomCategoryRepository.GetDistinctCustomCategories(searchModel);
            if (result == null)
                return NotFound("CustomCategory_NotFoundList");

            return Ok(result);
        }


        [HttpGet("GetCustomCategoryById/{id}")]
        public async Task<IActionResult> GetCustomCategoryById(int id)
        {

            if (id==0)
                return BadRequest("CustomCategory_InvalidId : "+ id);

            var result = await _CustomCategoryRepository.GetCustomCategoryById(id);
            if (result == null)
                return NotFound("CustomCategory_NotFoundId : "+ id);

            return Ok(result);
        }


        [HttpPost("InsertCustomCategory")]
        public async Task<IActionResult> InsertCustomCategory([FromBody] CustomCategoryRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("CustomCategory_Null");

                int insertedCustomCategoryId = await _CustomCategoryRepository.InsertCustomCategory(requestModel);
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



        [HttpPost("UpdateCustomCategory/{CustomCategoryId}")]
        public async Task<IActionResult> UpdateCustomCategory(int CustomCategoryId, [FromBody] CustomCategoryRequestDto updateRequestModel)
        {

            CustomCategoryRequestDto requestModel = updateRequestModel;
            if (requestModel == null)
                return BadRequest("CustomCategory_Null");

            var requestProcessConfirm = await _CustomCategoryRepository.GetCustomCategoryById(CustomCategoryId);
            if (requestProcessConfirm == null)
                return NotFound("CustomCategory_NotFoundId : "+ CustomCategoryId);

            int insertedCustomCategory = await _CustomCategoryRepository.UpdateCustomCategory(CustomCategoryId, requestModel);

            if (insertedCustomCategory <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeleteCustomCategory/{CustomCategoryId}")]
        public async Task<IActionResult> DeleteCustomCategory(int CustomCategoryId, [FromBody] CustomCategoryRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion

            CustomCategoryRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("CustomCategory_Null");

            var requestProcessConfirm = await _CustomCategoryRepository.GetCustomCategoryById(CustomCategoryId);
            if (requestProcessConfirm == null)
                return NotFound("CustomCategory_NotFoundId : "+ CustomCategoryId);

            int insertedCustomCategory = await _CustomCategoryRepository.DeleteCustomCategory(CustomCategoryId, requestModel);

            if (insertedCustomCategory <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }
    }
}
