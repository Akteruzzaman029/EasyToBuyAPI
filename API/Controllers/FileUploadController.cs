using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.UploadedFile;
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
    public class UploadedFileController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<UploadedFileController> _logger;
        private readonly IUploadedFileRepository _UploadedFileRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private ResponseDto _responseDto = new ResponseDto();

        public UploadedFileController(SecurityHelper securityHelper,
            ILogger<UploadedFileController> logger,
            IUploadedFileRepository UploadedFileRepository,
            UserManager<ApplicationUser> userManager
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._UploadedFileRepository = UploadedFileRepository;
            this._userManager = userManager;
        }

        [HttpPost("GetUploadedFile")]
        public async Task<IActionResult> GetUploadedFile(int pageNumber, [FromBody] UploadedFileFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("UploadedFile_InvalidPageNumber : "+ pageNumber);

            var result = await _UploadedFileRepository.GetUploadedFiles(pageNumber, searchModel);
            if (result == null)
                return NotFound("UploadedFile_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetAllUploadedFiles/{userId}")]
        public async Task<IActionResult> GetDistinctUploadedFile(string userId)
        {
            var result = await _UploadedFileRepository.GetDistinctUploadedFiles(userId);
            if (result == null)
                return NotFound("UploadedFile_NotFoundList");

            return Ok(result);
        }


        [HttpPost("Upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            // Convert file to byte array (VARBINARY)
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);

                //var userName = User.Identity?.Name;
                //var user = await _userManager.FindByNameAsync(userName);
                var requestModel = new UploadedFileRequestDto
                {
                    UserId = "bb4bbf26-5bae-4f1d-b0e1-4e9d6d7fc547",  // Assuming UserId is passed through query params
                    FileName = file.FileName,
                    FileData = memoryStream.ToArray(),  // Store the file as a binary stream
                    FileSize = file.Length,
                    ContentType = file.ContentType
                };

                int insertedUploadedFileId = await _UploadedFileRepository.InsertUploadedFile(requestModel);

                return Ok(new { Id=insertedUploadedFileId, requestModel.FileName, requestModel.FileSize });
            }
        }

        [HttpGet("GetImage/{id}")]
        public async Task<IActionResult> GetImage(int id)
        {
            if (id==0)
                return BadRequest("UploadedFile_InvalidId : "+ id);
            var file = await _UploadedFileRepository.GetUploadedFileById(id);

            if (file == null)
                return NotFound();

            return File(file.FileData, file.ContentType);
        }

        [HttpGet("download/{id}")]
        public async Task<IActionResult> DownloadFile(int id)
        {

            if (id==0)
                return BadRequest("UploadedFile_InvalidId : "+ id);
            var file = await _UploadedFileRepository.GetUploadedFileById(id);

            if (file == null)
                return NotFound();

            return File(file.FileData, file.ContentType, file.FileName);
        }

        [HttpGet("GetUploadedFileById/{id}")]
        public async Task<IActionResult> GetUploadedFileById(int id)
        {

            if (id==0)
                return BadRequest("UploadedFile_InvalidId : "+ id);

            var result = await _UploadedFileRepository.GetUploadedFileById(id);
            if (result == null)
                return NotFound("UploadedFile_NotFoundId : "+ id);

            return Ok(result);
        }


        [HttpPost("InsertUploadedFile")]
        public async Task<IActionResult> InsertUploadedFile([FromBody] UploadedFileRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("UploadedFile_Null");

                int insertedUploadedFileId = await _UploadedFileRepository.InsertUploadedFile(requestModel);
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



        [HttpPost("UpdateUploadedFile/{UploadedFileId}")]
        public async Task<IActionResult> UpdateUploadedFile(int UploadedFileId, [FromBody] UploadedFileRequestDto updateRequestModel)
        {

            UploadedFileRequestDto requestModel = updateRequestModel;
            if (requestModel == null)
                return BadRequest("UploadedFile_Null");

            var requestProcessConfirm = await _UploadedFileRepository.GetUploadedFileById(UploadedFileId);
            if (requestProcessConfirm == null)
                return NotFound("UploadedFile_NotFoundId : "+ UploadedFileId);

            int insertedUploadedFile = await _UploadedFileRepository.UpdateUploadedFile(UploadedFileId, requestModel);

            if (insertedUploadedFile <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeleteUploadedFile/{UploadedFileId}")]
        public async Task<IActionResult> DeleteUploadedFile(int UploadedFileId, [FromBody] UploadedFileRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion

            UploadedFileRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("UploadedFile_Null");

            var requestProcessConfirm = await _UploadedFileRepository.GetUploadedFileById(UploadedFileId);
            if (requestProcessConfirm == null)
                return NotFound("UploadedFile_NotFoundId : "+ UploadedFileId);

            int insertedUploadedFile = await _UploadedFileRepository.DeleteUploadedFile(UploadedFileId, requestModel);

            if (insertedUploadedFile <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }
    }
}
