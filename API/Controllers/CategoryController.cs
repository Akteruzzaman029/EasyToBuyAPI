using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.Category;
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
    public class CategoryController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryRepository _CategoryRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private ResponseDto _responseDto = new ResponseDto();

        public CategoryController(SecurityHelper securityHelper,
            ILogger<CategoryController> logger,
            ICategoryRepository CategoryRepository,
            UserManager<ApplicationUser> userManager
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._CategoryRepository = CategoryRepository;
            this._userManager = userManager;
        }

        [HttpPost("GetCategory")]
        public async Task<IActionResult> GetCategory(int pageNumber, [FromBody] CategoryFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("Category_InvalidPageNumber : "+ pageNumber);

            var result = await _CategoryRepository.GetCategories(pageNumber, searchModel);
            if (result == null)
                return NotFound("Category_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetAllCategories/{parentId}")]
        public async Task<IActionResult> GetDistinctCategory(int parentId)
        {
            var result = await _CategoryRepository.GetDistinctCategories(parentId);
            if (result == null)
                return NotFound("Category_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetCategoryById/{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {

            if (id==0)
                return BadRequest("Category_InvalidId : "+ id);

            var result = await _CategoryRepository.GetCategoryById(id);
            if (result == null)
                return NotFound("Category_NotFoundId : "+ id);

            return Ok(result);
        }


        [HttpPost("InsertCategory")]
        public async Task<IActionResult> InsertCategory([FromBody] CategoryRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("Category_Null");

                int insertedCategoryId = await _CategoryRepository.InsertCategory(requestModel);
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



        [HttpPost("UpdateCategory/{categoryId}")]
        public async Task<IActionResult> UpdateCategory(int categoryId, [FromBody] CategoryRequestDto updateRequestModel)
        {

            CategoryRequestDto requestModel = updateRequestModel;
            if (requestModel == null)
                return BadRequest("Category_Null");

            var requestProcessConfirm = await _CategoryRepository.GetCategoryById(categoryId);
            if (requestProcessConfirm == null)
                return NotFound("Category_NotFoundId : "+ categoryId);

            int insertedCategory = await _CategoryRepository.UpdateCategory(categoryId, requestModel);

            if (insertedCategory <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeleteCategory/{categoryId}")]
        public async Task<IActionResult> DeleteCategory(int categoryId, [FromBody] CategoryRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion

            CategoryRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("Category_Null");

            var requestProcessConfirm = await _CategoryRepository.GetCategoryById(categoryId);
            if (requestProcessConfirm == null)
                return NotFound("Category_NotFoundId : "+ categoryId);

            int insertedCategory = await _CategoryRepository.DeleteCategory(categoryId, requestModel);

            if (insertedCategory <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }
    }
}
