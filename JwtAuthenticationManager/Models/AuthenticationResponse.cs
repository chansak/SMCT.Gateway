namespace JwtAuthenticationManager.Models
{
    public class AuthenticationResponse
    {
        public string Id { get; set; }
        public string Token { get; set; }
        public int ExpiresIn { get; set; }
    }
}
