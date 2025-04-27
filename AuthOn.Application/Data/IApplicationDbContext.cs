using AuthOn.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace AuthOn.Application.Data
{
    public interface IApplicationDbContext
    {
        DbSet<User> Users { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}