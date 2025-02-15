using BookingSystem.IServices;
using BookingSystem.Models;
using BookingSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.Controllers
{
    [ApiController]
    [Route("api/search")]
    public class SearchController : Controller
    {
        private readonly IManager _managerService;

        public SearchController(IManager managerService)
        {
            _managerService = managerService;
        }

        [HttpPost("/options")]
        public async Task<IActionResult> SearchAsync([FromBody] SearchReq request)
        {
            var result = await _managerService.Search(request);
            return Ok(result);
        }
    }
}
