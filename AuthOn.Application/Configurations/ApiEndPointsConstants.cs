using Microsoft.Extensions.Options;

namespace AuthOn.Application.Configurations
{
    public class ApiEndPointsConstants
    {
        public string ActivateUrl { get; }

        public ApiEndPointsConstants(IOptions<ApiInformationConfiguration> apiInformationConfiguration)
        {
            var config = apiInformationConfiguration.Value ?? throw new ArgumentNullException(nameof(apiInformationConfiguration));
            ActivateUrl = $"{config.Url}{config.EndPoints.ActivateUser}";
        }
    }
}