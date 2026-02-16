namespace EventFlow.API.DTOs
{
    public class LoginResponseModel
    {
        public string Token { get; set; } = null!;     
        public DateTime ExpiresAt { get; set; }   
    }
}
