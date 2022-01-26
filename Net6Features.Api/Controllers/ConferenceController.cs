using Microsoft.AspNetCore.Mvc;
using Net6Features.Api.Services;

namespace Net6Features.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ConferenceController : Controller
    {
        private readonly ConferencesService _conferencesService;

        public ConferenceController(ConferencesService conferencesService)
        {
            _conferencesService = conferencesService ?? throw new ArgumentNullException(nameof(conferencesService));
        }

        [HttpGet]
        public async Task<IActionResult> GetConferencesAsync([FromQuery] int take = 100)
        {
            var result = await _conferencesService.GetCollectionAsync(new Shared.Services.CollectionRequest { Take = take });
            return Ok(result);
        }

        [HttpGet("filter/{searchTerm}")]
        public async Task<IActionResult> GetFilteredConferencesAsync([FromRoute]string searchTerm, [FromQuery] int take = 100)
        {
            var result = await _conferencesService.GetCollectionAsync(new Shared.Services.CollectionRequest { Take = take, SearchTerm = searchTerm });
            return Ok(result);
        }
    }
}
