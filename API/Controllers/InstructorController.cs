using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.Instructor;
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
    public class InstructorController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<InstructorController> _logger;
        private readonly IInstructorRepository _InstructorRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private ResponseDto _responseDto = new ResponseDto();

        public InstructorController(SecurityHelper securityHelper,
            ILogger<InstructorController> logger,
            IInstructorRepository InstructorRepository,
            UserManager<ApplicationUser> userManager
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._InstructorRepository = InstructorRepository;
            this._userManager = userManager;
        }

        [HttpPost("GetInstructor")]
        public async Task<IActionResult> GetInstructor(int pageNumber, [FromBody] InstructorFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("Instructor_InvalidPageNumber : "+ pageNumber);

            var result = await _InstructorRepository.GetInstructors(pageNumber, searchModel);
            if (result == null)
                return NotFound("Instructor_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetAllInstructores")]
        public async Task<IActionResult> GetDistinctInstructor()
        {
            var result = await _InstructorRepository.GetDistinctInstructors();
            if (result == null)
                return NotFound("Instructor_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetInstructorById/{id}")]
        public async Task<IActionResult> GetInstructorById(int id)
        {

            if (id==0)
                return BadRequest("Instructor_InvalidId : "+ id);

            var result = await _InstructorRepository.GetInstructorById(id);
            if (result == null)
                return NotFound("Instructor_NotFoundId : "+ id);

            return Ok(result);
        }


        [HttpPost("InsertInstructor")]
        public async Task<IActionResult> InsertInstructor([FromBody] InstructorRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("Instructor_Null");

                int insertedInstructorId = await _InstructorRepository.InsertInstructor(requestModel);
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



        [HttpPost("UpdateInstructor/{InstructorId}")]
        public async Task<IActionResult> UpdateInstructor(int InstructorId, [FromBody] InstructorRequestDto updateRequestModel)
        {

            InstructorRequestDto requestModel = updateRequestModel;
            if (requestModel == null)
                return BadRequest("Instructor_Null");

            var requestProcessConfirm = await _InstructorRepository.GetInstructorById(InstructorId);
            if (requestProcessConfirm == null)
                return NotFound("Instructor_NotFoundId : "+ InstructorId);

            int insertedInstructor = await _InstructorRepository.UpdateInstructor(InstructorId, requestModel);

            if (insertedInstructor <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeleteInstructor/{InstructorId}")]
        public async Task<IActionResult> DeleteInstructor(int InstructorId, [FromBody] InstructorRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion

            InstructorRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("Instructor_Null");

            var requestProcessConfirm = await _InstructorRepository.GetInstructorById(InstructorId);
            if (requestProcessConfirm == null)
                return NotFound("Instructor_NotFoundId : "+ InstructorId);

            int insertedInstructor = await _InstructorRepository.DeleteInstructor(InstructorId, requestModel);

            if (insertedInstructor <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }
    }
}
