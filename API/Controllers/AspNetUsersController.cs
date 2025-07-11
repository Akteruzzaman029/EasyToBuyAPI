using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.AspNetUsers;
using Core.ModelDto.Instructor;
using Core.ModelDto.Payment;
using Core.ModelDto.Student;
using Core.ModelDto.UserPackage;
using Infrastructure.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Persistence.Repository;
using System.Security.Claims;
using static Core.BaseEnum;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AspNetUsersController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<AspNetUsersController> _logger;
        private readonly IAspNetUsersRepository _AspNetUsersRepository;
        private readonly IInstructorRepository _InstructorRepository;
        private readonly IStudentRepository _StudentRepository;
        private readonly IPaymentRepository _PaymentRepository;
        private readonly IUserPackageRepository _UserPackageRepository;

        private readonly UserManager<ApplicationUser> _userManager;
        private ResponseDto _responseDto = new ResponseDto();

        public AspNetUsersController(SecurityHelper securityHelper,
            ILogger<AspNetUsersController> logger,
            IAspNetUsersRepository AspNetUsersRepository,
            UserManager<ApplicationUser> userManager,
            IInstructorRepository InstructorRepository,
            IStudentRepository StudentRepository,
            IPaymentRepository PaymentRepository,
            IUserPackageRepository UserPackageRepository
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._AspNetUsersRepository = AspNetUsersRepository;
            this._userManager = userManager;
            this._userManager = userManager;
            this._InstructorRepository = InstructorRepository;
            this._StudentRepository = StudentRepository;
            this._PaymentRepository = PaymentRepository;
            this._UserPackageRepository = UserPackageRepository;
        }

        [HttpPost("GetAspNetUsers")]
        public async Task<IActionResult> GetAspNetUsers(int pageNumber, AspNetUsersFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("AspNetUsers_InvalidPageNumber : "+ pageNumber);

            var result = await _AspNetUsersRepository.GetAspNetUserses(pageNumber, searchModel);
            if (result == null)
                return NotFound("AspNetUsers_NotFoundList");

            return Ok(result);
        }

        [HttpPost("GetAllUsers")]
        public async Task<IActionResult> GetDistinctAspNetUsers([FromBody] AspNetUsersFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            //var userName = User.Identity?.Name;
            //var user = await _userManager.FindByNameAsync(userName);
            var result = await _AspNetUsersRepository.GetDistinctAspNetUserses(searchModel);

            if (result == null)
                return NotFound("AspNetUsers_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetAspNetUsersByType")]
        public async Task<IActionResult> GetAspNetUsersByType(int Type)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion
            var result = await _AspNetUsersRepository.GetAspNetUsersByType(Type);

            if (result == null)
                return NotFound("AspNetUsers_NotFoundList");

            return Ok(result);
        }


        [HttpGet("GetAspNetUserById/{id}")]
        public async Task<IActionResult> GetAspNetUserById(string id)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (string.IsNullOrEmpty(id))
                return BadRequest("AspNetUsers_InvalidId : "+ id);
            var userName = User.Identity?.Name;
            var user = await _userManager.FindByNameAsync(userName);

            var result = await _AspNetUsersRepository.GetAspNetUsersById(id);
            if (result == null)
                return NotFound("AspNetUsers_NotFoundId : "+ id);

            return Ok(result);
        }


        [HttpPost("InsertAspNetUsers")]
        public async Task<IActionResult> InsertAspNetUsers([FromBody] AspNetUsersRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                #region "Hash Checking"


                #endregion

                if (requestModel == null)
                    return BadRequest("AspNetUsers_Null");

                var existingUser = await _userManager.FindByNameAsync(requestModel.Email);
                if (existingUser != null)
                {
                    _responseDto.StatusCode = (int)StatusCodes.Status400BadRequest;
                    _responseDto.Message = "User already exists";
                    return BadRequest(_responseDto);
                }
                var oUser = await _AspNetUsersRepository.InsertAspNetUsers(requestModel);

                if (oUser == null)
                {
                    _responseDto.StatusCode = (int)StatusCodes.Status400BadRequest;
                    _responseDto.Message = "Failed to create user";
                    return BadRequest(_responseDto);
                }

                if (oUser.Type == (int)UserRoleEnum.Instructor)
                {
                    await _InstructorRepository.InsertInstructor(new InstructorRequestDto
                    {
                        UserId = oUser.Id,
                        Name = oUser.FullName,
                        Email = oUser.Email,
                        Phone = oUser.PhoneNumber,
                        IsActive = true
                    });
                }
                else if (oUser.Type == (int)UserRoleEnum.Student)
                {
                    await _StudentRepository.InsertStudent(new StudentRequestDto
                    {
                        UserId = oUser.Id,
                        Name = requestModel.FullName,
                        Email = requestModel.Email,
                        Phone = requestModel.PhoneNumber,
                        DateOfBirth = requestModel.DateOfBirth,
                        IsActive = true
                    });


                }

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

        [HttpPost("Registration")]
        public async Task<IActionResult> Registration([FromBody] RegistrationRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                #region "Hash Checking"


                #endregion

                if (requestModel == null)
                    return BadRequest("AspNetUsers_Null");

                var existingUser = await _userManager.FindByNameAsync(requestModel.Email);
                if (existingUser != null)
                {
                    _responseDto.StatusCode = (int)StatusCodes.Status400BadRequest;
                    _responseDto.Message = "User already exists";
                    return BadRequest(_responseDto);
                }
                AspNetUsersRequestDto insertRequestModel = new AspNetUsersRequestDto();
                insertRequestModel.FullName = requestModel.FullName;
                insertRequestModel.UserName = requestModel.UserName;
                insertRequestModel.Email = requestModel.Email;
                insertRequestModel.PhoneNumber = requestModel.PhoneNumber;
                insertRequestModel.Password = requestModel.Password;
                insertRequestModel.ConfirmPassword = requestModel.ConfirmPassword;
                insertRequestModel.Type = requestModel.Type;
                var oUser = await _AspNetUsersRepository.InsertAspNetUsers(insertRequestModel);

                if (oUser == null)
                {
                    _responseDto.StatusCode = (int)StatusCodes.Status400BadRequest;
                    _responseDto.Message = "Failed to create user";
                    return BadRequest(_responseDto);
                }

                if (oUser.Type == (int)UserRoleEnum.Student)
                {
                    await _StudentRepository.InsertStudent(new StudentRequestDto
                    {
                        UserId = oUser.Id,
                        Name = requestModel.FullName,
                        Email = requestModel.Email,
                        Phone = requestModel.PhoneNumber,
                        DateOfBirth = requestModel.DateOfBirth,
                        Address = requestModel.Address,
                        LearningStage = requestModel.LearningStage,
                        BookingId = requestModel.BookingId,
                        VehicleType = requestModel.VehicleType,
                        FileId = requestModel.FileId,
                        PostalCode = requestModel.PostalCode,
                        IsActive = true
                    });

                    await _UserPackageRepository.InsertUserPackage(new UserPackageRequestDto
                    {
                        UserId = oUser.Id,
                        PackageId=requestModel.PackageId,
                        PaymentStatus = 1,
                        PackageStartDate= DateTime.UtcNow,
                        ExpiryDate= DateTime.UtcNow.AddMonths(3),
                        NoOfLesson = requestModel.Nooflesson,
                        LessonRate = requestModel.LessonRate,
                        Discount = requestModel.Discount,
                        Amount = requestModel.Amount,
                        NetAmount = requestModel.NetAmount,
                        Remarks="",
                        IsActive = true
                    });

                    await _PaymentRepository.InsertPayment(new PaymentRequestDto
                    {
                        UserId = oUser.Id,
                        PackageId=requestModel.PackageId,
                        Amount = requestModel.PaymentAmount,
                        Status=1,
                        TransactionDate= DateTime.UtcNow,
                        PaymentMethod="",
                        Remarks="",
                        IsActive = true
                    });
                }

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


        [HttpPost("UpdateAspNetUsers/{userId}")]
        public async Task<IActionResult> UpdateAspNetUsers(string userId, [FromBody] AspNetUsersRequestDto updateRequestModel)
        {
            #region "Hash Checking"

            #endregion
            var userName = User.Identity?.Name;
            var user = await _userManager.FindByNameAsync(userName);

            AspNetUsersRequestDto requestModel = updateRequestModel;

            if (requestModel == null)
                return BadRequest("AspNetUsers_Null");

            var requestProcessConfirm = await _AspNetUsersRepository.GetAspNetUsersById(userId);
            if (requestProcessConfirm == null)
                return NotFound("AspNetUsers_NotFoundId : "+ userId);

            var insertedAspNetUsers = await _AspNetUsersRepository.UpdateAspNetUsers(userId, requestModel);

            if (insertedAspNetUsers ==null)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeleteAspNetUsers/{userId}")]
        public async Task<IActionResult> DeleteAspNetUsers(string userId, [FromBody] AspNetUsersRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion
            var userName = User.Identity?.Name;
            var user = await _userManager.FindByNameAsync(userName);

            AspNetUsersRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("AspNetUsers_Null");

            var requestProcessConfirm = await _AspNetUsersRepository.GetAspNetUsersById(userId);
            if (requestProcessConfirm == null)
                return NotFound("AspNetUsers_NotFoundId : "+ userId);


            int insertedAspNetUsers = await _AspNetUsersRepository.DeleteAspNetUsers(userId, requestModel);

            if (insertedAspNetUsers <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }

    }
}
