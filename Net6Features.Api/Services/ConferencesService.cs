using Microsoft.EntityFrameworkCore;
using Net6Features.Api.Models;
using Net6Features.Shared.Models;
using Net6Features.Shared.Services;

namespace Net6Features.Api.Services
{
    public class ConferencesService : IConferenceService
    {
        private readonly SampleDatabaseContext _context;

        public ConferencesService(SampleDatabaseContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public Task<List<Conference>> CollectionAsync()
        {
            return _context.Conferences.ToListAsync();
        }
    }
}
