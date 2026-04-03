using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.SalesOrder;
using Infrastructure.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Persistence.Repository;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesOrderController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<SalesOrderController> _logger;
        private readonly ISalesOrderRepository _SalesOrderRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private ResponseDto _responseDto = new ResponseDto();

        public SalesOrderController(SecurityHelper securityHelper,
            ILogger<SalesOrderController> logger,
            ISalesOrderRepository SalesOrderRepository,
            UserManager<ApplicationUser> userManager
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._SalesOrderRepository = SalesOrderRepository;
            this._userManager = userManager;
        }

        [HttpPost("GetSalesOrder")]
        public async Task<IActionResult> GetSalesOrder(int pageNumber, [FromBody] SalesOrderFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("SalesOrder_InvalidPageNumber : "+ pageNumber);

            var result = await _SalesOrderRepository.GetSalesOrders(pageNumber, searchModel);
            if (result == null)
                return NotFound("SalesOrder_NotFoundList");

            return Ok(result);
        }

        [HttpPost("GetAllSalesOrders")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDistinctSalesOrder([FromBody] SalesOrderFilterDto searchModel)
        {
            var result = await _SalesOrderRepository.GetDistinctSalesOrders(searchModel);
            if (result == null)
                return NotFound("SalesOrder_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetSalesOrderById/{id}")]
        public async Task<IActionResult> GetSalesOrderById(int id)
        {

            if (id==0)
                return BadRequest("SalesOrder_InvalidId : "+ id);

            var result = await _SalesOrderRepository.GetSalesOrderById(id);
            if (result == null)
                return NotFound("SalesOrder_NotFoundId : "+ id);

            return Ok(result);
        }


        [HttpPost("InsertSalesOrder")]
        public async Task<IActionResult> InsertSalesOrder([FromBody] SalesOrderRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("SalesOrder_Null");

                int insertedSalesOrderId = await _SalesOrderRepository.InsertSalesOrder(requestModel);
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



        [HttpPost("UpdateSalesOrder/{SalesOrderId}")]
        public async Task<IActionResult> UpdateSalesOrder(int SalesOrderId, [FromBody] SalesOrderRequestDto updateRequestModel)
        {

            SalesOrderRequestDto requestModel = updateRequestModel;
            if (requestModel == null)
                return BadRequest("SalesOrder_Null");

            var requestProcessConfirm = await _SalesOrderRepository.GetSalesOrderById(SalesOrderId);
            if (requestProcessConfirm == null)
                return NotFound("SalesOrder_NotFoundId : "+ SalesOrderId);

            int insertedSalesOrder = await _SalesOrderRepository.UpdateSalesOrder(SalesOrderId, requestModel);

            if (insertedSalesOrder <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeleteSalesOrder/{SalesOrderId}")]
        public async Task<IActionResult> DeleteSalesOrder(int SalesOrderId, [FromBody] SalesOrderRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion

            SalesOrderRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("SalesOrder_Null");

            var requestProcessConfirm = await _SalesOrderRepository.GetSalesOrderById(SalesOrderId);
            if (requestProcessConfirm == null)
                return NotFound("SalesOrder_NotFoundId : "+ SalesOrderId);

            int insertedSalesOrder = await _SalesOrderRepository.DeleteSalesOrder(SalesOrderId, requestModel);

            if (insertedSalesOrder <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }
    }
}
