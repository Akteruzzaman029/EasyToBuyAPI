using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.Banner;
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
    public class BannerController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<BannerController> _logger;
        private readonly IBannerRepository _BannerRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private ResponseDto _responseDto = new ResponseDto();

        public BannerController(SecurityHelper securityHelper,
            ILogger<BannerController> logger,
            IBannerRepository BannerRepository,
            UserManager<ApplicationUser> userManager
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._BannerRepository = BannerRepository;
            this._userManager = userManager;
        }

        [HttpPost("GetBanner")]
        public async Task<IActionResult> GetBanner(int pageNumber, [FromBody] BannerFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("Banner_InvalidPageNumber : "+ pageNumber);

            var result = await _BannerRepository.GetCategories(pageNumber, searchModel);
            if (result == null)
                return NotFound("Banner_NotFoundList");

            return Ok(result);
        }

        [HttpPost("GetAllCategories")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDistinctBanner([FromBody] BannerFilterDto searchModel)
        {
            var result = await _BannerRepository.GetDistinctCategories(searchModel);
            if (result == null)
                return NotFound("Banner_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetBannerById/{id}")]
        public async Task<IActionResult> GetBannerById(int id)
        {

            if (id==0)
                return BadRequest("Banner_InvalidId : "+ id);

            var result = await _BannerRepository.GetBannerById(id);
            if (result == null)
                return NotFound("Banner_NotFoundId : "+ id);

            return Ok(result);
        }


        [HttpPost("InsertBanner")]
        public async Task<IActionResult> InsertBanner([FromBody] BannerRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("Banner_Null");

                int insertedBannerId = await _BannerRepository.InsertBanner(requestModel);
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



        [HttpPost("UpdateBanner/{BannerId}")]
        public async Task<IActionResult> UpdateBanner(int BannerId, [FromBody] BannerRequestDto updateRequestModel)
        {

            BannerRequestDto requestModel = updateRequestModel;
            if (requestModel == null)
                return BadRequest("Banner_Null");

            var requestProcessConfirm = await _BannerRepository.GetBannerById(BannerId);
            if (requestProcessConfirm == null)
                return NotFound("Banner_NotFoundId : "+ BannerId);

            int insertedBanner = await _BannerRepository.UpdateBanner(BannerId, requestModel);

            if (insertedBanner <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeleteBanner/{BannerId}")]
        public async Task<IActionResult> DeleteBanner(int BannerId, [FromBody] BannerRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion

            BannerRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("Banner_Null");

            var requestProcessConfirm = await _BannerRepository.GetBannerById(BannerId);
            if (requestProcessConfirm == null)
                return NotFound("Banner_NotFoundId : "+ BannerId);

            int insertedBanner = await _BannerRepository.DeleteBanner(BannerId, requestModel);

            if (insertedBanner <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }
    }
}
