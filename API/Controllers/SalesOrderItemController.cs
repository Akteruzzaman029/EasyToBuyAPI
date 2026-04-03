using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.SalesOrderItem;
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
    public class SalesOrderItemController : ControllerBase
    {
        private readonly SecurityHelper _securityHelper;
        private readonly ILogger<SalesOrderItemController> _logger;
        private readonly ISalesOrderItemRepository _SalesOrderItemRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private ResponseDto _responseDto = new ResponseDto();

        public SalesOrderItemController(SecurityHelper securityHelper,
            ILogger<SalesOrderItemController> logger,
            ISalesOrderItemRepository SalesOrderItemRepository,
            UserManager<ApplicationUser> userManager
            )
        {
            this._securityHelper = securityHelper;
            this._logger = logger;
            this._SalesOrderItemRepository = SalesOrderItemRepository;
            this._userManager = userManager;
        }

        [HttpPost("GetSalesOrderItem")]
        public async Task<IActionResult> GetSalesOrderItem(int pageNumber, [FromBody] SalesOrderItemFilterDto searchModel)
        {
            #region "Hash Checking"
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            #endregion

            if (pageNumber <= 0)
                return BadRequest("SalesOrderItem_InvalidPageNumber : "+ pageNumber);

            var result = await _SalesOrderItemRepository.GetSalesOrderItems(pageNumber, searchModel);
            if (result == null)
                return NotFound("SalesOrderItem_NotFoundList");

            return Ok(result);
        }

        [HttpPost("GetAllSalesOrderItems")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDistinctSalesOrderItem([FromBody] SalesOrderItemFilterDto searchModel)
        {
            var result = await _SalesOrderItemRepository.GetDistinctSalesOrderItems(searchModel);
            if (result == null)
                return NotFound("SalesOrderItem_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetSalesOrderItemById/{id}")]
        public async Task<IActionResult> GetSalesOrderItemById(int id)
        {

            if (id==0)
                return BadRequest("SalesOrderItem_InvalidId : "+ id);

            var result = await _SalesOrderItemRepository.GetSalesOrderItemById(id);
            if (result == null)
                return NotFound("SalesOrderItem_NotFoundId : "+ id);

            return Ok(result);
        }


        [HttpPost("InsertSalesOrderItem")]
        public async Task<IActionResult> InsertSalesOrderItem([FromBody] SalesOrderItemRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null)
                    return BadRequest("SalesOrderItem_Null");

                int insertedSalesOrderItemId = await _SalesOrderItemRepository.InsertSalesOrderItem(requestModel);
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



        [HttpPost("UpdateSalesOrderItem/{SalesOrderItemId}")]
        public async Task<IActionResult> UpdateSalesOrderItem(int SalesOrderItemId, [FromBody] SalesOrderItemRequestDto updateRequestModel)
        {

            SalesOrderItemRequestDto requestModel = updateRequestModel;
            if (requestModel == null)
                return BadRequest("SalesOrderItem_Null");

            var requestProcessConfirm = await _SalesOrderItemRepository.GetSalesOrderItemById(SalesOrderItemId);
            if (requestProcessConfirm == null)
                return NotFound("SalesOrderItem_NotFoundId : "+ SalesOrderItemId);

            int insertedSalesOrderItem = await _SalesOrderItemRepository.UpdateSalesOrderItem(SalesOrderItemId, requestModel);

            if (insertedSalesOrderItem <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;

            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Updated Successfully";

            return Ok(_responseDto);
        }


        [HttpPost("DeleteSalesOrderItem/{SalesOrderItemId}")]
        public async Task<IActionResult> DeleteSalesOrderItem(int SalesOrderItemId, [FromBody] SalesOrderItemRequestDto deleteRequestModel)
        {
            #region "Hash Checking"

            #endregion

            SalesOrderItemRequestDto requestModel = deleteRequestModel;

            if (requestModel == null)
                return BadRequest("SalesOrderItem_Null");

            var requestProcessConfirm = await _SalesOrderItemRepository.GetSalesOrderItemById(SalesOrderItemId);
            if (requestProcessConfirm == null)
                return NotFound("SalesOrderItem_NotFoundId : "+ SalesOrderItemId);

            int insertedSalesOrderItem = await _SalesOrderItemRepository.DeleteSalesOrderItem(SalesOrderItemId, requestModel);

            if (insertedSalesOrderItem <= 0)
            {
                _responseDto.StatusCode = StatusCodes.Status400BadRequest;
            }
            _responseDto.StatusCode = StatusCodes.Status200OK;
            _responseDto.Message = "Data Deleted Successfully";

            return Ok(_responseDto);
        }
    }
}
