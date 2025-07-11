using Core.Identity;
using Core.ModelDto;
using Infrastructure.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Persistence.Repository;
using System.Net;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly SecurityHelper _securityHelper;
        private readonly IAuthRepository _authRepository;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private ResponseDto _responseDto = new ResponseDto();

        public AuthController(ILogger<AuthController> logger, SecurityHelper securityHelper,
            IAuthRepository authRepository, SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _securityHelper = securityHelper;
            _authRepository = authRepository;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        private static Dictionary<string, int> requestCounts = new Dictionary<string, int>();


        [HttpPost("Login"), AllowAnonymous]
        [RateLimit]
        public async Task<IActionResult> Login([FromBody] LoginReequestDto loginReequestDto)
        {
            LoginResponseDto token = new LoginResponseDto();
            try
            {
                if (ModelState.IsValid)
                {
                    // Attempt to find the user by email or username
                    var userd = await _userManager.FindByEmailAsync(loginReequestDto.UserName);
                    if (userd == null)
                    {
                        userd = await _userManager.FindByNameAsync(loginReequestDto.UserName);
                    }

                    if (userd != null)
                    {
                        // Sign in the user without requiring a password
                        await _signInManager.SignInAsync(userd, false);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "User not found.");
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }

                var user = await _userManager.FindByNameAsync(loginReequestDto.UserName);


                if (user == null)
                {
                    return Unauthorized(new { message = "Invalid username or password." });
                }

                // Verify the password
                var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginReequestDto.Password);
                if (!isPasswordValid)
                {
                    return Unauthorized(new { message = "Invalid username or password." });
                }

                UserInfoDto userInfo = new UserInfoDto
                {
                    Id = user.Id,
                    UserName = loginReequestDto.UserName,
                    Name = user.FullName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Role = (await _userManager.GetRolesAsync(user)).First()
                };

                if (userInfo != null)
                {
                    token.JwtToken = await Task.Run(() => _securityHelper.GenerateJSONWebToken(userInfo));
                    token.Expires = DateTime.Now.AddMinutes(_securityHelper.GetJWTExpires());
                    token.RefreshToken = await Task.Run(() => _securityHelper.GenerateRefreshToken());
                    token.RefreshTokenExpires = DateTime.Now.AddMinutes(Convert.ToInt32(_securityHelper.GetJWTRefreshExpires()));
                    token.Type= user.Type;
                    token.UserName = user.FullName;
                    token.UserId= user.Id;
                    await _authRepository.UpdateRefreshToken(userInfo.Id, token);
                    _responseDto.Data = token;
                    _responseDto.StatusCode = 200;
                }

            }
            catch (Exception ex)
            {
                _ = Task.Run(() => { _logger.LogError(ex, ex.Message); });
                _responseDto.Message = ex.Message;
                _responseDto.StatusCode = 404;
            }

            return Ok(_responseDto);
        }


        [HttpPost("RefreshToken"), AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto requestDto)
        {
            LoginResponseDto token = new LoginResponseDto();
            try
            {
                #region Validatation
                var user = await _userManager.FindByIdAsync(requestDto.UserId);
                if (user == null) return NotFound("User Not Found");

                if (user.RefreshToten != requestDto.RefreshToken || user.RefreshTokenExpires < DateTime.Now)
                    return Unauthorized("Refresh Token Expires");
                #endregion

                UserInfoDto userInfo = new UserInfoDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Name = user.FullName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Role = (await _userManager.GetRolesAsync(user)).First()
                };

                if (userInfo != null)
                {
                    token.JwtToken = await Task.Run(() => _securityHelper.GenerateJSONWebToken(userInfo));
                    token.Expires = DateTime.Now.AddMinutes(_securityHelper.GetJWTExpires());
                    token.RefreshToken = await Task.Run(() => _securityHelper.GenerateRefreshToken());
                    token.RefreshTokenExpires = DateTime.Now.AddMinutes(Convert.ToInt32(_securityHelper.GetJWTRefreshExpires()));
                    token.Type = user.Type;
                    token.UserName = user.FullName;
                    token.UserId = user.Id;
                    await _authRepository.UpdateRefreshToken(userInfo.Id, token);
                    _responseDto.Data = token;
                    _responseDto.StatusCode = 200;
                }
            }
            catch (Exception ex)
            {
                _ = Task.Run(() => { _logger.LogError(ex, ex.Message); });
                _responseDto.Message = ex.Message;
                _responseDto.StatusCode = 404;
            }
            return Ok(_responseDto);
        }


        [HttpPost("ResetPassword"), AllowAnonymous]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null)
            {
                var isCorrect = await _userManager.CheckPasswordAsync(user, model.OldPassword);

                if (isCorrect)
                {
                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var ResetCode = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(code));
                    var result = await _userManager.ResetPasswordAsync(user, ResetCode, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return Ok(new { Message = "Successfully reset your password.", IsSuccess = true });
                    }
                    return Ok(new { Message = "Something went wrong.Please try again..", IsSuccess = false });
                }
                else
                {
                    return BadRequest(new { Message = "UserName/Password does not match.Please enter correct password.", IsSuccess = false });
                }
            }
            return Unauthorized();
        }

        public class RateLimitAttribute : ActionFilterAttribute
        {
            private const int MaxRequests = 10;
            private const int WindowSeconds = 60;

            public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                // Retrieve client IP address
                var ipAddress = context.HttpContext.Connection.RemoteIpAddress.ToString();

                // Check if the IP address is already being tracked
                if (!requestCounts.ContainsKey(ipAddress))
                {
                    requestCounts[ipAddress] = 1;
                }
                else
                {
                    requestCounts[ipAddress]++;
                }

                // Check if the number of requests exceeds the limit
                if (requestCounts[ipAddress] > MaxRequests)
                {
                    context.Result = new StatusCodeResult((int)HttpStatusCode.TooManyRequests);
                    return;
                }

                // timer to reset the request count after the window expires
                var timer = new Timer(state =>
                {
                    requestCounts.Remove(ipAddress);
                }, null, TimeSpan.FromSeconds(WindowSeconds), TimeSpan.Zero);

                context.HttpContext.Response.Headers.Add("X-Rate-Limit-Limit", MaxRequests.ToString());
                context.HttpContext.Response.Headers.Add("X-Rate-Limit-Remaining", Math.Max(0, MaxRequests - requestCounts[ipAddress]).ToString());
                await next();
            }
        }



    }
}
