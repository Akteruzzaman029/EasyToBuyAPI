using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.CheckList;
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
    public class CheckListController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<CheckListController> _logger;
        private readonly ICheckListRepository _CheckListRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private ResponseDto _responseDto = new ResponseDto();

        public CheckListController(SecurityHelper securityHelper,
            ILogger<CheckListController> logger,
            ICheckListRepository CheckListRepository,
            UserManager<ApplicationUser> userManager
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._CheckListRepository = CheckListRepository;
            this._userManager = userManager;
        }

        [HttpPost("GetCheckList")]
        public async Task<IActionResult> GetCheckList(int pageNumber, [FromBody] CheckListFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("CheckList_InvalidPageNumber : "+ pageNumber);

            var result = await _CheckListRepository.GetCheckLists(pageNumber, searchModel);
            if (result == null)
                return NotFound("CheckList_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetAllCheckListes")]
        public async Task<IActionResult> GetDistinctCheckList()
        {
            var result = await _CheckListRepository.GetDistinctCheckLists();
            if (result == null)
                return NotFound("CheckList_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetCheckListById/{id}")]
        public async Task<IActionResult> GetCheckListById(int id)
        {

            if (id==0)
                return BadRequest("CheckList_InvalidId : "+ id);

            var result = await _CheckListRepository.GetCheckListById(id);
            if (result == null)
                return NotFound("CheckList_NotFoundId : "+ id);

            return Ok(result);
        }


        [HttpPost("InsertCheckList")]
        public async Task<IActionResult> InsertCheckList([FromBody] CheckListRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("CheckList_Null");

                int insertedCheckListId = await _CheckListRepository.InsertCheckList(requestModel);
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



        [HttpPost("UpdateCheckList/{CheckListId}")]
        public async Task<IActionResult> UpdateCheckList(int CheckListId, [FromBody] CheckListRequestDto updateRequestModel)
        {

            CheckListRequestDto requestModel = updateRequestModel;
            if (requestModel == null)
                return BadRequest("CheckList_Null");

            var requestProcessConfirm = await _CheckListRepository.GetCheckListById(CheckListId);
            if (requestProcessConfirm == null)
                return NotFound("CheckList_NotFoundId : "+ CheckListId);

            int insertedCheckList = await _CheckListRepository.UpdateCheckList(CheckListId, requestModel);

            if (insertedCheckList <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeleteCheckList/{CheckListId}")]
        public async Task<IActionResult> DeleteCheckList(int CheckListId, [FromBody] CheckListRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion

            CheckListRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("CheckList_Null");

            var requestProcessConfirm = await _CheckListRepository.GetCheckListById(CheckListId);
            if (requestProcessConfirm == null)
                return NotFound("CheckList_NotFoundId : "+ CheckListId);

            int insertedCheckList = await _CheckListRepository.DeleteCheckList(CheckListId, requestModel);

            if (insertedCheckList <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }
    }
}
