namespace StudentHelper.Models.DTOs
{
    public class SuccessfulSignInDTO
    {
        public string Token { get; set; }
        public User User { get; set; }
        public double Expiration { get; set; }
    }
}