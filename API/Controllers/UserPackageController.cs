using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.UserPackage;
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
    public class UserPackageController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<UserPackageController> _logger;
        private readonly IUserPackageRepository _UserPackageRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private ResponseDto _responseDto = new ResponseDto();

        public UserPackageController(SecurityHelper securityHelper,
            ILogger<UserPackageController> logger,
            IUserPackageRepository UserPackageRepository,
            UserManager<ApplicationUser> userManager
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._UserPackageRepository = UserPackageRepository;
            this._userManager = userManager;
        }

        [HttpPost("GetUserPackage")]
        public async Task<IActionResult> GetUserPackage(int pageNumber, [FromBody] UserPackageFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("UserPackage_InvalidPageNumber : "+ pageNumber);

            var result = await _UserPackageRepository.GetUserPackages(pageNumber, searchModel);
            if (result == null)
                return NotFound("UserPackage_NotFoundList");

            return Ok(result);
        }
       
        [HttpPost("GetUserPackageDueList")]
        public async Task<IActionResult> GetUserPackageDueList([FromBody] UserPackageFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            var result = await _UserPackageRepository.GetUserPackagesDueList(searchModel);
            if (result == null)
                return NotFound("DueList_NotFoundList");

            return Ok(result);
        }


        [HttpPost("GetProfitAndLoss")]
        public async Task<IActionResult> GetProfitAndLoss([FromBody] UserPackageFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            var result = await _UserPackageRepository.GetProfitAndLoss(searchModel);
            if (result == null)
                return NotFound("DueList_NotFoundList");

            return Ok(result);
        }

        [HttpPost("GetProfitAndLossDetail")]
        public async Task<IActionResult> GetProfitAndLossDetail([FromBody] UserPackageFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            var result = await _UserPackageRepository.GetProfitAndLossDetail(searchModel);
            if (result == null)
                return NotFound("DueList_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetAllUserPackagees/{companyId}")]
        public async Task<IActionResult> GetDistinctUserPackage(int companyId)
        {
            var result = await _UserPackageRepository.GetDistinctUserPackages(companyId);
            if (result == null)
                return NotFound("UserPackage_NotFoundList");

            return Ok(result);
        }


        [HttpGet("GetUserPackageByStudentId/{id}")]
        public async Task<IActionResult> GetUserPackageByStudentId(int id)
        {

            if (id == 0)
                return BadRequest("UserPackage_Invalid_studentId : " + id);

            var result = await _UserPackageRepository.GetUserPackageByStudentId(id);
            if (result == null)
                return NotFound("UserPackage_NotFound_StudentId : " + id);

            return Ok(result);
        }


        [HttpGet("GetUserPackageById/{id}")]
        public async Task<IActionResult> GetUserPackageById(int id)
        {

            if (id==0)
                return BadRequest("UserPackage_InvalidId : "+ id);

            var result = await _UserPackageRepository.GetUserPackageById(id);
            if (result == null)
                return NotFound("UserPackage_NotFoundId : "+ id);

            return Ok(result);
        }


        [HttpPost("InsertUserPackage")]
        public async Task<IActionResult> InsertUserPackage([FromBody] UserPackageRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("UserPackage_Null");

                int insertedUserPackageId = await _UserPackageRepository.InsertUserPackage(requestModel);
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



        [HttpPost("UpdateUserPackage/{UserPackageId}")]
        public async Task<IActionResult> UpdateUserPackage(int UserPackageId, [FromBody] UserPackageRequestDto updateRequestModel)
        {

            UserPackageRequestDto requestModel = updateRequestModel;
            if (requestModel == null)
                return BadRequest("UserPackage_Null");

            var requestProcessConfirm = await _UserPackageRepository.GetUserPackageById(UserPackageId);
            if (requestProcessConfirm == null)
                return NotFound("UserPackage_NotFoundId : "+ UserPackageId);

            int insertedUserPackage = await _UserPackageRepository.UpdateUserPackage(UserPackageId, requestModel);

            if (insertedUserPackage <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeleteUserPackage/{UserPackageId}")]
        public async Task<IActionResult> DeleteUserPackage(int UserPackageId, [FromBody] UserPackageRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion

            UserPackageRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("UserPackage_Null");

            var requestProcessConfirm = await _UserPackageRepository.GetUserPackageById(UserPackageId);
            if (requestProcessConfirm == null)
                return NotFound("UserPackage_NotFoundId : "+ UserPackageId);

            int insertedUserPackage = await _UserPackageRepository.DeleteUserPackage(UserPackageId, requestModel);

            if (insertedUserPackage <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }
    }
}
