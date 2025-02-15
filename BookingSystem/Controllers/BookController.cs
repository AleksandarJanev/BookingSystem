using BookingSystem.IServices;
using BookingSystem.Models;
using BookingSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookingSystem.Controllers
{
    [ApiController]
    [Route("api")]
    public class BookController : Controller
    {
        private readonly IManager _managerService;
        public BookController(IManager managerService)
        {
            _managerService = managerService;
        }


        [HttpPost("/book")]
        public async Task<IActionResult> BookAsync([FromBody] BookReq request)
        {
            var result = await _managerService.Book(request);
            return Ok(result);
        }

    }
}
