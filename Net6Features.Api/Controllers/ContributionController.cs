using Microsoft.AspNetCore.Mvc;
using Net6Features.Api.Services;

namespace Net6Features.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ContributionController : Controller
    {
        private readonly ContributionsService _contributionService;

        public ContributionController(ContributionsService contributionService)
        {
            _contributionService = contributionService ?? throw new ArgumentNullException(nameof(contributionService));
        }

        [HttpGet]
        public async Task<IActionResult> ContributionsAsync()
        {
            var result = await _contributionService.CollectionAsync();
            return Ok(result);
        }
    }
}
