namespace EmailWebClient.Models
{
    public class ServerConfig
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Ip { get; set; }
        public int Port { get; set; }
        public bool Ssl { get; set; }
        public override string ToString()
        {
            return Name;
        }
        public ServerConfig Copy() {
            return (ServerConfig) this.MemberwiseClone();
        }
    }
}