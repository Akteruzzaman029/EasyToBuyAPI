using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.Company;
using Infrastructure.IRepository;
using Microsoft.AspNetCore.Authorization;
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
    public class CompanyController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<CompanyController> _logger;
        private readonly ICompanyRepository _CompanyRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<NotificationHub> _hubContext;
        private ResponseDto _responseDto = new ResponseDto();

        public CompanyController(SecurityHelper securityHelper,
            ILogger<CompanyController> logger,
            ICompanyRepository CompanyRepository,
            UserManager<ApplicationUser> userManager,
            IHubContext<NotificationHub> hubContext
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._CompanyRepository = CompanyRepository;
            this._userManager = userManager;
            this._hubContext = hubContext;
        }

        [HttpPost("GetCompany")]
        public async Task<IActionResult> GetCompany(int pageNumber, [FromBody] CompanyFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("Company_InvalidPageNumber : "+ pageNumber);

            var result = await _CompanyRepository.GetCompanys(pageNumber, searchModel);
            if (result == null)
                return NotFound("Company_NotFoundList");

            return Ok(result);
        }

        [HttpPost("GetAllCompanys")]
        public async Task<IActionResult> GetDistinctCompany([FromBody] CompanyFilterDto searchModel)
        {
            var result = await _CompanyRepository.GetDistinctCompanys(searchModel);
            if (result == null)
                return NotFound("Company_NotFoundList");

            return Ok(result);
        }


        [HttpGet("GetCompanyById/{id}")]
        public async Task<IActionResult> GetCompanyById(int id)
        {

            if (id==0)
                return BadRequest("Company_InvalidId : "+ id);

            var result = await _CompanyRepository.GetCompanyById(id);
            if (result == null)
                return NotFound("Company_NotFoundId : "+ id);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("GetCompanyByCode/{code}")]
        public async Task<IActionResult> GetCompanyByCode(string code)
        {

            if (string.IsNullOrWhiteSpace(code))
                return BadRequest("Company_InvalidCode : "+ code);

            var result = await _CompanyRepository.GetCompanyByCode(code);
            if (result == null)
                return NotFound("Company_NotFoundCode : "+ code);

            return Ok(result);
        }



        [HttpPost("InsertCompany")]
        public async Task<IActionResult> InsertCompany([FromBody] CompanyRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("Company_Null");

                int insertedCompanyId = await _CompanyRepository.InsertCompany(requestModel);
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



        [HttpPost("UpdateCompany/{CompanyId}")]
        public async Task<IActionResult> UpdateCompany(int CompanyId, [FromBody] CompanyRequestDto updateRequestModel)
        {

            CompanyRequestDto requestModel = updateRequestModel;
            if (requestModel == null)
                return BadRequest("Company_Null");

            var requestProcessConfirm = await _CompanyRepository.GetCompanyById(CompanyId);
            if (requestProcessConfirm == null)
                return NotFound("Company_NotFoundId : "+ CompanyId);

            int insertedCompany = await _CompanyRepository.UpdateCompany(CompanyId, requestModel);

            if (insertedCompany <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeleteCompany/{CompanyId}")]
        public async Task<IActionResult> DeleteCompany(int CompanyId, [FromBody] CompanyRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion

            CompanyRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("Company_Null");

            var requestProcessConfirm = await _CompanyRepository.GetCompanyById(CompanyId);
            if (requestProcessConfirm == null)
                return NotFound("Company_NotFoundId : "+ CompanyId);

            int insertedCompany = await _CompanyRepository.DeleteCompany(CompanyId, requestModel);

            if (insertedCompany <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }
    }
}
