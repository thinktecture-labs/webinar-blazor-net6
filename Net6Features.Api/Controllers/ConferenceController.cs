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
        public async Task<IActionResult> ConferencesAsync()
        {
            var result = await _conferencesService.CollectionAsync();
            return Ok(result);
        }
    }
}
