namespace AuthOn.Application.Users.Commands.Update.Login
{
    public class LoginUserCommandResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public double RefreshTokenExpiresInHours { get; set; }
    }
}
