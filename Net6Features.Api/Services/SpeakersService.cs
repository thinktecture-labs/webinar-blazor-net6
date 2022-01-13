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
        public Task<List<Speaker>> CollectionAsync()
        {
            return _context.Speakers.ToListAsync();
        }
    }
}
