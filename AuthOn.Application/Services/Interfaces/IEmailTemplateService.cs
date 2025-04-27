using AuthOn.Domain.Entities.Users;
using AuthOn.Domain.ValueObjects;

namespace AuthOn.Application.Services.Interfaces
{
    public interface IEmailTemplateService
    {
        string GenerateActivationEmail(Guid userId, string userName);
    }
}