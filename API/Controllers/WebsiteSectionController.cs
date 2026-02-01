using Core.Identity;
using Core.ModelDto;
using Core.ModelDto.WebsiteSection;
using Infrastructure.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebsiteSectionController : ControllerBase
    {
        private readonly ILogger<WebsiteSectionController> _logger;
        private readonly IWebsiteSectionRepository _websiteSectionRepository;
        private ResponseDto _responseDto = new ResponseDto();

        public WebsiteSectionController(
            ILogger<WebsiteSectionController> logger,
            IWebsiteSectionRepository websiteSectionRepository)
        {
            this._logger = logger;
            this._websiteSectionRepository = websiteSectionRepository;
        }

        [HttpPost("GetWebsiteSection")]
        public async Task<IActionResult> GetWebsiteSection(int pageNumber, [FromBody] WebsiteSectionFilterDto searchModel)
        {
            if (pageNumber <= 0)
                return BadRequest("WebsiteSection_InvalidPageNumber : " + pageNumber);

            var result = await _websiteSectionRepository.GetWebsiteSections(pageNumber, searchModel);
            if (result == null)
                return NotFound("WebsiteSection_NotFoundList");

            return Ok(result);
        }

        [HttpGet("GetWebsiteSectionById/{id}")]
        public async Task<IActionResult> GetWebsiteSectionById(int id)
        {
            if (id == 0)
                return BadRequest("WebsiteSection_InvalidId : " + id);

            var result = await _websiteSectionRepository.GetWebsiteSectionById(id);
            if (result == null)
                return NotFound("WebsiteSection_NotFoundId : " + id);

            return Ok(result);
        }

        [HttpPost("InsertWebsiteSection")]
        public async Task<IActionResult> InsertWebsiteSection([FromBody] WebsiteSectionRequestDto requestModel)
        {
            _responseDto = new ResponseDto();
            try
            {
                if (requestModel == null) return BadRequest("WebsiteSection_Null");

                int id = await _websiteSectionRepository.InsertWebsiteSection(requestModel);
                _responseDto.StatusCode = StatusCodes.Status200OK;
                _responseDto.Message = "Data Saved Successfully";
            }
            catch (Exception ex)
            {
                _responseDto.Message = ex.InnerException?.Message ?? ex.Message;
            }
            return Ok(_responseDto);
        }

        [HttpPost("UpdateWebsiteSection/{id}")]
        public async Task<IActionResult> UpdateWebsiteSection(int id, [FromBody] WebsiteSectionRequestDto updateRequestModel)
        {
            if (updateRequestModel == null) return BadRequest("WebsiteSection_Null");

            var exists = await _websiteSectionRepository.GetWebsiteSectionById(id);
            if (exists == null) return NotFound("WebsiteSection_NotFoundId : " + id);

            int result = await _websiteSectionRepository.UpdateWebsiteSection(id, updateRequestModel);

            _responseDto.StatusCode = result > 0 ? StatusCodes.Status200OK : StatusCodes.Status400BadRequest;
            _responseDto.Message = result > 0 ? "Data Updated Successfully" : "Update Failed";

            return Ok(_responseDto);
        }

        [HttpPost("DeleteWebsiteSection/{id}")]
        public async Task<IActionResult> DeleteWebsiteSection(int id, [FromBody] WebsiteSectionRequestDto deleteRequestModel)
        {
            var exists = await _websiteSectionRepository.GetWebsiteSectionById(id);
            if (exists == null) return NotFound("WebsiteSection_NotFoundId : " + id);

            int result = await _websiteSectionRepository.DeleteWebsiteSection(id, deleteRequestModel);

            _responseDto.StatusCode = result > 0 || result == -1 ? StatusCodes.Status200OK : StatusCodes.Status400BadRequest;
            _responseDto.Message = result > 0 || result == -1 ? "Data Deleted Successfully" : "Delete Failed";

            return Ok(_responseDto);
        }
    }
}