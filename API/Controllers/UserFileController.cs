using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.UserFile;
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
    public class UserFileController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<UserFileController> _logger;
        private readonly IUserFileRepository _UserFileRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private ResponseDto _responseDto = new ResponseDto();

        public UserFileController(SecurityHelper securityHelper,
            ILogger<UserFileController> logger,
            IUserFileRepository UserFileRepository,
            UserManager<ApplicationUser> userManager
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._UserFileRepository = UserFileRepository;
            this._userManager = userManager;
        }

        [HttpPost("GetUserFile")]
        public async Task<IActionResult> GetUserFile(int pageNumber, [FromBody] UserFileFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("UserFile_InvalidPageNumber : "+ pageNumber);

            var result = await _UserFileRepository.GetUserFiles(pageNumber, searchModel);
            if (result == null)
                return NotFound("UserFile_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetAllUserFiles/{userId}")]
        public async Task<IActionResult> GetDistinctUserFile(string userId)
        {
            var result = await _UserFileRepository.GetDistinctUserFiles(userId);
            if (result == null)
                return NotFound("UserFile_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetUserFileById/{id}")]
        public async Task<IActionResult> GetUserFileById(int id)
        {

            if (id==0)
                return BadRequest("UserFile_InvalidId : "+ id);

            var result = await _UserFileRepository.GetUserFileById(id);
            if (result == null)
                return NotFound("UserFile_NotFoundId : "+ id);

            return Ok(result);
        }


        [HttpPost("InsertUserFile")]
        public async Task<IActionResult> InsertUserFile([FromBody] UserFileRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("UserFile_Null");

                int insertedUserFileId = await _UserFileRepository.InsertUserFile(requestModel);
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



        [HttpPost("UpdateUserFile/{userfileId}")]
        public async Task<IActionResult> UpdateUserFile(int userfileId, [FromBody] UserFileRequestDto updateRequestModel)
        {

            UserFileRequestDto requestModel = updateRequestModel;
            if (requestModel == null)
                return BadRequest("UserFile_Null");

            var requestProcessConfirm = await _UserFileRepository.GetUserFileById(userfileId);
            if (requestProcessConfirm == null)
                return NotFound("UserFile_NotFoundId : "+ userfileId);

            int insertedUserFile = await _UserFileRepository.UpdateUserFile(userfileId, requestModel);

            if (insertedUserFile <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeleteUserFile/{userfileId}")]
        public async Task<IActionResult> DeleteUserFile(int userfileId, [FromBody] UserFileRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion

            UserFileRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("UserFile_Null");

            var requestProcessConfirm = await _UserFileRepository.GetUserFileById(userfileId);
            if (requestProcessConfirm == null)
                return NotFound("UserFile_NotFoundId : "+ userfileId);

            int insertedUserFile = await _UserFileRepository.DeleteUserFile(userfileId, requestModel);

            if (insertedUserFile <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }
    }
}
