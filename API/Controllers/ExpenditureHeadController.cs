using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.ExpenditureHead;
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
    public class ExpenditureHeadController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<ExpenditureHeadController> _logger;
        private readonly IExpenditureHeadRepository _ExpenditureHeadRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private ResponseDto _responseDto = new ResponseDto();

        public ExpenditureHeadController(SecurityHelper securityHelper,
            ILogger<ExpenditureHeadController> logger,
            IExpenditureHeadRepository ExpenditureHeadRepository,
            UserManager<ApplicationUser> userManager
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._ExpenditureHeadRepository = ExpenditureHeadRepository;
            this._userManager = userManager;
        }

        [HttpPost("GetExpenditureHead")]
        public async Task<IActionResult> GetExpenditureHead(int pageNumber, [FromBody] ExpenditureHeadFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("ExpenditureHead_InvalidPageNumber : "+ pageNumber);

            var result = await _ExpenditureHeadRepository.GetExpenditureHeads(pageNumber, searchModel);
            if (result == null)
                return NotFound("ExpenditureHead_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetAllExpenditureHeades")]
        public async Task<IActionResult> GetDistinctExpenditureHead()
        {
            var result = await _ExpenditureHeadRepository.GetDistinctExpenditureHeads();
            if (result == null)
                return NotFound("ExpenditureHead_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetExpenditureHeadById/{id}")]
        public async Task<IActionResult> GetExpenditureHeadById(int id)
        {

            if (id==0)
                return BadRequest("ExpenditureHead_InvalidId : "+ id);

            var result = await _ExpenditureHeadRepository.GetExpenditureHeadById(id);
            if (result == null)
                return NotFound("ExpenditureHead_NotFoundId : "+ id);

            return Ok(result);
        }


        [HttpPost("InsertExpenditureHead")]
        public async Task<IActionResult> InsertExpenditureHead([FromBody] ExpenditureHeadRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("ExpenditureHead_Null");

                int insertedExpenditureHeadId = await _ExpenditureHeadRepository.InsertExpenditureHead(requestModel);
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



        [HttpPost("UpdateExpenditureHead/{ExpenditureHeadId}")]
        public async Task<IActionResult> UpdateExpenditureHead(int ExpenditureHeadId, [FromBody] ExpenditureHeadRequestDto updateRequestModel)
        {

            ExpenditureHeadRequestDto requestModel = updateRequestModel;
            if (requestModel == null)
                return BadRequest("ExpenditureHead_Null");

            var requestProcessConfirm = await _ExpenditureHeadRepository.GetExpenditureHeadById(ExpenditureHeadId);
            if (requestProcessConfirm == null)
                return NotFound("ExpenditureHead_NotFoundId : "+ ExpenditureHeadId);

            int insertedExpenditureHead = await _ExpenditureHeadRepository.UpdateExpenditureHead(ExpenditureHeadId, requestModel);

            if (insertedExpenditureHead <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeleteExpenditureHead/{ExpenditureHeadId}")]
        public async Task<IActionResult> DeleteExpenditureHead(int ExpenditureHeadId, [FromBody] ExpenditureHeadRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion

            ExpenditureHeadRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("ExpenditureHead_Null");

            var requestProcessConfirm = await _ExpenditureHeadRepository.GetExpenditureHeadById(ExpenditureHeadId);
            if (requestProcessConfirm == null)
                return NotFound("ExpenditureHead_NotFoundId : "+ ExpenditureHeadId);

            int insertedExpenditureHead = await _ExpenditureHeadRepository.DeleteExpenditureHead(ExpenditureHeadId, requestModel);

            if (insertedExpenditureHead <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }
    }
}
