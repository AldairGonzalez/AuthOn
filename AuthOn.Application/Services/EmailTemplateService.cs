using AuthOn.Application.Configurations;
using AuthOn.Application.Services.Interfaces;
using AuthOn.Shared.Constants;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace AuthOn.Application.Services
{
    public class EmailTemplateService(ITokenManager tokenManager, ApiEndPointsConstants apiEndPoints) : IEmailTemplateService
    {
        private readonly ITokenManager _tokenManager = tokenManager;
        private readonly ApiEndPointsConstants _apiEndPoints = apiEndPoints;

        #region Properties

        private static string StyleBody => "margin: 0; padding: 0; font-family: Arial, sans-serif; background-color: #f4f4f4;";
        private static string TableConfig => "role='presentation' cellpadding='0' cellspacing='0'";
        private static string MainTableStyle => "background-color: #FFFFFF; border-radius: 8px; padding: 20px; box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);";
        private static string HeadingStyle => "font-size: 24px; color: #333333;";
        private static string ParagraphStyle => "font-size: 16px; color: #666666;";
        private static string ButtonStyle => "padding: 12px 25px; font-size: 16px; color: #FFFFFF; text-decoration: none; display: inline-block; border-radius: 5px;";

        #endregion

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
            baseMessage.Append($"<body style='{StyleBody}'>");
            baseMessage.Append($"<table {TableConfig} width='100%' style='background-color: #f4f4f4; padding: 20px;'>");
            baseMessage.Append("<tr>");
            baseMessage.Append("<td align='center'>");
            baseMessage.Append($"<table {TableConfig} width='600' style='{MainTableStyle}'>");
            baseMessage.Append("<tr>");
            baseMessage.Append("<td style='padding: 20px; text-align: left;'>");
            baseMessage.Append($"<h1 style='{HeadingStyle}'>{title}</h1>");
            baseMessage.Append($"<p style='{ParagraphStyle}'>¡Hola {userName}!,</p>");
            baseMessage.Append($"<p style='{ParagraphStyle}'>{detail}</p>");
            if (!string.IsNullOrWhiteSpace(buttonText))
            {
                baseMessage.Append("<table role='presentation' cellspacing='0' cellpadding='0'>");
                baseMessage.Append("<tr>");
                baseMessage.Append("<td align='center' style='border-radius: 5px;' bgcolor='#2c768f'>");
                baseMessage.Append($"<a href='{url}' target='_blank' style='{ButtonStyle}'>{buttonText}</a>");
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

        #region Emails

        public string GenerateActivationEmail(Guid userId, string userName)
        {
            var url = _apiEndPoints.ActivateAccountUrl + _tokenManager.GenerateActivationToken(userId).Value;
            return BaseMessage(
                EmailConstants.TitleActivationEmail, 
                userName,
                "Estamos emocionados de que estés a punto de unirte a AuthOn. Solo te falta un pequeño paso para activar tu cuenta.\n\n" +
                "Para garantizar la seguridad de tu información y completar el proceso de registro, necesitamos que verifiques tu dirección de correo electrónico. " +
                "Esto asegura que eres el propietario de esta cuenta y protege tu información personal.",
                "¡Verificar Mi Email!", 
                url
            );
        }

        #endregion

        #endregion
    }
}