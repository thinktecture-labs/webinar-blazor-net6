using Microsoft.AspNetCore.Mvc;
using Net6Features.Api.Services;

namespace Net6Features.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SpeakerController : Controller
    {
        private readonly SpeakersService _speakersService;

        public SpeakerController(SpeakersService speakersService)
        {
            _speakersService = speakersService ?? throw new ArgumentNullException(nameof(speakersService));
        }

        [HttpGet]
        public async Task<IActionResult> SpeakersAsync()
        {
            var result = await _speakersService.CollectionAsync();
            return Ok(result);
        }
    }
}
