using AuthOn.Domain.Entities.Emails;
using Microsoft.EntityFrameworkCore;

namespace AuthOn.Infrastructure.Persistence.Repositories
{
    public class EmailRepository(ApplicationDbContext context) : IEmailRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task AddAsync(Email email, CancellationToken cancellationToken)
        {
            await _context.Emails.AddAsync(email, cancellationToken);
        }

        public async Task<Email?> GetByIdAsync(long emailId, CancellationToken cancellationToken)
        {
            return await _context.Emails.FirstOrDefaultAsync(e => e.Id == emailId, cancellationToken);
        }

        public async Task Update(Email email)
        {
            _context.Emails.Update(email);
            await Task.CompletedTask;
        }
    }
}