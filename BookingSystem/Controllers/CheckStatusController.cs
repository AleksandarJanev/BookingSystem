using BookingSystem.IServices;
using BookingSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.Controllers
{
    [ApiController]
    [Route("api/status")]
    public class CheckStatusController : Controller
    {
        private readonly IManager _managerService;

        public CheckStatusController(IManager managerService)
        {
            _managerService = managerService;
        }

        [HttpPost("/check")]
        public async Task<IActionResult> CheckAsync([FromBody] CheckStatusReq request)
        {
            var result = await _managerService.CheckStatus(request);
            return Ok(result);
        }
    }
}
