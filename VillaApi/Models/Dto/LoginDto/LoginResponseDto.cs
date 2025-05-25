namespace VillaApi.Models.Dto.LoginDto
{
    public class LoginResponseDto
    {
        public bool IsLoggedIn { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
    }
}
