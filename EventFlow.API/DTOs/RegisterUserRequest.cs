namespace EventFlow.API.DTOs
{
    public class RegisterUserRequest
    {
        public string FullName { get; set; }  
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
