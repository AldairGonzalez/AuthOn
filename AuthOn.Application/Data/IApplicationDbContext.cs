using AuthOn.Domain.Entities.Emails;
using AuthOn.Domain.Entities.EmailStates;
using AuthOn.Domain.Entities.TokenTypes;
using AuthOn.Domain.Entities.Users;
using AuthOn.Domain.Entities.UserTokens;
using Microsoft.EntityFrameworkCore;

namespace AuthOn.Application.Data
{
    public interface IApplicationDbContext
    {
        public DbSet<Email> Emails { get; set; }
        public DbSet<EmailState> EmailStates { get; set; }
        public DbSet<TokenType> TokenTypes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}