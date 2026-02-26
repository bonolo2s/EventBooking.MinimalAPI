namespace EventFlow.API.DTOs
{
    public class UpdateUserDTO
    {
        public Guid Id { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
    }
}
