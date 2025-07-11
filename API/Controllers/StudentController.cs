using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.Student;
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
    public class StudentController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<StudentController> _logger;
        private readonly IStudentRepository _StudentRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private ResponseDto _responseDto = new ResponseDto();

        public StudentController(SecurityHelper securityHelper,
            ILogger<StudentController> logger,
            IStudentRepository StudentRepository,
            UserManager<ApplicationUser> userManager
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._StudentRepository = StudentRepository;
            this._userManager = userManager;
        }

        [HttpPost("GetStudent")]
        public async Task<IActionResult> GetStudent(int pageNumber, [FromBody] StudentFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("Student_InvalidPageNumber : "+ pageNumber);

            var result = await _StudentRepository.GetStudents(pageNumber, searchModel);
            if (result == null)
                return NotFound("Student_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetAllStudentes")]
        public async Task<IActionResult> GetDistinctStudent()
        {
            var result = await _StudentRepository.GetDistinctStudents();
            if (result == null)
                return NotFound("Student_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetStudentById/{id}")]
        public async Task<IActionResult> GetStudentById(int id)
        {

            if (id==0)
                return BadRequest("Student_InvalidId : "+ id);

            var result = await _StudentRepository.GetStudentById(id);
            if (result == null)
                return NotFound("Student_NotFoundId : "+ id);

            return Ok(result);
        }


        [HttpPost("InsertStudent")]
        public async Task<IActionResult> InsertStudent([FromBody] StudentRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("Student_Null");

                int insertedStudentId = await _StudentRepository.InsertStudent(requestModel);
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



        [HttpPost("UpdateStudent/{StudentId}")]
        public async Task<IActionResult> UpdateStudent(int StudentId, [FromBody] StudentRequestDto updateRequestModel)
        {

            StudentRequestDto requestModel = updateRequestModel;
            if (requestModel == null)
                return BadRequest("Student_Null");

            var requestProcessConfirm = await _StudentRepository.GetStudentById(StudentId);
            if (requestProcessConfirm == null)
                return NotFound("Student_NotFoundId : "+ StudentId);

            int insertedStudent = await _StudentRepository.UpdateStudent(StudentId, requestModel);

            if (insertedStudent <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeleteStudent/{StudentId}")]
        public async Task<IActionResult> DeleteStudent(int StudentId, [FromBody] StudentRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion

            StudentRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("Student_Null");

            var requestProcessConfirm = await _StudentRepository.GetStudentById(StudentId);
            if (requestProcessConfirm == null)
                return NotFound("Student_NotFoundId : "+ StudentId);

            int insertedStudent = await _StudentRepository.DeleteStudent(StudentId, requestModel);

            if (insertedStudent <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }
    }
}
