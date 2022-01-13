using Microsoft.EntityFrameworkCore;
using Net6Features.Shared.Models;

namespace Net6Features.Api.Models
{
    public class SampleDatabaseContext : DbContext
    {
        public SampleDatabaseContext(DbContextOptions<SampleDatabaseContext> options)
            : base(options)
        {
        }

        public DbSet<Conference> Conferences { get; set; }
        public DbSet<Contribution> Contributions { get; set; }
        public DbSet<Speaker> Speakers { get; set; }
    }
}
