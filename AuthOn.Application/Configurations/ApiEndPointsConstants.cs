using Microsoft.Extensions.Options;

namespace AuthOn.Application.Configurations
{
    public class ApiEndPointsConstants
    {
        public string ActivateAccountUrl { get; }

        public ApiEndPointsConstants(IOptions<ApiInformationConfiguration> apiInformationConfiguration)
        {
            var config = apiInformationConfiguration.Value ?? throw new ArgumentNullException(nameof(apiInformationConfiguration));
            ActivateAccountUrl = $"{config.Url}{config.EndPoints.ActivateAccount}";
        }
    }
}