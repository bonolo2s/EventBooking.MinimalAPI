namespace EventFlow.API.Helpers
{
    public class HashPasssword
    {
        public string hash(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Password cannot be null or empty");
            }

            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool Verify(string password, string hashed)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashed);
        }
    }
}
