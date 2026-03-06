using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.ProductRate;
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
    public class ProductRateController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<ProductRateController> _logger;
        private readonly IProductRateRepository _ProductRateRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<NotificationHub> _hubContext;
        private ResponseDto _responseDto = new ResponseDto();

        public ProductRateController(SecurityHelper securityHelper,
            ILogger<ProductRateController> logger,
            IProductRateRepository ProductRateRepository,
            UserManager<ApplicationUser> userManager,
            IHubContext<NotificationHub> hubContext
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._ProductRateRepository = ProductRateRepository;
            this._userManager = userManager;
            this._hubContext = hubContext;
        }

        [HttpPost("GetProductRate")]
        public async Task<IActionResult> GetProductRate(int pageNumber, [FromBody] ProductRateFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("ProductRate_InvalidPageNumber : "+ pageNumber);

            var result = await _ProductRateRepository.GetProductRates(pageNumber, searchModel);
            if (result == null)
                return NotFound("ProductRate_NotFoundList");

            return Ok(result);
        }

        [HttpPost("GetAllProductRates")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDistinctProductRate([FromBody] ProductRateFilterDto searchModel)
        {
            var result = await _ProductRateRepository.GetDistinctProductRates(searchModel);
            if (result == null)
                return NotFound("ProductRate_NotFoundList");

            return Ok(result);
        }


        [HttpGet("GetProductRateById/{id}")]
        public async Task<IActionResult> GetProductRateById(int id)
        {

            if (id==0)
                return BadRequest("ProductRate_InvalidId : "+ id);

            var result = await _ProductRateRepository.GetProductRateById(id);
            if (result == null)
                return NotFound("ProductRate_NotFoundId : "+ id);

            return Ok(result);
        }


        [HttpPost("InsertProductRate")]
        public async Task<IActionResult> InsertProductRate([FromBody] ProductRateRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("ProductRate_Null");

                int insertedProductRateId = await _ProductRateRepository.InsertProductRate(requestModel);
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



        [HttpPost("UpdateProductRate/{ProductRateId}")]
        public async Task<IActionResult> UpdateProductRate(int ProductRateId, [FromBody] ProductRateRequestDto updateRequestModel)
        {

            ProductRateRequestDto requestModel = updateRequestModel;
            if (requestModel == null)
                return BadRequest("ProductRate_Null");

            var requestProcessConfirm = await _ProductRateRepository.GetProductRateById(ProductRateId);
            if (requestProcessConfirm == null)
                return NotFound("ProductRate_NotFoundId : "+ ProductRateId);

            int insertedProductRate = await _ProductRateRepository.UpdateProductRate(ProductRateId, requestModel);

            if (insertedProductRate <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeleteProductRate/{ProductRateId}")]
        public async Task<IActionResult> DeleteProductRate(int ProductRateId, [FromBody] ProductRateRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion

            ProductRateRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("ProductRate_Null");

            var requestProcessConfirm = await _ProductRateRepository.GetProductRateById(ProductRateId);
            if (requestProcessConfirm == null)
                return NotFound("ProductRate_NotFoundId : "+ ProductRateId);

            int insertedProductRate = await _ProductRateRepository.DeleteProductRate(ProductRateId, requestModel);

            if (insertedProductRate <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }
    }
}
