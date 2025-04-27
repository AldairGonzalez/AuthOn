using AuthOn.Domain.Entities.Emails;
using Microsoft.EntityFrameworkCore;

namespace AuthOn.Infrastructure.Persistence.Repositories
{
    public class EmailRepository(ApplicationDbContext context) : IEmailRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task AddAsync(Email email)
        {
            await _context.Emails.AddAsync(email);
        }

        public async Task<Email?> GetByIdAsync(long emailId)
        {
            return await _context.Emails.FirstOrDefaultAsync(e => e.Id == emailId);
        }

        public async Task UpdateAsync(Email email)
        {
            _context.Emails.Update(email);
            await Task.CompletedTask;
        }
    }
}