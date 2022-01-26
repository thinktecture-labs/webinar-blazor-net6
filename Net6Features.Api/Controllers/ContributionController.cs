using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Net6Features.Api.Models;
using Net6Features.Api.Services;

namespace Net6Features.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ContributionController : Controller
    {
        private readonly ContributionsService _contributionsService;

        public ContributionController(ContributionsService contributionsService)
        {
            _contributionsService = contributionsService ?? throw new ArgumentNullException(nameof(contributionsService));
        }

        [HttpGet]
        public async Task<IActionResult> GetContributionsAsync([FromQuery] int take = 100)
        {
            var result = await _contributionsService.GetCollectionAsync(new Shared.Services.CollectionRequest { Take = take });
            return Ok(result);
        }

        [HttpGet("filter/{searchTerm}")]
        public async Task<IActionResult> GetFilteredContributionsAsync([FromRoute] string searchTerm, [FromQuery] int take = 100)
        {
            var result = await _contributionsService.GetCollectionAsync(new Shared.Services.CollectionRequest { Take = take, SearchTerm = searchTerm });
            return Ok(result);
        }
    }
}
