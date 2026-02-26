namespace EventFlow.API.Domain
{
    public class User
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
        public UserStatus status { get; set; }

    }
    public enum Role {  Admin , Attendee }
    public enum UserStatus { Active, Inactive }
}
