namespace StudentHelper.Models.DTOs
{
    public class SuccessfulSignInDTO
    {
        public string Token { get; set; }
        public User User { get; set; }
        public long Expiration { get; set; }
    }
}