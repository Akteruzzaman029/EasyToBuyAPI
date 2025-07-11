using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.Product;
using Infrastructure.IRepository;
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
    public class ProductController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<ProductController> _logger;
        private readonly IProductRepository _ProductRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<NotificationHub> _hubContext;
        private ResponseDto _responseDto = new ResponseDto();

        public ProductController(SecurityHelper securityHelper,
            ILogger<ProductController> logger,
            IProductRepository ProductRepository,
            UserManager<ApplicationUser> userManager,
            IHubContext<NotificationHub> hubContext
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._ProductRepository = ProductRepository;
            this._userManager = userManager;
            this._hubContext = hubContext;
        }

        [HttpPost("GetProduct")]
        public async Task<IActionResult> GetProduct(int pageNumber, [FromBody] ProductFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("Product_InvalidPageNumber : "+ pageNumber);

            var result = await _ProductRepository.GetProducts(pageNumber, searchModel);
            if (result == null)
                return NotFound("Product_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetDistinctProduct([FromBody] ProductFilterDto searchModel)
        {
            var result = await _ProductRepository.GetDistinctProducts(searchModel);
            if (result == null)
                return NotFound("Product_NotFoundList");

            return Ok(result);
        }


        [HttpGet("GetProductById/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {

            if (id==0)
                return BadRequest("Product_InvalidId : "+ id);

            var result = await _ProductRepository.GetProductById(id);
            if (result == null)
                return NotFound("Product_NotFoundId : "+ id);

            return Ok(result);
        }


        [HttpPost("InsertProduct")]
        public async Task<IActionResult> InsertProduct([FromBody] ProductRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("Product_Null");

                int insertedProductId = await _ProductRepository.InsertProduct(requestModel);
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



        [HttpPost("UpdateProduct/{ProductId}")]
        public async Task<IActionResult> UpdateProduct(int ProductId, [FromBody] ProductRequestDto updateRequestModel)
        {

            ProductRequestDto requestModel = updateRequestModel;
            if (requestModel == null)
                return BadRequest("Product_Null");

            var requestProcessConfirm = await _ProductRepository.GetProductById(ProductId);
            if (requestProcessConfirm == null)
                return NotFound("Product_NotFoundId : "+ ProductId);

            int insertedProduct = await _ProductRepository.UpdateProduct(ProductId, requestModel);

            if (insertedProduct <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeleteProduct/{ProductId}")]
        public async Task<IActionResult> DeleteProduct(int ProductId, [FromBody] ProductRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion

            ProductRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("Product_Null");

            var requestProcessConfirm = await _ProductRepository.GetProductById(ProductId);
            if (requestProcessConfirm == null)
                return NotFound("Product_NotFoundId : "+ ProductId);

            int insertedProduct = await _ProductRepository.DeleteProduct(ProductId, requestModel);

            if (insertedProduct <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }
    }
}
