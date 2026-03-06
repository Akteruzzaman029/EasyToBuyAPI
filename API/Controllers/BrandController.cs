using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.Brand;
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
    public class BrandController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<BrandController> _logger;
        private readonly IBrandRepository _BrandRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private ResponseDto _responseDto = new ResponseDto();

        public BrandController(SecurityHelper securityHelper,
            ILogger<BrandController> logger,
            IBrandRepository BrandRepository,
            UserManager<ApplicationUser> userManager
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._BrandRepository = BrandRepository;
            this._userManager = userManager;
        }

        [HttpPost("GetBrand")]
        public async Task<IActionResult> GetBrand(int pageNumber, [FromBody] BrandFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("Brand_InvalidPageNumber : "+ pageNumber);

            var result = await _BrandRepository.GetCategories(pageNumber, searchModel);
            if (result == null)
                return NotFound("Brand_NotFoundList");

            return Ok(result);
        }

        [HttpPost("GetAllCategories")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDistinctBrand([FromBody] BrandFilterDto searchModel)
        {
            var result = await _BrandRepository.GetDistinctCategories(searchModel);
            if (result == null)
                return NotFound("Brand_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetBrandById/{id}")]
        public async Task<IActionResult> GetBrandById(int id)
        {

            if (id==0)
                return BadRequest("Brand_InvalidId : "+ id);

            var result = await _BrandRepository.GetBrandById(id);
            if (result == null)
                return NotFound("Brand_NotFoundId : "+ id);

            return Ok(result);
        }


        [HttpPost("InsertBrand")]
        public async Task<IActionResult> InsertBrand([FromBody] BrandRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("Brand_Null");

                int insertedBrandId = await _BrandRepository.InsertBrand(requestModel);
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



        [HttpPost("UpdateBrand/{brandId}")]
        public async Task<IActionResult> UpdateBrand(int brandId, [FromBody] BrandRequestDto updateRequestModel)
        {

            BrandRequestDto requestModel = updateRequestModel;
            if (requestModel == null)
                return BadRequest("Brand_Null");

            var requestProcessConfirm = await _BrandRepository.GetBrandById(brandId);
            if (requestProcessConfirm == null)
                return NotFound("Brand_NotFoundId : "+ brandId);

            int insertedBrand = await _BrandRepository.UpdateBrand(brandId, requestModel);

            if (insertedBrand <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeleteBrand/{brandId}")]
        public async Task<IActionResult> DeleteBrand(int brandId, [FromBody] BrandRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion

            BrandRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("Brand_Null");

            var requestProcessConfirm = await _BrandRepository.GetBrandById(brandId);
            if (requestProcessConfirm == null)
                return NotFound("Brand_NotFoundId : "+ brandId);

            int insertedBrand = await _BrandRepository.DeleteBrand(brandId, requestModel);

            if (insertedBrand <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }
    }
}
