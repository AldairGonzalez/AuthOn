namespace AuthOn.Application.Configurations
{
    public class TokenConfiguration
    {
        public string Key { get; set; } = string.Empty;
        public int ExpiresInHours { get; set; }
    }
}