using Microsoft.EntityFrameworkCore;
using Net6Features.Api.Models;
using Net6Features.Shared.Models;
using Net6Features.Shared.Services;

namespace Net6Features.Api.Services
{
    public class ContributionsService : IContributionsService
    {
        private readonly SampleDatabaseContext _context;

        public ContributionsService(SampleDatabaseContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public Task<List<Contribution>> GetCollectionAsync(CollectionRequest request)
        {
            var result = String.IsNullOrWhiteSpace(request.SearchTerm)
                ? _context.Contributions
                : _context.Contributions.Where(c => c.Title.Contains(request.SearchTerm));
            return result.Take(request.Take).ToListAsync();
        }
    }
}
