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
        public async Task<IActionResult> GetSpeakersAsync([FromQuery] int take = 100)
        {
            var result = await _speakersService.GetCollectionAsync(new Shared.Services.CollectionRequest { Take = take });
            return Ok(result);
        }

        [HttpGet("filter/{searchTerm}")]
        public async Task<IActionResult> GetFilteredSpeakersAsync([FromRoute] string searchTerm, [FromQuery] int take = 100)
        {
            var result = await _speakersService.GetCollectionAsync(new Shared.Services.CollectionRequest { Take = take, SearchTerm = searchTerm });
            return Ok(result);
        }
    }
}
