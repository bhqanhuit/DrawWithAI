namespace DrawApi.Models
{
    public class UserRegisterRequest
    {
        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Email { get; set; } = null!;

    }
}
