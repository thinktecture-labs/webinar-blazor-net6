using Microsoft.EntityFrameworkCore;
using Net6Features.Api.Models;
using Net6Features.Shared.Models;
using Net6Features.Shared.Services;

namespace Net6Features.Api.Services
{
    public class SpeakersService : ISpeakersService
    {
        private readonly SampleDatabaseContext _context;

        public SpeakersService(SampleDatabaseContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public Task<List<Speaker>> GetCollectionAsync(CollectionRequest request)
        {
            var result = String.IsNullOrWhiteSpace(request.SearchTerm)
                ? _context.Speakers
                : _context.Speakers.Where(c => c.FirstName.Contains(request.SearchTerm) || c.LastName.Contains(request.SearchTerm));
            return result.Take(request.Take).ToListAsync();
        }
    }
}
