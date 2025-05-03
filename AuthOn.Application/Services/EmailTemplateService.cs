using AuthOn.Application.Configurations;
using AuthOn.Application.Services.Interfaces;
using AuthOn.Shared.Constants;
using System.Text;

namespace AuthOn.Application.Services
{
    public class EmailTemplateService(ITokenManager tokenManager, ApiInformationConfiguration apiInformationConfiguration) : IEmailTemplateService
    {
        private readonly ITokenManager _tokenManager = tokenManager;
        private readonly ApiInformationConfiguration _apiInformationConfiguration = apiInformationConfiguration;

        #region Methods

        #region Base Message

        private static string BaseMessage(string title, string userName, string detail, string? buttonText = null, string? url = null)
        {
            var baseMessage = new StringBuilder();

            baseMessage.Append("<html lang='en'>");
            baseMessage.Append("<head>");
            baseMessage.Append("<meta charset='UTF-8'>");
            baseMessage.Append("<meta name='viewport' content='width=device-width, initial-scale=1.0'>");
            baseMessage.Append($"<title>{title}</title>");
            baseMessage.Append("</head>");
            baseMessage.Append($"<body style='{EmailConstants.StyleBody}'>");
            baseMessage.Append($"<table {EmailConstants.TableConfig} width='100%' style='background-color: #f4f4f4; padding: 20px;'>");
            baseMessage.Append("<tr>");
            baseMessage.Append("<td align='center'>");
            baseMessage.Append($"<table {EmailConstants.TableConfig} width='600' style='{EmailConstants.MainTableStyle}'>");
            baseMessage.Append("<tr>");
            baseMessage.Append("<td style='padding: 20px; text-align: left;'>");
            baseMessage.Append($"<h1 style='{EmailConstants.HeadingStyle}'>{title}</h1>");
            baseMessage.Append($"<p style='{EmailConstants.ParagraphStyle}'>¡Hola {userName}!,</p>");
            baseMessage.Append($"<p style='{EmailConstants.ParagraphStyle}'>{detail}</p>");
            if (!string.IsNullOrWhiteSpace(buttonText))
            {
                baseMessage.Append("<table role='presentation' cellspacing='0' cellpadding='0'>");
                baseMessage.Append("<tr>");
                baseMessage.Append("<td align='center' style='border-radius: 5px;' bgcolor='#2c768f'>");
                baseMessage.Append($"<a href='{url}' target='_blank' style='{EmailConstants.ButtonStyle}'>{buttonText}</a>");
                baseMessage.Append("</td>");
                baseMessage.Append("</tr>");
                baseMessage.Append("</table>");
            }
            baseMessage.Append("</td>");
            baseMessage.Append("</tr>");
            baseMessage.Append("</table>");
            baseMessage.Append("</td>");
            baseMessage.Append("</tr>");
            baseMessage.Append("</table>");
            baseMessage.Append("</body>");
            baseMessage.Append("</html>");

            return baseMessage.ToString();
        }

        #endregion

        #region Informative Emails

        public string GenerateInformativeEmailActivatedUser(string userName)
        {
            return BaseMessage(
                title: EmailConstants.TitleUserActivated,
                userName,
                detail: EmailConstants.UserMessageSuccessfullyActivated
            );
        }

        #endregion

        #region Emails Whith Action Link

        public string GenerateEmailWithActivateUserAction(Guid userId, long emailId, string userName)
        {
            return BaseMessage(
                title: EmailConstants.TitleActivationUser,
                userName,
                detail: EmailConstants.MessageActivateYourUser,
                buttonText: EmailConstants.ButtonActivateUser,
                url: _apiInformationConfiguration.GetActivateUrl() + _tokenManager.GenerateActivationToken(userId, emailId).Value
            );
        }

        #endregion

        #endregion
    }
}