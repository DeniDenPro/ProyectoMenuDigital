using Application.Interfaces.IStatus;
using Application.Interfaces.IStatus.IStatusService;
using Application.Models.Response;
using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MenuDigital.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Asp.Versioning.ApiVersion("1.0")]
    public class StatusController : ControllerBase
    {
        private readonly IGetAllStatusesService _getAllStatus;
        public StatusController(IGetAllStatusesService getAllStatus)
        {
            _getAllStatus = getAllStatus;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<StatusResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllStatuses()
        {
            var statuses = await _getAllStatus.GetAllStatus();
            return Ok(statuses);
        }

    }
}
