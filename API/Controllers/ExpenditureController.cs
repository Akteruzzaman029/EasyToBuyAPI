using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.Expenditure;
using Core.ModelDto.ExpenditureHead;
using Core.ModelDto.UploadedFile;
using Infrastructure.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Persistence.Repository;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenditureController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<ExpenditureController> _logger;
        private readonly IExpenditureRepository _ExpenditureRepository;
        private readonly IUploadedFileRepository _UploadedFileRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private ResponseDto _responseDto = new ResponseDto();

        public ExpenditureController(SecurityHelper securityHelper,
            ILogger<ExpenditureController> logger,
             IUploadedFileRepository UploadedFileRepository,
            IExpenditureRepository ExpenditureRepository,
            UserManager<ApplicationUser> userManager
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._UploadedFileRepository = UploadedFileRepository;
            this._ExpenditureRepository = ExpenditureRepository;
            this._userManager = userManager;
        }

        [HttpPost("GetExpenditure")]
        public async Task<IActionResult> GetExpenditure(int pageNumber, [FromBody] ExpenditureFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("Expenditure_InvalidPageNumber : "+ pageNumber);

            var result = await _ExpenditureRepository.GetExpenditures(pageNumber, searchModel);
            if (result == null)
                return NotFound("Expenditure_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetAllExpenditurees")]
        public async Task<IActionResult> GetDistinctExpenditure()
        {
            var result = await _ExpenditureRepository.GetDistinctExpenditures();
            if (result == null)
                return NotFound("Expenditure_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetExpenditureById/{id}")]
        public async Task<IActionResult> GetExpenditureById(int id)
        {

            if (id==0)
                return BadRequest("Expenditure_InvalidId : "+ id);

            var result = await _ExpenditureRepository.GetExpenditureById(id);
            if (result == null)
                return NotFound("Expenditure_NotFoundId : "+ id);

            return Ok(result);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file, [FromForm] string model)
        {
            if (file == null || string.IsNullOrEmpty(model))
                return BadRequest("Missing file or model");

            // Deserialize the model
           

            // Save file if needed
            var filePath = Path.Combine("Uploads", file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Do something with the model (e.g., save to DB)

            return Ok(new { message = "File and model uploaded successfully." });
        }

        [HttpPost("InsertExpenditure")]
        public async Task<IActionResult> InsertExpenditure([FromBody] ExpenditureRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("Expenditure_Null");
                await _ExpenditureRepository.InsertExpenditure(requestModel);

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



        [HttpPost("UpdateExpenditure/{ExpenditureId}")]
        public async Task<IActionResult> UpdateExpenditure(int ExpenditureId, [FromBody] ExpenditureRequestDto requestModel)
        {
            if (requestModel == null)
                return BadRequest("Expenditure_Null");

            var requestProcessConfirm = await _ExpenditureRepository.GetExpenditureById(ExpenditureId);
            if (requestProcessConfirm == null)
                return NotFound("Expenditure_NotFoundId : " + ExpenditureId);

            int insertedExpenditure = await _ExpenditureRepository.UpdateExpenditure(ExpenditureId, requestModel);
            if (insertedExpenditure <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeleteExpenditure/{ExpenditureId}")]
        public async Task<IActionResult> DeleteExpenditure(int ExpenditureId, [FromBody] ExpenditureRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion

            ExpenditureRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("Expenditure_Null");

            var requestProcessConfirm = await _ExpenditureRepository.GetExpenditureById(ExpenditureId);
            if (requestProcessConfirm == null)
                return NotFound("Expenditure_NotFoundId : "+ ExpenditureId);

            int insertedExpenditure = await _ExpenditureRepository.DeleteExpenditure(ExpenditureId, requestModel);

            if (insertedExpenditure <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }
    }
}
