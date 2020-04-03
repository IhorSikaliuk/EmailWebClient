namespace EmailWebClient.Models
{
    public class Authentication
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public ServerConfig Server { get; set; }
    }
}