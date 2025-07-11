using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.Package;
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
    public class PackageController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<PackageController> _logger;
        private readonly IPackageRepository _PackageRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private ResponseDto _responseDto = new ResponseDto();

        public PackageController(SecurityHelper securityHelper,
            ILogger<PackageController> logger,
            IPackageRepository PackageRepository,
            UserManager<ApplicationUser> userManager
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._PackageRepository = PackageRepository;
            this._userManager = userManager;
        }

        [HttpPost("GetPackage")]
        public async Task<IActionResult> GetPackage(int pageNumber, [FromBody] PackageFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("Package_InvalidPageNumber : "+ pageNumber);

            var result = await _PackageRepository.GetPackages(pageNumber, searchModel);
            if (result == null)
                return NotFound("Package_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetAllPackages")]
        public async Task<IActionResult> GetDistinctPackage()
        {
            var result = await _PackageRepository.GetDistinctPackages();
            if (result == null)
                return NotFound("Package_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetPackageById/{id}")]
        public async Task<IActionResult> GetPackageById(int id)
        {

            if (id==0)
                return BadRequest("Package_InvalidId : "+ id);

            var result = await _PackageRepository.GetPackageById(id);
            if (result == null)
                return NotFound("Package_NotFoundId : "+ id);

            return Ok(result);
        }


        [HttpPost("InsertPackage")]
        public async Task<IActionResult> InsertPackage([FromBody] PackageRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("Package_Null");

                int insertedPackageId = await _PackageRepository.InsertPackage(requestModel);
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



        [HttpPost("UpdatePackage/{PackageId}")]
        public async Task<IActionResult> UpdatePackage(int PackageId, [FromBody] PackageRequestDto updateRequestModel)
        {

            PackageRequestDto requestModel = updateRequestModel;
            if (requestModel == null)
                return BadRequest("Package_Null");

            var requestProcessConfirm = await _PackageRepository.GetPackageById(PackageId);
            if (requestProcessConfirm == null)
                return NotFound("Package_NotFoundId : "+ PackageId);

            int insertedPackage = await _PackageRepository.UpdatePackage(PackageId, requestModel);

            if (insertedPackage <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeletePackage/{PackageId}")]
        public async Task<IActionResult> DeletePackage(int PackageId, [FromBody] PackageRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion

            PackageRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("Package_Null");

            var requestProcessConfirm = await _PackageRepository.GetPackageById(PackageId);
            if (requestProcessConfirm == null)
                return NotFound("Package_NotFoundId : "+ PackageId);

            int insertedPackage = await _PackageRepository.DeletePackage(PackageId, requestModel);

            if (insertedPackage <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }
    }
}
