namespace JwtAuthenticationManager.Models
{
    public class AuthenticationRequest
    {
        public string IdentityId { get; set; }
        public string OTP { get; set; }
        public string clientId { get; set; }
        public string clientSecret { get; set; }
    }
}
