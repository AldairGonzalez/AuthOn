namespace AuthOn.Application.Services.Interfaces
{
    public interface IEmailTemplateService
    {
        string GenerateInformativeEmailActivatedUser(string userName);
        string GenerateEmailWithActivateUserAction(string userName, string token);
    }
}