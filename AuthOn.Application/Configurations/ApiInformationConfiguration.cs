﻿namespace AuthOn.Application.Configurations
{
    public class ApiInformationConfiguration
    {
        public string Url { get; set; } = string.Empty;
        public EndPointsConfiguration EndPoints { get; set; } = new();

        public string GetActivateUrl()
        {
            return $"{Url}{EndPoints.ActivateUser}";
        }
    }
}