namespace AuthOn.Application.Services.Interfaces
{
    public interface IEmailTemplateService
    {
        string GenerateInformativeEmailActivatedUser(string userName);
        string GenerateEmailWithActivateUserAction(Guid userId, long emailId, string userName);
    }
}