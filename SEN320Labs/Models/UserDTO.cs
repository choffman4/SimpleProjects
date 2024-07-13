namespace SEN320Labs.Models
{
    public class UserDTO
    {
        private string _username;
        private string? _hash;
        private string? _salt;

        public UserDTO(string username, string hash, string salt)
        {
            _username = username;
            _hash = hash;
            _salt = salt;
        }

        public string GetUsername()
        {
            return _username;
        }

        public string? GetHash()
        {
            return _hash;
        }

        public string? GetSalt()
        {
            return _salt;
        }

        public void SetUsername(string username)
        {
            _username = username;
        }

        public void SetHash(string hash)
        {
            _hash = hash;
        }

        public void SetSalt(string salt)
        {
            _salt = salt;
        }
    }
}
