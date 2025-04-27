using Microsoft.EntityFrameworkCore;
using AuthOn.Application.Data;
using AuthOn.Domain.Entities.Users;
using AuthOn.Domain.Primitives;
using AuthOn.Domain.Entities.Emails;
using AuthOn.Domain.Entities.EmailStates;

namespace AuthOn.Infrastructure.Persistence
{
    public class ApplicationDbContext(DbContextOptions options) : DbContext(options), IApplicationDbContext, IUnitOfWork
    {
        public DbSet<Email> Emails { get; set; }
        public DbSet<EmailState> EmailStates { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var result = await base.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}